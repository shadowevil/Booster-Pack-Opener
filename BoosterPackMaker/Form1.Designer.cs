namespace BoosterPackMaker
{
    partial class Form1
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmQuickSave = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblConnection = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.txtName_short = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtRelease = new System.Windows.Forms.TextBox();
            this.txtCardNum = new System.Windows.Forms.TextBox();
            this.txtRareChance = new System.Windows.Forms.TextBox();
            this.txtSuperRareChance = new System.Windows.Forms.TextBox();
            this.txtUltraRareChance = new System.Windows.Forms.TextBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.cboActive = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(330, 282);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(213, 277);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(0, 55);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(324, 504);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.TabStop = false;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.tsmQuickSave,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1180, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.tsmSave,
            this.tsmSaveAs,
            this.toolStripSeparator2,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(111, 6);
            // 
            // tsmSave
            // 
            this.tsmSave.Enabled = false;
            this.tsmSave.Name = "tsmSave";
            this.tsmSave.Size = new System.Drawing.Size(114, 22);
            this.tsmSave.Text = "Save";
            // 
            // tsmSaveAs
            // 
            this.tsmSaveAs.Enabled = false;
            this.tsmSaveAs.Name = "tsmSaveAs";
            this.tsmSaveAs.Size = new System.Drawing.Size(114, 22);
            this.tsmSaveAs.Text = "Save As";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(111, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // tsmQuickSave
            // 
            this.tsmQuickSave.Enabled = false;
            this.tsmQuickSave.Name = "tsmQuickSave";
            this.tsmQuickSave.Size = new System.Drawing.Size(77, 20);
            this.tsmQuickSave.Text = "Quick Save";
            this.tsmQuickSave.Click += new System.EventHandler(this.tsmQuickSave_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // txtSearch
            // 
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Enabled = false;
            this.txtSearch.Location = new System.Drawing.Point(0, 33);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(324, 20);
            this.txtSearch.TabIndex = 100;
            this.txtSearch.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.lblConnection,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel3,
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 562);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1180, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel2.Text = " ";
            // 
            // lblConnection
            // 
            this.lblConnection.BackColor = System.Drawing.Color.IndianRed;
            this.lblConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(79, 17);
            this.lblConnection.Text = "Disconnected";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(1076, 17);
            this.toolStripStatusLabel3.Spring = true;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(548, 33);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(632, 526);
            this.treeView1.TabIndex = 7;
            this.treeView1.TabStop = false;
            // 
            // txtName_short
            // 
            this.txtName_short.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName_short.Enabled = false;
            this.txtName_short.Location = new System.Drawing.Point(329, 57);
            this.txtName_short.MaxLength = 4;
            this.txtName_short.Name = "txtName_short";
            this.txtName_short.Size = new System.Drawing.Size(213, 20);
            this.txtName_short.TabIndex = 0;
            // 
            // txtName
            // 
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.Enabled = false;
            this.txtName.Location = new System.Drawing.Point(329, 81);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(213, 20);
            this.txtName.TabIndex = 1;
            // 
            // txtDescription
            // 
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescription.Enabled = false;
            this.txtDescription.Location = new System.Drawing.Point(330, 105);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(213, 20);
            this.txtDescription.TabIndex = 2;
            // 
            // txtRelease
            // 
            this.txtRelease.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRelease.Enabled = false;
            this.txtRelease.Location = new System.Drawing.Point(329, 129);
            this.txtRelease.MaxLength = 4;
            this.txtRelease.Name = "txtRelease";
            this.txtRelease.Size = new System.Drawing.Size(213, 20);
            this.txtRelease.TabIndex = 3;
            // 
            // txtCardNum
            // 
            this.txtCardNum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCardNum.Enabled = false;
            this.txtCardNum.Location = new System.Drawing.Point(329, 153);
            this.txtCardNum.MaxLength = 2;
            this.txtCardNum.Name = "txtCardNum";
            this.txtCardNum.Size = new System.Drawing.Size(213, 20);
            this.txtCardNum.TabIndex = 4;
            // 
            // txtRareChance
            // 
            this.txtRareChance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRareChance.Enabled = false;
            this.txtRareChance.Location = new System.Drawing.Point(329, 177);
            this.txtRareChance.MaxLength = 3;
            this.txtRareChance.Name = "txtRareChance";
            this.txtRareChance.Size = new System.Drawing.Size(213, 20);
            this.txtRareChance.TabIndex = 5;
            // 
            // txtSuperRareChance
            // 
            this.txtSuperRareChance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSuperRareChance.Enabled = false;
            this.txtSuperRareChance.Location = new System.Drawing.Point(329, 201);
            this.txtSuperRareChance.MaxLength = 3;
            this.txtSuperRareChance.Name = "txtSuperRareChance";
            this.txtSuperRareChance.Size = new System.Drawing.Size(213, 20);
            this.txtSuperRareChance.TabIndex = 6;
            // 
            // txtUltraRareChance
            // 
            this.txtUltraRareChance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUltraRareChance.Enabled = false;
            this.txtUltraRareChance.Location = new System.Drawing.Point(329, 225);
            this.txtUltraRareChance.MaxLength = 3;
            this.txtUltraRareChance.Name = "txtUltraRareChance";
            this.txtUltraRareChance.Size = new System.Drawing.Size(213, 20);
            this.txtUltraRareChance.TabIndex = 7;
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.btnNew.Enabled = false;
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNew.Location = new System.Drawing.Point(329, 248);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(213, 31);
            this.btnNew.TabIndex = 101;
            this.btnNew.Text = "Create New Node";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // cboActive
            // 
            this.cboActive.Enabled = false;
            this.cboActive.FormattingEnabled = true;
            this.cboActive.Location = new System.Drawing.Point(330, 32);
            this.cboActive.Name = "cboActive";
            this.cboActive.Size = new System.Drawing.Size(212, 21);
            this.cboActive.TabIndex = 102;
            this.cboActive.SelectedIndexChanged += new System.EventHandler(this.cboActive_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 584);
            this.Controls.Add(this.cboActive);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.txtUltraRareChance);
            this.Controls.Add(this.txtSuperRareChance);
            this.Controls.Add(this.txtRareChance);
            this.Controls.Add(this.txtCardNum);
            this.Controls.Add(this.txtRelease);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtName_short);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.treeView1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Card Window";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmSave;
        private System.Windows.Forms.ToolStripMenuItem tsmSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmQuickSave;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblConnection;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TextBox txtName_short;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtRelease;
        private System.Windows.Forms.TextBox txtCardNum;
        private System.Windows.Forms.TextBox txtRareChance;
        private System.Windows.Forms.TextBox txtSuperRareChance;
        private System.Windows.Forms.TextBox txtUltraRareChance;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.ComboBox cboActive;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    }
}