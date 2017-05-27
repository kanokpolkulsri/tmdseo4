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
    public partial class frm_Main : Form
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\thaimaidensha\\dbstock\\database.accdb");
        public string Admin_Name = " ";
        public frm_Main()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            toolStripButton1.Visible = false;
            toolStripButton2.Visible = false;
            toolStripButton3.Visible = false;
            toolStripButton4.Visible = false;
            toolStripButton5.Visible = false;
            toolStripButton6.Visible = false;
            toolStripButton7.Visible = true;
            toolStripButton8.Visible = false;
            toolStripButton9.Visible = false;
            toolStripButton10.Visible = false;
            toolStripButton11.Visible = false;
            CloseAllChildForm();
            frm_login next = new frm_login();
            next.MdiParent = this;
            next.Show();
        }

        private void CloseAllChildForm()
        {
            foreach (Form showf in this.MdiChildren)
            {
                showf.Close();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            CloseAllChildForm();
            frm_admin next = new frm_admin();
            next.MdiParent = this;
            next.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            CloseAllChildForm();
            frm_Inv next = new frm_Inv();
            next.MdiParent = this;
            next.Show();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            CloseAllChildForm();
            frm_tempRec next = new frm_tempRec(Admin_Name);
            next.MdiParent = this;
            next.Show();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            CloseAllChildForm();
            frm_Rec next = new frm_Rec();
            next.MdiParent = this;
            next.Show();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            CloseAllChildForm();
            frm_tempOut next = new frm_tempOut(Admin_Name);
            next.MdiParent = this;
            next.Show();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            CloseAllChildForm();
            frm_Out next = new frm_Out();
            next.MdiParent = this;
            next.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Admin_Name = "";
            OleDbCommand checkLogin = new OleDbCommand();
            conn.Open();
            checkLogin.Connection = conn;
            checkLogin.CommandText = "SELECT LoginFirstName, LoginLastName FROM tb_login WHERE LoginName = '" + textBox1.Text + "' AND LoginPass = '" + textBox2.Text + "'";
            OleDbDataReader readerCheckLogin = checkLogin.ExecuteReader();
            int count = 0;
            while (readerCheckLogin.Read())
            {
                count = count + 1;
                if (count == 1)
                {
                    DataSet ds = new DataSet();
                    OleDbDataAdapter da = new OleDbDataAdapter("SELECT LoginFirstName, LoginLastName FROM tb_login WHERE LoginName = '" + textBox1.Text + "' AND LoginPass = '" + textBox2.Text + "'", conn);
                    da.Fill(ds, "FirstNameAndLastName");
                    foreach (DataRow dr in ds.Tables["FirstNameAndLastName"].Rows)
                    {
                        Admin_Name = dr.ItemArray[0] + " ";
                        Admin_Name += dr.ItemArray[1] + "";
                        break;
                    }
                    break;
                }
            }
            conn.Close();
            if (count == 1)
                {
                    MessageBox.Show("ยินดีต้อนรับเข้าสู่ระบบ");
                    toolStripButton1.Visible = true;
                    toolStripButton2.Visible = true;
                    toolStripButton3.Visible = true;
                    toolStripButton4.Visible = true;
                    toolStripButton5.Visible = true;
                    toolStripButton6.Visible = true;
                    toolStripButton7.Visible = false;
                    toolStripButton8.Visible = true;
                    toolStripButton9.Visible = true;
                    toolStripButton10.Visible = true;
                    toolStripButton11.Visible = true;
                    label1.Hide();
                    label2.Hide();
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox1.Hide();
                    textBox2.Hide();
                    button1.Hide();
                    CloseAllChildForm();
                    frm_Inv next = new frm_Inv();
                    next.MdiParent = this;
                    next.Show();
                }
                else
                {
                    MessageBox.Show("ข้อมูลไม่ถูกต้อง");
                }    
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label1.Hide();
            label2.Hide();
            textBox1.Hide();
            textBox2.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ออกจากระบบเรียบร้อย");
            toolStripButton1.Visible = false;
            toolStripButton2.Visible = false;
            toolStripButton3.Visible = false;
            toolStripButton4.Visible = false;
            toolStripButton5.Visible = false;
            toolStripButton6.Visible = false;
            toolStripButton7.Visible = true;
            toolStripButton9.Visible = false;
            toolStripButton10.Visible = false;
            toolStripButton11.Visible = false;
            label1.Show();
            label2.Show();
            textBox1.Show();
            textBox2.Show();
            button1.Show();
            CloseAllChildForm();
            frm_login next = new frm_login();
            next.MdiParent = this;
            next.Show();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label1.Show();
            label2.Show();
            textBox1.Show();
            textBox2.Show();
            button1.Show();
        }

        private void frm_Main_Load(object sender, EventArgs e)
        {
            ActiveControl = textBox1;
        }

        private void toolStripButton8_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("ออกจากระบบเรียบร้อย");
            toolStripButton1.Visible = false;
            toolStripButton2.Visible = false;
            toolStripButton3.Visible = false;
            toolStripButton4.Visible = false;
            toolStripButton5.Visible = false;
            toolStripButton6.Visible = false;
            toolStripButton7.Visible = true;
            toolStripButton8.Visible = false;
            toolStripButton9.Visible = false;
            toolStripButton10.Visible = false;
            toolStripButton11.Visible = false;
            label1.Show();
            label2.Show();
            textBox1.Show();
            textBox2.Show();
            button1.Show();
            CloseAllChildForm();
            frm_login next = new frm_login();
            next.MdiParent = this;
            next.Show();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            CloseAllChildForm();
            frm_Customer next = new frm_Customer();
            next.MdiParent = this;
            next.Show();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            CloseAllChildForm();
            MtrReport next = new MtrReport();
            next.MdiParent = this;
            next.Show();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            CloseAllChildForm();
            printt next = new printt();
            next.MdiParent = this;
            next.Show();
        }
    }
}
