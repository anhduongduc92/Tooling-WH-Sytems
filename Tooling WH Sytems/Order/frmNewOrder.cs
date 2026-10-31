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
using DevExpress.XtraGrid.Views.Grid;
using DTO;

namespace Tooling_WH_Sytems.Order
{
    /// <summary>
    /// Quản lý danh sách đơn đặt hàng mới.
    /// </summary>
    public partial class frmNewOrder : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmNewOrder()
        {
            InitializeComponent();
            LoadControl();
        }
        /// <summary>
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void LoadControl()
        {
            List<NewOrderDTO> listNO = NewOrderDAO.Instance.GetListNewOrder();
            foreach (NewOrderDTO item in listNO)
            {
                List<NewOrderDetail> listOD = NewOrderDAO.Instance.GetListNewOrdById(item.Id);
                int dem = 0;
                foreach (NewOrderDetail nitem in listOD)
                {
                    if(nitem.StatusODT == 1)
                    {
                        dem++;
                    }
                }
                if(dem == 0)
                {
                    NewOrderDAO.Instance.UpdateStatusNOD(item.Id, 0);
                }
            }
            GCData.DataSource = NewOrderDAO.Instance.GetListNewOrder();
        }
        /// <summary>
        /// Nạp hoặc mở dữ liệu chi tiết theo lựa chọn hiện tại.
        /// </summary>
        private void btnView_Click(object sender, EventArgs e)
        {
             if(gridView1.GetFocusedRowCellValue("Id") != null)
            {
                long id = long.Parse(gridView1.GetFocusedRowCellValue("Id").ToString());
                Kun_Static.MaxNewIdOrder = id;
                using (frmNewOrderDetail f = new frmNewOrderDetail())
            {
                    f.LamMoi += new EventHandler(btnUpdate_Click);
                    f.ShowDialog();
            }
            }
            else
            {
                XtraMessageBox.Show("bạn chưa chọn mã phiếu cần xem !".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }      
        }

        /// <summary>
        /// Chuẩn bị dữ liệu hoặc mở màn hình để thêm bản ghi mới.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (frmNewInportOrder f = new frmNewInportOrder())
            {
                f.LamMoi += new EventHandler(btnUpdate_Click);
                f.ShowDialog();
            }
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
            if (XtraMessageBox.Show("bạn thực sự muốn xóa thông tin này ?".ToUpper(), "Thông Báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                foreach (var item in gridView1.GetSelectedRows())
                {
                    long id = long.Parse(gridView1.GetRowCellValue(item, "Id").ToString());
                    NewOrderDAO.Instance.DeleteNOD(id);
                    NewOrderDAO.Instance.DeleteNOrd(id);
                }
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
        /// Tô màu dòng theo các điều kiện trạng thái nghiệp vụ hiện có.
        /// </summary>
        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle >= 0) // chỉ xử lý trong cột họ tên thôi 
            {
                int status = int.Parse(view.GetRowCellValue(e.RowHandle, view.Columns["StatusOrder"]).ToString());

                if (status == 1)
                {
                    e.Appearance.BackColor = Color.Aqua;
                    e.Appearance.ForeColor = Color.Black;
                }
            }
        }

        /// <summary>
        /// Đồng bộ thông tin của dòng đang chọn với vùng nhập liệu hoặc trạng thái thao tác.
        /// </summary>
        private void GCData_Click(object sender, EventArgs e)
        {
            if (gridView1.GetFocusedRowCellValue("Id") != null)
            {
                lblId.Text = gridView1.GetFocusedRowCellValue("Id").ToString();
            }
        }
        /// <summary>
        /// Lưu mã đơn đang chọn để các thao tác tiếp theo sử dụng.
        /// </summary>
        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Column.FieldName == "Id")
            {
               
            }
        }

        /// <summary>
        /// Mở chi tiết của đối tượng đang chọn khi nhấp đúp.
        /// </summary>
        private void GCData_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.GetFocusedRowCellValue("Id") != null)
            {
                long id = long.Parse(gridView1.GetFocusedRowCellValue("Id").ToString());
                Kun_Static.MaxNewIdOrder = id;
                using (frmNewOrderDetail f = new frmNewOrderDetail())
            {
                    f.LamMoi += new EventHandler(btnUpdate_Click);
                    f.ShowDialog();
            }
            }
            else
            {
                XtraMessageBox.Show("bạn chưa chọn mã phiếu cần xem !".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}