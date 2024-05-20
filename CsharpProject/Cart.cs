using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsharpProject
{
    public partial class Cart : Form
    {
        public Cart()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Cart_Load(object sender, EventArgs e)
        {
            int total = 0;
            listView1.Items.Clear();
            foreach (var item in Product.products)
            {
                ListViewItem li = new ListViewItem(item.Name);
                li.SubItems.Add(item.Description);
                li.SubItems.Add(item.Price.ToString());
                total += item.Price;
                listView1.Items.Add(li);
            }

            lblTotalPrice.Text = total.ToString() + "$";
            lblTotalPrice.ForeColor = Color.Red;
        }
    }
}
