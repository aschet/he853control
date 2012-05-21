namespace HE853.Test
{
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public class CommandEUTest
    {
        [TestMethod()]
        [DeploymentItem("HE853.dll")]
        public void BuildOnTest()
        {            
            byte[] dataExpected = { 3, 199, 143, 30, 116, 171, 146, 229, 4, 128, 0, 0, 0, 0, 0, 0 };

            CommandEU_Accessor target = new CommandEU_Accessor();
            MemoryStream stream = new MemoryStream();
            target.BuildData(ref stream, 1001, Command.On);
            byte[] data = stream.ToArray();

            CollectionAssert.AreEqual(dataExpected, data);
        }

        [TestMethod()]
        [DeploymentItem("HE853.dll")]
        public void BuildOffTest()
        {
            byte[] dataExpected = { 3, 199, 143, 30, 116, 171, 145, 229, 4, 128, 0, 0, 0, 0, 0, 0 };

            CommandEU_Accessor target = new CommandEU_Accessor();
            MemoryStream stream = new MemoryStream();
            target.BuildData(ref stream, 1001, Command.Off);
            byte[] data = stream.ToArray();

            CollectionAssert.AreEqual(dataExpected, data);
        }
    }
}