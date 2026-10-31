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
using DevExpress.Utils.Frames;
using DevExpress.XtraBars.Alerter;
using DTO;
using DevExpress.XtraGrid.Views.Grid;
using Tooling_WH_Sytems.Report;
using DevExpress.XtraReports.UI;

namespace Tooling_WH_Sytems.Order
{
    /// <summary>
    /// Quản lý, duyệt và xác nhận các đơn yêu cầu.
    /// </summary>
    public partial class frmOrderList : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmOrderList()
        {
            InitializeComponent();
            LoadControl();
            timer1.Start();
        }
        private OrderDTO _order;
        private OrderDetailDTO _orderDerail;
        /// <summary>
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void LoadControl()
        {
            LoadDate();
            LoadData();
            LadWarning();
        }
        /// <summary>
        /// Thiết lập khoảng ngày mặc định để lọc danh sách đơn.
        /// </summary>
        private void LoadDate()
        {
            DateTime today = DateTime.Now;
            dtpkFrom.Value = today.Date.AddDays(1 - today.Day);
            dtpkTo.Value = dtpkFrom.Value.AddMonths(1).AddSeconds(10);
        }
        /// <summary>
        /// Nạp dữ liệu nghiệp vụ vào bảng hiển thị.
        /// </summary>
        private void LoadData()
        {
            DateTime date1 = dtpkFrom.Value;
            DateTime date2 = dtpkTo.Value;
            string emCode = Kun_Static.UserStatic.EmCode;
            string nameR = EmployessDAO.Instance.NameGr(emCode);
            List<OrderDTO> listO = new List<OrderDTO>();
            if (emCode.ToUpper() == "ADMINISTRATOR" || (nameR.ToUpper() == "PC" && Kun_Static.UserRight >= 3))
            {
                listO = OrderDAO.Instance.GetListOrder().Where(x=>x.DateCreate>= date1 && x.DateCreate <= date2).OrderBy(x=>x.StatusOrder).ToList();
            }
            else if(Kun_Static.UserRight > 3)
            {
                listO = OrderDAO.Instance.GetListOrder().Where(x => x.Room != "PC" && x.DateCreate >= date1 && x.DateCreate <= date2).OrderBy(x => x.StatusOrder).ToList();
            }
            else
            {
                listO = OrderDAO.Instance.GetListOrder().Where(x => x.Room == nameR && x.DateCreate >= date1 && x.DateCreate <= date2).OrderBy(x => x.StatusOrder).ToList();
            }
            GCData.DataSource = listO;
        }
        /// <summary>
        /// Thiết lập nội dung thông báo cho danh sách đơn yêu cầu.
        /// </summary>
        private void LoadAlert()
        {
            AlertButton btn = new AlertButton(Properties.Resources.Apply_16x16);
            btn.Hint = "Open File";
            btn.Style = AlertButtonStyle.CheckButton;
            btn.Down = true;
            btn.Name = "buttonOpen";
            if (Kun_Static.UserRight >= 3)
            {
                //alCWarning.Buttons.Add(btn);
                //alCWarning.ButtonClick += new AlertButtonClickEventHandler(alCWarning_ButtonClick);
            }
        }
        /// <summary>
        /// Lọc các đơn cần xử lý theo phòng ban và hiển thị cảnh báo.
        /// </summary>
        private void LadWarning()
        {
            if (Kun_Static.UserRight >= 3)
            {
                string emCode = Kun_Static.UserStatic.EmCode;
                string nameR = EmployessDAO.Instance.NameGr(emCode);
                List<OrderDTO> listO = new List<OrderDTO>();
                if (emCode.ToUpper() == "ADMINISTRATOR" || (nameR.ToUpper() == "PC" && Kun_Static.UserRight >= 3))
                {
                    listO = OrderDAO.Instance.GetListOrder().Where(x => x.StatusOrder > 0 && x.StatusOrder < 4).ToList();
                }
                else
                {
                    listO = OrderDAO.Instance.GetListOrder().Where(x => x.Room == nameR && x.StatusOrder > 0 && x.StatusOrder < 4).ToList();
                }
                if (listO.Count > 0)
                {
                    alCWarning.Show(this, "Thông Báo", "Mã phiếu cần phê duyệt", Properties.Resources.Action_Bell_32x32);
                }
            }
        }
        /// <summary>
        /// Mở chi tiết đơn từ nút thao tác của thông báo cảnh báo.
        /// </summary>
        private void alCWarning_ButtonClick(object sender, AlertButtonClickEventArgs e)
        {
            if (e.ButtonName == "buttonOpen")
            {
                using (frmOrderList f = new frmOrderList())
            {
                    f.ShowDialog();
            }
            }
        }

