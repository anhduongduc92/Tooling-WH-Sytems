using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Tooling_WH_Sytems.Report
{
    /// <summary>
    /// Chuẩn bị báo cáo chi tiết đơn yêu cầu.
    /// </summary>
    public partial class rpOrderdetail : DevExpress.XtraReports.UI.XtraReport
    {
        /// <summary>
        /// Khởi tạo giao diện cùng các giá trị đầu vào của màn hình.
        /// </summary>
        public rpOrderdetail()
        {
            InitializeComponent();
            LoadData();
        }
        /// <summary>
        /// Nạp dữ liệu nghiệp vụ vào bảng hiển thị.
        /// </summary>
        public void LoadData()
        {
            txtBarCode.DataBindings.Add("Text", DataSource, "IdOrder");
            txtPartCode.DataBindings.Add("Text", DataSource, "PartCode");
            txtPartName.DataBindings.Add("Text", DataSource, "PartName");
            txtQuantityOr.DataBindings.Add("Text", DataSource, "QuantityOr");
            txtUnit.DataBindings.Add("Text", DataSource, "Unit");
            txtPurPose.DataBindings.Add("Text", DataSource, "PurPose");
            txtReason.DataBindings.Add("Text", DataSource, "Reason");
            txtDateCreate.DataBindings.Add("Text", DataSource, "DateCreate");
            txtDateConfirm.DataBindings.Add("Text", DataSource, "DateConfirm");
            txtPerConfirm.DataBindings.Add("Text", DataSource, "PersonConfirm");
            txtPersonCreate.DataBindings.Add("Text", DataSource, "PersonCreate");
            txtPerAppro.DataBindings.Add("Text", DataSource, "PersonAppro");
            txtDateAppro.DataBindings.Add("Text", DataSource, "DateAppro");
            txtNote.DataBindings.Add("Text", DataSource, "Note");
        }

    }
}
