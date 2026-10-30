using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public class InvenPartDTO
    {
        string partCode, partName, unit;
        float total_imported, total_exported, quantInven;
        int statusPart;

        public InvenPartDTO(string partCode, string partName, string unit, 
            float total_imported, float total_exported, float quantInven,int statusPart)
        {
            PartCode = partCode;
            PartName = partName;
            Unit = unit;
            Total_imported = total_imported;
            Total_exported = total_exported;
            QuantInven = quantInven;
            StatusPart = statusPart;
        }
        public InvenPartDTO(DataRow item)
        {
            PartCode = item["PartCode"].ToString();
            PartName = item["PartName"].ToString();
            Unit = item["Unit"].ToString();
            Total_imported = float.Parse(item["Total_imported"].ToString());
            Total_exported = float.Parse(item["Total_exported"].ToString());
            QuantInven = float.Parse(item["QuantInven"].ToString());
            StatusPart = (int)item["StatusPart"];
        }
        public string PartCode { get => partCode; set => partCode = value; }
        public string PartName { get => partName; set => partName = value; }
        public string Unit { get => unit; set => unit = value; }
        public float Total_imported { get => total_imported; set => total_imported = value; }
        public float Total_exported { get => total_exported; set => total_exported = value; }
        public float QuantInven { get => quantInven; set => quantInven = value; }
        public int StatusPart { get => statusPart; set => statusPart = value; }
    }
}
