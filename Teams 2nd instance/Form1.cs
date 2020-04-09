using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teams_2nd_instance
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Environment.SetEnvironmentVariable("OLDPROFILE", Environment.GetEnvironmentVariable("USERPROFILE"));
            Environment.SetEnvironmentVariable("USERPROFILE", Environment.GetEnvironmentVariable("OLDPROFILE") + "\\appdata\\local\\microsoft\\teams\\");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Environment.GetEnvironmentVariable("OLDPROFILE") + "\\appdata\\local\\microsoft\\teams\\update.exe", "--processStart Teams.exe");
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox1 a = new AboutBox1();
            a.Show();
        }
    }
}
