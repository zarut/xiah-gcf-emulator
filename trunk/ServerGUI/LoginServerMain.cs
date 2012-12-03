using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerEngine;
using System.Net;
using XiahBLL;

namespace ServerGUI
{
    public partial class LoginServerMain : Form
    {
        private LoginServer loginServer;

        public LoginServerMain()
        {
            InitializeComponent();

            loginServer = new LoginServer(Properties.Settings.Default.LoginServerIP,
                Properties.Settings.Default.LoginServerPort);

            loginServer.ClientConnected += new EventHandler<ClientConnectionEventArgs>(loginServer_ClientConnected);
        }

        void loginServer_ClientConnected(object sender, ClientConnectionEventArgs e)
        {
            Log(e.Client.IPAddress);
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            loginServer.BeginListen();
            ButtonStart.Enabled = false;
            ButtonStop.Enabled = true;
            Log("Login server started.");
        }

        delegate void LogDel(string text);

        private void Log(string text)
        {
            if (RichTextBoxLog.InvokeRequired)
            {
                RichTextBoxLog.Invoke(new LogDel(Log), new object[]{text});
            }
            else
            {
                RichTextBoxLog.AppendText(text + "\n");
            }
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            loginServer.Dispose();
            ButtonStop.Enabled = false;
            ButtonStart.Enabled = true;
            Log("Login server stopped.");
        }

        private void LoginServerMain_Load(object sender, EventArgs e)
        {
            ButtonStart_Click(null, null);
        }
    }
}
