using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace db
{
    public partial class frm_Customer : Form
    {

        OleDbDataAdapter oda;
        DataTable dt;
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\thaimaidensha\\dbstock\\database.accdb");

        public frm_Customer()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void frm_Customer_Load(object sender, EventArgs e)
        {
            oda = new OleDbDataAdapter("SELECT * FROM tb_Customer", conn);
            dt = new DataTable();
            oda.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "")
            {
                conn.Open();
                OleDbCommand checkNoInCust = new OleDbCommand();
                checkNoInCust.Connection = conn;
                checkNoInCust.CommandText = "SELECT * FROM tb_Customer WHERE CustNo = '"+textBox1.Text+"'";
                OleDbDataReader readerCheckNoInCust = checkNoInCust.ExecuteReader();
                int count2 = 0;
                while (readerCheckNoInCust.Read())
                {
                    count2 = count2 + 1;
                    if (count2 == 1)
                    {
                        break;
                    }
                }
                if (count2 == 1)
                {
                    MessageBox.Show("มีรหัสนี้ภายในระบบแล้ว");
                }
                else
                {

                    OleDbCommand addData = new OleDbCommand();
                    addData.Connection = conn;
                    addData.CommandText = "INSERT into tb_Customer (CustNo, CustFirstName, CustLastName, CustComp, CustPhoneNo) VALUES ('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "')";
                    addData.ExecuteNonQuery();
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                    MessageBox.Show("เพิ่มข้อมูลเรียบร้อย");
                }
                conn.Close();
            }
            else
            {
                MessageBox.Show("กรอกข้อมูลไม่ครบถ้วน");
            }
            frm_Customer_Load(sender, e);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text != "")
            {
                oda = new OleDbDataAdapter("SELECT * FROM tb_Customer WHERE CustNo like '" + textBox6.Text + "%'", conn);
                dt = new DataTable();
                oda.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            else
            {
                frm_Customer_Load(sender, e);
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text != "")
            {
                oda = new OleDbDataAdapter("SELECT * FROM tb_Customer WHERE (CustFirstName like '" + textBox7.Text + "%') OR (CustLastName like '" + textBox7.Text + "%') ", conn);
                dt = new DataTable();
                oda.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            else
            {
                frm_Customer_Load(sender, e);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            try
            {
                OleDbCommand delete = new OleDbCommand();
                delete.Connection = conn;
                delete.CommandText = "DELETE FROM tb_Customer WHERE ID = " + textBox8.Text + "";
                delete.ExecuteNonQuery();
                MessageBox.Show("ลบข้อมูลสมาชิกเรียบร้อย");
            }
            catch
            {
                MessageBox.Show("ไม่มี ID นี้อยู่ในระบบ");
            }
            frm_Customer_Load(sender, e);
            textBox8.Clear();
            conn.Close();
        }
    }
}
