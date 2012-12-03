using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace ServerBooter
{
    public partial class Form1 : Form
    {
        public string LoginServerName = "ServerGUI.exe";
        public string ChannelServerName = "ChannelGUI.exe";

        public string LoginPath = @"C:\Programming\C#\XiahGlobalServer\ServerGUI\bin\Debug\ServerGUI.exe";
        public string ChannelPath = @"C:\Programming\C#\XiahGlobalServer\ChannelGUI\bin\Debug\ChannelGUI.exe";

        public Process LoginProcess = null;
        public Process ChannelProcess = null;

        public int Invertal = 7200000;
        public Timer Booter;

        public int bootCount = 0;

        public Form1()
        {
            InitializeComponent();
            Booter = new Timer();
        }

        void Booter_Tick(object sender, EventArgs e)
        {
            ExecuteClose();
            ExecuteStart();

            bootCount++;
            bootLabel.Text = "Boots done: " + bootCount;
        }

        private void ExecuteClose()
        {
            LoginProcess.Kill();
            ChannelProcess.Kill();
        }

        private void ExecuteStart()
        {
            LoginProcess = Process.Start(LoginPath);
            ChannelProcess = Process.Start(ChannelPath);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            ExecuteStart();

            Booter.Enabled = true;
            Booter.Interval = Invertal;
            Booter.Tick += new EventHandler(Booter_Tick);
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            Booter.Enabled = false;
            Booter.Stop();
        }
    }
}
