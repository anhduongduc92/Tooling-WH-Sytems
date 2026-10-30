using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public class InventoryDTO
    {
        public InventoryDTO(long Id, string PartCode, string PartName, DateTime DateIn, float QuantityIn, float QuantityInventory, string Note, int StatusInput, int StatusIventory, string Unit)
        {
            this.Id = Id;
            this.PartCode = PartCode;
            this.PartName = PartName;
            this.DateIn = DateIn;
            this.QuantityIn = QuantityIn;
            this.QuantityInventory = QuantityInventory;
            this.Note = Note;
            this.StatusInput = StatusInput;
            this.StatusIventory = StatusIventory;
            this.Unit = Unit;
        }
        public InventoryDTO(DataRow row)
        {
            this.Id = (long)row["Id"];
            this.PartCode = row["PartCode"].ToString();
            this.PartName = row["PartName"].ToString();
            this.DateIn = (DateTime)row["DateIn"];
            this.QuantityIn = (float)Convert.ToDouble(row["QuantityIn"].ToString());
            this.QuantityInventory = (float)Convert.ToDouble(row["QuantityInventory"].ToString());
            this.Note = row["Note"].ToString();
            this.StatusInput = (int)row["StatusInput"];
            this.StatusIventory = (int)row["StatusIventory"];
            this.Unit = row["Unit"].ToString();
        }
        private long id;
        private string partCode;
        private string partName;
        private DateTime dateIn;
        private float quantityIn;
        private float quantityInventory;
        private string note;
        private int statusInput;
        private int statusIventory;
        private string unit;

        public long Id { get => id; set => id = value; }
        public string PartCode { get => partCode; set => partCode = value; }
        public string PartName { get => partName; set => partName = value; }
        public DateTime DateIn { get => dateIn; set => dateIn = value; }
        public float QuantityIn { get => quantityIn; set => quantityIn = value; }
        public float QuantityInventory { get => quantityInventory; set => quantityInventory = value; }
        public string Note { get => note; set => note = value; }
        public int StatusInput { get => statusInput; set => statusInput = value; }
        public int StatusIventory { get => statusIventory; set => statusIventory = value; }
        public string Unit { get => unit; set => unit = value; }
    }
}
