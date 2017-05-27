using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace db
{
    public partial class print_tempOut : Form
    {
        OleDbDataAdapter oda;
        DataTable dt;
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\thaimaidensha\\dbstock\\database.accdb");
        private string Comp = "", AppBy = "", ReqBy = "", ProjName = "", ProjWBS = "";

        public print_tempOut(string comp, string appby, string reqby, string projname, string projwbs)
        {
            InitializeComponent();
            Comp = comp;
            AppBy = appby;
            ReqBy = reqby;
            ProjName = projname;
            ProjWBS = projwbs;
            reportViewer1.ShowExportButton = false;
            reportViewer1.ShowFindControls = false;
        }

        private void print_tempOut_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'databaseDataSet.tb_tempOut' table. You can move, or remove it, as needed.
            this.tb_tempOutTableAdapter.Fill(this.databaseDataSet.tb_tempOut);

            System.Drawing.Printing.PageSettings pg = new System.Drawing.Printing.PageSettings();
            pg.Margins.Top = 5;
            pg.Margins.Bottom = 5;
            pg.Margins.Left = 20;
            pg.Margins.Right = 5;
            System.Drawing.Printing.PaperSize size = new PaperSize();
            size.RawKind = (int)PaperKind.A5;
            pg.PaperSize = size;
            pg.Landscape = true;
            oda = new OleDbDataAdapter("SELECT * FROM tb_tempOut", conn);
            dt = new DataTable();
            oda.Fill(dt);
            tb_tempOutBindingSource.DataSource = dt;
            ReportParameter[] parameters = new ReportParameter[6];
            parameters[0] = new ReportParameter("ReqDate", DateTime.Now.ToString("dd/MMM/yyyy"));
            parameters[1] = new ReportParameter("Project", ProjName);
            parameters[2] = new ReportParameter("WBSNo", ProjWBS);
            parameters[3] = new ReportParameter("Comp", Comp);
            parameters[4] = new ReportParameter("AppBy", AppBy);
            parameters[5] = new ReportParameter("ReqBy", ReqBy);
            this.reportViewer1.LocalReport.SetParameters(parameters);
            reportViewer1.SetPageSettings(pg);
            this.reportViewer1.RefreshReport();
        }
    }
}
