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
using DevExpress.XtraWaitForm;

namespace Tooling_WH_Sytems
{
    /// <summary>
    /// Hiển thị trạng thái chờ của thao tác đang thực hiện.
    /// </summary>
    public partial class frmWaitForm : WaitForm
    {
        /// <summary>
        /// Khởi tạo giao diện cùng các giá trị đầu vào của màn hình.
        /// </summary>
        public frmWaitForm()
        {
            InitializeComponent();
            this.progressPanel1.AutoSize = true;
        }

        #region Overrides

        /// <summary>
        /// Cập nhật tiêu đề trên màn hình chờ.
        /// </summary>
        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            this.progressPanel1.Caption = caption;
        }
        /// <summary>
        /// Cập nhật mô tả trên màn hình chờ.
        /// </summary>
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            this.progressPanel1.Description = description;
        }
        /// <summary>
        /// Chuyển lệnh điều khiển cho lớp màn hình chờ cơ sở.
        /// </summary>
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion
    }
}