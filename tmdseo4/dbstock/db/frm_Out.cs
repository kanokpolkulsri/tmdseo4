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
using System.Drawing.Printing;
using DGVPrinterHelper;

namespace db
{
    public partial class frm_Out : Form
    {
        OleDbDataAdapter oda;
        DataTable dt;
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\thaimaidensha\\dbstock\\database.accdb");

        public frm_Out()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void frm_Out_Load(object sender, EventArgs e)
        {
            oda = new OleDbDataAdapter("SELECT OutDate, OutNo, OutName, OutAmount, OutUnit, OutPriceUnit, OutPriceTotal, OutPerson, OutStorage, OutAdmin, OutComp FROM tb_Out", conn);
            dt = new DataTable();
            oda.Fill(dt);
            dataGridView1.DataSource = dt;

            OleDbDataAdapter oda2;
            DataTable dt2;
            oda2 = new OleDbDataAdapter("SELECT * FROM tb_Material", conn);
            dt2 = new DataTable();
            oda2.Fill(dt2);
            dataGridView2.DataSource = dt2;
            dataGridView2.Columns[0].HeaderText = "Date";
            dataGridView2.Columns[1].HeaderText = "Description";
            dataGridView2.Columns[2].HeaderText = "Unit\nหน่วยนับ";
            dataGridView2.Columns[3].HeaderText = "Currently\nยอดสั่ง";
            dataGridView2.Columns[4].HeaderText = "Brought forward\nยอดยกมา";
            dataGridView2.Columns[5].HeaderText = "Total\nรวม";
            dataGridView2.Columns[6].HeaderText = "Materials used\nยอดใช้ไป";
            dataGridView2.Columns[7].HeaderText = "Balance\nยอดคงเหลือ";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\thaimaidensha\\dbstock\\database.accdb");
                oda = new OleDbDataAdapter("SELECT OutDate, OutNo, OutName, OutAmount, OutUnit, OutPriceUnit, OutPriceTotal, OutPerson, OutStorage, OutAdmin, OutComp FROM tb_Out WHERE OutNo like '" + textBox1.Text + "%'", conn);
                dt = new DataTable();
                oda.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            else
            {
                frm_Out_Load(sender, e);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\thaimaidensha\\dbstock\\database.accdb");
                oda = new OleDbDataAdapter("SELECT OutDate, OutNo, OutName, OutAmount, OutUnit, OutPriceUnit, OutPriceTotal, OutPerson, OutStorage, OutAdmin, OutComp FROM tb_Out WHERE OutName like '" + textBox2.Text + "%'", conn);
                dt = new DataTable();
                oda.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            else
            {
                frm_Out_Load(sender, e);
            }
        }

        private void printdgv(object sender, EventArgs e, string name)
        {
            string ProjName = " ", ProjWBS = " ";
            try
            {
                DataSet ds = new DataSet();
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT ProjName, ProjWBS FROM tb_Project", conn);
                da.Fill(ds, "Inv_no");
                foreach (DataRow dr in ds.Tables["Inv_no"].Rows)
                {
                    ProjName = dr.ItemArray[0] + "";
                    ProjWBS = dr.ItemArray[1] + "";
                    break;
                }
            }
            catch
            { }

            DGVPrinter printer = new DGVPrinter();
            printer.DocName = "mtrReport";
            printer.Title = "Materials report";
            printer.TitleFont = new Font("Arial", 15, FontStyle.Bold);
            //printer.SubTitle = "\nหน่วยงานเลขที่ (WBS NO.) : " + ProjWBS + "      ชื่อหน่วยงาน (Project Name) : " + ProjName + " \nวันที่ขอเบิก (Requistion Date) : 09/09/1999         บริษัทที่ขอเบิก : " + textBox8.Text + "\nผู้อนุมัติเบิก (Approved By) : " + textBox7.Text + "\nผู้ขอเบิก (Requisted By) : " + textBox4.Text + "";
            printer.SubTitle = "Project Name : " + ProjName + " \nWBS No. : " + ProjWBS + "\nMaterial name : " + name + "";
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.SubTitleAlignment = StringAlignment.Near;
            printer.SubTitleFont = new Font("Arial", 10, FontStyle.Regular);
            printer.PageNumbers = false;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.PageText = "";
            printer.Footer = "Report by _______________________________________                                                   Approved by _______________________________________\n\n    Date _______________________________________                                                               Date _______________________________________";
            printer.FooterFont = new Font("Arial", 10, FontStyle.Regular);
            printer.SubTitleSpacing = 10;
            printer.TitleSpacing = 10;
            printer.HeaderCellAlignment = StringAlignment.Near;
            try
            {
                printer.PrintDataGridView(dataGridView2);
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                OleDbCommand checkname_bq = new OleDbCommand();
                checkname_bq.Connection = conn;
                checkname_bq.CommandText = "SELECT * FROM tb_Inv WHERE InvNo = '" + textBox3.Text + "'";
                int count = 0;
                OleDbDataReader readerCheckNameBQ = checkname_bq.ExecuteReader();
                while (readerCheckNameBQ.Read())
                {
                    count = count + 1;
                    if (count == 1)
                    {
                        break;
                    }
                }
                conn.Close();
                if (count == 1)
                {
                    DataSet ds = new DataSet();
                    OleDbDataAdapter da = new OleDbDataAdapter("SELECT InvName FROM tb_Inv WHERE InvNo = '" + textBox3.Text + "'", conn);

                    da.Fill(ds, "Inv_name");
                    string text_name = " ";
                    foreach (DataRow dr in ds.Tables["Inv_name"].Rows)
                    {
                        text_name = dr.ItemArray[0] + "";
                        break;
                    }
                    oda = new OleDbDataAdapter("SELECT * FROM tb_Material WHERE MtrNo = '" + textBox3.Text + "'", conn);
                    dt = new DataTable();
                    oda.Fill(dt);
                    dataGridView2.DataSource = dt;
                    printdgv(sender, e, text_name);
                    textBox3.Clear();
                }
                else
                {
                    MessageBox.Show("ไม่มีรหัสสินค้านี้");
                }
            }
            catch
            {
                frm_Out_Load(sender, e);
            }
        }
    }
}
