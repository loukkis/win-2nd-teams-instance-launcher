using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teams_2nd_instance
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;

        public Form1(string parameter)
        {
            var localappdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var downloads = Path.Combine(localappdata, @"Microsoft\Teams\Downloads\");
            // If directory does not exist, create it. 
            if (!Directory.Exists(downloads))
            {
                Directory.CreateDirectory(downloads);
            }

            //Initialize and create environment variables
            InitializeComponent();
            Environment.SetEnvironmentVariable("OLDPROFILE", Environment.GetEnvironmentVariable("USERPROFILE"));
            Environment.SetEnvironmentVariable("USERPROFILE", Environment.GetEnvironmentVariable("OLDPROFILE") + "\\appdata\\local\\microsoft\\teams\\");




            //InitializeComponent();
            MinimizeApp(parameter);
            

            this.components = new System.ComponentModel.Container();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();



            // Initialize contextMenu1
            this.contextMenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[2] {this.menuItem1,this.menuItem2});


            // Initialize menuItem1
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "E&xit";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);

            this.menuItem2.Index = 0;
            this.menuItem2.Text = "Launch!";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);

            notifyIcon1.ContextMenu = this.contextMenu1;

            //Get registry "startup" settings in application launch
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\"))
                {
                    if (key != null)
                    {

                        Object o = key.GetValue("Teams Launcher");
                        if (o != null)
                        {
                            checkBox1.Checked = true;
                        }
                        else
                        {
                            checkBox1.Checked = false;
                        }
                    }
                }
            }
            catch (Exception) 
            {
                
            }


        }


        //Launch second Teams instance with new virtual profile location
        private void launch_2nd()
        {
            System.Diagnostics.Process.Start(Environment.GetEnvironmentVariable("OLDPROFILE") + "\\appdata\\local\\microsoft\\teams\\update.exe", "--processStart Teams.exe");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            launch_2nd();
        }
        //Event handler for About
        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox1 a = new AboutBox1();
            a.Show();
        }

        //Event handler for tray Exit
        //Close the application and remove icon & dispose
        private void menuItem1_Click(object Sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            this.Close();
            Dispose();
        }
        private void menuItem2_Click(object Sender, EventArgs e)
        {
            launch_2nd();
        }

        //Bring application to active state when clicking notifyicon
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;

                // Activate the form
                Show();
                Activate();
            }
            
        }
        //Handle normal/minimized window actions
        private void Form1_Resize_1(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Visible = true;
                ShowInTaskbar = false;
            }
            if (WindowState == FormWindowState.Normal)
            {
                notifyIcon1.Visible = false;
                ShowInTaskbar = true;
                Activate();
            }
        }

        public void MinimizeApp(string parameter)
        {
            if (parameter == "-minimized")
            {
                //this.WindowState = FormWindowState.Normal;
                this.WindowState = FormWindowState.Minimized;
                notifyIcon1.Visible = true;
                ShowInTaskbar = false;
                notifyIcon1.BalloonTipTitle = "Teams Launcher";
                notifyIcon1.BalloonTipText = "Teams Launcher is started and running in the background...";
                notifyIcon1.ShowBalloonTip(500);
                //Hide();
            }

        }
 
        //Clean the stuff when closing the app
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon1.Visible = false;
            Dispose();
        }
        
        public static void AddApplicationToStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue("Teams Launcher", "\"" + Application.ExecutablePath + "\"" + " - minimized");
            }
        }

        public static void RemoveApplicationFromStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.DeleteValue("Teams Launcher", false);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                string exePath = Application.ExecutablePath;
                AddApplicationToStartup();
            }
            else
            {
                RemoveApplicationFromStartup();
            }
        }
    }

}
