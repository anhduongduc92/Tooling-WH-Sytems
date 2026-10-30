using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
   public class BackupDAO
    {
        private static BackupDAO instance;

        public static BackupDAO Instance
        { get
            {
                if (instance == null) instance = new BackupDAO();
                return instance;
            }
            set => instance = value;
        }
        public bool BackupDataBase(string dataBase, string disk)
        {

                string query = string.Format("BACKUP DATABASE [{0}] TO DISK = '{1}.BAK'", dataBase,disk);
                DataProvider.Instance.BackupDataBase(query);
                return true;
        }
        /// <summary>
        /// Tạo file .BAK theo cùng quy tắc đặt tên của hàm sao lưu đồng bộ.
        /// </summary>
        public async Task<bool> BackupDataBaseAsync(string dataBase, string disk)
        {
            string query = string.Format("BACKUP DATABASE [{0}] TO DISK = '{1}.BAK'", dataBase, disk);
            await DataProvider.Instance.BackupDataBaseAsync(query).ConfigureAwait(false);
            return true;
        }

    }
}
