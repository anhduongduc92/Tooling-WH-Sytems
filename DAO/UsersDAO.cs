using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
   public class UsersDAO
    {
        private static UsersDAO instance;

        public static UsersDAO Instance
        { get
            {
                if (instance == null) instance = new UsersDAO();
                return instance;
            }

            set => instance = value;
        }
        //Mã hóa MD5
        public string EnCodeMD5(string Pass)
        {
            string result = "";
            byte[] buffer = Encoding.UTF8.GetBytes(Pass);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            buffer = md5.ComputeHash(buffer);
            for (int i = 0; i < buffer.Length; i++)
            {
                result += buffer[i].ToString("x2");
            }
            return result;
        }
        //UserGroups
        #region User Group
        public object GetUserGroup()
        {
            return (object)DataProvider.Instance.ExecuteQuery("SELECT * FROM UserGroup");
        }
        public object GetUserGroupByName(string Name)
        {
            return (object)DataProvider.Instance.ExecuteQuery("SELECT * FROM UserGroup WHERE NameUserGroup = N'"+Name+"'");
        }
        public bool InsertUserGr(string NameGr)
        {
            string query = string.Format("INSERT INTO UserGroup (NameUserGroup) VALUES (N'{0}')", NameGr);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateUserGr(int id, string NameGr)
        {
            string query = string.Format("UPDATE UserGroup SET NameUserGroup = N'{1}' WHERE Id = {0}", id, NameGr);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteUserGr(int id)
        {
            string query = string.Format("DELETE UserGroup WHERE Id = {0}", id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public int TestUserGr(string name)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM UserGroup WHERE NameUserGroup = N'" + name + "'");
            if (data.Rows.Count > 0)
            {
                return 1;
            }
            return -1;
        }
        #endregion
        //User Login
        #region User Login
        public bool Login(string userName, string passWord)
        {
            string query = string.Format("Select * from Users where UserName = N'{0}' AND Pass = N'{1}'",userName,passWord);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            return data.Rows.Count > 0;
        }
        #endregion
        //User List
        #region User List
        public List<UsersDTO> GetListUser()
        {
            List<UsersDTO> listU = new List<UsersDTO>();
            DataTable data = DataProvider.Instance.ExecuteQuery("Select * from Users");
            foreach (DataRow item in data.Rows)
            {
                UsersDTO u = new UsersDTO(item);
                listU.Add(u);
            }
            return listU;
        }
        public UsersDTO GetListUserByCode(string user)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("Select * from Users where UserName = N'"+ user + "'");
            foreach (DataRow item in data.Rows)
            {
               return new UsersDTO(item);
            }
            return null;
        }
        public object GetUserPermission(string nameGr)
        {
            return (object)DataProvider.Instance.ExecuteQuery("SELECT * from Users, Employess Where EmCode = EmployessCode AND NameGr = N'"+ nameGr + "'");
        }
        public int IdUsers(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("Select * from Users where UserName = N'" + userName + "'");
            if(data.Rows.Count >0)
            {
                UsersDTO u = new UsersDTO(data.Rows[0]);
                return u.IdUser;
            }
            return -1;
        }
        public bool InsertUser(string UserName,string Pass, string DisplayName, string EmCode)
        {
            string query = string.Format("INSERT INTO [dbo].[Users] ([UserName],[Pass] ,[DisplayName] ,[EmCode]) VALUES(N'{0}',N'{1}',N'{2}',N'{3}')", UserName,Pass,DisplayName, EmCode);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateUser(string UserName, string EmCode, string disName)
        {
            string query = string.Format("UPDATE [dbo].[Users] SET [EmCode] = N'{1}',[DisplayName] = N'{2}' WHERE  [UserName] = N'{0}'", UserName, EmCode,disName);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool RePassWordUser(string UserName, string Pass)
        {
            string query = string.Format("UPDATE [dbo].[Users] SET [Pass] = N'{1}' WHERE  [UserName] = N'{0}'", UserName, Pass);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteUser(string userName)
        {
            string query = string.Format("Delete Users Where UserName = N'{0}'",userName);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        #endregion
        #region Permision
        public bool InsertPermission(int IdUser, string NamePer,string DescriptionPer,int IdGroup)
        {
            string query = string.Format("INSERT INTO [dbo].[Permision] ([IdUser],[NamePer],[DescriptionPer],[IdGroup]) VALUES ('{0}',N'{1}',N'{2}','{3}')", IdUser, NamePer, DescriptionPer, IdGroup);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdatePermission(int idUser, int idGroup)
        {
            string query = string.Format("UPDATE [dbo].[Permision] SET [IdGroup] = '{1}' WHERE [IdUser] = '{0}'",idUser,idGroup);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeletePermission(int idUser)
        {
            string query = string.Format("DELETE Permision WHERE IdUser = '{0}'",idUser);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        #endregion
    }
}
