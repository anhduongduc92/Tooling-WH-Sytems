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

namespace Tooling_WH_Sytems.Input
{
    /// <summary>
    /// Ghi nhận nhập kho linh kiện đặc biệt theo số lượng và kích thước.
    /// </summary>
    public partial class frmInputSpecPart : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện cùng các giá trị đầu vào của màn hình.
        /// </summary>
        public frmInputSpecPart()
        {
            InitializeComponent();
        }
        public EventHandler eventHandler;
        /// <summary>
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        void LoadControl()
        {
            LoadPartCode();
            LoadSupplierCode();
        }
        /// <summary>
        /// Nạp danh sách mã linh kiện có thể chọn trên màn hình.
        /// </summary>
        void LoadPartCode()
        {
            lkPartCode.Properties.DataSource = PartDAO.Instance.GetList(1);
            lkPartCode.Properties.DisplayMember = "PartCode";
            lkPartCode.Properties.ValueMember = "PartCode";
        }
        /// <summary>
        /// Nạp danh sách nhà cung cấp cho ô lựa chọn.
        /// </summary>
        void LoadSupplierCode()
        {
            lkSupplierCode.Properties.DataSource = SupplierDAO.Instance.GetListSupplier();
            lkSupplierCode.Properties.DisplayMember = "SupplierCode";
            lkSupplierCode.Properties.DisplayMember = "SupplierCode";
        }
        /// <summary>
        /// Hiển thị thông tin linh kiện ứng với mã đang chọn.
        /// </summary>
        void LoadPartName()
        {
            lblPartName.Text = PartDAO.Instance.PartName(lkPartCode.Text);
            
        }
        /// <summary>
        /// Xóa nội dung các ô nhập để chuẩn bị bản ghi mới.
        /// </summary>
        void ClearText()
        {
            lkPartCode.EditValue = null;
            txtQuantPcs.Text = string.Empty;
            txtQuantMM.Text = string.Empty;
        }
        /// <summary>
        /// Kiểm tra thông tin rồi lưu nhập kho linh kiện đặc biệt theo quy tắc quy đổi hiện tại.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string employess = Kun_Static.EmployessCode;
            string partCode = lkPartCode.Text;
            string supCode = lkSupplierCode.Text;
            float quantMM = 0;
            int quantPSC = 0;
            PartDTO partDTO = PartDAO.Instance.GetItemPart(partCode);
            float min = PartDAO.Instance.MinWaintPart(partCode);
            try
            {
                quantMM = (float)Convert.ToDouble(txtQuantMM.Text);
                quantPSC = int.Parse(txtQuantPcs.Text);
            }
            catch
            {
                XtraMessageBox.Show("bạn chưa điền số lượng cần nhập!".ToUpper(),
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime dateIn = DateTime.Now;
            long idPa = PartDAO.Instance.IdPartCode(partCode);
            int idSup = SupplierDAO.Instance.IdSupplier(supCode);
            if (idPa == -1)
            {
                XtraMessageBox.Show("bạn chưa chọn mã linh kiện cần nhập!".ToUpper(),
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (idSup == -1)
            {
                XtraMessageBox.Show("bạn chưa chọn mã nhà cũng cấp!".ToUpper(),
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (XtraMessageBox.Show("bạn muốn nhập linh kiện này?".ToUpper(),
                "Thông Báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                InventoryDAO.Instance.INSERT(partCode,
                    quantMM, dateIn, employess, supCode, "", 1, 1,quantPSC);
                XtraMessageBox.Show("nhập kho thành công !".ToUpper());
                LoadControl();
                ClearText();
            }
            Kun_Static.CheckAction = 1;
        }

        /// <summary>
        /// Giới hạn ký tự được nhập theo quy tắc số lượng hiện có.
        /// </summary>
        private void txtQuantPcs_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && (e.KeyChar != '.' ||
          (e.KeyChar == '.' && (txtQuantPcs.Text.Length == 0 || txtQuantPcs.Text.IndexOf('.') != -1))))
                e.Handled = true;
        }

        /// <summary>
        /// Giới hạn ký tự được nhập theo quy tắc số lượng hiện có.
        /// </summary>
        private void txtQuantMM_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar) && (e.KeyChar != '.' ||
            (e.KeyChar == '.' && (txtQuantMM.Text.Length == 0 || txtQuantMM.Text.IndexOf('.') != -1))))
                e.Handled = true;
        }

        /// <summary>
        /// Nạp lại thông tin liên quan đến mã hoặc đơn vừa được chọn.
        /// </summary>
        private void lkPartCode_EditValueChanged(object sender, EventArgs e)
        {
            LoadPartName();
        }
    }
}