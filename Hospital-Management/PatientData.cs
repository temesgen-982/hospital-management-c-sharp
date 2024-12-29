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
        SqlConnection connection = new SqlConnection("Server = localhost\\SQLEXPRESS; Database = hospitalDatabase; Integrated Security = True;");

        public DataSet LoadDatabaseSchemaAndData()
        {
            DataSet dataSet = new DataSet();

            try
            {
                connection.Open();

                string[] tableQueries = new string[]
                {
                    "SELECT * FROM Patients",
                    "SELECT * FROM Services",
                    "SELECT * FROM Appointments", // for relationships
                    "SELECT * FROM MedicalRecords", // for relationships
                    "SELECT * FROM Billing" // for relationships
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

                // relationships between tables
                
                dataSet.Relations.Add(new DataRelation(
                    "PatientsMedicalRecords",
                    dataSet.Tables["Patients"].Columns["patient_id"],
                    dataSet.Tables["MedicalRecords"].Columns["patient_id"]
                ));

                dataSet.Relations.Add(new DataRelation(
                    "PatientsAppointments",
                    dataSet.Tables["Patients"].Columns["patient_id"],
                    dataSet.Tables["Appointments"].Columns["patient_id"]
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

        public void LoadPatientsIntoDataGridView(DataSet dataSet, System.Windows.Forms.DataGridView dataGridView)
        {
            try
            {
                dataGridView.DataSource = dataSet.Tables["Patients"];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading Users table: {ex.Message}");
            }
        }

        public DataTable searchPatient(string searchTerm)
        {

            DataTable dataTable = new DataTable();

            string connectionString = "Server=localhost\\SQLEXPRESS;Database=hospitalDatabase;Integrated Security=True;";
            string query = "SELECT * FROM Patients WHERE first_name LIKE @SearchTerm";

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