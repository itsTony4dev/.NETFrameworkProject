using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsharpProject
{
    public partial class LogIn : Form
    {
        private string FormSource;
        public LogIn(string source)//source from where we press to login
        {
            InitializeComponent();
            txtPassword.PasswordChar = '*';
            FormSource = source;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void openForm()//  the parameter of class to this string so we can know from which form we are coming and reopen the same form
        {
            switch (FormSource)
            {
                case "Desktop_Laptop":
                    Desktop_Laptop desktop_Laptop = new Desktop_Laptop();
                    desktop_Laptop.ShowDialog();
                    break;
                case "Network":
                    Network network = new Network();
                    network.ShowDialog();
                    break;
                case "Printers":
                    Printers printers = new Printers();
                    printers.ShowDialog();
                    break;
                case "Screens":
                    Screens screens = new Screens();
                    screens.ShowDialog();
                    break;
                case "Accessories":
                    Accessories accessories = new Accessories();
                    accessories.ShowDialog();
                    break;
                case "Computer_Parts":
                    Computer_Parts computerParts = new Computer_Parts();
                    computerParts.ShowDialog();
                    break;
            }
        }

        //public static List<Product> products;

        string connectionString = "Data Source=.;Initial Catalog=csharp;Integrated Security=True;Encrypt=False";
        private void btnLogin_Click(object sender, EventArgs e)//a methode that check if a user is registred beofre to login and if it is admin or user
        {

            SqlConnection conn = new SqlConnection(connectionString);
            string query = $"SELECT * FROM [user] WHERE password ='{txtPassword.Text}' AND fname='{txtName.Text}';";

            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    MessageBox.Show("Incorret username or password!");
                    txtName.Text = string.Empty;
                    txtPassword.Text = string.Empty;
                    txtName.Focus();
                    return;
                }

                if (reader["role"].Equals("1"))
                {
                    this.Hide();
                    User.Name = reader["fname"].ToString();
                    User.LastName = reader["lname"].ToString();
                    User.userID = (int)reader["user_id"];
                    User.isAdmin = true;
                    User.isLogedIn = true;
                    openForm();
                }
                else
                {
                    this.Hide();
                    User.Name = reader["fname"].ToString();
                    User.LastName = reader["lname"].ToString();
                    User.userID = (int)reader["user_id"];
                    User.isAdmin = false;
                    User.isLogedIn = true;
                    openForm();
                    Product.products = new List<Product>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error002: " + ex.Message);
                return;
            }
            finally
            {
                conn.Close();
            }
        }
        private void LogIn_Load(object sender, EventArgs e)
        {
            txtName.Focus();
        }

        private void lblCreateAccount_Click(object sender, EventArgs e)
        {
            this.Hide();
            CreateAccount createAccount = new CreateAccount();
            createAccount.ShowDialog();
        }
    }
}
