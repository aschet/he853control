namespace CSharp
{
    using System;
    using System.IO;
    using HE853;

    class Program
    {
        static void Main(string[] args)
        {
            IDevice device = new Device();
            try
            {
                device.Open();
                device.SwitchOn(1001, CommandStyle.Comprehensive);
                device.SwitchOff(1001, CommandStyle.Comprehensive);
                device.Close();
            }
            catch (FileNotFoundException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception.Message);
                device.Close();
            }
        }
    }
}