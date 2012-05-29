namespace CSharp
{
    using System;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            HE853.IDevice device = new HE853.Device();
            try
            {
                device.Open();
                device.SwitchOn(1001, false);
                device.SwitchOff(1001, false);
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