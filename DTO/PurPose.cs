using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public class PurPose
    {
        public PurPose(int Id,string PPName)
        {
            this.Id = Id;
            this.PPName = PPName;
        }
        public PurPose(DataRow row)
        {
            this.Id = (int)row["Id"];
            this.PPName = row["PPName"].ToString();
        }
        private int id;
        private string pPName;

        public int Id { get => id; set => id = value; }
        public string PPName { get => pPName; set => pPName = value; }
    }
}
