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


namespace ChannelGUI
{
    public partial class ChannelServerMain : Form
    {
        private ChannelServer channelServer;

        public ChannelServerMain()
        {
            InitializeComponent();

            channelServer = new ChannelServer(Properties.Settings.Default.ChannelServerIP,
                Properties.Settings.Default.ChannelServerPort);

            channelServer.ClientConnected += new EventHandler<ClientConnectionEventArgs>(channelServer_ClientConnected);
            channelServer.ClientExceptionOccoured += new EventHandler<ClientExceptionEventArgs>(channelServer_ClientExceptionOccoured);
            channelServer.ClientUnknownPacket += new EventHandler<UnknownPacketEventArgs>(channelServer_ClientUnknownPacket);
        }

        void channelServer_ClientUnknownPacket(object sender, UnknownPacketEventArgs e)
        {
            LogBuffer(e.UnknownPacket.GetBuffer());
        }

        void channelServer_ClientExceptionOccoured(object sender, ClientExceptionEventArgs e)
        {
            Log(e.ReferencedException.ToString());
        }

        void channelServer_ClientConnected(object sender, ClientConnectionEventArgs e)
        {
            Log("Client Connected: " +  e.Client.IPAddress);
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            channelServer.BeginListen();
            ButtonStart.Enabled = false;
            ButtonStop.Enabled = true;
            Log("Channel server started.");
        }


        delegate void LogDel(string text);
        delegate void LogBufferDel(byte[] buffer);

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

        private void LogBuffer(byte[] buffer)
        {
            if (RichTextBoxLog.InvokeRequired)
            {
                RichTextBoxLog.Invoke(new LogBufferDel(LogBuffer), new object[] { buffer });
            }
            else
            {
                RichTextBoxLog.AppendText("Unknown Packet: ");
                for(int i = 0; i < buffer.Length; i++)
                {
                    RichTextBoxLog.AppendText(buffer[i].ToString("X2") + " ");
                }
                RichTextBoxLog.AppendText("\n");
            }
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            channelServer.Dispose();
            ButtonStop.Enabled = false;
            ButtonStart.Enabled = true;
            Log("Channel server stopped.");
        }
    }
}
