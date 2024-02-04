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
    public partial class fr_traphong : Form
    {
        private Database dtbase = new Database();
        private DataTable resultTable = new DataTable(); // Declare resultTable

        public fr_traphong()
        {
            InitializeComponent();
        }

        private void Form13_Load(object sender, EventArgs e)
        {
            //nạp dữ liệu vào combobox
            cbxmst.DataSource = dtbase.DataReader("Select MaSoThue from tblSinhVienPhong");
            cbxmst.ValueMember = "MaSoThue";
            cbxmst.DisplayMember = "MaSoThue";
            cbxmst.Text = "";

            Loaddata();
            dgvtraphong.Columns[0].HeaderText = "MÃ SỐ THUÊ";
            dgvtraphong.Columns[1].HeaderText = "NGÀY TRẢ PHÒNG";
            dgvtraphong.Columns[2].HeaderText = "TIỀN VI PHẠM";
            dgvtraphong.Columns[0].Width = 150;
            dgvtraphong.Columns[1].Width = 150;
            dgvtraphong.Columns[2].Width = 150;

            //ẩn nút sửa,xoá
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            Xoatrangdulieu();

            txtTKMST.TextChanged += new EventHandler(txtTKMST_TextChanged);
        }

        private void txtTKMST_TextChanged(object sender, EventArgs e)
        {
            TimKiem();
        }

        private void TimKiem()
        {
            // Viet cau lenh SQL cho tim kiem
            string sql = "SELECT * FROM tblTraPhong WHERE MaSoThue IS NOT NULL ";

            // Tim theo mã phòng
            if (txtTKMST.Text.Trim() != "")
            {
                sql += " AND MaSoThue LIKE '%" + txtTKMST.Text + "%'";
            }

            // Load dữ liệu tìm được lên dataGridView
            dgvtraphong.DataSource = dtbase.DataReader(sql);
            resultTable = dtbase.DataReader(sql); // Assign result to resultTable
            dgvtraphong.DataSource = resultTable;

            int count = resultTable.Rows.Count;

            // Hiển thị số lượng kết quả vào TextBox hoặc nơi bạn muốn
            txtkq.Text = count.ToString();
        }

        private void Loaddata()
        {
            dgvtraphong.DataSource = dtbase.DataReader("Select * from tblTraPhong");
        }

        private void Xoatrangdulieu()
        {
            cbxmst.SelectedIndex = -1;
            dtpngaytra.Value = DateTime.Today;
            txttienvp.Text = "";
            txtTKMST.Text = "";
        }

        private void dgvtraphong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cbxmst.SelectedValue = dgvtraphong.CurrentRow.Cells[0].Value.ToString();
            dtpngaytra.Value = (DateTime)dgvtraphong.CurrentRow.Cells[1].Value;
            txttienvp.Text = dgvtraphong.CurrentRow.Cells[2].Value.ToString();

            //Hiển thị các nút cần thiết
            btnsua.Enabled = true;
            btnxoa.Enabled = true;
            btnthem.Enabled = true;
        }

        private void btnTK_Click(object sender, EventArgs e)
        {
            //Kiểm tra đã nhập thông tin tìm kiếm chưa
            if (txtTKMST.Text.Trim() == "")
            {
                errorProvider1.SetError(txtTKMST, "Hãy Nhập Mã Phòng");
            }
            else
            {
                errorProvider1.Clear();
            }
            //Cấm nút Sửa và Xóa
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            //Viết câu lệnh tìm kiếm sql
            string sql = "SELECT * FROM tblTraPhong where MaSoThue is not null ";
            //Tim theo mã phòng
            if (txtTKMST.Text.Trim() != "")
            {
                sql += " and MaSoThue like '%" + txtTKMST.Text + "%'";
            }
            //Load dữ liệu tìm được lên dataGridView
            dgvtraphong.DataSource = dtbase.DataReader(sql);
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            string tienViPham = txttienvp.Text;
            //kiểm tra đã nhập đủ dữ liệu chưa

            if (cbxmst.Text == "")
            {
                errorProvider1.SetError(cbxmst, "Hãy nhập mã số thuê");
                MessageBox.Show("Hãy nhập mã số thuê");
                return;
            }
            if (tienViPham == "")
            {
                tienViPham = "0";
            }

            DataTable data = dtbase.DataReader("select * from tblTraPhong where MaSoThue='" + cbxmst.SelectedValue.ToString().Trim() + "'");

            if (data != null && data.Rows.Count > 0)
            {
                MessageBox.Show("Xin lỗi, hợp đồng có mã: " + cbxmst.SelectedValue.ToString().Trim() + "đã trả phòng rồi!", "Thông báo");
                return;
            }

            //Insert CSDL
            dtbase.DataChange("Insert into tblTraPhong Values('" + cbxmst.SelectedValue.ToString().Trim() + "', " +
         "'" + dtpngaytra.Value.ToString("yyyy/MM/dd") + "','" + tienViPham + "')");

            string updateSLNguoiO = "UPDATE tblPhong SET SoNguoiDangO = SoNguoiDangO - 1"
                + "WHERE MaPhong IN (SELECT tblPhong.MaPhong FROM tblPhong JOIN tblSinhVienPhong ON tblSinhVienPhong.MaPhong = tblPhong.MaPhong"
                + " JOIN tblTraPhong ON tblTraPhong.MaSoThue = tblSinhVienPhong.MaSoThue WHERE tblTraPhong.MaSoThue = '" + cbxmst.SelectedValue.ToString().Trim() + "');";
            dtbase.DataChange(updateSLNguoiO);
            Xoatrangdulieu();
            Loaddata();
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            string tienViPham = txttienvp.Text;
            //kiểm tra
            if (cbxmst.Text == "")
            {
                errorProvider1.SetError(cbxmst, "Hãy nhập mã số thuê");
                MessageBox.Show("Bạn phải nhập đủ dữ liệu");
                return;
            }
            if (tienViPham == "")
            {
                tienViPham = "0";
            }
            //cập nhật lại dữ liệu
            dtbase.DataChange("Update tblTraPhong set Ngaytra = '" + dtpngaytra.Value.ToString("yyyy/MM/dd") + "'," +
                 "TienViPham='" + tienViPham + "' Where MaSoThue = '" + cbxmst.SelectedValue.ToString() + "'");
            Loaddata();
            //Ẩn hiện các nút phù hợp
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            btnthem.Enabled = true;
            Xoatrangdulieu();
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xoá không?", "Thông Báo"
            , MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                dtbase.DataChange("Delete tblTraPhong Where MaSoThue= '" + cbxmst.SelectedValue.ToString() + "' ");
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

        private void txttienvp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }
    }
}