using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;


namespace nengajo
{
    public partial class Form1 : Form
    {
        private bool[] hantei= new bool[11];
        private string find, ins, upd, del;
        private DataSet ds;
        private int r;


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Control[] ctext = new Control[11] { textBox1, textBox2, textBox7, textBox8, textBox9,
                textBox10, textBox11, textBox3, textBox4, textBox5, textBox6 };
            string[] record = new string[11] { "姓", "名", "姓２", "名２", "名３", "名４",
                "名５", "郵便番号１", "郵便番号２", "住所１", "住所２" };
            for (int i = 0; i < 11; i++)
            {
                
                hantei[i] = String.IsNullOrEmpty(((TextBox)ctext[i]).Text);
                if (hantei[i]) { }
                else
                {
                    //クエリ用コマンド（where以下）作成
                    find += "(" + record[i] + " like '%" + ((TextBox)ctext[i]).Text + "%') and";
                }
            }
            if (find == "")
            {
                MessageBox.Show("値が入っていません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            find = find.Remove(find.Length - 3, 3);

            //SQLサーバーからのデータ格納用DataSetを作成
            ds = new DataSet();

            //App.configのconnectionStrings内情報を取得してSQLサーバーへ接続
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
            con.Open();

            try
            {
                //クエリ用コマンド確定し、findの内容をクリア
                string sqlstr = "SELECT * FROM jusyoroku where " + find;
                find = "";

                //SQLサーバーへクエリ送信し、結果をsdaへ格納
                SqlDataAdapter sda = new SqlDataAdapter(sqlstr,con);

                //結果の形式はそのままにして、jusyorokuテーブルの内容をdataGridView1へ反映
                sda.FillSchema(ds, SchemaType.Source);
                sda.Fill(ds,"jusyoroku");
                dataGridView1.DataSource = ds.Tables["jusyoroku"];
            }
            catch
            {
                MessageBox.Show("SQLエラー", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
            finally
            {
                // データベースの接続終了
                con.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Control[] ctext = new Control[11] { textBox1, textBox2, textBox7, textBox8, textBox9,
                textBox10, textBox11, textBox3, textBox4, textBox5, textBox6 };
            ins = "姓, 名, 姓２, 名２, 名３, 名４, 名５, 郵便番号１, 郵便番号２, 住所１, 住所２) values ('";

            for (int i = 0; i < 11; i++)
            {
                hantei[i] = String.IsNullOrEmpty(((TextBox)ctext[i]).Text);
                if (hantei[i])
                {
                    ins += "','";
                }
                else
                {
                    ins += ((TextBox)ctext[i]).Text + "','";
                }
            }

            ins = ins.Remove(ins.Length - 2, 2) + ")";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
            con.Open();

            try
            {
                string sqlstr = "insert into jusyoroku (" + ins;

                ins = "";

                SqlCommand sc = new SqlCommand(sqlstr, con);

                sc.ExecuteNonQuery();
                MessageBox.Show("レコードに追加しました。", "追加",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {
                MessageBox.Show("SQLエラー", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // データベースの接続終了
                con.Close();
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            del = dataGridView1.Rows[r].Cells[0].Value.ToString();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
            con.Open();

            try
            {
                string sqlstr = "delete from jusyoroku where ID = " + del;

                del = "";

                SqlCommand sc = new SqlCommand(sqlstr, con);

                sc.ExecuteNonQuery();
                MessageBox.Show("レコードID" + dataGridView1.Rows[r].Cells[0].Value.ToString() + "を削除しました。", "更新",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {
                MessageBox.Show("SQLエラー", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // データベースの接続終了
                con.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            r = dataGridView1.CurrentCell.RowIndex;
            Control[] ctext = new Control[11] { textBox1, textBox2, textBox7, textBox8, textBox9,
                textBox10, textBox11, textBox3, textBox4, textBox5, textBox6 };
            string[] record = new string[11] { "姓", "名", "姓２", "名２", "名３", "名４",
                "名５", "郵便番号１", "郵便番号２", "住所１", "住所２" };
            for (int i = 0; i < 11; i++)
            {

                hantei[i] = String.IsNullOrEmpty(((TextBox)ctext[i]).Text);
                if (hantei[i]) { }
                else
                {
                    upd += record[i] + " = '" + ((TextBox)ctext[i]).Text + "',";
                    find += "(" + record[i] + " like '%" + ((TextBox)ctext[i]).Text + "%') and";
                }
            }
            upd = upd.Remove(upd.Length - 1, 1);
            find = find.Remove(find.Length - 3, 3);

            ds = new DataSet();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
            con.Open();

            try
            {
                string sqlstr = "update jusyoroku set " + upd + " where ID = "
                    + dataGridView1.Rows[r].Cells[0].Value.ToString();

                upd = "";

                SqlCommand sc = new SqlCommand(sqlstr, con);

                sc.ExecuteNonQuery();
                MessageBox.Show("レコードID" + dataGridView1.Rows[r].Cells[0].Value.ToString() + "を更新しました。", "更新",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                sqlstr = "SELECT * FROM jusyoroku where " + find;

                find = "";

                SqlDataAdapter sda = new SqlDataAdapter(sqlstr, con);

                sda.FillSchema(ds, SchemaType.Source);
                sda.Fill(ds, "jusyoroku");
                dataGridView1.DataSource = ds.Tables["jusyoroku"];
            }
            catch
            {
                MessageBox.Show("SQLエラー", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // データベースの接続終了
                con.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Control[] ctext = new Control[11] { textBox1, textBox2, textBox7, textBox8, textBox9,
                textBox10, textBox11, textBox3, textBox4, textBox5, textBox6 };
            for (int i = 0; i < 11; i++)
            {
                ctext[i].Text = "";
            }
        }

        private void dataGridView1_select(object sender, EventArgs e)
        {
            r = dataGridView1.CurrentCell.RowIndex;            
            textBox1.Text = dataGridView1.Rows[r].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.Rows[r].Cells[2].Value.ToString();
            textBox7.Text = dataGridView1.Rows[r].Cells[3].Value.ToString();
            textBox8.Text = dataGridView1.Rows[r].Cells[4].Value.ToString();
            textBox9.Text = dataGridView1.Rows[r].Cells[5].Value.ToString();
            textBox10.Text = dataGridView1.Rows[r].Cells[6].Value.ToString();
            textBox11.Text = dataGridView1.Rows[r].Cells[7].Value.ToString();
            textBox3.Text = dataGridView1.Rows[r].Cells[8].Value.ToString();
            textBox4.Text = dataGridView1.Rows[r].Cells[9].Value.ToString();
            textBox5.Text = dataGridView1.Rows[r].Cells[10].Value.ToString();
            textBox6.Text = dataGridView1.Rows[r].Cells[11].Value.ToString();
        }
    }
}
