using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class InventoryDAO
    {
        private static InventoryDAO instance;

        public static InventoryDAO Instance
        {
            get
            {
                if (instance == null) instance = new InventoryDAO();
                return instance;
            }
            set => instance = value;
        }
        #region Input Part
        public object StatisticInput(DateTime date1, DateTime date2)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-CA");
            return (object)DataProvider.Instance.ExecuteQuery("SELECT InputPart.PartCode,PartName,QuantityIn,Unit,DateIn,EmployessCode,SupplierCode,Note FROM InputPart, Part WHERE Part.PartCode = InputPart.PartCode AND DateIn >='" + date1 + "' AND DateIn <='" + date2 + "'");
        }
        public bool InsertInput(string PartCode, float QuantityIn, 
            DateTime DateIn, string EmployessCode, string SupplierCode,
            string Note, int StatusInput, int StatusIventory)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-CA");
            string query = string.Format("INSERT INTO [dbo].[InputPart] ([PartCode],[QuantityIn],[DateIn],[EmployessCode],[SupplierCode],[Note],[StatusInput],[StatusIventory]) VALUES (N'{0}',{1},'{2}',N'{3}',N'{4}',N'{5}',{6},{7})", PartCode, QuantityIn, DateIn, EmployessCode, SupplierCode, Note, StatusInput, StatusIventory);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool INSERT(string PartCode, float QuantityIn,
           DateTime DateIn, string EmployessCode, string SupplierCode,
           string Note, int StatusInput, int StatusIventory,int QuantPSC)
        {          
            string query = @"INSERT INTO [dbo].[InputPart] ([PartCode],[QuantityIn],
                                [DateIn],[EmployessCode],[SupplierCode],[Note],[StatusInput],
                                [StatusIventory],[QuantPSC]) VALUES 
                                ( @1 , @2 , @3 , @4 , @5 , @6 , @7 , @8 , @9 )";
            int result = DataProvider.Instance.ExecuteNonQuery(query,new object[] {
                                PartCode, QuantityIn, DateIn, EmployessCode, SupplierCode,
                                Note, StatusInput, StatusIventory ,QuantPSC});
            return result > 0;
        }
        public bool UpdateStatusInput(long id, int StatusInput)
        {

            string query = string.Format("UPDATE InputPart SET StatusInput = {1} WHERE Id = {0}", id, StatusInput);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateStatusInventoryByCode(string Code, int StatusIventory, int StatusInput)
        {
            string query = string.Format("UPDATE InputPart SET StatusIventory = {1} WHERE StatusInput = {2} AND PartCode = N'{0}'", Code, StatusIventory, StatusInput);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateStatusInventory(long id, int StatusIventory)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-CA");
            string query = string.Format("UPDATE InputPart SET StatusIventory = {1} WHERE Id = {0}", id, StatusIventory);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public DateTime? DateMinInput(string PartCode)
        {
            DateTime? date = null;
            try
            {
                string query = "SELECT MIN(DateIn) FROM InputPart  WHERE StatusInput = 1 AND PartCode = @1 ";
                date = (DateTime)DataProvider.Instance.ExecuteScalar(query, new object[] { PartCode });
            }
            catch 
            {
            }
            return date;
        }
        #endregion
        #region Inventory Part
        public List<InventoryDTO> GetListInventory()
        {
            List<InventoryDTO> listI = new List<InventoryDTO>();
            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC USP_GetInventoryAll");
            foreach (DataRow item in data.Rows)
            {
                InventoryDTO i = new InventoryDTO(item);
                listI.Add(i);
            }
            return listI;
        }
        public List<InventoryDTO> GetListInventory0()
        {
            List<InventoryDTO> listI = new List<InventoryDTO>();
            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC USP_GetInventory0");
            foreach (DataRow item in data.Rows)
            {
                InventoryDTO i = new InventoryDTO(item);
                listI.Add(i);
            }
            return listI;
        }
        public List<InventoryDTO> GetListInventoryByCode(string partCode)
        {
            List<InventoryDTO> listI = new List<InventoryDTO>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT InputPart.Id, InputPart.PartCode,PartName,DateIn,QuantityIn,ISNULL(SUM(QuantityOut),0) AS CountXuat,(QuantityIn - ISNULL(SUM(QuantityOut),0)) AS QuantityInventory,InputPart.Note,StatusInput,StatusIventory,Unit FROM dbo.InputPart LEFT JOIN dbo.OutputPart ON InputPart.Id = IdInput LEFT JOIN dbo.Part ON Part.PartCode = InputPart.PartCode WHERE  StatusInput = 1 AND InputPart.PartCode = N'" + partCode + "' GROUP BY InputPart.Id, InputPart.PartCode, PartName, DateIn, QuantityIn, InputPart.Note, StatusInput, StatusIventory, Unit");
            foreach (DataRow item in data.Rows)
            {
                InventoryDTO i = new InventoryDTO(item);
                listI.Add(i);
            }
            return listI;
        }
  
        public List<InvenPartDTO> GetList(int spec)
        {
            List<InvenPartDTO> list = new List<InvenPartDTO>();
            string query = @"WITH total_imports AS (SELECT PartCode,SUM(QuantityIn) 
                AS total_imported FROM InputPart WHERE StatusInput = 1 GROUP BY PartCode), 
                total_exports AS (SELECT i.PartCode,SUM(e.QuantityOut) AS total_exported 
                FROM OutputPart e INNER JOIN InputPart i ON e.IdInput = i.Id WHERE StatusInput = 1 GROUP BY i.PartCode) 
                SELECT p.PartCode,p.PartName,p.Unit,p.StatusPart,
                COALESCE(ti.total_imported, 0) AS Total_imported,
                COALESCE(te.total_exported, 0) AS Total_exported,
                COALESCE(ti.total_imported, 0) - COALESCE(te.total_exported, 0) AS QuantInven,
				p.SpecPart 
                FROM Part p LEFT JOIN total_imports ti ON p.PartCode = ti.PartCode 
                LEFT JOIN total_exports te ON p.PartCode = te.PartCode 
				WHERE p.SpecPart = @1 
				ORDER BY p.PartCode;";
            DataTable da = DataProvider.Instance.ExecuteQuery(query,new object[] { spec});
            foreach (DataRow item in da.Rows)
            {
                InvenPartDTO inven = new InvenPartDTO(item);
                list.Add(inven);
            }
            return list;
        }
        public InvenPartDTO GetItem(string PartCode)
        {
           InvenPartDTO list = null;
            string query = "WITH total_imports AS (SELECT PartCode,SUM(QuantityIn) AS total_imported FROM InputPart WHERE StatusInput = 1 GROUP BY PartCode)," +
                "total_exports AS (SELECT i.PartCode,SUM(e.QuantityOut) AS total_exported " +
                "FROM OutputPart e INNER JOIN InputPart i ON e.IdInput = i.Id WHERE StatusInput = 1 GROUP BY i.PartCode) " +
                "SELECT p.PartCode,p.PartName,p.Unit,p.StatusPart," +
                "COALESCE(ti.total_imported, 0) AS Total_imported," +
                "COALESCE(te.total_exported, 0) AS Total_exported," +
                "COALESCE(ti.total_imported, 0) - COALESCE(te.total_exported, 0) AS QuantInven " +
                "FROM Part p LEFT JOIN total_imports ti ON p.PartCode = ti.PartCode " +
                "LEFT JOIN total_exports te ON p.PartCode = te.PartCode WHERE p.PartCode = @1 ORDER BY p.PartCode;";
            DataTable da = DataProvider.Instance.ExecuteQuery(query,new object[] { PartCode});
            if (da.Rows.Count > 0)
            {
                list = new InvenPartDTO(da.Rows[0]);
            }
            return list;
        }
        public float TotalInventoryByPart(string partCode)
        {
            List<InventoryDTO> listI = GetListInventoryByCode(partCode);
            float sum = 0;
            foreach (InventoryDTO item in listI)
            {
                sum += item.QuantityInventory;
            }
            return sum;
        }
        #endregion
        #region Only Part
        //public List<OnlyPart> OnlyPartCode()
        //{
        //    List<OnlyPart> listO = new List<OnlyPart>();
        //    DataTable data = DataProvider.Instance.ExecuteQuery("SELECT DISTINCT( InputPart.PartCode),PartName FROM InputPart,Part WHERE StatusInput = 1 AND InputPart.PartCode = Part.PartCode");
        //    foreach (DataRow item in data.Rows)
        //    {
        //        OnlyPart o = new OnlyPart(item);
        //        listO.Add(o);
        //    }
        //    return listO;
        //}
        public List<OnlyPart> OnlyPartCode()
        {
            List<OnlyPart> listO = new List<OnlyPart>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT DISTINCT(PartCode),PartName FROM Part");
            foreach (DataRow item in data.Rows)
            {
                OnlyPart o = new OnlyPart(item);
                listO.Add(o);
            }
            return listO;
        }
        #endregion
        #region Output Part
        public bool InsertOutputPart(long IdInput, DateTime DateOut, float QuantityOut, string EmployessCode, string Note, string FactoryCode)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-CA");
            string query = string.Format("INSERT INTO [dbo].[OutputPart] ([IdInput],[DateOut],[QuantityOut],[EmployessCode],[Note],[FactoryCode]) VALUES ({0},'{1}',{2},N'{3}',N'{4}',N'{5}')", IdInput, DateOut, QuantityOut, EmployessCode, Note, FactoryCode);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public object StatisticOutput(DateTime date1, DateTime date2)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-CA");
            return (object)DataProvider.Instance.ExecuteQuery("SELECT OutputPart.Id,InputPart.PartCode,PartName,DateOut,QuantityOut,Unit,OutputPart.EmployessCode,OutputPart.Note,OutputPart.FactoryCode FROM OutputPart,InputPart,Part WHERE Part.PartCode = InputPart.PartCode AND IdInput = InputPart.Id AND DateOut >='" + date1 + "' AND DateOut <='" + date2 + "'");
        }
        public int TestOutputByPart(string PartCode)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT OutputPart.Id,InputPart.PartCode,PartName,DateOut,QuantityOut,Unit,OutputPart.EmployessCode,OutputPart.Note,OutputPart.FactoryCode FROM OutputPart,InputPart,Part WHERE Part.PartCode = InputPart.PartCode AND IdInput = InputPart.Id AND Part.PartCode = N'" + PartCode + "'");
            if (data.Rows.Count > 0)
            {
                return 1;
            }
            return -1;
        }
        public DateTime MaxOutputByPart(string PartCode)
        {
            return (DateTime)DataProvider.Instance.ExecuteScalar("SELECT MAX(DateOut) FROM OutputPart,InputPart,Part WHERE Part.PartCode = InputPart.PartCode AND IdInput = InputPart.Id AND Part.PartCode = N'" + PartCode + "'");
        }
        #endregion
    }
}
