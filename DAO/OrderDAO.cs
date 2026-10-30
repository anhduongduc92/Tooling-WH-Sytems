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
    public class OrderDAO
    {
        private static OrderDAO instance;

        public static OrderDAO Instance
        {
            get
            {
                if (instance == null) instance = new OrderDAO();
                return instance;
            }
            set => instance = value;
        }
        public List<OrderDTO> GetListOrder()
        {
            List<OrderDTO> listO = new List<OrderDTO>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM Orders");
            foreach (DataRow item in data.Rows)
            {
                OrderDTO o = new OrderDTO(item);
                listO.Add(o);
            }
            return listO;
        }
        public OrderDTO GetListOrderById(long id)
        {
            return GetListOrder().FirstOrDefault(x => x.IdOrder == id);
        }
        public long MaxIdOrder()
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM Orders ORDER BY IdOrder DESC");
            if(data.Rows.Count>0)
            {
                OrderDTO o = new OrderDTO(data.Rows[0]);
                return o.IdOrder;
            }
            return -1;
        }
        public int StatusOrder(long idOrder)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM Orders WHERE IdOrder = " + idOrder);
            if (data.Rows.Count > 0)
            {
                OrderDTO o = new OrderDTO(data.Rows[0]);
                return o.StatusOrder;
            }
            return -1;
        }
        public bool InsertOrder(string NameOrder,DateTime DateCreate,string PersonCreate,string PersonAppro ,int StatusOrder,string Note,string Room,string Factory)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-CA");
            string query = string.Format("INSERT INTO [dbo].[Orders] ([NameOrder],[DateCreate],[PersonCreate],[PersonAppro] ,[StatusOrder],[Note],[Room],[Factory]) VALUES (N'{0}','{1}',N'{2}',N'{3}',{4},N'{5}',N'{6}',N'{7}')", NameOrder, DateCreate, PersonCreate, PersonAppro, StatusOrder, Note, Room, Factory);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool ApproOrder(long IdOrder, string PersonAppro, int StatusOrder, DateTime DateAppro)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-CA");
            string query = string.Format("UPDATE [dbo].[Orders] SET [PersonAppro] = N'{1}' ,[StatusOrder] = {2},[DateAppro] = '{3}' WHERE IdOrder = {0}",IdOrder,  PersonAppro, StatusOrder, DateAppro);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool ConfirmOrder(long IdOrder, string PersonConfirm, int StatusOrder, DateTime DateConfirm)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-CA");
            string query = string.Format("UPDATE [dbo].[Orders] SET [PersonConfirm] = N'{1}' ,[StatusOrder] = {2},[DateConfirm] = '{3}' WHERE IdOrder = {0}", IdOrder, PersonConfirm, StatusOrder, DateConfirm);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateOrder(long IdOrder,int StatusOrder,string Note)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-CA");
            string query = string.Format("UPDATE [dbo].[Orders] SET [Note] = N'{1}' ,[StatusOrder] = {2} WHERE IdOrder = {0}", IdOrder,Note, StatusOrder);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteOrder(long IdOrder)
        {
            string query = string.Format("DELETE Orders WHERE IdOrder = {0}", IdOrder);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        #region RePort Order Detail
        public List<ReportOrderDetail> GetListRPOrderDetail(long idOrder)
        {
            List<ReportOrderDetail> listR = new List<ReportOrderDetail>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT Orders.IdOrder,DateCreate,PersonCreate,PersonAppro,DateAppro,OrderDetail.PartCode,PartName,QuantityOr,PurPose,Reason,Unit,OrderDetail.Note,PersonConfirm,DateConfirm FROM Orders,OrderDetail,Part WHERE OrderDetail.IdOrder = Orders.IdOrder AND Part.PartCode = OrderDetail.PartCode AND OrderDetail.IdOrder = " + idOrder);
            foreach (DataRow item in data.Rows)
            {
                ReportOrderDetail r = new ReportOrderDetail(item);
                listR.Add(r);
            }
            return listR;
        }
        #endregion
    }
}
