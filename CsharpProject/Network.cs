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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;

namespace CsharpProject
{
    public partial class Network : Form
    {
        public Network()
        {
            InitializeComponent();
            if (User.isAdmin)
            {
                pnlAddProduct.Visible = true;
            }
            else
            {
                pnlAddProduct.Visible = false;
            }
        }
        string connectionString = "Data Source=.;Initial Catalog=csharp;Integrated Security=True;Encrypt=False";
        static string selectAll = " SELECT * FROM products WHERE category_id IN ( SELECT category_id FROM category WHERE name = 'Network');";

        private async void Network_Load(object sender, EventArgs e)
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

                   // Image newImage = Image.FromFile("C:\\Users\\T\\source\\repos\\CsharpProject\\CsharpProject\\Resources\\Capture.PNG");
                    PictureBox newPictureBox = new PictureBox();
                    newPictureBox.Name = "imgIcon";
                   // newPictureBox.Image = newImage;
                    newPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    newPictureBox.Size = new Size(201, 146);
                    newPictureBox.Location = new Point(100, 3);
                    //using (WebClient webClient = new WebClient())
                     using(  HttpClient httpClient = new HttpClient())
                    {
                       // byte[] imageData = webClient.DownloadData(imageUrl);
                        byte[] imageData = await httpClient.GetByteArrayAsync(imageUrl);
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

                    //Guna2Button btnSpecs = new Guna2Button();
                    //btnSpecs.Name = "btnShowFullSpecs";
                    //btnSpecs.Text = "Show Full Specs";
                    //btnSpecs.BorderRadius = 20;
                    //btnSpecs.BorderThickness = 2;
                    //btnSpecs.FillColor = Color.Silver;
                    //btnSpecs.ForeColor = Color.Black;
                    //btnSpecs.Size = new Size(90, 42);
                    //btnSpecs.Location = new Point(85, 256);
                    // btnSpecs.Click += new EventHandler(btnShowFullSpecs_Click);

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
                    // newPanel.Controls.Add(btnSpecs);
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
        private void AddProductToCart(string name, string description, int price)
        {

           
                Product product = new Product(name, description, price);
                Product.products.Add(product);
                Cart cartForm = new Cart();
                MessageBox.Show($"Product added to cart:\nCategory: {name}\nDescription: {description}\nPrice: {price}");
            
        }

        Cart cart = new Cart();
        private void btnAddToCart_Click(object sender, EventArgs e)
        {

            // Cast the sender back to Guna2Button to access its properties
            Guna2Button clickedButton = (Guna2Button)sender;

            // Get the parent Panel containing the clicked button
            Panel parentPanel = (Panel)clickedButton.Parent;

            // Find the corresponding controls within the parent Panel
            Label lblCategory = parentPanel.Controls.Find("lblCategory", true).FirstOrDefault() as Label;
            Label lblDescription = parentPanel.Controls.Find("lblSpecs", true).FirstOrDefault() as Label;
            Label lblPrice = parentPanel.Controls.Find("lblPrice", true).FirstOrDefault() as Label;

            if (lblCategory != null && lblDescription != null && lblPrice != null)
            {
                // Access the text values of lblCategory, lblDescription, and lblPrice
                string name = lblCategory.Text;
                string description = lblDescription.Text;
                string priceText = lblPrice.Text;

                // Remove the dollar sign ($) and parse the price value
                if (priceText.StartsWith("$"))
                {
                    string priceValue = priceText.Substring(1); // Remove the dollar sign
                    int price = int.Parse(priceValue);

                    // Now you can use the retrieved values in your cart logic
                    AddProductToCart(name, description, price);
                   // cart.AddProductToList(category, description, price);
                }
            }
        }
       
        private void lblBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 frm = new Form1();
            frm.ShowDialog();
        }

