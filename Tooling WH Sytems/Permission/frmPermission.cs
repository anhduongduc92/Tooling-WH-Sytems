using Tooling_WH_Sytems.Infrastructure;
using System;
using System.Collections.Generic;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DAO;
using DTO;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;


namespace Tooling_WH_Sytems.Permission
{
    /// <summary>
    /// Thiết lập quyền sử dụng chức năng cho từng tài khoản.
    /// </summary>
    public partial class frmPermission : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Khởi tạo giao diện và nạp dữ liệu ban đầu.
        /// </summary>
        public frmPermission()
        {
            InitializeComponent();
            LoadControl();
        }
        private int _IdUSer;
        /// <summary>
        /// Hiển thị mức quyền của chức năng đang chọn.
        /// </summary>
        private void LoadUserRight()
        {
            int userRight = Kun_Static.UserRight;
            switch(userRight)
            {
                case 1:
                    {
                        mnEnableFalse.Visible = false;
                        mnEdit.Visible = false;
                        btnConfirm.Visible = false;
                        mnFull.Visible = false;
                    }
                    break;
                case 2:
                    {
                        btnConfirm.Visible = false;
                        mnFull.Visible = false;
                    }
                    break;
                case 3:
                    {
                       
                        mnFull.Visible = false;
                    }
                    break;
                case 0:
                    {
                        mnEnableFalse.Visible = false;
                        mnEdit.Visible = false;
                        btnConfirm.Visible = false;
                        mnFull.Visible = false;
                    }
                    break;
                default:
                    {
                        mnEnableFalse.Visible = true;
                        mnOnlyView.Visible = true;
                        mnEdit.Visible = true;
                        btnConfirm.Visible = true;
                        mnFull.Visible = true;
                    }
                    break;
            }
        }
        /// <summary>
        /// Khởi tạo các giá trị dùng trong màn hình phân quyền.
        /// </summary>
        private void LoadValue()
        {
            for (int i = 0; i < treeList1.Nodes.Count; i++)
            {
                string userRight = (treeList1.Nodes[i].GetValue("User_Right")).ToString();
                if(userRight != "Cấm quyền")
                {
                    treeList1.Nodes[i].Checked = true;
                }
                for (int j = 0; j < treeList1.Nodes[i].Nodes.Count; j++)
                {
                    string userRight1 = (treeList1.Nodes[i].Nodes[j].GetValue("User_Right")).ToString();
                    if(userRight1 != "Cấm quyền")
                    {
                        treeList1.Nodes[i].Nodes[j].Checked = true;
                    }
                }
            }
        }
        /// <summary>
        /// Khởi tạo hoặc nạp lại dữ liệu và trạng thái điều khiển của màn hình.
        /// </summary>
        private void LoadControl()
        {
            string enCode = Kun_Static.UserStatic.EmCode;
            List<UsersDTO> listU = UsersDAO.Instance.GetListUser();
            if(enCode.ToUpper() == "ADMINISTRATOR")
            {
                GCData.DataSource = listU;
            }
            else
            {
                string nameGr = EmployessDAO.Instance.NameGr(enCode);
                GCData.DataSource = UsersDAO.Instance.GetUserPermission(nameGr);
            }
        }
        /// <summary>
        /// Nạp cây chức năng và quyền của tài khoản được chọn.
        /// </summary>
        private void LoadPermission(int idUser)
        {
            treeList1.OptionsView.CheckBoxStyle = DefaultNodeCheckBoxStyle.Check;
            var tuple = TablePermission.Instance.GetAllTableMenu(frmMain.Instance.Ribbon, idUser);
            treeList1.StateImageList = tuple.Item2;
            treeList1.DataSource = tuple.Item1;
            treeList1.ExpandAll();
        }
        /// <summary>
        /// Gán quyền không sử dụng chức năng (mức 0).
        /// </summary>
        private void mnEnableFalse_Click(object sender, EventArgs e)
        {
            int idUser = _IdUSer;
            List<TreeListNode> list = treeList1.GetAllCheckedNodes();
            foreach (TreeListNode node in list)
            {
                string Fnc = node["id"].ToString();
                UserRightDAO.Instance.UpdateUserRight(idUser, Fnc, 0);
            }
            LoadPermission(idUser);
        }

        /// <summary>
        /// Gán quyền chỉ xem chức năng (mức 1).
        /// </summary>
        private void mnOnlyView_Click(object sender, EventArgs e)
        {
            int idUser = _IdUSer;
            List<TreeListNode> list = treeList1.GetAllCheckedNodes();
            foreach (TreeListNode node in list)
            {
                string Fnc = node["id"].ToString();

                UserRightDAO.Instance.UpdateUserRight(idUser, Fnc, 1);
            }
            LoadPermission(idUser);
        }

