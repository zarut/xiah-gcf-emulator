namespace ChannelGUI
{
    partial class ChannelServerMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ButtonStart = new System.Windows.Forms.Button();
            this.ButtonStop = new System.Windows.Forms.Button();
            this.RichTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.LabelChat = new System.Windows.Forms.Label();
            this.TextBoxChat = new System.Windows.Forms.TextBox();
            this.ButtonSendChat = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.totalPlayerLabel = new System.Windows.Forms.Label();
            this.playerTreeView = new System.Windows.Forms.TreeView();
            this.PlayerControlMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.kickToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.PlayerControlMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonStart
            // 
            this.ButtonStart.Location = new System.Drawing.Point(6, 400);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(110, 23);
            this.ButtonStart.TabIndex = 0;
            this.ButtonStart.Text = "Start ChannelServer";
            this.ButtonStart.UseVisualStyleBackColor = true;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // ButtonStop
            // 
            this.ButtonStop.Location = new System.Drawing.Point(122, 400);
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.Size = new System.Drawing.Size(110, 23);
            this.ButtonStop.TabIndex = 1;
            this.ButtonStop.Text = "Stop ChannelServer";
            this.ButtonStop.UseVisualStyleBackColor = true;
            this.ButtonStop.Click += new System.EventHandler(this.ButtonStop_Click);
            // 
            // RichTextBoxLog
            // 
            this.RichTextBoxLog.Location = new System.Drawing.Point(10, 6);
            this.RichTextBoxLog.Name = "RichTextBoxLog";
            this.RichTextBoxLog.Size = new System.Drawing.Size(521, 362);
            this.RichTextBoxLog.TabIndex = 2;
            this.RichTextBoxLog.Text = "";
            // 
            // LabelChat
            // 
            this.LabelChat.AutoSize = true;
            this.LabelChat.Location = new System.Drawing.Point(7, 377);
            this.LabelChat.Name = "LabelChat";
            this.LabelChat.Size = new System.Drawing.Size(65, 13);
            this.LabelChat.TabIndex = 3;
            this.LabelChat.Text = "Send To All:";
            // 
            // TextBoxChat
            // 
            this.TextBoxChat.Location = new System.Drawing.Point(71, 374);
            this.TextBoxChat.Name = "TextBoxChat";
            this.TextBoxChat.Size = new System.Drawing.Size(391, 20);
            this.TextBoxChat.TabIndex = 4;
            this.TextBoxChat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxChat_KeyPress);
            // 
            // ButtonSendChat
            // 
            this.ButtonSendChat.Location = new System.Drawing.Point(468, 372);
            this.ButtonSendChat.Name = "ButtonSendChat";
            this.ButtonSendChat.Size = new System.Drawing.Size(63, 23);
            this.ButtonSendChat.TabIndex = 5;
            this.ButtonSendChat.Text = "Send";
            this.ButtonSendChat.UseVisualStyleBackColor = true;
            this.ButtonSendChat.Click += new System.EventHandler(this.ButtonSendChat_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(1, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(549, 456);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.RichTextBoxLog);
            this.tabPage1.Controls.Add(this.ButtonStop);
            this.tabPage1.Controls.Add(this.ButtonSendChat);
            this.tabPage1.Controls.Add(this.ButtonStart);
            this.tabPage1.Controls.Add(this.TextBoxChat);
            this.tabPage1.Controls.Add(this.LabelChat);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(541, 430);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main Menu";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.totalPlayerLabel);
            this.tabPage2.Controls.Add(this.playerTreeView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(541, 430);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Player Control";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // totalPlayerLabel
            // 
            this.totalPlayerLabel.AutoSize = true;
            this.totalPlayerLabel.Location = new System.Drawing.Point(7, 11);
            this.totalPlayerLabel.Name = "totalPlayerLabel";
            this.totalPlayerLabel.Size = new System.Drawing.Size(113, 13);
            this.totalPlayerLabel.TabIndex = 1;
            this.totalPlayerLabel.Text = "Total Players Online: 0";
            // 
            // playerTreeView
            // 
            this.playerTreeView.Location = new System.Drawing.Point(7, 27);
            this.playerTreeView.Name = "playerTreeView";
            this.playerTreeView.Size = new System.Drawing.Size(528, 397);
            this.playerTreeView.TabIndex = 0;
            this.playerTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.playerTreeView_AfterSelect);
            this.playerTreeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.playerTreeView_MouseUp);
            // 
            // PlayerControlMenuStrip
            // 
            this.PlayerControlMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kickToolStripMenuItem});
            this.PlayerControlMenuStrip.Name = "PlayerControlMenuStrip";
            this.PlayerControlMenuStrip.Size = new System.Drawing.Size(97, 26);
            // 
            // kickToolStripMenuItem
            // 
            this.kickToolStripMenuItem.Name = "kickToolStripMenuItem";
            this.kickToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.kickToolStripMenuItem.Text = "Kick";
            this.kickToolStripMenuItem.Click += new System.EventHandler(this.kickToolStripMenuItem_Click);
            // 
            // ChannelServerMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 458);
            this.Controls.Add(this.tabControl1);
            this.Name = "ChannelServerMain";
            this.Text = "Main Window";
            this.Load += new System.EventHandler(this.ChannelServerMain_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.PlayerControlMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.Button ButtonStop;
        private System.Windows.Forms.RichTextBox RichTextBoxLog;
        private System.Windows.Forms.Label LabelChat;
        private System.Windows.Forms.TextBox TextBoxChat;
        private System.Windows.Forms.Button ButtonSendChat;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label totalPlayerLabel;
        private System.Windows.Forms.TreeView playerTreeView;
        private System.Windows.Forms.ContextMenuStrip PlayerControlMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem kickToolStripMenuItem;
    }
}

