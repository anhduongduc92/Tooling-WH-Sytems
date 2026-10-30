using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public class SupplierDTO
    {
        public SupplierDTO(int Id,string SupplierCode,string SupplierName,string Address, string Phone, string Email)
        {
            this.Id = Id;
            this.SupplierCode = SupplierCode;
            this.SupplierName = SupplierName;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
        }
        public SupplierDTO(DataRow row)
        {
            this.Id = (int)row["Id"];
            this.SupplierCode = row["SupplierCode"].ToString();
            this.SupplierName = row["SupplierName"].ToString();
            this.Address = row["Address"].ToString();
            this.Phone = row["Phone"].ToString();
            this.Email = row["Email"].ToString();
        }
        private int id;
        private string supplierCode;
        private string supplierName;
        private string address;
        private string phone;
        private string email;

        public int Id { get => id; set => id = value; }
        public string SupplierCode { get => supplierCode; set => supplierCode = value; }
        public string SupplierName { get => supplierName; set => supplierName = value; }
        public string Address { get => address; set => address = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Email { get => email; set => email = value; }
    }
}
