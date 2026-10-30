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

namespace Tooling_WH_Sytems.Order
{
    /// <summary>
    /// Tra cứu chi tiết đơn yêu cầu.
    /// </summary>
    public partial class frmViewOrderDetail : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện cùng các giá trị đầu vào của màn hình.
        /// </summary>
        public frmViewOrderDetail()
        {
            InitializeComponent();
            PartCode();
        }
        /// <summary>
        /// Nạp danh sách đơn yêu cầu theo mã đơn giảm dần.
        /// </summary>
        void PartCode()
        {
            lkPart.Properties.DataSource = OrderDAO.Instance.GetListOrder().OrderByDescending(x=>x.IdOrder);
            lkPart.Properties.DisplayMember = "IdOrder";
            lkPart.Properties.ValueMember = "IdOrder";
        }
        /// <summary>
        /// Nạp lại thông tin liên quan đến mã hoặc đơn vừa được chọn.
        /// </summary>
        private void lkPart_EditValueChanged(object sender, EventArgs e)
        {
            long idOrder = long.Parse(lkPart.Text);
            GCData.DataSource = GCData.DataSource = OrderDetailDAO.Instance.GetListOrderDetailById(idOrder);
        }
    }
}