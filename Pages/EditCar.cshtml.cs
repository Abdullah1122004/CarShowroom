using CarShowroom.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CarShowroom.Pages
{
    public class EditCarModel : PageModel
    {
        private readonly DatabaseHelper db = new DatabaseHelper();

        [BindProperty]
        public Car Car { get; set; } = new Car();

        public string Message { get; set; }

        public void OnGet(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Cars WHERE Id = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Car.Id = reader.GetInt32(0);
                            Car.Model = reader.GetString(1);
                            Car.Year = reader.GetInt32(2);
                            Car.Price = reader.GetDecimal(3);
                            Car.ImageUrl = reader.GetString(4);
                        }
                    }
                }
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            using (SqlConnection con = db.GetConnection())
            {
               try
               {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        "UPDATE Cars SET Model=@m, Year=@y, Price=@p, ImageUrl=@i WHERE Id=@id", con))
                    {
                        cmd.Parameters.AddWithValue("@m", Car.Model);
                        cmd.Parameters.AddWithValue("@y", Car.Year);
                        cmd.Parameters.AddWithValue("@p", Car.Price);
                        cmd.Parameters.AddWithValue("@i", Car.ImageUrl);
                        cmd.Parameters.AddWithValue("@id", Car.Id);

                        cmd.ExecuteNonQuery();
                    }

               }
               catch (SqlException ex)
               {
                   Message = "❌ حدث خطأ أثناء تحديث بيانات السيارة: " + ex.Message;
                   return Page();
               }
            }

            TempData["SuccessMessage"] = "تم تعديل بيانات السيارة بنجاح ✏️";

            return Redirect("/Cars");
        }
    }
}