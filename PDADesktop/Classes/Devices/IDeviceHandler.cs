using PDADesktop.Model.Dto;

namespace PDADesktop.Classes.Devices
{
    public interface IDeviceHandler
    {
        string GetNameToDisplay();
        string GetAdapterName();
        bool IsDeviceConnected();

        ResultFileOperation CopyFromDeviceToPublicFolder(string filename, string deviceFolder, string publicFolder);
        ResultFileOperation CopyDeviceFileToPublicLookUp(string filenameAndExtension);
        ResultFileOperation CopyDeviceFileToPublicData(string filenameAndExtension);

        ResultFileOperation CopyPublicDataFileToDevice(string destinationDirectory, string filenameAndExtension);
        ResultFileOperation CopyPublicBinFileToDevice(string destinationDirectory, string filenameAndExtension);
        ResultFileOperation CopyPublicLookUpFileToDevice(string destinationDirectory, string filenameAndExtension);
        ResultFileOperation DeleteDeviceDataFile(string filename);
        ResultFileOperation DeletePublicDataFile(string filename);
        ResultFileOperation DeleteDeviceAndPublicDataFiles(string filename);

        string ReadSynchronizationStateFromDefaultData();
        string ReadSynchronizationDateFromDefaultData();
        string ReadStoreIdFromDefaultData();
        string ReadVersionDeviceProgramFileFromDefaultData();

        void CreateEmptyDefaultDataFile();
        string GetLastVersionProgramFileFromServer();
        string GetNewDefaultDataContent();
        ActionResultDto ControlDeviceLock(long syncId, string storeId);
        void ChangeSynchronizationState(string syncState);

        string ReadPriceControlDataFile();
        bool OverWritePriceControlMade(string newContent);
        string ReadAdjustmentsDataFile();
        bool OverWriteAdjustmentMade(string newContent);
        string ReadReceptionDataFile();
        bool OverWriteReceptionMade(string newContent);
        string ReadLabelDataFile();
        bool OverWriteLabelMade(string newContent);
    }
}
