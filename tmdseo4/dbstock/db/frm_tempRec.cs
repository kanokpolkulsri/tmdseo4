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
    public partial class frm_tempRec : Form
    {
        OleDbDataAdapter oda;
        DataTable dt;
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\thaimaidensha\\dbstock\\database.accdb");
        public frm_tempRec(string Admin_Name)
        {
            InitializeComponent();
            textBox4.Text = Admin_Name;
            this.WindowState = FormWindowState.Maximized;
        }

        private void frm_tempRec_Load(object sender, EventArgs e)
        { 
            oda = new OleDbDataAdapter("SELECT ID, RecDate, RecNo, RecName, RecAmount, RecUnit, RecPriceUnit, RecPriceTotal, RecPerson, RecStorage, RecComp FROM tb_tempRec", conn);
            dt = new DataTable();
            oda.Fill(dt);
            dataGridView1.DataSource = dt;
            textBox1.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            ActiveControl = textBox5;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            OleDbCommand addinv = new OleDbCommand();
            OleDbCommand checkInvNo = new OleDbCommand();
            addinv.Connection = conn;
            checkInvNo.Connection = conn;
            checkInvNo.CommandText = "SELECT * FROM tb_Inv WHERE InvNo = '"+textBox2.Text+"'";
            OleDbDataReader readerCheckInvNo = checkInvNo.ExecuteReader();
            int count = 0;
            while (readerCheckInvNo.Read())
            {
                count = count + 1;
                if (count == 1)
                {
                    break;
                }
            }
            //พบว่ามีสินค้าที่ต้องการอยู่
            if (count == 1)
            {
                DataSet ds = new DataSet();
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT InvName, InvUnit, InvPriceUnit, InvStorage FROM tb_Inv WHERE InvNo = '"+ textBox2.Text +"'", conn);
                da.Fill(ds, "Inv_name");
                string text_invname = " ", text_recunit = " ", text_priceunit = " ", text_storage = " ";
                foreach (DataRow dr in ds.Tables["Inv_name"].Rows)
                {
                    text_invname = dr.ItemArray[0] + "";
                    text_recunit = dr.ItemArray[1] + "";
                    text_priceunit = dr.ItemArray[2] + "";
                    text_storage = dr.ItemArray[3] + "";
                    break;
                }

                //หาว่าใน tempRec มีสินค้าชนิดนี้อยู่ไหม
                OleDbCommand checkInTempRec = new OleDbCommand();
                checkInTempRec.Connection = conn;
                checkInTempRec.CommandText = "SELECT * FROM tb_tempRec WHERE RecNo = '" + textBox2.Text + "' AND RecPerson = '" + textBox4.Text + "' AND RecComp = '" + textBox7.Text + "'";
                OleDbDataReader readerCheckInTempRec = checkInTempRec.ExecuteReader();
                int count2 = 0;
                while (readerCheckInTempRec.Read())
                {
                    count2 = count2 + 1;
                    if (count2 == 1)
                    {
                        break;
                    }
                }
                int RecAmount, rec_amount_tempRec;
                //พบว่ามีสินค้านี้อยู่ใน tempRec แล้ว
                if (count2 == 1)
                {
                    if (int.TryParse(textBox3.Text, out rec_amount_tempRec))
                    {
                        if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox7.Text != "")
                        {
                            addinv.CommandText = "UPDATE tb_tempRec SET RecAmount = RecAmount + '" + rec_amount_tempRec + "' WHERE RecNo = '" + textBox2.Text + "' AND RecPerson = '" + textBox4.Text + "' AND RecComp = '" + textBox7.Text + "'";
                            addinv.ExecuteNonQuery();
                            textBox1.Clear();
                            textBox2.Clear();
                            textBox3.Clear();
                        }
                        else
                        {
                            MessageBox.Show("กรอกข้อมูลไม่ครบ");
                        }
                    }
                    else
                    {
                        MessageBox.Show("กรอกข้อมูลไม่ครบ");
                    }
                }
                else
                {
                    if (textBox1.Text != "" && textBox2.Text != "" && int.TryParse(textBox3.Text, out RecAmount) && textBox4.Text != "")
                    {
                        addinv.CommandText = "INSERT into tb_tempRec (RecDate, RecNo, RecName, RecAmount, RecUnit, RecPriceUnit, RecPerson, RecStorage, RecComp) VALUES ('" + textBox1.Text + "','" + textBox2.Text + "', '" + text_invname + "', '" + RecAmount + "','" + text_recunit + "','" + text_priceunit + "','" + textBox4.Text + "','" + text_storage + "','" + textBox7.Text + "')";
                        addinv.ExecuteNonQuery();
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                    }
                    else
                    {
                        MessageBox.Show("กรอกข้อมูลไม่ครบ");
                    }
                }
            }
            else
            {
                MessageBox.Show("ไม่มีรหัสสินค้านี้");
            }
            conn.Close();
            frm_tempRec_Load(sender, e);
        }

        private void updateMtrByTempRec()
        {
            conn.Open();
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT RecNo, RecAmount, RecName, RecUnit, RecDate FROM tb_tempRec", conn);
            da.Fill(ds, "All_No_tempRec");
            string num_recNo = " ", text_Rec_Amount = " ", text_recName = " ", text_recUnit = " ", text_recDate = " ";
            string amountInInv = " ";
            int num_Rec_Amount = 0;
            OleDbCommand checkInMat = new OleDbCommand();
            checkInMat.Connection = conn;
            int count2 = 0;
            conn.Close();
            foreach (DataRow dr in ds.Tables["All_No_TempRec"].Rows)
            {
                num_recNo = dr.ItemArray[0] + "";
                text_Rec_Amount = dr.ItemArray[1] + "";
                text_recName = dr.ItemArray[2] + "";
                text_recUnit = dr.ItemArray[3] + "";
                text_recDate = dr.ItemArray[4] + "";
                //Console.WriteLine("{0} - {1} - {2} - {3} - {4}", num_recNo, text_Rec_Amount, text_recName, text_recUnit, text_recDate);
                if (int.TryParse(text_Rec_Amount, out num_Rec_Amount))
                {
                    //จะเช็คว่าใน material มีรหัสพวกนี้หรือยัง
                    conn.Open();
                    checkInMat.CommandText = "SELECT * FROM tb_Material WHERE MtrNo = '" + num_recNo + "' AND MtrDate = '" + textBox1.Text + "'";
                    OleDbDataReader readerCheckInMat = checkInMat.ExecuteReader();
                    //int count2 = 0;
                    while (readerCheckInMat.Read())
                    {
                        count2 = count2 + 1;
                        Console.WriteLine(count2);
                        if (count2 == 1)
                        {
                            break;
                        }
                    }
                    conn.Close();
                    conn.Open();
                    //พบว่ามีใน material คราวนี้จะอัพเดตค่าจำนวนที่รับเข้ามาใหม่
                    if (count2 == 1)
                    {
                        OleDbCommand updateRecInMat = new OleDbCommand();
                        updateRecInMat.Connection = conn;
                        updateRecInMat.CommandText = "UPDATE tb_Material SET MtrCurrent = MtrCurrent + '" + num_Rec_Amount + "' WHERE MtrNo = '" + num_recNo + "' AND MtrDate = '" + textBox1.Text + "'";
                        updateRecInMat.ExecuteNonQuery();
                    }
                    else // ยังไม่เคยมีการบันทึกเข้าใน material
                    {
                        DataSet ds2 = new DataSet();
                        OleDbDataAdapter da2 = new OleDbDataAdapter("SELECT InvAmount FROM tb_Inv WHERE InvNo = '" + num_recNo + "'", conn);
                        da2.Fill(ds2, "inv_amount");
                        foreach (DataRow dr2 in ds2.Tables["inv_amount"].Rows)
                        {
                            amountInInv = dr2.ItemArray[0] + "";
                        }
                        OleDbCommand insertRecInMat = new OleDbCommand();
                        insertRecInMat.Connection = conn;
                        insertRecInMat.CommandText = "INSERT into tb_Material(MtrDate, MtrNo, MtrName, MtrUnit, MtrCurrent, MtrBrought, MtrUsed) VALUES ('" + textBox1.Text + "', '" + num_recNo + "', '" + text_recName + "', '" + text_recUnit + "', '" + num_Rec_Amount + "', '" + amountInInv + "', 0)";
                        insertRecInMat.ExecuteNonQuery();

                    }
                    count2 = 0;
                    conn.Close();
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            updateMtrByTempRec();

            conn.Open();

            DataSet ds3= new DataSet();
            OleDbDataAdapter da3 = new OleDbDataAdapter("SELECT RecNo, RecAmount FROM tb_tempRec", conn);
            da3.Fill(ds3, "Inv_no");
            OleDbCommand addtoinv = new OleDbCommand();
            addtoinv.Connection = conn;

            string text_invno = " ", text_invamount = " ";
            foreach (DataRow dr in ds3.Tables["Inv_no"].Rows)
            {
                text_invno = dr.ItemArray[0] + "";
                text_invamount = dr.ItemArray[1] + "";
                addtoinv.CommandText = "UPDATE tb_Inv SET InvAmount = InvAmount + '"+text_invamount+"' WHERE InvNo = '" + text_invno + "'";
                addtoinv.ExecuteNonQuery();
            }

            //เพิ่มไปยังประวัติการรับสินค้า
            OleDbCommand addtorec = new OleDbCommand();
            addtorec.Connection = conn;
            addtorec.CommandText = "INSERT into tb_Rec(RecDate, RecNo, RecName, RecAmount, RecUnit, RecPriceUnit, RecPriceTotal, RecPerson, RecStorage, RecComp) SELECT RecDate, RecNo, RecName, RecAmount, RecUnit, RecPriceUnit, RecPriceTotal, RecPerson, RecStorage, RecComp FROM tb_tempRec";
            addtorec.ExecuteNonQuery();

            
            OleDbCommand deleteTempRec = new OleDbCommand();
            deleteTempRec.Connection = conn;
            deleteTempRec.CommandText = "DELETE * FROM tb_tempRec";
            deleteTempRec.ExecuteNonQuery();
            

            conn.Close();
            MessageBox.Show("บันทึกข้อมูลเรียบร้อย");
            frm_tempRec_Load(sender, e);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            int count = 0;
            if (textBox5.Text != "")
            {
                conn.Open();
                OleDbCommand checkname_bq = new OleDbCommand();
                checkname_bq.Connection = conn;
                //checkname_bq.CommandText = "SELECT * FROM tb_Inv WHERE InvNo LIKE '" + textBox5.Text + "%'";
                checkname_bq.CommandText = "SELECT * FROM tb_Inv WHERE InvNo = '" + textBox5.Text + "'";

                OleDbDataReader readerCheckNameBQ = checkname_bq.ExecuteReader();
                while (readerCheckNameBQ.Read())
                {
                    count = count + 1;
                    if (count > 1)
                    {
                        break;
                    }
                }
                conn.Close();
            }
            if (count == 1)
            {
                conn.Open();
                DataSet ds = new DataSet();
                //OleDbDataAdapter da = new OleDbDataAdapter("SELECT InvNo FROM tb_Inv WHERE InvNo LIKE '" + textBox5.Text + "%'", conn);
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT InvNo FROM tb_Inv WHERE InvNo = '" + textBox5.Text + "'", conn);
                da.Fill(ds, "Inv_no");
                string text_no = " ";
                foreach (DataRow dr in ds.Tables["Inv_no"].Rows)
                {
                    text_no = dr.ItemArray[0] + "";
                    break;
                }
                conn.Close();
                if (text_no != textBox2.Text)
                {
                    textBox2.Text = text_no;
                    textBox3.Text = "1";
                }
                else
                {
                    textBox3.Text = Convert.ToString(int.Parse(textBox3.Text) + 1);
                }
                textBox5.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            conn.Open();
            try
            {
                OleDbCommand delete = new OleDbCommand();
                delete.Connection = conn;
                delete.CommandText = "DELETE FROM tb_tempRec WHERE ID = " + textBox6.Text + "";
                delete.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("ไม่มี ID นี้อยู่ในระบบ");
            }
            frm_tempRec_Load(sender, e);
            textBox6.Clear();
            conn.Close();
        }
    }
}
