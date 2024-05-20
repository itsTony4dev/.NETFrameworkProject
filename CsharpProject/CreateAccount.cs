using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsharpProject
{
    public partial class CreateAccount : Form
    {
        public CreateAccount()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=.;Initial Catalog=csharp;Integrated Security=True;Encrypt=False";
            SqlConnection conn = new SqlConnection(connectionString);

            string firstName = txtName.Text;
            string lastName = txtLastName.Text;
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (password != confirmPassword)
            {
                MessageBox.Show("Password does not match");
                txtConfirmPassword.Text = "";
                txtConfirmPassword.Focus();
                return;
            }
            else
            {
                string query = $"INSERT INTO [user] ([fname], [lname], [password], [role]) VALUES ('{firstName}', '{lastName}', '{password}', '0')";
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Test completed");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex.Message);
                }
            }

        }
    }
}
