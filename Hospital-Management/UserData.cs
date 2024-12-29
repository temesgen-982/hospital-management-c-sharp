using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management
{
    class UserData
    {
        SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=hospitalDatabase; Integrated Security=True;");

        public UserData()
        {
        }

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
                    "SELECT * FROM Services",
                    "SELECT * FROM Appointments", // Keeping if needed for relationships
                    "SELECT * FROM MedicalRecords", // Keeping if needed for relationships
                    "SELECT * FROM Billing" // Keeping if needed for relationships
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
                // For example, Users -> Doctors, Nurses, Receptionists, etc.
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

                // Appointments and MedicalRecords relationships can also be defined if needed
                dataSet.Relations.Add(new DataRelation(
                    "DoctorsAppointments",
                    dataSet.Tables["Doctors"].Columns["doctor_id"],
                    dataSet.Tables["Appointments"].Columns["doctor_id"]
                ));

                dataSet.Relations.Add(new DataRelation(
                    "PatientsMedicalRecords",
                    dataSet.Tables["Patients"].Columns["patient_id"],
                    dataSet.Tables["MedicalRecords"].Columns["patient_id"]
                ));

                // You can continue to add relationships as necessary
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

        public void LoadUsersIntoDataGridView(DataSet dataSet, System.Windows.Forms.DataGridView dataGridView)
        {
            try
            {
                dataGridView.DataSource = dataSet.Tables["Users"];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading Users table: {ex.Message}");
            }
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

                    int userId = (int) (command.ExecuteScalar());

                    AssignToRole(connection, userId, role, firstName, lastName, username, dob, phone, email);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the user: {ex.Message}");
            }
        }

        // Assign the user to the respective role-specific table
        private void AssignToRole(SqlConnection connection, int userId, string role, string firstName, string lastName, string username, DateTime dob, string phone, string email)
        {
            string roleTable = string.Empty;

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
                    throw new ArgumentException("Invalid role selected");
            }

            string roleQuery = $@"
        INSERT INTO {roleTable} (user_id, first_name, last_name, dob, email, phone, username)
        VALUES (@UserId, @FirstName, @LastName, @DateOfBirth, @Email, @Phone, @Username);";

            using (SqlCommand roleCommand = new SqlCommand(roleQuery, connection))
            {
                roleCommand.Parameters.AddWithValue("@UserId", userId);
                roleCommand.Parameters.AddWithValue("@FirstName", firstName);
                roleCommand.Parameters.AddWithValue("@LastName", lastName);
                roleCommand.Parameters.AddWithValue("@DateOfBirth", dob);
                roleCommand.Parameters.AddWithValue("@Email", email);
                roleCommand.Parameters.AddWithValue("@Phone", phone);
                roleCommand.Parameters.AddWithValue("@Username", username);
                roleCommand.ExecuteNonQuery();
            }
        }

        public void UpdateUser(int userId, string firstName, string lastName, string username, string email, string phone, DateTime dob, string password, string role)
        {
            try
            {
                connection.Open();

                string query = "UPDATE Users SET first_name = @FirstName, last_name = @LastName, username = @Username, email = @Email, phone = @Phone, dob = @Dob, password = @Password, role = @Role WHERE user_id = @UserId";
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

                // Delete dependent records
                //string deleteAppointmentsQuery = "DELETE FROM Appointments WHERE doctor_id = @UserId";
                //string deleteMedicalRecordsQuery = "DELETE FROM MedicalRecords WHERE doctor_id = @UserId";
                //string deleteBillingQuery = "DELETE FROM Billing WHERE patient_id IN (SELECT patient_id FROM Patients WHERE user_id = @UserId)";

                //using (SqlCommand command = new SqlCommand(deleteAppointmentsQuery, connection))
                //{
                //    command.Parameters.AddWithValue("@UserId", userId);
                //    command.ExecuteNonQuery();
                //}

                //using (SqlCommand command = new SqlCommand(deleteMedicalRecordsQuery, connection))
                //{
                //    command.Parameters.AddWithValue("@UserId", userId);
                //    command.ExecuteNonQuery();
                //}

                //using (SqlCommand command = new SqlCommand(deleteBillingQuery, connection))
                //{
                //    command.Parameters.AddWithValue("@UserId", userId);
                //    command.ExecuteNonQuery();
                //}

                // Finally, delete the user
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

        public DataTable searchUser(string searchTerm)
        {

            DataTable dataTable = new DataTable();

            string connectionString = "Server=localhost\\SQLEXPRESS;Database=hospitalDatabase;Integrated Security=True;";
            string query = "SELECT * FROM Users WHERE first_name LIKE @SearchTerm";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add the search term parameter, use '%' for partial matching
                        command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        dataAdapter.Fill(dataTable);

                        if (dataTable.Rows.Count > 0)
                        {
                            return dataTable;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error ");
            }

            return dataTable;
        }
    }
}