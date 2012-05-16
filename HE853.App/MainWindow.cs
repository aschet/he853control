/*
Home Easy HE853 Control
Copyright (C) 2012 Thomas Ascher

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/

namespace HE853.App
{
    using System;
    using System.Windows.Forms;

    public partial class MainWindow : Form
    {
        private Device device = new Device();

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void OnButton_Click(object sender, EventArgs e)
        {
            this.onButton.Enabled = false;
            this.Notify(this.device.On(this.GetDeviceCode()));
            this.onButton.Enabled = true;
        }

        private void OffButton_Click(object sender, EventArgs e)
        {
            this.offButton.Enabled = false;
            this.Notify(this.device.Off(this.GetDeviceCode()));
            this.offButton.Enabled = true;
        }

        private void DimButton_Click(object sender, EventArgs e)
        {
            this.dimButton.Enabled = false;
            this.Notify(this.device.Dim(this.GetDeviceCode(), int.Parse(this.dimEdit.Text)));
            this.dimButton.Enabled = true;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.device.Open())
                {
                    MessageBox.Show("The device is not attached or in use!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("The service does not respond!");
            }
        }

        private void Notify(bool result)
        {
            if (!result)
            {
                MessageBox.Show("Error during command send!");
            }
        }

        private int GetDeviceCode()
        {
            return int.Parse(this.deviceCodeEdit.Text);
        }
    }
}