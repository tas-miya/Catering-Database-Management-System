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
    public partial class IngredientsAdmin : Form
    {
        public IngredientsAdmin()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {//adding ingredients
            ConnectingData c = new ConnectingData();
            if (textBox2.Text == "" || textBox6.Text == "")
            {
                MessageBox.Show("Fill all fields!");
            }
            else
            {
                c.Inserts("insert into ingredients (ingredientsID, ingredientName, QtyInStock_kg) values ((select max(ingredientsID) from ingredients)+1, '" + textBox2.Text + "'," + textBox6.Text + ")");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {// show all ingredients
             ConnectingData c = new ConnectingData();
            dataGridView1.DataSource = c.Select("select * from ingredients");
        }

        private void button3_Click(object sender, EventArgs e)
        {//update ingredients
            if (textBox7.Text != "")
            {//if an ID is entered
                ConnectingData c = new ConnectingData();
                if (textBox3.Text != "")
                {//quantity
                    c.Inserts("update ingredients set QtyInStock_kg = " + textBox3.Text + "where ingredientsID = " + textBox7.Text);
                }
                else
                {
                    MessageBox.Show("Specify quantity to update.");
                }
            }
            else
            {
                MessageBox.Show("Enter ID of ingredient to update.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {//delete ingredient
            ConnectingData c = new ConnectingData();
            if (textBox4.Text == "")
            {
                MessageBox.Show("Enter ingredient ID");
            }
            else
            {
                c.Inserts("delete from ingredients where ingredientsID =" + textBox4.Text);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {// show all items with this ingredient using VIEW
            ConnectingData c = new ConnectingData();
            if (textBox9.Text == "")
            {
                MessageBox.Show("Enter ingredient ID");
            }
            else
            {
                dataGridView1.DataSource = c.Select("select [Food Item], Ingredient, [Quantity Required (grams)] from ItemsWithIngrnt where ID = " + textBox9.Text);
            }
        }
    }
}
