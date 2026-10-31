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

namespace Tooling_WH_Sytems.User
{
    /// <summary>
    /// Quản lý danh mục nhóm tài khoản.
    /// </summary>
    public partial class frmUserGroup : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmUserGroup()
        {
            InitializeComponent();
            LoadControl();
        }
        #region Control
        /// <summary>
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void LoadControl()
        {
            LoadData();
            LockControl();
        }
        private bool IsInsert = false;
        /// <summary>
        /// Nạp dữ liệu nghiệp vụ vào bảng hiển thị.
        /// </summary>
        private void LoadData()
        {
            GCData.DataSource = UsersDAO.Instance.GetUserGroup();
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

            txtNameGr.Enabled = false;
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

            txtNameGr.Enabled = true;
        }
        /// <summary>
        /// Xóa nội dung các ô nhập để chuẩn bị bản ghi mới.
        /// </summary>
        private void ClearText()
        {
            txtNameGr.Text = String.Empty;
            lblId.Text = String.Empty;
        }
        /// <summary>
        /// Đưa dữ liệu của dòng đang chọn lên các ô thông tin.
        /// </summary>
        private void AddText()
        {
            try
            {
                txtNameGr.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["NameUserGroup"]).ToString();
                lblId.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["Id"]).ToString();
            }
            catch (Exception ex)
            {
                AppLog.Write("frmUserGroup", ex);
            }
        }
        #endregion
        #region Event
        /// <summary>
        /// Chuẩn bị dữ liệu hoặc mở màn hình để thêm bản ghi mới.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearText();
            OpenControl();
            IsInsert = true;
        }

        /// <summary>
        /// Xóa dữ liệu đang chọn theo các điều kiện và xác nhận hiện tại.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
           if(MessageBox.Show("bạn muốn xóa thông tin này?".ToUpper(),"Thông Báo",MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                foreach (var item in gridView1.GetSelectedRows())
                {
                    int id = int.Parse(gridView1.GetRowCellValue(item, "Id").ToString());
                    UsersDAO.Instance.DeleteUserGr(id);
                }
                LoadControl();
            }
        }

        /// <summary>
        /// Bắt đầu chỉnh sửa dữ liệu đang chọn theo quyền hiện tại.
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            OpenControl();
            IsInsert = false;
        }

        /// <summary>
        /// Nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            LoadControl();
        }
        /// <summary>
        /// Kiểm tra tên nhóm rồi thêm mới hoặc cập nhật nhóm tài khoản.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtNameGr.Text;
            int test = UsersDAO.Instance.TestUserGr(name);
            if (IsInsert == true)
            {
                if(test == 1)
                {
                    MessageBox.Show("Group đã tồn tại !".ToUpper(), "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                UsersDAO.Instance.InsertUserGr(name);
                MessageBox.Show("thêm thông tin thành công !".ToUpper());
                LoadControl();
            }
            else
            {
                int id = int.Parse(lblId.Text);
                UsersDAO.Instance.UpdateUserGr(id,name);
                MessageBox.Show("sửa thông tin thành công !".ToUpper());
                LoadControl();
            }
        }
        /// <summary>
        /// Đồng bộ thông tin của dòng đang chọn với vùng nhập liệu hoặc trạng thái thao tác.
        /// </summary>
        private void GCData_Click(object sender, EventArgs e)
        {
            AddText();
        }
        #endregion


    }
}