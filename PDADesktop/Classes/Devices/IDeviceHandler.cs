﻿namespace PDADesktop.Classes.Devices
{
    public interface IDeviceHandler
    {
        string GetName();
        bool IsDeviceConnected();
        DeviceResultName CopyDeviceFileToAppData(string sourceDirectory, string filename);
        DeviceResultName CopyAppDataFileToDevice(string DestinationDirectory, string filename);
        void CreateDefaultDataFile();
        string ReadAdjustmentsDataFile(string desDir, string filename);

    }
}