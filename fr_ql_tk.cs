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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QLKTX
{
    public partial class fr_ql_tk : Form
    {
        private Database database = new Database();
        private string ID_NV = "";
        private DataTable resultTable = new DataTable(); // Declare resultTable

        public fr_ql_tk()
        {
            InitializeComponent();
        }

        private void HienChiTiet(bool hien)
        {
            txtUserName.Enabled = hien;
            txtHoten.Enabled = hien;
            dtpngaysinh.Enabled = hien;
            rdonam.Enabled = hien;
            rdonu.Enabled = hien;
            cbxRoles.Enabled = hien;
            txtDiachi.Enabled = hien;
            txtSDT.Enabled = hien;
            //Ẩn nút lưu,huỷ,ảnh
            btnluu.Enabled = hien;
            btnanh.Enabled = hien;
        }

        private void fr_ql_tk_Load(object sender, EventArgs e)
        {
            LoadData();
            dataGridView1.Columns[0].HeaderText = "ID NHÂN VIÊN";
            dataGridView1.Columns[1].HeaderText = "USER NAME";
            dataGridView1.Columns[2].HeaderText = "PASSWORD";
            dataGridView1.Columns[3].HeaderText = "ROLE_ID";
            dataGridView1.Columns[4].HeaderText = "TÊN";
            dataGridView1.Columns[5].HeaderText = "NGÀY SINH";
            dataGridView1.Columns[6].HeaderText = "SDT";
            dataGridView1.Columns[7].HeaderText = "ĐỊA CHỈ";
            dataGridView1.Columns[8].HeaderText = "GIỚI TÍNH";
            LoadData();
            //Ẩn nút Sửa,xóa
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            //Ẩn groupBox chi tiết
            HienChiTiet(false);

            txtTKIDNV.TextChanged += new EventHandler(txtTKIDNV_TextChanged);
            txtTKHoten.TextChanged += new EventHandler(txtTKHoten_TextChanged);
        }

        private void txtTKIDNV_TextChanged(object sender, EventArgs e)
        {
            TimKiem();
        }

        private void txtTKHoten_TextChanged(object sender, EventArgs e)
        {
            TimKiem();
        }

        private void TimKiem()
        {
            // Viet cau lenh SQL cho tim kiem
            string sql = "SELECT * FROM TaiKhoan WHERE id_NV IS NOT NULL ";

            // Tim theo mã phòng
            if (txtTKIDNV.Text.Trim() != "")
            {
                sql += " AND id_NV LIKE '%" + txtTKIDNV.Text + "%'";
            }

            // Tìm theo tên phòng
            if (txtTKHoten.Text.Trim() != "")
            {
                sql += " AND Ten LIKE N'%" + txtTKHoten.Text + "%'";
            }

            // Load dữ liệu tìm được lên dataGridView
            dataGridView1.DataSource = database.DataReader(sql);

            resultTable = database.DataReader(sql); // Assign result to resultTable
            dataGridView1.DataSource = resultTable;

            int count = resultTable.Rows.Count;

            // Hiển thị số lượng kết quả vào TextBox hoặc nơi bạn muốn
            txtkq.Text = count.ToString();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void Xoatrangdulieu()
        {
            txtUserName.Text = "";
            txtHoten.Text = "";
            dtpngaysinh.Value = DateTime.Today;
            rdonam.Checked = false;
            rdonu.Checked = false;
            cbxRoles.SelectedIndex = -1;
            txtDiachi.Text = "";
            txtSDT.Text = "";
        }

        private void LoadData()
        {
            dataGridView1.DataSource = database.DataReader("select * from TaiKhoan");
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell clickedCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

                if (clickedCell.Value != null && !string.IsNullOrEmpty(clickedCell.Value.ToString()))
                {
                    ID_NV = dataGridView1.CurrentRow.Cells[0].Value.ToString().Trim();
                    txtUserName.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    txtHoten.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                    dtpngaysinh.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                    txtDiachi.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                    txtSDT.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();

                    if (dataGridView1.CurrentRow.Cells[8].Value.ToString().Trim() == "Nam")
                    {
                        rdonam.Checked = true;
                    }
                    else
                    {
                        rdonu.Checked = true;
                    }

                    int selectedIndex;
                    if (int.TryParse(dataGridView1.CurrentRow.Cells[3].Value.ToString(), out selectedIndex))
                    {
                        cbxRoles.SelectedIndex = selectedIndex - 1;
                    }
                    else
                    {
                        cbxRoles.SelectedIndex = -1;
                    }
                    btnsua.Enabled = true;
                    btnxoa.Enabled = true;
                    HienChiTiet(false);
                }
                else
                {
                    MessageBox.Show("Vui lòng click vào nơi có data");
                }
            }
        }

        private void btntimkiem_Click(object sender, EventArgs e)
        {
            //Kiểm tra đã nhập thông tin tìm kiếm chưa
            if (txtTKIDNV.Text.Trim() == "")
            {
                errorProvider1.SetError(txtTKIDNV, "Vui lòng nhập ID nhân viên!");
            }
            else
            {
                errorProvider1.Clear();
            }
            if (txtTKHoten.Text.Trim() == "")
            {
                errorProvider1.SetError(txtTKHoten, "Vui lòng nhập tên nhân viên!");
            }
            else
            {
                errorProvider1.Clear();
            }
            //Cấm nút Sửa và Xóa
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            //Viet cau lenh SQL cho tim kiem
            string sql = "SELECT * FROM TaiKhoan where id_NV is not null ";
            //Tim theo mã sinh viên
            if (txtTKIDNV.Text.Trim() != "")
            {
                sql += " and id_NV like '%" + txtTKIDNV.Text.Trim() + "%'";
            }
            //Tìm theo tên sinh viên
            if (txtTKHoten.Text.Trim() != "")
            {
                sql += " and Ten like N'%" + txtTKHoten.Text + "%'";
            }
            //Load dữ liệu tìm được lên dataGridView
            dataGridView1.DataSource = database.DataReader(sql);
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            //Ẩn hai nút Thêm và Xóa
            btnxoa.Enabled = false;
            //Hiện gropbox chi tiết
            HienChiTiet(true);
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            //Bật Message Box cảnh báo người sử dụng
            if (MessageBox.Show("Bạn có muốn xoá không?", "Thông Báo"
               , MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                database.DataChange("Delete TaiKhoan Where id_NV = '" + ID_NV + "'");
                btnsua.Enabled = false;
                btnxoa.Enabled = false;
                Xoatrangdulieu();
                LoadData();
            }
            //Hiện gropbox chi tiết
            HienChiTiet(true);
        }

        private void btnluu_Click(object sender, EventArgs e)
        {
            string sql = "";
            // sử dụng control ErrorProvider để hiển thị lỗi
            //Kiểm tra đã nhập tên sinh viên chưa
            if (txtHoten.Text.Trim() == "")
            {
                errorProvider1.SetError(txtHoten, "Bạn phải nhập tên sinh viên!");
            }
            else
            {
                errorProvider1.Clear();
            }
            //Kiểm  tra các cbx bị thiếu
            if (cbxRoles.Text.Trim() == "" ||
                (rdonam.Checked == false && rdonu.Checked == false))
            {
                errorProvider1.SetError(cbxRoles, "Bạn phải chọn quyền");
            }
            else
            {
                errorProvider1.Clear();
            }
            //Nếu nút Sửa enable=true thì thực hiện cập nhật dữ liệu
            string Gioitinh = "";
            if (rdonam.Checked == true)
                Gioitinh = "Nam";
            if (rdonu.Checked == true)
                Gioitinh = "Nữ";

            if (btnsua.Enabled == true)
            {
                sql = "Update TaiKhoan SET Username ='" + txtUserName.Text.Trim() + "',NgaySinh='" + dtpngaysinh.Value.ToString("MM/dd/yyyy") + "'" +
                     ",GioiTinh=N'" + Gioitinh + "',id_rold='" + (cbxRoles.SelectedIndex + 1).ToString() + "',Ten=N'" + txtHoten.Text.ToString().Trim() + "',DiaChi=N'" + txtDiachi.Text.Trim() + "',SDT='" + txtSDT.Text.Trim() + "' Where id_NV='" + ID_NV + "'";
                dataGridView1.DataSource = database.DataReader(sql);
                LoadData();
                MessageBox.Show("Sửa thành công", "Thông báo");
            }
            //Nếu nút Xóa enable=true thì thực hiện xóa dữ liệu
            if (btnxoa.Enabled == true)
            {
                sql = "Delete TaiKhoan Where id_NV = '" + ID_NV + "'";
            }
            database.DataChange(sql);
            //Ẩn hiện các nút phù hợp chức năng
            Xoatrangdulieu();
            LoadData();
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông Báo",
             MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }

        private void bt_refesh_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}