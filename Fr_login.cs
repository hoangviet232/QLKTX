using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLKTX
{
    public partial class fr_login : Form

    {
        Database dt = new Database();
        public static int id_rold=0;
        public static string username = "";
        public fr_login()
        {
            InitializeComponent();
        }
        void xoatrang()
        {
            txtTK.Text = "";
            txtMK.Text = "";
        }

       
        private void btnDN_Click_1(object sender, EventArgs e)
        {
            ktra_login();
            if (id_rold == 1)
            {
                MessageBox.Show("Đăng nhập thành công");
                Fr_Main add = new Fr_Main();
                add.Show();
                xoatrang();
                this.Hide();
            }
            else if (id_rold == 2)
            {
                MessageBox.Show("Đăng nhập thành công");
                Fr_Main add = new Fr_Main();
                add.Show();
                xoatrang();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu sai");
            }


        }

        private int ktra_login()
        {
            string pass = FunctionMD5.Create_md5(txtMK.Text);
            DataTable data = dt.DataReader("select * from TaiKhoan where username=N'" + txtTK.Text.Trim() + "' and password=N'" + pass + "' ");

            foreach (DataRow data1 in data.Rows)
            {
                id_rold = int.Parse(data1["id_rold"].ToString());
                username = data1["username"].ToString();
            }
            return id_rold;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtMK.UseSystemPasswordChar = true;

            dt.OpenConnect();
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtMK.UseSystemPasswordChar = false;
            }
            else
            {
                txtMK.UseSystemPasswordChar = true;
            }
        }

        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát?", "Thông báo", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
                dt.CloseConnect();
                Application.Exit();
        }
        private void txtMK_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Gọi phương thức kiểm tra đăng nhập
                ktra_login();

                if (id_rold == 1 || id_rold == 2)
                {
                    MessageBox.Show("Đăng nhập thành công");
                    Fr_Main add = new Fr_Main();
                    add.Show();
                    xoatrang();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu sai");
                }
            }
        }

        /* private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
         {
             fr_dangky fr_dy= new fr_dangky();
             fr_dy.Show();
         }*/
    }
}   

