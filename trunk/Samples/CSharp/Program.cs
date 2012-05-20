namespace CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            HE853.Device device = new HE853.Device();
            if (device.Open())
            {
                device.On(1001);
                device.Off(1001);
                device.Close();
            }
        }
    }
}