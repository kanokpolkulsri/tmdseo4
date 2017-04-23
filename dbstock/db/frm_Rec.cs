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
    public partial class frm_Rec : Form
    {
        OleDbDataAdapter oda;
        DataTable dt;
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\thaimaidensha\\dbstock\\database.accdb");

        public frm_Rec()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void frm_Rec_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'databaseDataSet.tb_Rec' table. You can move, or remove it, as needed.
            this.tb_RecTableAdapter.Fill(this.databaseDataSet.tb_Rec);
            oda = new OleDbDataAdapter("SELECT RecDate, RecNo, RecName, RecAmount, RecUnit, RecPriceUnit, RecPriceTotal, RecPerson, RecStorage, RecComp FROM tb_Rec", conn);
            dt = new DataTable();
            oda.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[2].Width = 150;
            dataGridView1.Columns[8].Width = 150;

            OleDbDataAdapter oda2;
            DataTable dt2;
            oda2 = new OleDbDataAdapter("SELECT * FROM tb_Material", conn);
            dt2 = new DataTable();
            oda2.Fill(dt2);
            dataGridView2.DataSource = dt2;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\thaimaidensha\\dbstock\\database.accdb");
                oda = new OleDbDataAdapter("SELECT RecDate, RecNo, RecName, RecAmount, RecUnit, RecPriceUnit, RecPriceTotal, RecPerson, RecStorage FROM tb_Rec WHERE RecNo like '" + textBox1.Text + "%'", conn);
                dt = new DataTable();
                oda.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            else
            {
                frm_Rec_Load(sender, e);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\thaimaidensha\\dbstock\\database.accdb");
                oda = new OleDbDataAdapter("SELECT RecDate, RecNo, RecName, RecAmount, RecUnit, RecPriceUnit, RecPriceTotal, RecPerson, RecStorage FROM tb_Rec WHERE RecName like '" + textBox2.Text + "%'", conn);
                dt = new DataTable();
                oda.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            else
            {
                frm_Rec_Load(sender, e);
            }
        }
    }
}
