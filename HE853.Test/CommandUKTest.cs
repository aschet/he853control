namespace HE853.Test
{
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public class CommandUKTest
    {
        [TestMethod()]
        [DeploymentItem("HE853.dll")]
        public void BuildOnTest()
        {            
            byte[] dataExpected = { 3, 60, 193, 21, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0 };

            CommandUK_Accessor target = new CommandUK_Accessor();
            MemoryStream stream = new MemoryStream();
            target.BuildData(ref stream, 1001, Command.On);
            byte[] data = stream.ToArray();

            CollectionAssert.AreEqual(dataExpected, data);
        }

        [TestMethod()]
        [DeploymentItem("HE853.dll")]
        public void BuildOffTest()
        {
            byte[] dataExpected = { 3, 60, 193, 20, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0 };

            CommandUK_Accessor target = new CommandUK_Accessor();
            MemoryStream stream = new MemoryStream();
            target.BuildData(ref stream, 1001, Command.Off);
            byte[] data = stream.ToArray();

            CollectionAssert.AreEqual(dataExpected, data);
        }
    }
}