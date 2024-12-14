using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management
{
    class Billing
    {
        SqlConnection connection = new SqlConnection("Server = localhost; Database = hospitalDatabase; Integrated Security = True;");
        public int BillId { get; set; }
        public int PatientId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }

        public List<Billing> getBillings()
        {
            List<Billing> listData = new List<Billing>();

            try
            {
                connection.Open();
                string query = @"
                                SELECT b.bill_id, p.patient_id, p.first_name, p.last_name, s.service_id, s.service_name, b.amount, b.status, b.date
                                FROM Billing b
                                JOIN Patients p ON b.patient_id = p.patient_id
                                JOIN Services s ON b.service_id = s.service_id";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        Billing billing = new Billing
                        {
                            BillId = (int)reader["bill_id"],
                            PatientId = (int)reader["patient_id"],
                            PatientFirstName = reader["first_name"].ToString(),
                            PatientLastName = reader["last_name"].ToString(),
                            ServiceId = (int)reader["service_id"],
                            ServiceName = reader["service_name"].ToString(),
                            Amount = (decimal)reader["amount"],
                            Status = reader["status"].ToString(),
                            Date = (DateTime)reader["date"]
                        };

                        listData.Add(billing);
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
    }
}