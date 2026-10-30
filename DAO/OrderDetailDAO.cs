using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class OrderDetailDAO
    {
        private static OrderDetailDAO instance;

        public static OrderDetailDAO Instance
        {
            get {
                if (instance == null) instance = new OrderDetailDAO();
                return instance;
            }
            set => instance = value;
        }
        public List<OrderDetailDTO> GetListOrderDetail()
        {
            List<OrderDetailDTO> listO = new List<OrderDetailDTO>();
            DataTable data = DataProvider.Instance.ExecuteQuery("	SELECT IdOrd,IdOrder,OrderDetail.PartCode,PartName,QuantityOr,PurPose,Reason,Note,StatusODetail from OrderDetail,Part WHERE Part.PartCode = OrderDetail.PartCode");
            foreach (DataRow item in data.Rows)
            {
                OrderDetailDTO o = new OrderDetailDTO(item);
                listO.Add(o);
            }
            return listO;
        }
        public List<OrderDetailDTO> GetListOrderDetailById(long idOrder)
        {
            List<OrderDetailDTO> listO = new List<OrderDetailDTO>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT IdOrd,IdOrder,OrderDetail.PartCode,PartName,QuantityOr,PurPose,Reason,Note,StatusODetail from OrderDetail,Part WHERE Part.PartCode = OrderDetail.PartCode AND IdOrder = " + idOrder);
            foreach (DataRow item in data.Rows)
            {
                OrderDetailDTO o = new OrderDetailDTO(item);
                listO.Add(o);
            }
            return listO;
        }
        public OrderDetailDTO OrderDetailByIdOrder(long idOrder)
        {
            return  GetListOrderDetail().FirstOrDefault(x => x.IdOrder == idOrder);
        }
        public bool InsertOrderDetail(long IdOrder, string PartCode, float QuantityOr, string PurPose, string Reason, string Note, int status)
        {
            string query = string.Format("INSERT INTO [dbo].[OrderDetail] ([IdOrder],[PartCode],[QuantityOr],[PurPose],[Reason],[Note],[StatusODetail]) VALUES ({0},N'{1}',{2},N'{3}',N'{4}',N'{5}','{6}')", IdOrder,PartCode,QuantityOr,PurPose,Reason,Note, status);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateOrderDetail(long IdOrd,long IdOrder, string PartCode, float QuantityOr, string PurPose, string Reason, string Note,int status)
        {
            string query = string.Format("UPDATE [dbo].[OrderDetail] SET [IdOrder] = '{1}',[PartCode] = N'{2}',[QuantityOr] = '{3}',[PurPose] = N'{4}',[Reason] = N'{5}',[Note] = N'{6}' , [StatusODetail] = '{7}' WHERE IdOrd = {0}", IdOrd, IdOrder, PartCode, QuantityOr, PurPose, Reason, Note, status);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateNoteOrderDetail(long IdOrd, string Note)
        {
            string query = string.Format("UPDATE [dbo].[OrderDetail] SET [Note] = N'{1}'  WHERE IdOrd = {0}", IdOrd,Note);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteOrderDetail(long IdOrd)
        {
            string query = string.Format("Delete OrderDetail Where IdOrd = "+IdOrd);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteOrderDetailByOrder(long IdOrd)
        {
            string query = string.Format("Delete OrderDetail Where IdOrder = " + IdOrd);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
