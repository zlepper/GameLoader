﻿namespace GameLoader
{
    partial class GameLoaderForm
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label fastFolderLabel;
            System.Windows.Forms.Label label5;
            this.folderGridView = new System.Windows.Forms.DataGridView();
            this.newGamePathTextBox = new System.Windows.Forms.TextBox();
            this.addNewGameButton = new System.Windows.Forms.Button();
            this.newGameName = new System.Windows.Forms.TextBox();
            this.gameDataGroupbox = new System.Windows.Forms.GroupBox();
            this.activateGameButton = new System.Windows.Forms.Button();
            this.saveChangesButton = new System.Windows.Forms.Button();
            this.pathEditTextBox = new System.Windows.Forms.TextBox();
            this.nameEditTextBox = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusStipProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.statusToolStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.FileProgressStatusStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.fastFolderTextBox = new System.Windows.Forms.TextBox();
            this.saveFastFolderButton = new System.Windows.Forms.Button();
            this.AddAutoDiscoveryTextBox = new System.Windows.Forms.TextBox();
            this.AddAutodiscoveryFolderButton = new System.Windows.Forms.Button();
            this.BrowseForAutoDiscoveryFolderButton = new System.Windows.Forms.Button();
            this.BrowseForGamePath = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.resetApplicationStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DiskSpaceLeftLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            fastFolderLabel = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.folderGridView)).BeginInit();
            this.gameDataGroupbox.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(13, 437);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(66, 13);
            label1.TabIndex = 4;
            label1.Text = "Game Name";
            // 
            // label2
            // 
            label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(150, 437);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(60, 13);
            label2.TabIndex = 5;
            label2.Text = "Game Path";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(6, 20);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(35, 13);
            label3.TabIndex = 2;
            label3.Text = "Name";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(6, 59);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(29, 13);
            label4.TabIndex = 2;
            label4.Text = "Path";
            // 
            // fastFolderLabel
            // 
            fastFolderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            fastFolderLabel.AutoSize = true;
            fastFolderLabel.Location = new System.Drawing.Point(13, 540);
            fastFolderLabel.Name = "fastFolderLabel";
            fastFolderLabel.Size = new System.Drawing.Size(59, 13);
            fastFolderLabel.TabIndex = 8;
            fastFolderLabel.Text = "Fast Folder";
            // 
            // label5
            // 
            label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(13, 482);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(128, 13);
            label5.TabIndex = 4;
            label5.Text = "Add Auto-discovery folder";
            // 
            // folderGridView
            // 
            this.folderGridView.AllowUserToAddRows = false;
            this.folderGridView.AllowUserToOrderColumns = true;
            this.folderGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.folderGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.folderGridView.Location = new System.Drawing.Point(13, 27);
            this.folderGridView.MultiSelect = false;
            this.folderGridView.Name = "folderGridView";
            this.folderGridView.ReadOnly = true;
            this.folderGridView.Size = new System.Drawing.Size(544, 407);
            this.folderGridView.TabIndex = 0;
            this.folderGridView.CurrentCellChanged += new System.EventHandler(this.folderGridView_CurrentCellChanged);
            // 
            // newGamePathTextBox
            // 
            this.newGamePathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newGamePathTextBox.Location = new System.Drawing.Point(150, 456);
            this.newGamePathTextBox.Name = "newGamePathTextBox";
            this.newGamePathTextBox.Size = new System.Drawing.Size(398, 20);
            this.newGamePathTextBox.TabIndex = 1;
            this.newGamePathTextBox.TextChanged += new System.EventHandler(this.newGamePathTextBox_TextChanged);
            // 
            // addNewGameButton
            // 
            this.addNewGameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addNewGameButton.Location = new System.Drawing.Point(635, 454);
            this.addNewGameButton.Name = "addNewGameButton";
            this.addNewGameButton.Size = new System.Drawing.Size(75, 23);
            this.addNewGameButton.TabIndex = 2;
            this.addNewGameButton.Text = "Add Game";
            this.addNewGameButton.UseVisualStyleBackColor = true;
            this.addNewGameButton.Click += new System.EventHandler(this.addNewGameButton_Click);
            // 
            // newGameName
            // 
            this.newGameName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.newGameName.Location = new System.Drawing.Point(13, 456);
            this.newGameName.Name = "newGameName";
            this.newGameName.Size = new System.Drawing.Size(131, 20);
            this.newGameName.TabIndex = 3;
            // 
            // gameDataGroupbox
            // 
            this.gameDataGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gameDataGroupbox.Controls.Add(this.activateGameButton);
            this.gameDataGroupbox.Controls.Add(this.saveChangesButton);
            this.gameDataGroupbox.Controls.Add(label4);
            this.gameDataGroupbox.Controls.Add(label3);
            this.gameDataGroupbox.Controls.Add(this.pathEditTextBox);
            this.gameDataGroupbox.Controls.Add(this.nameEditTextBox);
            this.gameDataGroupbox.Enabled = false;
            this.gameDataGroupbox.Location = new System.Drawing.Point(563, 27);
            this.gameDataGroupbox.Name = "gameDataGroupbox";
            this.gameDataGroupbox.Size = new System.Drawing.Size(146, 406);
            this.gameDataGroupbox.TabIndex = 6;
            this.gameDataGroupbox.TabStop = false;
            this.gameDataGroupbox.Text = "Game Data";
            // 
            // activateGameButton
            // 
            this.activateGameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.activateGameButton.Location = new System.Drawing.Point(9, 377);
            this.activateGameButton.Name = "activateGameButton";
            this.activateGameButton.Size = new System.Drawing.Size(131, 23);
            this.activateGameButton.TabIndex = 4;
            this.activateGameButton.Text = "Activate";
            this.activateGameButton.UseVisualStyleBackColor = true;
            this.activateGameButton.Click += new System.EventHandler(this.activateGameButton_Click);
            // 
            // saveChangesButton
            // 
            this.saveChangesButton.Location = new System.Drawing.Point(9, 102);
            this.saveChangesButton.Name = "saveChangesButton";
            this.saveChangesButton.Size = new System.Drawing.Size(131, 23);
            this.saveChangesButton.TabIndex = 3;
            this.saveChangesButton.Text = "Save Changes";
            this.saveChangesButton.UseVisualStyleBackColor = true;
            this.saveChangesButton.Click += new System.EventHandler(this.saveChangesButton_Click);
            // 
            // pathEditTextBox
            // 
            this.pathEditTextBox.Location = new System.Drawing.Point(9, 75);
            this.pathEditTextBox.Name = "pathEditTextBox";
            this.pathEditTextBox.Size = new System.Drawing.Size(131, 20);
            this.pathEditTextBox.TabIndex = 1;
            // 
            // nameEditTextBox
            // 
            this.nameEditTextBox.Location = new System.Drawing.Point(9, 36);
            this.nameEditTextBox.Name = "nameEditTextBox";
            this.nameEditTextBox.Size = new System.Drawing.Size(131, 20);
            this.nameEditTextBox.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusStipProgressBar,
            this.statusToolStripLabel,
            this.FileProgressStatusStripLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 560);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(722, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusStipProgressBar
            // 
            this.statusStipProgressBar.Name = "statusStipProgressBar";
            this.statusStipProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // statusToolStripLabel
            // 
            this.statusToolStripLabel.Name = "statusToolStripLabel";
            this.statusToolStripLabel.Size = new System.Drawing.Size(42, 17);
            this.statusToolStripLabel.Text = "Ready!";
            // 
            // FileProgressStatusStripLabel
            // 
            this.FileProgressStatusStripLabel.Name = "FileProgressStatusStripLabel";
            this.FileProgressStatusStripLabel.Size = new System.Drawing.Size(117, 17);
            this.FileProgressStatusStripLabel.Text = "PLACEHOLDER TEXT";
            // 
            // fastFolderTextBox
            // 
            this.fastFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fastFolderTextBox.Location = new System.Drawing.Point(78, 537);
            this.fastFolderTextBox.Name = "fastFolderTextBox";
            this.fastFolderTextBox.Size = new System.Drawing.Size(470, 20);
            this.fastFolderTextBox.TabIndex = 9;
            this.fastFolderTextBox.TextChanged += new System.EventHandler(this.fastFolderTextBox_TextChanged);
            // 
            // saveFastFolderButton
            // 
            this.saveFastFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveFastFolderButton.Location = new System.Drawing.Point(554, 535);
            this.saveFastFolderButton.Name = "saveFastFolderButton";
            this.saveFastFolderButton.Size = new System.Drawing.Size(75, 23);
            this.saveFastFolderButton.TabIndex = 10;
            this.saveFastFolderButton.Text = "Save";
            this.saveFastFolderButton.UseVisualStyleBackColor = true;
            this.saveFastFolderButton.Click += new System.EventHandler(this.saveFastFolderButton_Click);
            // 
            // AddAutoDiscoveryTextBox
            // 
            this.AddAutoDiscoveryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddAutoDiscoveryTextBox.Location = new System.Drawing.Point(12, 498);
            this.AddAutoDiscoveryTextBox.Name = "AddAutoDiscoveryTextBox";
            this.AddAutoDiscoveryTextBox.Size = new System.Drawing.Size(536, 20);
            this.AddAutoDiscoveryTextBox.TabIndex = 11;
            // 
            // AddAutodiscoveryFolderButton
            // 
            this.AddAutodiscoveryFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddAutodiscoveryFolderButton.Location = new System.Drawing.Point(635, 495);
            this.AddAutodiscoveryFolderButton.Name = "AddAutodiscoveryFolderButton";
            this.AddAutodiscoveryFolderButton.Size = new System.Drawing.Size(75, 23);
            this.AddAutodiscoveryFolderButton.TabIndex = 12;
            this.AddAutodiscoveryFolderButton.Text = "Add";
            this.AddAutodiscoveryFolderButton.UseVisualStyleBackColor = true;
            this.AddAutodiscoveryFolderButton.Click += new System.EventHandler(this.AddAutodiscoveryFolderButton_Click);
            // 
            // BrowseForAutoDiscoveryFolderButton
            // 
            this.BrowseForAutoDiscoveryFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseForAutoDiscoveryFolderButton.Location = new System.Drawing.Point(554, 496);
            this.BrowseForAutoDiscoveryFolderButton.Name = "BrowseForAutoDiscoveryFolderButton";
            this.BrowseForAutoDiscoveryFolderButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseForAutoDiscoveryFolderButton.TabIndex = 13;
            this.BrowseForAutoDiscoveryFolderButton.Text = "Browse";
            this.BrowseForAutoDiscoveryFolderButton.UseVisualStyleBackColor = true;
            this.BrowseForAutoDiscoveryFolderButton.Click += new System.EventHandler(this.BrowseForAutoDiscoveryFolderButton_Click);
            // 
            // BrowseForGamePath
            // 
            this.BrowseForGamePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseForGamePath.Location = new System.Drawing.Point(554, 454);
            this.BrowseForGamePath.Name = "BrowseForGamePath";
            this.BrowseForGamePath.Size = new System.Drawing.Size(75, 23);
            this.BrowseForGamePath.TabIndex = 14;
            this.BrowseForGamePath.Text = "Browse";
            this.BrowseForGamePath.UseVisualStyleBackColor = true;
            this.BrowseForGamePath.Click += new System.EventHandler(this.BrowseForGamePathButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(722, 24);
            this.menuStrip1.TabIndex = 15;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetApplicationStateToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(61, 20);
            this.toolStripMenuItem1.Text = "Settings";
            // 
            // resetApplicationStateToolStripMenuItem
            // 
            this.resetApplicationStateToolStripMenuItem.Name = "resetApplicationStateToolStripMenuItem";
            this.resetApplicationStateToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.resetApplicationStateToolStripMenuItem.Text = "Reset application state";
            this.resetApplicationStateToolStripMenuItem.Click += new System.EventHandler(this.resetApplicationStateToolStripMenuItem_Click);
            // 
            // DiskSpaceLeftLabel
            // 
            this.DiskSpaceLeftLabel.AutoSize = true;
            this.DiskSpaceLeftLabel.Location = new System.Drawing.Point(635, 540);
            this.DiskSpaceLeftLabel.Name = "DiskSpaceLeftLabel";
            this.DiskSpaceLeftLabel.Size = new System.Drawing.Size(35, 13);
            this.DiskSpaceLeftLabel.TabIndex = 16;
            this.DiskSpaceLeftLabel.Text = "label6";
            // 
            // GameLoaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 582);
            this.Controls.Add(this.DiskSpaceLeftLabel);
            this.Controls.Add(this.BrowseForGamePath);
            this.Controls.Add(this.BrowseForAutoDiscoveryFolderButton);
            this.Controls.Add(this.AddAutodiscoveryFolderButton);
            this.Controls.Add(this.AddAutoDiscoveryTextBox);
            this.Controls.Add(this.saveFastFolderButton);
            this.Controls.Add(this.fastFolderTextBox);
            this.Controls.Add(fastFolderLabel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.gameDataGroupbox);
            this.Controls.Add(label2);
            this.Controls.Add(label5);
            this.Controls.Add(label1);
            this.Controls.Add(this.newGameName);
            this.Controls.Add(this.addNewGameButton);
            this.Controls.Add(this.newGamePathTextBox);
            this.Controls.Add(this.folderGridView);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(738, 400);
            this.Name = "GameLoaderForm";
            this.Text = "Game Loader";
            ((System.ComponentModel.ISupportInitialize)(this.folderGridView)).EndInit();
            this.gameDataGroupbox.ResumeLayout(false);
            this.gameDataGroupbox.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView folderGridView;
        private System.Windows.Forms.TextBox newGamePathTextBox;
        private System.Windows.Forms.Button addNewGameButton;
        private System.Windows.Forms.TextBox newGameName;
        private System.Windows.Forms.GroupBox gameDataGroupbox;
        private System.Windows.Forms.Button saveChangesButton;
        private System.Windows.Forms.TextBox pathEditTextBox;
        private System.Windows.Forms.TextBox nameEditTextBox;
        private System.Windows.Forms.Button activateGameButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusToolStripLabel;
        private System.Windows.Forms.TextBox fastFolderTextBox;
        private System.Windows.Forms.Button saveFastFolderButton;
        private System.Windows.Forms.TextBox AddAutoDiscoveryTextBox;
        private System.Windows.Forms.Button AddAutodiscoveryFolderButton;
        private System.Windows.Forms.Button BrowseForAutoDiscoveryFolderButton;
        private System.Windows.Forms.Button BrowseForGamePath;
        private System.Windows.Forms.ToolStripProgressBar statusStipProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel FileProgressStatusStripLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem resetApplicationStateToolStripMenuItem;
        private System.Windows.Forms.Label DiskSpaceLeftLabel;
    }
}

