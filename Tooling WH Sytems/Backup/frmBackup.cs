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
using DevExpress.XtraSplashScreen;
using DAO;

namespace Tooling_WH_Sytems.Backup
{
    /// <summary>
    /// Chọn thư mục và sao lưu cơ sở dữ liệu ToolWH.
    /// </summary>
    public partial class frmBackup : DevExpress.XtraEditors.XtraForm
    {
        // Ngăn chạy hai lần hoặc đóng form trong khi SQL Server đang sao lưu.
        private bool _isBackingUp;

        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmBackup()
        {
            InitializeComponent();
            LoadControl();
        }
        /// <summary>
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void LoadControl()
        {
            txtUrl.Enabled = false;
            btnBackup.Enabled = false;
        }
        /// <summary>
        /// Mở hộp thoại chọn thư mục sao lưu.
        /// </summary>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            if(fb.ShowDialog() == DialogResult.OK)
            {
                btnBackup.Enabled = true;
                txtUrl.Text = fb.SelectedPath;
            }
            else
            {
                btnBackup.Enabled = false;
            }
        }

        /// <summary>
        /// Sao lưu bất đồng bộ, khóa thao tác lặp và thông báo kết quả.
        /// </summary>
        private async void btnBackup_Click(object sender, EventArgs e)
        {
            if (_isBackingUp)
            {
                return;
            }
            if (string.IsNullOrEmpty(txtUrl.Text))
            {
                XtraMessageBox.Show("bạn chưa chọn đường dẫn lưu File", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Chụp đường dẫn trước await; chỉ thao tác control trên luồng giao diện.
            string dataBase = "ToolWH";
            string disk = txtUrl.Text + @"\" + dataBase + "-" + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss");
            bool splashShown = false;
            _isBackingUp = true;
            btnOpen.Enabled = false;
            btnBackup.Enabled = false;
            try
            {
                SplashScreenManager.ShowForm(this, typeof(frmWaitForm), true, true, false);
                splashShown = true;
                try
                {
                    // SQL chạy bất đồng bộ; câu lệnh, tên database và tên file giữ nguyên.
                    await BackupDAO.Instance.BackupDataBaseAsync(dataBase, disk);
                }
                finally
                {
                    if (splashShown)
                    {
                        SplashScreenManager.CloseForm(false);
                    }
                }
                XtraMessageBox.Show("Lưu thành công !".ToUpper());
            }
            catch (Exception ex)
            {
                // async void chỉ dành cho sự kiện; lỗi được xử lý tại đây để không thoát ứng dụng.
                XtraMessageBox.Show("Không thể sao lưu dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isBackingUp = false;
                btnOpen.Enabled = true;
                btnBackup.Enabled = !string.IsNullOrEmpty(txtUrl.Text);
            }
        }
        /// <summary>
        /// Ngăn đóng màn hình khi thao tác sao lưu chưa hoàn tất.
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (_isBackingUp)
            {
                // Chờ lệnh sao lưu hoàn tất để phần tiếp tục sau await còn form hợp lệ.
                e.Cancel = true;
            }
        }

    }
}