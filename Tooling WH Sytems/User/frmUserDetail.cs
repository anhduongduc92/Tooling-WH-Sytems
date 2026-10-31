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

namespace Tooling_WH_Sytems.User
{
    /// <summary>
    /// Thay đổi mật khẩu tài khoản hiện tại.
    /// </summary>
    public partial class frmUserDetail : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmUserDetail()
        {
            InitializeComponent();
            LoadControl();
        }
        /// <summary>
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void LoadControl()
        {
            txtUserName.Text = Kun_Static.UserStatic.UserName;
            txtDisplayName.Text = Kun_Static.UserStatic.EmCode;
        }
        /// <summary>
        /// Xóa nội dung các ô mật khẩu sau thao tác.
        /// </summary>
        private void LoadClearText()
        {
            txtPass.Text = String.Empty;
            txtNewPass.Text = String.Empty;
            txtReNewPass.Text = String.Empty;
        }
        /// <summary>
        /// Kiểm tra mật khẩu hiện tại và mật khẩu mới trước khi cập nhật.
        /// </summary>
        private void UpdateUser()
        {
            string password = UsersDAO.Instance.EnCodeMD5(txtPass.Text);
            string newpass = UsersDAO.Instance.EnCodeMD5(txtNewPass.Text);
            string renewpass = UsersDAO.Instance.EnCodeMD5(txtReNewPass.Text);
            string userName = txtUserName.Text;

            if (!newpass.Equals(renewpass))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mật khẩu mới!".ToUpper(),"Thông Báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            else if(newpass.Length == 0 || renewpass.Length ==0)
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu mới!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (UsersDAO.Instance.Login(userName,password))
                {
                    UsersDAO.Instance.RePassWordUser(userName,newpass);
                    MessageBox.Show("Cập nhật thành công".ToUpper());
                    /*if (updateAccount != null)
                        updateAccount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUserName(userName)));*/
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khấu".ToUpper());
                }
            }
        }
        /// <summary>
        /// Đổi mật khẩu và xóa các ô nhập mật khẩu.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            UpdateUser();
            LoadClearText();
        }
    }
}