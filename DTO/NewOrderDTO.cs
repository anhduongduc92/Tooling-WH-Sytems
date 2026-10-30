using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class NewOrderDTO
    {
        public NewOrderDTO(long Id, string NameOrder, DateTime DateCreate, string PersonCreate, string PersonAppro, int StatusOrder, DateTime? DateAppro, string Note, string Room, string PersonConfirm, DateTime? DateConfirm)
        {
            this.Id = Id;
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
        }
        public NewOrderDTO(DataRow row)
        {
            this.Id = (long)row["Id"];
            this.NameOrder = row["NameOrder"].ToString();
            this.DateCreate = (DateTime)row["DateCreate"];
            this.PersonCreate = row["PersonCreate"].ToString();
            this.PersonAppro = row["PersonAppro"].ToString();
            this.StatusOrder = (int)row["StatusOrder"];
            var CheckDate = row["DateAppro"];
            if (CheckDate.ToString().Length != 0)
            {
                this.DateAppro = (DateTime)CheckDate;
            }
            this.Note = row["Note"].ToString();
            this.Room = row["Room"].ToString();
            this.PersonConfirm = row["PersonConfirm"].ToString();
            var CheckCon = row["DateConfirm"];
            if (CheckCon.ToString().Length != 0)
            {
                this.DateConfirm = (DateTime)CheckCon;
            }
        }
        private long id;
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

        public long Id { get => id; set => id = value; }
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
    }
}
