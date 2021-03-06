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
    using System.Windows.Forms;

    /// <summary>
    /// Main program.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Main program.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        [STAThread]
        private static void Main(string[] args)
        {
            if (Rpc.HasServiceArg(args))
            {
                Rpc.RegisterClient();
            }

            Application.EnableVisualStyles();
            Application.Run(new MainWindow());
        }
    }
}