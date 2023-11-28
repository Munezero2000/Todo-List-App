using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace TodoSoft.Pages.Todo
{
    public class IndexModel : PageModel
    {
        Todo todo = new Todo();
        public List<Todo> todoList = new List<Todo>();
        public string errorMessage = "";
        public string successMessage = "";
        string connectionString = "Data Source=MAG\\SQLEXPRESS;Initial Catalog=TuesdayDB;Integrated Security=True";
        public void OnGet()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM todos ORDER BY updateAt DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while(reader.Read())
                        {
                            todo = new Todo();
                            todo.Id = reader.GetString(0);
                            todo.title = reader.GetString(1);
                            todo.description = reader.GetString(2);
                            todo.updatedAt = reader.GetDateTime(4);
                           
                            todoList.Add(todo);
                            
                        }
                }
                } }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                Console.WriteLine(ex);
            }


        }

        public void OnPost()
        {
            todo.title = Request.Form["title"];
            todo.description = Request.Form["description"];
            if (todo.title.Length < 3)
            {
                errorMessage = "A title is required for every todo";
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO TODOS (id ,title, description, createAt, updateAt) values (@id, @title, @desc, @createdAt, @updatedAt)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", Guid.NewGuid().ToString());
                        cmd.Parameters.AddWithValue("@title", todo.title);
                        cmd.Parameters.AddWithValue("@desc", todo.description);
                        cmd.Parameters.AddWithValue("@createdAt", DateTime.Now);
                        cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected >= 1)
                        {
                            successMessage = "Todo saved successfully";
                            Response.Redirect("todo");
                        }
                    }

                }
            }catch(Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
    }

    public class Todo
    {
        
       

        public Todo()
        {
        }
        public string Id ;
        public string title { get; set; }
        public string description { get; set; }

        public DateTime  createdAt { get; set; }

        public DateTime updatedAt { get; set; }


    }
}
