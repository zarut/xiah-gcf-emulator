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

namespace WorldServerGUI
{
    public partial class WorldServerMain : Form
    {
        private WorldServer worldServer;

        public WorldServerMain()
        {
            InitializeComponent();

            worldServer = new WorldServer(Properties.Settings.Default.WorldServerIP,
                Properties.Settings.Default.WorldServerPort);

            worldServer.ClientConnected += new EventHandler<ClientConnectionEventArgs>(worldServer_ClientConnected);
        }

        void worldServer_ClientConnected(object sender, ClientConnectionEventArgs e)
        {
            Log(e.Client.IPAddress);
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            worldServer.BeginListen();
            ButtonStart.Enabled = false;
            ButtonStop.Enabled = true;
            Log("World server started.");
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            worldServer.Dispose();
            ButtonStop.Enabled = false;
            ButtonStart.Enabled = true;
            Log("World server stopped.");
        }

        delegate void LogDel(string text);

        private void Log(string text)
        {
            if (RichTextBoxLog.InvokeRequired)
            {
                RichTextBoxLog.Invoke(new LogDel(Log), new object[] { text });
            }
            else
            {
                RichTextBoxLog.AppendText(text + "\n");
            }
        }
    }
}
