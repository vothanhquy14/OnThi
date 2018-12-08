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

namespace Buoi_Cuoi_SQL_TiepTheo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection cnn = new SqlConnection(@"Data Source = VTHANHQUD8DD\SQLEXPRESS; Initial Catalog = NhanVien2; Integrated Security = True");
        SqlDataAdapter daNV;

        SqlDataAdapter daCVu; //chu yeu de load combobox chuc vu len

        DataSet ds = new DataSet();


        private void Form1_Load(object sender, EventArgs e)
        {
            string query = "select * from chucvu";
            daCVu = new SqlDataAdapter(query, cnn);
            daCVu.Fill(ds, "chucvu");
            comboBox_chucvu.DataSource = ds.Tables[0];


            comboBox_chucvu.DisplayMember = "tenchucvu";
            comboBox_chucvu.ValueMember = "machucvu";


            Refesh();

            //them
            

            string query_them = "insert into nhanvien values (@maso, @ho,@ten ,@ngaysinh ,@gioitinh,@machucvu)";
            SqlCommand cmd_them = new SqlCommand(query_them, cnn);
            cmd_them.Parameters.Add("@maso",SqlDbType.NVarChar,20, "maso");
            cmd_them.Parameters.Add("@ho", SqlDbType.NVarChar,20, "ho");
            cmd_them.Parameters.Add("@ten", SqlDbType.NVarChar,20, "ten");
            cmd_them.Parameters.Add("@ngaysinh", SqlDbType.DateTime,10, "ngaysinh");
            cmd_them.Parameters.Add("@gioitinh", SqlDbType.NVarChar, 20, "gioitinh");
            cmd_them.Parameters.Add("@machucvu", SqlDbType.NVarChar, 2, "machucvu");

            daNV.InsertCommand = cmd_them;
            


            

            //xoa

            string query_xoa = "delete from nhanvien where maso=@maso";
            SqlCommand cmd_xoa = new SqlCommand(query_xoa, cnn);
            cmd_xoa.Parameters.Add("@maso", SqlDbType.NVarChar, 20, "maso");

            daNV.DeleteCommand = cmd_xoa;


            //sua
            string query_sua = "update NhanVien set maso=@maso, ho=@ho, ten=@ten, ngaysinh=@ngaysinh, gioitinh=@gioitinh, machucvu=@machucvu  where maso=@maso";
            SqlCommand cmd_sua = new SqlCommand(query_sua, cnn);
            cmd_sua.Parameters.Add("@maso", SqlDbType.NVarChar, 20, "maso");
            cmd_sua.Parameters.Add("@ho", SqlDbType.NVarChar, 20, "ho");
            cmd_sua.Parameters.Add("@ten", SqlDbType.NVarChar, 20, "ten");
            cmd_sua.Parameters.Add("@ngaysinh", SqlDbType.DateTime, 10, "ngaysinh");
            cmd_sua.Parameters.Add("@gioitinh", SqlDbType.NVarChar, 20, "gioitinh");
            cmd_sua.Parameters.Add("@machucvu", SqlDbType.NVarChar, 2, "machucvu");

            daNV.UpdateCommand = cmd_sua;


        }

        

        public void Refesh()
        {
            string query = @"select nv.*, cv.tenchucvu from NhanVien as nv, ChucVu as cv where nv.machucvu = cv.machucvu ";
            daNV = new SqlDataAdapter(query, cnn);
            daNV.Fill(ds, "nhanvien");
            dataGridView1.DataSource = ds.Tables[1];
          
            //dataGridView1.Columns["machucvu"].Visible = false;


        }
        private void button_thoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int current = dataGridView1.CurrentCell.RowIndex;
            DataGridViewRow dr = dataGridView1.Rows[current];

            textBox_ms.Text = dr.Cells["maso"].Value.ToString();
            textBox_ho.Text = dr.Cells["ho"].Value.ToString();
            textBox_ten.Text = dr.Cells["ten"].Value.ToString();
            comboBox_chucvu.SelectedValue = dr.Cells["machucvu"].Value.ToString();
            dateTimePicker1.Text = dr.Cells["ngaysinh"].Value.ToString();

            if (dr.Cells["gioitinh"].Value.ToString() == "nu")
                radioButton_nu.Checked = true;
            else
                radioButton_nam.Checked = true;
        }

        private void button_them_Click(object sender, EventArgs e)
        {
            DataRow row = ds.Tables["nhanvien"].NewRow();
            row["maso"] = textBox_ms.Text;
            row["ho"] = textBox_ho.Text;
            row["ten"] = textBox_ten.Text;
            if(radioButton_nam.Checked == true)
            {
                row["gioitinh"] = "nam";
            }
            else
            {
                row["gioitinh"] = "nu";
            }
            row["ngaysinh"] = dateTimePicker1.Value;
            row["machucvu"] = comboBox_chucvu.SelectedValue;

            ds.Tables["nhanvien"].Rows.Add(row);
        }

        private void button_luu_Click(object sender, EventArgs e)
        {
            daNV.Update(ds, "nhanvien");
            MessageBox.Show("Done !");
            dataGridView1.Refresh();

        }

        private void button_xoa_Click(object sender, EventArgs e)
        {
            DataGridViewRow dgv1 = dataGridView1.SelectedRows[0];
            dataGridView1.Rows.Remove(dgv1);
        }

        private void button_sua_Click(object sender, EventArgs e)
        {
            int current = dataGridView1.CurrentCell.RowIndex;
            DataGridViewRow dr = dataGridView1.Rows[current];

            dataGridView1.BeginEdit(true);

            dr.Cells["maso"].Value = textBox_ms.Text;
            dr.Cells["ho"].Value = textBox_ho.Text;
            dr.Cells["ten"].Value = textBox_ten.Text;
            if (radioButton_nam.Checked == true)
            {
                dr.Cells["gioitinh"].Value = "nam";
            }
            else
            {
                dr.Cells["gioitinh"].Value = "nu";
            }
            dr.Cells["ngaysinh"].Value = dateTimePicker1.Value;
            dr.Cells["machucvu"].Value = comboBox_chucvu.SelectedValue;

            dataGridView1.EndEdit();

        }
    }
}
