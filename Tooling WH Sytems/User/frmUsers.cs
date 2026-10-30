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
using DTO;

namespace Tooling_WH_Sytems.User
{
    /// <summary>
    /// Quản lý tài khoản và liên kết với nhân viên.
    /// </summary>
    public partial class frmUsers : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmUsers()
        {
            InitializeComponent();
            LoadControl();
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
            LoadEmployess();
        }
        /// <summary>
        /// Nạp dữ liệu nghiệp vụ vào bảng hiển thị.
        /// </summary>
        void LoadData()
        {
            string enCode = Kun_Static.UserStatic.EmCode;
            List<UsersDTO> listU = UsersDAO.Instance.GetListUser();
            if (enCode.ToUpper() == "ADMINISTRATOR")
            {
                GCData.DataSource = listU;
            }
            else
            {
                string nameGr = EmployessDAO.Instance.NameGr(enCode);
                GCData.DataSource = UsersDAO.Instance.GetUserPermission(nameGr);
            }
        }
        /// <summary>
        /// Khóa vùng nhập liệu và đưa các nút về chế độ xem.
        /// </summary>
        void LockControl()
        {
            cbEmployess.Enabled = false;
            txtUserName.Enabled = false;
            txtPass.Enabled = false;

            btnAdd.Enabled = true;
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            btnSave.Enabled = false;
            btnRePass.Enabled = false;
        }
        /// <summary>
        /// Mở vùng nhập liệu cho thao tác thêm hoặc chỉnh sửa.
        /// </summary>
        void OpenControl()
        {
            txtUserName.Enabled = true;
            txtPass.Enabled = true;
            cbEmployess.Enabled = true;

            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = true;
            btnRePass.Enabled = false;
        }
        /// <summary>
        /// Xóa nội dung các ô nhập để chuẩn bị bản ghi mới.
        /// </summary>
        void ClearText()
        {
            txtUserName.Text = String.Empty;
            txtPass.Text = String.Empty;
            cbEmployess.Text = String.Empty;
        }
        /// <summary>
        /// Đưa dữ liệu của dòng đang chọn lên các ô thông tin.
        /// </summary>
        void AddText()
        {
            try
            {
                txtUserName.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["UserName"]).ToString();
                cbEmployess.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["DisplayName"]).ToString();
            }
            catch
            {
            }
        }
        /// <summary>
        /// Nạp danh sách nhân viên theo phạm vi của tài khoản hiện tại.
        /// </summary>
        void LoadEmployess()
        {
            List<EmployesDTO> listE = new List<EmployesDTO>(); 
            string enCode = Kun_Static.UserStatic.EmCode;
            if (enCode.ToUpper() == "ADMINISTRATOR")
            {
                listE = EmployessDAO.Instance.GetListEmployess();
            }
            else
            {
                string nameGr = EmployessDAO.Instance.NameGr(enCode);
                listE = EmployessDAO.Instance.GetListEmployess().Where(x=>x.NameGr == nameGr).ToList();
            }
            cbEmployess.DataSource = listE;
            cbEmployess.DisplayMember = "EmployessCode";
            cbEmployess.ValueMember = "EmployessCode";
        }
        #endregion
        #region Event

        #endregion

        /// <summary>
        /// Đặt lại mật khẩu tài khoản đang chọn về giá trị mặc định hiện có.
        /// </summary>
        private void btnRePass_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (Kun_Static.UserRight < 2)
            {
                XtraMessageBox.Show("bạn không có quyền !".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string pass = UsersDAO.Instance.EnCodeMD5("1");
            UsersDAO.Instance.RePassWordUser(txtUserName.Text, pass);
        }

        /// <summary>
        /// Đồng bộ thông tin của dòng đang chọn với vùng nhập liệu hoặc trạng thái thao tác.
        /// </summary>
        private void GCData_Click(object sender, EventArgs e)
        {
            AddText();
            btnRePass.Enabled = true;
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
            OpenControl();
            Isinsert = true;
        }

        /// <summary>
        /// Xóa dữ liệu đang chọn theo các điều kiện và xác nhận hiện tại.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (Kun_Static.UserRight < 4)
            {
                XtraMessageBox.Show("bạn không có quyền !".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show("bạn muốn xóa tài khoản này?".ToUpper(),"Thông Báo",MessageBoxButtons.OK) == DialogResult.OK)
            {
                foreach (var item in gridView1.GetSelectedRows())
                {
                    string UserName = gridView1.GetRowCellValue(item, "UserName").ToString();
                    UsersDAO.Instance.DeleteUser(UserName);
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
            OpenControl();
            Isinsert = false;
            txtUserName.Enabled = false;
            txtPass.Enabled = false;
        }

        /// <summary>
        /// Kiểm tra nhân viên, tài khoản rồi thêm mới hoặc cập nhật thông tin người dùng.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string UserName = txtUserName.Text;
            string Pass = UsersDAO.Instance.EnCodeMD5(txtPass.Text);
            string Emcode = cbEmployess.Text.ToUpper();
            int idEm = EmployessDAO.Instance.IdEmployess(Emcode);
            string disName = EmployessDAO.Instance.EmployessName(Emcode);
            if(Emcode == "administrator".ToUpper())
            {
                disName = "administrator".ToUpper();
            }
            else if(idEm == -1)
            {
                XtraMessageBox.Show("mã nhân viên không đúng !".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int id = UsersDAO.Instance.IdUsers(UserName);
            if (Isinsert ==true)
            {
                if(id == -1)
                {
                    UsersDAO.Instance.InsertUser(UserName, Pass, disName, Emcode);
                    XtraMessageBox.Show("thêm thông tin thành công!".ToUpper());
                    LoadControl();
                }
                else
                {
                    XtraMessageBox.Show("tài khoản đã tồn tại !".ToUpper(),"Thông Báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                UsersDAO.Instance.UpdateUser(UserName, Emcode,disName);
                XtraMessageBox.Show("sửa thông tin thành công!".ToUpper());
                LoadControl();
            }
        }
    }
}