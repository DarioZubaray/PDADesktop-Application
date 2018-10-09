namespace PDADesktop.Classes.Devices
{
    public interface IDeviceHandler
    {
        string GetName();
        bool IsDeviceConnected();

        ResultFileOperation CopyDeviceFileToPublicLookUp(string filenameAndExtension);
        ResultFileOperation CopyDeviceFileToPublicData(string filenameAndExtension);

        ResultFileOperation CopyPublicDataFileToDevice(string destinationDirectory, string filenameAndExtension);
        ResultFileOperation CopyPublicBinFileToDevice(string destinationDirectory, string filenameAndExtension);
        ResultFileOperation CopyPublicLookUpFileToDevice(string destinationDirectory, string filenameAndExtension);
        ResultFileOperation DeleteDeviceDataFile(string filename);
        ResultFileOperation DeletePublicDataFile(string filename);
        ResultFileOperation DeleteDeviceAndPublicDataFiles(string filename);

        string ReadVersionDeviceProgramFileFromDefaultData();
        string ReadSynchronizationDateFromDefaultData();
        string ReadBranchOfficeFromDefaultData();
        void CreateEmptyDefaultDataFile();
        string GetLastVersionProgramFileFromServer();
        string GetNewDefaultDataContent();

        string ReadAdjustmentsDataFile();
        bool OverWriteAdjustmentMade(string newContent);
    }
}
