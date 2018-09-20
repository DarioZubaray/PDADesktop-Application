namespace PDADesktop.Classes.Devices
{
    public interface IDeviceHandler
    {
        bool isDeviceConnected();
        string ReadAjustesDataFile(string desDir, string filename);

    }
}
