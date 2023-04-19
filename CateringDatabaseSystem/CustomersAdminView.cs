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
    public partial class CustomersAdminView : Form
    {
        public CustomersAdminView()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {//add customer
            ConnectingData c = new ConnectingData();
            //error messages for null values
            if (textBox2.Text == "")
            {
                MessageBox.Show("Please enter customer name.");
            }
            else if (textBox6.Text == "")
            {
                MessageBox.Show("Please enter customer phone number.");
            }
            else if (textBox9.Text == "")
            {
                MessageBox.Show("Please enter customer address.");
            }
            else if (textBox11.Text == "")
            {
                MessageBox.Show("Please enter customer email address.");
            }
            else
            {//add to database
                int i = 0;
                if (!int.TryParse(textBox6.Text, out i) ||
                     !int.TryParse(textBox8.Text, out i)
                  )
                {
                    MessageBox.Show("Invalid Phone Number");
                }
                else
                {
                    c.Inserts("insert into customers (CustomerID, CustomerFName,CustomerLName ,CustomerContactNo, CustomerAddress, AlternatePhone,Email) values ((select max(CustomerID) from customers)+1, '" + textBox2.Text + "', '" + textBox15.Text + "', '" + textBox6.Text + "', '" + textBox9.Text + "', '" + textBox8.Text + "', '" + textBox11.Text + "'" + ")");
                }
            }
            if (textBox10.Text != "")
            {//if credit card info is given, add to database
                int i = 0;
                if (!int.TryParse(textBox10.Text, out i)
                  )
                {
                    MessageBox.Show("Invalid Credit Card No");
                }
                else
                {
                    c.Inserts("insert into customers (CreditCardNo) values ('" + textBox10.Text + "')");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {//update customer
            ConnectingData c = new ConnectingData();
            if (textBox7.Text == "")
            {
                MessageBox.Show("Please enter ID of customer to update.");
            }
            else
            {
                int i = 0;
                if (!int.TryParse(textBox7.Text, out i)
                  )
                {
                    MessageBox.Show("Invalid ID");
                }
                else
                {
                    if (textBox5.Text != "")
                    {
                        c.Inserts("update customers set CustomerFName = '" + textBox5.Text + "'" + " where CustomerID = " + textBox7.Text);
                    }
                    if (textBox16.Text != "")
                    {
                        c.Inserts("update customers set CustomerLName= '" + textBox16.Text + "'" + " where CustomerID = " + textBox7.Text);
                    }
                    if (textBox3.Text != "")
                    {
                        int j = 0;
                        if (!int.TryParse(textBox3.Text, out j)
                          )
                        {
                            MessageBox.Show("Invalid Phone number");
                        }
                        else
                        {
                            c.Inserts("update customers set CustomerContactNo= '" + textBox3.Text + "'" + " where CustomerID = " + textBox7.Text);
                        }
                    }
                    if (textBox1.Text != "")
                    {
                        c.Inserts("update customers set CustomerAddress= '" + textBox1.Text + "'" + " where CustomerID = " + textBox7.Text);
                    }
                    if (textBox14.Text != "")
                    {
                        int j = 0;
                        if (!int.TryParse(textBox14.Text, out j)
                          )
                        {
                            MessageBox.Show("Invalid Phone number");
                        }
                        else
                        {
                            c.Inserts("update customers set AlternatePhone= '" + textBox14.Text + "'" + " where CustomerID = " + textBox7.Text);
                        }
                    }
                    if (textBox11.Text != "")
                    {
                        c.Inserts("update customers set Email= '" + textBox11.Text + "'" + " where CustomerID = " + textBox7.Text);
                    }
                    if (textBox12.Text != "")
                    {
                        int j = 0;
                        if (!int.TryParse(textBox12.Text, out j)
                          )
                        {
                            MessageBox.Show("Invalid Credit Card No");
                        }
                        else
                        {
                            c.Inserts("update customers set CreditCardNo= '" + textBox12.Text + "'" + " where CustomerID = " + textBox7.Text);
                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {//show all customers
            ConnectingData c = new ConnectingData();
            dataGridView1.DataSource = c.Select("select * from Customers");
        }

        private void button2_Click(object sender, EventArgs e)
        {//delete customer
            ConnectingData c = new ConnectingData();
            
            if (textBox4.Text == "")
            {
                MessageBox.Show("Please enter the ID of the customer to delete.");
            }
            else
            {
                int i = 0;
                if (!int.TryParse(textBox4.Text, out i)
                  )
                {
                    MessageBox.Show("Invalid ID type");
                }
                else
                {
                    DataTable ds = c.Select("select orderstatus from orders where Customers_CustomerID = " + textBox4.Text);
                    if (ds.Rows.Count > 0 && ds.Rows[0][0].ToString() != "Delivered")
                    {
                        DialogResult dialogResult = MessageBox.Show("This customer has an order that has not been delivered. Deleting them would result in a deletion of the order. Would you like to continue?", "Warning",MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            c.Inserts("exec deleteCustomerAndOrder @customer = " + textBox4.Text);
                        }
                    }
                }
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text != "")
            {
                textBox1.Text = "";
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
