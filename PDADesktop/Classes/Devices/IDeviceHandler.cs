namespace PDADesktop.Classes.Devices
{
    public interface IDeviceHandler
    {
        string GetName();
        bool IsDeviceConnected();
        DeviceResultName CopyDeviceFileToAppData(string sourceDirectory, string filenameAndExtension);
        DeviceResultName CopyAppDataFileToDevice(string DestinationDirectory, string filenameAndExtension);

        string getVersionProgramFileFromDevice();
        void CreateDefaultDataFile(string idSucursal);
        string getLastVersionProgramFileFromServer();
        string getNewDefaultDatacontent();

        string ReadAdjustmentsDataFile();
        bool OverWriteAdjustmentMade(string newContent);
    }
}
