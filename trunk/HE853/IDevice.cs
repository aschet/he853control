namespace HE853
{
    using System.Runtime.InteropServices;

    [ComVisible(true), GuidAttribute("A968C162-5E23-4448-A92C-95588B21A0B7")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IDevice
    {
        bool Open();

        void Close();

        bool On(int deviceCode);

        bool Off(int deviceCode);

        bool Dim(int deviceCode, int percent);
    }
}