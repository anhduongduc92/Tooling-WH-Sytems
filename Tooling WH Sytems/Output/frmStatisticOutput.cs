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
using DevExpress.XtraGrid.Views.Grid;
using System.IO;

namespace Tooling_WH_Sytems.Output
{
    /// <summary>
    /// Tra cứu và xuất thống kê xuất kho theo thời gian.
    /// </summary>
    public partial class frmStatisticOutput : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmStatisticOutput()
        {
            InitializeComponent();
            LoadControl();
            gridView1.CustomDrawRowIndicator += gridView1_CustomDrawRowIndicator;
            LoadColum();
        }
        /// <summary>
        /// Thiết lập cách hiển thị cột dữ liệu.
        /// </summary>
        void LoadColum()
        {
            gridView1.Columns["QuantityOut"].Summary.Add(DevExpress.Data.SummaryItemType.Sum, "QuantityOut", "Tổng = {0}");
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
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        void LoadControl()
        {
            LoadDatetime();
        }
        /// <summary>
        /// Thiết lập khoảng thời gian mặc định rồi nạp dữ liệu thống kê.
        /// </summary>
        void LoadDatetime()
        {
            DateTime today = DateTime.Now.Date;
            dtpkFrom.Value = today.AddDays(-(today.Day - 1));
            dtpkTo.Value = dtpkFrom.Value.AddMonths(1).AddSeconds(-10);
            LoadData();
        }
        /// <summary>
        /// Nạp dữ liệu nghiệp vụ vào bảng hiển thị.
        /// </summary>
        void LoadData()
        {
            DateTime date1 = dtpkFrom.Value;
            DateTime date2 = dtpkTo.Value;
            GCData.DataSource = InventoryDAO.Instance.StatisticOutput(date1, date2);
        }

        /// <summary>
        /// Nạp hoặc mở dữ liệu chi tiết theo lựa chọn hiện tại.
        /// </summary>
        private void btnView_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// Nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            LoadControl();
        }

        /// <summary>
        /// Chọn đường dẫn và xuất dữ liệu thống kê ra tệp theo định dạng được chọn.
        /// </summary>
        private void btnExcel_Click(object sender, EventArgs e)
        {
            #region Xuất Excel
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel (2010) (.xlsx)|*.xlsx |RichText File (.rtf)|*.rtf |Pdf File (.pdf)|*.pdf |Html File (.html)|*.html";
                if (saveDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string exportFilePath = saveDialog.FileName;
                    string fileExtenstion = new FileInfo(exportFilePath).Extension;

                    switch (fileExtenstion)
                    {
                        case ".xls":
                            GCData.ExportToXls(exportFilePath);
                            break;
                        case ".xlsx":
                            GCData.ExportToXlsx(exportFilePath);
                            break;
                        case ".rtf":
                            GCData.ExportToRtf(exportFilePath);
                            break;
                        case ".pdf":
                            GCData.ExportToPdf(exportFilePath);
                            break;
                        case ".html":
                            GCData.ExportToHtml(exportFilePath);
                            break;
                        case ".mht":
                            GCData.ExportToMht(exportFilePath);
                            break;
                        default:
                            break;
                    }

                    if (File.Exists(exportFilePath))
                    {
                        try
                        {
                            //Try to open the file and let windows decide how to open it.
                            System.Diagnostics.Process.Start(exportFilePath);
                        }
                        catch
                        {
                            String msg = "The file could not be opened." + Environment.NewLine + Environment.NewLine + "Path: " + exportFilePath;
                            MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        String msg = "The file could not be saved." + Environment.NewLine + Environment.NewLine + "Path: " + exportFilePath;
                        MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            #endregion
        }
    }
}