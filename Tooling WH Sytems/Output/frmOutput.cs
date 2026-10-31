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
    /// Thực hiện xuất kho theo đơn yêu cầu.
    /// </summary>
    public partial class frmOutput : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện cùng các giá trị đầu vào của màn hình.
        /// </summary>
        public frmOutput()
        {
            InitializeComponent();
        }
        public EventHandler eventHandler;
        /// <summary>
        /// Kiểm tra đơn và tồn kho, phân bổ xuất theo từng phiếu nhập rồi cập nhật trạng thái liên quan.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string barCode = txtBarCode.Text;
                string[] array = barCode.Split('&');
                if(array.Count() == 3)
                {
                    XtraMessageBox.Show("đây là phiếu xuất nội bộ\nbạn hãy chọn lại xuất nội bộ"
                        .ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                long idOrder = long.Parse(array[0]);
                OrderDTO _order = OrderDAO.Instance.GetListOrderById(idOrder);
                int statusOr = _order.StatusOrder;
                string factory = _order.Factory;
                if(statusOr == 4)
                {
                    XtraMessageBox.Show("phiếu này đã xuất kho rồi \n\nbạn hãy kiểm tra lại!"
                        .ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (statusOr == 1 || statusOr == 2 || statusOr == 3)
                {
                    XtraMessageBox.Show("phiếu này đã chưa được phê duyệt \n\nbạn hãy liên lạc cấp trên để phê duyệt!"
                        .ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<OrderDetailDTO> listO = new List<OrderDetailDTO>();
                listO = OrderDetailDAO.Instance.GetListOrderDetailById(idOrder);
                DateTime DateOut = DateTime.Now;
                string EmployessCode = Kun_Static.EmployessCode;
                string Note = array[1];
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
                    XtraMessageBox.Show("Số lượng tồn kho không đủ bạn hãy làm lại phiếu xuất kho!"
                        .ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string msg =string.Format("bạn muốn xuất phiếu : {0}",idOrder);
                if (XtraMessageBox.Show(msg.ToUpper(), "Thông Báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    int kun = 0;
                    
                    foreach (OrderDetailDTO item in listO)
                    {
                       
                        string partCode = item.PartCode;
                        string unit = PartDAO.Instance.UnitPart(partCode);
                        string partOld = "";
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
                                    InventoryDAO.Instance.InsertOutputPart(idInput, DateOut, QuantityOut, EmployessCode, Note,factory);
                                    if(b == 0)
                                    {
                                        InventoryDAO.Instance.UpdateStatusInput(item.IdOrd, 0);
                                    }                      
                                    QuantityOut = 0;
                                    break;
                                }
                            }
                            else
                            {
                                InventoryDAO.Instance.InsertOutputPart(idInput, DateOut, inventory, EmployessCode, Note,factory);
                                InventoryDAO.Instance.UpdateStatusInput(item.IdOrd, 0);
                                QuantityOut = b;
                            }
                        }
                        if (item.Reason.Length > 0 && item.PurPose == "Đổi")
                        {
                            if(kun == 0)
                            {
                                if (XtraMessageBox.Show("Số lượng đổi trả đủ!".ToUpper()
                                    , "Thông Báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    kun = 2;
                                    partOld = partCode + "-OLD";
                                    long idPart = PartDAO.Instance.IdPartCode(partOld);
                                    if (idPart != -1)
                                    {
                                        InventoryDAO.Instance.InsertInput(partOld,
                                            item.QuantityOr, DateOut, EmployessCode, "-", "Hàng cũ", 1, 1);
                                    }
                                    else
                                    {
                                        PartDAO.Instance.InsertPart(partOld, item.PartName, unit, 0, 0,0);
                                        InventoryDAO.Instance.InsertInput(partOld,
                                            item.QuantityOr, DateOut, EmployessCode, "-", "Hàng cũ", 1, 1);
                                    }
                                }
                                else
                                {
                                    kun = 1;
                                }
                            }
                            else if(kun == 2)
                            {
                                partOld = partCode + "-OLD";
                                long idPart = PartDAO.Instance.IdPartCode(partOld);
                                if (idPart != -1)
                                {
                                    InventoryDAO.Instance.InsertInput(partOld,
                                        item.QuantityOr, DateOut, EmployessCode, "-", "Hàng cũ", 1, 1);
                                }
                                else
                                {
                                    PartDAO.Instance.InsertPart(partOld, item.PartName, unit, 0, 0,0);
                                    InventoryDAO.Instance.InsertInput(partOld,
                                        item.QuantityOr, DateOut, EmployessCode, "-", "Hàng cũ", 1, 1);
                                }
                            }
                        }
                    }
                    if (kun == 1)
                    {
                        Kun_Static.MaxIdOrder = idOrder;
                        using (frmReasonError f = new frmReasonError())
            {
                            f.ShowDialog();
            }
                    }
                    string noteOrder = "Đã xuất kho -- "+ Kun_Static.NoteOut ;
                    MessageBox.Show("bạn xuất thành công !".ToUpper());
                    OrderDAO.Instance.UpdateOrder(idOrder,4, noteOrder.ToUpper());
                    Kun_Static.CheckAction = 1;
                    this.Close();
                }
                #region Xuất kho
                #endregion
            }
            catch
            {
                XtraMessageBox.Show("Mã vạch không đúng!".ToUpper(),
                    "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        /// <summary>
        /// Phát sự kiện để màn hình gọi nạp lại dữ liệu khi đóng form.
        /// </summary>
        private void frmOutput_FormClosing(object sender, FormClosingEventArgs e)
        {
            eventHandler?.Invoke(sender, e);
        }
    }
}