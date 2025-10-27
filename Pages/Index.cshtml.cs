using CarShowroom.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CarShowroom.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DatabaseHelper db = new DatabaseHelper();

        public int CarsCount { get; set; }
        public int SalesCount { get; set; }
        public int CustomersCount { get; set; }

        public void OnGet()
        {
            // 1- الاتصال بقاعدة البيانات وجلب الإحصاءات
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();

                // 2- عدد السيارات
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Cars", con))
                    CarsCount = (int)cmd.ExecuteScalar();

                // 3- عدد المبيعات
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Sales", con))
                    SalesCount = (int)cmd.ExecuteScalar();

                // 4- عدد العملاء
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Customers", con))
                    CustomersCount = (int)cmd.ExecuteScalar();
            }
         }


        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return Redirect("/Login");
        }

    }
}
