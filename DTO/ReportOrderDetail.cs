using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public class ReportOrderDetail
    {
        public ReportOrderDetail(string IdOrder, string PartCode, string PartName, float QuantityOr, string Unit, string PurPose, string Reason,string DateCreate, string PersonCreate, string DateAppro, string PersonAppro,string Note,string PersonConfirm,string DateConfirm)
        {
            this.IdOrder = IdOrder;
            this.PartCode = PartCode;
            this.PartName = PartName;
            this.QuantityOr = QuantityOr;
            this.Unit = Unit;
            this.PurPose = PurPose;
            this.Reason = Reason;
            this.DateCreate = DateCreate;
            this.PersonCreate = PersonCreate;
            this.DateAppro = DateAppro;
            this.PersonAppro = PersonAppro;
            this.Note = Note;
            this.PersonConfirm = PersonConfirm;
            this.DateConfirm = DateConfirm;
        }
        public ReportOrderDetail(DataRow row)
        {
            this.IdOrder = row["IdOrder"].ToString();
            this.PartCode = row["PartCode"].ToString();
            this.PartName = row["PartName"].ToString();
            this.QuantityOr = (float)Convert.ToDouble(row["QuantityOr"].ToString());
            this.Unit = row["Unit"].ToString();
            this.PurPose = row["PurPose"].ToString();
            this.Reason = row["Reason"].ToString();
            this.DateCreate = row["DateCreate"].ToString();
            this.PersonCreate = row["PersonCreate"].ToString();
            this.DateAppro = row["DateAppro"].ToString();
            this.PersonAppro = row["PersonAppro"].ToString();
            this.Note = row["Note"].ToString();
            this.PersonConfirm = row["PersonConfirm"].ToString();
            this.DateConfirm = row["DateConfirm"].ToString();
        }
        private string idOrder;
        private string partCode;
        private string partName;
        private float quantityOr;
        private string unit;
        private string purPose;
        private string reason;
        private string dateCreate;
        private string personCreate;
        private string dateAppro;
        private string personAppro;
        private string note;
        private string personConfirm;
        private string dateConfirm;


        public string IdOrder { get => idOrder; set => idOrder = value; }
        public string PartCode { get => partCode; set => partCode = value; }
        public string PartName { get => partName; set => partName = value; }
        public float QuantityOr { get => quantityOr; set => quantityOr = value; }
        public string Unit { get => unit; set => unit = value; }
        public string PurPose { get => purPose; set => purPose = value; }
        public string Reason { get => reason; set => reason = value; }
        public string DateCreate { get => dateCreate; set => dateCreate = value; }
        public string PersonCreate { get => personCreate; set => personCreate = value; }
        public string DateAppro { get => dateAppro; set => dateAppro = value; }
        public string PersonAppro { get => personAppro; set => personAppro = value; }
        public string Note { get => note; set => note = value; }
        public string PersonConfirm { get => personConfirm; set => personConfirm = value; }
        public string DateConfirm { get => dateConfirm; set => dateConfirm = value; }
    }
}
