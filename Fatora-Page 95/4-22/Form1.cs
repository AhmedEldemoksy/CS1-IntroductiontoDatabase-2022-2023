using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace _4_22
{
    public partial class Form1 : Form
    {
        SqlConnection conn;
        SqlDataAdapter da1,da2;
        SqlCommand com3;
        DataSet ds = new DataSet();
        DataSet dss = new DataSet();
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            string strconnection = "data source=DESKTOP-LMOVISH; initial catalog=master; integrated security=true";
            conn = new SqlConnection(strconnection);
            conn.Open();
            try
            {
                SqlCommand com1 = new SqlCommand("Create database fatora_db", conn);
                com1.ExecuteNonQuery();

                com1.Connection.ChangeDatabase("fatora_db");

                SqlCommand com2 = new SqlCommand("Create table fatora_tb (fatora_id int primary key, ameelname char(50),fatora_date char(50),sanf_count int default (0),total float default(0))", conn);
                com2.ExecuteNonQuery();
                for (int i = 0; i <= 19; i++)
                {
                    SqlCommand com3 = new SqlCommand("alter table fatora_tb add Sanf_Num" + i + " int default (0), Sanf_Name" + i + " char(20), Sanf_Quantity" + i + " int default(0), Sanf_Price" + i + " float default(0)", conn);
                    com3.ExecuteNonQuery();

                }
            }
            catch (Exception e1)
            {
                conn.ChangeDatabase("fatora_db");
            }
            da1 = new SqlDataAdapter("select * from fatora_tb", conn);
            SqlCommandBuilder x = new SqlCommandBuilder(da1);
            da1.Fill(ds, "fatora_tb");
            
            DataTable dt = new DataTable("dt1");
            ds.Tables.Add(dt);
            DataColumn c0 = new DataColumn("Mosalsal", typeof(int));
            DataColumn c1 = new DataColumn("Sanf_Num", typeof(int));
            DataColumn c2 = new DataColumn("Sanf_Name", typeof(string));
            DataColumn c3 = new DataColumn("Sanf_Quantity", typeof(int));
            DataColumn c4 = new DataColumn("Sanf_Price", typeof(float));
            DataColumn c5 = new DataColumn("Sanf_Total", typeof(float));
            ds.Tables["dt1"].Columns.Add(c0);
            ds.Tables["dt1"].Columns.Add(c1);
            ds.Tables["dt1"].Columns.Add(c2);
            ds.Tables["dt1"].Columns.Add(c3);
            ds.Tables["dt1"].Columns.Add(c4);
            ds.Tables["dt1"].Columns.Add(c5);

            for (int i = 1; i <= 20; i++)
            {
                DataRow dr1 = ds.Tables["dt1"].NewRow();
                dr1["Mosalsal"] = i;
                dr1["Sanf_Num"] = 0;
                dr1["Sanf_Name"] = " ";
                dr1["Sanf_Quantity"] = 0;
                dr1["Sanf_Price"] = 0;
                dr1["Sanf_Total"] = 0;
                ds.Tables["dt1"].Rows.Add(dr1);
            }
            dataGridView1.DataSource = ds.Tables["dt1"];
            }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dss.Tables.Contains("fatora_tb"))
                dss.Tables["fatora_tb"].Clear();
            da2 = new SqlDataAdapter("select fatora_id, ameelname,fatora_date,sanf_count,total from fatora_tb where fatora_date>= '"+dateTimePicker2.Value.Date+"' and fatora_date<= '"+dateTimePicker3.Value.Date+"'", conn);
            SqlCommandBuilder comb = new SqlCommandBuilder(da2);
            da2.Fill(dss, "fatora_tb");
            dataGridView2.DataSource = dss.Tables["fatora_tb"];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ds.Tables.Contains("dt1"))
                ds.Tables["dt1"].Clear();
            foreach(DataRow dr in ds.Tables["fatora_tb"].Rows)
            {
                if (Convert.ToInt32(dr["fatora_id"]) == int.Parse(textBox1.Text))
                {
                    textBox2.Text = dr["ameelname"].ToString();
                    dateTimePicker1.Text = dr["fatora_date"].ToString();
                    textBox3.Text= dr["total"].ToString();
                    textBox4.Text = dr["sanf_count"].ToString();
                    for (int i=0; i<=19; i++)
                    {
                        DataRow dr1 = ds.Tables["dt1"].NewRow();
                        dr1[0] = i + 1;
                        dr1[1] = dr["Sanf_Num" + i];
                        dr1[2] = dr["Sanf_Name" + i];
                        dr1[3] = dr["Sanf_Quantity" + i];
                        dr1[4] = dr["Sanf_Price" + i];
                        dr1[5] =Convert.ToInt32(dr1[3])*Convert.ToSingle(dr1[4]);
                        ds.Tables["dt1"].Rows.Add(dr1);
                    }
                    dataGridView1.DataSource = ds.Tables["dt1"];
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataRow dr = ds.Tables["fatora_tb"].NewRow();
            dr["fatora_id"] = int.Parse(textBox1.Text);
            dr["ameelname"] = textBox2.Text;
            dr["fatora_date"] = Convert.ToString(dateTimePicker1.Value.Date);
            dr["total"] = 0;           
            dr["sanf_count"] = 0;
            int i=0;
            foreach (DataRow dr1 in ds.Tables["dt1"].Rows)
            {
                dr["Sanf_Num" + i] = dr1[1];
                dr["Sanf_Name" + i] = dr1[2];
                dr["Sanf_Quantity" + i] = dr1[3];
                dr["Sanf_Price" + i] = dr1[4];

                dr1[5] = Convert.ToInt32(dr1[3]) * Convert.ToSingle(dr1[4]);
                dr["total"] = Convert.ToSingle(dr["total"]) + Convert.ToSingle(dr1[5]);
                if (Convert.ToInt32(dr1["Sanf_Quantity"]) != 0)
                    dr["sanf_count"] = Convert.ToSingle(dr["sanf_count"]) + 1;
                i++;
            }
            ds.Tables["fatora_tb"].Rows.Add(dr);
            da1.Update(ds, "fatora_tb");
            dataGridView1.DataSource = ds.Tables["dt1"];
            textBox3.Text = dr["total"].ToString();
            textBox4.Text = dr["sanf_count"].ToString();

        }

    }
}
