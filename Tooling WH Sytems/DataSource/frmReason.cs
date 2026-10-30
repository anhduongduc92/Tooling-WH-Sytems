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
        bool Isinsert = false;
        /// <summary>
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        void LoadControl()
        {
            LoadData();
            LockControl();
        }
        /// <summary>
        /// Nạp dữ liệu nghiệp vụ vào bảng hiển thị.
        /// </summary>
        void LoadData()
        {
            GCData.DataSource = DataSorceDAO.Instance.GetListReason();
        }
        /// <summary>
        /// Khóa vùng nhập liệu và đưa các nút về chế độ xem.
        /// </summary>
        void LockControl()
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
        void OpenControl()
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
        void ClearText()
        {
            txtName.Text = String.Empty;
            lblId.Text = String.Empty;
        }
        /// <summary>
        /// Đưa dữ liệu của dòng đang chọn lên các ô thông tin.
        /// </summary>
        void AddText()
        {
            try
            {
                txtName.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["RName"]).ToString();
                lblId.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Id"]).ToString();
            }
            catch
            {
            }
        }
        /// <summary>
        /// Chuẩn bị dữ liệu hoặc mở màn hình để thêm bản ghi mới.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (Kun_Static.UserRight < 2)
            {
                XtraMessageBox.Show("bạn không có quyền !".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            OpenControl();
            Isinsert = true;
            ClearText();
        }

        /// <summary>
        /// Xóa dữ liệu đang chọn theo các điều kiện và xác nhận hiện tại.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Kiểm tra mức quyền tối thiểu trước khi thay đổi dữ liệu.
            if (Kun_Static.UserRight < 3)
            {
                XtraMessageBox.Show("bạn không có quyền !".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (Kun_Static.UserRight < 2)
            {
                XtraMessageBox.Show("bạn không có quyền !".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            OpenControl();
            Isinsert = false;

        }

        /// <summary>
        /// Kiểm tra tên rồi thêm mới hoặc cập nhật lý do sử dụng.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            int id = DataSorceDAO.Instance.IdReason(name);
            if (Isinsert == true)
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