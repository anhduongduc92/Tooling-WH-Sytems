using System.Data;
using System.IO;
using ExcelDataReader;

namespace Tooling_WH_Sytems.Infrastructure
{
    /// <summary>Đọc workbook độc lập với giao diện; đóng stream/reader cả khi đọc thất bại.</summary>
    internal static class ExcelWorkbookReader
    {
        public static DataSet Read(string fileName, bool isBinary)
        {
            using (FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            using (IExcelDataReader reader = isBinary
                ? ExcelReaderFactory.CreateBinaryReader(stream)
                : ExcelReaderFactory.CreateOpenXmlReader(stream))
            {
                return reader.AsDataSet(new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                });
            }
        }
    }
}
