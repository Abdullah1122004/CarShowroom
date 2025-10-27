using CarShowroom.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CarShowroom.Pages
{
    public class CarsModel : PageModel
    {
        private readonly DatabaseHelper db = new DatabaseHelper();

        public List<Car> CarsList { get; set; } = new List<Car>();


        public void OnGet(string? search)
        {
            using (SqlConnection con = db.GetConnection())
            {
                try
                {
                    con.Open();
                    string query = "SELECT Id, Model, Year, Price, ImageUrl FROM Cars";

                    if (!string.IsNullOrEmpty(search))
                    {
                        query += " WHERE Model LIKE @Search OR CAST(Year AS NVARCHAR) LIKE @Search OR CAST(Price AS NVARCHAR) LIKE @Search";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        if (!string.IsNullOrEmpty(search))
                            cmd.Parameters.AddWithValue("@Search", "%" + search + "%");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CarsList.Add(new Car
                                {
                                    Id = reader.GetInt32(0),
                                    Model = reader.GetString(1),
                                    Year = reader.GetInt32(2),
                                    Price = reader.GetDecimal(3),
                                    ImageUrl = reader.GetString(4)
                                });
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }


        public IActionResult OnPostDelete(int id)
        {
            using (SqlConnection con = db.GetConnection())
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Cars WHERE Id = @Id", con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error : " + ex.Message);
                }


                TempData["SuccessMessage"] = "تم حذف السيارة بنجاح ✅";

                return Redirect("/Cars");
            }
        }
    }

    public class Car
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
