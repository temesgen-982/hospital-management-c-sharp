using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System;
using System.Data;
using System.Data.SqlClient;

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

                // Define queries for each relevant table
                string[] tableQueries = new string[]
                {
                    "SELECT * FROM Users",
                    "SELECT * FROM Doctors",
                    "SELECT * FROM Nurses",
                    "SELECT * FROM Receptionists",
                    "SELECT * FROM BillingOfficers",
                    "SELECT * FROM Admins",
                    "SELECT * FROM Patients",
                    "SELECT * FROM Appointments",
                    "SELECT * FROM MedicalRecords",
                    "SELECT * FROM Billing"
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

                // Define relationships between tables
                dataSet.Relations.Add(new DataRelation(
                    "UsersDoctors",
                    dataSet.Tables["Users"].Columns["user_id"],
                    dataSet.Tables["Doctors"].Columns["user_id"]
                ));

                dataSet.Relations.Add(new DataRelation(
                    "UsersNurses",
                    dataSet.Tables["Users"].Columns["user_id"],
                    dataSet.Tables["Nurses"].Columns["user_id"]
                ));

                dataSet.Relations.Add(new DataRelation(
                    "UsersReceptionists",
                    dataSet.Tables["Users"].Columns["user_id"],
                    dataSet.Tables["Receptionists"].Columns["user_id"]
                ));

                dataSet.Relations.Add(new DataRelation(
                    "UsersBillingOfficers",
                    dataSet.Tables["Users"].Columns["user_id"],
                    dataSet.Tables["BillingOfficers"].Columns["user_id"]
                ));

                dataSet.Relations.Add(new DataRelation(
                    "UsersAdmins",
                    dataSet.Tables["Users"].Columns["user_id"],
                    dataSet.Tables["Admins"].Columns["user_id"]
                ));
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


        public void InsertUser(string firstName, string lastName, string username, string email, string phone, DateTime dob, string password, string role)
        {
            try
            {
                connection.Open();

                // Insert into Users table
                string query = @"
                INSERT INTO Users (first_name, last_name, username, dob, email, phone, password, role)
                VALUES (@FirstName, @LastName, @Username, @DateOfBirth, @Email, @Phone, @Password, @Role);
                SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@DateOfBirth", dob);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Password", password); // Hash password in production
                    command.Parameters.AddWithValue("@Role", role);

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
                case "Nurse":
                    roleTable = "Nurses";
                    break;
                case "Receptionist":
                    roleTable = "Receptionists";
                    break;
                case "Billing Officer":
                    roleTable = "BillingOfficers";
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

        public void UpdateUser(int userId, string firstName, string lastName, string username, string email, string phone, DateTime dob, string password, string role)
        {
            try
            {
                connection.Open();

                // Update the Users table
                string query = @"
        UPDATE Users 
        SET first_name = @FirstName, 
            last_name = @LastName, 
            username = @Username, 
            email = @Email, 
            phone = @Phone, 
            dob = @Dob, 
            password = @Password, 
            role = @Role
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
                    command.Parameters.AddWithValue("@Password", password); // Hash in production
                    command.Parameters.AddWithValue("@Role", role);

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

                // Delete from Users table; dependent rows in role-specific tables are deleted due to cascade
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

    }
}
