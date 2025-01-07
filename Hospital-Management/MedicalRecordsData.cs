using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management
{
    class MedicalRecordsData
    {
        SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=hospitalDatabase; Integrated Security=True;");

        public MedicalRecordsData() { }

        public DataSet LoadMedicalRecords()
        {
            DataSet dataSet = new DataSet();

            try
            {
                connection.Open();

                string query = "SELECT * FROM MedicalRecords";

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    adapter.Fill(dataSet, "MedicalRecords");
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

        public void InsertMedicalRecord(int patientId, int doctorId, string diagnosis, string treatment)
        {
            try
            {
                connection.Open();

                string query = @"
        INSERT INTO MedicalRecords (patient_id, doctor_id, diagnosis, treatment, date)
        VALUES (@PatientId, @DoctorId, @Diagnosis, @Treatment, GETDATE())"; // Using GETDATE() for the current date and time

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PatientId", patientId);
                    command.Parameters.AddWithValue("@DoctorId", doctorId);
                    command.Parameters.AddWithValue("@Diagnosis", diagnosis);
                    command.Parameters.AddWithValue("@Treatment", treatment);

                    command.ExecuteNonQuery();
                    Console.WriteLine("Medical record inserted successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while inserting the medical record: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }


        public void UpdateMedicalRecord(int recordId, string diagnosis, string treatment, DateTime date)
        {
            try
            {
                connection.Open();

                string query = @"
                UPDATE MedicalRecords
                SET diagnosis = @Diagnosis, treatment = @Treatment, date = @Date
                WHERE record_id = @RecordId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RecordId", recordId);
                    command.Parameters.AddWithValue("@Diagnosis", diagnosis);
                    command.Parameters.AddWithValue("@Treatment", treatment);
                    command.Parameters.AddWithValue("@Date", date);

                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Medical record updated successfully." : "No record found with the specified ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the medical record: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        public void DeleteMedicalRecord(int recordId)
        {
            try
            {
                connection.Open();

                string query = "DELETE FROM MedicalRecords WHERE record_id = @RecordId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RecordId", recordId);

                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Medical record deleted successfully." : "No record found with the specified ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the medical record: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        public DataTable SearchMedicalRecords(int patientId)
        {
            DataTable dataTable = new DataTable();
            string query = "SELECT * FROM MedicalRecords WHERE patient_id = @PatientId";

            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@PatientId", patientId);

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
