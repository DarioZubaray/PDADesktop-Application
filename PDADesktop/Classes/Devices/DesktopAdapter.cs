using log4net;
using PDADesktop.Utils;
using System.Configuration;

namespace PDADesktop.Classes.Devices
{
    class DesktopAdapter : IDeviceHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string GetUserDesktopPDATestFolderPath(string deviceRelPath)
        {
            string desktopFolder = ConfigurationManager.AppSettings.Get(Constants.DESKTOP_FOLDER);
            string userDesktopPDATest = TextUtils.ExpandEnviromentVariable(desktopFolder);
            string desktopDirectory = userDesktopPDATest + deviceRelPath;
            FileUtils.VerifyFoldersOrCreate(desktopDirectory);
            return desktopDirectory;
        }

        public string GetName()
        {
            return Constants.DESKTOP_ADAPTER;
        }

        public bool IsDeviceConnected()
        {
            return true;
        }

        public DeviceResultName CopyDeviceFileToAppData(string sourceDirectory, string filename)
        {
            if (FileUtils.VerifyIfExitsFile(sourceDirectory+filename))
            {
                string clientPathData = ConfigurationManager.AppSettings.Get(Constants.CLIENT_PATH_DATA);
                logger.Debug("obteniendo archivo desde dispositivo: " + sourceDirectory + filename);
                logger.Debug("copiando hacia la ruta public: " + clientPathData + filename);
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

        public DeviceResultName CopyAppDataFileToDevice(string sourceDirectory, string filename)
        {
            string fileRelPath = ConfigurationManager.AppSettings.Get(Constants.CLIENT_PATH_DATA);
            string desDirExpanded = TextUtils.ExpandEnviromentVariable(fileRelPath);
            logger.Debug("obteniendo archivo desde public: " + desDirExpanded);
            FileUtils.VerifyFoldersOrCreate(desDirExpanded);
            string clientPathData = sourceDirectory + filename;
            logger.Debug("copiando hacia la ruta: " + clientPathData);
            FileUtils.CopyFile(desDirExpanded, clientPathData);
            return DeviceResultName.OK;
        }

        public void CreateDefaultDataFile()
        {
            string fileDefaultDat = ConfigurationManager.AppSettings.Get(Constants.DEVICE_FILE_DEFAULT);
            logger.Debug("Creando el archivo: " + fileDefaultDat);
            string deviceRelPathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_VERSION);
            string userDesktopPdaTestFolderPath = GetUserDesktopPDATestFolderPath(deviceRelPathData);
            string pdaControl = "0|0|0000000000000|0|0.0.0|0|0";
            logger.Debug("Guardando en: " + deviceRelPathData + fileDefaultDat);
            FileUtils.WriteFile(deviceRelPathData + fileDefaultDat, pdaControl);
            logger.Debug("Resultado de crear archivo: " + FileUtils.VerifyIfExitsFile(deviceRelPathData + fileDefaultDat));
        }

        public string ReadAdjustmentsDataFile(string desDir, string filename)
        {
            string deviceRelPathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
            string userDesktopPDATestFolderPath = GetUserDesktopPDATestFolderPath(deviceRelPathData);
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
