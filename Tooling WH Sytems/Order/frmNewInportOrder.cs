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
using System.IO;
using ExcelDataReader;
using DAO;

namespace Tooling_WH_Sytems.Order
{
    /// <summary>
    /// Nhập đơn đặt hàng mới từ Excel.
    /// </summary>
    public partial class frmNewInportOrder : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện cùng các giá trị đầu vào của màn hình.
        /// </summary>
        public frmNewInportOrder()
        {
            InitializeComponent();
            Disposed += DisposeWorkbook;
            LoadName();
        }
        private List<string> _errors = new List<string>();
        private DataSet _workbook;
        private bool _hasSelectedSheet;
        public EventHandler LamMoi;
        /// <summary>
        /// Chọn tệp Excel, đọc các sheet và nạp danh sách tên sheet.
        /// </summary>
        private async Task ReadDataAsync()
        {
            using (OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Excel Workbook|*.xlsx|Excel Workbook 97-2003|*.xls",
                ValidateNames = true
            })
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                string fileName = dialog.FileName;
                bool isBinary = dialog.FilterIndex == 2;
                using (new BusyFormScope(this))
                {
                    DataSet workbook = await Task.Run(() => ExcelWorkbookReader.Read(fileName, isBinary));
                    if (IsDisposed)
                    {
                        workbook.Dispose();
                        return;
                    }

                    // Chỉ thay dữ liệu hiện tại sau khi tệp mới đã đọc thành công.
                    DataSet previousWorkbook = _workbook;
                    _hasSelectedSheet = false;
                    GCData.DataSource = null;
                    cbSheet.Items.Clear();
                    _workbook = workbook;
                    if (previousWorkbook != null) previousWorkbook.Dispose();
                    lblLink.Text = fileName;
                    foreach (DataTable sheet in workbook.Tables)
                    {
                        cbSheet.Items.Add(sheet.TableName);
                    }
                }
            }
        }
        /// <summary>
        /// Chọn và đọc tệp Excel để chuẩn bị nhập dữ liệu.
        /// </summary>
        private async void btnOpen_Click(object sender, EventArgs e)
        {
            if (_isReadingWorkbook) return;
            _isReadingWorkbook = true;
            try
            {
                await ReadDataAsync();
            }
            catch (Exception ex)
            {
                UiActions.ShowError("Không thể đọc tệp Excel", ex);
            }
            finally
            {
                _isReadingWorkbook = false;
            }
        }
        /// <summary>
        /// Tạo đơn đặt hàng và nhập từng dòng chi tiết từ sheet đang chọn.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string Employess = "";
            if(txtOrderName.Text.Length ==0)
            {
                MessageBox.Show("bạn hãy điền tên đơn hàng!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if(txtRoom.Text.Length == 0)
            {
                MessageBox.Show("bạn hãy điền phòng ban!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            NewOrderDAO.Instance.InsertNewOrder(txtOrderName.Text,Employess,DateTime.Now,1,"",txtRoom.Text);
            if (!_hasSelectedSheet)
            {
                XtraMessageBox.Show("bạn chưa chọn sheet cần thêm!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblError.Text = "";
            _errors = new List<string>();
            int i = 0;
            int slLoi = 0;
            long IdOrder = NewOrderDAO.Instance.MaxIdNewOrder();
            string PartCode = "";
            string PartName = "";
            float Quantity = 0;
            string Unit = "";
            string PurPose = "";
            string Note = "";
            int StatusODT = 0;
            float QuantityOut = 0;
            string Maker = "";
            string DateOrder = "";
            

            foreach (DataGridViewRow row in GCData.Rows)
            {
                if (row.IsNewRow) continue;
                i++;
                try
                {
                    if (i >= 9)
                    {
                        PartName = row.Cells[2].Value.ToString();
                        PartCode = row.Cells[3].Value.ToString();
                        Maker = row.Cells[4].Value.ToString();
                        Quantity = (float)Convert.ToDouble(row.Cells[5].Value.ToString());
                        Unit = row.Cells[6].Value.ToString();
                        PurPose = row.Cells[7].Value.ToString();
                      
                        StatusODT = 1;
                        QuantityOut = 0;
                        DateOrder = row.Cells[11].Value.ToString();
                        if(PartName.Trim() == "Tổ trưởng trở lên lập --> Trưởng phòng kiểm tra và chấp nhận  --> Trưởng phòng QLSX xác nhận.")
                        {
                            MessageBox.Show("Nhập Dữ Liệu Xong".ToUpper());
                            this.Close();
                        }
                        else
                        {
                            NewOrderDAO.Instance.InsertNOrd(IdOrder, PartCode, PartName, Quantity, Unit, PurPose, Note, StatusODT, QuantityOut, Maker, DateOrder, Employess);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppLog.Write("frmNewInportOrder: nhập dòng " + i, ex);
                    _errors.Add("Dòng " + i + ": Dữ liệu không hợp lệ hoặc không thể ghi dữ liệu.");
                    slLoi++;
                }
                lblError.Text = "Lỗi: " + (slLoi) + " Lỗi";
            }

            if (_errors.Count == 0)
            {
                MessageBox.Show("Nhập Dữ Liệu Xong".ToUpper());
                this.Close();
            }
            else
            {
                MessageBox.Show("Có lỗi khi nhập!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Mở danh sách lỗi phát sinh khi nhập dữ liệu từ Excel.
        /// </summary>
        private void btnErrorList_Click(object sender, EventArgs e)
        {
            using (frmErrorList f = new frmErrorList(_errors))
            {
                f.ShowDialog();
            }
        }
        /// <summary>
        /// Hiển thị dữ liệu của sheet Excel được chọn và đánh dấu đã chọn sheet.
        /// </summary>
        private void cbSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cbSheet.SelectedIndex;
            if (_workbook == null || index < 0 || index >= _workbook.Tables.Count) return;
            GCData.DataSource = _workbook.Tables[index];
            _hasSelectedSheet = true;
        }

        /// <summary>
        /// Tạo tên đơn mặc định theo thời điểm hiện tại.
        /// </summary>
        private void LoadName()
        {
            txtOrderName.Text = "Đơn hàng ngày " + DateTime.Now.Day + " - " + DateTime.Now.Month + " - " + DateTime.Now.Year;
        }

        /// <summary>
        /// Phát sự kiện để màn hình gọi nạp lại dữ liệu khi đóng form.
        /// </summary>
        private void frmNewInportOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            LamMoi?.Invoke(sender, e);
        }
        private bool _isReadingWorkbook;

        /// <summary>Giải phóng workbook khi form không còn sử dụng dữ liệu.</summary>
        private void DisposeWorkbook(object sender, EventArgs e)
        {
            if (_workbook != null)
            {
                _workbook.Dispose();
                _workbook = null;
            }
        }

    }
}