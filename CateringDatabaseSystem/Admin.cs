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
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Equals("Orders"))
            {
                var OrdersAdminView = new OrdersAdminView();
                OrdersAdminView.Show();
            }
            else if (comboBox1.Text.Equals("Customers"))
            {
                var CustomersAdminView = new CustomersAdminView();
                CustomersAdminView.Show();
            }
            else if (comboBox1.Text.Equals("Ingredients"))
            {
                var IngredientsAdmin = new IngredientsAdmin();
                IngredientsAdmin.Show();
            }
            else if (comboBox1.Text.Equals("Food Items"))
            {
                var FoodItems = new FoodItems();
                FoodItems.Show();
            }
            else if (comboBox1.Text.Equals("Weekly Menu"))
            {
                var WeeklyMenu = new WeeklyMenu();
                WeeklyMenu.Show();
            }
            else if (comboBox1.Text.Equals("Riders"))
            {
                var RiderAdminView = new RiderAdminView();
                RiderAdminView.Show();
            }
            else if (comboBox1.Text.Equals("Categories"))
            {
                var Categories = new Categories();
                Categories.Show();
            }
            else if (comboBox1.Text.Equals("Region"))
            {
                var Region = new Region();
                Region.Show();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {//showing all admins
            ConnectingData c = new ConnectingData();    //selecting admins from the table
            DataTable ds = c.Select("Select * from Admin"); //displaying admins in the grid view
            dataGridView2.DataSource = ds;
        }

        private void button1_Click(object sender, EventArgs e)
        {//Inserting admins
            if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "")
            {//error message
                MessageBox.Show("Please fill all fields!");
            }
            else
            {
                ConnectingData c = new ConnectingData();
                c.Inserts("Insert into Admin(AdminID, [Name], Role, Email, Password_2, DateAdded)\n" + "values((select max(AdminID) from Admin)+1, '" + textBox2.Text + "', '" + textBox5.Text + "', '" + textBox3.Text + "', '" + textBox4.Text + "' , getdate())");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {//updating admins, working fine
            ConnectingData c = new ConnectingData();
            if (textBox11.Text != "" && textBox11.Text != "Enter ID of Admin to update")
            {
                int i = 0;
                if (!int.TryParse(textBox11.Text, out i)) // || !int.TryParse(textBox10.Text, out i)
                {
                    MessageBox.Show("Invalid ID: enter digits only.");
                }
                else
                {
                    if (textBox7.Text != "")
                    {
                        c.Inserts("Update Admin set Password_2 = '" + textBox7.Text + "' where AdminId = " + textBox11.Text);
                    }
                    if (textBox6.Text != "")
                    {
                        c.Inserts("Update Admin set Role = '" + textBox6.Text + "' where AdminId = " + textBox11.Text);
                    }
                }
            }
            else
            {//error message
                MessageBox.Show("Please enter the ID of admin to update.");
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {//delete admin - only owner can delete
            ConnectingData c = new ConnectingData();
            DataTable ds = c.Select("select role from admin where adminID = " + textBox8.Text);
            if (ds.Rows[0][0].ToString() == "Owner")
            {
                if (textBox9.Text != "" && textBox9.Text != "Enter ID of Admin to remove")
                {
                    c.Inserts("Delete from Admin where AdminId = " + textBox9.Text);
                }
                else
                {//error message if null
                    MessageBox.Show("Please enter the ID of admin to remove.");
                }
            }
            else
            {
                MessageBox.Show("Only the owner can remove admins.");
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {//generate report
            ConnectingData c = new ConnectingData();
            int days = int.Parse((dateTimePicker2.Value - dateTimePicker1.Value).Days.ToString());
            //total orders placed
            DataTable ds = c.Select("select count(*) as 'Total Orders Placed' from Orders where OrderDate >= '"+dateTimePicker1.Value.ToString()+ "' and OrderDate <= '" + dateTimePicker2.Value.ToString() + "'"); 
            textBox14.Text = ds.Rows[0][0].ToString();
            //avg orders placed/day
            textBox12.Text = (int.Parse(ds.Rows[0][0].ToString()) / days).ToString();
            //total orders delivered
            ds = c.Select("select count(*) as 'Total Orders Delivered' from Orders where OrderDate >= '" + dateTimePicker1.Value.ToString() + "' and OrderDate <= '" + dateTimePicker2.Value.ToString() + "' and OrderStatus = 'Delivered'");
            textBox13.Text = ds.Rows[0][0].ToString();
            //avg orders delivered/day
            textBox1.Text = (int.Parse(ds.Rows[0][0].ToString()) / days).ToString();
            //most frequent customer
            ds = c.Select("exec MostFrequentCustomer @datefrom = '" + dateTimePicker1.Value.ToString() + "', @dateto = '" + dateTimePicker2.Value.ToString() + "'");
            textBox15.Text = ds.Rows[0][1].ToString() + " (ID: " + ds.Rows[0][0].ToString() + ")";
            //most popular item
            ds = c.Select("exec MostPopularItem @datefrom = '" + dateTimePicker1.Value.ToString() + "', @dateto = '" + dateTimePicker2.Value.ToString() + "'");
            textBox16.Text = ds.Rows[0][1].ToString() + " (ID: " + ds.Rows[0][0].ToString() + ")";
            //region with most orders
            ds = c.Select("exec RegionMostOrders @datefrom = '" + dateTimePicker1.Value.ToString() + "', @dateto = '" + dateTimePicker2.Value.ToString() + "'");
            textBox18.Text = ds.Rows[0][1].ToString() + " (ID: " + ds.Rows[0][0].ToString() + ")";

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            ConnectingData c = new ConnectingData();
            if (textBox10.Text != "")
            {
                DataTable ds = c.Select("select role from admin where adminID = " + textBox10.Text);
                if (ds.Rows.Count > 0)
                {
                    if (ds.Rows[0][0].ToString() == "Owner")
                    {
                        textBox6.Enabled = true;
                    }
                }
                else
                {
                    textBox6.Text = "";
                    textBox6.Enabled = false;
                }
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            ConnectingData c = new ConnectingData();
            if (textBox8.Text != "")
            {
                DataTable ds = c.Select("select role from admin where adminID = " + textBox8.Text);
                if (ds.Rows.Count > 0)
                {
                    if (ds.Rows[0][0].ToString() == "Owner")
                    {
                        textBox9.Enabled = true;
                        button3.Enabled = true;
                    }
                }
                else
                {
                    textBox9.Text = "Enter ID of Admin to remove";
                    textBox9.Enabled = false;
                }
            }
        }
    }
}
