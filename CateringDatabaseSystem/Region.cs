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
    public partial class Region : Form
    {
        public Region()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {//adding region
            ConnectingData c = new ConnectingData();
            c.Inserts("insert into region (regionID, regionDescription) values ((select max(regionID) from region)+1, '" + textBox2.Text + "')");
        }

        private void button4_Click(object sender, EventArgs e)
        {// show all regions
            ConnectingData c = new ConnectingData();
            dataGridView1.DataSource = c.Select("select * from region");
        }

        private void button3_Click(object sender, EventArgs e)
        {//update region
            ConnectingData c = new ConnectingData();
            c.Inserts("update region set regionDescription = '" + textBox3.Text + "' where regionID = " + textBox7.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {//delete region
            ConnectingData c = new ConnectingData();
            c.Inserts("delete from region where regionID =" + textBox4.Text);
        }
    }
}