        /// <summary>
        /// Chuẩn bị dữ liệu hoặc mở màn hình để thêm bản ghi mới.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (!UiActions.HasPermission(2))
            {
                return;
            }
            using (frmTheFirtOrderDetail f = new frmTheFirtOrderDetail())
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
                    long id = long.Parse(gridView1.GetRowCellValue(item, "IdOrder").ToString());
                    OrderDAO.Instance.DeleteOrder(id);
                    OrderDetailDAO.Instance.DeleteOrderDetailByOrder(id);
                }
                LoadControl();
            }
        }

        /// <summary>
        /// Nạp hoặc mở dữ liệu chi tiết theo lựa chọn hiện tại.
        /// </summary>
        private void btnView_Click(object sender, EventArgs e)
        {
            long id = 0;
            try
            {
                id = long.Parse(lblId.Text);
            }
            catch
            {
                XtraMessageBox.Show("bạn chưa chọn phiếu cần xem!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Kun_Static.MaxIdOrder = id;
            using (frmOrderDetail f = new frmOrderDetail())
            {
                f.LamMoi += new EventHandler(btnUpdate_Click);
                f.ShowDialog();
            }
        }

        /// <summary>
        /// Duyệt đơn theo tài khoản và trạng thái được phép.
        /// </summary>
        private void btnAppro_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (!UiActions.HasPermission(4))
            {
                return;
            }
            DateTime today = DateTime.Now;
            string user = Kun_Static.UserStatic.EmCode;
            if (XtraMessageBox.Show("bạn muốn phê duyệt phiếu này ?".ToUpper(), "Thông Báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                long id = long.Parse(lblId.Text);
                int status = OrderDAO.Instance.StatusOrder(id);
                if (status != 1)
                {
                    XtraMessageBox.Show("phiếu này chưa được xác nhận!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                OrderDAO.Instance.ApproOrder(id, user, 0, today);
                LoadControl();
            }
        }
        /// <summary>
        /// Chuẩn bị dữ liệu và hiển thị bản xem trước khi in.
        /// </summary>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (gridView1.GetFocusedRowCellValue("IdOrder") != null)
            {
                long idOrder = long.Parse(gridView1.GetFocusedRowCellValue("IdOrder").ToString());
                _order = OrderDAO.Instance.GetListOrderById(idOrder);
                _orderDerail = OrderDetailDAO.Instance.OrderDetailByIdOrder(idOrder);
                if(_orderDerail == null)
                {
                    XtraMessageBox.Show("mã phiếu chưa có linh kiện nào".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<ReportOrderDetail> listR = new List<ReportOrderDetail>();
                int statusOrder = _order.StatusOrder;
                string room = _order.Room;
                string barcode = idOrder + "&" + room;
                string purPose = _orderDerail.PurPose;
                if (purPose == "Xuất nội bộ")
                {
                    barcode = barcode + "&A";
                }
                if (statusOrder > 0 && statusOrder < 3)
                {
                    XtraMessageBox.Show("mã phiếu chưa được phê duyệt".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<ReportOrderDetail> listOR = OrderDAO.Instance.GetListRPOrderDetail(idOrder);
                foreach (ReportOrderDetail item in listOR)
                {
                    string emCreate = EmployessDAO.Instance.EmployessName(item.PersonCreate);
                    string emAppro = EmployessDAO.Instance.EmployessName(item.PersonAppro);
                    string emConfirm = EmployessDAO.Instance.EmployessName(item.PersonConfirm);
                    listR.Add(new ReportOrderDetail(barcode, item.PartCode, item.PartName, item.QuantityOr, item.Unit, item.PurPose, item.Reason, item.DateCreate, emCreate, item.DateAppro, emAppro, item.Note, emConfirm, item.DateConfirm));
                }
                rpOrderdetail rp = new rpOrderdetail();
                rp.DataSource = listR;
                rp.PrintDialog();
            }
            else
            {
                XtraMessageBox.Show("bạn chưa chọn mã phiếu cần in".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Đồng bộ thông tin của dòng đang chọn với vùng nhập liệu hoặc trạng thái thao tác.
        /// </summary>
        private void GCData_Click(object sender, EventArgs e)
        {
            try
            {
                lblId.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["IdOrder"]).ToString();
            }
            catch (Exception ex)
            {
                AppLog.Write("frmOrderList", ex);
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

                if (status == 2)
                {
                    e.Appearance.BackColor = Color.Orange;
                    e.Appearance.ForeColor = Color.Black;
                }
                else if (status == 3)
                {
                    e.Appearance.BackColor = Color.Yellow;
                    e.Appearance.ForeColor = Color.Black;
                }
                else if (status == 1)
                {
                    e.Appearance.BackColor = Color.Aqua;
                    e.Appearance.ForeColor = Color.Black;
                }
                else if (status == 0)
                {
                    e.Appearance.BackColor = Color.Green;
                    e.Appearance.ForeColor = Color.Black;
                }
            }
        }

        /// <summary>
        /// Tự động nạp lại danh sách đơn theo chu kỳ.
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 600000;
            LoadControl();
        }

        /// <summary>
        /// Khởi tạo cảnh báo khi màn hình danh sách đơn được tải.
        /// </summary>
        private void frmOrderList_Load(object sender, EventArgs e)
        {
            LoadAlert();
        }

        /// <summary>
        /// Xác nhận đơn theo quyền và trạng thái được phép.
        /// </summary>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (!UiActions.HasPermission(3))
            {
                return;
            }
            DateTime today = DateTime.Now;
            string user = Kun_Static.UserStatic.EmCode;
            if (XtraMessageBox.Show("bạn muốn phê duyệt phiếu này?".ToUpper(), "Thông Báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                long id = long.Parse(lblId.Text);
                int status = OrderDAO.Instance.StatusOrder(id);
                if (status != 2 && status != 3)
                {
                    XtraMessageBox.Show("phiếu này đã được xác nhận!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                OrderDAO.Instance.ConfirmOrder(id, user, 1, today);
                LoadControl();
            }
        }

        /// <summary>
        /// Dừng bộ định thời khi đóng danh sách đơn.
        /// </summary>
        private void frmOrderList_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }

        /// <summary>
        /// Lưu mã đơn đang chọn để các thao tác tiếp theo sử dụng.
        /// </summary>
        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Column.FieldName == "IdOrder")
            {
                
            }
        }

        /// <summary>
        /// Mở chi tiết của đối tượng đang chọn khi nhấp đúp.
        /// </summary>
        private void btnView_DoubleClick(object sender, EventArgs e)
        {
            long id = 0;
            try
            {
                id = long.Parse(lblId.Text);
            }
            catch
            {
                XtraMessageBox.Show("bạn chưa chọn phiếu cần xem!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Kun_Static.MaxIdOrder = id;
            using (frmOrderDetail f = new frmOrderDetail())
            {
                f.LamMoi += new EventHandler(btnUpdate_Click);
                f.ShowDialog();
            }
        }

        /// <summary>
        /// Lọc danh sách đơn theo các điều kiện hiện tại.
        /// </summary>
        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}