using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace TodoSoft.Pages.Todo
{
    public class editModel : PageModel
    {
        public Todo todo = new Todo(); 
        public string errorMessage = "";
        public string successMessage = "";
        string connectionString = "Data Source=MAG\\SQLEXPRESS;Initial Catalog=TuesdayDB;Integrated Security=True";
        public void OnGet()
        {
            string id = Request.Query["id"];
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM todos where id = @id";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            todo.Id = reader.GetString(0);
                            todo.title = reader.GetString(1);
                            todo.description = reader.GetString(2);
                            todo.updatedAt = reader.GetDateTime(4);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                Console.WriteLine(ex);
            }


        }

        public void OnPost()
        {
            string id = Request.Query["id"];
            todo.title = Request.Form["title"];
            todo.description = Request.Form["description"];
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "Update todos set title=@title, description=@desc, updateAt=@updatedAt where id=@id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@title", todo.title);
                        cmd.Parameters.AddWithValue("@desc", todo.description);
                        cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now);
                        cmd.ExecuteNonQuery();
                        Response.Redirect("../../todo");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


        }
    }
}
