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
    /// Khởi tạo thông tin chung của đơn yêu cầu.
    /// </summary>
    public partial class frmTheFirtOrderDetail : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmTheFirtOrderDetail()
        {
            InitializeComponent();
            LoadControl();
        }
        public EventHandler LamMoi;
        /// <summary>
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        void LoadControl()
        {
            MaximizeBox = false;
            string employess = Kun_Static.UserStatic.EmCode;
            string room = EmployessDAO.Instance.NameGr(employess);
            if(room == "PC")
            {
                plStyle.Visible = true;
                LoadStyleC();
            }
            else
            {
                plStyle.Visible = false;
            }
            LoadFactory();
        }
   
        /// <summary>
        /// Nạp các lựa chọn kiểu đơn yêu cầu.
        /// </summary>
        void LoadStyleC()
        {
            cbStyleC.Items.Add("Phiếu xuất kho");
            cbStyleC.Items.Add("Phiếu xuất nội bộ");
        }
        /// <summary>
        /// Nạp các lựa chọn nhà máy.
        /// </summary>
        void LoadFactory()
        {
            cbFactory.Items.Add("Đông Dương");
            cbFactory.Items.Add("Đông Dương 2");
            cbFactory.Items.Add("Đông Dương Khuôn");
        }
        /// <summary>
        /// Tạo thông tin chung của đơn rồi mở màn hình nhập chi tiết phù hợp.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string NameOrder = txtOrderName.Text;
            DateTime today = DateTime.Now;
            string employess = Kun_Static.UserStatic.EmCode;
            string room = EmployessDAO.Instance.NameGr(employess);
            string styleC = cbStyleC.Text;
            string Factory = cbFactory.Text;
            if(Factory.Length == 0)
            {
                XtraMessageBox.Show("bạn chưa chọn nhà máy!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (NameOrder.Length == 0)
            {
                XtraMessageBox.Show("bạn chưa điền tên phiếu yêu cầu!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (employess == "ADMINISTRATOR")
            {
                room = "ADMINISTRATOR";
            }
            if (plStyle.Visible == true && styleC.Length == 0)
            {
                XtraMessageBox.Show("bạn chưa chọn loại phiếu yêu cầu!".ToUpper(),"Thông Báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            OrderDAO.Instance.InsertOrder(NameOrder, today, employess, "", 2, styleC, room, Factory);
            long maxId = OrderDAO.Instance.MaxIdOrder();
            Kun_Static.MaxIdOrder = maxId;
            XtraMessageBox.Show("thêm phiếu thành công !\n\nbạn hãy thêm chi tiết phiếu!".ToUpper());
            if(styleC == "Phiếu xuất nội bộ")
            {
                frmOrderDetailLocal f = new frmOrderDetailLocal();
                this.Hide();
                f.ShowDialog();
                this.Close();
            }
            else
            {
                frmInputOrderDetail f = new frmInputOrderDetail();
                this.Hide();
                f.ShowDialog();
                this.Close();
            }
        }
        /// <summary>
        /// Phát sự kiện để màn hình gọi nạp lại dữ liệu khi đóng form.
        /// </summary>
        private void frmTheFirtOrderDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            LamMoi?.Invoke(sender, e);
        }

        /// <summary>
        /// Đồng bộ lựa chọn loại linh kiện trên đơn yêu cầu.
        /// </summary>
        private void ckbPartNomal_CheckedChanged(object sender, EventArgs e)
        {
            ckbPartNomal.Checked = true;
            chkSpecPart.Checked = false;
        }

        /// <summary>
        /// Đồng bộ lựa chọn loại linh kiện trên đơn yêu cầu.
        /// </summary>
        private void chkSpecPart_CheckedChanged(object sender, EventArgs e)
        {
            ckbPartNomal.Checked = false;
            chkSpecPart.Checked = true;
        }
    }
}