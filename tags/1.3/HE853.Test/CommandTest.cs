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

namespace HE853.Test
{
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CommandTest
    {
        [TestMethod]
        public void BuildStatusTest()
        {
            byte[] dataExpected = { 6, 1, 0, 0, 0, 0, 0, 0 };

            byte[] data = Command.BuildStatus();
            CollectionAssert.AreEqual(dataExpected, data);        
        }

        [TestMethod]
        [DeploymentItem("HE853.dll")]
        public void WriteRFSpecTest()
        {
            byte[] dataExpected = { 0, 32, 1, 224, 0, 0, 0, 0, 32, 96, 96, 32, 28, 7 };

            Command_Accessor target = this.CreateCommand_Accessor();
            using (MemoryStream stream = new MemoryStream())
            {
                target.WriteRFSpec(stream);
                CollectionAssert.AreEqual(dataExpected, stream.ToArray());
            }
        }

        [TestMethod]
        [DeploymentItem("HE853.dll")]
        public void WriteExecTest()
        {
            byte[] dataExpected = { 0, 0, 0, 0, 0, 0, 0 };

            using (MemoryStream stream = new MemoryStream())
            {
                Command_Accessor.WriteExec(stream);
                CollectionAssert.AreEqual(dataExpected, stream.ToArray());
            }
        }

        [TestMethod]
        [DeploymentItem("HE853.dll")]
        public void WriteZeroTest()
        {
            byte[] dataExpected = { 0, 0, 0 };

            using (MemoryStream stream = new MemoryStream())
            {
                Command_Accessor.WriteZero(stream, 3);
                CollectionAssert.AreEqual(dataExpected, stream.ToArray());
            }
        }

        [TestMethod]
        [DeploymentItem("HE853.dll")]
        public void WriteUShortTest()
        {
            byte[] dataExpected = { 0, 10 };

            using (MemoryStream stream = new MemoryStream())
            {
                Command_Accessor.WriteUShort(stream, 10);
                CollectionAssert.AreEqual(dataExpected, stream.ToArray());
            }
        }

        [TestMethod]
        [DeploymentItem("HE853.dll")]
        public void IsValidDeviceCodeInRangeTest()
        {
            Assert.AreEqual(true, Command_Accessor.IsValidDeviceCode(Command_Accessor.MinDeviceCode));
            Assert.AreEqual(true, Command_Accessor.IsValidDeviceCode(Command_Accessor.MaxDeviceCode));
            Assert.AreEqual(true, Command_Accessor.IsValidDeviceCode(Command_Accessor.MinDeviceCode + ((Command_Accessor.MaxDeviceCode - Command_Accessor.MinDeviceCode) / 2)));
        }

        [TestMethod]
        [DeploymentItem("HE853.dll")]
        public void IsValidDeviceCodeOutOfRangeTest()
        {
            Assert.AreEqual(false, Command_Accessor.IsValidDeviceCode(Command_Accessor.MinDeviceCode - 1));
            Assert.AreEqual(false, Command_Accessor.IsValidDeviceCode(Command_Accessor.MaxDeviceCode + 1));
        }

        [TestMethod]
        [DeploymentItem("HE853.dll")]
        public void IsValidDimInRangeTest()
        {
            Assert.AreEqual(true, Command_Accessor.IsValidDim(Command_Accessor.MinDim));
            Assert.AreEqual(true, Command_Accessor.IsValidDim(Command_Accessor.MaxDim));
            Assert.AreEqual(true, Command_Accessor.IsValidDim(Command_Accessor.MinDim + ((Command_Accessor.MaxDim - Command_Accessor.MinDim) / 2)));
        }

        [TestMethod]
        [DeploymentItem("HE853.dll")]
        public void IsValidDimOutOfRangeTest()
        {
            Assert.AreEqual(false, Command_Accessor.IsValidDim(Command_Accessor.MinDim - 1));
            Assert.AreEqual(false, Command_Accessor.IsValidDim(Command_Accessor.MaxDim + 1));
        }

        [TestMethod]
        [DeploymentItem("HE853.dll")]
        public void PackSevenWithSequenceNumberTest()
        {
            byte[] dataExpected = new byte[] { 1, 0, 1, 2, 3, 4, 5, 6, 2, 7, 8, 9, 10, 11, 12, 13 };
            byte[] data = Command_Accessor.PackSevenWithSequenceNumber(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 });
            CollectionAssert.AreEqual(dataExpected, data);
        }

        internal virtual Command CreateCommand()
        {
            return new CommandCN();
        }

        internal virtual Command_Accessor CreateCommand_Accessor()
        {
            return new Command_Accessor(new PrivateObject(this.CreateCommand(), new PrivateType(typeof(Command))));
        }
    }
}
