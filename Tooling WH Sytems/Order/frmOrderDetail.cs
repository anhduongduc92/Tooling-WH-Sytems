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
using DevExpress.XtraGrid.Views.Grid;
using DAO;
using DevExpress.XtraGrid;
using DTO;

namespace Tooling_WH_Sytems.Order
{
    /// <summary>
    /// Quản lý chi tiết linh kiện trong đơn yêu cầu.
    /// </summary>
    public partial class frmOrderDetail : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmOrderDetail()
        {
            InitializeComponent();
            LoadControl();
            gridView1.CustomDrawRowIndicator += gridView1_CustomDrawRowIndicator;

        }
        bool Isinsert = false;
        public EventHandler LamMoi;
        long maxId = Kun_Static.MaxIdOrder;
        OrderDTO _order;
        #region Control
        /// <summary>
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        void LoadControl()
        {
            LoadData();
            LockControl();
            LoadPart();
           
        }
        /// <summary>
        /// Khóa vùng nhập liệu và đưa các nút về chế độ xem.
        /// </summary>
        void LockControl()
        {
            btnAdd.Enabled = true;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnSave.Enabled = false;

            lkPartCode.Enabled = false;
            txtQuantity.Enabled = false;
            txtNote.Enabled = false;
            cbPurPose.Enabled = false;
            cbReason.Enabled = false;
        }
        /// <summary>
        /// Mở vùng nhập liệu cho thao tác thêm hoặc chỉnh sửa.
        /// </summary>
        void OpenControl()
        {
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = true;

            lkPartCode.Enabled = true;
            txtQuantity.Enabled = true;
            txtNote.Enabled = true;
            cbPurPose.Enabled = true;
            cbReason.Enabled = true;
        }
        /// <summary>
        /// Xóa nội dung các ô nhập để chuẩn bị bản ghi mới.
        /// </summary>
        void ClearText()
        {
            lkPartCode.Text = String.Empty;
            txtQuantity.Text = String.Empty;
            txtNote.Text = String.Empty;
            cbPurPose.Text = String.Empty;
            cbReason.Text = String.Empty;
            lblId.Text = String.Empty;
        }
        /// <summary>
        /// Đưa dữ liệu của dòng đang chọn lên các ô thông tin.
        /// </summary>
        void AddText()
        {
            try
            {
                lkPartCode.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PartCode"]).ToString();
                txtQuantity.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["QuantityOr"]).ToString();
                txtNote.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Note"]).ToString();
                cbPurPose.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["PurPose"]).ToString();
                lblId.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["IdOrd"]).ToString();
                cbReason.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Reason"]).ToString();
            }
            catch
            {
            }
        }
        /// <summary>
        /// Nạp danh sách linh kiện cho đơn yêu cầu.
        /// </summary>
        void LoadPart()
        {
            List<OnlyPart> listO = InventoryDAO.Instance.OnlyPartCode();
            lkPartCode.Properties.DataSource = listO;
            lkPartCode.Properties.DisplayMember = "PartCode";
            lkPartCode.Properties.ValueMember = "PartCode";
        }
        /// <summary>
        /// Nạp danh mục mục đích sử dụng linh kiện.
        /// </summary>
        void LoadPurPose()
        {
            List<PurPose> listP = DataSorceDAO.Instance.GetListPurPose();
            cbPurPose.DataSource = listP;
            cbPurPose.ValueMember = "PPName";
            cbPurPose.DisplayMember = "PPName";
        }
        /// <summary>
        /// Nạp danh mục lý do sử dụng linh kiện.
        /// </summary>
        void LoadReason()
        {
            List<Reason> listR = DataSorceDAO.Instance.GetListReason();
            cbReason.DataSource = listR;
            cbReason.DisplayMember = "RName";
            cbReason.ValueMember = "RName";
        }
        /// <summary>
        /// Tăng độ rộng cột số thứ tự khi nội dung hiển thị dài hơn.
        /// </summary>
        bool cal(Int32 _Width, GridView _View)
        {
            _View.IndicatorWidth = _View.IndicatorWidth < _Width ? _Width : _View.IndicatorWidth;
            return true;
        }
        /// <summary>
        /// Vẽ số thứ tự dòng và điều chỉnh độ rộng cột chỉ báo của bảng.
        /// </summary>
        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (!gridView1.IsGroupRow(e.RowHandle)) //Nếu không phải là Group
            {
                if (e.Info.IsRowIndicator) //Nếu là dòng Indicator
                {
                    if (e.RowHandle < 0)
                    {
                        e.Info.ImageIndex = 0;
                        e.Info.DisplayText = string.Empty;
                    }
                    else
                    {
                        e.Info.ImageIndex = -1; //Không hiển thị hình
                        e.Info.DisplayText = (e.RowHandle + 1).ToString(); //Số thứ tự tăng dần
                    }
                    SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font); //Lấy kích thước của vùng hiển thị Text
                    Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                    BeginInvoke(new MethodInvoker(delegate { cal(_Width, gridView1); })); //Tăng kích thước nếu Text vượt quá
                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1)); //Nhân -1 để đánh lại số thứ tự tăng dần
                SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                BeginInvoke(new MethodInvoker(delegate { cal(_Width, gridView1); }));
            }
        }
        /// <summary>
        /// Nạp dữ liệu nghiệp vụ vào bảng hiển thị.
        /// </summary>
        void LoadData()
        {
            GCData.DataSource = OrderDetailDAO.Instance.GetListOrderDetailById(maxId);
        }
        /// <summary>
        /// Hiển thị tên và số lượng tồn của linh kiện đang chọn.
        /// </summary>
        void LoadPartInfor()
        {
            string part = lkPartCode.Text;
            txtName.Text = PartDAO.Instance.PartName(part);
            txtTotal.Text = InventoryDAO.Instance.TotalInventoryByPart(part).ToString();
        }
        #endregion
        #region Event
        /// <summary>
        /// Thêm hoặc sửa chi tiết linh kiện và cập nhật trạng thái đơn theo quy tắc hiện tại.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            _order = OrderDAO.Instance.GetListOrderById(maxId);
            int statusOr = _order.StatusOrder;
            if(statusOr == 4)
            {
                XtraMessageBox.Show("phiếu này đã xuất bạn không thể chỉnh sửa!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string PartCode = lkPartCode.Text;
            float QuantityOr = (float)Convert.ToDouble(txtQuantity.Text);
            float total = (float)Convert.ToDouble(txtTotal.Text);
            if (QuantityOr>total)
            {
                XtraMessageBox.Show("số lượng yêu cầu quá lớn!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string PurPose = cbPurPose.Text;
            string Reason = cbReason.Text;
            string Note = txtNote.Text;
            string noteOrder = "Phiều này đã được chỉnh sửa bởi : " + Kun_Static.UserStatic.DisplayName;
            if (Isinsert == true)
            {
                OrderDetailDAO.Instance.InsertOrderDetail(maxId, PartCode, QuantityOr, PurPose, Reason, Note,1);
                OrderDAO.Instance.UpdateOrder(maxId,3, noteOrder);
                XtraMessageBox.Show("thêm thông tin thành công!".ToUpper());
                LoadControl();
            }
            else
            {
                long IdOrd = long.Parse(lblId.Text);
                OrderDetailDAO.Instance.UpdateOrderDetail(IdOrd, maxId, PartCode, QuantityOr, PurPose, Reason, Note,1);
                OrderDAO.Instance.UpdateOrder(maxId, 3, noteOrder);
                XtraMessageBox.Show("sửa thông tin thành công!".ToUpper());
                LoadControl();
            }
            LoadData();
        }
        /// <summary>
        /// Đồng bộ thông tin của dòng đang chọn với vùng nhập liệu hoặc trạng thái thao tác.
        /// </summary>
        private void GCData_Click(object sender, EventArgs e)
        {
            AddText();
        }
        /// <summary>
        /// Chuẩn bị dữ liệu hoặc mở màn hình để thêm bản ghi mới.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Isinsert = true;
            ClearText();
            OpenControl();
        }

        /// <summary>
        /// Xóa dữ liệu đang chọn theo các điều kiện và xác nhận hiện tại.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int statusOr = OrderDAO.Instance.StatusOrder(maxId);
            if (statusOr == 4)
            {
                XtraMessageBox.Show("phiếu này đã xuất bạn không thể chỉnh sửa!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string noteOrder = "Phiều này đã được chỉnh sửa bởi : " + Kun_Static.UserStatic.DisplayName;
            if (XtraMessageBox.Show("bạn muốn xóa thông tin này?".ToUpper(), "Thông Báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                foreach (var item in gridView1.GetSelectedRows())
                {
                    long id = long.Parse(gridView1.GetRowCellValue(item, "IdOrd").ToString());
                    OrderDetailDAO.Instance.DeleteOrderDetail(id);
                    OrderDAO.Instance.UpdateOrder(maxId, 2, noteOrder);
                }
                OrderDAO.Instance.UpdateOrder(maxId, 3, noteOrder);
            }
        }

        /// <summary>
        /// Bắt đầu chỉnh sửa dữ liệu đang chọn theo quyền hiện tại.
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            Isinsert = false;
            OpenControl();
        }

        /// <summary>
        /// Nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            LoadControl();
        }

        #endregion

        /// <summary>
        /// Nạp lại thông tin liên quan đến mã hoặc đơn vừa được chọn.
        /// </summary>
        private void lkPartCode_EditValueChanged(object sender, EventArgs e)
        {
            LoadPartInfor();
            LoadPurPose();
        }

        /// <summary>
        /// Điều chỉnh lựa chọn lý do theo mục đích sử dụng được chọn.
        /// </summary>
        private void cbPurPose_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbPurPose.Text == "Đổi")
            {
                cbReason.Enabled = true;
                txtNote.Enabled = false;
                LoadReason();
            }
            else 
            {
                cbReason.Enabled = false;
                cbReason.Text = String.Empty;
                txtNote.Enabled = true;
            }
        }
        /// <summary>
        /// Tô màu dòng theo các điều kiện trạng thái nghiệp vụ hiện có.
        /// </summary>
        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle >= 0) // chỉ xử lý trong cột họ tên thôi 
            {
                int status = int.Parse(view.GetRowCellValue(e.RowHandle, view.Columns["StatusODetail"]).ToString());

                if (status == 1)
                {
                    e.Appearance.BackColor = Color.Yellow;
                    e.Appearance.ForeColor = Color.Black;
                }
            }
        }

        /// <summary>
        /// Giới hạn ký tự được nhập theo quy tắc số lượng hiện có.
        /// </summary>
        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && (e.KeyChar != '.' ||
            (e.KeyChar == '.' && (txtQuantity.Text.Length == 0 || txtQuantity.Text.IndexOf('.') != -1))))
                e.Handled = true;
        }

        /// <summary>
        /// Phát sự kiện để màn hình gọi nạp lại dữ liệu khi đóng form.
        /// </summary>
        private void frmOrderDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            LamMoi?.Invoke(sender, e);
        }

        /// <summary>
        /// Xác nhận hoàn tất xử lý chi tiết và cập nhật trạng thái đơn.
        /// </summary>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            _order = OrderDAO.Instance.GetListOrderById(maxId);
            int statusOrder = _order.StatusOrder;
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (Kun_Static.UserRight < 3)
            {
                XtraMessageBox.Show("bạn không có quyền !".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if(_order.StatusOrder != 4)
            {
                XtraMessageBox.Show("phiếu này chưa xuất kho!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (XtraMessageBox.Show("bạn thực sự muốn xóa thông tin này ?".ToUpper(), "Thông Báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                foreach (var item in gridView1.GetSelectedRows())
                {
                    long id = long.Parse(gridView1.GetRowCellValue(item, "IdOrd").ToString());
                    string purPose = gridView1.GetRowCellValue(item, "PurPose").ToString();
                    if(purPose == "Đổi")
                    {
                        OrderDetailDAO.Instance.UpdateNoteOrderDetail(id, "");
                    }
                }
                OrderDAO.Instance.UpdateOrder(maxId, statusOrder, "ĐÃ XUẤT KHO");
                LoadControl();
            }
        }
    }
}