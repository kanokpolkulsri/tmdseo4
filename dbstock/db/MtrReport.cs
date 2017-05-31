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
    public partial class MtrReport : Form
    {
        OleDbDataAdapter oda;
        DataTable dt;
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\thaimaidensha\\dbstock\\database.accdb");
        private string text_name = " ", ProjName = " ", ProjWBS = " ";
        public MtrReport()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            System.Drawing.Printing.PageSettings pg = new System.Drawing.Printing.PageSettings();
            pg.Margins.Top = 5;
            pg.Margins.Bottom = 5;
            pg.Margins.Left = 20;
            pg.Margins.Right = 5;
            System.Drawing.Printing.PaperSize size = new PaperSize();
            size.RawKind = (int)PaperKind.A4;
            pg.PaperSize = size;
            pg.Landscape = true;
            reportViewer1.SetPageSettings(pg);
            this.reportViewer1.RefreshReport();
            reportViewer1.ShowExportButton = false;
            reportViewer1.ShowFindControls = false;
        }

        private void MtrReport_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'databaseDataSet.tb_Material' table. You can move, or remove it, as needed.
            this.tb_MaterialTableAdapter.Fill(this.databaseDataSet.tb_Material);
            conn.Open();
            OleDbCommand updateMtr = new OleDbCommand();
            updateMtr.Connection = conn;
            updateMtr.CommandText = "UPDATE tb_Material SET MtrDes = 'Receiving for Material' WHERE MtrCurrent > 0 AND MtrUsed = 0";
            updateMtr.ExecuteNonQuery();
            updateMtr.CommandText = "UPDATE tb_Material SET MtrDes = 'Requistion for Material' WHERE MtrCurrent = 0 AND MtrUsed > 0";
            updateMtr.ExecuteNonQuery();
            updateMtr.CommandText = "UPDATE tb_Material SET MtrDes = 'Receiving and Requistion' WHERE MtrCurrent > 0 AND MtrUsed > 0";
            updateMtr.ExecuteNonQuery();
            conn.Close();
            oda = new OleDbDataAdapter("SELECT * FROM tb_Material", conn);
            dt = new DataTable();
            oda.Fill(dt);
            tbMaterialBindingSource.DataSource = dt;
            this.reportViewer1.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                OleDbCommand checkname_bq = new OleDbCommand();
                checkname_bq.Connection = conn;
                checkname_bq.CommandText = "SELECT * FROM tb_Inv WHERE InvNo = '" + textBox1.Text + "'";
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
                    OleDbDataAdapter da = new OleDbDataAdapter("SELECT InvName FROM tb_Inv WHERE InvNo = '" + textBox1.Text + "'", conn);

                    da.Fill(ds, "Inv_name");
                    foreach (DataRow dr in ds.Tables["Inv_name"].Rows)
                    {
                        text_name = dr.ItemArray[0] + "";
                        break;
                    }
                    oda = new OleDbDataAdapter("SELECT * FROM tb_Material WHERE MtrNo = '" + textBox1.Text + "' AND MtrName = '"+text_name+"'", conn);
                    dt = new DataTable();
                    oda.Fill(dt);
                    tbMaterialBindingSource.DataSource = dt;
                    this.reportViewer1.RefreshReport();
                    textBox1.Clear();
                }
                else
                {
                    MessageBox.Show("ไม่มีรหัสสินค้านี้");
                    textBox1.Clear();
                }
            }
            catch
            {
                MtrReport_Load(sender, e);
            }

            getDataProj();
            ReportParameter[] parameters = new ReportParameter[3];
            parameters[0] = new ReportParameter("text_name", text_name);
            parameters[1] = new ReportParameter("ProjName", ProjName);
            parameters[2] = new ReportParameter("ProjWBS", ProjWBS);
            this.reportViewer1.LocalReport.SetParameters(parameters);
            this.reportViewer1.RefreshReport();
        }

        void getDataProj()
        {
            try
            {
                DataSet ds3 = new DataSet();
                OleDbDataAdapter da3 = new OleDbDataAdapter("SELECT ProjName, ProjWBS FROM tb_Project", conn);
                da3.Fill(ds3, "Inv_no");
                foreach (DataRow dr in ds3.Tables["Inv_no"].Rows)
                {
                    ProjName = dr.ItemArray[0] + "";
                    ProjWBS = dr.ItemArray[1] + "";
                    break;
                }
            }
            catch
            {
                MessageBox.Show("ยังไม่ได้ลงทะเบียนโปรเจค/WBS No.");
            }
        }
    }
}