        private void lblYourCart_Click(object sender, EventArgs e)
        {
            cart.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            parentPanel.Controls.Clear();
            string search = txtSearch.Text;


            SqlConnection conn = new SqlConnection(connectionString);
            var query = $" SELECT * FROM products WHERE category_id = 3 AND description LIKE '%{search}%';";

            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                //reader.Read();
                int locationx = -220;
                int locationy = 200;

                foreach (var row in reader)
                {
                    string name = reader["p_name"].ToString();
                    string category = reader["category_id"].ToString();
                    string description = reader["description"].ToString();
                    string price = reader["price"].ToString();
                    string image = reader["pic"].ToString();

                    Product product = new Product(name,/* category,*/ description, int.Parse(price));

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
                    lblPrice.Location = new Point(167, 233);
                    lblPrice.Font = new Font("Microsoft Sans Serif", 12);

                    //Guna2Button btnSpecs = new Guna2Button();
                    //btnSpecs.Name = "btnShowFullSpecs";
                    //btnSpecs.Text = "Show Full Specs";
                    //btnSpecs.BorderRadius = 20;
                    //btnSpecs.BorderThickness = 2;
                    //btnSpecs.FillColor = Color.Silver;
                    //btnSpecs.ForeColor = Color.Black;
                    //btnSpecs.Size = new Size(90, 42);
                    //btnSpecs.Location = new Point(85, 256);
                    // btnSpecs.Click += new EventHandler(btnShowFullSpecs_Click);

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
                    // newPanel.Controls.Add(btnSpecs);
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            parentPanel.Controls.Clear();// we need to remove the old products first to display the new ones
            string search = txtSearch.Text;


            SqlConnection conn = new SqlConnection(connectionString);
            var query = $" SELECT * FROM products WHERE category_id IN ( SELECT category_id FROM category WHERE name IN ('Network') AND description LIKE '%{search}%'";

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

                    Product product = new Product(name,/* category,*/ description, int.Parse(price));

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
                    btnAddTocart.Location = new Point(203, 256);
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


        int locationX;
        int locationY;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtPrice.Text.Trim() == "" || txtDescription.Text.Trim() == "")
            {
                MessageBox.Show("Please fill the fields");
                return;
            }
            else
            {
                SqlConnection conn = new SqlConnection(connectionString);

                string name = txtName.Text;
                // string category = cbbCategory.Text;
                string description = txtDescription.Text;
                string price = txtPrice.Text;
                string imageUrl = txtImageUrl.Text;
                int category_id;
                var query = $"INSERT INTO products (p_name,description,category_id,price) VALUES('{name}','{description}',3,{price})";

                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    locationX = pnlAddProduct.Location.X;
                    locationY = pnlAddProduct.Location.Y;

                    Panel newPanel = new Panel();
                    newPanel.Name = "pnlInfo";
                    newPanel.Location = new Point(locationX, locationY);
                    newPanel.Size = new Size(350, 318);

                    //Image newImage = Image.FromFile("C:\\Users\\T\\source\\repos\\CsharpProject\\CsharpProject\\Resources\\Capture.PNG");
                    PictureBox newPictureBox = new PictureBox();
                    newPictureBox.Name = "imgIcon";
                    //newPictureBox.Image = newImage;
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
                    lblCategory.Text = name;
                    lblCategory.TextAlign = ContentAlignment.MiddleCenter;
                    lblCategory.Size = new Size(75, 15);
                    lblCategory.Location = new Point(146, 174);

                    Label lblSpecs = new Label();
                    lblSpecs.Name = "lblValue1";
                    lblSpecs.Text = $"{description}";
                    lblSpecs.TextAlign = ContentAlignment.MiddleCenter;
                    lblSpecs.Size = new Size(221, 26);
                    lblSpecs.Location = new Point(93, 192);

                    Label lblPrice = new Label();
                    lblPrice.Name = "lblValue2";
                    lblPrice.Text = $"${price}";
                    lblPrice.TextAlign = ContentAlignment.MiddleCenter;
                    //lblPrice.BackColor = Color.White;
                    lblPrice.Size = new Size(45, 20);
                    lblPrice.Location = new Point(167, 233);
                    lblPrice.Font = new Font("Microsoft Sans Serif", 12);

                    //Guna2Button btnSpecs = new Guna2Button();
                    //btnSpecs.Name = "btnShowFullSpecs";
                    //btnSpecs.Text = "Show Full Specs";
                    //btnSpecs.BorderRadius = 20;
                    //btnSpecs.BorderThickness = 2;
                    //btnSpecs.FillColor = Color.Silver;
                    //btnSpecs.ForeColor = Color.Black;
                    //btnSpecs.Size = new Size(90, 42);
                    //btnSpecs.Location = new Point(85, 256);
                    //btnSpecs.Click += new EventHandler(btnShowFullSpecs_Click);

                    Guna2Button btnAddTocart = new Guna2Button();
                    btnAddTocart.Name = "btnAddToCart";
                    btnAddTocart.Text = "Add To Cart";
                    btnAddTocart.BorderRadius = 20;
                    btnAddTocart.BorderThickness = 2;
                    btnAddTocart.FillColor = Color.Red;
                    btnAddTocart.ForeColor = Color.White;
                    btnAddTocart.Size = new Size(90, 42);
                    btnAddTocart.Location = new Point(203, 256);
                    btnAddTocart.Enabled = User.isLogedIn;

                    newPanel.Controls.Add(newPictureBox);
                    newPanel.Controls.Add(lblCategory);
                    newPanel.Controls.Add(lblSpecs);
                    newPanel.Controls.Add(lblPrice);
                    //newPanel.Controls.Add(btnSpecs);
                    newPanel.Controls.Add(btnAddTocart);

                    parentPanel.Controls.Add(newPanel);
                    //changing the location of the panel everytime we add a new product
                    if (locationX < 1080)
                    {
                        locationX += 500;
                    }
                    else
                    {
                        locationY += 365;
                        locationX = 278;
                    }
                    pnlAddProduct.Location = new Point(locationX, locationY);

                    txtPrice.Clear();
                    txtDescription.Clear();
                }
                catch (Exception ex)
                {

                    MessageBox.Show("error006: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
