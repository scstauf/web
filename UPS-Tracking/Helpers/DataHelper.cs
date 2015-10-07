using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace UPSTracker.Helpers
{
    public class DataHelper
    {
        public static DataTable GetDT(string sql)
        {
            var connectionString = "someconnectionstring"; 
            //ConfigurationManager.ConnectionStrings["SomeConnectionString"].ConnectionString;
            SqlConnection conn = null;
            DataTable dt = null;

            try
            {
                using (conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (var adapter = new SqlDataAdapter(sql, conn))
                    {
                        dt = new DataTable();
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception e)
            {
                Helpers.ErrorHelper.Log(e);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

            return dt;
        }
    }
}
