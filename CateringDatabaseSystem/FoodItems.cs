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
    public partial class FoodItems : Form
    {
        public FoodItems()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {//add ingredient from combobox to listview
            if (comboBox1.Text == "" || textBox6.Text == "")
            {
                MessageBox.Show("Select ingredient and quantity to add");
            }
            else
            {
                int i = 0;

                if (!int.TryParse(textBox6.Text, out i))
                {
                    MessageBox.Show("Invalid quantity, enter a number.");
                }
                else
                {
                    listView1.Items.Add(comboBox1.Text);
                    listView2.Items.Add(textBox6.Text);
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {//adding each item to fooditems table, and adding itemid and ingid to junction table
            //int i = listView1.Items.Count;
            ConnectingData c = new ConnectingData();
            if (textBox2.Text == "" || comboBox2.Text == "" || textBox5.Text == "" || textBox1.Text == "" || listView1.Items.Count == 0)
            {
                MessageBox.Show("Please Fill all fields");
            }
            else
            {

                int j = 0;
                if (!int.TryParse(textBox5.Text, out j) || !int.TryParse(textBox1.Text, out j)
                    )
                {
                    MessageBox.Show("Incorrect Unit Price or Unit Quantity");
                }
                else
                {
                    c.Inserts("insert into fooditem (fooditemID, itemName, Categories_CategoriesID, unitprice, unitquantity) values ((select max(fooditemID) from fooditem)+1 , '" + textBox2.Text + "', (select categoriesID from categories where categoryname = '" + comboBox2.Text + "'), " + textBox5.Text + ", " + textBox1.Text + ")");
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        c.Inserts("insert into ingredients_for_fooditem (fooditem_fooditemID, ingredients_ingredientsID, quantity_grams) values ((select max(fooditemID) from fooditem),(select ingredientsid from ingredients where ingredientname = '" + listView1.Items[i].Text + "')," + listView2.Items[i].Text + ")");
                    }
                    listView1.Clear();
                    listView2.Clear();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {//delete food item
            ConnectingData c = new ConnectingData();
            if (textBox4.Text == "")
            {
                MessageBox.Show("Insert ID of food item to delete");
            }
            else
            {
                int i = 0;
                if (!int.TryParse(textBox4.Text, out i))
                {
                    MessageBox.Show("Invalid ID");
                }
                else
                {
                    c.Inserts("delete from ingredients_for_fooditem where fooditem_fooditemID =" + textBox4.Text);
                    c.Inserts("delete from fooditem where fooditemID =" + textBox4.Text);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {//update food item
            ConnectingData c = new ConnectingData();
            if (textBox7.Text != "")
            {//if ID not null
                int k = 0;
                if (!int.TryParse(textBox7.Text, out k))
                {
                    MessageBox.Show("Invalid ID");
                }
                else
                {
                    if (textBox3.Text != "")
                    {//updating unitprice
                        int i = 0;
                        if (!int.TryParse(textBox7.Text, out i))
                        {
                            MessageBox.Show("Invalid Unit Price");
                        }
                        else
                        {
                            c.Inserts("update fooditem set unitprice = " + textBox3.Text + "where fooditemID = " + textBox7.Text);
                        }
                    }
                    if (textBox8.Text != "")
                    {//updating item name
                        c.Inserts("update fooditem set itemName = '" + textBox8.Text + "' where fooditemID = " + textBox7.Text);
                    }
                    if (comboBox3.Text != "")
                    {//updating item category
                        c.Inserts("update fooditem set Categories_CategoriesID = (select categoriesID from categories where categoryname = '" + comboBox3.Text + "') where fooditemID = " + textBox7.Text);
                    }
                    if (textBox10.Text != "")
                    {//updating item quantity
                        int i = 0;
                        if (!int.TryParse(textBox10.Text, out i))
                        {
                            MessageBox.Show("Invalid item quantity");
                        }
                        else
                        {
                            c.Inserts("update fooditem set unitQuantity = " + textBox10.Text + " where fooditemID = " + textBox7.Text);
                        }
                    }
                    if (comboBox4.Text != "" && comboBox4.Text != comboBox5.Text) //if ingredient to add and remove are diff
                    {//adding ingredient
                        if (textBox11.Text != "")
                        {//if quantity given
                            int i = 0;
                            if (!int.TryParse(textBox11.Text, out i))
                            {
                                MessageBox.Show("Invalid ingredient quantity");
                            }
                            else
                            {
                                c.Inserts("if not exists (select ingredients_ingredientsID from ingredients_for_fooditem where ingredients_ingredientsID = (select ingredientsid from ingredients where ingredientname = '" + comboBox4.Text + "')) begin insert into ingredients_for_fooditem (fooditem_fooditemID, ingredients_ingredientsID, quantity_grams) values (" + textBox7.Text + ",(select ingredientsid from ingredients where ingredientname = '" + comboBox4.Text + "')," + textBox11.Text + ") end");
                                MessageBox.Show("Note: ingredient will only be added if it does not already exist. There will be no change otherwise.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Enter ingredient quantity (grams).");
                        }
                    }
                    else if (comboBox4.Text != "" && comboBox4.Text == comboBox5.Text)
                    {
                        MessageBox.Show("Cannot add and remove the same ingredient!");
                    }
                    if (comboBox5.Text != "")
                    {//removing ingredient
                        c.Inserts("delete from ingredients_for_fooditem where ingredients_ingredientsID = (select ingredientsid from ingredients where ingredientname = '" + comboBox5.Text + "')");
                    }
                }
            }
            else
            {
                MessageBox.Show("Enter ID of ingredient to update!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {//show all food items
            ConnectingData c = new ConnectingData();
            //using VIEW
            dataGridView1.DataSource = c.Select("select * from showAllFoodItems");
        }

        private void button7_Click(object sender, EventArgs e)
        {//show ingredients for selected food item
            ConnectingData c = new ConnectingData();
            //using VIEW
            if (textBox9.Text == "")
            {
                MessageBox.Show("Insert ID");
            }
            else
            {
                int i = 0;
                if (!int.TryParse(textBox9.Text, out i))
                {
                    MessageBox.Show("Invalid ID");
                }
                else
                {
                    dataGridView1.DataSource = c.Select("select * from showFoodItemIngredients where [Item ID] = " + textBox9.Text);
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void FoodItems_Load(object sender, EventArgs e)
        {
            //populating categories combobox with values from the database (categories table)
            ConnectingData c = new ConnectingData();
            DataTable ds = c.Select("Select CategoriesID, CategoryName from Categories");
            DataRow row = ds.NewRow(); //adding default null value as first element in combobox
            row[0] = 0;
            row[1] = "";
            ds.Rows.InsertAt(row, 0);
            comboBox2.DataSource = ds;
            comboBox2.DisplayMember = "CategoryName";
            comboBox2.ValueMember = "CategoriesID";

            DataTable ds5 = c.Select("Select CategoriesID, CategoryName from Categories");
            DataRow row5 = ds5.NewRow(); //adding default null value as first element in combobox
            row5[0] = 0;
            row5[1] = "";
            ds5.Rows.InsertAt(row5, 0);
            comboBox3.DataSource = ds5;
            comboBox3.DisplayMember = "CategoryName";
            comboBox3.ValueMember = "CategoriesID";

            //populating ingredients combobox with values from the database (categories table)
            DataTable ds2 = c.Select("Select IngredientsID, IngredientName from Ingredients");
            DataRow row2 = ds2.NewRow(); //adding default null value as first element in combobox
            row2[0] = 0;
            row2[1] = "";
            ds2.Rows.InsertAt(row2, 0);
            comboBox1.DataSource = ds2;
            comboBox1.DisplayMember = "IngredientName";
            comboBox1.ValueMember = "IngredientsID";

            DataTable ds3 = c.Select("Select IngredientsID, IngredientName from Ingredients");
            DataRow row3 = ds3.NewRow(); //adding default null value as first element in combobox
            row3[0] = 0;
            row3[1] = "";
            ds3.Rows.InsertAt(row3, 0);
            comboBox5.DataSource = ds3;
            comboBox5.DisplayMember = "IngredientName";
            comboBox5.ValueMember = "IngredientsID";

            DataTable ds4 = c.Select("Select IngredientsID, IngredientName from Ingredients");
            DataRow row4 = ds4.NewRow(); //adding default null value as first element in combobox
            row4[0] = 0;
            row4[1] = "";
            ds4.Rows.InsertAt(row4, 0);
            comboBox4.DataSource = ds4;
            comboBox4.DisplayMember = "IngredientName";
            comboBox4.ValueMember = "IngredientsID";
        }
    }
}
