using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
   public class SupplierDAO
    {
        private static SupplierDAO instance;

        public static SupplierDAO Instance
        { get
            {
                if (instance == null) instance = new SupplierDAO();
                return instance;
            }
            set => instance = value;
        }
        public List<SupplierDTO> GetListSupplier()
        {
            List<SupplierDTO> listS = new List<SupplierDTO>();
            DataTable data = DataProvider.Instance.ExecuteQuery("Select * from Supplier");
            foreach (DataRow item in data.Rows)
            {
                SupplierDTO s = new SupplierDTO(item);
                listS.Add(s);
            }
            return listS;
        }
        public int IdSupplier(string code)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("Select * from Supplier Where SupplierCode = N'" + code + "'");
            if(data.Rows.Count >0)
            {
                SupplierDTO s = new SupplierDTO(data.Rows[0]);
                return s.Id;
            }
            return -1;
        }
        public bool InsertSupplier(string SupplierCode, string SupplierName , string Address, string Phone, string Email)
        {
            string query = string.Format("INSERT INTO [dbo].[Supplier] ([SupplierCode],[SupplierName] ,[Address],[Phone],[Email]) VALUES(N'{0}',N'{1}',N'{2}',N'{3}',N'{4}')", SupplierCode, SupplierName, Address, Phone, Email);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateSupplier(string SupplierCode, string SupplierName, string Address, string Phone, string Email)
        {
            string query = string.Format("UPDATE [dbo].[Supplier] SET [SupplierName] = N'{1}',[Address] = N'{2}',[Phone] = N'{3}',[Email] = N'{4}' WHERE [SupplierCode] = N'{0}'", SupplierCode, SupplierName, Address, Phone, Email);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteSupplier(string SupplierCode)
        {
            string query = string.Format("DELETE Supplier WHERE SupplierCode = N'{0}'",SupplierCode);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
