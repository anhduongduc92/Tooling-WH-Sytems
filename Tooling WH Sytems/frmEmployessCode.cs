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
using DAO;
using DTO;

namespace Tooling_WH_Sytems
{
    /// <summary>
    /// Nhận và kiểm tra mã nhân viên trước khi thực hiện nghiệp vụ.
    /// </summary>
    public partial class frmEmployessCode : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmEmployessCode()
        {
            InitializeComponent();
            LoadControl();
        }
        private string _emCode = "";
      
        /// <summary>
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void LoadControl()
        {
            this.AcceptButton = btnOK;
            this.CancelButton = btnExit;
            MaximizeBox = false;
        }
        /// <summary>
        /// Chuẩn hóa, kiểm tra và lưu mã nhân viên hợp lệ.
        /// </summary>
        private void LoadFunction()
        {
            string code = txtEmployessCode.Text;
            string emCode = code.ToUpper();
            if (code.Contains("-"))
            {
                string[] ar = emCode.Split('-');
                emCode = ar[0];
            }
            EmployesDTO em = EmployessDAO.Instance.GetItem(emCode);
            if (em == null)
            {
                Kun_Static.EmployessCode = "";
                MessageBox.Show("mã nhân viên không đúng!".ToUpper(), 
                    "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Kun_Static.EmployessCode = emCode;
            _emCode = emCode;
            this.Close();
        }
        /// <summary>
        /// Đóng màn hình theo luồng thoát hiện tại.
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Kiểm tra mã nhân viên và cập nhật trạng thái thao tác.
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            LoadFunction();
            Kun_Static.CheckAction = 0;
        }
        /// <summary>
        /// Khóa ô mã nhân viên và dừng bộ định thời.
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            txtEmployessCode.Enabled = false;
            timer1.Stop();
        }

        /// <summary>
        /// Bắt đầu bộ định thời khi nội dung mã nhân viên thay đổi.
        /// </summary>
        private void txtEmployessCode_TextChanged(object sender, EventArgs e)
        {
            timer1.Start();
        }

        /// <summary>
        /// Lưu lại mã nhân viên đã được xác nhận khi đóng hộp thoại.
        /// </summary>
        private void frmEmployessCode_FormClosing(object sender, FormClosingEventArgs e)
        {
    
            Kun_Static.EmployessCode = _emCode;
        }
    }
}