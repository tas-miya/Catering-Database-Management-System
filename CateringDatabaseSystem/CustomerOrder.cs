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
    public partial class CustomerOrder : Form
    {
        public CustomerOrder()
        {
            InitializeComponent();
        }

        DataTable cart = new DataTable(); //globally declaring cart to add and remove items from
        private void CustomerDetails_Load(object sender, EventArgs e)
        {
            //populating categories combobox with values from the database (categories table)
            ConnectingData c = new ConnectingData();   
            DataTable ds = c.Select("Select CategoriesID, CategoryName from Categories"); 
            DataRow row = ds.NewRow(); //adding default null value as first element in combobox
            row[0] = 0;
            row[1] = "";
            ds.Rows.InsertAt(row, 0);

            comboBox3.DataSource = ds;
            comboBox3.DisplayMember = "CategoryName";
            comboBox3.ValueMember = "CategoriesID";

            //populating region combobox with values from the database (region table)
            ConnectingData c2 = new ConnectingData();
            DataTable ds2 = c2.Select("Select regionID, regionDescription from region");
            DataRow row2 = ds2.NewRow(); //adding default null value as first element in combobox
            row2[0] = 0;
            row2[1] = "";
            ds2.Rows.InsertAt(row2, 0);

            comboBox2.DataSource = ds2;
            comboBox2.DisplayMember = "regionDescription";
            comboBox2.ValueMember = "regionID";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {//confirm order 
            // error messages for null values
             if (textBox2.Text == "")
            {
                MessageBox.Show("Please enter your name.");
            }
            else if (textBox6.Text == "")
            {
                MessageBox.Show("Please enter your contact number.");
            }
            else if (comboBox2.Text == "")
            {
                MessageBox.Show("Please select your region.");
            }
            else if (textBox9.Text == "")
            {
                MessageBox.Show("Please enter your address.");
            }
            else if (textBox11.Text == "")
            {
                MessageBox.Show("Please enter your email.");
            }
            else if (textBox14.Text == "0")
            {
                MessageBox.Show("Please select items to order!");
            }
            else if (!radioButton4.Checked && !radioButton5.Checked)
            {
                MessageBox.Show("Please select a payment method!");
            }
            else if (radioButton4.Checked && textBox4.Text == "")
            {
                MessageBox.Show("Please enter your credit card number.");
            }
            else if (dateTimePicker1.Value < DateTime.Today)
            {
                MessageBox.Show("Invalid date!\nAn order can not be placed for before today.");
            }
             else
            {//add order to database
                //adding customer details in customers table only if the exact record doesn't exist
                ConnectingData c = new ConnectingData();
                c.Inserts("if not exists (select * from Customers where (CustomerFName = '" + textBox16.Text + "' and CustomerLName = '" + textBox2.Text + "' and CustomerContactNo = '" + textBox6.Text + "' and CustomerAddress = '" + textBox9.Text + "' and Email = '" + textBox11.Text + "' and CreditCardNo = '" + textBox4.Text + "')) begin insert into customers (customerID, customerFName, customerLName, customerContactNo, customerAddress, alternatePhone, email, creditCardNo) values ((select max(customerID) from customers)+1, '" + textBox16.Text + "', '" + textBox2.Text + "' , '" + textBox6.Text + "' , '" + textBox9.Text + "' , '" + textBox8.Text + "' , '" + textBox11.Text + "' , '" + textBox4.Text + "') end");
                //adding payment details to payment table
                if (radioButton4.Checked)
                {
                    c.Inserts("insert into payment (paymentID, paymentType) values ((select max(paymentID) from payment)+1, 'Credit Card')");
                } 
                else if (radioButton5.Checked)
                {
                    c.Inserts("insert into payment (paymentID, paymentType) values ((select max(paymentID) from payment)+1, 'COD')");
                }
                //adding order to orders table
                c.Inserts("insert into orders (orderID, Payment_PaymentID, Region_RegionID, Customers_CustomerID, OrderDate, RequiredDate, OrderStatus, TotalPrice) values ((select max(orderID) from orders)+1, (select max(paymentID) from payment), (select regionID from region where regionDescription = '" + comboBox2.Text + "'), (select customerID from customers where (customerFName = '" + textBox16.Text + "' and customerLName = '" + textBox2.Text + "' and customerContactNo = '" + textBox6.Text + "' and customerAddress = '" + textBox9.Text + "' and email = '" + textBox11.Text + "' and creditCardNo = '" + textBox4.Text + "')), getdate(), '" + dateTimePicker1.Value + "', 'In Process', " + textBox10.Text + ")");
                
                //adding each food item in order to orderByItem table
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    double quantity = double.Parse(row.Cells[1].Value.ToString()); //getting quantity of food item from listview2
                    c.Inserts("insert into orderByItem (orderID, FoodItem_FoodItemID, quantity, discount, unitprice) values ((select max(orderID) from orders), (select foodItemID from foodItem where itemName = '" + row.Cells[0].Value.ToString() + "'), " + quantity + ", " + textBox13.Text + ", (select unitprice from foodItem where itemName = '" + row.Cells[0].Value.ToString() + "')) ");
                }
                updateIngrStock();
                MessageBox.Show("Thank You! \nYour order has been recorded.");
                this.Close();
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {//if payment method is COD, disabling credit card textbox
            if (radioButton5.Checked)
            {
                textBox4.Clear();
                textBox4.Enabled = false;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        //enabling categories combobox only if categories is selected
        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked)
            {
                comboBox3.Enabled = true;
            }
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {//todays special - get from weekly menu
            if (radioButton7.Checked == true)
            {
                comboBox3.Enabled = false; //disable categories box
                textBox3.Text = "0"; //resetting price to 0
                textBox7.Text = "Enter Amount";
                textBox5.Text = "Enter Weight (Kg)";
                int dayOfWeek; 
                int.TryParse(DateTime.Now.DayOfWeek.ToString(), out dayOfWeek);
                string[] day = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
                ConnectingData c = new ConnectingData();
                //getting item from weekly menu for today if a menu is valid for today
                dataGridView1.DataSource = c.Select("exec displayTodaysSpecial @weekday = '"+ day[dayOfWeek] + "', @today = '"+DateTime.Today.ToString()+"'");
                if (dataGridView1.Rows.Count <= 0)
                {//if no menu exists for today's date
                    MessageBox.Show("There is no item for Today's Special.");
                }
                else
                {//enabling relevant textbox for quantity
                    textBox3.Text = "0"; //resetting price to 0
                    if (dataGridView1.SelectedCells[3].Value.ToString() == "Weight (Kg)")
                    {//measured in weight 
                        textBox5.Enabled = true;
                        textBox7.Text = "Enter Amount"; //reset default
                        textBox7.Enabled = false; //disable amount textbox
                    }
                    else
                    {//measured in units 
                        textBox7.Enabled = true;
                        textBox5.Text = "Enter Weight (Kg)"; //reset default
                        textBox5.Enabled = false; //disable weight textbox
                    }
                }
            }
        }

       

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private bool isFoodAvailable()
        {//if quantity entered is possible, calculate price
            ConnectingData c = new ConnectingData();
            DataTable dt = c.Select("exec GetIngrQtyInItem @ItemName = '" + dataGridView1.SelectedCells[0].Value.ToString() + "'"); //datatable to temporarily store required and available ingredients of selected fooditem 
            bool isAvailable = true; //to check if entered amount of item is available
            if (textBox5.Enabled)
            {
                foreach (DataRow row in dt.Rows)
                {//check each ingredient for availability
                    double serving = double.Parse(dataGridView1.SelectedCells[2].Value.ToString());
                    double qty_required = double.Parse(row["Quantity_grams"].ToString()) * (double.Parse(textBox5.Text) / serving); //calculating quantity of ingredient required for amount of item customer wants
                    double qty_in_stock = double.Parse(row["QtyInStock_kg"].ToString()) * 1000; //converting to grams
                    if (qty_required > qty_in_stock)
                    {
                        isAvailable = false;
                        break;
                    }
                }
            }
            else if (textBox7.Enabled)
            {
                foreach (DataRow row in dt.Rows)
                {//check each ingredient for availability
                    double serving = double.Parse(dataGridView1.SelectedCells[2].Value.ToString());
                    double qty_required = double.Parse(row["Quantity_grams"].ToString()) * (int.Parse(textBox7.Text) / serving); //calculating quantity of ingredient required for amount of item customer wants
                    double qty_in_stock = double.Parse(row["QtyInStock_kg"].ToString()) * 1000; //converting to grams
                    if (qty_required > qty_in_stock)
                    {
                        isAvailable = false;
                        break;
                    }
                }
            }
            return isAvailable;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {//calculating and displaying price of amount entered of food item 
            if (textBox7.Text != "Enter Amount")
            {
                int temp;
                if (!int.TryParse(textBox7.Text, out temp))
                {//checking if the value entered is invalid
                    textBox1.Text = "Invalid, please enter a whole number!";
                    button1.Enabled = false;
                }
                else if (int.Parse(textBox7.Text) < int.Parse(dataGridView1.SelectedCells[2].Value.ToString()))
                {//if quantity is less than unit size
                    textBox1.Text = "Invalid, quantity cannot be below serving size!";
                    button1.Enabled = false;
                }
                else if (int.Parse(textBox7.Text)%int.Parse(dataGridView1.SelectedCells[2].Value.ToString()) != 0)
                {//if quantity is not multiple of serving
                    textBox1.Text = "Invalid, quantity must be a multiple of serving size!";
                    button1.Enabled = false;
                }
                else
                {
                    if (isFoodAvailable())
                    {
                        textBox1.Text = "";
                        string unitprice = dataGridView1.SelectedCells[1].Value.ToString();
                        string unitquantity = dataGridView1.SelectedCells[2].Value.ToString();
                        textBox3.Text = (double.Parse(textBox7.Text) * double.Parse(unitprice) / double.Parse(unitquantity)).ToString();
                        button1.Enabled = true;
                    }
                    else
                    {
                        button1.Enabled = false;
                        textBox1.Text = "Quantity entered exceeds availability.";
                    }
                }
            } 
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {//search by category/ category selected changed
            textBox3.Text = "0"; //resetting price to 0
            textBox7.Text = "Enter Amount";
            textBox5.Text = "Enter Weight (Kg)";
            if (radioButton8.Checked == true & comboBox3.Text != "")
            {//populating datagridview with food items in selected category
                ConnectingData c = new ConnectingData();
                dataGridView1.DataSource = c.Select("exec displayItems @categoryName = '" + comboBox3.Text + "'");
                //enabling relevant textbox for quantity
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    textBox3.Text = "0"; //resetting price to 0
                    if (row.Cells[3].Value.ToString() == "Weight (Kg)")
                    {//measured in weight 
                        textBox5.Enabled = true;
                        textBox7.Text = "Enter Amount"; //reset default
                        textBox7.Enabled = false; //disable amount textbox
                    }
                    else
                    {//measured in units 
                        textBox7.Enabled = true;
                        textBox5.Text = "Enter Weight (Kg)"; //reset default
                        textBox5.Enabled = false; //disable weight textbox
                    }
                }
            }
            else if (radioButton8.Checked == true & comboBox3.Text == "")
            {//error message if no category selected
                MessageBox.Show("Please select a category!");
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {//calculating and displaying price of amount entered of food item by weight
            if (textBox5.Text != "Enter Weight (Kg)")
            {
                double temp;
                if (!double.TryParse(textBox5.Text, out temp))
                {//checking if the value entered is invalid
                    textBox1.Text = "Invalid, please enter a number!";
                    button1.Enabled = false;
                }
                else if (double.Parse(textBox5.Text) < double.Parse(dataGridView1.SelectedCells[2].Value.ToString()))
                {//if quantity is less than unit size
                    textBox1.Text = "Invalid, quantity cannot be below serving size!";
                    button1.Enabled = false;
                }
                else 
                {
                    if (isFoodAvailable())
                    {
                        textBox1.Text = "";
                        string unitprice = dataGridView1.SelectedCells[1].Value.ToString();
                        string unitquantity = dataGridView1.SelectedCells[2].Value.ToString();
                        textBox3.Text = (double.Parse(textBox5.Text) * double.Parse(unitprice) / double.Parse(unitquantity)).ToString();
                        button1.Enabled = true;
                    }
                    else
                    {
                        button1.Enabled = false;
                        textBox1.Text = "Quantity entered exceeds availability.";
                    }
                }
            } 

        } 

        private void updateIngrStock()
        {//function to update qty of each ingredient after it is added to cart.
            ConnectingData c = new ConnectingData();
            
            foreach (DataGridViewRow item in dataGridView2.Rows)
            {
                DataTable dt = c.Select("exec GetIngrQtyInItem @ItemName = '" + item.Cells[0].Value.ToString() + "'"); //datatable to temporarily store ingredient details of selected fooditem 
                foreach (DataRow row in dt.Rows)
                {//update qty of each ingredient in database
                    double unit_quantity = double.Parse(row["unitquantity"].ToString());
                    double qty_required = (double.Parse(row["Quantity_grams"].ToString()) * (double.Parse(item.Cells[1].Value.ToString()) / unit_quantity)) / 1000; //qty required in KG
                    c.Inserts("update Ingredients set QtyInStock_kg = QtyInStock_kg - " + qty_required + " where ingredientsID = " + row["ingredientsID"].ToString());
                }
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {//add to cart  
            if (textBox3.Text == "0")
            {//error message if quantity not specified
                MessageBox.Show("Please determine item and quantity to order!");
            }
            else
            {
                if (dataGridView2.Rows.Count <= 0 && cart.Columns.Count <= 0)
                {//if cart is empty, create add columns to cart table
                    cart.Columns.Add("Item");
                    cart.Columns.Add("Quantity");
                    cart.Columns.Add("TotalPrice");
                }
                //add item and qty entered, and price to cart
                if (textBox5.Enabled)
                {
                    cart.Rows.Add(dataGridView1.SelectedCells[0].Value, textBox5.Text, textBox3.Text);
                }
                if (textBox7.Enabled)
                {
                    cart.Rows.Add(dataGridView1.SelectedCells[0].Value, textBox7.Text, textBox3.Text);
                }
                dataGridView2.DefaultCellStyle.Font = new Font("CeraPro-Regular", 9);
                dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font("Cera Pro", 9);
                dataGridView2.DataSource = cart;
                button2.Enabled = true; //enable remove and clear buttons
                button4.Enabled = true;
            }
            
        } 

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {//if date selected is before today, it is invalid
            if (dateTimePicker1.Value < DateTime.Today)
            {
                textBox15.Text = "Invalid \n date!";
            }
            else
            {
                textBox15.Text = "";
            }
        }
        
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {//if payment method is online, enabling credit card textbox
            if (radioButton4.Checked)
            {
                textBox4.Enabled = true;
            }
        } 

        private void groupBox3_ControlAdded(object sender, ControlEventArgs e)
        {

        }

        private void listView1_ControlAdded(object sender, ControlEventArgs e)
        {
            
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {//remove selected item(s) from cart
            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                //cart.Rows.Remove();
                dataGridView2.Rows.Remove(row);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {//clear cart
            cart.Rows.Clear();
            
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                dataGridView2.Rows.Remove(row);
            }
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {//if diff food item selected from list, update price
            textBox3.Text = "0"; //resetting price to 0
            if (dataGridView1.SelectedCells[3].Value.ToString() == "Weight (Kg)")
            {//measured in weight
                textBox5_TextChanged(sender, e); 
            }
            else
            {//measured in units
                textBox7_TextChanged(sender, e);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {//update costs when a row is removed
            if (dataGridView2.Rows.Count <= 0)
            {//reset to default if cart is empty
                button2.Enabled = false;
                button4.Enabled = false;
                textBox14.Text = "0";
                textBox10.Text = "200";
            }
            //calculating and displaying cart total
            double cart_total = 0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                cart_total = cart_total + double.Parse(row.Cells[2].Value.ToString());
                textBox14.Text = cart_total.ToString();
            }
            //calculating and displaying total cost
            double total_cost = 200 + (cart_total * (100 - int.Parse(textBox13.Text)) / 100);
            textBox10.Text = total_cost.ToString();
        }

        private void dataGridView2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {//updating cost when item added to cart
            //calculating and displaying cart total
            double cart_total = 0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                cart_total = cart_total + double.Parse(row.Cells[2].Value.ToString());
                textBox14.Text = cart_total.ToString();
            }
            //calculating and displaying total cost
            double total_cost = 200 + ( cart_total * (100 - int.Parse(textBox13.Text)) / 100);
            textBox10.Text = total_cost.ToString();  
        }
    }
}

