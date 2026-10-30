using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public class OrderDTO
    {
        public OrderDTO(long IdOrder,string NameOrder,DateTime DateCreate,string PersonCreate,string PersonAppro ,int StatusOrder,DateTime? DateAppro,string Note, string Room,string PersonConfirm, DateTime? DateConfirm,string Factory)
        {
            this.IdOrder = IdOrder;
            this.NameOrder = NameOrder;
            this.DateCreate = DateCreate;
            this.PersonCreate = PersonCreate;
            this.PersonAppro = PersonAppro;
            this.StatusOrder = StatusOrder;
            this.DateAppro = DateAppro;
            this.Note = Note;
            this.Room = Room;
            this.PersonConfirm = PersonConfirm;
            this.DateConfirm = DateConfirm;
            this.Factory = Factory;
        }
        public OrderDTO(DataRow row)
        {
            this.IdOrder = (long)row["IdOrder"];
            this.NameOrder = row["NameOrder"].ToString();
            this.DateCreate = (DateTime)row["DateCreate"];
            this.PersonCreate = row["PersonCreate"].ToString();
            this.PersonAppro = row["PersonAppro"].ToString();
            this.StatusOrder = (int)row["StatusOrder"];
            var CheckDate = row["DateAppro"];
            if(CheckDate.ToString().Length !=0)
            {
                this.DateAppro = (DateTime)CheckDate;
            }
            this.Note = row["Note"].ToString();
            this.Room = row["Room"].ToString();
            this.PersonConfirm = row["PersonConfirm"].ToString();
            var CheckCon = row["DateConfirm"];
            if(CheckCon.ToString().Length !=0)
            {
                this.DateConfirm = (DateTime)CheckCon;
            }
            this.Factory = row["Factory"].ToString();
        }
        private long idOrder;
        private string nameOrder;
        private DateTime dateCreate;
        private string personCreate;
        private string personAppro;
        private int statusOrder;
        private DateTime? dateAppro;
        private string note;
        private string room;
        private string personConfirm;
        private DateTime? dateConfirm;
        private string factory;

        public long IdOrder { get => idOrder; set => idOrder = value; }
        public string NameOrder { get => nameOrder; set => nameOrder = value; }
        public DateTime DateCreate { get => dateCreate; set => dateCreate = value; }
        public string PersonCreate { get => personCreate; set => personCreate = value; }
        public string PersonAppro { get => personAppro; set => personAppro = value; }
        public int StatusOrder { get => statusOrder; set => statusOrder = value; }
        public DateTime? DateAppro { get => dateAppro; set => dateAppro = value; }
        public string Note { get => note; set => note = value; }
        public string Room { get => room; set => room = value; }
        public string PersonConfirm { get => personConfirm; set => personConfirm = value; }
        public DateTime? DateConfirm { get => dateConfirm; set => dateConfirm = value; }
        public string Factory { get => factory; set => factory = value; }
    }
}
