using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;

namespace Hospital_Management
{
    class AppointmentsData
    {
        public DataTable GetAppointments(int doctorId, DateTime? appointmentDate = null)
        {
            DataTable appointments = new DataTable();
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=hospitalDatabase; Integrated Security=True;";


            string query = @"
        SELECT appointment_id, patient_id, status, appointment_date 
        FROM Appointments 
        WHERE doctor_id = @DoctorId";
            
            if (appointmentDate.HasValue)
            {
                query += " AND CAST(appointment_date AS DATE) = @AppointmentDate";
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@DoctorId", SqlDbType.Int).Value = doctorId;
                        
                        if (appointmentDate.HasValue)
                        {
                            command.Parameters.Add("@AppointmentDate", SqlDbType.Date).Value = appointmentDate.Value.Date;
                        }

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(appointments);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error fetching appointments.", ex);
            }

            return appointments;
        }

        public DataTable GetAppointmentsGroupedByPatient(int doctorId)
        {
            DataTable groupedAppointments = new DataTable();
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=hospitalDatabase; Integrated Security=True;";

            // Query to group appointments by patient_id
            string query = @"
        SELECT patient_id, 
               COUNT(appointment_id) AS appointment_count,
               MIN(appointment_date) AS first_appointment_date,
               MAX(appointment_date) AS last_appointment_date
        FROM Appointments
        WHERE doctor_id = @DoctorId
        GROUP BY patient_id";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@DoctorId", SqlDbType.Int).Value = doctorId;
                        
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(groupedAppointments);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error fetching grouped appointments.", ex);
            }

            return groupedAppointments;
        }

        public DataTable GetAppointmentsGroupedByDate(int doctorId)
        {
            DataTable groupedAppointments = new DataTable();
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=hospitalDatabase; Integrated Security=True;";

            // Query to group appointments by appointment_date
            string query = @"
        SELECT CAST(appointment_date AS DATE) AS appointment_date, 
               COUNT(appointment_id) AS appointment_count,
               MIN(patient_id) AS first_patient_id,
               MAX(patient_id) AS last_patient_id
        FROM Appointments
        WHERE doctor_id = @DoctorId
        GROUP BY CAST(appointment_date AS DATE)
        ORDER BY appointment_date";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@DoctorId", SqlDbType.Int).Value = doctorId;
                        
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(groupedAppointments);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error fetching grouped appointments by date.", ex);
            }

            return groupedAppointments;
        }

        public int CountAppointmentsByReceptionist(int receptionistId)
        {
            int appointmentCount = 0;
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=hospitalDatabase; Integrated Security=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("CountAppointmentsByReceptionist", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@ReceptionistId", SqlDbType.Int).Value = receptionistId;

                        connection.Open();
                        appointmentCount = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error counting appointments by receptionist.", ex);
            }

            return appointmentCount;
        }



    }
}
