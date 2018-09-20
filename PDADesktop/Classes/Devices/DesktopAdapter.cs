using log4net;
using PDADesktop.Utils;
using System.Configuration;

namespace PDADesktop.Classes.Devices
{
    class DesktopAdapter : IDeviceHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string GetUserDesktopPDATestFolderPath()
        {
            string userDesktopPDATest = TextUtils.ExpandEnviromentVariable(@"%USERPROFILE%\\Desktop\\PDATest");
            string deviceRelPathData = ConfigurationManager.AppSettings.Get("DEVICE_RELPATH_DATA");
            string desktopDirectory = userDesktopPDATest + deviceRelPathData;
            FileUtils.VerifyFoldersOrCreate(desktopDirectory);
            return desktopDirectory;
        }

        public string GetName()
        {
            return "DesktopAdapter";
        }

        public bool IsDeviceConnected()
        {
            return true;
        }

        public DeviceResultName CopyDeviceFileToAppData(string sourceDirectory, string filename)
        {
            if (FileUtils.VerifyIfExitsFile(sourceDirectory+filename))
            {
                string clientPathData = ConfigurationManager.AppSettings.Get("CLIENT_PATH_DATA");
                logger.Debug("obteniendo archivo desde: " + sourceDirectory + filename);
                logger.Debug("copiando hacia la ruta: " + clientPathData + filename);
                FileUtils.CopyFile(sourceDirectory + filename, clientPathData + filename);
                if(FileUtils.VerifyIfExitsFile(clientPathData + filename))
                {
                    return DeviceResultName.OK;
                }
                else
                {
                    return DeviceResultName.FILE_DOWNLOAD_NOTOK;
                }
            }
            return DeviceResultName.NONEXISTENT_FILE;
        }

        public string ReadAdjustmentsDataFile(string desDir, string filename)
        {
            string userDesktopPDATestFolderPath = GetUserDesktopPDATestFolderPath();
            if(FileUtils.VerifyIfExitsFile(userDesktopPDATestFolderPath + filename))
            {
                string ajustes = FileUtils.ReadFile(userDesktopPDATestFolderPath + filename);
                return TextUtils.ParseAdjustmentDAT2JsonStr(ajustes);
            }
            else
            {
                return null;
            }
        }
    }
}
