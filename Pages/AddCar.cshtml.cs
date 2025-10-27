using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using CarShowroom.Data;

namespace CarShowroom.Pages
{

    [IgnoreAntiforgeryToken(Order = 1001)]
    public class AddCarModel : PageModel
    {
        private readonly DatabaseHelper db = new DatabaseHelper();

        [BindProperty]
        public string Model { get; set; }
        [BindProperty]
        public int Year { get; set; }
        [BindProperty]
        public decimal Price { get; set; }
        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            string fileName = Path.GetFileName(ImageFile.FileName);
            string uploadPath = Path.Combine("wwwroot/Images", fileName);

            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                ImageFile.CopyTo(stream);
            }

            string dbImagePath = "/Images/" + fileName;

            using (SqlConnection con = db.GetConnection())
            {
                try
                {
                    con.Open();
                    string query = "INSERT INTO Cars (Model, Year, Price, ImageUrl) VALUES (@Model, @Year, @Price, @ImageUrl)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Model", Model);
                        cmd.Parameters.AddWithValue("@Year", Year);
                        cmd.Parameters.AddWithValue("@Price", Price);
                        cmd.Parameters.AddWithValue("@ImageUrl", dbImagePath);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch(Exception ex)
                {
                    // Handle exception (log it, show error message, etc.)
                    ModelState.AddModelError(string.Empty, "An error occurred while adding the car. is: " + ex.Message);
                    return Page();
                }
            }


            TempData["SuccessMessage"] = "تمت إضافة السيارة بنجاح 🚗";


            return Redirect("/Cars");
        }
    }
}
