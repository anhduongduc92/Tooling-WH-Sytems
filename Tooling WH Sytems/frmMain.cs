using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using Tooling_WH_Sytems.Order;
using Tooling_WH_Sytems.Permission;
using Tooling_WH_Sytems.User;
using Tooling_WH_Sytems.Part;
using Tooling_WH_Sytems.Supplier;
using Tooling_WH_Sytems.Input;
using Tooling_WH_Sytems.Output;
using DTO;
using Tooling_WH_Sytems.Employess;
using Tooling_WH_Sytems.DataSource;
using System.Collections;
using DevExpress.XtraBars.Ribbon;
using DAO;

namespace Tooling_WH_Sytems
{

    /// <summary>
    /// Điều hướng các màn hình nghiệp vụ theo quyền tài khoản.
    /// </summary>
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private static frmMain _defaultInstance;
        public static frmMain Instance
        {
            get
            {
                if (_defaultInstance == null)
                {
                    _defaultInstance = new frmMain();
                }
                return _defaultInstance;
            }
            set => _defaultInstance = value;
        }
        /// <summary>
        /// Khởi tạo giao diện cùng các giá trị đầu vào của màn hình.
        /// </summary>
        public frmMain()
        {
            InitializeComponent();
            LoadParent();
            TextHeader();
        }
        /// <summary>
        /// Bật hoặc tắt chức năng trên Ribbon theo quyền của tài khoản.
        /// </summary>
        private void LoadParent()
        {
            ArrayList visiblePages = this.Ribbon.TotalPageCategory.GetVisiblePages();
            foreach (RibbonPage page in visiblePages)
            {
                foreach (RibbonPageGroup group in page.Groups)
                {
                    foreach (BarItemLink item in group.ItemLinks)
                    {
                        var item_name = item.Item.Name;
                        int userRight = UserRightDAO.Instance.UserRight(Kun_Static.UserStatic.IdUser, item_name);
                        if (Kun_Static.UserStatic.UserName == "admin")
                        {
                            userRight = 3;
                        }
                        switch (userRight)
                        {
                            case 0:
                                item.Item.Enabled = false;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Lưu mức quyền của chức năng được chọn vào trạng thái dùng chung.
        /// </summary>
        private void LoadItem(ItemClickEventArgs e)
        {
            int userRight = 0;
            if (Kun_Static.UserStatic.UserName == "admin")
            {
                userRight = 4;
            }
            else
            {
                userRight = UserRightDAO.Instance.UserRight(Kun_Static.UserStatic.IdUser, e.Item.Name);
            }
            Kun_Static.UserRight = userRight;
        }
        /// <summary>
        /// Hiển thị tên tài khoản đăng nhập trên thanh công cụ.
        /// </summary>
        private void TextHeader()
        {
            txtAccont.Caption = "TÀI KHOẢN: " +Kun_Static.UserStatic.DisplayName.ToUpper();
        }
        /// <summary>
        /// Kích hoạt form MDI đã mở hoặc tạo và hiển thị form mới.
        /// </summary>
        public void OpenForm(Type typeform)
        {
            foreach (var frm in MdiChildren.Where(frm => frm.GetType() == typeform))
            {
                frm.Activate();
                return;
            }

            var form = (Form)(Activator.CreateInstance(typeform));
            BeginInvoke(new Action(() =>
            {
                form.MdiParent = this;
                form.Show();
            }));

        }
        #region Load Thông Báo
        /// <summary>
        /// Điểm mở rộng hiện chưa có xử lý; giữ nguyên hành vi hiện tại.
        /// </summary>
        private void LoadOrder()
        {

        }
        #endregion
        /// <summary>
        /// Mở màn hình chức năng tương ứng rồi lưu quyền của chức năng được chọn.
        /// </summary>
        private void btnOrderDetail_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(frmViewOrderDetail));
            LoadItem(e);
        }
        /// <summary>
        /// Mở màn hình chức năng tương ứng rồi lưu quyền của chức năng được chọn.
        /// </summary>
        private void btnPermision_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(frmPermission));
            LoadItem(e);
        }
        /// <summary>
        /// Mở màn hình chức năng tương ứng rồi lưu quyền của chức năng được chọn.
        /// </summary>
        private void btnUserList_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(frmUsers));
            LoadItem(e);
        }
        /// <summary>
        /// Mở màn hình chức năng tương ứng rồi lưu quyền của chức năng được chọn.
        /// </summary>
        private void btnUserGroup_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(frmUserGroup));
            LoadItem(e);
        }
        /// <summary>
        /// Mở màn hình chức năng tương ứng rồi lưu quyền của chức năng được chọn.
        /// </summary>
        private void btnInventory_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(frmInventory));
            LoadItem(e);
        }
        /// <summary>
        /// Mở màn hình chức năng tương ứng rồi lưu quyền của chức năng được chọn.
        /// </summary>
        private void btnPart_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(frmPart));
            LoadItem(e);
        }
        /// <summary>
        /// Mở màn hình chức năng tương ứng rồi lưu quyền của chức năng được chọn.
        /// </summary>
        private void btnSupplier_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(frmSupplier));
            LoadItem(e);
        }
        /// <summary>
        /// Mở màn hình chức năng tương ứng rồi lưu quyền của chức năng được chọn.
        /// </summary>
        private void btnUserInfor_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(frmUserDetail));
            LoadItem(e);
        }
        /// <summary>
        /// Mở màn hình chức năng tương ứng rồi lưu quyền của chức năng được chọn.
        /// </summary>
        private void btnStatisticInput_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(frmStatisticInput));
            LoadItem(e);
        }
        /// <summary>
        /// Mở màn hình chức năng tương ứng rồi lưu quyền của chức năng được chọn.
        /// </summary>
        private void btnStatisticOutput_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(frmStatisticOutput));
            LoadItem(e);
        }
        /// <summary>
        /// Mở màn hình chức năng tương ứng rồi lưu quyền của chức năng được chọn.
        /// </summary>
        private void btnOrder_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(frmOrderList));
            LoadItem(e);
        }
        /// <summary>
        /// Mở màn hình chức năng tương ứng rồi lưu quyền của chức năng được chọn.
        /// </summary>
        private void btnEmployess_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(frmEmployess));
            LoadItem(e);
        }

        /// <summary>
        /// Mở màn hình chức năng tương ứng rồi lưu quyền của chức năng được chọn.
        /// </summary>
        private void btnPurPose_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(frmPurPose));
            LoadItem(e);
        }

        /// <summary>
        /// Mở màn hình chức năng tương ứng rồi lưu quyền của chức năng được chọn.
        /// </summary>
        private void btnReason_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(frmReason));
            LoadItem(e);
        }

        /// <summary>
        /// Mở màn hình chức năng tương ứng rồi lưu quyền của chức năng được chọn.
        /// </summary>
        private void btnNewOrder_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(frmNewOrder));
            LoadItem(e);
        }

        /// <summary>
        /// Mở màn hình chức năng tương ứng rồi lưu quyền của chức năng được chọn.
        /// </summary>
        private void btnNewOrderInfor_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(frmViewNewOrderDetail));
            LoadItem(e);
        }

        /// <summary>
        /// Mở màn hình chức năng tương ứng rồi lưu quyền của chức năng được chọn.
        /// </summary>
        private void btnInvenSpecPart_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenForm(typeof(frmInvenSpecPart));
            LoadItem(e);
        }
    }
}