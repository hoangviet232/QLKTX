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
    public partial class fr_thiet_bi : Form
    {
        Database dtbase = new Database();
        public fr_thiet_bi()
        {
            InitializeComponent();
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            LoadData();
            dgvthietbi.Columns[0].HeaderText = "Mã Thiết Bị";
            dgvthietbi.Columns[1].HeaderText = "Tên Thiết Bị";
            dgvthietbi.Columns[0].Width = 150;
            dgvthietbi.Columns[1].Width = 150;
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
        
        }
        void LoadData()
        {
            DataTable dtThietBi = new DataTable();
            dtThietBi = dtbase.DataReader("select * from tblThietBi");
            dgvthietbi.DataSource = dtThietBi; //Gán dữ liệu vào dgv
        }
        void Xoatrangdulieu()
        {
            txttenthietbi.Text = "";
            txtmathietbi.Text = "";
        }

        private void dgvthietbi_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            btnsua.Enabled = true;
            btnxoa.Enabled = true;
            try
            {
                txtmathietbi.Text = dgvthietbi.CurrentRow.Cells[0].Value.ToString();
                txttenthietbi.Text = dgvthietbi.CurrentRow.Cells[1].Value.ToString();
            }
            catch
            {
            }
            LoadData();
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            if (txtmathietbi.Text.Trim() == "" || txttenthietbi.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập đủ dữ liệu");
                return;
            }
            //Kiểm tra xem mã có trùng trước khi thêm vào csdl
            string maque = txtmathietbi.Text;
            DataTable dtThietBi = dtbase.DataReader("Select * from tblThietBi where MaThietBi='" + txtmathietbi.Text + "'");
            if (dtThietBi.Rows.Count > 0)
            {
                MessageBox.Show("Đã có thiết bị với mã " + txtmathietbi.Text + ", Bạn hãy nhập mã khác");
                txtmathietbi.Focus();
                return;
            }
            //Tạo câu lệnh sql
            string sqlInsertThietBi = "insert into tblThietBi values('" + txtmathietbi.Text + "'," +
                "N'" + txttenthietbi.Text + "')";
            dtbase.DataChange(sqlInsertThietBi);
            LoadData();
            Xoatrangdulieu();
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            if (txtmathietbi.Text.Trim() == "" || txttenthietbi.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập đủ dữ liệu");
                return;
            }
            dtbase.DataChange("Update tblThietBi set MaThietBi = '" + txtmathietbi.Text + "'," +
                "TenThietBi = N'" + txttenthietbi.Text + "'Where MaThietBi='" + txtmathietbi.Text + "'");
            LoadData();
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            btnthem.Enabled = true;
            Xoatrangdulieu();
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa thiết bị có mã " + txtmathietbi.Text + " Không?","Thông Báo",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataTable data = dtbase.DataReader("select * from tblThietBiPhong where MaThietBi='" + txtmathietbi.Text.Trim() + "'");

                if (data != null && data.Rows.Count > 0)
                {
                    MessageBox.Show("Không thể xóa, vui lòng gỡ bỏ tất cả thiết bị khỏi phòng trước khi xóa thiết bị!", "Thông báo");
                }
                else
                {
                    dtbase.DataChange("delete tblThietBi where MaThietBi='" + txtmathietbi.Text + "'");
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

        private void dgvthietbi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtmathietbi.Text = dgvthietbi.CurrentRow.Cells[0].Value.ToString();
                txttenthietbi.Text = dgvthietbi.CurrentRow.Cells[1].Value.ToString();
            }
            catch
            {
            }
            LoadData();
            
            btnsua.Enabled = true;
            btnxoa.Enabled = true;
        }
    }
}
