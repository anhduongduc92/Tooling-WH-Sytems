using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public class OnlyPart
    {
        public OnlyPart(string PartCode,string PartName)
        {
            this.PartCode = PartCode;
            this.PartName = PartName;
        }
        public OnlyPart(DataRow row)
        {
            this.PartCode = row["PartCode"].ToString();
            this.PartName = row["PartName"].ToString();
        }
        private string partCode;
        private string partName;
        public string PartCode { get => partCode; set => partCode = value; }
        public string PartName { get => partName; set => partName = value; }
    }
}
