using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management
{
    class User
    {
        SqlConnection connection = new SqlConnection("Server = localhost; Database = hospitalDatabase; Integrated Security = True;");
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public User(string username, string password, string role)
        {
            Username = username;
            Password = password;
            Role = role;
        }

        public User()
        {

        }

        public List<User> getUsers(){

            List<User> listData = new List<User>();

            try
            {
                connection.Open();
                string query = @"
                                SELECT u.user_id, u.first_name, u.last_name, u.password, r.role_name
                                FROM Users u
                                JOIN Roles r ON u.role_id = r.role_id";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    

                    while (reader.Read())
                    {
                        User user = new User();
                        user.Password = reader["password"].ToString();
                        user.Username = reader["first_name"].ToString();
                        user.Role = reader["role_name"].ToString();

                        listData.Add(user);
                                        }

                        reader.Close();
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }

            return listData;
        }
    };
}