namespace Tooling_WH_Sytems
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.txtAccont = new DevExpress.XtraBars.BarStaticItem();
            this.btnStatisticInput = new DevExpress.XtraBars.BarButtonItem();
            this.btnStatisticOutput = new DevExpress.XtraBars.BarButtonItem();
            this.btnInventory = new DevExpress.XtraBars.BarButtonItem();
            this.btnOrder = new DevExpress.XtraBars.BarButtonItem();
            this.btnOrderDetail = new DevExpress.XtraBars.BarButtonItem();
            this.btnPart = new DevExpress.XtraBars.BarButtonItem();
            this.btnSupplier = new DevExpress.XtraBars.BarButtonItem();
            this.btnUserInfor = new DevExpress.XtraBars.BarButtonItem();
            this.btnUserList = new DevExpress.XtraBars.BarButtonItem();
            this.btnPermision = new DevExpress.XtraBars.BarButtonItem();
            this.txtVersion = new DevExpress.XtraBars.BarStaticItem();
            this.btnUserGroup = new DevExpress.XtraBars.BarButtonItem();
            this.btnEmployess = new DevExpress.XtraBars.BarButtonItem();
            this.btnPurPose = new DevExpress.XtraBars.BarButtonItem();
            this.btnReason = new DevExpress.XtraBars.BarButtonItem();
            this.btnSkin = new DevExpress.XtraBars.SkinRibbonGalleryBarItem();
            this.btnNewOrder = new DevExpress.XtraBars.BarButtonItem();
            this.btnNewOrderInfor = new DevExpress.XtraBars.BarButtonItem();
            this.rbpQuanLyKho = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rbpGWarehourse = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rbpOrder = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rbpGInOut = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rdpDataSource = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.rbpGDataSource = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rbpGUsers = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.rpGSkin = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.xtraTabbedMdiManager1 = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(this.components);
            this.btnInvenSpecPart = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.CaptionBarItemLinks.Add(this.txtAccont);
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.txtAccont,
            this.ribbon.ExpandCollapseItem,
            this.ribbon.SearchEditItem,
            this.btnStatisticInput,
            this.btnStatisticOutput,
            this.btnInventory,
            this.btnOrder,
            this.btnOrderDetail,
            this.btnPart,
            this.btnSupplier,
            this.btnUserInfor,
            this.btnUserList,
            this.btnPermision,
            this.txtVersion,
            this.btnUserGroup,
            this.btnEmployess,
            this.btnPurPose,
            this.btnReason,
            this.btnSkin,
            this.btnNewOrder,
            this.btnNewOrderInfor,
            this.btnInvenSpecPart});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.Margin = new System.Windows.Forms.Padding(4);
            this.ribbon.MaxItemId = 26;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.rbpQuanLyKho,
            this.rbpOrder,
            this.rdpDataSource});
            this.ribbon.Size = new System.Drawing.Size(1356, 143);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            // 
            // txtAccont
            // 
            this.txtAccont.Caption = "Administrator";
            this.txtAccont.Id = 13;
            this.txtAccont.Name = "txtAccont";
            // 
            // btnStatisticInput
            // 
            this.btnStatisticInput.Caption = "THỐNG KÊ NHẬP KHO";
            this.btnStatisticInput.Id = 1;
            this.btnStatisticInput.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnStatisticInput.ImageOptions.SvgImage")));
            this.btnStatisticInput.Name = "btnStatisticInput";
            this.btnStatisticInput.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnStatisticInput_ItemClick);
            // 
            // btnStatisticOutput
            // 
            this.btnStatisticOutput.Caption = "THỐNG KÊ XUẤT KHO";
            this.btnStatisticOutput.Id = 2;
            this.btnStatisticOutput.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnStatisticOutput.ImageOptions.SvgImage")));
            this.btnStatisticOutput.Name = "btnStatisticOutput";
            this.btnStatisticOutput.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnStatisticOutput_ItemClick);
            // 
            // btnInventory
            // 
            this.btnInventory.Caption = "TỒN KHO";
            this.btnInventory.Id = 3;
            this.btnInventory.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnInventory.ImageOptions.Image")));
            this.btnInventory.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnInventory.ImageOptions.LargeImage")));
            this.btnInventory.Name = "btnInventory";
            this.btnInventory.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnInventory_ItemClick);
            // 
            // btnOrder
            // 
            this.btnOrder.Caption = "DANH SÁCH PHIẾU YC";
            this.btnOrder.Id = 4;
            this.btnOrder.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnOrder.ImageOptions.SvgImage")));
            this.btnOrder.Name = "btnOrder";
            this.btnOrder.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOrder_ItemClick);
            // 
            // btnOrderDetail
            // 
            this.btnOrderDetail.Caption = "CHI TIẾT PHIẾU YC";
            this.btnOrderDetail.Id = 5;
            this.btnOrderDetail.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnOrderDetail.ImageOptions.SvgImage")));
            this.btnOrderDetail.Name = "btnOrderDetail";
            this.btnOrderDetail.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOrderDetail_ItemClick);
            // 
            // btnPart
            // 
            this.btnPart.Caption = "LINH KIỆN";
            this.btnPart.Id = 7;
            this.btnPart.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnPart.ImageOptions.SvgImage")));
            this.btnPart.Name = "btnPart";
            this.btnPart.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPart_ItemClick);
            // 
            // btnSupplier
            // 
            this.btnSupplier.Caption = "NHÀ CUNG CẤP";
            this.btnSupplier.Id = 8;
            this.btnSupplier.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnSupplier.ImageOptions.SvgImage")));
            this.btnSupplier.Name = "btnSupplier";
            this.btnSupplier.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSupplier_ItemClick);
            // 
            // btnUserInfor
            // 
            this.btnUserInfor.Caption = "TÀI KHOẢN NGƯỜI DÙNG";
            this.btnUserInfor.Id = 10;
            this.btnUserInfor.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnUserInfor.ImageOptions.SvgImage")));
            this.btnUserInfor.Name = "btnUserInfor";
            this.btnUserInfor.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUserInfor_ItemClick);
            // 
            // btnUserList
            // 
            this.btnUserList.Caption = "DANH SÁCH USER";
            this.btnUserList.Id = 11;
            this.btnUserList.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnUserList.ImageOptions.SvgImage")));
            this.btnUserList.Name = "btnUserList";
            this.btnUserList.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUserList_ItemClick);
            // 
            // btnPermision
            // 
            this.btnPermision.Caption = "PHÂN QUYỀN USER";
            this.btnPermision.Id = 12;
            this.btnPermision.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnPermision.ImageOptions.SvgImage")));
            this.btnPermision.Name = "btnPermision";
            this.btnPermision.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPermision_ItemClick);
            // 
            // txtVersion
            // 
            this.txtVersion.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.txtVersion.Id = 14;
            this.txtVersion.Name = "txtVersion";
            // 
            // btnUserGroup
            // 
            this.btnUserGroup.Caption = "PHÒNG BAN";
            this.btnUserGroup.Id = 15;
            this.btnUserGroup.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnUserGroup.ImageOptions.Image")));
            this.btnUserGroup.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnUserGroup.ImageOptions.LargeImage")));
            this.btnUserGroup.Name = "btnUserGroup";
            this.btnUserGroup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUserGroup_ItemClick);
            // 
            // btnEmployess
            // 
            this.btnEmployess.Caption = "NHÂN VIÊN";
            this.btnEmployess.Id = 16;
            this.btnEmployess.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnEmployess.ImageOptions.Image")));
            this.btnEmployess.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnEmployess.ImageOptions.LargeImage")));
            this.btnEmployess.Name = "btnEmployess";
            this.btnEmployess.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnEmployess_ItemClick);
            // 
            // btnPurPose
            // 
            this.btnPurPose.Caption = "MỤC  ĐÍCH";
            this.btnPurPose.Id = 17;
            this.btnPurPose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnPurPose.ImageOptions.SvgImage")));
            this.btnPurPose.Name = "btnPurPose";
            this.btnPurPose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPurPose_ItemClick);
            // 
            // btnReason
            // 
            this.btnReason.Caption = "LÝ DO";
            this.btnReason.Id = 18;
            this.btnReason.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnReason.ImageOptions.SvgImage")));
            this.btnReason.Name = "btnReason";
            this.btnReason.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnReason_ItemClick);
            // 
            // btnSkin
            // 
            this.btnSkin.Caption = "skinRibbonGalleryBarItem1";
            this.btnSkin.Id = 21;
            this.btnSkin.Name = "btnSkin";
            // 
            // btnNewOrder
            // 
            this.btnNewOrder.Caption = "DANH SÁCH  ĐƠN HÀNG";
            this.btnNewOrder.Id = 22;
            this.btnNewOrder.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnNewOrder.ImageOptions.SvgImage")));
            this.btnNewOrder.Name = "btnNewOrder";
            this.btnNewOrder.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnNewOrder_ItemClick);
            // 
            // btnNewOrderInfor
            // 
            this.btnNewOrderInfor.Caption = "CHI TIẾT ĐƠN HÀNG";
            this.btnNewOrderInfor.Id = 23;
            this.btnNewOrderInfor.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnNewOrderInfor.ImageOptions.SvgImage")));
            this.btnNewOrderInfor.Name = "btnNewOrderInfor";
            this.btnNewOrderInfor.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnNewOrderInfor_ItemClick);
            // 
            // rbpQuanLyKho
            // 
            this.rbpQuanLyKho.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rbpGWarehourse});
            this.rbpQuanLyKho.Name = "rbpQuanLyKho";
            this.rbpQuanLyKho.Text = "QUẢN LÝ KHO";
            // 
            // rbpGWarehourse
            // 
            this.rbpGWarehourse.ItemLinks.Add(this.btnInventory);
            this.rbpGWarehourse.ItemLinks.Add(this.btnStatisticInput, true);
            this.rbpGWarehourse.ItemLinks.Add(this.btnStatisticOutput, true);
            this.rbpGWarehourse.ItemLinks.Add(this.btnInvenSpecPart, true);
            this.rbpGWarehourse.Name = "rbpGWarehourse";
            this.rbpGWarehourse.Text = "KHO ĐÔNG DƯƠNG KHUÔN";
            // 
            // rbpOrder
            // 
            this.rbpOrder.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rbpGInOut,
            this.ribbonPageGroup1});
            this.rbpOrder.Name = "rbpOrder";
            this.rbpOrder.Text = "QUẢN LÝ PHIẾU YÊU CẦU XUẤT KHO";
            // 
            // rbpGInOut
            // 
            this.rbpGInOut.ItemLinks.Add(this.btnOrder);
            this.rbpGInOut.ItemLinks.Add(this.btnOrderDetail, true);
            this.rbpGInOut.Name = "rbpGInOut";
            this.rbpGInOut.Text = "PHIẾU XUẤT KHO";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.btnNewOrder);
            this.ribbonPageGroup1.ItemLinks.Add(this.btnNewOrderInfor, true);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "ĐƠN HÀNG";
            // 
            // rdpDataSource
            // 
            this.rdpDataSource.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.rbpGDataSource,
            this.rbpGUsers,
            this.rpGSkin});
            this.rdpDataSource.Name = "rdpDataSource";
            this.rdpDataSource.Text = "DỮ LIỆU NGUỒN";
            // 
            // rbpGDataSource
            // 
            this.rbpGDataSource.ItemLinks.Add(this.btnPart);
            this.rbpGDataSource.ItemLinks.Add(this.btnSupplier, true);
            this.rbpGDataSource.ItemLinks.Add(this.btnEmployess, true);
            this.rbpGDataSource.ItemLinks.Add(this.btnUserGroup, true);
            this.rbpGDataSource.ItemLinks.Add(this.btnPurPose, true);
            this.rbpGDataSource.ItemLinks.Add(this.btnReason, true);
            this.rbpGDataSource.Name = "rbpGDataSource";
            this.rbpGDataSource.Text = "DỮ LIỆU NGUỒN";
            // 
            // rbpGUsers
            // 
            this.rbpGUsers.ItemLinks.Add(this.btnUserInfor);
            this.rbpGUsers.ItemLinks.Add(this.btnUserList, true);
            this.rbpGUsers.ItemLinks.Add(this.btnPermision, true);
            this.rbpGUsers.Name = "rbpGUsers";
            this.rbpGUsers.Text = "QUẢN LÝ USER";
            // 
            // rpGSkin
            // 
            this.rpGSkin.ItemLinks.Add(this.btnSkin);
            this.rpGSkin.Name = "rpGSkin";
            this.rpGSkin.Text = "GIAO DIỆN";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.ItemLinks.Add(this.txtVersion);
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 736);
            this.ribbonStatusBar.Margin = new System.Windows.Forms.Padding(4);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(1356, 31);
            // 
            // xtraTabbedMdiManager1
            // 
            this.xtraTabbedMdiManager1.MdiParent = this;
            // 
            // btnInvenSpecPart
            // 
            this.btnInvenSpecPart.Caption = "TỒN KHO LINH KIỆN ĐẶC BIỆT";
            this.btnInvenSpecPart.Id = 25;
            this.btnInvenSpecPart.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.ImageOptions.Image")));
            this.btnInvenSpecPart.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.ImageOptions.LargeImage")));
            this.btnInvenSpecPart.Name = "btnInvenSpecPart";
            this.btnInvenSpecPart.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnInvenSpecPart_ItemClick);
            // 
            // frmMain
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1356, 767);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMain";
            this.Ribbon = this.ribbon;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "TOOL WH SYSTEMS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage rbpQuanLyKho;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbpGWarehourse;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.BarButtonItem btnStatisticInput;
        private DevExpress.XtraBars.BarButtonItem btnStatisticOutput;
        private DevExpress.XtraBars.BarButtonItem btnInventory;
        private DevExpress.XtraBars.BarButtonItem btnOrder;
        private DevExpress.XtraBars.BarButtonItem btnOrderDetail;
        private DevExpress.XtraBars.BarButtonItem btnPart;
        private DevExpress.XtraBars.BarButtonItem btnSupplier;
        private DevExpress.XtraBars.BarButtonItem btnUserInfor;
        private DevExpress.XtraBars.BarButtonItem btnUserList;
        private DevExpress.XtraBars.BarButtonItem btnPermision;
        private DevExpress.XtraBars.Ribbon.RibbonPage rbpOrder;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbpGInOut;
        private DevExpress.XtraBars.Ribbon.RibbonPage rdpDataSource;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbpGDataSource;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rbpGUsers;
        private DevExpress.XtraBars.BarStaticItem txtAccont;
        private DevExpress.XtraBars.BarStaticItem txtVersion;
        private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager xtraTabbedMdiManager1;
        private DevExpress.XtraBars.BarButtonItem btnUserGroup;
        private DevExpress.XtraBars.BarButtonItem btnEmployess;
        private DevExpress.XtraBars.BarButtonItem btnPurPose;
        private DevExpress.XtraBars.BarButtonItem btnReason;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpGSkin;
        private DevExpress.XtraBars.SkinRibbonGalleryBarItem btnSkin;
        private DevExpress.XtraBars.BarButtonItem btnNewOrder;
        private DevExpress.XtraBars.BarButtonItem btnNewOrderInfor;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem btnInvenSpecPart;
    }
}