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
using DevExpress.XtraGrid.Views.Grid;
using DAO;
using DTO;

namespace Tooling_WH_Sytems.Output
{
    /// <summary>
    /// Ghi nhận lý do đổi và nhập lại linh kiện cũ.
    /// </summary>
    public partial class frmReasonError : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmReasonError()
        {
            InitializeComponent();
            LoadControl();
            gridView1.CustomDrawRowIndicator += gridView1_CustomDrawRowIndicator;

        }
   
       
        private long maxId = Kun_Static.MaxIdOrder;
        #region Control
        /// <summary>
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void LoadControl()
        {
            LoadData();
        }

        /// <summary>
        /// Tăng độ rộng cột số thứ tự khi nội dung hiển thị dài hơn.
        /// </summary>
        private bool cal(Int32 _Width, GridView _View)
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
        private void LoadData()
        {
            GCData.DataSource = OrderDetailDAO.Instance.GetListOrderDetailById(maxId).Where(x=>x.PurPose == "Đổi");
        }

        #endregion
        #region Event
        /// <summary>
        /// Ghi nhận lý do đổi, cập nhật ghi chú và nhập lại linh kiện cũ theo lựa chọn.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            DateTime DateOut = DateTime.Now;
            if (XtraMessageBox.Show("bạn hãy thêm số lượng thực tế đổi tra!".ToUpper(),"Thông Báo",MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    long id =long.Parse(gridView1.GetRowCellValue(i, "IdOrd").ToString());
                    string partCode = gridView1.GetRowCellValue(i, "PartCode").ToString();
                    string partName = gridView1.GetRowCellValue(i, "PartName").ToString();
                    string unit = PartDAO.Instance.UnitPart(partCode);
                    float quantity = (float)Convert.ToDouble(gridView1.GetRowCellValue(i, "QuantityOr").ToString());
                    float quantityTT = 0;
                    try
                    {
                        quantityTT = (float)Convert.ToDouble(gridView1.GetRowCellValue(i, "Note").ToString());
                    }
                    catch (Exception ex)
            {
                AppLog.Write("frmReasonError", ex);
            }
                    string partOld = partCode + "-OLD";
                    long idPart = PartDAO.Instance.IdPartCode(partOld);
                    if(quantity - quantityTT >0)
                    {
                        Kun_Static.NoteOut = "Số lượng đổi trả thiếu" .ToUpper();
                        string msg = "Số lượng đổi trả thiếu : " + (quantity - quantityTT).ToString();
                        OrderDetailDAO.Instance.UpdateNoteOrderDetail(id, msg.ToUpper());
                    }
                    else
                    {
                        Kun_Static.NoteOut = "";
                    }
                    if (idPart != -1)
                    {
                        InventoryDAO.Instance.InsertInput(partOld, quantityTT, DateOut, Kun_Static.EmployessCode, "-", "Hàng cũ", 1, 1);
                    }
                    else
                    {
                        PartDAO.Instance.InsertPart(partOld, partName, unit, 0, 0,0);
                        InventoryDAO.Instance.InsertInput(partOld, quantityTT, DateOut, Kun_Static.EmployessCode, "-", "Hàng cũ", 1, 1);
                    }
                }
                this.Close();
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

        #endregion

        /// <summary>
        /// Điểm mở rộng hiện chưa có xử lý; giữ nguyên hành vi hiện tại.
        /// </summary>
        private void frmReasonError_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }
    }
}