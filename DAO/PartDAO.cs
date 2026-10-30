using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class PartDAO
    {
        private static PartDAO instance;

        public static PartDAO Instance
        {
            get
            {
                if (instance == null) instance = new PartDAO();
                return instance;
            }

            set => instance = value;
        }
        public List<PartDTO> GetList(int SpecPart)
        {
            List<PartDTO> listP = new List<PartDTO>();
            string query = "select * From Part WHERE SpecPart = @1 ";
            DataTable data = DataProvider.Instance.ExecuteQuery(query,new object[] { SpecPart});
            foreach (DataRow item in data.Rows)
            {
                PartDTO p = new PartDTO(item);
                listP.Add(p);
            }
            return listP;
        }
        public List<PartDTO> GetListPart()
        {
            List<PartDTO> listP = new List<PartDTO>();
            DataTable data = DataProvider.Instance.ExecuteQuery("select * From Part");
            foreach (DataRow item in data.Rows)
            {
                PartDTO p = new PartDTO(item);
                listP.Add(p);
            }
            return listP;
        }
        public PartDTO GetItemPart(string PartCode)
        {
            List<PartDTO> listP = new List<PartDTO>();
            DataTable data = DataProvider.Instance.ExecuteQuery("select * From Part Where PartCode = N'" + PartCode + "'");
            if(data.Rows.Count >0)
            {
                PartDTO p = new PartDTO(data.Rows[0]);
                return p;
            }
            return null;
        }
        public List<PartDTO> GetListPart(int number, int numberSize)
        {
            List<PartDTO> listP = new List<PartDTO>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * from Part ORDER BY Id OFFSET "+number+" ROWS FETCH NEXT "+numberSize+" ROWS ONLY");
            foreach (DataRow item in data.Rows)
            {
                PartDTO p = new PartDTO(item);
                listP.Add(p);
            }
            return listP;
        }
        public long IdPartCode(string code)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * From Part Where PartCode = N'" + code + "'");
            if(data.Rows.Count >0)
            {
                PartDTO p = new PartDTO(data.Rows[0]);
                return p.Id;
            }
            return -1;
        }
        public bool InsertPart(string partCode, string partName, string unit, float minWaint, float maxWaint, int SpecPart)
        {   
            string query = @"INSERT INTO [dbo].[Part] ([PartCode] ,[PartName] ,[Unit] ,[MinWaint] 
                            ,[MaxWaint] ,[SpecPart]) VALUES ( @1 , @2 , @3 , @4 , @5 , @6 )";
            int result = DataProvider.Instance.ExecuteNonQuery(query,
                new object[] { partCode, partName, unit, minWaint, maxWaint, SpecPart });
            return result > 0;
        }
        public bool UpdatePart(string partCode, string partName, string unit, float minWaint, float maxWaint, int SpecPart)
        {
            string query = @"UPDATE [dbo].[Part] SET [PartName] = @1 ,[Unit] = @2 ,[MinWaint] = @3 
                        ,[MaxWaint] = @4 , [SpecPart] = @5 WHERE [PartCode] = @6 ";
            int result = DataProvider.Instance.ExecuteNonQuery(query,new object[] {
                partName, unit, minWaint, maxWaint,SpecPart, partCode });
            return result > 0;
        }
        public bool UpdatePart(string partCode, int Status)
        {
            string query = string.Format("UPDATE [dbo].[Part] SET [StatusPart] = N'{1}' WHERE [PartCode] = N'{0}'", partCode, Status);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool Update(string partCode, int SpecPart)
        {
            string query = @"UPDATE [dbo].[Part] SET [SpecPart] = @1 WHERE [PartCode] = @2 ";
            int result = DataProvider.Instance.ExecuteNonQuery(query,new object[] { SpecPart,partCode});
            return result > 0;
        }
        public bool DeletePart(string partCode)
        {
            string query = string.Format("DELETE Part WHERE PartCode = N'{0}'", partCode);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public float MinWaintPart(string code)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * From Part Where PartCode = N'" + code + "'");
            if (data.Rows.Count > 0)
            {
                PartDTO p = new PartDTO(data.Rows[0]);
                return p.MinWaint;
            }
            return -1;
        }
        public float MaxWaintPart(string code)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * From Part Where PartCode = N'" + code + "'");
            if (data.Rows.Count > 0)
            {
                PartDTO p = new PartDTO(data.Rows[0]);
                return p.MaxWaint;

            }
            return -1;
        }
        public string PartName(string code)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * From Part Where PartCode = N'" + code + "'");
            if (data.Rows.Count > 0)
            {
                PartDTO p = new PartDTO(data.Rows[0]);
                return p.PartName;

            }
            return null;
        }
        public string UnitPart(string code)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * From Part Where PartCode = N'" + code + "'");
            if (data.Rows.Count > 0)
            {
                PartDTO p = new PartDTO(data.Rows[0]);
                return p.Unit;

            }
            return null;
        }
    }
}
