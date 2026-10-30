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
using System.IO;
using ExcelDataReader;
using DAO;

namespace Tooling_WH_Sytems.Input
{
    /// <summary>
    /// Nhập kho hàng loạt từ dữ liệu Excel.
    /// </summary>
    public partial class frmInputExcel : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện cùng các giá trị đầu vào của màn hình.
        /// </summary>
        public frmInputExcel()
        {
            InitializeComponent();
        }
        List<string> eRror = new List<string>();
        DataSet ds;
        int dk = 0;
        public EventHandler eventHandler;
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
        /// Kiểm tra từng dòng Excel và ghi nhận các dòng nhập kho hợp lệ.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dk == 0)
            {
                XtraMessageBox.Show("bạn chưa chọn sheet cần thêm!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string employess = Kun_Static.EmployessCode;
            DateTime dateIn = DateTime.Now;
            lblError.Text = "";
            eRror = new List<string>();
            int i = 0;
            int slLoi = 0;
            string partCode = "";
            string partName = "";
            float quantity = 0;
            string supplier = "";

            foreach (DataGridViewRow row in GCData.Rows)
            {
                i++;
                try
                {
                    partCode = row.Cells["Part Code"].Value.ToString();
                    partName = row.Cells["Part Name"].Value.ToString();
                    supplier = row.Cells["Supplier Code"].Value.ToString();
                    try
                    {
                        quantity = (float)Convert.ToDouble(row.Cells["Quantity"].Value.ToString());
                    }
                    catch
                    {

                    }
                    int testSup = SupplierDAO.Instance.IdSupplier(supplier);
                    if (partCode == "")
                    {
                        eRror.Add("Dòng " + i + ": Mã linh kiện Trống".ToUpper());
                        slLoi++;
                    }
                    else if (quantity <= 0)
                    {
                        eRror.Add("Dòng " + i + ": bạn chưa điền số lượng nhập".ToUpper());
                        slLoi++;
                    }
                    else if (testSup == -1)
                    {
                        eRror.Add("Dòng " + i + ": mã nhà cung cấp không chính xác".ToUpper());
                        slLoi++;
                    }
                    else
                    {
                        long test = PartDAO.Instance.IdPartCode(partCode);
                        if (test != -1)
                        {
                            InventoryDAO.Instance.InsertInput(partCode, quantity, dateIn, employess, supplier, "", 1, 1);
                        }
                        else
                        {
                            eRror.Add("Dòng " + i + ": Mã linh kiện không tồn tại".ToUpper());
                            slLoi++;
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
                XtraMessageBox.Show("Nhập Dữ Liệu Xong".ToUpper());
                Kun_Static.CheckAction = 1;
                this.Close();
            }
            else
            {
                XtraMessageBox.Show("Có lỗi khi nhập!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void frmInputExcel_FormClosing(object sender, FormClosingEventArgs e)
        {
            eventHandler?.Invoke(sender, e);
        }
    }
}