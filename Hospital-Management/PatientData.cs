using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management
{
    class PatientData
    {
        SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=hospitalDatabase; Integrated Security=True;");

        public PatientData() { }

        public DataSet LoadDatabaseSchemaAndData()
        {
            DataSet dataSet = new DataSet();

            try
            {
                connection.Open();

                // Define queries for each relevant table
                string[] tableQueries = new string[]
                {
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
                    "PatientsAppointments",
                    dataSet.Tables["Patients"].Columns["patient_id"],
                    dataSet.Tables["Appointments"].Columns["patient_id"]
                ));

                dataSet.Relations.Add(new DataRelation(
                    "PatientsMedicalRecords",
                    dataSet.Tables["Patients"].Columns["patient_id"],
                    dataSet.Tables["MedicalRecords"].Columns["patient_id"]
                ));

                dataSet.Relations.Add(new DataRelation(
                    "PatientsBilling",
                    dataSet.Tables["Patients"].Columns["patient_id"],
                    dataSet.Tables["Billing"].Columns["patient_id"]
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

        public void LoadPatientsIntoDataGridView(DataSet dataSet, System.Windows.Forms.DataGridView dataGrid)
        {
            dataGrid.DataSource = dataSet.Tables["Patients"];
        }

        public void InsertPatient(string firstName, string lastName, DateTime dob, char gender, string phone, string address, string email, string emergencyContact, int receptionistId)
        {
            try
            {
                connection.Open();

                // Insert into Patients table
                string query = @"
        INSERT INTO Patients (first_name, last_name, dob, gender, phone, address, email, emergency_contact, registered_by)
        VALUES (@FirstName, @LastName, @Dob, @Gender, @Phone, @Address, @Email, @EmergencyContact, @ReceptionistId);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Dob", dob);
                    command.Parameters.AddWithValue("@Gender", gender);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@Email", email ?? (object)DBNull.Value); // Allow null values for email
                    command.Parameters.AddWithValue("@EmergencyContact", emergencyContact);
                    command.Parameters.AddWithValue("@ReceptionistId", receptionistId);

                    command.ExecuteNonQuery();
                    Console.WriteLine("Patient added successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the patient: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        public void UpdatePatient(int patientId, string firstName, string lastName, DateTime dob, string gender, string phone, string address, string email, string emergencyContact)
        {
            try
            {
                connection.Open();

                // Update the Patients table
                string query = @"
                UPDATE Patients
                SET first_name = @FirstName,
                    last_name = @LastName,
                    dob = @Dob,
                    gender = @Gender,
                    phone = @Phone,
                    address = @Address,
                    email = @Email,
                    emergency_contact = @EmergencyContact
                WHERE patient_id = @PatientId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PatientId", patientId);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Dob", dob);
                    command.Parameters.AddWithValue("@Gender", gender.ToString());
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@Email", email ?? (object)DBNull.Value); // Allow null values for email
                    command.Parameters.AddWithValue("@EmergencyContact", emergencyContact);

                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Patient updated successfully." : "No patient found with the specified ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the patient: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        public void DeletePatient(int patientId)
        {
            try
            {
                connection.Open();

                // Delete from Patients table; dependent rows in related tables will cascade
                string deletePatientQuery = "DELETE FROM Patients WHERE patient_id = @PatientId";

                using (SqlCommand command = new SqlCommand(deletePatientQuery, connection))
                {
                    command.Parameters.AddWithValue("@PatientId", patientId);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Patient deleted successfully." : "No patient found with the specified ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the patient: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        public DataTable SearchPatient(string searchTerm)
        {
            DataTable dataTable = new DataTable();

            string query = @"
            SELECT * 
            FROM Patients
            WHERE first_name LIKE @SearchTerm
               OR last_name LIKE @SearchTerm
               OR phone LIKE @SearchTerm
               OR email LIKE @SearchTerm";

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