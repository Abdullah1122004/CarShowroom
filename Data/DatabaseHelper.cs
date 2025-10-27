using System;
using System.Data;
using System.Data.SqlClient;

namespace CarShowroom.Data
{
    public class DatabaseHelper
    {
        private string connectionString = @"Data Source=.;Initial Catalog=CarShowroomDB;Integrated Security=True";

        // 🔹 ترجع اتصال جاهز
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }




        // 🔹 تُنفذ أوامر مثل INSERT / UPDATE / DELETE
        public void Execute(string query)
        {
            using (SqlConnection con = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        


        // 🔹 ترجع قيمة واحدة فقط (مثلاً COUNT أو اسم مستخدم)
        public object GetSingleValue(string query)
        {
            using (SqlConnection con = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    return cmd.ExecuteScalar(); // ترجع أول قيمة في أول صف
                }
            }
        }

     


        // 🔹 ترجع كل الصفوف (وتستخدم SqlDataReader)
        public DataTable GetData(string query)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader); // تحميل كل البيانات من القارئ إلى الجدول
                    }
                }
            }
            return dt;
        }
    }
}
