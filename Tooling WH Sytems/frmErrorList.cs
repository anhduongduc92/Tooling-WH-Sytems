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

namespace Tooling_WH_Sytems
{
    /// <summary>
    /// Hiển thị danh sách lỗi nhập dữ liệu.
    /// </summary>
    public partial class frmErrorList : DevExpress.XtraEditors.XtraForm
    {
        private List<string> errorList;
        /// <summary>
        /// Khởi tạo giao diện cùng các giá trị đầu vào của màn hình.
        /// </summary>
        public frmErrorList()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Khởi tạo giao diện cùng các giá trị đầu vào của màn hình.
        /// </summary>
        public frmErrorList(List<string> errorlist)
        {
            errorList = errorlist;
            InitializeComponent();
        }

        /// <summary>
        /// Hiển thị danh sách lỗi được truyền vào màn hình.
        /// </summary>
        private void frmErrorList_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < errorList.Count; i++)
            {
                listBox1.Items.Add(errorList[i]);
            }
        }
    }
}