namespace CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            HE853.IDevice device = new HE853.Device();
            if (device.Open())
            {
                device.SwitchOn(1001, false);
                device.SwitchOff(1001, false);
                device.Close();
            }
        }
    }
}