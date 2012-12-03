namespace GCFLauncher
{
    partial class LauncherMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LauncherMain));
            this.AccountBox = new System.Windows.Forms.MaskedTextBox();
            this.LabelAccount = new System.Windows.Forms.Label();
            this.PasswordBox = new System.Windows.Forms.MaskedTextBox();
            this.LabelPassword = new System.Windows.Forms.Label();
            this.btnLaunch = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AccountBox
            // 
            this.AccountBox.Enabled = false;
            this.AccountBox.Location = new System.Drawing.Point(438, 230);
            this.AccountBox.Name = "AccountBox";
            this.AccountBox.Size = new System.Drawing.Size(137, 20);
            this.AccountBox.TabIndex = 0;
            this.AccountBox.TextChanged += new System.EventHandler(this.AccountBox_TextChanged);
            // 
            // LabelAccount
            // 
            this.LabelAccount.AutoSize = true;
            this.LabelAccount.BackColor = System.Drawing.Color.Transparent;
            this.LabelAccount.ForeColor = System.Drawing.Color.White;
            this.LabelAccount.Location = new System.Drawing.Point(435, 214);
            this.LabelAccount.Name = "LabelAccount";
            this.LabelAccount.Size = new System.Drawing.Size(47, 13);
            this.LabelAccount.TabIndex = 1;
            this.LabelAccount.Text = "Account";
            // 
            // PasswordBox
            // 
            this.PasswordBox.Enabled = false;
            this.PasswordBox.Location = new System.Drawing.Point(438, 269);
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.PasswordChar = '*';
            this.PasswordBox.Size = new System.Drawing.Size(137, 20);
            this.PasswordBox.TabIndex = 2;
            this.PasswordBox.TextChanged += new System.EventHandler(this.PasswordBox_TextChanged);
            // 
            // LabelPassword
            // 
            this.LabelPassword.AutoSize = true;
            this.LabelPassword.BackColor = System.Drawing.Color.Transparent;
            this.LabelPassword.ForeColor = System.Drawing.Color.White;
            this.LabelPassword.Location = new System.Drawing.Point(435, 253);
            this.LabelPassword.Name = "LabelPassword";
            this.LabelPassword.Size = new System.Drawing.Size(53, 13);
            this.LabelPassword.TabIndex = 3;
            this.LabelPassword.Text = "Password";
            // 
            // btnLaunch
            // 
            this.btnLaunch.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnLaunch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnLaunch.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnLaunch.Location = new System.Drawing.Point(463, 241);
            this.btnLaunch.Name = "btnLaunch";
            this.btnLaunch.Size = new System.Drawing.Size(89, 36);
            this.btnLaunch.TabIndex = 5;
            this.btnLaunch.TabStop = false;
            this.btnLaunch.Text = "GameStart";
            this.btnLaunch.UseVisualStyleBackColor = false;
            this.btnLaunch.Visible = false;
            this.btnLaunch.Click += new System.EventHandler(this.btnLaunch_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.Transparent;
            this.btnConnect.Enabled = false;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnConnect.ForeColor = System.Drawing.Color.Black;
            this.btnConnect.Location = new System.Drawing.Point(517, 182);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(65, 26);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // LauncherMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::GCFLauncher.Properties.Resources.test;
            this.ClientSize = new System.Drawing.Size(584, 316);
            this.Controls.Add(this.btnLaunch);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.LabelPassword);
            this.Controls.Add(this.PasswordBox);
            this.Controls.Add(this.LabelAccount);
            this.Controls.Add(this.AccountBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 350);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 350);
            this.Name = "LauncherMain";
            this.Text = "XiahGCF Launcher";
            this.Load += new System.EventHandler(this.LauncherMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox AccountBox;
        private System.Windows.Forms.Label LabelAccount;
        private System.Windows.Forms.MaskedTextBox PasswordBox;
        private System.Windows.Forms.Label LabelPassword;
        private System.Windows.Forms.Button btnLaunch;
        private System.Windows.Forms.Button btnConnect;
    }
}

