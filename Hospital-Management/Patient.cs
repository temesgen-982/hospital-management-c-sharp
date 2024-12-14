using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management
{
    class Patient
    {
        SqlConnection connection = new SqlConnection("Server = localhost; Database = hospitalDatabase; Integrated Security = True;");

        public string first_name { get; set; }
        public string last_name { get; set; }
        public string gender { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string emergency_contact { get; set; }

        public Patient()
        {

        }

        public List<Patient> getPatients()
        {

            List<Patient> listData = new List<Patient>();

            try
            {
                connection.Open();
                string query = @"
                            SELECT *
                            FROM Patients";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        Patient patient = new Patient();
                        patient.first_name = reader["first_name"].ToString();
                        patient.last_name = reader["last_name"].ToString();
                        patient.gender = reader["gender"].ToString();
                        patient.phone = reader["phone"].ToString();
                        patient.address = reader["address"].ToString();
                        patient.emergency_contact = reader["emergency_contact"].ToString();

                        listData.Add(patient);
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