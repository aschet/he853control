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
            this.dimLabel = new System.Windows.Forms.Label();
            this.dimButton = new System.Windows.Forms.Button();
            this.dimUnitLabel = new System.Windows.Forms.Label();
            this.dimComboBox = new System.Windows.Forms.ComboBox();
            this.deviceCodeUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.deviceCodeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // onButton
            // 
            this.onButton.Location = new System.Drawing.Point(15, 32);
            this.onButton.Name = "onButton";
            this.onButton.Size = new System.Drawing.Size(75, 23);
            this.onButton.TabIndex = 2;
            this.onButton.Text = "On";
            this.onButton.UseVisualStyleBackColor = true;
            this.onButton.Click += new System.EventHandler(this.OnButton_Click);
            // 
            // offButton
            // 
            this.offButton.Location = new System.Drawing.Point(101, 32);
            this.offButton.Name = "offButton";
            this.offButton.Size = new System.Drawing.Size(75, 23);
            this.offButton.TabIndex = 3;
            this.offButton.Text = "Off";
            this.offButton.UseVisualStyleBackColor = true;
            this.offButton.Click += new System.EventHandler(this.OffButton_Click);
            // 
            // deviceCodeLabel
            // 
            this.deviceCodeLabel.AutoSize = true;
            this.deviceCodeLabel.Location = new System.Drawing.Point(12, 9);
            this.deviceCodeLabel.Name = "deviceCodeLabel";
            this.deviceCodeLabel.Size = new System.Drawing.Size(72, 13);
            this.deviceCodeLabel.TabIndex = 0;
            this.deviceCodeLabel.Text = "Device Code:";
            // 
            // dimLabel
            // 
            this.dimLabel.AutoSize = true;
            this.dimLabel.Location = new System.Drawing.Point(12, 71);
            this.dimLabel.Name = "dimLabel";
            this.dimLabel.Size = new System.Drawing.Size(28, 13);
            this.dimLabel.TabIndex = 4;
            this.dimLabel.Text = "Dim:";
            // 
            // dimButton
            // 
            this.dimButton.Location = new System.Drawing.Point(15, 94);
            this.dimButton.Name = "dimButton";
            this.dimButton.Size = new System.Drawing.Size(161, 23);
            this.dimButton.TabIndex = 7;
            this.dimButton.Text = "Dim";
            this.dimButton.UseVisualStyleBackColor = true;
            this.dimButton.Click += new System.EventHandler(this.DimButton_Click);
            // 
            // dimUnitLabel
            // 
            this.dimUnitLabel.AutoSize = true;
            this.dimUnitLabel.Location = new System.Drawing.Point(157, 71);
            this.dimUnitLabel.Name = "dimUnitLabel";
            this.dimUnitLabel.Size = new System.Drawing.Size(15, 13);
            this.dimUnitLabel.TabIndex = 6;
            this.dimUnitLabel.Text = "%";
            // 
            // dimComboBox
            // 
            this.dimComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dimComboBox.FormattingEnabled = true;
            this.dimComboBox.Items.AddRange(new object[] {
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80"});
            this.dimComboBox.Location = new System.Drawing.Point(90, 67);
            this.dimComboBox.Name = "dimComboBox";
            this.dimComboBox.Size = new System.Drawing.Size(61, 21);
            this.dimComboBox.TabIndex = 5;
            // 
            // deviceCodeUpDown
            // 
            this.deviceCodeUpDown.Location = new System.Drawing.Point(90, 7);
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
            this.deviceCodeUpDown.Size = new System.Drawing.Size(86, 20);
            this.deviceCodeUpDown.TabIndex = 1;
            this.deviceCodeUpDown.Value = new decimal(new int[] {
            1001,
            0,
            0,
            0});
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(184, 124);
            this.Controls.Add(this.deviceCodeUpDown);
            this.Controls.Add(this.dimComboBox);
            this.Controls.Add(this.dimUnitLabel);
            this.Controls.Add(this.dimButton);
            this.Controls.Add(this.dimLabel);
            this.Controls.Add(this.deviceCodeLabel);
            this.Controls.Add(this.offButton);
            this.Controls.Add(this.onButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainWindow";
            this.Text = "HE853 Control";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.deviceCodeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button onButton;
        private System.Windows.Forms.Button offButton;
        private System.Windows.Forms.Label deviceCodeLabel;
        private System.Windows.Forms.Label dimLabel;
        private System.Windows.Forms.Button dimButton;
        private System.Windows.Forms.Label dimUnitLabel;
        private System.Windows.Forms.ComboBox dimComboBox;
        private System.Windows.Forms.NumericUpDown deviceCodeUpDown;
    }
}