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
    public partial class fr_phong : Form
    {
        private Database dtbase = new Database();
        private DataTable resultTable = new DataTable(); // Declare resultTable

        public fr_phong()
        {
            InitializeComponent();
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            //nạp dữ liệu vào combobox
            cbxmanha.DataSource = dtbase.DataReader("Select MaNha,TenNha from tblKhuNha");
            cbxmanha.ValueMember = "MaNha";
            cbxmanha.DisplayMember = "TenNha";
            cbxmanha.Text = "";

            Loaddata();
            dgvphong.Columns[0].HeaderText = "MÃ PHÒNG";
            dgvphong.Columns[1].HeaderText = "TÊN PHÒNG";
            dgvphong.Columns[2].HeaderText = "MÃ NHÀ";
            dgvphong.Columns[3].HeaderText = "LOẠI PHÒNG";
            dgvphong.Columns[4].HeaderText = "SỐ NGƯỜI TỐI ĐA";
            dgvphong.Columns[5].HeaderText = "Số NGƯỜI ĐANG Ở";
            dgvphong.Columns[6].HeaderText = "GHI CHÚ";
            //ẩn nút sửa,xoá
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            Xoatrangdulieu();

            txtTKMP.TextChanged += new EventHandler(txtTKMP_TextChanged);
            txtTKtenphong.TextChanged += new EventHandler(txtTKtenphong_TextChanged);
        }

        private void Loaddata()
        {
            dgvphong.DataSource = dtbase.DataReader("Select * from tblPhong");
        }

        private void txtTKMP_TextChanged(object sender, EventArgs e)
        {
            TimKiem();
        }

        private void txtTKtenphong_TextChanged(object sender, EventArgs e)
        {
            TimKiem();
        }

        private void TimKiem()
        {
            // Viet cau lenh SQL cho tim kiem
            string sql = "SELECT * FROM tblPhong WHERE MaPhong IS NOT NULL ";

            // Tim theo mã phòng
            if (txtTKMP.Text.Trim() != "")
            {
                sql += " AND MaPhong LIKE '%" + txtTKMP.Text + "%'";
            }

            // Tìm theo tên phòng
            if (txtTKtenphong.Text.Trim() != "")
            {
                sql += " AND TenPhong LIKE N'%" + txtTKtenphong.Text + "%'";
            }

            // Load dữ liệu tìm được lên dataGridView
            dgvphong.DataSource = dtbase.DataReader(sql);

            resultTable = dtbase.DataReader(sql); // Assign result to resultTable
            dgvphong.DataSource = resultTable;

            int count = resultTable.Rows.Count;

            // Hiển thị số lượng kết quả vào TextBox hoặc nơi bạn muốn
            txtkq.Text = count.ToString();
        }

        private void txtsonguoitoida_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                MessageBox.Show("Bạn chỉ được nhập số nguyên");
                e.Handled = true;
            }
        }

        private void dgvphong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMP.Text = dgvphong.CurrentRow.Cells[0].Value.ToString();
            txttenphong.Text = dgvphong.CurrentRow.Cells[1].Value.ToString();
            cbxmanha.SelectedValue = dgvphong.CurrentRow.Cells[2].Value.ToString();
            cbxloaiphong.SelectedItem = dgvphong.CurrentRow.Cells[3].Value.ToString().Trim();
            txtsonguoitoida.Text = dgvphong.CurrentRow.Cells[4].Value.ToString();
            txtsonguoio.Text = dgvphong.CurrentRow.Cells[5].Value.ToString();
            txtghichu.Text = dgvphong.CurrentRow.Cells[6].Value.ToString();

            //Hiển thị các nút cần thiết
            btnsua.Enabled = true;
            btnxoa.Enabled = true;
            btnthem.Enabled = true;
        }

        private void btntimkiem_Click(object sender, EventArgs e)
        {
            //Kiểm tra đã nhập thông tin tìm kiếm chưa
            if (txtTKMP.Text.Trim() == "")
            {
                errorProvider1.SetError(txtTKMP, "Hãy Nhập Mã Phòng");
                errorProvider1.SetError(txtTKtenphong, "Hãy Nhập Tên Phòng");
            }
            else
            {
                errorProvider1.Clear();
            }
            //Cấm nút Sửa và Xóa
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            //Viet cau lenh SQL cho tim kiem
            string sql = "SELECT * FROM tblPhong where MaPhong is not null ";
            //Tim theo mã phòng
            if (txtTKMP.Text.Trim() != "")
            {
                sql += " and MaPhong like '%" + txtTKMP.Text + "%'";
            }
            //Tìm theo tên phòng
            if (txtTKtenphong.Text.Trim() != "")
            {
                sql += " and TenPhong like N'%" + txtTKtenphong.Text + "%'";
            }
            //Load dữ liệu tìm được lên dataGridView
            dgvphong.DataSource = dtbase.DataReader(sql);
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            //kiểm tra đã nhập đủ dữ liệu chưa
            if (txtMP.Text.Trim() == "" || txttenphong.Text.Trim() == "" ||
               cbxloaiphong.Text == "" || cbxmanha.Text == "" || txtsonguoitoida.Text.Trim() == ""
               || txtsonguoio.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập đủ dữ liệu");
                return;
            }
            //Kiểm tra mã có chưa
            string maphong = txtMP.Text;
            DataTable dtphong = dtbase.DataReader("Select * from tblPhong Where MaPhong ='" + maphong + "'");
            if (dtphong.Rows.Count > 0)
            {
                MessageBox.Show("Mã đã tồn tại. Hãy nhập mã khác");
                txtMP.Focus();
                return;
            }
            //Insert CSDL
            dtbase.DataChange("Insert into tblPhong Values('" + maphong + "'," +
                "N'" + txttenphong.Text + "','" + cbxmanha.SelectedValue.ToString() + "', " +
                "N'" + cbxloaiphong.Text + "','" + txtsonguoitoida.Text + "'," +
                "'" + txtsonguoio.Text + "',N'" + txtghichu.Text + "')");
            Xoatrangdulieu();
            Loaddata();
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            //kiểm tra
            if (txtMP.Text.Trim() == "" || txttenphong.Text.Trim() == "" ||
               cbxloaiphong.Text == "" || cbxmanha.Text == "" || txtsonguoitoida.Text.Trim() == ""
               || txtsonguoio.Text.Trim() == "")
            {
                MessageBox.Show("Bạn phải nhập đủ dữ liệu");
                return;
            }
            //cập nhật lại dữ liệu
            dtbase.DataChange("Update tblPhong  set TenPhong = N'" + txttenphong.Text + "'," +
               " MaNha = '" + cbxmanha.SelectedValue.ToString() + "',LoaiPhong = N'" + cbxloaiphong.Text + "'," +
               "SoNguoiToiDa='" + txtsonguoitoida.Text + "', SoNguoiDangO='" + txtsonguoio.Text + "', GhiChu=N'" + txtghichu.Text + "' Where MaPhong = '" + txtMP.Text + "' ");
            Loaddata();
            //Ẩn hiện các nút phù hợp
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            btnthem.Enabled = true;
            Xoatrangdulieu();
        }

        private void Xoatrangdulieu()
        {
            txtMP.Text = "";
            txttenphong.Text = "";
            cbxmanha.SelectedIndex = -1;
            txtsonguoitoida.Text = "";
            txtsonguoio.Text = "";
            txtghichu.Text = "";
            cbxloaiphong.SelectedIndex = -1;
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xoá không?", "Thông Báo"
               , MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DataTable data = dtbase.DataReader("select * from tblThietBiPhong where MaPhong='" + txtMP.Text.Trim() + "'");

                if (data != null && data.Rows.Count > 0)
                {
                    MessageBox.Show("Không thể xóa, vui lòng gỡ bỏ tất cả thiết bị trước khi xóa phòng!", "Thông báo");
                    return;
                }

                DataTable data2 = dtbase.DataReader("select * from tblSinhVienPhong where MaPhong='" + txtMP.Text.Trim() + "'");

                if (data2 != null && data2.Rows.Count > 0)
                {
                    MessageBox.Show("Không thể xóa, phòng này vẫn còn sinh viên ở!", "Thông báo");
                    return;
                }
                dtbase.DataChange("Delete tblPhong Where MaPhong= '" + txtMP.Text.Trim() + "' ");
                btnsua.Enabled = false;
                btnthem.Enabled = true;
                btnxoa.Enabled = false;
                Xoatrangdulieu();
                Loaddata();
            }
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông Báo",
              MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }

        private void cbxloaiphong_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}