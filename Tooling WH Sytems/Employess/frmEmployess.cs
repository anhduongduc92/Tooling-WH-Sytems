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
using DAO;
using System.IO;
using DTO;

namespace Tooling_WH_Sytems.Employess
{
    /// <summary>
    /// Quản lý danh sách nhân viên và nhóm phòng ban.
    /// </summary>
    public partial class frmEmployess : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmEmployess()
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
            LoadGroup();
        }
        /// <summary>
        /// Nạp dữ liệu nghiệp vụ vào bảng hiển thị.
        /// </summary>
        private void LoadData()
        {
            GCData.DataSource = EmployessDAO.Instance.GetListEmployess();
        }
        /// <summary>
        /// Khóa vùng nhập liệu và đưa các nút về chế độ xem.
        /// </summary>
        private void LockControl()
        {
            txtEmployesCode.Enabled = false;
            txtEmployesName.Enabled = false;
            txtEMail.Enabled = false;
            lkGroup.Enabled = false;

            btnAdd.Enabled = true;
            btnEdit.Enabled = true;
            btnSave.Enabled = false;
            btnDelete.Enabled = true;
        }
        /// <summary>
        /// Mở vùng nhập liệu cho thao tác thêm hoặc chỉnh sửa.
        /// </summary>
        private void OpenControl()
        {
            txtEmployesCode.Enabled = true;
            txtEmployesName.Enabled = true;
            txtEMail.Enabled = true;
            lkGroup.Enabled = true;

            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
        }
        /// <summary>
        /// Đưa dữ liệu của dòng đang chọn lên các ô thông tin.
        /// </summary>
        private void AddText()
        {
            try
            {
                txtEmployesCode.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle,gridView1.Columns["EmployessCode"]).ToString();
                txtEmployesName.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["EmployessName"]).ToString();
                txtEMail.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Email"]).ToString();
                lkGroup.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["NameGr"]).ToString();
            }
            catch (Exception ex)
            {
                AppLog.Write("frmEmployess", ex);
            }
        }
        /// <summary>
        /// Xóa nội dung các ô nhập để chuẩn bị bản ghi mới.
        /// </summary>
        private void ClearText()
        {
            txtEmployesCode.Text = String.Empty;
            txtEmployesName.Text = String.Empty;
            txtEMail.Text = String.Empty;
            lkGroup.Text = String.Empty;
        }
        /// <summary>
        /// Nạp danh sách nhóm nhân viên theo tài khoản đăng nhập.
        /// </summary>
        private void LoadGroup()
        {
            string enCode = Kun_Static.UserStatic.EmCode;
            if (enCode.ToUpper() == "ADMINISTRATOR")
            {
                lkGroup.Properties.DataSource = UsersDAO.Instance.GetUserGroup();
                lkGroup.Properties.DisplayMember = "NameUserGroup";
                lkGroup.Properties.ValueMember = "NameUserGroup";
            }
            else if (Kun_Static.UserRight == 4)
            {
                lkGroup.Properties.DataSource = UsersDAO.Instance.GetUserGroup();
                lkGroup.Properties.DisplayMember = "NameUserGroup";
                lkGroup.Properties.ValueMember = "NameUserGroup";
            }
            else
            {
                string nameGr = EmployessDAO.Instance.NameGr(enCode);
                lkGroup.Properties.DataSource = UsersDAO.Instance.GetUserGroupByName(nameGr);
                lkGroup.Properties.DisplayMember = "NameUserGroup";
                lkGroup.Properties.ValueMember = "NameUserGroup";
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
            OpenControl();
            ClearText();
            _isInsert = true;
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
            if (XtraMessageBox.Show("bạn muốn xóa thông tin này ?".ToUpper(),"Thông Báo",MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                foreach (var item in gridView1.GetSelectedRows())
                {
                    string code = gridView1.GetRowCellValue(item, "EmployessCode").ToString();
                    EmployessDAO.Instance.DeleteEmployess(code);
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
            OpenControl();
            txtEmployesCode.Enabled = false;
            _isInsert = false;
        }

        /// <summary>
        /// Kiểm tra thông tin rồi thêm mới hoặc cập nhật nhân viên theo chế độ hiện tại.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string EmployessCode = txtEmployesCode.Text.ToUpper();
            string EmployessName = txtEmployesName.Text.ToUpper();
            string Email = txtEMail.Text;
            string NameGr = lkGroup.Text.ToUpper();
            int id = EmployessDAO.Instance.IdEmployess(EmployessCode);
            if(_isInsert == true)
            {
                if(id   ==  -1 && EmployessCode.Length > 0)
                {
                    EmployessDAO.Instance.InsertEmployess(EmployessCode, EmployessName, Email, NameGr);
                    XtraMessageBox.Show("thêm thông tin thành công!".ToUpper());
                    LoadControl();
                }
                else
                {
                    XtraMessageBox.Show("mã nhân viên đã tồn tại !".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                EmployessDAO.Instance.UpdateEmployess(EmployessCode, EmployessName, Email, NameGr);
                XtraMessageBox.Show("sửa thông tin thành công!".ToUpper());
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
            using (frmFormEmployess f = new frmFormEmployess())
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
        /// Mở màn hình nhập danh sách nhân viên từ Excel.
        /// </summary>
        private void btnInport_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (!UiActions.HasPermission(2))
            {
                return;
            }
            using (frmImportEmployess f = new frmImportEmployess())
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
            //if(gridView1.GetFocusedRowCellValue("Id") != null)
            //{
            //    var em = EmployessDAO.Instance.GetListEmployessById( int.Parse(gridView1.GetFocusedRowCellValue("Id").ToString()));
            //    txtEMail.Text = em.Email;
            //    txtEmployesCode.Text = em.EmployessCode;

            //}
            AddText();
        }

        #endregion
    }
}