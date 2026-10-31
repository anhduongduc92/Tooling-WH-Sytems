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
            Disposed += DisposeWorkbook;
        }
        private List<string> _errors = new List<string>();
        private DataSet _workbook;
        private bool _hasSelectedSheet;
        public EventHandler eventHandler;
        /// <summary>
        /// Chọn tệp Excel, đọc các sheet và nạp danh sách tên sheet.
        /// </summary>
        private async Task ReadDataAsync()
        {
            using (OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Excel Workbook|*.xlsx|Excel Workbook 97-2003|*.xls",
                ValidateNames = true
            })
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                string fileName = dialog.FileName;
                bool isBinary = dialog.FilterIndex == 2;
                using (new BusyFormScope(this))
                {
                    DataSet workbook = await Task.Run(() => ExcelWorkbookReader.Read(fileName, isBinary));
                    if (IsDisposed)
                    {
                        workbook.Dispose();
                        return;
                    }

                    // Chỉ thay dữ liệu hiện tại sau khi tệp mới đã đọc thành công.
                    DataSet previousWorkbook = _workbook;
                    _hasSelectedSheet = false;
                    GCData.DataSource = null;
                    cbSheet.Items.Clear();
                    _workbook = workbook;
                    if (previousWorkbook != null) previousWorkbook.Dispose();
                    lblLink.Text = fileName;
                    foreach (DataTable sheet in workbook.Tables)
                    {
                        cbSheet.Items.Add(sheet.TableName);
                    }
                }
            }
        }
        /// <summary>
        /// Chọn và đọc tệp Excel để chuẩn bị nhập dữ liệu.
        /// </summary>
        private async void btnOpen_Click(object sender, EventArgs e)
        {
            if (_isReadingWorkbook) return;
            _isReadingWorkbook = true;
            try
            {
                await ReadDataAsync();
            }
            catch (Exception ex)
            {
                UiActions.ShowError("Không thể đọc tệp Excel", ex);
            }
            finally
            {
                _isReadingWorkbook = false;
            }
        }
        /// <summary>
        /// Kiểm tra từng dòng Excel và ghi nhận các dòng nhập kho hợp lệ.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_hasSelectedSheet)
            {
                XtraMessageBox.Show("bạn chưa chọn sheet cần thêm!".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string employess = Kun_Static.EmployessCode;
            DateTime dateIn = DateTime.Now;
            lblError.Text = "";
            _errors = new List<string>();
            int i = 0;
            int slLoi = 0;
            string partCode = "";
            string partName = "";
            float quantity = 0;
            string supplier = "";

            foreach (DataGridViewRow row in GCData.Rows)
            {
                if (row.IsNewRow) continue;
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
                    catch (Exception ex)
                    {
                        AppLog.Write("Đọc số lượng nhập Excel, dòng " + i, ex);
                        quantity = 0;
                    }
                    int testSup = SupplierDAO.Instance.IdSupplier(supplier);
                    if (partCode == "")
                    {
                        _errors.Add("Dòng " + i + ": Mã linh kiện Trống".ToUpper());
                        slLoi++;
                    }
                    else if (quantity <= 0)
                    {
                        _errors.Add("Dòng " + i + ": bạn chưa điền số lượng nhập".ToUpper());
                        slLoi++;
                    }
                    else if (testSup == -1)
                    {
                        _errors.Add("Dòng " + i + ": mã nhà cung cấp không chính xác".ToUpper());
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
                            _errors.Add("Dòng " + i + ": Mã linh kiện không tồn tại".ToUpper());
                            slLoi++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppLog.Write("frmInputExcel: nhập dòng " + i, ex);
                    _errors.Add("Dòng " + i + ": Dữ liệu không hợp lệ hoặc không thể ghi dữ liệu.");
                    slLoi++;
                }
                lblError.Text = "Lỗi: " + (slLoi) + " Lỗi";
            }

            if (_errors.Count == 0)
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
            using (frmErrorList f = new frmErrorList(_errors))
            {
                f.ShowDialog();
            }
        }
        /// <summary>
        /// Hiển thị dữ liệu của sheet Excel được chọn và đánh dấu đã chọn sheet.
        /// </summary>
        private void cbSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cbSheet.SelectedIndex;
            if (_workbook == null || index < 0 || index >= _workbook.Tables.Count) return;
            GCData.DataSource = _workbook.Tables[index];
            _hasSelectedSheet = true;
        }

        /// <summary>
        /// Phát sự kiện để màn hình gọi nạp lại dữ liệu khi đóng form.
        /// </summary>
        private void frmInputExcel_FormClosing(object sender, FormClosingEventArgs e)
        {
            eventHandler?.Invoke(sender, e);
        }
        private bool _isReadingWorkbook;

        /// <summary>Giải phóng workbook khi form không còn sử dụng dữ liệu.</summary>
        private void DisposeWorkbook(object sender, EventArgs e)
        {
            if (_workbook != null)
            {
                _workbook.Dispose();
                _workbook = null;
            }
        }

    }
}