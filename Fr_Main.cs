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
    public partial class Fr_Main : Form
    {
        private int id_rold = fr_login.id_rold;
        private string username = fr_login.username;

        public Fr_Main()
        {
            InitializeComponent();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            fr_login add = new fr_login();
            add.Show();
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void thôngTinSinhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fr_qly_sv add = new fr_qly_sv();
            add.Show();
        }

        private void tsmnLienhe_Click(object sender, EventArgs e)
        {
            fr_lien_he add = new fr_lien_he();
            add.Show();
        }

        private void khoaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fr_khoa add = new fr_khoa();
            add.Show();
        }

        private void lớpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fr_lop add = new fr_lop();
            add.Show();
        }

        private void thiếtBịToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fr_thiet_bi add = new fr_thiet_bi();
            add.Show();
        }

        private void khuNhàToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fr_khu_nha add = new fr_khu_nha();
            add.Show();
        }

        private void quảnLýPhòngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fr_phong add = new fr_phong();
            add.Show();
        }

        private void quảnLýToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fr_thuephong add = new fr_thuephong();
            add.Show();
        }

        private void quảnLýThiếtBịToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fr_quanly_thietbi add = new fr_quanly_thietbi();
            add.Show();
        }

        private void quảnLýToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fr_traphong add = new fr_traphong();
            add.Show();
        }

        private void chiPhíPhòngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fr_chi_phiphong add = new fr_chi_phiphong();
            add.Show();
        }

        private bool hp;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (hp)
            {
                if ((lblchu.Left + lblchu.Width) < this.Width)
                    lblchu.Left = lblchu.Left + 5;
                else
                    hp = false;
            }
            else
            {
                if (lblchu.Left > 0)
                    lblchu.Left = lblchu.Left - 5;
                else
                    hp = true;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            hp = true;
            timer1.Start();
            if (id_rold == 1)//tk admin
            {
                tạoTàiKhoảnToolStripMenuItem.Visible = true;
                quảnLýTàiKhoảnToolStripMenuItem.Visible = true;
            }
            else
            {
                tạoTàiKhoảnToolStripMenuItem.Visible = false;
                quảnLýTàiKhoảnToolStripMenuItem.Visible = false;
            }

            label_xinchao.Text = "Xin chào : " + username;
        }

        private void tạoTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (id_rold == 1)
            {
                fr_dangky fr_dky = new fr_dangky();
                fr_dky.Show();
            }
        }

        private void mnsptuychon_Click(object sender, EventArgs e)
        {
        }

        private void quảnLýTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fr_ql_tk add = new fr_ql_tk();
            add.Show();
        }

        private void mnspquanli_Click(object sender, EventArgs e)
        {
        }
    }
}