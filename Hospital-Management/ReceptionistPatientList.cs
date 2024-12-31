using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hospital_Management
{
    public partial class ReceptionistPatientList : Form
    {
        public ReceptionistPatientList()
        {
            InitializeComponent();

            loadData();
        }

        private void loadData()
        {
            PatientData patientData = new PatientData();
            patientData.LoadPatientsIntoDataGridView(patientData.LoadDatabaseSchemaAndData(), dataGridView1);

            Edit.DisplayIndex = dataGridView1.Columns.Count - 2;
            Delete.DisplayIndex = dataGridView1.Columns.Count - 1;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {

                int patientId = (int)(dataGridView1.Rows[e.RowIndex].Cells["patient_id"].Value);

                if (senderGrid.Columns[e.ColumnIndex].Name == "Edit")
                {
                    PatientInformation pr = new PatientInformation(patientId);
                    pr.Show();
                    loadData();
                }
                else if (senderGrid.Columns[e.ColumnIndex].Name == "Delete")
                {
                    deletePatient(patientId);
                }
            }
        }

        private void deletePatient(int patientId)
        {
            var confirmResult = MessageBox.Show(
                $"Are you sure you want to delete the patient with ID {patientId}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                PatientData patientData = new PatientData();
                patientData.DeletePatient(patientId);

                MessageBox.Show("success");
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            string searchText = searchTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                loadData();
                return;
            }

            PatientData patientData = new PatientData();

            dataGridView1.DataSource = patientData.SearchPatient(searchText);
        }

        private void comboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = comboBoxFilter.SelectedItem.ToString();

            if (dataGridView1.DataSource is DataTable dataTable)
            {
                if (selectedValue == "First Name")
                {
                    dataTable.DefaultView.Sort = "first_name ASC";
                }
                else if (selectedValue == "Gender")
                {
                    dataTable.DefaultView.Sort = "gender ASC";
                }
                else if(selectedValue == "DOB")
                {
                    dataTable.DefaultView.Sort = "dob ASC";
                }

                dataGridView1.DataSource = dataTable.DefaultView.ToTable();

                Delete.DisplayIndex = dataGridView1.Columns.Count - 1;
                Edit.DisplayIndex = dataGridView1.Columns.Count - 2;
            }
        }

        private void homeButton_click(object sender, EventArgs e)
        {
            ReceptionistDashboard rd = new ReceptionistDashboard();
            rd.Show();
            this.Hide();
        }

        private void patientRegistration_click(object sender, EventArgs e)
        {
            PatientInformation pr = new PatientInformation(0);
            pr.Show();
            this.Hide();
        }

        private void AppointmentScheduling_click(object sender, EventArgs e)
        {
            ReceptionistAppointmentScheduling asc = new ReceptionistAppointmentScheduling();
            asc.Show();
            this.Hide();

        }
    }
}
