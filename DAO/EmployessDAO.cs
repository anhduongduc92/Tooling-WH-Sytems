using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
   public class EmployessDAO
    {
        private static EmployessDAO instance;

        public static EmployessDAO Instance
        { get
            {
                if (instance == null) instance = new EmployessDAO();
                return instance;
            }
            set => instance = value;
        }
        public List<EmployesDTO> GetListEmployess()
        {
            List<EmployesDTO> listE = new List<EmployesDTO>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM Employess");
            foreach (DataRow item in data.Rows)
            {
                EmployesDTO e = new EmployesDTO(item);
                listE.Add(e);
            }
            return listE;
        }
        public EmployesDTO GetListEmployessById(int id)
        {
            return GetListEmployess().FirstOrDefault(x => x.Id == id);
        }
        public EmployesDTO GetItem(string emCode )
        {
            EmployesDTO employes = null;
            string query = "SELECT * FROM Employess WHERE EmployessCode = @1 ";
            DataTable data = DataProvider.Instance.ExecuteQuery(query,new object[] { emCode});
            if (data.Rows.Count > 0)
            {
                employes = new EmployesDTO(data.Rows[0]);
                
            }
            return employes;
        }
        public int IdEmployess(string employ)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM Employess WHERE EmployessCode = N'" + employ + "'");
            if(data.Rows.Count >0)
            {
                EmployesDTO e = new EmployesDTO(data.Rows[0]);
               return e.Id;
            }
            return -1;
        }
        public string EmployessName(string employ)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM Employess WHERE EmployessCode = N'" + employ + "'");
            if (data.Rows.Count > 0)
            {
                EmployesDTO e = new EmployesDTO(data.Rows[0]);
                return e.EmployessName;
            }
            return null;
        }
        public string NameGr(string employ)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM Employess WHERE EmployessCode = N'" + employ + "'");
            if (data.Rows.Count > 0)
            {
                EmployesDTO e = new EmployesDTO(data.Rows[0]);
                return e.NameGr;
            }
            return null;
        }
        public bool InsertEmployess(string EmployessCode,string EmployessName, string Email,string NameGr)
        {
            string query = string.Format("INSERT INTO [dbo].[Employess]([EmployessCode],[EmployessName],[Email],[NameGr]) VALUES (N'{0}',N'{1}',N'{2}',N'{3}')",EmployessCode,EmployessName,Email,NameGr);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateEmployess(string EmployessCode, string EmployessName, string Email, string NameGr)
        {
            string query = string.Format("UPDATE [dbo].[Employess] SET [EmployessName] = N'{1}',[Email] = N'{2}',[NameGr] = N'{3}' WHERE [EmployessCode] = N'{0}'", EmployessCode, EmployessName, Email, NameGr);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteEmployess(string EmployessCode)
        {
            string query = string.Format("DELETE Employess WHERE EmployessCode = N'{0}'", EmployessCode);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
