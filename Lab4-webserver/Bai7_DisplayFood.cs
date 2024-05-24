using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4_webserver
{
    public partial class Bai7_DisplayFood : Form
    {
        int a = 0;
        string in4 = "";
        public Bai7_DisplayFood(Control selectedControl, string s, int dem)
        {
            InitializeComponent();
            flowLayoutPanel1.Controls.Add(selectedControl);
            in4 = s;
            a = dem;
        }
    }
}
