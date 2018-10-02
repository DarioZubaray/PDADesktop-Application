using log4net;
using PDADesktop.Classes.Utils;
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

        public ResultFileOperation CopyDeviceFileToPublicData(string sourceDirectory, string filenameAndExtension)
        {
            string fileRelPath = sourceDirectory + filenameAndExtension;
            logger.Debug("obteniendo archivo desde dispositivo: " + fileRelPath);
            string clientPathData = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);
            string desDirExpanded = TextUtils.ExpandEnviromentVariable(clientPathData);
            logger.Debug("copiando hacia la ruta: " + desDirExpanded);
            FileUtils.VerifyFoldersOrCreate(desDirExpanded);
            int codigoResultado = MotoApi.downloadFileFromAppData(fileRelPath, desDirExpanded + filenameAndExtension);
            return getResult(codigoResultado);
        }

        public ResultFileOperation CopyPublicDataFileToDevice(string relativeDestinationDirectory, string filenameAndExtension)
        {
            string publicPDADataFolder = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);
            string publicPDADataFolderExtended = TextUtils.ExpandEnviromentVariable(publicPDADataFolder);
            logger.Debug("obteniendo archivo desde public: " + publicPDADataFolderExtended + filenameAndExtension);
            FileUtils.VerifyFoldersOrCreate(publicPDADataFolderExtended);
            logger.Debug("copiando hacia la ruta: " + relativeDestinationDirectory);
            int codigoResultado = MotoApi.copyFileToAppData(publicPDADataFolderExtended + filenameAndExtension, relativeDestinationDirectory);
            return getResult(codigoResultado);
        }

        public ResultFileOperation DeleteDeviceDataFile(string filename)
        {
            string deviceRelpathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
            string fileToDelete = deviceRelpathData + FileUtils.WrapSlashAndDATExtension(filename);
            logger.Debug("obteniendo archivo desde public: " + fileToDelete);
            int motoApiResult = MotoApi.deleteFileFromAppData(fileToDelete);
            return getResult(motoApiResult);
        }

        public ResultFileOperation DeletePublicDataFile(string filename)
        {
            string publicFolderPath = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);
            string publicFolderPathExtended = TextUtils.ExpandEnviromentVariable(publicFolderPath);
            string fileToDelete = publicFolderPathExtended + FileUtils.WrapSlashAndDATExtension(filename);
            logger.Debug("obteniendo archivo desde public: " + fileToDelete);
            bool verificationPreviousInexistenceFile = FileUtils.VerifyIfExitsFile(fileToDelete);
            if (!verificationPreviousInexistenceFile)
            {
                logger.Debug("Archivo solicitado para borrar no existe");
                return ResultFileOperation.NONEXISTENT_FILE;
            }
            logger.Debug("borrando el siguiente archivo: " + fileToDelete);
            FileUtils.DeleteFile(fileToDelete);
            bool verificationExistenceFile = FileUtils.VerifyIfExitsFile(fileToDelete);
            if (verificationExistenceFile)
            {
                logger.Info("No se pudo borrar el archivo " + filename);
                return ResultFileOperation.DELETE_ERROR;
            }
            else
            {
                return ResultFileOperation.OK;
            }
        }

        public string getVersionProgramFileFromDevice()
        {
            string filenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_DEFAULT);
            logger.Debug("Leyendo el archivo: " + filenameAndExtension);
            string deviceRelativePathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_VERSION);

            ResultFileOperation copyfileResult = CopyDeviceFileToPublicData(deviceRelativePathData, filenameAndExtension);
            logger.Debug("resultado de copiar el default.dat: " + copyfileResult.ToString());
            if (copyfileResult.Equals(ResultFileOperation.NONEXISTENT_FILE))
            {
                return null;
            }
            else
            {
                string userPublicFolder = ConfigurationManager.AppSettings.Get(Constants.CLIENT_PATH_VERSION);
                string userPublicFolderExtended = TextUtils.ExpandEnviromentVariable(userPublicFolder);
                FileUtils.VerifyFoldersOrCreate(userPublicFolderExtended);

                string contentDefault = FileUtils.ReadFile(userPublicFolderExtended + filenameAndExtension);
                return TextUtils.getVersionFromDefaultDat(contentDefault);
            }
        }

        public void CreateDefaultDataFile(string idSucursal)
        {
            string fileDefaultDat = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_DEFAULT);
            logger.Debug("Creando el archivo: " + fileDefaultDat);
            string deviceRelPathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_VERSION);
            string userPDADocumentFolder = ConfigurationManager.AppSettings.Get(Constants.CLIENT_PATH_VERSION);
            string userPDADocumenteFolderExtended = TextUtils.ExpandEnviromentVariable(userPDADocumentFolder);
            FileUtils.VerifyFoldersOrCreate(userPDADocumenteFolderExtended);
            string pdaControl = getNewDefaultDatacontent();
            logger.Debug("Guardando temporalmente en: " + userPDADocumenteFolderExtended);
            FileUtils.WriteFile(userPDADocumenteFolderExtended + fileDefaultDat, pdaControl);
            int copyResult = MotoApi.copyFileToProgramFiles(userPDADocumenteFolderExtended + fileDefaultDat, deviceRelPathData);
            logger.Debug("Moviendo al dispositivo: " + deviceRelPathData);
            logger.Debug("Resultado obtenido: " + getResult(copyResult));
        }
        public string getLastVersionProgramFileFromServer()
        {
            string urlLastVersion = ConfigurationManager.AppSettings.Get(Constants.API_GET_LAST_VERSION_FILE_PROGRAM);
            string programFilename = ConfigurationManager.AppSettings.Get("DEVICE_RELPATH_FILENAME");
            string queryParameters = "?nombreDispositivo=" + Constants.MOTO + "&nombreArchivoPrograma=" + programFilename;
            return HttpWebClient.SendHttpGetRequest(urlLastVersion + queryParameters);
        }
        public string getNewDefaultDatacontent()
        {
            return "0|0|yyyyMMddHHmmss|sucursal|"+ getLastVersionProgramFileFromServer() + "|0";
        }

        public string ReadAdjustmentsDataFile()
        {
            string filenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_AJUSTES);
            string deviceRelativePathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
            ResultFileOperation copyfileResult = CopyDeviceFileToPublicData(deviceRelativePathData, filenameAndExtension);
            if (copyfileResult.Equals(ResultFileOperation.OK))
            {
                string destinationDirectory = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);
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
            string clientPathData = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);
            string clientPathDataExtended = TextUtils.ExpandEnviromentVariable(clientPathData);
            FileUtils.VerifyFoldersOrCreate(clientPathDataExtended);
            if (FileUtils.VerifyIfExitsFile(clientPathDataExtended + filenameAndExtension))
            {
                FileUtils.WriteFile(clientPathDataExtended + filenameAndExtension, newContent);
                string deviceRelPathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
                CopyPublicDataFileToDevice(deviceRelPathData, filenameAndExtension);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static ResultFileOperation getResult(int intResult)
        {
            ResultFileOperation result;
            switch (intResult)
            {

                case FileOperationCode.OK:
                    result = ResultFileOperation.OK;
                    break;

                case FileOperationCode.ERROR_INIT:
                    result = ResultFileOperation.CONNECTION_ERROR;
                    break;

                case FileOperationCode.ERROR_FILENOTEXISTS:
                    result = ResultFileOperation.NONEXISTENT_FILE;
                    break;

                case FileOperationCode.ERROR_NOTAFILE:
                    result = ResultFileOperation.NONEXISTENT_FILE;
                    break;

                case (int)FileOperationCode.ERROR_FILEOPENING:
                    result = ResultFileOperation.UNKNOWN;
                    break;

                case FileOperationCode.ERROR_FILEWRITING:
                    result = ResultFileOperation.UNKNOWN;
                    break;

                case FileOperationCode.ERROR_FILEREADING:
                    result = ResultFileOperation.UNKNOWN;
                    break;

                case FileOperationCode.ERROR_DELETE:
                    result = ResultFileOperation.DELETE_ERROR;
                    break;

                case FileOperationCode.ERROR_UNKNOWN:
                    result = ResultFileOperation.UNKNOWN;
                    break;
                default:
                    result = ResultFileOperation.UNKNOWN;
                    break;
            }

            return result;
        }
    }
}
