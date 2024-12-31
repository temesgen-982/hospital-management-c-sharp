using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace Hospital_Management
{
    public partial class AdminUserManagement : Form
    {
        public AdminUserManagement()
        {
            InitializeComponent();

            loadData();
        }

        private void loadData()
        {
            UserData users = new UserData();

            users.LoadUsersIntoDataGridView(users.LoadDatabaseSchemaAndData(), dataGridView1);

            Delete.DisplayIndex = dataGridView1.Columns.Count - 1;
            Edit.DisplayIndex = dataGridView1.Columns.Count - 2;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                
                int userId = (int) (dataGridView1.Rows[e.RowIndex].Cells["user_id"].Value);

                if (senderGrid.Columns[e.ColumnIndex].Name == "Edit")
                {
                    EditUserForm editUser = new EditUserForm(userId);
                    editUser.Show();
                    loadData();
                }
                else if (senderGrid.Columns[e.ColumnIndex].Name == "Delete")
                {
                    deleteUser(userId);
                }
            }
        }

        private void deleteUser(int userId)
        {
            var confirmResult = MessageBox.Show(
                $"Are you sure you want to delete the user with ID {userId}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                UserData userData = new UserData();
                userData.DeleteUser(userId);

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

            UserData userData = new UserData();

            dataGridView1.DataSource = userData.SearchUser(searchText);
        }

        private void comboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = comboBoxFilter.SelectedItem.ToString();

            if (dataGridView1.DataSource is DataTable dataTable)
            {
                if (selectedValue == "First Name")
                {
                    dataTable.DefaultView.Sort = "first_name ASC";
                }else if(selectedValue == "Role")
                {
                    dataTable.DefaultView.Sort = "role ASC";
                }

                dataGridView1.DataSource = dataTable.DefaultView.ToTable();

                Delete.DisplayIndex = dataGridView1.Columns.Count - 1;
                Edit.DisplayIndex = dataGridView1.Columns.Count - 2;
            }
        }


        private void homeButton_Click(object sender, EventArgs e)
        {
            AdminDashboard ad = new AdminDashboard();
            ad.Show();
            this.Hide();
        }

        private void reportingTools_Click(object sender, EventArgs e)
        {
            AdminReportingTools rt = new AdminReportingTools();
            rt.Show();
            this.Hide();
        }

        private void systemAlerts_Click(object sender, EventArgs e)
        {
            AdminSystemAlerts sa = new AdminSystemAlerts();
            sa.Show();
            this.Hide();
        }

        private void performanceMetrics_Click(object sender, EventArgs e)
        {
            AdminPerformanceMetrics pm = new AdminPerformanceMetrics();
            pm.Show();
            this.Hide();
        }

        private void AdminUserManagement_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void addUserButton_Click(object sender, EventArgs e)
        {
            SignupForm signup = new SignupForm("Add User");
            signup.Show();
        }
    }
}
