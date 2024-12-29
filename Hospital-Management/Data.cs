using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management
{
    class Data
    {
        SqlConnection connection = new SqlConnection("Server = localhost\\SQLEXPRESS; Database = hospitalDatabase; Integrated Security = True;");

        public Data()
        {
        }

        public DataSet LoadDatabaseSchemaAndData()
        {
            DataSet dataSet = new DataSet();

            try
            {
                connection.Open();

                // Define queries for each table
                string[] tableQueries = new string[]
                {
                    "SELECT * FROM Users",
                    "SELECT * FROM Roles",
                    "SELECT * FROM Departments",
                    "SELECT * FROM Patients",
                    "SELECT * FROM Appointments",
                    "SELECT * FROM MedicalRecords",
                    "SELECT * FROM Billing",
                    "SELECT * FROM Services"
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

                // Define relationships between tables (if any)
                // Example: Users -> Roles
                //DataColumn usersRoleColumn = dataSet.Tables["Users"].Columns["role_id"];
                //DataColumn rolesRoleColumn = dataSet.Tables["Roles"].Columns["role_id"];
                //DataRelation usersRolesRelation = new DataRelation("UsersRoles", rolesRoleColumn, usersRoleColumn);
                //dataSet.Relations.Add(usersRolesRelation);

                //// Example: Users -> Departments
                //DataColumn usersDeptColumn = dataSet.Tables["Users"].Columns["department_id"];
                //DataColumn departmentsDeptColumn = dataSet.Tables["Departments"].Columns["department_id"];
                //DataRelation usersDepartmentsRelation = new DataRelation("UsersDepartments", departmentsDeptColumn, usersDeptColumn);
                //dataSet.Relations.Add(usersDepartmentsRelation);

                // Relationships

                dataSet.Relations.Add(new DataRelation(
                    "UsersRoles",
                    dataSet.Tables["Roles"].Columns["role_id"],
                    dataSet.Tables["Users"].Columns["role_id"]
                ));

                dataSet.Relations.Add(new DataRelation(
                    "UsersDepartments",
                    dataSet.Tables["Departments"].Columns["department_id"],
                    dataSet.Tables["Users"].Columns["department_id"]
                ));

                dataSet.Relations.Add(new DataRelation(
                    "AppointmentsPatients",
                    dataSet.Tables["Patients"].Columns["patient_id"],
                    dataSet.Tables["Appointments"].Columns["patient_id"]
                ));

                dataSet.Relations.Add(new DataRelation(
                    "AppointmentsDoctors",
                    dataSet.Tables["Users"].Columns["user_id"],
                    dataSet.Tables["Appointments"].Columns["doctor_id"]
                ));

                dataSet.Relations.Add(new DataRelation(
                    "MedicalRecordsPatients",
                    dataSet.Tables["Patients"].Columns["patient_id"],
                    dataSet.Tables["MedicalRecords"].Columns["patient_id"]
                ));

                dataSet.Relations.Add(new DataRelation(
                    "MedicalRecordsDoctors",
                    dataSet.Tables["Users"].Columns["user_id"],
                    dataSet.Tables["MedicalRecords"].Columns["doctor_id"]
                ));

                dataSet.Relations.Add(new DataRelation(
                    "BillingPatients",
                    dataSet.Tables["Patients"].Columns["patient_id"],
                    dataSet.Tables["Billing"].Columns["patient_id"]
                ));

                dataSet.Relations.Add(new DataRelation(
                    "BillingServices",
                    dataSet.Tables["Services"].Columns["service_id"],
                    dataSet.Tables["Billing"].Columns["service_id"]
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

        public void LoadUsersIntoDataGridView(DataSet dataSet, System.Windows.Forms.DataGridView DataGridView)
        {
            try
            {
                DataGridView.DataSource = dataSet.Tables["Users"];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading Users table: {ex.Message}");
            }
        }

        public void UpdateUser(int userId, string name, string email, int roleId, int departmentId)
        {
            try
            {
                connection.Open();

                string query = "UPDATE Users SET name = @Name, email = @Email, role_id = @RoleId, department_id = @DepartmentId WHERE user_id = @UserId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@RoleId", roleId);
                    command.Parameters.AddWithValue("@DepartmentId", departmentId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("User updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("No user found with the specified ID.");
                    }
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
                string deleteAppointmentsQuery = "DELETE FROM Appointments WHERE doctor_id = @UserId";
                string deleteMedicalRecordsQuery = "DELETE FROM MedicalRecords WHERE doctor_id = @UserId";
                string deleteBillingQuery = "DELETE FROM Billing WHERE patient_id IN (SELECT patient_id FROM Patients WHERE user_id = @UserId)";

                using (SqlCommand command = new SqlCommand(deleteAppointmentsQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand(deleteMedicalRecordsQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }

                using (SqlCommand command = new SqlCommand(deleteBillingQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }

                // Finally, delete the user
                string deleteUserQuery = "DELETE FROM Users WHERE user_id = @UserId";
                using (SqlCommand command = new SqlCommand(deleteUserQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("User deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("No user found with the specified ID.");
                    }
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

    }
}