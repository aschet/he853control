﻿/*
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
    using System.Reflection;
    using System.Windows.Forms;

    /// <summary>
    /// GUI control for device interaction.
    /// </summary>
    public partial class MainWindow : Form
    {
        /// <summary>
        /// Device for interaction.
        /// </summary>
        private Device device = new Device();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.dimComboBox.SelectedIndex = 0;
            this.Text = this.Text + " " + Assembly.GetExecutingAssembly().GetName().Version;
        }

        /// <summary>
        /// Presents common error message to user.
        /// </summary>
        /// <param name="result">False if error message should be shown.</param>
        private static void Notify(bool result)
        {
            if (!result)
            {
                Notify("Error during command send!");
            }
        }

        /// <summary>
        /// Presents specific message to user.
        /// </summary>
        /// <param name="message">Message to present.</param>
        private static void Notify(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// Handles On button click, switches device on.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnButton_Click(object sender, EventArgs e)
        {
            this.onButton.Enabled = false;
            Notify(this.device.SwitchOn(this.GetDeviceCode(), this.UseShortCommands()));
            this.onButton.Enabled = true;
        }

        /// <summary>
        /// Handles Off button click, switches device off.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OffButton_Click(object sender, EventArgs e)
        {
            this.offButton.Enabled = false;
            Notify(this.device.SwitchOff(this.GetDeviceCode(), this.UseShortCommands()));
            this.offButton.Enabled = true;
        }

        /// <summary>
        /// Handles Dim button click, adjusts dim.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void DimButton_Click(object sender, EventArgs e)
        {
            this.dimButton.Enabled = false;
            Notify(this.device.AdjustDim(this.GetDeviceCode(), int.Parse(this.dimComboBox.Text)));
            this.dimButton.Enabled = true;
        }

        /// <summary>
        /// Initializes device communication on startup.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void MainWindow_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.device.Open())
                {
                    Notify("The device is not attached or in use!");
                }
            }
            catch
            {
                Notify("The service does not respond!");
            }
        }

        /// <summary>
        /// Get the selected device code.
        /// </summary>
        /// <returns>The selected device code.</returns>
        private int GetDeviceCode()
        {
            return Convert.ToInt32(this.deviceCodeUpDown.Value);
        }

        /// <summary>
        /// Determines if user selected shorts commands.
        /// </summary>
        /// <returns>True if short commands should be used.</returns>
        private bool UseShortCommands()
        {
            return this.checkBoxShortCommands.Checked;
        }
    }
}