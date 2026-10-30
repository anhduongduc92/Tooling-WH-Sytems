using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class EmployesDTO
    {
        public EmployesDTO(int Id, string EmployessCode, string EmployessName, string Email, string NameGr)
        {
            this.Id = Id;
            this.EmployessCode = EmployessCode;
            this.EmployessName = EmployessName;
            this.Email = Email;
            this.NameGr = NameGr;
        }
        public EmployesDTO(DataRow row)
        {
            this.Id = (int)row["Id"];
            this.EmployessCode = row["EmployessCode"].ToString();
            this.EmployessName = row["EmployessName"].ToString();
            this.Email = row["Email"].ToString();
            this.NameGr = row["NameGr"].ToString();
        }
        private int id;
        private string employessCode;
        private string employessName;
        private string email;
        private string nameGr;

        public int Id { get => id; set => id = value; }
        public string EmployessCode { get => employessCode; set => employessCode = value; }
        public string EmployessName { get => employessName; set => employessName = value; }
        public string Email { get => email; set => email = value; }
        public string NameGr { get => nameGr; set => nameGr = value; }
    }
}
