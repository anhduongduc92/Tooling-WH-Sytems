using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tooling_WH_Sytems.Backup;
using Tooling_WH_Sytems.Order;

namespace Tooling_WH_Sytems
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Seven Classic", "White");
            WindowsFormsSettings.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.False;
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmLogin());
        }
        private static void initCulturalFormattingChanges()
        {
            CultureInfo cultureDefinition = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            cultureDefinition.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            cultureDefinition.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
            Thread.CurrentThread.CurrentCulture = cultureDefinition;
        }
    }
}
