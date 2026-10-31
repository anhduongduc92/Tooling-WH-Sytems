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
using DTO;
using DevExpress.XtraGrid.Views.Grid;

namespace Tooling_WH_Sytems.Order
{
    /// <summary>
    /// Xem, chỉnh sửa và xác nhận chi tiết đơn đặt hàng mới.
    /// </summary>
    public partial class frmNewOrderDetail : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmNewOrderDetail()
        {
            InitializeComponent();
            LoadControl();
        }
        public EventHandler LamMoi;
        /// <summary>
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void LoadControl()
        {
            LoadData();
        }
        /// <summary>
        /// Nạp dữ liệu nghiệp vụ vào bảng hiển thị.
        /// </summary>
        private void LoadData()
        {
            GCData.DataSource = NewOrderDAO.Instance.GetListNewOrdById(Kun_Static.MaxNewIdOrder);
        }
        /// <summary>
        /// Xác nhận số lượng nhận của dòng chi tiết đang chọn.
        /// </summary>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            DateTime today = DateTime.Now;
            if (XtraMessageBox.Show("bạn hãy kiểm tra số lượng giao trước khi xác nhận!".ToUpper(), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                foreach (var item in gridView1.GetSelectedRows())
                {
                    int status = 0;
                    long id = long.Parse(gridView1.GetRowCellValue(item, "Id").ToString());
                    float qauntity = (float)Convert.ToDouble(gridView1.GetRowCellValue(item, "Quantity").ToString());
                    float QuantityOr = 0;
                    try
                    {
                        QuantityOr = (float)Convert.ToDouble(gridView1.GetRowCellValue(item, "QuantityOut").ToString());
                    }
                    catch (Exception ex)
            {
                AppLog.Write("frmNewOrderDetail", ex);
            }
                    if((qauntity-QuantityOr) == 0)
                    {
                        status = 0;
                    }
                    else
                    {
                        status = 1;
                    }
                    NewOrderDAO.Instance.UpdateNOrd(id, QuantityOr, today, status, Kun_Static.UserStatic.UserName);
                }
                LoadControl();
            }
        }
        /// <summary>
        /// Xác nhận toàn bộ chi tiết và cập nhật trạng thái đơn đặt hàng.
        /// </summary>
        private void btnConfirmAll_Click(object sender, EventArgs e)
        {
            DateTime today = DateTime.Now;
            DataRow dr;
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                dr = gridView1.GetDataRow(i);
                long id = long.Parse(gridView1.GetRowCellValue(i, "Id").ToString());
                float QuantityOr = 0;
                try
                {
                    QuantityOr = (float)Convert.ToDouble(gridView1.GetRowCellValue(i, "Quantity").ToString());
                }
                catch (Exception ex)
            {
                AppLog.Write("frmNewOrderDetail", ex);
            }
                NewOrderDAO.Instance.UpdateNOrd(id, QuantityOr, today, 0, Kun_Static.UserStatic.UserName);
            }
            XtraMessageBox.Show("xác nhận phiếu thành công !".ToUpper());
            NewOrderDAO.Instance.UpdateStatusNOD(Kun_Static.MaxNewIdOrder, 0);
            LoadControl();
            this.Close();
        }

        /// <summary>
        /// Xóa dữ liệu đang chọn theo các điều kiện và xác nhận hiện tại.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("bạn muốn xóa thông tin này!".ToUpper(), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                foreach (var item in gridView1.GetSelectedRows())
                {
                    long id = long.Parse(gridView1.GetRowCellValue(item, "Id").ToString());
                    NewOrderDAO.Instance.DeleteNOrd(id);
                }
                LoadControl();
            }
        }

        /// <summary>
        /// Bắt đầu chỉnh sửa dữ liệu đang chọn theo quyền hiện tại.
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("bạn muốn sửa thông tin này!".ToUpper(), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (gridView1.GetFocusedRowCellValue("Id") != null)
                {
                    long id = long.Parse(gridView1.GetFocusedRowCellValue("Id").ToString());
                    string PartCode = gridView1.GetFocusedRowCellValue("PartCode").ToString();
                    string PartName = gridView1.GetFocusedRowCellValue("PartName").ToString();
                    float Quantity = (float)Convert.ToDouble(gridView1.GetFocusedRowCellValue("Id").ToString());
                    string Unit = gridView1.GetFocusedRowCellValue("Unit").ToString();
                    string PurPose = gridView1.GetFocusedRowCellValue("PurPose").ToString();
                    string Note = gridView1.GetFocusedRowCellValue("Note").ToString();
                    string Maker = gridView1.GetFocusedRowCellValue("Maker").ToString();
                    NewOrderDAO.Instance.UpdateNOrdNotConfirm(id, PartCode, PartName, Quantity, Unit, PurPose, Note, Maker);
                }
                LoadControl();
            }
        }

        /// <summary>
        /// Tô màu dòng theo các điều kiện trạng thái nghiệp vụ hiện có.
        /// </summary>
        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if ( e.RowHandle >= 0)
            {
                int status = int.Parse(view.GetRowCellValue(e.RowHandle, view.Columns["StatusODT"]).ToString());
                if(status == 1)
                {
                    e.Appearance.BackColor = Color.Aqua;
                    e.Appearance.ForeColor = Color.Black;
                }
            }
        }

        /// <summary>
        /// Phát sự kiện để màn hình gọi nạp lại dữ liệu khi đóng form.
        /// </summary>
        private void frmNewOrderDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            LamMoi?.Invoke(sender, e);
        }
    }
}