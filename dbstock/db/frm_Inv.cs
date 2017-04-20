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
    public partial class frm_Inv : Form
    {
        OleDbDataAdapter oda;
        DataTable dt;
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\thaimaidensha\\dbstock\\database.accdb");

        public frm_Inv()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void frm_Inv_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'databaseDataSet.tb_Inv' table. You can move, or remove it, as needed.
            this.tb_InvTableAdapter.Fill(this.databaseDataSet.tb_Inv);
            oda = new OleDbDataAdapter("SELECT ID, InvNo, InvName, InvAmount, InvUnit, InvPriceUnit, InvPriceTotal, InvStorage FROM tb_Inv", conn);
            dt = new DataTable();
            oda.Fill(dt);
            dataGridView1.DataSource = dt;
            textBox3.Text = "0";
            textBox5.Text = "0";
            ActiveControl = textBox1;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text != "")
            {
                oda = new OleDbDataAdapter("SELECT ID, InvNo, InvName, InvAmount, InvUnit, InvPriceUnit, InvPriceTotal, InvStorage FROM tb_Inv WHERE InvName like '" + textBox8.Text + "%'", conn);
                dt = new DataTable();
                oda.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            else
            {
                frm_Inv_Load(sender, e);
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text != "")
            {
                oda = new OleDbDataAdapter("SELECT ID, InvNo, InvName, InvAmount, InvUnit, InvPriceUnit, InvPriceTotal, InvStorage FROM tb_Inv WHERE InvNo like '" + textBox7.Text + "%'", conn);
                dt = new DataTable();
                oda.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            else
            {
                frm_Inv_Load(sender, e);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            conn.Open();
            OleDbCommand command = new OleDbCommand();
            command.Connection = conn;
            command.CommandText = "SELECT * FROM tb_Inv WHERE InvNo='" + textBox1.Text + "'";
            OleDbDataReader reader = command.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                count = count + 1;
                if (count == 1)
                {
                    break;
                }
            }
            if (count == 1)
            {
                MessageBox.Show("มีรหัสสินค้านี้แล้ว", "WARNING");
            }
            else
            {
                OleDbCommand command2 = new OleDbCommand();
                command2.Connection = conn;
                double InvAmount, InvPriceUnit;
                if (textBox1.Text != "" && textBox2.Text != "" && double.TryParse(textBox3.Text, out InvAmount) && textBox4.Text != "" && double.TryParse(textBox5.Text, out InvPriceUnit) && textBox6.Text != "")
                {
                    command2.CommandText = "INSERT into tb_Inv (InvNo, InvName, InvAmount, InvUnit, InvPriceUnit, InvStorage) VALUES ('" + textBox1.Text + "','" + textBox2.Text + "','" + InvAmount + "','" + textBox4.Text + "','" + InvPriceUnit + "','" + textBox6.Text + "')";
                    command2.ExecuteNonQuery();
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                    textBox6.Clear();
                    MessageBox.Show("เพิ่มข้อมูลสินค้าเรียบร้อย");
                }
                else
                {
                    MessageBox.Show("กรอกข้อมูลไม่ครบ");
                }

            }
            conn.Close();
            frm_Inv_Load(sender, e);
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((e.Value != null) && (e.ColumnIndex == 3))
            {
                if (Convert.ToInt32(e.Value) <= 5)
                {
                    e.CellStyle.BackColor = Color.Red;
                }
                else if (Convert.ToInt32(e.Value) <= 10)
                {
                    e.CellStyle.BackColor = Color.Yellow;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            conn.Open();
            try
            {
                OleDbCommand delete = new OleDbCommand();
                delete.Connection = conn;
                delete.CommandText = "DELETE FROM tb_Inv WHERE ID = " + textBox9.Text + "";
                delete.ExecuteNonQuery();
            }
            catch {
                MessageBox.Show("ไม่มี ID นี้อยู่ในระบบ");
            }
            frm_Inv_Load(sender, e);
            textBox9.Clear();
            conn.Close();
        }
    }
}
