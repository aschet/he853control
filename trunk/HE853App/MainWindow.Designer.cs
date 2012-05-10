namespace HE853App
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
            this.onButton = new System.Windows.Forms.Button();
            this.offButton = new System.Windows.Forms.Button();
            this.deviceIDLabel = new System.Windows.Forms.Label();
            this.dimLabel = new System.Windows.Forms.Label();
            this.dimButton = new System.Windows.Forms.Button();
            this.deviceIDEdit = new System.Windows.Forms.TextBox();
            this.dimEdit = new System.Windows.Forms.TextBox();
            this.dimUnitLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // onButton
            // 
            this.onButton.Location = new System.Drawing.Point(15, 32);
            this.onButton.Name = "onButton";
            this.onButton.Size = new System.Drawing.Size(75, 23);
            this.onButton.TabIndex = 0;
            this.onButton.Text = "On";
            this.onButton.UseVisualStyleBackColor = true;
            this.onButton.Click += new System.EventHandler(this.OnButton_Click);
            // 
            // offButton
            // 
            this.offButton.Location = new System.Drawing.Point(101, 32);
            this.offButton.Name = "offButton";
            this.offButton.Size = new System.Drawing.Size(75, 23);
            this.offButton.TabIndex = 1;
            this.offButton.Text = "Off";
            this.offButton.UseVisualStyleBackColor = true;
            this.offButton.Click += new System.EventHandler(this.OffButton_Click);
            // 
            // deviceIDLabel
            // 
            this.deviceIDLabel.AutoSize = true;
            this.deviceIDLabel.Location = new System.Drawing.Point(12, 9);
            this.deviceIDLabel.Name = "deviceIDLabel";
            this.deviceIDLabel.Size = new System.Drawing.Size(58, 13);
            this.deviceIDLabel.TabIndex = 2;
            this.deviceIDLabel.Text = "Device-ID:";
            // 
            // dimLabel
            // 
            this.dimLabel.AutoSize = true;
            this.dimLabel.Location = new System.Drawing.Point(12, 71);
            this.dimLabel.Name = "dimLabel";
            this.dimLabel.Size = new System.Drawing.Size(28, 13);
            this.dimLabel.TabIndex = 3;
            this.dimLabel.Text = "Dim:";
            // 
            // dimButton
            // 
            this.dimButton.Location = new System.Drawing.Point(15, 94);
            this.dimButton.Name = "dimButton";
            this.dimButton.Size = new System.Drawing.Size(161, 23);
            this.dimButton.TabIndex = 5;
            this.dimButton.Text = "Dim";
            this.dimButton.UseVisualStyleBackColor = true;
            this.dimButton.Click += new System.EventHandler(this.DimButton_Click);
            // 
            // deviceIDEdit
            // 
            this.deviceIDEdit.Location = new System.Drawing.Point(76, 6);
            this.deviceIDEdit.Name = "deviceIDEdit";
            this.deviceIDEdit.Size = new System.Drawing.Size(100, 20);
            this.deviceIDEdit.TabIndex = 6;
            this.deviceIDEdit.Text = "1001";
            // 
            // dimEdit
            // 
            this.dimEdit.Location = new System.Drawing.Point(78, 68);
            this.dimEdit.Name = "dimEdit";
            this.dimEdit.Size = new System.Drawing.Size(73, 20);
            this.dimEdit.TabIndex = 7;
            this.dimEdit.Text = "10";
            // 
            // dimUnitLabel
            // 
            this.dimUnitLabel.AutoSize = true;
            this.dimUnitLabel.Location = new System.Drawing.Point(157, 71);
            this.dimUnitLabel.Name = "dimUnitLabel";
            this.dimUnitLabel.Size = new System.Drawing.Size(15, 13);
            this.dimUnitLabel.TabIndex = 8;
            this.dimUnitLabel.Text = "%";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(184, 127);
            this.Controls.Add(this.dimUnitLabel);
            this.Controls.Add(this.dimEdit);
            this.Controls.Add(this.deviceIDEdit);
            this.Controls.Add(this.dimButton);
            this.Controls.Add(this.dimLabel);
            this.Controls.Add(this.deviceIDLabel);
            this.Controls.Add(this.offButton);
            this.Controls.Add(this.onButton);
            this.Name = "MainWindow";
            this.Text = "HE853";
            this.Load += new System.EventHandler(this.HE853ControlWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button onButton;
        private System.Windows.Forms.Button offButton;
        private System.Windows.Forms.Label deviceIDLabel;
        private System.Windows.Forms.Label dimLabel;
        private System.Windows.Forms.Button dimButton;
        private System.Windows.Forms.TextBox deviceIDEdit;
        private System.Windows.Forms.TextBox dimEdit;
        private System.Windows.Forms.Label dimUnitLabel;
    }
}