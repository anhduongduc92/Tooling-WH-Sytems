using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class DataProvider
    {
        private static DataProvider instance;

        public static DataProvider Instance
        {
            get
            {
                if (instance == null) instance = new DataProvider();
                return DataProvider.instance;
            }
            set => instance = value;
        }
        //private string connectionSTR = @"Data Source = 192.168.3.10,1433;Initial Catalog = ToolWH;User ID=sa;Password=P@ssword090992";

        private string connectionSTR = @"Data Source = .;Initial Catalog = ToolWH;User ID=sa;Password=P@ssword090992";
        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionSTR))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameter != null)
                    {
                        String[] Listpara = query.Split(' ');
                        int i = 0;
                        foreach (string item in Listpara)
                        {
                            if (item.Contains('@'))
                            {
                                cmd.Parameters.AddWithValue(item, parameter[i]);
                                i++;
                            }
                        }
                }
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
                conn.Close();
                }
            }
            return dt;
        }
        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int dt = 0;
            using (SqlConnection conn = new SqlConnection(connectionSTR))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameter != null)
                    {
                        String[] Listpara = query.Split(' ');
                        int i = 0;
                        foreach (string item in Listpara)
                        {
                            if (item.Contains('@'))
                            {
                                cmd.Parameters.AddWithValue(item, parameter[i]);
                                i++;
                            }
                        }
                }
                dt = cmd.ExecuteNonQuery();

                conn.Close();
                }
            }
            return dt;
        }
        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object dt = 0;
            using (SqlConnection conn = new SqlConnection(connectionSTR))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameter != null)
                    {
                        String[] Listpara = query.Split(' ');
                        int i = 0;
                        foreach (string item in Listpara)
                        {
                            if (item.Contains('@'))
                            {
                                cmd.Parameters.AddWithValue(item, parameter[i]);
                                i++;
                            }
                        }
                }
                dt = cmd.ExecuteScalar();
                conn.Close();
                }
            }
            return dt;
        }
        public void BackupDataBase(string query, object[] parameter = null)
        {
          
            using (SqlConnection conn = new SqlConnection(connectionSTR))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
        /// <summary>
        /// Sao lưu bằng I/O bất đồng bộ; giữ nguyên câu SQL và thời gian chờ mặc định.
        /// </summary>
        public async Task BackupDataBaseAsync(string query)
        {
            using (SqlConnection conn = new SqlConnection(connectionSTR))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                await conn.OpenAsync().ConfigureAwait(false);
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

    }
}
