using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsharpProject
{
    public partial class Screens : Form
    {
        public Screens()
        {
            InitializeComponent();
            if(User.isAdmin)
            {
                pnlAddProduct.Visible = true;
            }
            else
            {
                pnlAddProduct.Visible = false;
            }
        }

        string connectionString = "Data Source=.;Initial Catalog=csharp;Integrated Security=True;Encrypt=False";
        static string selectAll = " SELECT * FROM products WHERE category_id IN ( SELECT category_id FROM category WHERE name = 'Screens');";
        private void Screens_Load(object sender, EventArgs e)
        {
            lblYourAccount.Text = User.Name;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(selectAll, conn);


            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                //reader.Read();
                int locationx = -220;//278
                int locationy = 200;

                foreach (var row in reader)
                {
                    string name = reader["p_name"].ToString();
                    string category = reader["category_id"].ToString();
                    string description = reader["description"].ToString();
                    string price = reader["price"].ToString();
                    string imageUrl = reader["pic"].ToString();

                    Product product = new Product(name, /* category,*/ description, int.Parse(price));

                    Panel newPanel = new Panel();
                    newPanel.Name = "pnlInfo";
                    newPanel.Location = new Point(locationx, locationy);
                    newPanel.Size = new Size(350, 318);

                    Image newImage = Image.FromFile("C:\\Users\\T\\source\\repos\\CsharpProject\\CsharpProject\\Resources\\Capture.PNG");
                    PictureBox newPictureBox = new PictureBox();
                    newPictureBox.Name = "imgIcon";
                    newPictureBox.Image = newImage;
                    newPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    newPictureBox.Size = new Size(201, 146);
                    newPictureBox.Location = new Point(100, 3);
                    using (WebClient webClient = new WebClient())
                    // using(  var httpClient = new HttpClient())
                    {
                        byte[] imageData = webClient.DownloadData(imageUrl);
                        //byte[] imageData = await httpClient.GetByteArrayAsync(imageUrl);
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            Image images = Image.FromStream(ms);
                            newPictureBox.Image = images;
                        }
                    }

                    Label lblCategory = new Label();
                    lblCategory.Name = "lblCategory";
                    lblCategory.Text = $"{product.Name}";
                    lblCategory.TextAlign = ContentAlignment.MiddleCenter;
                    lblCategory.Size = new Size(105, 25);
                    lblCategory.Location = new Point(146, 154);

                    Label lblSpecs = new Label();
                    lblSpecs.Name = "lblSpecs";
                    lblSpecs.Text = $"{product.Description}";
                    lblSpecs.TextAlign = ContentAlignment.MiddleCenter;
                    lblSpecs.Size = new Size(221, 40);
                    lblSpecs.Location = new Point(93, 182);

                    Label lblPrice = new Label();
                    lblPrice.Name = "lblPrice";
                    lblPrice.Text = $"${product.Price}";
                    lblPrice.TextAlign = ContentAlignment.MiddleCenter;
                    lblPrice.Size = new Size(55, 20);
                    lblPrice.Location = new Point(167, 230);
                    lblPrice.Font = new Font("Microsoft Sans Serif", 12);

                    //Guna2Button btnDelete = new Guna2Button();
                    //btnDelete.Name = "btnShowFullSpecs";
                    //btnDelete.Text = "Show Full Specs";
                    //btnDelete.BorderRadius = 20;
                    //btnDelete.BorderThickness = 2;
                    //btnDelete.FillColor = Color.Silver;
                    //btnDelete.ForeColor = Color.Black;
                    //btnDelete.Size = new Size(90, 42);
                    //btnDelete.Location = new Point(85, 256);
                    //btnDelete.Click += new EventHandler(btnDelete_Click);

                    Guna2Button btnAddTocart = new Guna2Button();
                    btnAddTocart.Name = "btnAddToCart";
                    btnAddTocart.Text = "Add To Cart";
                    btnAddTocart.BorderRadius = 20;
                    btnAddTocart.BorderThickness = 2;
                    btnAddTocart.FillColor = Color.Red;
                    btnAddTocart.ForeColor = Color.White;
                    btnAddTocart.Size = new Size(90, 42);
                    btnAddTocart.Location = new Point(153, 256);
                    btnAddTocart.Enabled = User.isLogedIn;
                    btnAddTocart.Click += new EventHandler(btnAddToCart_Click);

                    newPanel.Controls.Add(newPictureBox);
                    newPanel.Controls.Add(lblCategory);
                    newPanel.Controls.Add(lblSpecs);
                    newPanel.Controls.Add(lblPrice);
                    // newPanel.Controls.Add(btnDelete);
                    newPanel.Controls.Add(btnAddTocart);

                    // Add the new Panel to the form
                    parentPanel.Controls.Add(newPanel);

                    if (locationx < 1080)
                    {
                        locationx += 500;
                    }
                    else
                    {
                        locationy += 365;
                        locationx = 278;
                    }
                    newPanel.Location = new Point(locationx, locationy);

                    if (locationx + 500 < 1000)
                    {
                        pnlAddProduct.Location = new Point(locationx + 500, locationy);
                    }
                    else
                    {
                        pnlAddProduct.Location = new Point(278, locationy + 365);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error001: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        //private void btnDelete_Click(object sender, EventArgs e)
        //{
        //    Guna2Button clickedButton = (Guna2Button)sender;

        //    Panel parentPanel = (Panel)clickedButton.Parent;

        //    Label lblCategory = parentPanel.Controls.Find("lblCategory", true).FirstOrDefault() as Label;
        //    Label lblDescription = parentPanel.Controls.Find("lblSpecs", true).FirstOrDefault() as Label;
        //    Label lblPrice = parentPanel.Controls.Find("lblPrice", true).FirstOrDefault() as Label;

        //    if (lblCategory != null && lblDescription != null && lblPrice != null)
        //    {
        //        string name = lblCategory.Text;
        //        string description = lblDescription.Text;
        //        string priceText = lblPrice.Text;
        //    }

        //    SqlConnection conn = new SqlConnection(connectionString);
        //    var query = $" DELETE FROM [products] WHERE ()";

        //    SqlCommand cmd = new SqlCommand(query, conn);
        //}

        Cart cart = new Cart();
        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            Guna2Button clickedButton = (Guna2Button)sender;

            Panel parentPanel = (Panel)clickedButton.Parent;

            Label lblCategory = parentPanel.Controls.Find("lblCategory", true).FirstOrDefault() as Label;
            Label lblDescription = parentPanel.Controls.Find("lblSpecs", true).FirstOrDefault() as Label;
            Label lblPrice = parentPanel.Controls.Find("lblPrice", true).FirstOrDefault() as Label;

            if (lblCategory != null && lblDescription != null && lblPrice != null)
           {
                string name = lblCategory.Text;
                string description = lblDescription.Text;
                string priceText = lblPrice.Text;

                if (priceText.StartsWith("$"))
                {
                    string priceValue = priceText.Substring(1); // Remove the dollar sign
                    int price = int.Parse(priceValue);

                    // Now after we target the required labels we can use the retrieved values in the cart logic
                    AddProductToCart(name, description, price);
                    //cart.AddProductToList(name, description, price);
                }
            }
        }

        private void AddProductToCart(string name, string description, int price)
        {
            Product p = new Product(name,description,price);
            Cart cartForm = new Cart();
            Product.products.Add(p);
            MessageBox.Show($"Product added to cart:\nCategory: {name}\nDescription: {description}\nPrice: {price}");
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 frm = new Form1();
            frm.ShowDialog();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            parentPanel.Controls.Clear();// we need to remove the old products first to display the new ones
            string search = txtSearch.Text;


            SqlConnection conn = new SqlConnection(connectionString);
            var query = $" SELECT * FROM products WHERE category_id IN ( SELECT category_id FROM category WHERE name IN ('Screens') AND description LIKE '%{search}%'";

            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                //reader.Read();
                int locationx = -220;
                int locationy = 27;

                foreach (var row in reader)
                {
                    string name = reader["p_name"].ToString();
                    string category = reader["category_id"].ToString();
                    string description = reader["description"].ToString();
                    string price = reader["price"].ToString();
                    string imageUrl = reader["pic"].ToString();

                    Product product = new Product(name, /* category,*/ description, int.Parse(price));

                    Panel newPanel = new Panel();
                    newPanel.Name = "pnlInfo";
                    newPanel.Location = new Point(locationx, locationy);
                    newPanel.Size = new Size(350, 318);

                    // Image newImage = Image.FromFile("C:\\Users\\T\\source\\repos\\CsharpProject\\CsharpProject\\Resources\\Capture.PNG");
                    PictureBox newPictureBox = new PictureBox();
                    newPictureBox.Name = "imgIcon";
                    // newPictureBox.Image = newImage;
                    newPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    newPictureBox.Size = new Size(201, 146);
                    newPictureBox.Location = new Point(100, 3);
                    using (WebClient webClient = new WebClient())
                    // using(  var httpClient = new HttpClient())
                    {
                        byte[] imageData = webClient.DownloadData(imageUrl);
                        //byte[] imageData = await httpClient.GetByteArrayAsync(imageUrl);
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            Image images = Image.FromStream(ms);
                            newPictureBox.Image = images;
                        }
                    }

                    Label lblCategory = new Label();
                    lblCategory.Name = "lblCategory";
                    lblCategory.Text = $"{product.Category}";
                    lblCategory.TextAlign = ContentAlignment.MiddleCenter;
                    lblCategory.Size = new Size(105, 25);
                    lblCategory.Location = new Point(146, 154);

                    Label lblSpecs = new Label();
                    lblSpecs.Name = "lblSpecs";
                    lblSpecs.Text = $"{product.Description}";
                    lblSpecs.TextAlign = ContentAlignment.MiddleCenter;
                    lblSpecs.Size = new Size(221, 40);
                    lblSpecs.Location = new Point(93, 182);

                    Label lblPrice = new Label();
                    lblPrice.Name = "lblPrice";
                    lblPrice.Text = $"${product.Price}";
                    lblPrice.TextAlign = ContentAlignment.MiddleCenter;
                    lblPrice.Size = new Size(55, 20);
                    lblPrice.Location = new Point(167, 233);
                    lblPrice.Font = new Font("Microsoft Sans Serif", 12);

                    Guna2Button btnAddTocart = new Guna2Button();
                    btnAddTocart.Name = "btnAddToCart";
                    btnAddTocart.Text = "Add To Cart";
                    btnAddTocart.BorderRadius = 20;
                    btnAddTocart.BorderThickness = 2;
                    btnAddTocart.FillColor = Color.Red;
                    btnAddTocart.ForeColor = Color.White;
                    btnAddTocart.Size = new Size(90, 42);
                    btnAddTocart.Location = new Point(153, 256);
                    btnAddTocart.Enabled = User.isLogedIn;
                    btnAddTocart.Click += new EventHandler(btnAddToCart_Click);

                    newPanel.Controls.Add(newPictureBox);
                    newPanel.Controls.Add(lblCategory);
                    newPanel.Controls.Add(lblSpecs);
                    newPanel.Controls.Add(lblPrice);
                    newPanel.Controls.Add(btnAddTocart);

                    // Add the new Panel to the form
                    parentPanel.Controls.Add(newPanel);

                    if (locationx < 1080)
                    {
                        locationx += 500;
                    }
                    else
                    {
                        locationy += 365;
                        locationx = 278;
                    }
                    newPanel.Location = new Point(locationx, locationy);

                    if (locationx + 500 < 1000)
                    {

                        pnlAddProduct.Location = new Point(locationx + 500, locationy);
                    }
                    else
                    {
                        pnlAddProduct.Location = new Point(278, locationy + 365);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error005: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            LogIn logIn = new LogIn(this.Name);
            logIn.ShowDialog();
        }

        private void lblYourCart_Click(object sender, EventArgs e)
        {
            Cart cart = new Cart();
            cart.ShowDialog();
        }
    }
}
