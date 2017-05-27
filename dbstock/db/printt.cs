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
using Microsoft.Reporting.WinForms;
using System.Drawing.Printing;

namespace db
{
    public partial class printt : Form
    {
        OleDbDataAdapter oda;
        DataTable dt;
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\thaimaidensha\\dbstock\\database.accdb");

        public printt()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void printt_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'databaseDataSet.tb_Report' table. You can move, or remove it, as needed.
            this.tb_ReportTableAdapter.Fill(this.databaseDataSet.tb_Report);
            System.Drawing.Printing.PageSettings pg = new System.Drawing.Printing.PageSettings();
            pg.Margins.Top = 5;
            pg.Margins.Bottom = 5;
            pg.Margins.Left = 5;
            pg.Margins.Right = 5;
            System.Drawing.Printing.PaperSize size = new PaperSize();
            size.RawKind = (int)PaperKind.A4;
            pg.PaperSize = size;
            reportViewer1.SetPageSettings(pg);
            oda = new OleDbDataAdapter("SELECT * FROM tb_Report", conn);
            dt = new DataTable();
            oda.Fill(dt);
            tb_ReportBindingSource.DataSource = dt;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            OleDbCommand delete = new OleDbCommand();
            delete.Connection = conn;
            delete.CommandText = "DELETE * FROM tb_Report";
            delete.ExecuteNonQuery();
            conn.Close();

            try
            {
                string ProjName = "", ProjWBS = "", ProjAdmin = "";
                DataSet ds3 = new DataSet();
                OleDbDataAdapter da3 = new OleDbDataAdapter("SELECT ProjName, ProjWBS, ProjAdmin FROM tb_Project", conn);
                da3.Fill(ds3, "Inv_no");
                foreach (DataRow dr in ds3.Tables["Inv_no"].Rows)
                {
                    ProjName = dr.ItemArray[0] + "";
                    ProjWBS = dr.ItemArray[1] + "";
                    ProjAdmin = dr.ItemArray[2] + "";
                    break;
                }
                ReportParameter[] parameters = new ReportParameter[4];
                parameters[0] = new ReportParameter("Date", DateTime.Now.ToString("dd/MMM/yyyy"));
                parameters[1] = new ReportParameter("ProjAdmin", ProjAdmin);
                parameters[2] = new ReportParameter("ProjName", ProjName);
                parameters[3] = new ReportParameter("ProjWBS", ProjWBS);
                this.reportViewer1.LocalReport.SetParameters(parameters);
                this.reportViewer1.RefreshReport();
            }
            catch
            {
                MessageBox.Show("ยังไม่ได้ลงทะเบียนโปรเจค/WBS No.");
            }

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
                command2.CommandText = "INSERT into tb_Report (SumNo, SumName, SumUnit, SumPO, SumUsed, SumUnitPrice) VALUES ('" + ItemNo + "','" + text_InvName + "','" + text_InvUnit + "','" + sum_current + "','" + sum_used + "','" + text_InvUnitPrice + "')";
                command2.ExecuteNonQuery();
                conn.Close();
            }
            printt_Load(sender, e);
        }
    }
}
