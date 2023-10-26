namespace RazorEnhanced.UI
{
    partial class EnhancedLauncher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnhancedLauncher));
            this.openclientlocation = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.removeShard = new System.Windows.Forms.Button();
            this.addShard = new System.Windows.Forms.Button();
            this.shardlistCombobox = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cuoPathClick = new System.Windows.Forms.Button();
            this.cuoClientLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.osiEnc = new System.Windows.Forms.CheckBox();
            this.patchEnc = new System.Windows.Forms.CheckBox();
            this.portLabel = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.hostLabel = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.clientFolderLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.bNameCopy = new System.Windows.Forms.Button();
            this.clientPathLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkupdatebutton = new System.Windows.Forms.Button();
            this.quit = new System.Windows.Forms.Button();
            this.launch = new System.Windows.Forms.Button();
            this.launchCUO = new System.Windows.Forms.Button();
            this.m_Tip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // openclientlocation
            // 
            this.openclientlocation.DefaultExt = "exe";
            this.openclientlocation.FileName = "client.exe";
            this.openclientlocation.Filter = "Executable Files|Client.exe";
            this.openclientlocation.RestoreDirectory = true;
            this.openclientlocation.Title = "Select Client";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Shard:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.removeShard);
            this.groupBox1.Controls.Add(this.addShard);
            this.groupBox1.Controls.Add(this.shardlistCombobox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(373, 51);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Shard Selection";
            // 
            // removeShard
            // 
            this.removeShard.Location = new System.Drawing.Point(307, 19);
            this.removeShard.Name = "removeShard";
            this.removeShard.Size = new System.Drawing.Size(60, 21);
            this.removeShard.TabIndex = 3;
            this.removeShard.Text = "Remove";
            this.removeShard.UseVisualStyleBackColor = true;
            this.removeShard.Click += new System.EventHandler(this.RazorButton2_Click);
            // 
            // addShard
            // 
            this.addShard.Location = new System.Drawing.Point(242, 19);
            this.addShard.Name = "addShard";
            this.addShard.Size = new System.Drawing.Size(60, 21);
            this.addShard.TabIndex = 2;
            this.addShard.Text = "Add";
            this.addShard.UseVisualStyleBackColor = true;
            this.addShard.Click += new System.EventHandler(this.RazorButton1_Click);
            // 
            // shardlistCombobox
            // 
            this.shardlistCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shardlistCombobox.FormattingEnabled = true;
            this.shardlistCombobox.Location = new System.Drawing.Point(51, 19);
            this.shardlistCombobox.Name = "shardlistCombobox";
            this.shardlistCombobox.Size = new System.Drawing.Size(185, 21);
            this.shardlistCombobox.TabIndex = 1;
            this.shardlistCombobox.SelectedIndexChanged += new System.EventHandler(this.ShardlistCombobox_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cuoPathClick);
            this.groupBox2.Controls.Add(this.cuoClientLabel);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.osiEnc);
            this.groupBox2.Controls.Add(this.patchEnc);
            this.groupBox2.Controls.Add(this.portLabel);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.hostLabel);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.clientFolderLabel);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.bNameCopy);
            this.groupBox2.Controls.Add(this.clientPathLabel);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 69);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(373, 153);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Shard Config";
            // 
            // cuoPathClick
            // 
            this.cuoPathClick.BackgroundImage = global::Assistant.Properties.Resources.document_open_7;
            this.cuoPathClick.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.cuoPathClick.Location = new System.Drawing.Point(6, 67);
            this.cuoPathClick.Name = "cuoPathClick";
            this.cuoPathClick.Size = new System.Drawing.Size(20, 20);
            this.cuoPathClick.TabIndex = 30;
            this.cuoPathClick.UseVisualStyleBackColor = true;
            this.cuoPathClick.Click += new System.EventHandler(this.CuoClient_Click);
            // 
            // cuoClientLabel
            // 
            this.cuoClientLabel.ForeColor = System.Drawing.Color.SteelBlue;
            this.cuoClientLabel.Location = new System.Drawing.Point(118, 70);
            this.cuoClientLabel.Name = "cuoClientLabel";
            this.cuoClientLabel.Size = new System.Drawing.Size(249, 13);
            this.cuoClientLabel.TabIndex = 32;
            this.cuoClientLabel.Text = "Optional";
            this.cuoClientLabel.Click += new System.EventHandler(this.CuoClient_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 31;
            this.label4.Text = "CUO Client:";
            // 
            // osiEnc
            // 
            this.osiEnc.Location = new System.Drawing.Point(217, 125);
            this.osiEnc.Name = "osiEnc";
            this.osiEnc.Size = new System.Drawing.Size(140, 22);
            this.osiEnc.TabIndex = 51;
            this.osiEnc.Text = "Use OSI Encryption";
            this.osiEnc.CheckedChanged += new System.EventHandler(this.OsiEnc_CheckedChanged);
            // 
            // patchEnc
            // 
            this.patchEnc.BackColor = System.Drawing.SystemColors.Control;
            this.patchEnc.Location = new System.Drawing.Point(15, 124);
            this.patchEnc.Name = "patchEnc";
            this.patchEnc.Size = new System.Drawing.Size(140, 22);
            this.patchEnc.TabIndex = 50;
            this.patchEnc.Text = "Patch client encryption";
            this.patchEnc.UseVisualStyleBackColor = false;
            this.patchEnc.CheckedChanged += new System.EventHandler(this.PatchEncy_CheckedChanged);
            // 
            // portLabel
            // 
            this.portLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.portLabel.BackColor = System.Drawing.Color.White;
            this.portLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.portLabel.Location = new System.Drawing.Point(333, 101);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(35, 20);
            this.portLabel.TabIndex = 43;
            this.portLabel.Text = "2593";
            this.portLabel.Leave += new System.EventHandler(this.ServerportT_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(299, 101);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 42;
            this.label7.Text = "Port:";
            // 
            // hostLabel
            // 
            this.hostLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hostLabel.BackColor = System.Drawing.Color.White;
            this.hostLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hostLabel.Location = new System.Drawing.Point(95, 101);
            this.hostLabel.Name = "hostLabel";
            this.hostLabel.Size = new System.Drawing.Size(200, 20);
            this.hostLabel.TabIndex = 41;
            this.hostLabel.Leave += new System.EventHandler(this.ServeraddressT_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 40;
            this.label6.Text = "Server Address:";
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::Assistant.Properties.Resources.document_open_7;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.Location = new System.Drawing.Point(6, 43);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(20, 20);
            this.button1.TabIndex = 20;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // clientFolderLabel
            // 
            this.clientFolderLabel.ForeColor = System.Drawing.Color.SteelBlue;
            this.clientFolderLabel.Location = new System.Drawing.Point(118, 46);
            this.clientFolderLabel.Name = "clientFolderLabel";
            this.clientFolderLabel.Size = new System.Drawing.Size(249, 13);
            this.clientFolderLabel.TabIndex = 22;
            this.clientFolderLabel.Text = "Not Set";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "UO Folder:";
            // 
            // bNameCopy
            // 
            this.bNameCopy.BackgroundImage = global::Assistant.Properties.Resources.document_open_7;
            this.bNameCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bNameCopy.Location = new System.Drawing.Point(6, 20);
            this.bNameCopy.Name = "bNameCopy";
            this.bNameCopy.Size = new System.Drawing.Size(20, 20);
            this.bNameCopy.TabIndex = 10;
            this.bNameCopy.UseVisualStyleBackColor = true;
            this.bNameCopy.Click += new System.EventHandler(this.BNameCopy_Click);
            // 
            // clientPathLabel
            // 
            this.clientPathLabel.ForeColor = System.Drawing.Color.SteelBlue;
            this.clientPathLabel.Location = new System.Drawing.Point(118, 23);
            this.clientPathLabel.Name = "clientPathLabel";
            this.clientPathLabel.Size = new System.Drawing.Size(249, 13);
            this.clientPathLabel.TabIndex = 12;
            this.clientPathLabel.Text = "Not Set";
            this.m_Tip.SetToolTip(this.clientPathLabel, "Not Set");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Client Location:";
            // 
            // checkupdatebutton
            // 
            this.checkupdatebutton.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.checkupdatebutton.Location = new System.Drawing.Point(205, 229);
            this.checkupdatebutton.Name = "checkupdatebutton";
            this.checkupdatebutton.Size = new System.Drawing.Size(84, 21);
            this.checkupdatebutton.TabIndex = 102;
            this.checkupdatebutton.Text = "Check Update";
            this.checkupdatebutton.Click += new System.EventHandler(this.Checkupdatebutton_Click);
            // 
            // quit
            // 
            this.quit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.quit.Location = new System.Drawing.Point(295, 229);
            this.quit.Name = "quit";
            this.quit.Size = new System.Drawing.Size(84, 21);
            this.quit.TabIndex = 103;
            this.quit.Text = "Exit";
            this.quit.Click += new System.EventHandler(this.Quit_Click);
            // 
            // launch
            // 
            this.launch.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.launch.Location = new System.Drawing.Point(27, 229);
            this.launch.Name = "launch";
            this.launch.Size = new System.Drawing.Size(84, 21);
            this.launch.TabIndex = 100;
            this.launch.Text = "Launch";
            this.launch.Click += new System.EventHandler(this.Okay_Click);
            // 
            // launchCUO
            // 
            this.launchCUO.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.launchCUO.Enabled = false;
            this.launchCUO.Location = new System.Drawing.Point(116, 229);
            this.launchCUO.Name = "launchCUO";
            this.launchCUO.Size = new System.Drawing.Size(84, 21);
            this.launchCUO.TabIndex = 101;
            this.launchCUO.Text = "Launch CUO";
            this.launchCUO.Click += new System.EventHandler(this.LaunchCUO_Click);
            // 
            // EnhancedLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 259);
            this.Controls.Add(this.launchCUO);
            this.Controls.Add(this.checkupdatebutton);
            this.Controls.Add(this.quit);
            this.Controls.Add(this.launch);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EnhancedLauncher";
            this.Text = "Welcome to Razor Enhanced";
            this.Load += new System.EventHandler(this.EnhancedLauncher_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.ToolTip m_Tip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button removeShard;
        private System.Windows.Forms.Button addShard;
        private System.Windows.Forms.Label clientPathLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bNameCopy;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label clientFolderLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox portLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox hostLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox osiEnc;
        private System.Windows.Forms.CheckBox patchEnc;
        private System.Windows.Forms.Button quit;
        private System.Windows.Forms.Button launch;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.ComboBox shardlistCombobox;
        private System.Windows.Forms.OpenFileDialog openclientlocation;
        private System.Windows.Forms.Button checkupdatebutton;
        private System.Windows.Forms.Button cuoPathClick;
        private System.Windows.Forms.Label cuoClientLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button launchCUO;
    }
}
