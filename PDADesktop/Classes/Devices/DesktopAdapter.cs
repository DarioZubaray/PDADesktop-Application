using PDADesktop.Utils;
using System.Configuration;

namespace PDADesktop.Classes.Devices
{
    class DesktopAdapter : IDeviceHandler
    {
        public bool isDeviceConnected()
        {
            return true;
        }

        public string ReadAjustesDataFile(string desDir, string filename)
        {
            string userDesktopPDATest = TextUtils.ExpandEnviromentVariable(@"%USERPROFILE%/PDATest");
            string deviceRelPathData = ConfigurationManager.AppSettings.Get("DEVICE_RELPATH_DATA");
            string directory = userDesktopPDATest + deviceRelPathData;
            FileUtils.VerifyFoldersOrCreate(directory);
            if(FileUtils.VerifyIfExitsFile(directory+filename))
            {
                string ajustes = FileUtils.ReadFile(userDesktopPDATest + deviceRelPathData + filename);
                return TextUtils.ParseAjusteDAT2Json(ajustes);
            }
            else
            {
                return null;
            }
        }
    }
}
