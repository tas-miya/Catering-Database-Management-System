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
    public partial class OrdersAdminView : Form
    {
        public OrdersAdminView()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {//update order 
            ConnectingData c = new ConnectingData();
            if (comboBox1.Text != "")
            {//update order status
                c.Inserts("update orders set orderstatus = '" + comboBox1.Text + "'" + "where OrderID = " + textBox1.Text);
                if (comboBox1.Text == "Dispatched" || comboBox1.Text == "Delivered")
                {//set shipped date to the day it was delivered
                    c.Inserts("update orders set shippedDate = getdate() where OrderID = " + textBox1.Text);
                }
            }
            if (radioButton5.Checked && textBox6.Text != "")
            {//update assigned rider
                c.Inserts("update orders set rider_riderID = " + textBox6.Text + " where OrderID = " + textBox1.Text);
            }
            if (radioButton6.Checked)
            {//remove rider by assigning null
                c.Inserts("update orders set rider_riderID = null where OrderID = " + textBox1.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {//delete order
            ConnectingData c = new ConnectingData();
            if (textBox4.Text == "")
            {//error message for null
                MessageBox.Show("Enter ID of order to remove.");
            }
            else
            {//delete data from all tables
                //delete payment details from payment table
                c.Inserts("delete from Payment where PaymentID = (select payment_paymentID from orders where orderID = " + textBox4.Text + ")");
                //delete order item details from orderbyitem table
                c.Inserts("delete from OrderbyItem where OrderID = " + textBox4.Text);
                //delete order from orders table
                c.Inserts("delete from Orders where OrderID = " + textBox4.Text);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {//show all orders using VIEW
            ConnectingData c = new ConnectingData();
            dataGridView1.DataSource = c.Select("select * from ViewOrder");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {//show all orders with particular food item or customer
            ConnectingData c = new ConnectingData();
            if (textBox2.Text != "")
            {//select by customer
                dataGridView1.DataSource = c.Select("select * from ViewOrder where [Customer ID] = " + textBox2.Text);
            }
            else if (comboBox3.Text != "")
            {//select by food item using a stored procedure
                dataGridView1.DataSource = c.Select("exec viewOrdersByItem @itemname = '" + comboBox3.Text + "'");
            }
            else if (textBox5.Text != "")
            {//select by order id using VIEW
                dataGridView1.DataSource = c.Select("select * from ViewOrder where ID = " + textBox5.Text);
            }
            else if (comboBox2.Text != "")
            {//select by order status using VIEW
                dataGridView1.DataSource = c.Select("select * from ViewOrder where [Order Status] = '" + comboBox2.Text + "'");
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {//search by customer id selected
            if (radioButton1.Enabled == true)
            {
                textBox2.Enabled = true;
                //disable rest
                comboBox3.Text = "";
                comboBox3.Enabled = false;
                textBox5.Clear();
                textBox5.Enabled = false;
                comboBox2.Text = "";
                comboBox2.Enabled = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {//search by food item selected
            if (radioButton2.Enabled == true)
            {
                comboBox3.Enabled = true;
                //disable rest
                textBox2.Clear();
                textBox2.Enabled = false;
                textBox5.Clear();
                textBox5.Enabled = false;
                comboBox2.Text = "";
                comboBox2.Enabled = false;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {//search by order id selected
            if (radioButton3.Enabled == true)
            {
                textBox5.Enabled = true;
                //disable rest
                textBox2.Clear();
                textBox2.Enabled = false;
                comboBox3.Text = "";
                comboBox3.Enabled = false;
                comboBox2.Text = "";
                comboBox2.Enabled = false;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {//search by status selected
            if (radioButton3.Enabled == true)
            {
                comboBox2.Enabled = true;
                //disable rest
                textBox2.Clear();
                textBox2.Enabled = false;
                comboBox3.Enabled = false;
                textBox5.Clear();
                textBox5.Enabled = false;
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                textBox6.Enabled = true;
            }
            else
            {
                textBox6.Clear();
                textBox6.Enabled = false;
            }
        }

        private void OrdersAdminView_Load(object sender, EventArgs e)
        {
            ConnectingData c = new ConnectingData();
            DataTable ds = c.Select("Select fooditemID, itemName from fooditem");
            DataRow row = ds.NewRow(); //adding default null value as first element in combobox
            row[0] = 0;
            row[1] = "";
            ds.Rows.InsertAt(row, 0);
            comboBox3.DataSource = ds;
            comboBox3.DisplayMember = "itemName";
            comboBox3.ValueMember = "fooditemID";
        }
    }
}
