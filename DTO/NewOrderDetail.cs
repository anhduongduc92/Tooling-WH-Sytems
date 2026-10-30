using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class NewOrderDetail
    {
        public NewOrderDetail(long Id, long IdOrder, string PartCode, string PartName, float Quantity,string Unit, string PurPose, string Note, int StatusODT,DateTime? DateOut,float QuantityOut,string Maker,string DateOrder,string Employess)
        {
            this.Id = Id;
            this.IdOrder = IdOrder;
            this.PartCode = PartCode;
            this.PartName = PartName;
            this.Quantity = Quantity;
            this.PurPose = PurPose;
            this.Maker = Maker;
            this.Note = Note;
            this.StatusODT = StatusODT;
            this.Unit = Unit;
            this.DateOut = DateOut;
            this.QuantityOut = QuantityOut;
            this.DateOrder = DateOrder;
            this.Employess = Employess;
        }
        public NewOrderDetail(DataRow row)
        {
            this.Id = (long)row["Id"];
            this.IdOrder = (long)row["IdOrder"];
            this.PartCode = row["PartCode"].ToString();
            this.PartName = row["PartName"].ToString();
            this.Quantity = (float)Convert.ToDouble(row["Quantity"].ToString());
            this.PurPose = row["PurPose"].ToString();
            this.Unit = row["Unit"].ToString();
            this.Note = row["Note"].ToString();
            this.StatusODT = (int)row["StatusODT"];
            this.Maker = row["Maker"].ToString();
            var checkDate = row["DateOut"];
            if (checkDate.ToString().Length != 0)
            {
                this.DateOut = (DateTime)checkDate;
            }
            this.QuantityOut = (float)Convert.ToDouble(row["QuantityOut"].ToString());
            this.DateOrder = row["DateOrder"].ToString();
            this.Employess = row["Employess"].ToString();
        }
        private long id;
        private long idOrder;
        private string partCode;
        private string partName;
        private float quantity;
        private string purPose;
        private string maker;
        private string note;
        private string unit;
        private int statusODT;
        private DateTime? dateOut;
        private float quantityOut;
        private string dateOrder;
        private string employess;

        public long Id { get => id; set => id = value; }
        public long IdOrder { get => idOrder; set => idOrder = value; }
        public string PartCode { get => partCode; set => partCode = value; }
        public float Quantity { get => quantity; set => quantity = value; }
        public string PurPose { get => purPose; set => purPose = value; }
      
        public string Note { get => note; set => note = value; }
      
        public string PartName { get => partName; set => partName = value; }
        public string Maker { get => maker; set => maker = value; }
        public int StatusODT { get => statusODT; set => statusODT = value; }
        public DateTime? DateOut { get => dateOut; set => dateOut = value; }
        public float QuantityOut { get => quantityOut; set => quantityOut = value; }
        public string DateOrder { get => dateOrder; set => dateOrder = value; }
        public string Unit { get => unit; set => unit = value; }
        public string Employess { get => employess; set => employess = value; }
    }
}
