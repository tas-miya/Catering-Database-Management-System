using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CateringDatabaseSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*if (comboBox1.Text.Equals("Admin") || comboBox1.Text.Equals("Rider"))
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                button2.Enabled = true;
            }
            else*/ 
            if (comboBox1.Text.Equals("Customer"))
            {//if role selected is customer, open customer order form
                var CustomerOrder = new CustomerOrder();
                CustomerOrder.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {//opens forms for role selected
            ConnectingData c = new ConnectingData();
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Enter email and password!");
            }
            else
            {
                if (comboBox1.Text.Equals("Admin"))
                {
                    DataTable ds = c.Select("select password_2 from admin where email = '" + textBox1.Text + "'");
                    if (ds.Rows.Count <= 0)
                    {
                        MessageBox.Show("This email does not exist.");
                    }
                    else
                    {
                        if (ds.Rows[0][0].ToString() == textBox2.Text)
                        {
                            var Admin = new Admin();
                            Admin.Show();
                        }
                        else
                        {
                            MessageBox.Show("Incorrect Password");
                        }
                    }
                }
                else if (comboBox1.Text.Equals("Rider"))
                {
                    DataTable ds = c.Select("select riderPassword from rider where rideremail = '" + textBox1.Text + "'");
                    if (ds.Rows.Count <= 0)
                    {
                        MessageBox.Show("This email does not exist.");
                    }
                    else
                    {
                        if (ds.Rows[0][0].ToString() == textBox2.Text)
                        {
                            var Rider = new Rider();
                            Rider.Show();
                        }
                        else
                        {
                            MessageBox.Show("Incorrect Password");
                        }
                    }
                    
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {//enable login textboxes only if role selected is rider or admin, disable otherwise
            if (comboBox1.Text == "Rider" || comboBox1.Text == "Admin")
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                button2.Enabled = true;
                button1.Enabled = false;
            }
            else if (comboBox1.Text == "Customer")
            {
                textBox1.Clear();
                textBox2.Clear();
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                button2.Enabled = false;
                button1.Enabled = true;
            }
        }
    }
}
