using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CarShowroom.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // 🔹 تحقق من إدخال المستخدم
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "يرجى إدخال اسم المستخدم وكلمة المرور.";
                return Page();
            }

            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=CarShowroomDB;Integrated Security=True"))
                {
                    con.Open();
                    string query = "SELECT * FROM Users WHERE Username=@Username AND Password=@Password";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Username", Username);
                        cmd.Parameters.AddWithValue("@Password", Password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                HttpContext.Session.SetString("User", Username);
                                return Redirect("/Index");
                            }
                            else
                            {
                                ErrorMessage = "اسم المستخدم أو كلمة المرور غير صحيحة.";
                                return Page();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "حدث خطأ أثناء الاتصال بقاعدة البيانات: " + ex.Message;
                return Page();
            }
        }
    }
}
