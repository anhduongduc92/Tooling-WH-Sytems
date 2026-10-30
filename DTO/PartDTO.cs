using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public class PartDTO
    {
        public PartDTO(long Id, string PartCode, string PartName, 
            string Unit, float MinWaint, float MaxWaint,int StatusPart,int SpecPart)
        {
            this.Id = Id;
            this.PartCode = PartCode;
            this.PartName = PartName;
            this.Unit = Unit;
            this.MinWaint = MinWaint;
            this.MaxWaint = MaxWaint;
            this.StatusPart = StatusPart;
            this.SpecPart = SpecPart;
        }
        public PartDTO(DataRow row)
        {
            this.Id = (long)row["Id"];
            this.PartCode = row["PartCode"].ToString();
            this.PartName = row["PartName"].ToString();
            this.Unit = row["Unit"].ToString();
            this.MinWaint = (float)Convert.ToDouble(row["MinWaint"].ToString());
            this.MaxWaint = (float)Convert.ToDouble(row["MaxWaint"].ToString());
            this.StatusPart = (int)row["StatusPart"];
            this.SpecPart = (int)row["SpecPart"];
        }
        private long id;
        private string partCode;
        private string partName;
        private string unit;
        private float minWaint;
        private float maxWaint;
        private int statusPart;
        private int specPart;

        public long Id { get => id; set => id = value; }
        public string PartCode { get => partCode; set => partCode = value; }
        public string PartName { get => partName; set => partName = value; }
        public string Unit { get => unit; set => unit = value; }
        public float MinWaint { get => minWaint; set => minWaint = value; }
        public float MaxWaint { get => maxWaint; set => maxWaint = value; }
        public int StatusPart { get => statusPart; set => statusPart = value; }
        public int SpecPart { get => specPart; set => specPart = value; }
    }
}
