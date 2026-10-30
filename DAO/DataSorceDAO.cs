using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
   public class DataSorceDAO
    {
        private static DataSorceDAO instance;

        public static DataSorceDAO Instance
        { get
            {
                if (instance == null) instance = new DataSorceDAO();
                return instance;
            }
            set => instance = value;
        }
        #region PurPose

        public List<PurPose> GetListPurPose()
        {
            List<PurPose> listP = new List<PurPose>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM PurPose");
            foreach (DataRow item in data.Rows)
            {
                PurPose p = new PurPose(item);
                listP.Add(p);
            }
            return listP;
        }
        public bool InsertPurpose(string name)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("INSERT INTO PurPose (PPName) VALUES (N'" + name + "') ");
            return result > 0;
        }
        public bool UpdatePurpose(int id,string name)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("UPDATE PurPose SET PPName = N'" + name + "' WHERE Id = " + id);
            return result > 0;
        }
        public bool DeletePurpose(int id)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("DELETE PurPose WHERE Id = " + id);
            return result > 0;
        }
        public int IdPurPose(string name)
        {
            try
            {
               return int.Parse(DataProvider.Instance.ExecuteScalar("SELECT Id FROM PurPose WHERE PPName = N'" + name + "'").ToString());
            }
            catch 
            {
                return -1;
            }
        }
        #endregion
        #region Reason

        public List<Reason> GetListReason()
        {
            List<Reason> listR = new List<Reason>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM Reason");
            foreach (DataRow item in data.Rows)
            {
                Reason r = new Reason(item);
                listR.Add(r);
            }
            return listR;
        }
        public bool InsertReason(string name)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("INSERT INTO Reason (RName) VALUES (N'" + name+"') ");
            return result > 0;
        }
        public bool UpdateReason(int id,string name)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("UPDATE Reason SET RName = N'" + name+"' WHERE Id = "+id);
            return result > 0;
        }
        public bool DeleteReason(int id)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("DELETE Reason WHERE Id = " + id);
            return result > 0;
        }
        public int IdReason(string name)
        {
            try
            {
                return int.Parse(DataProvider.Instance.ExecuteScalar("SELECT Id FROM Reason WHERE RName = N'" + name + "'").ToString());
            }
            catch
            {
                return -1;
            }
        }
        #endregion
    }
}
