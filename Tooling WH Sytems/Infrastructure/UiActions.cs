using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Tooling_WH_Sytems.Infrastructure
{
    /// <summary>Các thao tác thông báo và kiểm tra quyền dùng chung cho form.</summary>
    internal static class UiActions
    {
        public static bool HasPermission(int minimumRight)
        {
            if (Kun_Static.UserRight >= minimumRight) return true;
            XtraMessageBox.Show("bạn không có quyền !".ToUpper(), "Thông Báo",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        public static void ShowError(string operation, Exception exception)
        {
            AppLog.Write(operation, exception);
            XtraMessageBox.Show(operation + ". Vui lòng kiểm tra dữ liệu hoặc kết nối rồi thử lại.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
