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
    public partial class fr_khu_nha : Form
    {
        Database databasse = new Database();
        public fr_khu_nha()
        {
            InitializeComponent();
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            LoadData();
            dgvkhunha.Columns[0].HeaderText = "MÃ NHÀ";
            dgvkhunha.Columns[1].HeaderText = "TÊN NHÀ";
            dgvkhunha.Columns[0].Width = 150;
            dgvkhunha.Columns[1].Width = 150;
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
        }

        void LoadData()
        {
            DataTable dtkhunha = new DataTable();
            dtkhunha = databasse.DataReader("select * from tblKhuNha");
            dgvkhunha.DataSource = dtkhunha; //Gán dữ liệu vào dgv
        }
        void Xoatrangdulieu()
        {
            txtmanha.Text = "";
            txttennha.Text = "";
        }

        private void dgvkhunha_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnsua.Enabled = true;
            btnxoa.Enabled = true;
            try
            {
                txtmanha.Text = dgvkhunha.CurrentRow.Cells[0].Value.ToString();
                txttennha.Text = dgvkhunha.CurrentRow.Cells[1].Value.ToString();
            }
            catch
            {
            }
            LoadData();
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            if (txtmanha.Text.Trim() == "" || txttennha.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập đủ dữ liệu");
                return;
            }
            //Kiểm tra xem mã có trùng trước khi thêm vào csdl
            string manha = txtmanha.Text;
            DataTable dtNha = databasse.DataReader("Select * from tblKhuNha where MaNha='" + manha + "'");
            if (dtNha.Rows.Count > 0)
            {
                MessageBox.Show("Đã có nhà với mã " + manha + ", Bạn hãy nhập mã khác");
                txtmanha.Focus();
                return;
            }
            //Tạo câu lệnh sql
            string sqlInsertNha = "Insert Into tblKhuNha Values('" + txtmanha.Text + "',N'" + txttennha.Text + "')";
            databasse.DataChange(sqlInsertNha);
            LoadData();
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            if (txtmanha.Text.Trim() == "" || txttennha.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập đủ dữ liệu");
                return;
            }
            databasse.DataChange("Update tblKhuNha set MaNha = '" + txtmanha.Text + "'," +
                "TenNha = N'" + txttennha.Text + "'Where MaNha='" + txtmanha.Text + "'");
            LoadData();
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            btnthem.Enabled = true;
            Xoatrangdulieu();
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa nhà có mã " + txtmanha.Text + " không?", "Thông Báo",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataTable data = databasse.DataReader("select * from tblPhong where MaNha='" + txtmanha.Text.Trim() + "'");

                if (data != null && data.Rows.Count > 0)
                {
                    MessageBox.Show("Không thể xóa,vui lòng xóa tất cả các phòng trước khi xóa 'Nhà'!", "Thông báo");
                }
                else
                {
                    databasse.DataChange("delete tblKhuNha where MaNha='" + txtmanha.Text + "'");
                    btnsua.Enabled = false;
                    btnxoa.Enabled = false;
                    btnthem.Enabled = true;
                    LoadData();
                    Xoatrangdulieu();
                }
            }
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông báo",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }
    }
}
