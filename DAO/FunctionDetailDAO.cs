using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class FunctionDetailDAO
    {
        private static FunctionDetailDAO instance;

        public static FunctionDetailDAO Instance
        {
            get
            {
                if (instance == null) instance = new FunctionDetailDAO();
                return instance;
            }

            set => instance = value;
        }
        public bool InsertFuctionDetail(string code, string parentid, string name)
        {
            string query = string.Format("INSERT INTO [dbo].[FunctionDetail] ([CodeFuc],[ParentId],[NameFuc]) VALUES (N'{0}' ,N'{1}' ,N'{2}')", code, parentid, name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateFuctionDetail(string code, string parentid, string name)
        {
            string query = string.Format("UPDATE [dbo].[FunctionDetail] SET [ParentId] = N'{1}',[NameFuc] = N'{2}' WHERE [CodeFuc] = N'{0}'", code, parentid, name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public int TestFuncDetail(string code)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM FunctionDetail WHERE CodeFuc = N'" + code + "'");
            if (data.Rows.Count > 0)
            {
                return 1;
            }
            return -1;
        }

    }
}
