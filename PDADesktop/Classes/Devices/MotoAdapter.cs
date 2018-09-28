using log4net;
using PDADesktop.Utils;
using System.Configuration;

namespace PDADesktop.Classes.Devices
{
    class MotoAdapter : IDeviceHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string GetName()
        {
            return Constants.MOTO_ADAPTER;
        }

        public bool IsDeviceConnected()
        {
            return MotoApi.isDeviceConnected() != 0;
        }

        public DeviceResultName CopyDeviceFileToAppData(string sourceDirectory, string filenameAndExtension)
        {
            string fileRelPath = sourceDirectory + filenameAndExtension;
            logger.Debug("obteniendo archivo desde dispositivo: " + fileRelPath);
            string clientPathData = ConfigurationManager.AppSettings.Get(Constants.CLIENT_PATH_DATA);
            string desDirExpanded = TextUtils.ExpandEnviromentVariable(clientPathData);
            logger.Debug("copiando hacia la ruta: " + desDirExpanded);
            FileUtils.VerifyFoldersOrCreate(desDirExpanded);
            int codigoResultado = MotoApi.downloadFileFromAppData(fileRelPath, desDirExpanded + filenameAndExtension);
            return getResult(codigoResultado);
        }

        public DeviceResultName CopyAppDataFileToDevice(string relativeDestinationDirectory, string filenameAndExtension)
        {
            string publicPDADataFolder = ConfigurationManager.AppSettings.Get(Constants.CLIENT_PATH_DATA);
            string publicPDADataFolderExtended = TextUtils.ExpandEnviromentVariable(publicPDADataFolder);
            logger.Debug("obteniendo archivo desde public: " + publicPDADataFolderExtended + filenameAndExtension);
            FileUtils.VerifyFoldersOrCreate(publicPDADataFolderExtended);
            logger.Debug("copiando hacia la ruta: " + relativeDestinationDirectory);
            int codigoResultado = MotoApi.copyFileToAppData(publicPDADataFolderExtended + filenameAndExtension, relativeDestinationDirectory);
            return getResult(codigoResultado);
        }

        public void CreateDefaultDataFile()
        {
            string fileDefaultDat = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_DEFAULT);
            logger.Debug("Creando el archivo: " + fileDefaultDat);
            string deviceRelPathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_VERSION);
            string userPDADocumentFolder = ConfigurationManager.AppSettings.Get(Constants.CLIENT_PATH_VERSION);
            string userPDADocumenteFolderExtended = TextUtils.ExpandEnviromentVariable(userPDADocumentFolder);
            FileUtils.VerifyFoldersOrCreate(userPDADocumenteFolderExtended);
            string pdaControl = getDefaultDatacontent();
            logger.Debug("Guardando temporalmente en: " + userPDADocumenteFolderExtended);
            FileUtils.WriteFile(userPDADocumenteFolderExtended + fileDefaultDat, pdaControl);
            int copyResult = MotoApi.copyFileToProgramFiles(userPDADocumenteFolderExtended + fileDefaultDat, deviceRelPathData);
            logger.Debug("Moviendo al dispositivo: " + deviceRelPathData);
            logger.Debug("Resultado obtenido: " + getResult(copyResult));
        }

        public string getDefaultDatacontent()
        {
            string urlLastVersion = ConfigurationManager.AppSettings.Get(Constants.API_GET_LAST_VERSION_FILE_PROGRAM);
            string programFilename = ConfigurationManager.AppSettings.Get("DEVICE_RELPATH_FILENAME");
            string queryParameters = "?nombreDispositivo="+Constants.MOTO+"&nombreArchivoPrograma="+ programFilename;
            return HttpWebClient.sendHttpGetRequest(urlLastVersion+queryParameters);
        }

        public string ReadDefaultDataFile()
        {
            string filenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_DEFAULT);
            logger.Debug("Leyendo el archivo: " + filenameAndExtension);
            string deviceRelativePathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_VERSION);

            DeviceResultName copyfileResult = CopyDeviceFileToAppData(deviceRelativePathData, filenameAndExtension);
            logger.Debug("resultado de copiar el default.dat: " + copyfileResult.ToString());
            if (copyfileResult.Equals(DeviceResultName.NONEXISTENT_FILE))
            {
                CreateDefaultDataFile();
                CopyDeviceFileToAppData(deviceRelativePathData, filenameAndExtension);
            }
            string userPublicFolder = ConfigurationManager.AppSettings.Get(Constants.CLIENT_PATH_VERSION);
            string userPublicFolderExtended = TextUtils.ExpandEnviromentVariable(userPublicFolder);
            FileUtils.VerifyFoldersOrCreate(userPublicFolderExtended);

            string contentDefault = FileUtils.ReadFile(userPublicFolderExtended+ filenameAndExtension);
            return contentDefault;
        }

        public string ReadAdjustmentsDataFile()
        {
            string filenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_AJUSTES);
            string deviceRelativePathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
            DeviceResultName copyfileResult = CopyDeviceFileToAppData(deviceRelativePathData, filenameAndExtension);
            if (copyfileResult.Equals(DeviceResultName.OK))
            {
                string destinationDirectory = ConfigurationManager.AppSettings.Get(Constants.CLIENT_PATH_DATA);
                string destinationDirectoryExpanded = TextUtils.ExpandEnviromentVariable(destinationDirectory);
                string adjustments = FileUtils.ReadFile(destinationDirectoryExpanded + filenameAndExtension);
                return TextUtils.ParseAdjustmentDAT2JsonStr(adjustments);
            }
            else
            {
                logger.Debug("No existe archivo: " + filenameAndExtension + ", en: " + deviceRelativePathData);
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
                string deviceRelPathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
                CopyAppDataFileToDevice(deviceRelPathData, filenameAndExtension);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static DeviceResultName getResult(int intResult)
        {
            DeviceResultName result;
            switch (intResult)
            {

                case DeviceCodeResult.OK:
                    result = DeviceResultName.OK;
                    break;

                case DeviceCodeResult.ERROR_INIT:
                    result = DeviceResultName.CONNECTION_ERROR;
                    break;

                case DeviceCodeResult.ERROR_FILENOTEXISTS:
                    result = DeviceResultName.NONEXISTENT_FILE;
                    break;

                case DeviceCodeResult.ERROR_NOTAFILE:
                    result = DeviceResultName.NONEXISTENT_FILE;
                    break;

                case (int)DeviceCodeResult.ERROR_FILEOPENING:
                    result = DeviceResultName.UNKNOWN;
                    break;

                case DeviceCodeResult.ERROR_FILEWRITING:
                    result = DeviceResultName.UNKNOWN;
                    break;

                case DeviceCodeResult.ERROR_FILEREADING:
                    result = DeviceResultName.UNKNOWN;
                    break;

                case DeviceCodeResult.ERROR_DELETE:
                    result = DeviceResultName.DELETE_ERROR;
                    break;

                case DeviceCodeResult.ERROR_UNKNOWN:
                    result = DeviceResultName.UNKNOWN;
                    break;
                default:
                    result = DeviceResultName.UNKNOWN;
                    break;
            }

            return result;
        }
    }
}
