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
    public partial class Categories : Form
    {
        public Categories()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {//add category
            ConnectingData c = new ConnectingData();
            if (textBox2.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("Fill all fields");
            }
            else
            {
                c.Inserts("insert into categories (CategoriesID, CategoryName, MeasuredIn) values ((select max(CategoriesID) from categories)+1, '" + textBox2.Text + "', '" + comboBox2.Text + "')");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {//show all categories
            ConnectingData c = new ConnectingData();
            dataGridView1.DataSource = c.Select("select * from Categories");
        }

        private void button2_Click(object sender, EventArgs e)
        {//delete category
            ConnectingData c = new ConnectingData();
            if (textBox4.Text == "")
            {
                MessageBox.Show("Enter ID of category to delete.");
            }
            else
            {
                int i = 0;
                if (!int.TryParse(textBox4.Text, out i)
                  )
                {
                    MessageBox.Show("Invalid ID: enter digits only.");
                }
                else
                {
                    c.Inserts("delete from Categories where CategoriesID =" + textBox4.Text);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {//update category
            ConnectingData c = new ConnectingData();
            if (textBox7.Text == "")
            {
                MessageBox.Show("Enter ID of category to update.");
            }
            else if (textBox3.Text == "" && comboBox1.Text == "")
            {
                MessageBox.Show("Enter details to update.");
            }
            else
            {
                int i = 0;
                if (!int.TryParse(textBox7.Text, out i)
                  )
                {
                    MessageBox.Show("Invalid ID: enter digits only.");
                }
                else
                {
                    if (textBox3.Text != "")
                    {//name
                        c.Inserts("update Categories set CategoryName = '" + textBox3.Text + "'" + "where CategoriesID = " + textBox7.Text);
                    }
                    if (comboBox1.Text != "")
                    {//measured in
                        c.Inserts("update Categories set MeasuredIn = '" + comboBox1.Text + "'" + "where CategoriesID = " + textBox7.Text);
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {//show all food items in this category
            ConnectingData c = new ConnectingData();
            dataGridView1.DataSource = c.Select("select * from showAllFoodItems where Category = '" + comboBox3.Text + "'");

        }

        private void Categories_Load(object sender, EventArgs e)
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
        }
    }
}
