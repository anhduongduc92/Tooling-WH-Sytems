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
    public class NewOrderDAO
    {
        private static NewOrderDAO instance;

        public static NewOrderDAO Instance
        {
            get
            {
                if (instance == null) instance = new NewOrderDAO();
                return instance;
            }
            set => instance = value;
        }
        #region NewOrder
        public List<NewOrderDTO> GetListNewOrder()
        {
            List<NewOrderDTO> listO = new List<NewOrderDTO>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * from NewOrder");
            foreach (DataRow item in data.Rows)
            {
                NewOrderDTO n = new NewOrderDTO(item);
                listO.Add(n);
            }
            return listO;
        }
        public NewOrderDTO GetItemNewOrder(long id)
        {
            return GetListNewOrder().FirstOrDefault(x => x.Id == id);
        }
        public long MaxIdNewOrder()
        {
            return long.Parse(DataProvider.Instance.ExecuteScalar("Select MAX(Id) from NewOrder").ToString());
        }
        public bool InsertNewOrder(string NameOrder, string PersonCreate,DateTime DateCreate, int StatusOrder,string Note,string Room)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-CA");
            string query = string.Format("INSERT INTO [dbo].[NewOrder]([NameOrder],[PersonCreate],[DateCreate],[StatusOrder],[Note],[Room]) VALUES (N'{0}',N'{1}','{2}',{3},N'{4}',N'{5}')",NameOrder,PersonCreate,DateCreate,StatusOrder,Note,Room);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateStatusNOD(long id, int status)
        {
            string query = string.Format("UPDATE NewOrder SET StatusOrder = {1}  WHERE Id = {0}",id,status);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteNOD(long id)
        {
            string query = string.Format("DELETE NewOrder WHERE Id = {0}",id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        #endregion
        #region NewOrderDetail
        public List<NewOrderDetail> GetListNewOrd()
        {
            List<NewOrderDetail> listN = new List<NewOrderDetail>();
            DataTable data = DataProvider.Instance.ExecuteQuery("Select * From NewOrderDetail");
            foreach (DataRow item in data.Rows)
            {
                NewOrderDetail n = new NewOrderDetail(item);
                listN.Add(n);
            }
            return listN;
        }
        public List<NewOrderDetail> GetListNewOrdById(long idOrder)
        {
            return GetListNewOrd().Where(x => x.IdOrder == idOrder).ToList();
        }
        public NewOrderDetail GetItemNewOrd(long id)
        {
            return GetListNewOrd().FirstOrDefault(x => x.Id == id);
        }
        public bool InsertNOrd(long IdOrder,string PartCode, string PartName,float Quantity, string Unit, string PurPose, string Note,int StatusODT,float QuantityOut, string Maker, string DateOrder, string Employess)
        {
            string query = string.Format("INSERT INTO [dbo].[NewOrderDetail] ([IdOrder],[PartCode],[PartName],[Quantity],[Unit],[PurPose],[Note],[StatusODT],[QuantityOut],[Maker],[DateOrder],[Employess]) VALUES ({0},N'{1}',N'{2}',{3},N'{4}',N'{5}',N'{6}',{7},{8},N'{9}',N'{10}',N'{11}')", IdOrder, PartCode, PartName, Quantity, Unit, PurPose, Note, StatusODT, QuantityOut, Maker, DateOrder, Employess);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteNOrd(long id)
        {
            string query = string.Format("DELETE NewOrderDetail WHERE IdOrder = {0}", id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateNOrd(long id, float QuantityOut, DateTime DateOut, int StatusODT, string Employess)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-CA");
            string query = string.Format("UPDATE NewOrderDetail SET QuantityOut = {1}, DateOut = '{2}', StatusODT = {3}, Employess = N'{4}' WHERE Id = {0}", id,QuantityOut,DateOut,StatusODT,Employess);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateNOrdNotConfirm(long id,string PartCode ,string PartName ,float Quantity ,string Unit ,string PurPose,string Note ,string Maker )
        {
            string query = string.Format("UPDATE NewOrderDetail SET PartCode = N'{1}', PartName = N'{2}', Quantity = {3}, Unit = N'{4}', PurPose = N'{5}', Note = N'{6}', Maker = N'{7}' WHERE Id = {0}", id, PartCode, PartName, Quantity, Unit, PurPose, Note, Maker);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        #endregion
    }
}
