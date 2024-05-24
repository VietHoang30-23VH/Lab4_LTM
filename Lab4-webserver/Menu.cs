using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4_webserver
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bai1 bai1=new Bai1();
            bai1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bai2 bai2=new Bai2();
            bai2.ShowDialog();
        }

        private void btnBai3_Click(object sender, EventArgs e)
        {
            Bai3 bai3 = new Bai3();
            bai3.ShowDialog();

        }

        private void btnBai5_Click(object sender, EventArgs e)
        {
            Bai5 bai5 = new Bai5();
            bai5.ShowDialog();
        }

        private void btnBai6_Click(object sender, EventArgs e)
        {
            Bai6 bai6 = new Bai6();
            bai6.ShowDialog();
        }

        private void btnBai7_Click(object sender, EventArgs e)
        {
            Bai7_Login bai7_login = new Bai7_Login();
            bai7_login.ShowDialog();
        }
    }
}
