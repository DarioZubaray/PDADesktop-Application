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

        public DeviceResultName CopyAppDataFileToDevice(string destinationDirectory, string filenameAndExtension)
        {
            string fileRelPath = ConfigurationManager.AppSettings.Get(Constants.CLIENT_PATH_DATA);
            string desDirExpanded = TextUtils.ExpandEnviromentVariable(fileRelPath);
            logger.Debug("obteniendo archivo desde public: " + desDirExpanded + filenameAndExtension);
            FileUtils.VerifyFoldersOrCreate(desDirExpanded);
            string clientPathData = destinationDirectory;
            logger.Debug("copiando hacia la ruta: " + clientPathData);
            int codigoResultado = MotoApi.copyFileToAppData(desDirExpanded + filenameAndExtension, clientPathData);
            return getResult(codigoResultado);
        }

        public void CreateDefaultDataFile()
        {
            string fileDefaultDat = ConfigurationManager.AppSettings.Get(Constants.DEVICE_FILE_DEFAULT);
            logger.Debug("Creando el archivo: " + fileDefaultDat);
            string deviceRelPathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_VERSION);
            string userPDADocumentFolder = ConfigurationManager.AppSettings.Get(Constants.CLIENT_PATH_DATA);
            string userPDADocumenteFolderExtended = TextUtils.ExpandEnviromentVariable(userPDADocumentFolder);
            string pdaControl = "0|0|0000000000000|0|0.0.0|0|0";
            logger.Debug("Guardando temporalmente en: " + userPDADocumenteFolderExtended);
            FileUtils.WriteFile(userPDADocumenteFolderExtended + fileDefaultDat, pdaControl);
            int copyResult = MotoApi.copyFileToProgramFiles(userPDADocumenteFolderExtended + fileDefaultDat, deviceRelPathData);
            logger.Debug("Moviendo al dispositivo: " + deviceRelPathData);
            logger.Debug("Resultado obtenido: " + getResult(copyResult));
        }

        public string ReadDefaultDataFile()
        {
            return "";
        }

        public string ReadAdjustmentsDataFile(string destinationDirectory, string filenameAndExtension)
        {
            string deviceRelativePathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
            DeviceResultName copyfileResult = CopyDeviceFileToAppData(deviceRelativePathData, filenameAndExtension);
            if (copyfileResult.Equals(DeviceResultName.OK))
            {
                string desDirExpanded = TextUtils.ExpandEnviromentVariable(destinationDirectory);
                string adjustments = FileUtils.ReadFile(desDirExpanded + filenameAndExtension);
                return TextUtils.ParseAdjustmentDAT2JsonStr(adjustments);
            }
            else
            {
                logger.Debug("No existe archivo: " + filenameAndExtension + ", en: " + deviceRelativePathData);
                return null;
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
