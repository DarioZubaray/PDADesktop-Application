namespace PDADesktop.Classes.Devices
{
    public interface IDeviceHandler
    {
        string GetName();
        bool IsDeviceConnected();
        DeviceResultName CopyDeviceFileToAppData(string sourceDirectory, string filenameAndExtension);
        DeviceResultName CopyAppDataFileToDevice(string DestinationDirectory, string filenameAndExtension);
        void CreateDefaultDataFile();
        string ReadDefaultDataFile();
        string ReadAdjustmentsDataFile(string desDir, string filenameAndExtension);

    }
}
