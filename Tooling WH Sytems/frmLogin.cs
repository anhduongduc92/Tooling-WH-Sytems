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
using System.Runtime.InteropServices;
using DAO;
using DTO;
using DevExpress.XtraSplashScreen;

namespace Tooling_WH_Sytems
{
    /// <summary>
    /// Xác thực tài khoản và mở màn hình chính.
    /// </summary>
    public partial class frmLogin : DevExpress.XtraEditors.XtraForm
    {
        public const int LOCALE_USER_DEFAULT = 0x00000400;
        public const int LOCALE_SSHORTDATE = 0x0000001F;
        public const int LOCALE_STIMEFORMAT = 0x1003;
        public const int LOCALE_SSHORTTIME = 0x0079;

        public const int BSF_QUERY = 0x00000001;
        public const int BSF_IGNORECURRENTTASK = 0x00000002;
        public const int BSF_FLUSHDISK = 0x00000004;
        public const int BSF_NOHANG = 0x00000008;
        public const int BSF_POSTMESSAGE = 0x00000010;
        public const int BSF_FORCEIFHUNG = 0x00000020;
        public const int BSF_NOTIMEOUTIFNOTHUNG = 0x00000040;
        public const int BSF_MSGSRV32ISOK = unchecked((int)0x80000000);
        public const int BSF_MSGSRV32ISOK_BIT = 31;

        public const int BSM_ALLCOMPONENTS = 0x00000000;
        public const int BSM_VXDS = 0x00000001;
        public const int BSM_NETDRIVER = 0x00000002;
        public const int BSM_INSTALLABLEDRIVERS = 0x00000004;
        public const int BSM_APPLICATIONS = 0x00000008;
        public const int BSM_ALLDESKTOPS = 0x00000010;

        public const int WM_WININICHANGE = 0x001A;

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Boolean SetLocaleInfo(int Locale, int LCType, string lpLCData);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int BroadcastSystemMessage(int flags, ref int lpInfo, uint Msg, uint wParam, IntPtr lParam);
        /// <summary>
        /// Thiết lập định dạng ngày ngắn của Windows và thông báo thay đổi.
        /// </summary>
        private void SetupDate()
        {
            if (SetLocaleInfo(LOCALE_USER_DEFAULT, LOCALE_SSHORTDATE, "dd/MM/yyyy"))
            {
                int dwRecipients = BSM_APPLICATIONS | BSM_ALLDESKTOPS;
                string sString = "intl";
                GCHandle GCH = GCHandle.Alloc(sString, GCHandleType.Pinned);
                try
                {
                    IntPtr pString = GCH.AddrOfPinnedObject();
                    BroadcastSystemMessage(BSF_FORCEIFHUNG | BSF_IGNORECURRENTTASK | BSF_NOHANG | BSF_NOTIMEOUTIFNOTHUNG, ref dwRecipients, WM_WININICHANGE, 0, pString);
                }
                finally
                {
                    GCH.Free();
                }
            }
        }
        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmLogin()
        {
            InitializeComponent();
            LoadControl();
            SetupDate();
        }
        #region Control
        /// <summary>
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void LoadControl()
        {
            this.AcceptButton = btnLogin;
            this.CancelButton = btnExit;
            MaximizeBox = false;
        }
        /// <summary>
        /// Kiểm tra thông tin đăng nhập qua tầng truy cập dữ liệu.
        /// </summary>
        private bool Login(string userN, string passW)
        {
            return UsersDAO.Instance.Login(userN, passW);
        }
        #endregion
        #region Event
        /// <summary>
        /// Xác thực tài khoản, lưu người dùng hiện tại rồi mở màn hình chính.
        /// </summary>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userN = txtUserName.Text;
            string passW = UsersDAO.Instance.EnCodeMD5(txtPassWord.Text);
            if(Login(userN,passW))
            {
                UsersDTO userLogin = UsersDAO.Instance.GetListUserByCode(userN);
                Kun_Static.UserStatic = userLogin;
                using (frmMain f = new frmMain())
            {
                    this.Hide();
                    f.ShowDialog();
            }
                this.Show();
            }
            else
            {
                XtraMessageBox.Show("Tài khoản hoặc mật khẩu không đúng!".ToUpper(),
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassWord.SelectAll();
                return;
            }
        }

        /// <summary>
        /// Đóng màn hình theo luồng thoát hiện tại.
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        /// <summary>
        /// Yêu cầu xác nhận trước khi đóng màn hình đăng nhập.
        /// </summary>
        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("bạn thực sự muốn thoát phần mềm !".ToUpper(),
                "Thông Báo",MessageBoxButtons.OKCancel) != DialogResult.OK)
            {                
                e.Cancel = true;
            }
            //else
            //{
            //    if (XtraMessageBox.Show("bạn có muốn lưu dữ liệu phần mềm không?".ToUpper(), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            //    {
            //        try
            //        {
            //            SplashScreenManager.ShowForm(this, typeof(frmWaitForm), true, true, false);
            //            string dataBase = "ToolWH";
            //            string disk = @"\\192.168.2.10\datasave\BACKUP\TOOL WH\" + dataBase + "-" + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss");
            //            BackupDAO.Instance.BackupDataBase(dataBase, disk);
            //            SplashScreenManager.CloseForm(false);
            //            XtraMessageBox.Show("Lưu thành công !".ToUpper(), "Thông báo", MessageBoxButtons.OKCancel);
            //        }
            //        catch (Exception ex)
            //        {

            //            MessageBox.Show(ex.ToString());
            //        }
           
            //    }
            //}
        }

        #endregion
    }
}