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
using DGVPrinterHelper;

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
            // TODO: This line of code loads data into the 'databaseDataSet3.tb_Report' table. You can move, or remove it, as needed.
            this.tb_ReportTableAdapter.Fill(this.databaseDataSet3.tb_Report);
            // TODO: This line of code loads data into the 'databaseDataSet.tb_Inv' table. You can move, or remove it, as needed.
            this.tb_InvTableAdapter.Fill(this.databaseDataSet.tb_Inv);
            oda = new OleDbDataAdapter("SELECT ID, InvNo, InvName, InvAmount, InvUnit, InvPriceUnit, InvPriceTotal, InvStorage FROM tb_Inv", conn);
            dt = new DataTable();
            oda.Fill(dt);
            dataGridView1.DataSource = dt;
            textBox3.Text = "0";
            textBox5.Text = "0";
            ActiveControl = textBox1;
            textBox3.ReadOnly = true;

            OleDbDataAdapter oda2;
            DataTable dt2;
            oda2 = new OleDbDataAdapter("SELECT * FROM tb_Report", conn);
            dt2 = new DataTable();
            oda2.Fill(dt2);
            dataGridView2.DataSource = dt2;
            dataGridView2.Columns[0].HeaderText = "Item";
            dataGridView2.Columns[1].HeaderText = "Description";
            dataGridView2.Columns[2].HeaderText = "Unit";
            dataGridView2.Columns[3].HeaderText = "Q'ty\n(PO.)";
            dataGridView2.Columns[4].HeaderText = "Q'ty\n(Used)";
            dataGridView2.Columns[5].HeaderText = "Q'ty\n(Remaining)";
            dataGridView2.Columns[6].HeaderText = "Unit price";
            dataGridView2.Columns[7].HeaderText = "Total";
            dataGridView2.Columns[8].HeaderText = "Remark";
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

        private void printdgv(object sender, EventArgs e)
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
            printer.DocName = "RemainingReport";
            printer.Title = "Material Remaining Control List Report\n";
            printer.TitleFont = new Font("Arial", 15, FontStyle.Regular);
            printer.SubTitle = "Date (MM/DD/YYYY) : "+DateTime.Now.ToString("MM/dd/yyyy")+"     WBS No. : "+ProjWBS+"    Project Name : "+ProjName+ "\n\nCustomer Name : __________________ Contract price : _______________ Incharge (SEO) : ____________________";
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.SubTitleAlignment = StringAlignment.Near;
            printer.SubTitleFont = new Font("Arial", 9, FontStyle.Regular);
            printer.PageNumbers = false;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.PageText = "";
            printer.Footer = "Remark : __________________________________________________________________________________________\n__________________________________________________________________________________________________\n__________________________________________________________________________________________________\n____________________________________________________________________Checker (PC) : __________________\n\nSEO                                                    Approve                                                    Admin                       Witness\nGM./DGM.                                     AMGR.                                                       MGR. (AC)\nMGR.                                             PM.                                                            MGR. (PC)";
            printer.FooterAlignment = StringAlignment.Near;
            printer.FooterFont = new Font("Arial", 9, FontStyle.Regular);
            printer.SubTitleSpacing = 10;
            printer.TitleSpacing = 10;
            printer.FooterSpacing = 12;
            printer.HeaderCellAlignment = StringAlignment.Near;
            try
            {
                printer.PrintDataGridView(dataGridView2);
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //เช็คว่ามีตัวไหนที่ยังเหลือสินค้าอยู่
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT InvNo, InvName, InvUnit, InvPriceUnit FROM tb_Inv WHERE InvAmount > 0", conn);
            da.Fill(ds, "Inv_Data");
            string text_InvNo = " ", text_InvName = " ", text_InvUnit = " ", text_InvUnitPrice = " ";
            int ItemNo = 0;
            foreach (DataRow dr in ds.Tables["Inv_Data"].Rows)
            {
                text_InvNo = dr.ItemArray[0] + "";
                text_InvName = dr.ItemArray[1] + "";
                text_InvUnit = dr.ItemArray[2] + "";
                text_InvUnitPrice = dr.ItemArray[3] + "";
                Console.WriteLine(text_InvNo);
                //วนจำนวน MtrCurrent และ MtrUsed
                DataSet ds2 = new DataSet();
                OleDbDataAdapter da2 = new OleDbDataAdapter("SELECT MtrCurrent, MtrUsed FROM tb_Material WHERE MtrNo = '" + text_InvNo + "'", conn);
                da2.Fill(ds2, "Mat_currentAndused");
                string text_MatCurrent = " ", text_MatUsed = " ";
                int sum_current = 0, sum_used = 0;
                foreach (DataRow dr2 in ds2.Tables["Mat_currentAndused"].Rows)
                {
                    text_MatCurrent = dr2.ItemArray[0] + "";
                    sum_current += int.Parse(text_MatCurrent);
                    text_MatUsed = dr2.ItemArray[1] + "";
                    sum_used += int.Parse(text_MatUsed);
                }
                //Console.WriteLine("{0} - {1}", sum_current, sum_used); //ตอนนี้ได้ผลรวม current, used ของรหัสนี้แล้ว จะอัพเดตใส่ตาราง report
                //ขั้นตอนต่อไปจะ insert เข้าไปในตาราง report ครับผม
                conn.Open();
                OleDbCommand command2 = new OleDbCommand();
                command2.Connection = conn;
                ItemNo += 1;
                command2.CommandText = "INSERT into tb_Report (SumNo, SumName, SumUnit, SumPO, SumUsed, SumUnitPrice) VALUES ('"+ItemNo+"','"+text_InvName+"','"+text_InvUnit+"','"+sum_current+"','"+sum_used+"','"+text_InvUnitPrice+"')";
                command2.ExecuteNonQuery();
                conn.Close();
            }
            frm_Inv_Load(sender, e);
            printdgv(sender, e);
            conn.Open();
            OleDbCommand delete = new OleDbCommand();
            delete.Connection = conn;
            delete.CommandText = "DELETE * FROM tb_Report";
            delete.ExecuteNonQuery();
            conn.Close();
            frm_Inv_Load(sender, e);
        }
    }
}