        /// <summary>
        /// Gán quyền chỉnh sửa chức năng (mức 2).
        /// </summary>
        private void mnEdit_Click(object sender, EventArgs e)
        {
            int idUser = _IdUSer;
            List<TreeListNode> list = treeList1.GetAllCheckedNodes();
            foreach (TreeListNode node in list)
            {
                string Fnc = node["id"].ToString();

                UserRightDAO.Instance.UpdateUserRight(idUser, Fnc, 2);
            }
            LoadPermission(idUser);
        }
        /// <summary>
        /// Gán quyền xác nhận chức năng (mức 3).
        /// </summary>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            int idUser = _IdUSer;
            List<TreeListNode> list = treeList1.GetAllCheckedNodes();
            foreach (TreeListNode node in list)
            {
                string Fnc = node["id"].ToString();

                UserRightDAO.Instance.UpdateUserRight(idUser, Fnc, 3);
            }
            LoadPermission(idUser);
        }
        /// <summary>
        /// Gán toàn quyền chức năng (mức 4).
        /// </summary>
        private void mnFull_Click(object sender, EventArgs e)
        {
            int idUser = _IdUSer;
            List<TreeListNode> list = treeList1.GetAllCheckedNodes();
            foreach (TreeListNode node in list)
            {
                string Fnc = node["id"].ToString();

                UserRightDAO.Instance.UpdateUserRight(idUser, Fnc, 4);
            }
            LoadPermission(idUser);
        }

        /// <summary>
        /// Cập nhật chức năng được chọn và trạng thái các lệnh phân quyền.
        /// </summary>
        private void treeList1_AfterFocusNode(object sender, NodeEventArgs e)
        {
            try
            {
                int userRight = int.Parse(e.Node.GetValue("User_Right").ToString());
                if(userRight>=0)
                {
                    e.Node.Checked = true;
                }
            }
            catch (Exception ex)
            {
                AppLog.Write("frmPermission", ex);
            }
        }

        /// <summary>
        /// Chọn biểu tượng hiển thị cho nút chức năng trên cây quyền.
        /// </summary>
        private void treeList1_GetStateImage(object sender, DevExpress.XtraTreeList.GetStateImageEventArgs e)
        {
            try
            {
                int index = int.Parse(e.Node.GetValue("image_index").ToString());
                if (index >= 0)
                {
                    e.NodeImageIndex = index;
                }
            }
            catch (Exception ex)
            {
                AppLog.Write("frmPermission", ex);
            }
        }

        private bool lockEvent = false;
        /// <summary>
        /// Đồng bộ tài khoản và quyền hiển thị khi thay đổi lựa chọn.
        /// </summary>
        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if (lockEvent) return;
            GridView view = sender as GridView;
            if (view.SelectedRowsCount == 2 && e.Action == System.ComponentModel.CollectionChangeAction.Add)
            {
                int[] selectedRows = view.GetSelectedRows();
                foreach (int rowHandle in selectedRows)
                {
                    if (rowHandle != e.ControllerRow)
                    {
                        lockEvent = true;
                        view.UnselectRow(rowHandle);
                        lockEvent = false;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Điểm mở rộng hiện chưa có xử lý; giữ nguyên hành vi hiện tại.
        /// </summary>
        private void GCData_DoubleClick(object sender, EventArgs e)
        {
          
        }

        /// <summary>
        /// Đồng bộ thông tin của dòng đang chọn với vùng nhập liệu hoặc trạng thái thao tác.
        /// </summary>
        private void GCData_Click(object sender, EventArgs e)
        {
            addText();
        }

        /// <summary>
        /// Nạp thông tin và quyền của tài khoản đang chọn.
        /// </summary>
        private void addText()
        {
            int idUser = 0;
            int a = gridView1.SelectedRowsCount;
            try
            {
                _IdUSer = int.Parse(gridView1.GetRowCellValue(
                    gridView1.FocusedRowHandle, gridView1.Columns["IdUser"]).ToString());
            }
            catch (Exception ex)
            {
                AppLog.Write("frmPermission", ex);
            }
            idUser = _IdUSer;
            if (idUser == 0)
            {
                MessageBox.Show("Lỗi");
                return;
            }
            else if (a > 1)
            {
                MessageBox.Show("bạn chỉ được chọn 1 tài khoản".ToUpper());
                return;
            }
            LoadPermission(idUser);
            LoadUserRight();
        }
    }
}