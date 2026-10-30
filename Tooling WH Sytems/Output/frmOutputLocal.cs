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

namespace Tooling_WH_Sytems.Output
{
    /// <summary>
    /// Thực hiện xuất kho theo đơn nội bộ.
    /// </summary>
    public partial class frmOutputLocal : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện cùng các giá trị đầu vào của màn hình.
        /// </summary>
        public frmOutputLocal()
        {
            InitializeComponent();
           
        }
        public EventHandler eventHandler;
        /// <summary>
        /// Kiểm tra đơn nội bộ và tồn kho, ghi nhận xuất theo từng phiếu nhập.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string barCode = txtBarCode.Text;
                string[] array = barCode.Split('&');
                if (array.Count() == 2)
                {
                    XtraMessageBox.Show("đây là phiếu xuất kho\nbạn hãy chọn lại xuất kho".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                long idOrder = long.Parse(array[0]);
                int statusOr = OrderDAO.Instance.StatusOrder(idOrder);
                if (statusOr == 4)
                {
                    XtraMessageBox.Show("phiếu này đã xuất kho rồi \n\nbạn hãy kiểm tra lại!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (statusOr == 1 || statusOr == 2 || statusOr == 3)
                {
                    XtraMessageBox.Show("phiếu này đã chưa được phê duyệt \n\nbạn hãy liên lạc cấp trên để phê duyệt!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<OrderDetailDTO> listO = new List<OrderDetailDTO>();
                listO = OrderDetailDAO.Instance.GetListOrderDetailById(idOrder);
                DateTime DateOut = DateTime.Now;
                string EmployessCode = Kun_Static.EmployessCode;
                string Note = "Xuất nội bộ";
                int dem = 0;
                foreach (OrderDetailDTO item in listO)
                {
                    string partCode = item.PartCode;
                    float QuantityOut = item.QuantityOr;
                    float total = InventoryDAO.Instance.TotalInventoryByPart(partCode);
                    if (total < QuantityOut)
                    {
                        dem++;
                    }
                }
                if (dem > 0)
                {
                    XtraMessageBox.Show("Số lượng tồn kho không đủ bạn hãy làm lại phiếu xuất kho!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string msg = string.Format("bạn muốn xuất phiếu : {0}", idOrder);
                if (XtraMessageBox.Show(msg.ToUpper(), "Thông Báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    foreach (OrderDetailDTO item in listO)
                    {
                        string partCode = item.PartCode;
                        float QuantityOut = item.QuantityOr;
                        List<InventoryDTO> listI = InventoryDAO.Instance.GetListInventoryByCode(partCode);
                        foreach (InventoryDTO newitem in listI)
                        {
                            long idInput = newitem.Id;
                            float inventory = newitem.QuantityInventory;
                            float b = QuantityOut - inventory;
                            if (b <= 0)
                            {
                                if (QuantityOut == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    InventoryDAO.Instance.InsertOutputPart(idInput, DateOut, QuantityOut, EmployessCode, Note,"");
                                    if (b == 0)
                                    {
                                        InventoryDAO.Instance.UpdateStatusInput(item.IdOrd, 0);
                                    }
                                    QuantityOut = 0;
                                    break;
                                }
                            }
                            else
                            {
                                InventoryDAO.Instance.InsertOutputPart(idInput, DateOut, inventory, EmployessCode, Note,"");
                                InventoryDAO.Instance.UpdateStatusInput(item.IdOrd, 0);
                                QuantityOut = b;
                            }
                        }
                    }
                    MessageBox.Show("bạn xuất thành công !".ToUpper());
                    OrderDAO.Instance.UpdateOrder(idOrder, 4, "Đã xuất kho".ToUpper());
                    Kun_Static.CheckAction = 1;
                    this.Close();
                }
                #region Xuất kho
                #endregion


            }
            catch
            {
                XtraMessageBox.Show("Mã vạch không đúng!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Phát sự kiện để màn hình gọi nạp lại dữ liệu khi đóng form.
        /// </summary>
        private void frmOutputLocal_FormClosing(object sender, FormClosingEventArgs e)
        {
            eventHandler?.Invoke(sender, e);
        }
    }
}