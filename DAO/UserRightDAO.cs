using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class UserRightDAO
    {
        private static UserRightDAO instance;

        public static UserRightDAO Instance
        {
            get
            {
                if (instance == null) instance = new UserRightDAO();
                return instance;
            }
            set => instance = value;
        }
        public string NameUserRight(int IdUser,string FunCode)
        {
            try
            {
                return DataProvider.Instance.ExecuteScalar("SELECT CASE UserRight WHEN 0 THEN N'Cấm quyền' WHEN 1 THEN N'Chỉ xem' WHEN 2 THEN N'Thêm, Sửa' WHEN 3 THEN N'Xác nhận' WHEN 4 THEN N'Toàn quyền' END AS NameUserRight from User_Right, FunctionDetail WHERE FunCode = FunctionDetail.CodeFuc AND IdUser = " + IdUser + " AND FunCode = N'" + FunCode + "'").ToString();
            }
            catch 
            {
              return  "Cấm quyền";
            }
        }
        public bool UpdateUserRight(int IdUser,string FunCode, int UserRight)
        {
            string query = string.Format("UPDATE User_Right SET UserRight = {2} WHERE FunCode = N'{1}' AND IdUser = {0}", IdUser, FunCode, UserRight);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public int UserRight(int IdUser, string FunCode)
        {
            try
            {
                return int.Parse(DataProvider.Instance.ExecuteScalar("SELECT UserRight from User_Right Where IdUser = "+IdUser+" AND FunCode = N'"+FunCode+"'").ToString());
            }
            catch 
            {
                return 0;
            }
        }
    }
}
