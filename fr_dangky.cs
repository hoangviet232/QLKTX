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
    public partial class fr_dangky : Form
    {
        int id_rold = fr_login.id_rold;
        Database dtbase  = new Database();
        DataTable dt = new DataTable();
        public fr_dangky()
        {
            InitializeComponent();
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            txt_holot.Text = "";
            txt_holot.Focus();
            txt_ten.Text = "";
            txt_password.Text = "";
            txt_sdt.Text = "";
            txt_usename.Text = "";
            
        }

        public void bt_dky_Click(object sender, EventArgs e)
        {
            string pass = txt_password.Text.Trim();
            string pass_md5=FunctionMD5.Create_md5(pass);
            string username= txt_usename.Text.Trim();
            DateTime brithday = date_brithday.Value;
            string holot= txt_holot.Text.Trim();
            string ten= txt_ten.Text.Trim();    
            string sdt = txt_sdt.Text.Trim();
            string diachi=txt_dchi.Text.Trim();
            string gioitinh = "";
            if(check_box_nam.Checked==true)
            {
                gioitinh = "Nam";
            }
            else if (checkbox_nu.Checked==true)
            {
                gioitinh = "Nữ";
            }
          

            if (txt_usename.Text.Trim() == "" && txt_password.Text.Trim() == "" && txt_ten.Text.Trim()=="")
            {
                MessageBox.Show("Bạn phải username, password, tên không được bỏ trống");
                return;
            }
            //Kiểm tra xem mã 
           
            DataTable dtlop = dtbase.DataReader("Select * from TaiKhoan where Username=N'" + username + "'");
            if (dtlop.Rows.Count > 0)
            {
                MessageBox.Show("Đã tồn tại username: " +username + ", Bạn hãy nhập username khác!");
                txt_usename.Focus();
                return;
            }

            int id_role = cbxroles.SelectedIndex;
            string sql_tk = "insert into TaiKhoan(username,password,id_rold,Ten,NgaySinh,SDT,DiaChi,GioiTinh) values(N'" + username + "',N'" + pass_md5 + "','" + id_role + "', N'" +holot + ten + "', '" + brithday + "','" + sdt + "',N'" + diachi + "',N'" + gioitinh + "')";
            dtbase.DataChange(sql_tk);
            MessageBox.Show("Tạo tài khoản thành công", "Thông báo");
           // fr_login fr_login = new fr_login();
           //fr_login.Show();

            
        }

        private void fr_dangky_Load(object sender, EventArgs e)
        {

            load_data_to_cbox();

        }

        private void check_box_nam_CheckedChanged(object sender, EventArgs e)
        {
            if(check_box_nam.Checked == true)
            {
                checkbox_nu.Checked = false;
            }
        }

        private void checkbox_nu_CheckedChanged(object sender, EventArgs e)
        {
            if (checkbox_nu.Checked == true)
            {

                check_box_nam.Checked = false;
            }
        }
        public void load_data_to_cbox()
        {

        }
    }
}
