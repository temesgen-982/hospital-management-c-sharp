using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

using System.IO;
using System.Drawing;

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

                string[] tableQueries = new string[]
                {
                    "SELECT * FROM Patients",
                    "SELECT * FROM Appointments",
                    "SELECT * FROM MedicalRecords"
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
                //dataSet.Relations.Add(new DataRelation(
                //    "PatientsAppointments",
                //    dataSet.Tables["Patients"].Columns["patient_id"],
                //    dataSet.Tables["Appointments"].Columns["patient_id"]
                //));

                //dataSet.Relations.Add(new DataRelation(
                //    "PatientsMedicalRecords",
                //    dataSet.Tables["Patients"].Columns["patient_id"],
                //    dataSet.Tables["MedicalRecords"].Columns["patient_id"]
                //));
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

        public void InsertPatient(string firstName, string lastName, DateTime dob, char gender, string phone, string address, string email, string emergencyContact, int receptionistId, byte[] profileImage = null)
        {
            try
            {
                connection.Open();

                string query = @"
                    INSERT INTO Patients (first_name, last_name, dob, gender, phone, address, email, emergency_contact, registered_by, profile_image)
                    VALUES (@FirstName, @LastName, @Dob, @Gender, @Phone, @Address, @Email, @EmergencyContact, @ReceptionistId, @ProfileImage);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Dob", dob);
                    command.Parameters.AddWithValue("@Gender", gender);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@Email", email ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EmergencyContact", emergencyContact);
                    command.Parameters.AddWithValue("@ReceptionistId", receptionistId);
                    command.Parameters.AddWithValue("@ProfileImage", (object)profileImage ?? DBNull.Value);

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

        public void UpdatePatient(int patientId, string firstName, string lastName, DateTime dob, char gender, string phone, string address, string email, string emergencyContact, byte[] profileImage = null)
        {
            try
            {
                connection.Open();

                string query = @"
                    UPDATE Patients
                    SET first_name = @FirstName,
                        last_name = @LastName,
                        dob = @Dob,
                        gender = @Gender,
                        phone = @Phone,
                        address = @Address,
                        email = @Email,
                        emergency_contact = @EmergencyContact,
                        profile_image = @ProfileImage
                    WHERE patient_id = @PatientId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PatientId", patientId);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Dob", dob);
                    command.Parameters.AddWithValue("@Gender", gender);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@Email", email ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EmergencyContact", emergencyContact);
                    command.Parameters.AddWithValue("@ProfileImage", (object)profileImage ?? DBNull.Value);

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

        public DataTable GetPatientsByReceptionist(int receptionistId)
        {
            DataTable dataTable = new DataTable();

            string query = @"
                SELECT * 
                FROM Patients
                WHERE registered_by = @ReceptionistId";

            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@ReceptionistId", receptionistId);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving patients: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }

            return dataTable.Rows.Count > 0 ? dataTable : null;
        }

        public int CountPatientsByReceptionist(int receptionistId)
        {
            int patientCount = 0;

            try
            {
                using (SqlCommand command = new SqlCommand("CountPatientsByReceptionist", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ReceptionistId", receptionistId);

                    connection.Open();
                    patientCount = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while counting patients: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }

            return patientCount;
        }

        public Image GetProfileImage(int patientId)
        {
            try
            {
                connection.Open();

                string query = "SELECT profile_image FROM Patients WHERE patient_id = @PatientId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PatientId", patientId);

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

    }
}