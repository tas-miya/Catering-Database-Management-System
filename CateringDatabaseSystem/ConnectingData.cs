using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringDatabaseSystem
{
    class ConnectingData
    {
        //data members

        // connection string
        public SqlConnection conn = new SqlConnection(@"Data Source=E-7440;Initial Catalog=cateringSystemDatabase;Integrated Security=SSPI;User ID=sa;Password= ");
        public SqlCommand cmd = new SqlCommand();


        //consructor
        public ConnectingData()
        { }

        //function members

        public void Inserts(string query) // insert / update / delete
        {
            conn.Open();
            cmd.CommandText = query;
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public DataTable Select(string query) // select query
        {
            conn.Open();
            cmd.CommandText = query;
            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }

        public string getStringValue(string query)
        {
            conn.Open();
            cmd.CommandText = query;
            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            string result = (string)cmd.ExecuteScalar();
            //DataTable dt = new DataTable();
            //da.Fill(dt);
            conn.Close();
            return result;
        }
    }
}
