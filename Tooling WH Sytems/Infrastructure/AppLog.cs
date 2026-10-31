using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Tooling_WH_Sytems.Infrastructure
{
    /// <summary>Ghi lỗi kỹ thuật vào thư mục riêng của người dùng, không cản trở nghiệp vụ.</summary>
    internal static class AppLog
    {
        private static readonly object SyncRoot = new object();

        public static void Write(string operation, Exception exception)
        {
            if (exception == null) return;
            try
            {
                string directory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Tooling WH Sytems", "Logs");
                string entry = string.Format("[{0:O}] {1}{2}{3}{2}{2}",
                    DateTime.Now, operation, Environment.NewLine, exception);
                lock (SyncRoot)
                {
                    Directory.CreateDirectory(directory);
                    File.AppendAllText(Path.Combine(directory, DateTime.Today.ToString("yyyy-MM-dd") + ".log"),
                        entry, Encoding.UTF8);
                }
            }
            catch (Exception)
            {
                // Không được để lỗi ghi log che mất lỗi nghiệp vụ ban đầu.
                Trace.WriteLine("Không thể ghi nhật ký: " + operation);
            }
        }
    }
}
