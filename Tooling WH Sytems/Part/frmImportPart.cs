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
using ExcelDataReader;
using System.IO;
using DAO;

namespace Tooling_WH_Sytems.Part
{
    /// <summary>
    /// Đọc và nhập danh mục linh kiện từ Excel.
    /// </summary>
    public partial class frmImportPart : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện cùng các giá trị đầu vào của màn hình.
        /// </summary>
        public frmImportPart()
        {
            InitializeComponent();
        }
        List<string> eRror = new List<string>();
        DataSet ds;
        int dk = 0;
        public EventHandler LamMoi;
        /// <summary>
        /// Chọn tệp Excel, đọc các sheet và nạp danh sách tên sheet.
        /// </summary>
        void ReadData()
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel Workbook|*.xlsx|Excel Workbook 97-2003|*.xls", ValidateNames = true })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    lblLink.Text = ofd.FileName;
                    using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
                    {
                        IExcelDataReader reader;
                        if (ofd.FilterIndex == 2)
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }

                        ds = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });

                        cbSheet.Items.Clear();
                        foreach (DataTable dt in ds.Tables)
                        {
                            cbSheet.Items.Add(dt.TableName);
                        }
                        reader.Close();

                    }
                }
            }
        }
        /// <summary>
        /// Chọn và đọc tệp Excel để chuẩn bị nhập dữ liệu.
        /// </summary>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            ReadData();
        }
        /// <summary>
        /// Kiểm tra mã linh kiện từng dòng và nhập danh mục từ sheet đang chọn.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if(dk == 0)
            {
                XtraMessageBox.Show("bạn chưa chọn sheet cần thêm!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblError.Text = "";
            eRror = new List<string>();
            int i = 0;
            int slLoi = 0;
            string partCode = "";
            string partName = "";
            string unit = "";
            float minWaint = 0;
            float maxWaint = 0;
            
            foreach (DataGridViewRow row in GCData.Rows)
            {
                i++;
                try
                {
                    partCode = row.Cells["PartCode"].Value.ToString();
                    partName = row.Cells["PartName"].Value.ToString();
                    unit = row.Cells["Unit"].Value.ToString();
                    minWaint = (float)Convert.ToDouble(row.Cells["MinWaint"].Value.ToString());
                    maxWaint = (float)Convert.ToDouble(row.Cells["MaxWaint"].Value.ToString());
                 
                    if (partCode == "")
                    {
                        eRror.Add("Dòng " + i + ": Mã Linh Kiện Trống".ToUpper());
                        slLoi++;
                    }
                    else
                    {
                        long test = PartDAO.Instance.IdPartCode(partCode);
                        if (test != -1)
                        {
                            eRror.Add("Dòng " + i + ": Mã Linh Kiện đã tồn tại".ToUpper());
                            slLoi++;
                        }
                        else
                        {
                            PartDAO.Instance.InsertPart(partCode, partName, unit, minWaint, maxWaint,0);
                        }
                    }
                }
                catch
                {

                }
                lblError.Text = "Lỗi: " + (slLoi) + " Lỗi";
            }
            if (eRror.Count == 0)
            {
                MessageBox.Show("Nhập Dữ Liệu Xong".ToUpper());
                this.Close();
            }
            else
            {
                MessageBox.Show("Có lỗi khi nhập!".ToUpper(),"Thông Báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Mở danh sách lỗi phát sinh khi nhập dữ liệu từ Excel.
        /// </summary>
        private void btnErrorList_Click(object sender, EventArgs e)
        {
            frmErrorList f = new frmErrorList(eRror);
            f.ShowDialog();
        }
        /// <summary>
        /// Hiển thị dữ liệu của sheet Excel được chọn và đánh dấu đã chọn sheet.
        /// </summary>
        private void cbSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            GCData.DataSource = ds.Tables[cbSheet.SelectedIndex];
            dk = 1;
        }
        /// <summary>
        /// Phát sự kiện để màn hình gọi nạp lại dữ liệu khi đóng form.
        /// </summary>
        private void frmImportPart_FormClosing(object sender, FormClosingEventArgs e)
        {
            LamMoi?.Invoke(sender, e);
        }
    }
}