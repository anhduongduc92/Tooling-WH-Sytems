using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public class Reason
    {
        public Reason(int Id, string RName)
        {
            this.Id = Id;
            this.RName = RName;
        }
        public Reason(DataRow row)
        {
            this.Id = (int)row["Id"];
            this.RName = row["RName"].ToString();
        }
        private int id;
        private string rName;

        public int Id { get => id; set => id = value; }
        public string RName { get => rName; set => rName = value; }
    }
}
