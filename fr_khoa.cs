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

namespace QLKTX
{
    public partial class fr_khoa : Form
    {
        public fr_khoa()
        {
            InitializeComponent();
        }
        //Khai báo và khởi tạo biến toàn cục trong class frm5 sử dụng class DataBaseProcess
        Database dtbase = new Database();
        private void Form5_Load(object sender, EventArgs e)
        {
            LoadData();
            dgvkhoa.Columns[0].HeaderText = "MÃ KHOA";
            dgvkhoa.Columns[1].HeaderText = "TÊN KHOA";
            dgvkhoa.Columns[0].Width = 200;
            dgvkhoa.Columns[1].Width = 200;
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
        }
        void LoadData()
        {
            DataTable dtKhoa = new DataTable();
            dtKhoa = dtbase.DataReader("select * from tblKhoa");
            dgvkhoa.DataSource = dtKhoa; //Gán dữ liệu vào dgv
        }
        void Xoatrangdulieu()
        {
            txtmakhoa.Text = "";
            txttenkhoa.Text = "";
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông báo",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            if (txtmakhoa.Text.Trim() == "" || txttenkhoa.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập đủ dữ liệu");
                return;
            }
            //Kiểm tra xem mã có trùng trước khi thêm vào csdl
            string makhoa = txtmakhoa.Text;
            DataTable dtkhoa = dtbase.DataReader("Select * from tblKhoa where MaKhoa='" + makhoa + "'");
            if (dtkhoa.Rows.Count > 0)
            {
                MessageBox.Show("Đã có khoa với mã " + makhoa + ", Bạn hãy nhập mã khác");
                txtmakhoa.Focus();
                return;
            }
            //Tạo câu lệnh sql
            string sqlInsertKhoa = "insert into tblKhoa values('" + txtmakhoa.Text.Trim() + "',N'" + txttenkhoa.Text + "')";
            dtbase.DataChange(sqlInsertKhoa);
            LoadData();
            MessageBox.Show("Thêm thành công", "Thông báo");
            Xoatrangdulieu();
        }

        private void btnsua_Click_1(object sender, EventArgs e)
        {
            
            if (txtmakhoa.Text.Trim() == "" || txttenkhoa.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập đủ dữ liệu");
                return;
            }
            dtbase.DataChange("Update tblKhoa set MaKhoa = '" + txtmakhoa.Text.Trim() + "',TenKhoa = N'" + txttenkhoa.Text + "'Where MaKhoa='" + txtmakhoa.Text + "' ");
            LoadData();
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            btnThem.Enabled = true;
            MessageBox.Show("Sửa thành công","Thông báo");
            Xoatrangdulieu();
        }

        private void btnxoa_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa khoa có mã " + txtmakhoa.Text + " không?",
                "TB", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataTable data = dtbase.DataReader("select * from tblLop where MaKhoa='" + txtmakhoa.Text.Trim() + "'");

                if (data != null && data.Rows.Count > 0)
                {
                    MessageBox.Show("Không thể xóa, khoa này vẫn còn lớp!", "Thông báo");
                    return;
                }
                DataTable data2 = dtbase.DataReader("select * from tblSinhVien where MaKhoa='" + txtmakhoa.Text.Trim() + "'");

                if (data2 != null && data2.Rows.Count > 0)
                {
                    MessageBox.Show("Không thể xóa, khoa này vẫn còn sinh viên!", "Thông báo");
                    return;
                }
                dtbase.DataChange("delete tblKhoa where MaKhoa='" + txtmakhoa.Text + "'");
                    btnsua.Enabled = false;
                    btnxoa.Enabled = false;
                    btnThem.Enabled = true;
                    LoadData();
                    Xoatrangdulieu();

            }
        }
        private void dgvkhoa_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            btnsua.Enabled = true;
            btnxoa.Enabled = true;
            try
            {
                txtmakhoa.Text = dgvkhoa.CurrentRow.Cells[0].Value.ToString();
                txttenkhoa.Text = dgvkhoa.CurrentRow.Cells[1].Value.ToString();

            }
            catch
            {

            }
            LoadData();
        }
    }
}
