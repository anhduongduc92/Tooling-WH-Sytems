using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public class OrderDetailDTO
    {
        public OrderDetailDTO(long IdOrd, long IdOrder, string PartCode,string PartName, float QuantityOr,string PurPose, string Reason, string Note,int StatusODetail)
        {
            this.IdOrd = IdOrd;
            this.IdOrder = IdOrder;
            this.PartCode = PartCode;
            this.PartName = PartName;
            this.QuantityOr = QuantityOr;
            this.PurPose = PurPose;
            this.Reason = Reason;
            this.Note = Note;
            this.StatusODetail = StatusODetail;
        }
        public OrderDetailDTO(DataRow row)
        {
            this.IdOrd = (long)row["IdOrd"];
            this.IdOrder = (long)row["IdOrder"];
            this.PartCode = row["PartCode"].ToString();
            this.PartName = row["PartName"].ToString();
            this.QuantityOr = (float)Convert.ToDouble(row["QuantityOr"].ToString());
            this.PurPose = row["PurPose"].ToString();
            this.Reason = row["Reason"].ToString();
            this.Note = row["Note"].ToString();
            this.StatusODetail = (int)row["StatusODetail"];
        }
        private long idOrd;
        private long idOrder;
        private string partCode;
        private string partName;
        private float quantityOr;
        private string purPose;
        private string reason;
        private string note;
        private int statusODetail;

        public long IdOrd { get => idOrd; set => idOrd = value; }
        public long IdOrder { get => idOrder; set => idOrder = value; }
        public string PartCode { get => partCode; set => partCode = value; }
        public float QuantityOr { get => quantityOr; set => quantityOr = value; }
        public string PurPose { get => purPose; set => purPose = value; }
        public string Reason { get => reason; set => reason = value; }
        public string Note { get => note; set => note = value; }
        public int StatusODetail { get => statusODetail; set => statusODetail = value; }
        public string PartName { get => partName; set => partName = value; }
    }
}
