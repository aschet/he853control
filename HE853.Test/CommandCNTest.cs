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

    [TestClass]
    public class CommandCNTest
    {
        [TestMethod]
        [DeploymentItem("HE853.dll")]
        public void WriteDataOnTest()
        {
            byte[] dataExpected = { 37, 101, 73, 224, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            CommandCN_Accessor target = new CommandCN_Accessor();
            using (MemoryStream stream = new MemoryStream())
            {
                target.WriteData(stream, 1001, Command.On);
                CollectionAssert.AreEqual(dataExpected, stream.ToArray());
            }
        }

        [TestMethod]
        [DeploymentItem("HE853.dll")]
        public void WriteDataOffTest()
        {
            byte[] dataExpected = { 36, 224, 28, 96, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            CommandCN_Accessor target = new CommandCN_Accessor();
            using (MemoryStream stream = new MemoryStream())
            {
                target.WriteData(stream, 1001, Command.Off);
                CollectionAssert.AreEqual(dataExpected, stream.ToArray());
            }
        }

        [TestMethod]
        [DeploymentItem("HE853.dll")]
        public void WriteDataDim1Test()
        {
            byte[] dataExpected = { 5, 101, 73, 224, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            CommandCN_Accessor target = new CommandCN_Accessor();
            using (MemoryStream stream = new MemoryStream())
            {
                target.WriteData(stream, 1001, "1");
                CollectionAssert.AreEqual(dataExpected, stream.ToArray());
            }
        }

        [TestMethod]
        public void BuildTest()
        {
            byte[] dataExpected =
            {
                1, 0, 32, 1, 224, 0, 0, 0,
                2, 0, 32, 96, 96, 32, 28, 7,
                3, 37, 101, 73, 224, 0, 0, 0,
                4, 0, 0, 0, 0, 0, 0, 0,
                5, 0, 0, 0, 0, 0, 0, 0
            };

            CommandCN_Accessor target = new CommandCN_Accessor();
            byte[] data = target.Build(1001, Command.On);
            CollectionAssert.AreEqual(dataExpected, data);
        }
    }
}