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
using DAO;

namespace Tooling_WH_Sytems.DataSource
{
    /// <summary>
    /// Quản lý danh mục lý do sử dụng linh kiện.
    /// </summary>
    public partial class frmReason : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmReason()
        {
            InitializeComponent();
            LoadControl();
        }
        private bool _isInsert = false;
        /// <summary>
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void LoadControl()
        {
            LoadData();
            LockControl();
        }
        /// <summary>
        /// Nạp dữ liệu nghiệp vụ vào bảng hiển thị.
        /// </summary>
        private void LoadData()
        {
            GCData.DataSource = DataSorceDAO.Instance.GetListReason();
        }
        /// <summary>
        /// Khóa vùng nhập liệu và đưa các nút về chế độ xem.
        /// </summary>
        private void LockControl()
        {
            btnAdd.Enabled = true;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnSave.Enabled = false;

            txtName.Enabled = false;
        }
        /// <summary>
        /// Mở vùng nhập liệu cho thao tác thêm hoặc chỉnh sửa.
        /// </summary>
        private void OpenControl()
        {
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = true;

            txtName.Enabled = true;
        }
        /// <summary>
        /// Xóa nội dung các ô nhập để chuẩn bị bản ghi mới.
        /// </summary>
        private void ClearText()
        {
            txtName.Text = String.Empty;
            lblId.Text = String.Empty;
        }
        /// <summary>
        /// Đưa dữ liệu của dòng đang chọn lên các ô thông tin.
        /// </summary>
        private void AddText()
        {
            try
            {
                txtName.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["RName"]).ToString();
                lblId.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Id"]).ToString();
            }
            catch (Exception ex)
            {
                AppLog.Write("frmReason", ex);
            }
        }
        /// <summary>
        /// Chuẩn bị dữ liệu hoặc mở màn hình để thêm bản ghi mới.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (!UiActions.HasPermission(2))
            {
                return;
            }
            OpenControl();
            _isInsert = true;
            ClearText();
        }

        /// <summary>
        /// Xóa dữ liệu đang chọn theo các điều kiện và xác nhận hiện tại.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (!UiActions.HasPermission(3))
            {
                return;
            }
            if (XtraMessageBox.Show("bạn muốn xóa thông tin này?".ToUpper(), "Thông Báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                foreach (var item in gridView1.GetSelectedRows())
                {
                    int id = int.Parse(gridView1.GetRowCellValue(item, "Id").ToString());
                    DataSorceDAO.Instance.DeleteReason(id);
                }
                LoadControl();
            }
        }

        /// <summary>
        /// Bắt đầu chỉnh sửa dữ liệu đang chọn theo quyền hiện tại.
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (!UiActions.HasPermission(2))
            {
                return;
            }
            OpenControl();
            _isInsert = false;

        }

        /// <summary>
        /// Kiểm tra tên rồi thêm mới hoặc cập nhật lý do sử dụng.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            int id = DataSorceDAO.Instance.IdReason(name);
            if (_isInsert == true)
            {
                if (id == -1)
                {
                    DataSorceDAO.Instance.InsertReason(name);
                    LoadControl();
                    XtraMessageBox.Show("thêm thông tin thành công !".ToUpper());
                }
            }
            else
            {
                id = int.Parse(lblId.Text);
                DataSorceDAO.Instance.UpdateReason(id, name);
                LoadControl();
                XtraMessageBox.Show("sửa thông tin thành công !".ToUpper());
            }
        }

        /// <summary>
        /// Nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            LoadControl();
        }

        /// <summary>
        /// Đồng bộ thông tin của dòng đang chọn với vùng nhập liệu hoặc trạng thái thao tác.
        /// </summary>
        private void GCData_Click(object sender, EventArgs e)
        {
            AddText();
        }
    }
}