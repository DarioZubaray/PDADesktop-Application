namespace PDADesktop.Classes.Devices
{
    public interface IDeviceHandler
    {
        string GetName();
        bool IsDeviceConnected();

        ResultFileOperation CopyDeviceFileToPublicData(string sourceDirectory, string filenameAndExtension);
        ResultFileOperation CopyPublicDataFileToDevice(string DestinationDirectory, string filenameAndExtension);
        ResultFileOperation CopyPublicBinFileToDevice(string DestinationDirectory, string filenameAndExtension);
        ResultFileOperation DeleteDeviceDataFile(string filename);
        ResultFileOperation DeletePublicDataFile(string filename);

        string ReadVersionDeviceProgramFileFromDefaultData();
        void CreateEmptyDefaultDataFile();
        string GetLastVersionProgramFileFromServer();
        string GetNewDefaultDataContent();

        string ReadAdjustmentsDataFile();
        bool OverWriteAdjustmentMade(string newContent);
    }
}
