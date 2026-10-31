using Tooling_WH_Sytems.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Tooling_WH_Sytems.Input;
using Tooling_WH_Sytems.Output;
using DTO;
using DAO;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using System.Threading;
using DevExpress.XtraSplashScreen;

namespace Tooling_WH_Sytems
{
    /// <summary>
    /// Theo dõi tồn kho và cảnh báo linh kiện thông thường.
    /// </summary>
    public partial class frmInventory : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmInventory()
        {
            InitializeComponent();
            InventoryRefreshRequested += OnInventoryRefreshRequested;
            LoadControl();
            gridView1.CustomDrawRowIndicator += gridView1_CustomDrawRowIndicator;
        }
        /// <summary>
        /// Tăng độ rộng cột số thứ tự khi nội dung hiển thị dài hơn.
        /// </summary>
        private bool cal(Int32 _Width, GridView _View)
        {
            _View.IndicatorWidth = _View.IndicatorWidth < _Width ? _Width : _View.IndicatorWidth;
            return true;
        }
        /// <summary>
        /// Vẽ số thứ tự dòng và điều chỉnh độ rộng cột chỉ báo của bảng.
        /// </summary>
        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (!gridView1.IsGroupRow(e.RowHandle)) //Nếu không phải là Group
            {
                if (e.Info.IsRowIndicator) //Nếu là dòng Indicator
                {
                    if (e.RowHandle < 0)
                    {
                        e.Info.ImageIndex = 0;
                        e.Info.DisplayText = string.Empty;
                    }
                    else
                    {
                        e.Info.ImageIndex = -1; //Không hiển thị hình
                        e.Info.DisplayText = (e.RowHandle + 1).ToString(); //Số thứ tự tăng dần
                    }
                    SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font); //Lấy kích thước của vùng hiển thị Text
                    Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                    BeginInvoke(new MethodInvoker(delegate { cal(_Width, gridView1); })); //Tăng kích thước nếu Text vượt quá
                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1)); //Nhân -1 để đánh lại số thứ tự tăng dần
                SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                BeginInvoke(new MethodInvoker(delegate { cal(_Width, gridView1); }));
            }
        }
        /// <summary>
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void LoadControl()
        {
            InventoryRefreshRequested?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Lần lượt cập nhật trạng thái phiếu nhập và trạng thái tồn linh kiện ở nền.
        /// </summary>
        private async void OnInventoryRefreshRequested(object sender, EventArgs e)
        {
            _inventoryRefreshRequested = true;
            if (_isRefreshingInventory) return;
            _isRefreshingInventory = true;
            try
            {
                using (new BusyFormScope(this))
                {
                    do
                    {
                        _inventoryRefreshRequested = false;
                        await LoadIventory();
                        await loadInvenPart();
                        if (IsDisposed) return;
                        // Hiển thị sau khi cập nhật xong; không chạy chồng các lần làm mới.
                        loadData();
                        LoadCheck();
                    }
                    while (_inventoryRefreshRequested);
                }
            }
            catch (Exception ex)
            {
                UiActions.ShowError("Không thể nạp dữ liệu tồn kho", ex);
            }
            finally
            {
                _isRefreshingInventory = false;
            }
        }
        /// <summary>
        /// Bỏ chọn các bộ lọc trạng thái tồn kho.
        /// </summary>
        private void LoadCheck()
        {
            ckStatus1.Checked = false;
            ckStatus2.Checked = false;
            ckStatus4.Checked = false;
            ckStatus5.Checked = false;
            ckStatus6.Checked = false;
        }
        /// <summary>
        /// Cập nhật trạng thái các phiếu nhập còn số lượng tồn bằng tác vụ nền.
        /// </summary>
        private async Task LoadIventory()
        {
            await Task.Run(() =>
            {

                List<InventoryDTO> listIven = InventoryDAO.Instance.GetListInventory0().Where(x => x.QuantityInventory > 0).ToList();
                if (listIven.Count > 0)
                {
                    foreach (InventoryDTO item in listIven)
                    {
                        InventoryDAO.Instance.UpdateStatusInput(item.Id, 1);
                    }
                }
            });
        }
        /// <summary>
        /// Cập nhật cảnh báo tồn kho theo số lượng, ngưỡng và ngày nhập hiện có.
        /// </summary>
        private async Task loadInvenPart()
        {
            await Task.Run(()=> {

                List<PartDTO> listP = PartDAO.Instance.GetListPart();
                List<InvenPartDTO> listInven = InventoryDAO.Instance.GetList(0);
                foreach (PartDTO item in listP)
                {
                    string a = item.PartCode;
                    InvenPartDTO invenPart = InventoryDAO.Instance.GetItem(a);
                    int stPart = item.StatusPart;
                    if (invenPart.QuantInven > 0)
                    {
                        DateTime dateMin = (DateTime)InventoryDAO.Instance.DateMinInput(item.PartCode);
                        if (dateMin.AddDays(90) == DateTime.Now)
                        {
                            stPart = 5;
                        }
                        else if (invenPart.QuantInven < item.MinWaint && item.StatusPart != 6)
                        {
                            stPart = 2;
                        }
                        else if (invenPart.QuantInven >= item.MaxWaint)
                        {
                            stPart = 4;
                        }
                        else if ((invenPart.QuantInven>=item.MinWaint) && (invenPart.QuantInven <item.MaxWaint))
                        {
                            stPart = 1;
                        }
                    }
                    else
                    {
                        if(item.MinWaint == 0 && item.MaxWaint == 0)
                        {
                            stPart = 1;                          
                        }
                        else
                        {
                            stPart = 2;
                        }
                    }
                    if(item.StatusPart != stPart)
                    {
                        PartDAO.Instance.UpdatePart(a, stPart);
                    }
                }
            });          
        }
        /// <summary>
        /// Nạp tồn kho và sắp xếp theo trạng thái linh kiện.
        /// </summary>
        private void loadData()
        {
            List<InvenPartDTO> listData = InventoryDAO.Instance.GetList(0);
            GCData.DataSource = listData.OrderByDescending(x => x.StatusPart);
        }
        /// <summary>
        /// Yêu cầu nhập mã nhân viên và kiểm tra mã trước khi tiếp tục nghiệp vụ.
        /// </summary>
        private bool TryGetEmployee(out string employeeCode)
        {
            employeeCode = string.Empty;

            using (frmEmployessCode form = new frmEmployessCode())
            {
                form.ShowDialog(this);
            }

            employeeCode = Kun_Static.EmployessCode?.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(employeeCode))
            {
                ShowError("Bạn chưa nhập mã nhân viên.");
                return false;
            }

            EmployesDTO employee = EmployessDAO.Instance.GetItem(employeeCode);

            if (employee == null)
            {
                ShowError("Mã nhân viên không đúng.");
                return false;
            }

            return true;
        }
        /// <summary>
        /// Hiển thị thông báo lỗi theo định dạng dùng chung của màn hình.
        /// </summary>
        private static void ShowError(string message)
        {
            MessageBox.Show(
                message.ToUpper(),
                "LỖI",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        /// <summary>
        /// Kiểm tra quyền và mã nhân viên trước khi mở màn hình nhập kho.
        /// </summary>
        private void btnInput_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (!UiActions.HasPermission(2))
            {
                return;
            }
            string employeeCode;
            if (!TryGetEmployee(out employeeCode))
            {
                return;
            }
            using (frmInput f = new frmInput())
            {
                f.eventHandler += new EventHandler(btnUpdate_Click);
                f.ShowDialog();
            }
        }

        /// <summary>
        /// Kiểm tra quyền và mã nhân viên trước khi mở màn hình xuất kho.
        /// </summary>
        private void btnOutput_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (!UiActions.HasPermission(2))
            {
                return;
            }
            string employeeCode;
            if (!TryGetEmployee(out employeeCode))
            {
                return;
            }
            using (frmOutput f = new frmOutput())
            {
                f.eventHandler += new EventHandler(btnUpdate_Click);
                f.ShowDialog();
            }
        }

        /// <summary>
        /// Kiểm tra quyền và mã nhân viên trước khi mở màn hình xuất kho nội bộ.
        /// </summary>
        private void btnOutLocal_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (!UiActions.HasPermission(2))
            {
                return;
            }
            string employeeCode;
            if (!TryGetEmployee(out employeeCode))
            {
                return;
            }
            using (frmOutputLocal f = new frmOutputLocal())
            {
                f.eventHandler += new EventHandler(btnUpdate_Click);
                f.ShowDialog();
            }
        }
        /// <summary>
        /// Nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            LoadControl();
        }

        /// <summary>
        /// Kiểm tra quyền và mã nhân viên trước khi mở màn hình nhập kho nội bộ.
        /// </summary>
        private void btnInputLocal_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (!UiActions.HasPermission(3))
            {
                return;
            }
            string employeeCode;
            if (!TryGetEmployee(out employeeCode))
            {
                return;
            }
            using (frmInputLocal f = new frmInputLocal())
            {
                f.eventHandler += new EventHandler(btnUpdate_Click);
                f.ShowDialog();
            }
        }

        /// <summary>
        /// Tô màu dòng theo các điều kiện trạng thái nghiệp vụ hiện có.
        /// </summary>
        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle >= 0)
            {
                int status = int.Parse(view.GetRowCellValue(e.RowHandle, view.Columns["StatusPart"]).ToString());
                if (status == 1)
                {
                    e.Appearance.BackColor = Color.White;
                    e.Appearance.ForeColor = Color.Black;
                }
                else if (status == 2)
                {
                    e.Appearance.BackColor = Color.Red;
                    e.Appearance.ForeColor = Color.Black;
                }
                else if (status == 3)
                {
                    e.Appearance.BackColor = Color.Orange;
                    e.Appearance.ForeColor = Color.Black;
                }
                else if (status == 4)
                {
                    e.Appearance.BackColor = Color.Aqua;
                    e.Appearance.ForeColor = Color.Black;
                }
                else if (status == 5)
                {
                    e.Appearance.BackColor = Color.Black;
                    e.Appearance.ForeColor = Color.White;
                }
                else if (status == 6)
                {
                    e.Appearance.BackColor = Color.DeepPink;
                    e.Appearance.ForeColor = Color.Black;
                }
            }
        }

        /// <summary>
        /// Chọn đường dẫn và xuất dữ liệu trên bảng ra tệp theo định dạng được chọn.
        /// </summary>
        private void btnExport_Click(object sender, EventArgs e)
        {
            GridExportService.Export(GCData, "Excel (2010) (.xlsx)|*.xlsx |RichText File (.rtf)|*.rtf |Pdf File (.pdf)|*.pdf |Html File (.html)|*.html");
        }

        /// <summary>
        /// Lọc danh sách tồn kho theo trạng thái được chọn; giữ nguyên mã trạng thái hiện tại.
        /// </summary>
        private void ckStatus5_CheckedChanged(object sender, EventArgs e)
        {
            List<InventoryDTO> listI2 = new List<InventoryDTO>();
            if (ckStatus5.Checked == true)
            {
                ckStatus1.Checked = false;
                ckStatus2.Checked = false;
                ckStatus4.Checked = false;
                ckStatus6.Checked = false;
                List<InvenPartDTO> listData = InventoryDAO.Instance.GetList(0).Where(x=>x.StatusPart == 5).ToList();
                GCData.DataSource = listData.OrderByDescending(x => x.StatusPart);
            }
        }

        /// <summary>
        /// Lọc danh sách tồn kho theo trạng thái được chọn; giữ nguyên mã trạng thái hiện tại.
        /// </summary>
        private void ckStatus2_CheckedChanged(object sender, EventArgs e)
        {
            List<InventoryDTO> listI2 = new List<InventoryDTO>();
            if (ckStatus2.Checked == true)
            {
                ckStatus1.Checked = false;
                ckStatus5.Checked = false;
                ckStatus4.Checked = false;
                ckStatus6.Checked = false;
                List<InvenPartDTO> listData = InventoryDAO.Instance.GetList(0).Where(x => x.StatusPart == 2).ToList();
                GCData.DataSource = listData.OrderByDescending(x => x.StatusPart);
            }
        }
        /// <summary>
        /// Lọc danh sách tồn kho theo trạng thái được chọn; giữ nguyên mã trạng thái hiện tại.
        /// </summary>
        private void ckStatus4_CheckedChanged(object sender, EventArgs e)
        {
            List<InventoryDTO> listI2 = new List<InventoryDTO>();
            if (ckStatus4.Checked == true)
            {
                ckStatus2.Checked = false;
                ckStatus5.Checked = false;
                ckStatus1.Checked = false;
                ckStatus6.Checked = false;
                List<InvenPartDTO> listData = InventoryDAO.Instance.GetList(0).Where(x => x.StatusPart == 4).ToList();
                GCData.DataSource = listData.OrderByDescending(x => x.StatusPart);
            }
        }

        /// <summary>
        /// Lọc danh sách tồn kho theo trạng thái được chọn; giữ nguyên mã trạng thái hiện tại.
        /// </summary>
        private void ckStatus1_CheckedChanged(object sender, EventArgs e)
        {
            List<InventoryDTO> listI2 = new List<InventoryDTO>();
            if (ckStatus1.Checked == true)
            {
                ckStatus2.Checked = false;
                ckStatus4.Checked = false;
                ckStatus5.Checked = false;
                ckStatus6.Checked = false;
                List<InvenPartDTO> listData = InventoryDAO.Instance.GetList(0).Where(x => x.StatusPart <= 1).ToList();
                GCData.DataSource = listData.OrderByDescending(x => x.StatusPart);
            }
        }
        /// <summary>
        /// Lọc danh sách tồn kho theo trạng thái được chọn; giữ nguyên mã trạng thái hiện tại.
        /// </summary>
        private void ckStatus6_CheckedChanged(object sender, EventArgs e)
        {
            List<InventoryDTO> listI2 = new List<InventoryDTO>();
            if (ckStatus6.Checked == true)
            {
                ckStatus2.Checked = false;
                ckStatus4.Checked = false;
                ckStatus5.Checked = false;
                ckStatus1.Checked = false;
                List<InvenPartDTO> listData = InventoryDAO.Instance.GetList(0).Where(x => x.StatusPart == 6).ToList();
                GCData.DataSource = listData.OrderByDescending(x => x.StatusPart);
            }
        }
        /// <summary>
        /// Cập nhật trạng thái xác nhận của linh kiện đang chọn.
        /// </summary>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (!UiActions.HasPermission(3))
            {
                return;
            }
            if (XtraMessageBox.Show("bạn muốn xác nhận đặt hàng cho linh kiện này ?".ToUpper(), "Thông Báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                foreach (var item in gridView1.GetSelectedRows())
                {
                    string code = gridView1.GetRowCellValue(item, "PartCode").ToString();
                    int status = int.Parse(gridView1.GetRowCellValue(item, "StatusPart").ToString());
                    InventoryDAO.Instance.UpdateStatusInventoryByCode(code, 6, status);
                    PartDAO.Instance.UpdatePart(code, 6);
                }
                LoadControl();
            }
        }

        /// <summary>
        /// Mở hoặc xuất biểu mẫu Excel dùng để nhập dữ liệu.
        /// </summary>
        private void btnDownload_Click(object sender, EventArgs e)
        {
            using (frmFormInput f = new frmFormInput())
            {
                f.ShowDialog();
            }
        }

        /// <summary>
        /// Kiểm tra quyền và mở màn hình nhập kho từ Excel.
        /// </summary>
        private void btnInputExcel_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (!UiActions.HasPermission(2))
            {
                return;
            }
            string employeeCode;
            if (!TryGetEmployee(out employeeCode))
            {
                return;
            }
            using (frmInputExcel f = new frmInputExcel())
            {
                f.eventHandler += new EventHandler(btnUpdate_Click);
                f.ShowDialog();
            }
        }
        /// <summary>
        /// Đổi kích thước chữ của bảng rồi nạp lại trạng thái màn hình.
        /// </summary>
        private void nudNumberSize_ValueChanged(object sender, EventArgs e)
        {
            LoadControl();
        }
        private event EventHandler InventoryRefreshRequested;
        private bool _isRefreshingInventory;
        private bool _inventoryRefreshRequested;

    }
}