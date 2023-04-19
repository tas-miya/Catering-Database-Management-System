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
    public partial class WeeklyMenu : Form
    {
        public WeeklyMenu()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {//show all menus using VIEW
            ConnectingData c = new ConnectingData();
            dataGridView1.DataSource = c.Select("select * from viewAllMenus");
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
        private void WeeklyMenu_Load(object sender, EventArgs e)
        {
        }

        private void button8_Click(object sender, EventArgs e)
        {//show all food items using VIEW
            ConnectingData c = new ConnectingData();
            dataGridView1.DataSource = c.Select("select ID, Item, Category from showAllFoodItems");
        }

        

        private void button9_Click(object sender, EventArgs e)
        {
            //checking data is not null and dates are valid
            if (listBox1.Items.Count < 7)
            {
                MessageBox.Show("Specify items for all days");
            }
            else if (dateTimePicker6.Value < DateTime.Now || dateTimePicker5.Value <= DateTime.Now)
            {
                MessageBox.Show("Invalid date(s).");
            }
            else
            {//add menu
                ConnectingData c = new ConnectingData();
                //add new menu in weeklyMenu table
                c.Inserts("insert into weeklyMenu (weeklyMenuID, ValidFrom, ValidTill) values ((select max(WeeklyMenuID) from WeeklyMenu)+1, '" + dateTimePicker6.Value + "', '" + dateTimePicker5.Value + "')");
                //add menu details in weeklyMenuItems table
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    c.Inserts("insert into weeklyMenuItems (weeklyMenuID, Weekday, FoodItem_FoodItemID) values ((select max(WeeklyMenuID) from WeeklyMenu), '" + listBox1.Items[i].ToString() + "', " + listBox2.Items[i].ToString() + ")");
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {//add day and item id to listviews
            bool day_exists = false;
            if (comboBox1.Text != "" && textBox9.Text != "")
            {
                for(int i = 0; i < listBox1.Items.Count; i++)
                {
                    if (listBox1.Items[i].ToString() == comboBox1.Text)
                    {
                        MessageBox.Show("Day already has assigned item.");
                        day_exists = true;
                        break;
                    }
                }
                if (!day_exists)
                {
                    listBox1.Items.Add(comboBox1.Text);
                    listBox2.Items.Add(textBox9.Text);
                }
            }
            else
            {
                MessageBox.Show("Specify day and food item.");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {//remove item from temp adding list
            if (listBox1.Items.Count <= 0)
            {
                MessageBox.Show("There are no items to remove.");
            }
            else if (listBox1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Select an item to remove.");
            }
            else
            {
                int ind = listBox1.SelectedIndex;
                listBox2.Items.RemoveAt(ind);
                listBox1.Items.RemoveAt(ind);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {//update menu
            ConnectingData c = new ConnectingData();
            if (textBox2.Text != "")
            {//if ID not null
                if (comboBox2.Text != "")
                {//updating item for a particular day
                    if (textBox1.Text != "")
                    {
                        c.Inserts("update weeklyMenuItems set FoodItem_FoodItemID = " + textBox1.Text + "where weeklyMenuID = " + textBox2.Text + "and weekday = '" + comboBox2.Text + "'");
                    }
                    else
                    {
                        MessageBox.Show("Enter ID of food item to update!");
                    }
                }
                if (dateTimePicker4.Value != DateTime.Now)
                {
                    c.Inserts("update weeklyMenu set ValidFrom = '" + dateTimePicker4.Value.ToString() + "' where weeklyMenuID = " + textBox2.Text);
                }
                if (dateTimePicker1.Value != DateTime.Now)
                {
                    c.Inserts("update weeklyMenu set ValidTill = '" + dateTimePicker1.Value.ToString() + "' where weeklyMenuID = " + textBox2.Text);
                }
            }
            else
            {
                MessageBox.Show("Enter ID of menu to update!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {//delete menu
            ConnectingData c = new ConnectingData();
            c.Inserts("delete from weeklyMenuItems where weeklyMenuID =" + textBox3.Text);
            c.Inserts("delete from weeklyMenu where weeklyMenuID =" + textBox3.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {//show menu of selected week using VIEW
            ConnectingData c = new ConnectingData();
            dataGridView1.DataSource = c.Select("select * from viewAllMenus where validfrom <= '" + dateTimePicker2.Value.ToString() + "' and validtill >= '" + dateTimePicker2.Value.ToString() + "'");
        }
    }
}
