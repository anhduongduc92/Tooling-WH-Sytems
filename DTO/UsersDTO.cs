using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public class UsersDTO
    {
        public UsersDTO (int IdUser, string UserName, string Pass, string DisplayName, string EmCode)
        {
            this.IdUser = IdUser;
            this.UserName = UserName;
            this.Pass = Pass;
            this.DisplayName = DisplayName;
            this.EmCode = EmCode;
        }
        public UsersDTO(DataRow row)
        {
            this.IdUser = (int)row["IdUser"];
            this.UserName = row["UserName"].ToString();
            this.Pass = row["Pass"].ToString();
            this.DisplayName = row["DisplayName"].ToString();
            this.EmCode = row["EmCode"].ToString();
        }
        private int idUser;
        private string userName;
        private string pass;
        private string displayName;
        private string emCode;

        public int IdUser { get => idUser; set => idUser = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Pass { get => pass; set => pass = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public string EmCode { get => emCode; set => emCode = value; }
    }
}
