using log4net;
using PDADesktop.Classes.Utils;
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

        public DeviceResultName CopyDeviceFileToPublicData(string sourceDirectory, string filenameAndExtension)
        {
            if (FileUtils.VerifyIfExitsFile(sourceDirectory+filenameAndExtension))
            {
                string clientPathData = ConfigurationManager.AppSettings.Get(Constants.CLIENT_PATH_DATA);
                logger.Debug("obteniendo archivo desde dispositivo: " + sourceDirectory + filenameAndExtension);
                logger.Debug("copiando hacia la ruta public: " + clientPathData + filenameAndExtension);
                FileUtils.CopyFile(sourceDirectory + filenameAndExtension, clientPathData + filenameAndExtension);
                if(FileUtils.VerifyIfExitsFile(clientPathData + filenameAndExtension))
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

        public DeviceResultName CopyPublicDataFileToDevice(string destinationDirectory, string filenameAndExtension)
        {
            string fileRelPath = ConfigurationManager.AppSettings.Get(Constants.CLIENT_PATH_DATA);
            string desDirExpanded = TextUtils.ExpandEnviromentVariable(fileRelPath);
            logger.Debug("obteniendo archivo desde public: " + desDirExpanded);
            FileUtils.VerifyFoldersOrCreate(desDirExpanded);
            string desktopFolder = ConfigurationManager.AppSettings.Get(Constants.DESKTOP_FOLDER);
            string desktopFolderExtended = TextUtils.ExpandEnviromentVariable(desktopFolder);
            string clientPathData = desktopFolderExtended + destinationDirectory + filenameAndExtension;
            logger.Debug("copiando hacia la ruta: " + clientPathData);
            FileUtils.CopyFile(desDirExpanded + filenameAndExtension, clientPathData);
            return DeviceResultName.OK;
        }

        public string getVersionProgramFileFromDevice()
        {
            string fileDefaultDat = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_DEFAULT);
            string deviceRelPathVersion = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_VERSION);
            string userDesktopPdaTestFolderPath = GetUserDesktopPDATestFolderPath(deviceRelPathVersion);
            if (!FileUtils.VerifyIfExitsFile(userDesktopPdaTestFolderPath + fileDefaultDat))
            {
                return null;
            }
            string clientPathVersion = ConfigurationManager.AppSettings.Get(Constants.CLIENT_PATH_VERSION);
            string clientPathVersionExtended = TextUtils.ExpandEnviromentVariable(clientPathVersion);

            FileUtils.VerifyFoldersOrCreate(clientPathVersionExtended);
            FileUtils.CopyFile(userDesktopPdaTestFolderPath + fileDefaultDat, clientPathVersionExtended + fileDefaultDat);
            string contentDefault = FileUtils.ReadFile(clientPathVersionExtended + fileDefaultDat);
            return TextUtils.getVersionFromDefaultDat(contentDefault);
        }

        public void CreateDefaultDataFile(string idSucursal)
        {
            string fileDefaultDat = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_DEFAULT);
            logger.Debug("Creando el archivo: " + fileDefaultDat);
            string deviceRelPathVersion = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_VERSION);
            string userDesktopPdaTestFolderPath = GetUserDesktopPDATestFolderPath(deviceRelPathVersion);
            FileUtils.VerifyFoldersOrCreate(userDesktopPdaTestFolderPath);
            string pdaControl = getNewDefaultDatacontent();
            logger.Debug("Guardando en: " + userDesktopPdaTestFolderPath + fileDefaultDat);
            FileUtils.WriteFile(userDesktopPdaTestFolderPath + fileDefaultDat, pdaControl);
            logger.Debug("Resultado de crear archivo: " + FileUtils.VerifyIfExitsFile(userDesktopPdaTestFolderPath + fileDefaultDat));
        }

        public string getLastVersionProgramFileFromServer()
        {
            string urlLastVersion = ConfigurationManager.AppSettings.Get(Constants.API_GET_LAST_VERSION_FILE_PROGRAM);
            string programFilename = ConfigurationManager.AppSettings.Get("DEVICE_RELPATH_FILENAME");
            string queryParameters = "?nombreDispositivo=" + Constants.DESKTOP + "&nombreArchivoPrograma=" + programFilename;
            return HttpWebClient.SendHttpGetRequest(urlLastVersion + queryParameters);
        }
        public string getNewDefaultDatacontent()
        {
            return "0|0|yyyyMMddHHmmss|sucursal|" + getLastVersionProgramFileFromServer() + "|0";
        }

        public string ReadAdjustmentsDataFile()
        {
            string deviceRelPathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
            string userDesktopPDATestFolderPath = GetUserDesktopPDATestFolderPath(deviceRelPathData);
            string filenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_DEFAULT);
            if(FileUtils.VerifyIfExitsFile(userDesktopPDATestFolderPath + filenameAndExtension))
            {
                string ajustes = FileUtils.ReadFile(userDesktopPDATestFolderPath + filenameAndExtension);
                return TextUtils.ParseAdjustmentDAT2JsonStr(ajustes);
            }
            else
            {
                return null;
            }
        }

        public bool OverWriteAdjustmentMade(string newContent)
        {
            string filenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_AJUSTES);
            string clientPathData = ConfigurationManager.AppSettings.Get(Constants.CLIENT_PATH_DATA);
            string clientPathDataExtended = TextUtils.ExpandEnviromentVariable(clientPathData);
            FileUtils.VerifyFoldersOrCreate(clientPathDataExtended);
            if (FileUtils.VerifyIfExitsFile(clientPathDataExtended + filenameAndExtension))
            {
                FileUtils.WriteFile(clientPathDataExtended + filenameAndExtension, newContent);
                CopyPublicDataFileToDevice(clientPathData, filenameAndExtension);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
