using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

using System.IO;
using System.Drawing;

namespace Hospital_Management
{
    class UserData
    {
        SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=hospitalDatabase; Integrated Security=True;");

        public UserData() { }

        public DataSet LoadDatabaseSchemaAndData()
        {
            DataSet dataSet = new DataSet();

            try
            {
                connection.Open();

                // Define updated query for Users table with new fields
                string[] tableQueries = new string[]
                {
                    "SELECT * FROM Users",
                    "SELECT * FROM Doctors",
                    "SELECT * FROM Receptionists",
                    "SELECT * FROM Admins"
                };

                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    foreach (string query in tableQueries)
                    {
                        adapter.SelectCommand = new SqlCommand(query, connection);
                        string tableName = query.Split(' ')[3]; // Extract table name
                        adapter.Fill(dataSet, tableName);
                    }
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

            return dataSet;
        }

        public void LoadUsersIntoDataGridView(DataSet dataSet, System.Windows.Forms.DataGridView dataGrid)
        {
            dataGrid.DataSource = dataSet.Tables["Users"];
        }

        public string GetUserFullName(int userId)
        {
            string fullName = null;

            try
            {
                connection.Open();
                
                string query = @"
                SELECT first_name, last_name 
                FROM Users 
                WHERE user_id = @UserId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string firstName = reader["first_name"].ToString();
                            string lastName = reader["last_name"].ToString();
                            fullName = $"{firstName} {lastName}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the user's full name: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }

            return fullName ?? "User not found";
        }

        public void InsertUser(string firstName, string lastName, string username, string email, string phone, DateTime dob, string password, string role, byte[] profileImage = null)
        {
            try
            {
                connection.Open();

                string query = @"
        INSERT INTO Users (first_name, last_name, username, dob, email, phone, password, role, profile_image)
        VALUES (@FirstName, @LastName, @Username, @Dob, @Email, @Phone, @Password, @Role, @ProfileImage);
        SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Dob", dob);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@ProfileImage", (object)profileImage ?? DBNull.Value);

                    int userId = Convert.ToInt32(command.ExecuteScalar());
                    AssignToRole(connection, userId, role);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while inserting the user: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }


        private void AssignToRole(SqlConnection connection, int userId, string role)
        {
            string roleTable;

            switch (role)
            {
                case "Doctor":
                    roleTable = "Doctors";
                    break;
                case "Receptionist":
                    roleTable = "Receptionists";
                    break;
                case "Admin":
                    roleTable = "Admins";
                    break;
                default:
                    throw new ArgumentException("Invalid role specified");
            }

            string roleQuery = $"INSERT INTO {roleTable} (user_id) VALUES (@UserId);";

            using (SqlCommand roleCommand = new SqlCommand(roleQuery, connection))
            {
                roleCommand.Parameters.AddWithValue("@UserId", userId);
                roleCommand.ExecuteNonQuery();
            }
        }

        public void UpdateUser(int userId, string firstName, string lastName, string username, string email, string phone, DateTime dob, string password, string role, string imagePath = null)
        {
            try
            {
                connection.Open();

                byte[] profileImage = imagePath != null ? File.ReadAllBytes(imagePath) : null;

                string query = @"
                UPDATE Users 
                SET first_name = @FirstName, 
                    last_name = @LastName, 
                    username = @Username, 
                    email = @Email, 
                    phone = @Phone, 
                    dob = @Dob, 
                    password = @Password, 
                    role = @Role,
                    profile_image = @ProfileImage
                WHERE user_id = @UserId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Dob", dob);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@ProfileImage", (object)profileImage ?? DBNull.Value);

                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "User updated successfully." : "No user found with the specified ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the user: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        public void DeleteUser(int userId)
        {
            try
            {
                connection.Open();
                    
                string deleteUserQuery = "DELETE FROM Users WHERE user_id = @UserId";

                using (SqlCommand command = new SqlCommand(deleteUserQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "User deleted successfully." : "No user found with the specified ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the user: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }


        public DataTable SearchUser(string searchTerm)
        {
            DataTable dataTable = new DataTable();
            string query = @"
    SELECT * 
    FROM Users 
    WHERE first_name LIKE @SearchTerm 
       OR last_name LIKE @SearchTerm 
       OR username LIKE @SearchTerm 
       OR email LIKE @SearchTerm 
       OR phone LIKE @SearchTerm";

            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during search: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }

            return dataTable.Rows.Count > 0 ? dataTable : null;
        }

        public Image GetProfileImage(int userId)
        {
            try
            {
                connection.Open();

                string query = "SELECT profile_image FROM Users WHERE user_id = @UserId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        byte[] imageData = (byte[])result;
                        using (var ms = new MemoryStream(imageData))
                        {
                            return Image.FromStream(ms);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the profile image: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }

            return null;
        }

        public int GetReceptionistIdByUserId(int userId)
        {
            int receptionistId = 0;

            string query = "SELECT receptionist_id FROM Receptionists WHERE user_id = @UserId";

            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@UserId", userId);
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        receptionistId = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving receptionist ID: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }

            return receptionistId;
        }

        public int GetDoctorIdByUserId(int userId)
        {
            int doctorId = 0;

            string query = "SELECT doctor_id FROM Doctors WHERE user_id = @UserId";

            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@UserId", userId);
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        doctorId = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving doctor ID: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }

            return doctorId;
        }
    }
}