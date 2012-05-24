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

            Command target = this.CreateCommand();       
            MemoryStream stream = new MemoryStream();
            byte[] data = target.BuildStatus();

            CollectionAssert.AreEqual(dataExpected, data);        
        }

        [TestMethod]
        [DeploymentItem("HE853.dll")]
        public void BuildSpecTest()
        {
            byte[] dataExpected = { 0, 32, 1, 224, 0, 0, 0, 0, 32, 96, 96, 32, 28, 7 };

            Command_Accessor target = this.CreateCommand_Accessor();
            MemoryStream stream = new MemoryStream();
            target.BuildSpec(stream);
            byte[] data = stream.ToArray();
            CollectionAssert.AreEqual(dataExpected, data);
        }

        [TestMethod]
        [DeploymentItem("HE853.dll")]
        public void BuildExecTest()
        {
            byte[] dataExpected = { 0, 0, 0, 0, 0, 0, 0 };

            Command_Accessor target = this.CreateCommand_Accessor();
            MemoryStream stream = new MemoryStream();
            target.BuildExec(stream);
            byte[] data = stream.ToArray();
            CollectionAssert.AreEqual(dataExpected, data);
        }

        [TestMethod]
        [DeploymentItem("HE853.dll")]
        public void WriteZeroTest()
        {
            byte[] dataExpected = { 0, 0, 0 };

            Command_Accessor target = this.CreateCommand_Accessor();
            MemoryStream stream = new MemoryStream();
            target.WriteZero(stream, 3);
            byte[] data = stream.ToArray();
            CollectionAssert.AreEqual(dataExpected, data);
        }

        internal virtual Command CreateCommand()
        {
            Command target = new CommandCN();
            return target;
        }

        internal virtual Command_Accessor CreateCommand_Accessor()
        {
            Command_Accessor target = new Command_Accessor(new PrivateObject(this.CreateCommand(), new PrivateType(typeof(Command))));
            return target;
        }

        [TestMethod()]
        [DeploymentItem("HE853.dll")]
        public void PackSevenWithSequenceNumberTest()
        {
            byte[] dataExpected = new byte[] { 1, 0, 1, 2, 3, 4, 5, 6, 2, 7, 8, 9, 10, 11, 12, 13 };

            Command_Accessor target = this.CreateCommand_Accessor();
            byte[] data = target.PackSevenWithSequenceNumber(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 });
            CollectionAssert.AreEqual(dataExpected, data);
        }
    }
}
