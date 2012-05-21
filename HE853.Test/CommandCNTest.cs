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