namespace HE853.App
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.onButton = new System.Windows.Forms.Button();
            this.offButton = new System.Windows.Forms.Button();
            this.deviceCodeLabel = new System.Windows.Forms.Label();
            this.dimButton = new System.Windows.Forms.Button();
            this.dimComboBox = new System.Windows.Forms.ComboBox();
            this.deviceCodeUpDown = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxShortCommands = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.deviceCodeUpDown)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // onButton
            // 
            this.onButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.onButton.Location = new System.Drawing.Point(8, 34);
            this.onButton.Name = "onButton";
            this.onButton.Size = new System.Drawing.Size(84, 23);
            this.onButton.TabIndex = 2;
            this.onButton.Text = "On";
            this.onButton.UseVisualStyleBackColor = true;
            this.onButton.Click += new System.EventHandler(this.OnButton_Click);
            // 
            // offButton
            // 
            this.offButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.offButton.Location = new System.Drawing.Point(98, 34);
            this.offButton.Name = "offButton";
            this.offButton.Size = new System.Drawing.Size(83, 23);
            this.offButton.TabIndex = 3;
            this.offButton.Text = "Off";
            this.offButton.UseVisualStyleBackColor = true;
            this.offButton.Click += new System.EventHandler(this.OffButton_Click);
            // 
            // deviceCodeLabel
            // 
            this.deviceCodeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.deviceCodeLabel.AutoSize = true;
            this.deviceCodeLabel.Location = new System.Drawing.Point(8, 11);
            this.deviceCodeLabel.Name = "deviceCodeLabel";
            this.deviceCodeLabel.Size = new System.Drawing.Size(84, 13);
            this.deviceCodeLabel.TabIndex = 0;
            this.deviceCodeLabel.Text = "Device Code:";
            this.deviceCodeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dimButton
            // 
            this.dimButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dimButton.AutoSize = true;
            this.dimButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.dimButton.Location = new System.Drawing.Point(8, 63);
            this.dimButton.Name = "dimButton";
            this.dimButton.Size = new System.Drawing.Size(84, 23);
            this.dimButton.TabIndex = 4;
            this.dimButton.Text = "Dim:";
            this.dimButton.UseVisualStyleBackColor = true;
            this.dimButton.Click += new System.EventHandler(this.DimButton_Click);
            // 
            // dimComboBox
            // 
            this.dimComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dimComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dimComboBox.FormattingEnabled = true;
            this.dimComboBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.dimComboBox.Location = new System.Drawing.Point(98, 63);
            this.dimComboBox.Name = "dimComboBox";
            this.dimComboBox.Size = new System.Drawing.Size(83, 21);
            this.dimComboBox.TabIndex = 5;
            // 
            // deviceCodeUpDown
            // 
            this.deviceCodeUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.deviceCodeUpDown.Location = new System.Drawing.Point(98, 8);
            this.deviceCodeUpDown.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.deviceCodeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.deviceCodeUpDown.Name = "deviceCodeUpDown";
            this.deviceCodeUpDown.Size = new System.Drawing.Size(83, 20);
            this.deviceCodeUpDown.TabIndex = 1;
            this.deviceCodeUpDown.Value = new decimal(new int[] {
            1001,
            0,
            0,
            0});
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.47619F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.52381F));
            this.tableLayoutPanel.Controls.Add(this.deviceCodeUpDown, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.deviceCodeLabel, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.onButton, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.offButton, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.dimComboBox, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.dimButton, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.checkBoxShortCommands, 0, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(189, 118);
            this.tableLayoutPanel.TabIndex = 7;
            // 
            // checkBoxShortCommands
            // 
            this.checkBoxShortCommands.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxShortCommands.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.checkBoxShortCommands, 2);
            this.checkBoxShortCommands.Location = new System.Drawing.Point(8, 92);
            this.checkBoxShortCommands.Name = "checkBoxShortCommands";
            this.checkBoxShortCommands.Size = new System.Drawing.Size(173, 17);
            this.checkBoxShortCommands.TabIndex = 6;
            this.checkBoxShortCommands.Text = "Use short commands";
            this.checkBoxShortCommands.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(189, 118);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(200, 156);
            this.Name = "MainWindow";
            this.Text = "HE853 Control";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.deviceCodeUpDown)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button onButton;
        private System.Windows.Forms.Button offButton;
        private System.Windows.Forms.Label deviceCodeLabel;
        private System.Windows.Forms.Button dimButton;
        private System.Windows.Forms.ComboBox dimComboBox;
        private System.Windows.Forms.NumericUpDown deviceCodeUpDown;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.CheckBox checkBoxShortCommands;
    }
}