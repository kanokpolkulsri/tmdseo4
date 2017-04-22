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
            // TODO: This line of code loads data into the 'databaseDataSet.tb_Out' table. You can move, or remove it, as needed.
            this.tb_OutTableAdapter.Fill(this.databaseDataSet.tb_Out);
            oda = new OleDbDataAdapter("SELECT OutDate, OutNo, OutName, OutAmount, OutUnit, OutPriceUnit, OutPriceTotal, OutPerson, OutStorage, OutAdmin, OutComp FROM tb_Out", conn);
            dt = new DataTable();
            oda.Fill(dt);
            dataGridView1.DataSource = dt;
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

        
        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            /*
            //print header
            e.Graphics.DrawString("Header Report Title", new Font("Arial", 20), Brushes.Black, 10, 10);
            e.Graphics.DrawLine(Pens.Gray, 10, 50, 830, 50);
            
            
            //print content
            int position = 130;
            e.Graphics.DrawString("- - data from datagrid view --", new Font("Arial", 16), Brushes.Black, 10, 100);
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                e.Graphics.DrawString((string)item.Cells[1].Value, new Font("Arial", 10), Brushes.Black, 10, position);
                position += 20;
            } */

        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = "Test Document";
            printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);

            PrintDialog printDlg = new PrintDialog();
            //PrintPreviewDialog printPrvDlg = new PrintPreviewDialog();
            //printPrvDlg.ShowDialog();

            printDlg.Document = printDoc;
            if (printDlg.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
            */

            
            DGVPrinter printer = new DGVPrinter();

            printer.Title = "Customer Report"; //Header
            printer.SubTitle = "subtitle"; 
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "Foxlearn";
            printer.FooterSpacing = 15;
            try
            {
                printer.PrintDataGridView(dataGridView1);
            }
            catch { }
            
            
        }

    }
}
