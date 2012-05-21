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

namespace HE853.Test
{
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public class CommandCNTest
    {
        [TestMethod()]
        [DeploymentItem("HE853.dll")]
        public void BuildOnTest()
        {
            byte[] dataExpected = { 3, 37, 101, 73, 224, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0 };

            CommandCN_Accessor target = new CommandCN_Accessor();
            MemoryStream stream = new MemoryStream();
            target.BuildData(ref stream, 1001, Command.On);
            byte[] data = stream.ToArray();

            CollectionAssert.AreEqual(dataExpected, data);
        }

        [TestMethod()]
        [DeploymentItem("HE853.dll")]
        public void BuildOffTest()
        {
            byte[] dataExpected = { 3, 36, 224, 28, 96, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0 };

            CommandCN_Accessor target = new CommandCN_Accessor();
            MemoryStream stream = new MemoryStream();
            target.BuildData(ref stream, 1001, Command.Off);
            byte[] data = stream.ToArray();

            CollectionAssert.AreEqual(dataExpected, data);
        }

        [TestMethod()]
        [DeploymentItem("HE853.dll")]
        public void BuildDimTest()
        {
            byte[] dataExpected = { 3, 5, 101, 73, 224, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0 };

            CommandCN_Accessor target = new CommandCN_Accessor();
            MemoryStream stream = new MemoryStream();
            target.BuildData(ref stream, 1001, "10");
            byte[] data = stream.ToArray();

            CollectionAssert.AreEqual(dataExpected, data);
        }
    }
}