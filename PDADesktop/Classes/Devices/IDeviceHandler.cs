namespace PDADesktop.Classes.Devices
{
    public interface IDeviceHandler
    {
        string GetName();
        bool IsDeviceConnected();
        DeviceResultName CopyDeviceFileToAppData(string sourceDirectory, string filename);
        string ReadAdjustmentsDataFile(string desDir, string filename);

    }
}
