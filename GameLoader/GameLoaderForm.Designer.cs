namespace GameLoader
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
            this.folderGridView = new System.Windows.Forms.DataGridView();
            this.newGamePathTextBox = new System.Windows.Forms.TextBox();
            this.addNewGameButton = new System.Windows.Forms.Button();
            this.newGameName = new System.Windows.Forms.TextBox();
            this.gameDataGroupbox = new System.Windows.Forms.GroupBox();
            this.activateGameButton = new System.Windows.Forms.Button();
            this.saveChangesButton = new System.Windows.Forms.Button();
            this.pathEditTextBox = new System.Windows.Forms.TextBox();
            this.nameEditTextBox = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.folderGridView)).BeginInit();
            this.gameDataGroupbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(13, 310);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(66, 13);
            label1.TabIndex = 4;
            label1.Text = "Game Name";
            // 
            // label2
            // 
            label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(150, 310);
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
            // folderGridView
            // 
            this.folderGridView.AllowUserToAddRows = false;
            this.folderGridView.AllowUserToOrderColumns = true;
            this.folderGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.folderGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.folderGridView.Location = new System.Drawing.Point(13, 13);
            this.folderGridView.MultiSelect = false;
            this.folderGridView.Name = "folderGridView";
            this.folderGridView.ReadOnly = true;
            this.folderGridView.Size = new System.Drawing.Size(544, 286);
            this.folderGridView.TabIndex = 0;
            this.folderGridView.CurrentCellChanged += new System.EventHandler(this.folderGridView_CurrentCellChanged);
            // 
            // newGamePathTextBox
            // 
            this.newGamePathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newGamePathTextBox.Location = new System.Drawing.Point(150, 329);
            this.newGamePathTextBox.Name = "newGamePathTextBox";
            this.newGamePathTextBox.Size = new System.Drawing.Size(479, 20);
            this.newGamePathTextBox.TabIndex = 1;
            // 
            // addNewGameButton
            // 
            this.addNewGameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addNewGameButton.Location = new System.Drawing.Point(635, 326);
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
            this.newGameName.Location = new System.Drawing.Point(13, 329);
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
            this.gameDataGroupbox.Location = new System.Drawing.Point(563, 13);
            this.gameDataGroupbox.Name = "gameDataGroupbox";
            this.gameDataGroupbox.Size = new System.Drawing.Size(146, 285);
            this.gameDataGroupbox.TabIndex = 6;
            this.gameDataGroupbox.TabStop = false;
            this.gameDataGroupbox.Text = "Game Data";
            // 
            // activateGameButton
            // 
            this.activateGameButton.Location = new System.Drawing.Point(9, 256);
            this.activateGameButton.Name = "activateGameButton";
            this.activateGameButton.Size = new System.Drawing.Size(131, 23);
            this.activateGameButton.TabIndex = 4;
            this.activateGameButton.Text = "Activate";
            this.activateGameButton.UseVisualStyleBackColor = true;
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
            // GameLoaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 361);
            this.Controls.Add(this.gameDataGroupbox);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.newGameName);
            this.Controls.Add(this.addNewGameButton);
            this.Controls.Add(this.newGamePathTextBox);
            this.Controls.Add(this.folderGridView);
            this.MinimumSize = new System.Drawing.Size(738, 400);
            this.Name = "GameLoaderForm";
            this.Text = "Game Loader";
            ((System.ComponentModel.ISupportInitialize)(this.folderGridView)).EndInit();
            this.gameDataGroupbox.ResumeLayout(false);
            this.gameDataGroupbox.PerformLayout();
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
    }
}

