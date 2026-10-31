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

namespace Tooling_WH_Sytems.Input
{
    /// <summary>
    /// Ghi nhận nhập kho nội bộ.
    /// </summary>
    public partial class frmInputLocal : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmInputLocal()
        {
            InitializeComponent();
            LoadControl();
        }
        public EventHandler eventHandler;
        /// <summary>
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void LoadControl()
        {
            LoadPartCode();
            LoadSupplierCode();
        }
        /// <summary>
        /// Nạp danh sách mã linh kiện có thể chọn trên màn hình.
        /// </summary>
        private void LoadPartCode()
        {
            lkPartCode.Properties.DataSource = PartDAO.Instance.GetListPart();
            lkPartCode.Properties.DisplayMember = "PartCode";
            lkPartCode.Properties.ValueMember = "PartCode";
        }
        /// <summary>
        /// Nạp danh sách nhà cung cấp cho ô lựa chọn.
        /// </summary>
        private void LoadSupplierCode()
        {
            lkSupplierCode.Properties.DataSource = SupplierDAO.Instance.GetListSupplier();
            lkSupplierCode.Properties.DisplayMember = "SupplierCode";
            lkSupplierCode.Properties.DisplayMember = "SupplierCode";
        }
        /// <summary>
        /// Hiển thị thông tin linh kiện ứng với mã đang chọn.
        /// </summary>
        private void LoadPartName()
        {
            lblPartName.Text = PartDAO.Instance.PartName(lkPartCode.Text);
            lblUnit.Text = PartDAO.Instance.UnitPart(lkPartCode.Text);
        }
        /// <summary>
        /// Kiểm tra thông tin và ghi nhận nhập kho nội bộ.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string employess = Kun_Static.EmployessCode;
            string partCode = lkPartCode.Text;
            string supCode = lkSupplierCode.Text;
            float quantity = 0;
            try
            {
                quantity = (float)Convert.ToDouble(txtQuantity.Text);
            }
            catch
            {
                XtraMessageBox.Show("bạn chưa điền số lượng cần nhập!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string Note = txtNote.Text;
            DateTime dateIn = DateTime.Now;
            long idPa = PartDAO.Instance.IdPartCode(partCode);
            int idSup = SupplierDAO.Instance.IdSupplier(supCode);
            if (idPa == -1)
            {
                XtraMessageBox.Show("bạn chưa chọn mã linh kiện cần nhập!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (idSup == -1)
            {
                XtraMessageBox.Show("bạn chưa chọn mã nhà cũng cấp!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(Note.Length ==0)
            {
                XtraMessageBox.Show("bạn chưa điền lý do nhập!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            InventoryDAO.Instance.InsertInput(partCode, quantity, dateIn, employess, supCode, Note, 1, 1);
            XtraMessageBox.Show("nhập kho thành công !".ToUpper());
            Kun_Static.CheckAction = 1;
            this.Close();
        }

        /// <summary>
        /// Nạp lại thông tin liên quan đến mã hoặc đơn vừa được chọn.
        /// </summary>
        private void lkPartCode_EditValueChanged(object sender, EventArgs e)
        {
            LoadPartName();
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
        private void frmInputLocal_FormClosing(object sender, FormClosingEventArgs e)
        {
            eventHandler?.Invoke(sender, e);
        }
    }
}