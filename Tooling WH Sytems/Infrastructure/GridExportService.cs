using System;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraGrid;

namespace Tooling_WH_Sytems.Infrastructure
{
    /// <summary>Xuất grid, giữ danh sách định dạng riêng của từng màn hình.</summary>
    internal static class GridExportService
    {
        /// <summary>Chọn đường dẫn, xuất dữ liệu và mở tệp bằng ứng dụng mặc định.</summary>
        public static void Export(GridControl grid, string filter)
        {
            #region Xuất Excel
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = filter;
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string exportFilePath = saveDialog.FileName;
                    string fileExtension = new FileInfo(exportFilePath).Extension.ToLowerInvariant();

                    try
                    {
                        switch (fileExtension)
                        {
                            case ".xls":
                                grid.ExportToXls(exportFilePath);
                                break;
                            case ".xlsx":
                                grid.ExportToXlsx(exportFilePath);
                                break;
                            case ".rtf":
                                grid.ExportToRtf(exportFilePath);
                                break;
                            case ".pdf":
                                grid.ExportToPdf(exportFilePath);
                                break;
                            case ".html":
                                grid.ExportToHtml(exportFilePath);
                                break;
                            case ".mht":
                                grid.ExportToMht(exportFilePath);
                                break;
                            default:
                                break;
                        }

                    }
                    catch (Exception ex)
                    {
                        UiActions.ShowError("Không thể xuất tệp", ex);
                        return;
                    }

                    if (File.Exists(exportFilePath))
                    {
                        try
                        {
                            // Mở tệp bằng ứng dụng mặc định của Windows.
                            System.Diagnostics.Process.Start(exportFilePath);
                        }
                        catch (Exception ex)
                        {
                            AppLog.Write("Mở tệp vừa xuất", ex);
                            String msg = "The file could not be opened." + Environment.NewLine + Environment.NewLine + "Path: " + exportFilePath;
                            MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        String msg = "The file could not be saved." + Environment.NewLine + Environment.NewLine + "Path: " + exportFilePath;
                        MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            #endregion
        }
    }
}
