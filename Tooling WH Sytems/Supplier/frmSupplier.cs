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
using DAO;

namespace Tooling_WH_Sytems.Supplier
{
    /// <summary>
    /// Quản lý thông tin nhà cung cấp.
    /// </summary>
    public partial class frmSupplier : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmSupplier()
        {
            InitializeComponent();
            LoadControl();
        }
        private bool _isInsert = false;
        #region Control
        /// <summary>
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void LoadControl()
        {
            LoadData();
            LockControl();
            ClearText();
        }
        /// <summary>
        /// Nạp dữ liệu nghiệp vụ vào bảng hiển thị.
        /// </summary>
        private void LoadData()
        {
            GCData.DataSource = SupplierDAO.Instance.GetListSupplier();
        }
        /// <summary>
        /// Khóa vùng nhập liệu và đưa các nút về chế độ xem.
        /// </summary>
        private void LockControl()
        {
            txtSupplierCode.Enabled = false;
            txtSupplierName.Enabled = false;
            txtAddress.Enabled = false;
            txtPhone.Enabled = false;
            txtEmail.Enabled = false;

            btnAdd.Enabled = true;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnSave.Enabled = false;
        }
        /// <summary>
        /// Mở vùng nhập liệu cho thao tác thêm hoặc chỉnh sửa.
        /// </summary>
        private void OpenControl()
        {
            txtSupplierCode.Enabled = true;
            txtSupplierName.Enabled = true;
            txtAddress.Enabled = true;
            txtPhone.Enabled = true;
            txtEmail.Enabled = true;

            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = true;
        }
        /// <summary>
        /// Xóa nội dung các ô nhập để chuẩn bị bản ghi mới.
        /// </summary>
        private void ClearText()
        {
            txtSupplierCode.Text = String.Empty;
            txtSupplierName.Text = String.Empty;
            txtAddress.Text = String.Empty;
            txtPhone.Text = String.Empty;
            txtEmail.Text = String.Empty;
        }
        /// <summary>
        /// Đưa dữ liệu của dòng đang chọn lên các ô thông tin.
        /// </summary>
        private void AddText()
        {
            try
            {
                txtSupplierCode.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle,gridView1.Columns["SupplierCode"]).ToString();
                txtSupplierName.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["SupplierName"]).ToString();
                txtAddress.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Address"]).ToString();
                txtPhone.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Phone"]).ToString();
                txtEmail.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Email"]).ToString();
            }
            catch (Exception ex)
            {
                AppLog.Write("frmSupplier", ex);
            }
        }
        #endregion
        #region Event
        /// <summary>
        /// Chuẩn bị dữ liệu hoặc mở màn hình để thêm bản ghi mới.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (!UiActions.HasPermission(2))
            {
                return;
            }
            ClearText();
            _isInsert = true;
            OpenControl();
        }

        /// <summary>
        /// Xóa dữ liệu đang chọn theo các điều kiện và xác nhận hiện tại.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (!UiActions.HasPermission(3))
            {
                return;
            }
            if (XtraMessageBox.Show("bạn thực sự muốn xóa thông tin này?".ToUpper(),"Thông Báo",MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                foreach (var item in gridView1.GetSelectedRows())
                {
                    string code = gridView1.GetRowCellValue(item, "SupplierCode").ToString();
                    SupplierDAO.Instance.DeleteSupplier(code);
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
            if (!UiActions.HasPermission(2))
            {
                return;
            }
            _isInsert = false;
            OpenControl();
            txtSupplierCode.Enabled = false;
        }

        /// <summary>
        /// Kiểm tra thông tin rồi thêm mới hoặc cập nhật nhà cung cấp.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string SupplierCode = txtSupplierCode.Text;
            string SupplierName = txtSupplierName.Text;
            string Address = txtAddress.Text;
            string Phone = txtPhone.Text;
            string Email = txtEmail.Text;
            if (_isInsert == true)
            {
                int id = SupplierDAO.Instance.IdSupplier(SupplierCode);
                if(id == -1)
                {
                    SupplierDAO.Instance.InsertSupplier(SupplierCode, SupplierName, Address, Phone, Email);
                    XtraMessageBox.Show("thêm thông tin thành công !".ToUpper());
                    LoadControl();
                }
                else
                {
                    XtraMessageBox.Show("Mã NCC đã tồn tại !".ToUpper(),"Thông Báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                SupplierDAO.Instance.UpdateSupplier(SupplierCode, SupplierName, Address, Phone, Email);
                XtraMessageBox.Show("sửa thông tin thành công !".ToUpper());
                LoadControl();
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
        /// Mở hoặc xuất biểu mẫu Excel dùng để nhập dữ liệu.
        /// </summary>
        private void btnDownload_Click(object sender, EventArgs e)
        {
            using (frmFormSupplier f = new frmFormSupplier())
            {
                f.ShowDialog();
            }
        }

        /// <summary>
        /// Chọn đường dẫn và xuất dữ liệu trên bảng ra tệp theo định dạng được chọn.
        /// </summary>
        private void btnExport_Click(object sender, EventArgs e)
        {
            GridExportService.Export(GCData, "Excel (2003)(.xls)|*.xls|Excel (2010) (.xlsx)|*.xlsx |RichText File (.rtf)|*.rtf |Pdf File (.pdf)|*.pdf |Html File (.html)|*.html");
        }

        /// <summary>
        /// Mở màn hình nhập dữ liệu từ Excel và đăng ký nạp lại sau khi đóng.
        /// </summary>
        private void btnImport_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (!UiActions.HasPermission(2))
            {
                return;
            }
            using (frmImportSupplier f = new frmImportSupplier())
            {
                f.LamMoi += new EventHandler(btnUpdate_Click);
                f.ShowDialog();
            }
        }

        /// <summary>
        /// Đồng bộ thông tin của dòng đang chọn với vùng nhập liệu hoặc trạng thái thao tác.
        /// </summary>
        private void GCData_Click(object sender, EventArgs e)
        {
            AddText();
        }

        #endregion
    }
}