using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CsharpProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(1300, 1040);
        }

        private void btnDesktopLaptop_Click(object sender, EventArgs e)
        {

            this.Hide();
            Desktop_Laptop dl = new Desktop_Laptop();
            dl.Show();
        }

        private void pbAccount_Click(object sender, EventArgs e)
        {
            LogIn login = new LogIn(this.Name);
            login.ShowDialog();
        }

        private void lblYourCart_Click(object sender, EventArgs e)
        {
            
            Cart cart = new Cart();
            cart.ShowDialog();
        }

        private void btnNetwork_Click(object sender, EventArgs e)
        {
            this.Hide();
            Network network = new Network();
            network.ShowDialog();
        }

        private void btnPrinters_Click(object sender, EventArgs e)
        {
            this.Hide();
            Printers printers = new Printers();
            printers.ShowDialog();
        }

        private void btnScreens_Click(object sender, EventArgs e)
        {
            this.Hide();
            Screens screens = new Screens();
            screens.ShowDialog();
        }

        private void btnAccessories_Click(object sender, EventArgs e)
        {
            this.Hide();
            Accessories accessories = new Accessories();
            accessories.ShowDialog();
        }

        private void btnComputerParts_Click(object sender, EventArgs e)
        {
            this.Hide();
            Computer_Parts computerParts = new Computer_Parts();
            computerParts.ShowDialog();
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}
