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

namespace Exam
{
    public partial class Form1 : Form
    {
        SqlConnection conn;
        SqlDataAdapter da;
        DataSet ds = new DataSet();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string strconnection = "data source=DESKTOP-LMOVISH; initial catalog=db; integrated security=true";
            conn = new SqlConnection(strconnection);
            conn.Open();          
            da = new SqlDataAdapter("select * from student", conn);
            SqlCommandBuilder x = new SqlCommandBuilder(da);
            da.Fill(ds,"student");
            dataGridView1.DataSource = ds.Tables["student"];
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DataRow r = ds.Tables["student"].NewRow();
            r["id"] = int.Parse(textBox1.Text);
            r["name"] = textBox2.Text;
            r["city"] = textBox3.Text;
            ds.Tables["student"].Rows.Add(r);
            da.Update(ds, "student");
        }

        private void button2_Click(object sender, EventArgs e)
        {        
            foreach (DataRow r in ds.Tables["student"].Rows)
            {
                if (Convert.ToInt32(r["id"]) == int.Parse(textBox1.Text))
                {
                    textBox2.Text = Convert.ToString(r["name"]);
                    textBox3.Text = Convert.ToString(r["city"]);
                    DialogResult x = MessageBox.Show("Are you Sure to delete student", "Delete Student", MessageBoxButtons.YesNo);
                    if (x == DialogResult.Yes)
                    {
                        r.Delete();
                    }
                }           
            }
            da.Update(ds, "student");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (DataRow r in ds.Tables["student"].Rows)
            {
                if (Convert.ToInt32(r["id"]) == int.Parse(textBox1.Text))
                {
                    textBox2.Text = Convert.ToString(r["name"]);
                    textBox3.Text = Convert.ToString(r["city"]);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (DataRow r in ds.Tables["student"].Rows)
            {
                if (Convert.ToInt32(r["id"]) == int.Parse(textBox1.Text))
                {
                    DialogResult x = MessageBox.Show("Are you Sure to edit student", "Editting Student", MessageBoxButtons.YesNo);
                    if (x == DialogResult.Yes)
                    {
                        r["name"] = textBox2.Text;
                        r["city"] = textBox3.Text;
                        da.Update(ds, "student");
                    }
                }
            }
        }
    }
}
