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
using DAO;
using DevExpress.XtraGrid.Views.Grid;

namespace Tooling_WH_Sytems.Part
{
    /// <summary>
    /// Quản lý danh mục linh kiện và ngưỡng cảnh báo tồn kho.
    /// </summary>
    public partial class frmPart : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmPart()
        {
            InitializeComponent();
            LoadControl();
            gridView1.CustomDrawRowIndicator += gridView1_CustomDrawRowIndicator;
        }
        /// <summary>
        /// Tăng độ rộng cột số thứ tự khi nội dung hiển thị dài hơn.
        /// </summary>
        bool cal(Int32 _Width, GridView _View)
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
        bool Isinsert = false;
        #region Control
        /// <summary>
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        void LoadControl()
        {
            LockControl();
            LoadData();
            ClearText();
        }
        /// <summary>
        /// Nạp dữ liệu nghiệp vụ vào bảng hiển thị.
        /// </summary>
        void LoadData()
        {
            GCData.DataSource = PartDAO.Instance.GetListPart();
        }
        /// <summary>
        /// Khóa vùng nhập liệu và đưa các nút về chế độ xem.
        /// </summary>
        void LockControl()
        {
            txtPartCode.Enabled = false;
            txtPartName.Enabled = false;
            txtUnit.Enabled = false;
            txtMin.Enabled = false;
            txtMax.Enabled = false;
            chkSpecPart.Enabled = false;

            btnAdd.Enabled = true;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnSave.Enabled = false;
        }
        /// <summary>
        /// Mở vùng nhập liệu cho thao tác thêm hoặc chỉnh sửa.
        /// </summary>
        void OpenControl()
        {
            txtPartCode.Enabled = true;
            txtPartName.Enabled = true;
            txtUnit.Enabled = true;
            txtMin.Enabled = true;
            txtMax.Enabled = true;
            chkSpecPart.Enabled = true;

            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = true;
        }
        /// <summary>
        /// Xóa nội dung các ô nhập để chuẩn bị bản ghi mới.
        /// </summary>
        void ClearText()
        {
            txtPartCode.Text = String.Empty;
            txtPartName.Text = String.Empty;
            txtUnit.Text = String.Empty;
            txtMin.Text = String.Empty;
            txtMax.Text = String.Empty;
            chkSpecPart.Checked = false;
        }
        /// <summary>
        /// Đưa dữ liệu của dòng đang chọn lên các ô thông tin.
        /// </summary>
        void AddText()
        {
            try
            {
                txtPartCode.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle,gridView1.Columns["PartCode"]).ToString();
                txtPartName.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PartName"]).ToString();
                txtUnit.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Unit"]).ToString();
                txtMin.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["MinWaint"]).ToString();
                txtMax.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["MaxWaint"]).ToString();
                int spec = 0;
                try
                {
                   spec= int.Parse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SpecPart"]).ToString());
                    if(spec != 0)
                    {
                        chkSpecPart.Checked = true;
                    }
                }
                catch
                {

                }
            }
            catch 
            {

            }
        }
        /// <summary>
        /// Điểm mở rộng hiện chưa có xử lý; giữ nguyên hành vi hiện tại.
        /// </summary>
        private void btnPrint_Click(object sender, EventArgs e)
        {

        }
        #endregion
        #region Event

        /// <summary>
        /// Mở hoặc xuất biểu mẫu Excel dùng để nhập dữ liệu.
        /// </summary>
        private void btnDownload_Click(object sender, EventArgs e)
        {
            frmFormPart f = new frmFormPart();
            f.ShowDialog();
        }

        /// <summary>
        /// Mở màn hình nhập dữ liệu từ Excel và đăng ký nạp lại sau khi đóng.
        /// </summary>
        private void btnImport_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (Kun_Static.UserRight < 2)
            {
                XtraMessageBox.Show("bạn không có quyền !".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            frmImportPart f = new frmImportPart();
            f.LamMoi += new EventHandler(btnUpdate_Click);
            f.ShowDialog();
        }
        /// <summary>
        /// Chuẩn bị dữ liệu hoặc mở màn hình để thêm bản ghi mới.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (Kun_Static.UserRight < 2)
            {
                XtraMessageBox.Show("bạn không có quyền !".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ClearText();
            Isinsert = true;
            OpenControl();
        }
        /// <summary>
        /// Xóa dữ liệu đang chọn theo các điều kiện và xác nhận hiện tại.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (Kun_Static.UserRight < 3)
            {
                XtraMessageBox.Show("bạn không có quyền !".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (XtraMessageBox.Show("bạn muốn xóa thông tin này?".ToUpper(),"Thông Báo",MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                foreach (var item in gridView1.GetSelectedRows())
                {
                    string code = gridView1.GetRowCellValue(item, "PartCode").ToString();
                    PartDAO.Instance.DeletePart(code);
                }
                LoadControl();
            }
        }
        /// <summary>
        /// Bắt đầu chỉnh sửa dữ liệu đang chọn theo quyền hiện tại.
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (Kun_Static.UserRight < 2)
            {
                XtraMessageBox.Show("bạn không có quyền !".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Isinsert = false;
            OpenControl();
            txtPartCode.Enabled = false;
        }
        /// <summary>
        /// Nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            LoadControl();
        }
        /// <summary>
        /// Chọn đường dẫn và xuất dữ liệu trên bảng ra tệp theo định dạng được chọn.
        /// </summary>
        private void btnExport_Click(object sender, EventArgs e)
        {
            #region Xuất Excel
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel (2003)(.xls)|*.xls|Excel (2010) (.xlsx)|*.xlsx |RichText File (.rtf)|*.rtf |Pdf File (.pdf)|*.pdf |Html File (.html)|*.html";
                if (saveDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string exportFilePath = saveDialog.FileName;
                    string fileExtenstion = new FileInfo(exportFilePath).Extension;

                    switch (fileExtenstion)
                    {
                        case ".xls":
                            GCData.ExportToXls(exportFilePath);
                            break;
                        case ".xlsx":
                            GCData.ExportToXlsx(exportFilePath);
                            break;
                        case ".rtf":
                            GCData.ExportToRtf(exportFilePath);
                            break;
                        case ".pdf":
                            GCData.ExportToPdf(exportFilePath);
                            break;
                        case ".html":
                            GCData.ExportToHtml(exportFilePath);
                            break;
                        case ".mht":
                            GCData.ExportToMht(exportFilePath);
                            break;
                        default:
                            break;
                    }

                    if (File.Exists(exportFilePath))
                    {
                        try
                        {
                            //Try to open the file and let windows decide how to open it.
                            System.Diagnostics.Process.Start(exportFilePath);
                        }
                        catch
                        {
                            String msg = "The file could not be opened." + Environment.NewLine + Environment.NewLine + "Path: " + exportFilePath;
                            MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        String msg = "The file could not be saved." + Environment.NewLine + Environment.NewLine + "Path: " + exportFilePath;
                        MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// Đồng bộ thông tin của dòng đang chọn với vùng nhập liệu hoặc trạng thái thao tác.
        /// </summary>
        private void GCData_Click(object sender, EventArgs e)
        {
            AddText();
        }
        /// <summary>
        /// Giới hạn ký tự được nhập theo quy tắc số lượng hiện có.
        /// </summary>
        private void txtMin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && (e.KeyChar != '.' ||
                (e.KeyChar == '.' && (txtMin.Text.Length == 0 || txtMin.Text.IndexOf('.') != -1))))
                e.Handled = true;
        }

        /// <summary>
        /// Giới hạn ký tự được nhập theo quy tắc số lượng hiện có.
        /// </summary>
        private void txtMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && (e.KeyChar != '.' ||
                (e.KeyChar == '.' && (txtMax.Text.Length == 0 || txtMax.Text.IndexOf('.') != -1))))
                e.Handled = true;
        }
        /// <summary>
        /// Kiểm tra thông tin rồi thêm hoặc sửa linh kiện, ngưỡng cảnh báo và loại linh kiện.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string partCode = txtPartCode.Text;
            string partName = txtPartName.Text;
            string unit = txtUnit.Text;
            float minWaint = (float)Convert.ToDouble(txtMin.Text);
            float maxWaint = (float)Convert.ToDouble(txtMax.Text);
            int spec = 0;
            if(chkSpecPart.Checked)
            {
                spec = 1;
            }
            else
            {
                spec = 0;
            }
            
            if(Isinsert == true)
            {
                long id = PartDAO.Instance.IdPartCode(partCode);
                if (id == -1)
                {
                    PartDAO.Instance.InsertPart(partCode, partName, unit, minWaint, maxWaint,spec);
                    XtraMessageBox.Show("thêm thông tin thành công !".ToUpper());
                    LoadControl();
                }
                else
                {
                    XtraMessageBox.Show("mã linh kiện đã tồn tịa !".ToUpper(),
                        "Thông Báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            else
            {
                PartDAO.Instance.UpdatePart(partCode, partName, unit, minWaint, maxWaint, spec);
                XtraMessageBox.Show("sửa thông tin thành công !".ToUpper());
                LoadControl();
            }
        }
        #endregion

        /// <summary>
        /// Chuyển linh kiện đang chọn sang loại linh kiện đặc biệt.
        /// </summary>
        private void btnSpecPart_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("bạn muốn xác nhận linh kiện đặc biệt".ToUpper(),
                "Thông báo",MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                foreach (var item in gridView1.GetSelectedRows())
                {
                    string partCode = gridView1.GetRowCellValue(item, "PartCode").ToString();
                    PartDAO.Instance.Update(partCode, 1);
                }
                LoadData();
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
                int status = int.Parse(view.GetRowCellValue(e.RowHandle, view.Columns["SpecPart"]).ToString());
                if(status == 1)
                {
                    e.Appearance.BackColor = Color.Orange;
                    e.Appearance.ForeColor = Color.Black;
                }
            }
        }
    }
}