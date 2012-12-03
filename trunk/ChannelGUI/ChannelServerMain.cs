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
using Entities;


namespace ChannelGUI
{
    public partial class ChannelServerMain : Form
    {
        private ChannelServer channelServer;

        public ChannelServerMain()
        {
            InitializeComponent();

            channelServer = new ChannelServer(Properties.Settings.Default.ChannelServerIP,
                Properties.Settings.Default.ChannelServerPort, Properties.Settings.Default.ExpRate,
                Properties.Settings.Default.ItemDropChance, Properties.Settings.Default.MoneyDropChance,
                Properties.Settings.Default.StoneDropChance, Properties.Settings.Default.BookDropChance,
                Properties.Settings.Default.PillDropChance, Properties.Settings.Default.StoneCombineChance);

            channelServer.ClientConnected += new EventHandler<ClientConnectionEventArgs>(channelServer_ClientConnected);
            channelServer.ClientExceptionOccoured += new EventHandler<ClientExceptionEventArgs>(channelServer_ClientExceptionOccoured);
            channelServer.ClientUnknownPacket += new EventHandler<UnknownPacketEventArgs>(channelServer_ClientUnknownPacket);
            channelServer.ClientLogPacket += new EventHandler<LogPacketEventArgs>(channelServer_ClientLogPacket);
            channelServer.ClientConnectionChanged += new EventHandler<PlayerConnectionChangedEventArgs>(channelServer_ClientConnectionChanged);
        }

