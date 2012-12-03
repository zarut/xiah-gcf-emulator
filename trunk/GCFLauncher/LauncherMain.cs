using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;
using GCFLauncher.LauncherServiceNS;

namespace GCFLauncher
{
    public partial class LauncherMain : Form
    {
        private string loginServerIp, account, password, hash = "";
        private int loginServerPort;
        private LauncherServiceClient service;
        
        public LauncherMain()
        {
            InitializeComponent();

            loginServerIp = Properties.Settings.Default.LoginServerIP;
            loginServerPort = Properties.Settings.Default.LoginServerPort;
            service = new LauncherServiceClient();
            if (service.IsServerOn())
            {
                Enable();
            }
            else
            {
                Disable();
            }
        }

        private string MakeInfoDat(string ip)
        {
            char[] asd = ip.ToCharArray();
            for (int i = 0; i < ip.Length; i++)
            {
                if (asd[i] == 0x09)
                    break;
                if (asd[i] == 0x0A)
                    break;
                asd[i] ^= (char)0x1F;
            }
            return new string(asd);
        }

        private void AccountBox_TextChanged(object sender, EventArgs e)
        {
            account = AccountBox.Text;
        }

        private void PasswordBox_TextChanged(object sender, EventArgs e)
        {
            password = PasswordBox.Text;
        }

        private void Enable()
        {
            AccountBox.Enabled = true;
            PasswordBox.Enabled = true;
            btnConnect.Enabled = true;
        }

        private void Disable()
        {
            AccountBox.Enabled = false;
            PasswordBox.Enabled = false;
            btnConnect.Enabled = false;
        }

        private void HideStuff()
        {

            btnLaunch.Show();

            AccountBox.Hide();
            PasswordBox.Hide();
            btnConnect.Hide();
            LabelPassword.Hide();
            LabelAccount.Text = "Logged in as " + account;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            account = AccountBox.Text;
            password = PasswordBox.Text;
            if (account == "" || password == "")
                MessageBox.Show("Fill up the textboxes dimwit.");
            else
            {
                bool loginSuccess = service.TryLogin(out hash, account, password);
                if (loginSuccess)
                {
                    HideStuff();
                }
                else
                {
                    MessageBox.Show("Login failed. Please check your username and password.");
                }
            }
        }

        private void LauncherMain_Load(object sender, EventArgs e)
        {

        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            try
            {
                bool exists = File.Exists(@"XiahGXClient.exe");
                if (exists)
                {
                    TextWriter tw = new StreamWriter("Info.dat");
                    //tw.WriteLine(1);
                    tw.WriteLine(1);
                    tw.WriteLine(MakeInfoDat(loginServerIp));
                    tw.WriteLine(Properties.Settings.Default.LoginServerPort);
                    tw.Close();

                    Process.Start("XiahGXClient.exe", account + "|" + hash);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("XiahGXClient cannot be found!");
                }
            }
            catch (Exception ea)
            {
                MessageBox.Show(ea.ToString());
            }
        }

    }
}
