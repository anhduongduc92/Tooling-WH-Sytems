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
using DevExpress.XtraGrid.Views.Grid;

namespace Tooling_WH_Sytems.Order
{
    /// <summary>
    /// Tra cứu chi tiết đơn đặt hàng mới.
    /// </summary>
    public partial class frmViewNewOrderDetail : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện cùng các giá trị đầu vào của màn hình.
        /// </summary>
        public frmViewNewOrderDetail()
        {
            InitializeComponent();
            LoadOrder();
        }
        /// <summary>
        /// Nạp danh sách đơn đặt hàng để tra cứu.
        /// </summary>
        private void LoadOrder()
        {
            lkOrder.Properties.DataSource = NewOrderDAO.Instance.GetListNewOrder();
            lkOrder.Properties.DisplayMember = "Id";
            lkOrder.Properties.ValueMember = "Id";
        }
        /// <summary>
        /// Nạp hoặc mở dữ liệu chi tiết theo lựa chọn hiện tại.
        /// </summary>
        private void btnView_Click(object sender, EventArgs e)
        {
            long id = long.Parse(lkOrder.Text);
            GCData.DataSource = NewOrderDAO.Instance.GetListNewOrdById(id);
        }

        /// <summary>
        /// Tô màu dòng theo các điều kiện trạng thái nghiệp vụ hiện có.
        /// </summary>
        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if(e.RowHandle >=0)
            {
                int status = int.Parse(view.GetRowCellValue(e.RowHandle, view.Columns["StatusODT"]).ToString());
                if(status == 1)
                {
                    e.Appearance.BackColor = Color.Aqua;
                    e.Appearance.ForeColor = Color.Black;
                }
            }
        }
    }
}