        void channelServer_ClientLogPacket(object sender, LogPacketEventArgs e)
        {
            LogPacket(e.LogPacket.GetBuffer());
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
            Log("Client Connected: " + e.Client.IPAddress);
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

        private void LogPacket(byte[] buffer)
        {
            if (RichTextBoxLog.InvokeRequired)
            {
                RichTextBoxLog.Invoke(new LogBufferDel(LogPacket), new object[] { buffer });
            }
            else
            {
                RichTextBoxLog.AppendText("Log Packet: ");
                for (int i = 0; i < buffer.Length; i++)
                {
                    RichTextBoxLog.AppendText(buffer[i].ToString("X2") + " ");
                }
                RichTextBoxLog.AppendText("\n");
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
                if (RichTextBoxLog.Text.Length >= 10000)
                    RichTextBoxLog.Clear();
                RichTextBoxLog.AppendText("Unknown Packet: ");
                for (int i = 0; i < buffer.Length; i++)
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

        private void ChannelServerMain_Load(object sender, EventArgs e)
        {
            ButtonStart_Click(null, null);
        }

        private void TextBoxChat_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                ButtonSendChat_Click(ButtonSendChat, new EventArgs());
        }

        private void ButtonSendChat_Click(object sender, EventArgs e)
        {
            if (TextBoxChat.Text != null && TextBoxChat.Text != string.Empty)
            {
                channelServer.SendChatToAll(TextBoxChat.Text);
                TextBoxChat.Text = string.Empty;
            }
        }


        #region Player Online Stuff

        delegate void delConnectionChanged(object sender, PlayerConnectionChangedEventArgs e);

        void channelServer_ClientConnectionChanged(object sender, PlayerConnectionChangedEventArgs e)
        {
            Character ch = e.Character;
            bool InGame = e.InGame;

            if (playerTreeView.InvokeRequired)
                playerTreeView.Invoke(new delConnectionChanged(channelServer_ClientConnectionChanged), new object[] { sender, e });
            else
            {
                bool exist = false;
                foreach (TreeNode node in playerTreeView.Nodes)
                {
                    if ((int)node.Tag == ch.CharacterId)
                    {
                        exist = true;
                    }
                }

                if (InGame)
                {
                    if (!exist)
                        AddPlayerTreeViewItem(ch);
                    else
                        UpdatePlayerTreeViewItem(ch);
                }
                else
                    RemovePlayerTreeViewItem(ch);

                totalPlayerLabel.Text = string.Format("Total Online Players: {0}", playerTreeView.Nodes.Count);
            }
        }

        private void AddPlayerTreeViewItem(Character ch)
        {
            string map = String.Format("Map: {0}", (MapName)ch.MapId);
            TreeNode nodeMap = new TreeNode(map);
            nodeMap.Tag = "Map";
            string posx = String.Format("X: {0}", ch.Position.X);
            TreeNode nodeX = new TreeNode(posx);
            nodeX.Tag = "X";
            string posy = String.Format("Y: {0}", ch.Position.Y);
            TreeNode nodeY = new TreeNode(posy);
            nodeY.Tag = "Y";

            TreeNode mapInfo = new TreeNode("Map Info", new TreeNode[] { nodeMap, nodeX, nodeY });
            mapInfo.Tag = "Info";

            string charId = String.Format("CharacterID: {0}", ch.CharacterId);
            TreeNode nodeID = new TreeNode(charId);
            nodeID.Tag = "ID";
            string accountId = String.Format("AccountID: {0}", ch.AccountId);
            TreeNode nodeAccountID = new TreeNode(accountId);
            nodeAccountID.Tag = "AccountID";
            string level = String.Format("Level: {0}", ch.Level);
            TreeNode nodeLevel = new TreeNode(level);
            nodeLevel.Tag = "Level";

            TreeNode character = new TreeNode(ch.Name, new TreeNode[] { nodeID, nodeAccountID, nodeLevel, mapInfo });
            character.Tag = ch.CharacterId; // this is only so we can update etc..

            playerTreeView.Nodes.Add(character);
        }

        private void RemovePlayerTreeViewItem(Character ch)
        {
            foreach (TreeNode node in playerTreeView.Nodes)
            {
                if ((int)node.Tag == ch.CharacterId)
                {
                    playerTreeView.Nodes.Remove(node);
                }
            }
        }

        private void UpdatePlayerTreeViewItem(Character ch)
        {
            TreeNode found = null;

            foreach (TreeNode node in playerTreeView.Nodes)
            {
                if ((int)node.Tag == ch.CharacterId)
                {
                    found = node;
                }
            }

            if (found != null)
            {
                foreach (TreeNode node in found.Nodes)
                {
                    string tag = (string)node.Tag;
                    switch (tag)
                    {
                        case "ID":
                            string charId = String.Format("CharacterID: {0}", ch.CharacterId);
                            node.Text = charId;
                            break;

                        case "AccountID":
                            string accountId = String.Format("AccountID: {0}", ch.AccountId);
                            node.Text = accountId;
                            break;

                        case "Level":
                            string level = String.Format("Level: {0}", ch.Level);
                            node.Text = level;
                            break;

                        case "Info":
                            foreach (TreeNode nod in node.Nodes)
                            {
                                tag = (string)nod.Tag;
                                switch (tag)
                                {
                                    case "Map":
                                        string map = String.Format("Map: {0}", (MapName)ch.MapId);
                                        nod.Text = map;
                                        break;

                                    case "X":
                                        string posx = String.Format("X: {0}", ch.Position.X);
                                        nod.Text = posx;
                                        break;

                                    case "Y":
                                        string posy = String.Format("Y: {0}", ch.Position.Y);
                                        nod.Text = posy;
                                        break;
                                }
                            }
                            break;
                    }
                }
            }
        }

        private void kickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            channelServer.KickPlayer(playerTreeView.SelectedNode.Text);
        }

        #endregion

        private void playerTreeView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                playerTreeView.SelectedNode = playerTreeView.GetNodeAt(e.X, e.Y);

                if (playerTreeView.SelectedNode != null)
                {
                    playerTreeView.ContextMenuStrip = PlayerControlMenuStrip;
                    playerTreeView.ContextMenuStrip.Show(playerTreeView, new Point(e.X, e.Y));
                }
                else
                    playerTreeView.ContextMenuStrip = null;
            }
        }

        private void playerTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}
