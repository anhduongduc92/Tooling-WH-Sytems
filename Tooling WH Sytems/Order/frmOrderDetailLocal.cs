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
using DTO;
using DAO;

namespace Tooling_WH_Sytems.Order
{
    /// <summary>
    /// Lập danh sách linh kiện trong đơn nội bộ.
    /// </summary>
    public partial class frmOrderDetailLocal : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmOrderDetailLocal()
        {
            InitializeComponent();
            LoadControl();
        }
        private List<OrderDetailDTO> listO = new List<OrderDetailDTO>();
        /// <summary>
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void LoadControl()
        {
            LoadPart();
            ControlBox = false;
            ShowIcon = false;
        }
        /// <summary>
        /// Nạp dữ liệu nghiệp vụ vào bảng hiển thị.
        /// </summary>
        private void LoadData()
        {
            List<OrderDetailDTO> listOd = new List<OrderDetailDTO>();
            foreach (OrderDetailDTO item in listO)
            {
                listOd.Add(new OrderDetailDTO(item.IdOrd, item.IdOrder, 
                    item.PartCode, item.PartName, item.QuantityOr,
                    item.PurPose, item.Reason, item.Note, item.StatusODetail));
            }
            GCData.DataSource = listOd;
        }
        /// <summary>
        /// Nạp danh sách linh kiện cho đơn yêu cầu.
        /// </summary>
        private void LoadPart()
        {
            lkPartCode.Properties.DataSource = InventoryDAO.Instance.OnlyPartCode();
            lkPartCode.Properties.DisplayMember = "PartCode";
            lkPartCode.Properties.ValueMember = "PartCode";
        }
        /// <summary>
        /// Hiển thị tên và số lượng tồn của linh kiện đang chọn.
        /// </summary>
        private void LoadPartInfor()
        {
            string part = lkPartCode.Text;
            txtPartName.Text = PartDAO.Instance.PartName(part);
            txtTotal.Text = InventoryDAO.Instance.TotalInventoryByPart(part).ToString();
        }
        /// <summary>
        /// Xóa nội dung các ô nhập để chuẩn bị bản ghi mới.
        /// </summary>
        private void ClearText()
        {
            lkPartCode.Text = String.Empty;
            //txtPartName.Text = String.Empty;
            txtQuantityOr.Text = String.Empty;
            //txtTotal.Text = String.Empty;
            txtNote.Text = String.Empty;
        }
        /// <summary>
        /// Tạo các chi tiết đơn nội bộ từ danh sách linh kiện đã nhập.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            long IdOrder = Kun_Static.MaxIdOrder;
            DataRow dr;
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                dr = gridView1.GetDataRow(i);
                string PartCode = gridView1.GetRowCellValue(i, "PartCode").ToString();
                float QuantityOr = 0;
                try
                {
                    QuantityOr = (float)Convert.ToDouble(gridView1.GetRowCellValue(i, "QuantityOr").ToString());
                }
                catch (Exception ex)
            {
                AppLog.Write("frmOrderDetailLocal", ex);
            }
                string PurPose = gridView1.GetRowCellValue(i, "PurPose").ToString();
                string Reason = "";
                string Note = gridView1.GetRowCellValue(i, "Note").ToString();
                if (PurPose == "Đổi")
                {
                    Note = "";
                }
                if (PartDAO.Instance.IdPartCode(PartCode) > 0)
                {
                    OrderDetailDAO.Instance.InsertOrderDetail(IdOrder, PartCode, QuantityOr, PurPose, Reason, Note, 0);
                }
            }
            XtraMessageBox.Show("Thêm phiếu thành công !".ToUpper());
            LoadData();
            this.Close();
        }
        /// <summary>
        /// Chuẩn bị dữ liệu hoặc mở màn hình để thêm bản ghi mới.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string PartCode = lkPartCode.Text;
            string partName = txtPartName.Text;
            string PurPose = "Xuất nội bộ";
            string Reason = "";
            string Note = txtNote.Text;
            if (PartDAO.Instance.IdPartCode(PartCode) == -1)
            {
                XtraMessageBox.Show("mã linh kiện không đúng!".ToUpper(),
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if ( Note.Length == 0)
            {
                XtraMessageBox.Show("bạn phải ghi dõ lý do xuất nội bộ!".ToUpper(), 
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            float QuantityOr = 0;
            float total = (float)Convert.ToDouble(txtTotal.Text);
            try
            {
                QuantityOr = (float)Convert.ToDouble(txtQuantityOr.Text);
                if (QuantityOr > total)
                {
                    XtraMessageBox.Show("Số lượng yêu cầu quá lớn!".ToUpper(),
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch
            {
                XtraMessageBox.Show("bạn chưa điền số lượng yêu cầu!".ToUpper(),
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            long Maxid = 0;
            if (listO.Count > 0)
            {
                Maxid = listO.Count();
            }
            listO.Add(new OrderDetailDTO(Maxid, Kun_Static.MaxIdOrder,
                PartCode, partName, QuantityOr, PurPose, Reason, Note, 1));
            LoadData();
            ClearText();
        }

        /// <summary>
        /// Nạp lại thông tin liên quan đến mã hoặc đơn vừa được chọn.
        /// </summary>
        private void lkPartCode_EditValueChanged(object sender, EventArgs e)
        {
            LoadPartInfor();
        }
        /// <summary>
        /// Giới hạn ký tự được nhập theo quy tắc số lượng hiện có.
        /// </summary>
        private void txtQuantityOr_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && (e.KeyChar != '.' ||
         (e.KeyChar == '.' && (txtQuantityOr.Text.Length == 0 || txtQuantityOr.Text.IndexOf('.') != -1))))
                e.Handled = true;
        }

        /// <summary>
        /// Xóa dữ liệu đang chọn theo các điều kiện và xác nhận hiện tại.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (var item in gridView1.GetSelectedRows())
            {
                long id = long.Parse(gridView1.GetRowCellValue(item, "IdOrd").ToString());
                listO.RemoveAll(x => x.IdOrd == id);
            }
            LoadData();
        }

    }
}