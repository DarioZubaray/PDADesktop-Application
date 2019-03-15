using log4net;
using PDADesktop.Classes.Utils;
using PDADesktop.Model;
using PDADesktop.Model.Dto;
using System.Configuration;

namespace PDADesktop.Classes.Devices
{
    class MotoAdapter : IDeviceHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string GetNameToDisplay()
        {
            return Constants.MOTOROLA_MC_2100;
        }

        public string GetAdapterName()
        {
            return Constants.MOTO_ADAPTER;
        }

        public bool IsDeviceConnected()
        {
            return MotoApi.isDeviceConnected() != 0;
        }

        public ResultFileOperation CopyFromDeviceToPublicFolder(string filename, string deviceFolder, string publicFolder)
        {
            logger.Debug("obteniendo archivo desde dispositivo: " + deviceFolder + filename);

            logger.Debug("copiando hacia la ruta: " + publicFolder + filename);
            FileUtils.VerifyFoldersOrCreate(publicFolder);
            int codigoResultado = MotoApi.downloadFileFromAppData(deviceFolder + filename, publicFolder + filename);
            return getResult(codigoResultado);
        }

        public ResultFileOperation CopyDeviceFileToPublicLookUp(string filenameAndExtension)
        {
            string deviceLookupRelPath = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_LOOKUP);
            logger.Debug("obteniendo archivo desde dispositivo: " + deviceLookupRelPath + filenameAndExtension);
            string publicPathLookup = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_LOOKUP);
            string publicPathLookupExtended = TextUtils.ExpandEnviromentVariable(publicPathLookup);
            logger.Debug("copiando hacia la ruta: " + publicPathLookupExtended + filenameAndExtension);
            FileUtils.VerifyFoldersOrCreate(publicPathLookupExtended);
            int codigoResultado = MotoApi.downloadFileFromAppData(deviceLookupRelPath + filenameAndExtension, publicPathLookupExtended + filenameAndExtension);
            return getResult(codigoResultado);
        }

        public ResultFileOperation CopyDeviceFileToPublicData(string filenameAndExtension)
        {
            string deviceRelPathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
            string fileRelPath = deviceRelPathData + filenameAndExtension;
            logger.Debug("obteniendo archivo desde dispositivo: " + fileRelPath);
            string clientPathData = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);
            string desDirExpanded = TextUtils.ExpandEnviromentVariable(clientPathData);
            logger.Debug("copiando hacia la ruta: " + desDirExpanded);
            FileUtils.VerifyFoldersOrCreate(desDirExpanded);
            int codigoResultado = MotoApi.downloadFileFromAppData(fileRelPath, desDirExpanded + filenameAndExtension);
            ResultFileOperation resultFile = getResult(codigoResultado);
            logger.Debug("resultado de copiado: " + resultFile.ToString());
            return resultFile;
        }

        public ResultFileOperation CopyPublicRootFileToDevice(string relativeDestinationDirectory, string filenameAndExtension)
        {
            string publicPDARootFolder = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_ROOT);
            string publicPDARootFolderExtended = TextUtils.ExpandEnviromentVariable(publicPDARootFolder);
            return CopyPublicFileToDevice(publicPDARootFolderExtended, relativeDestinationDirectory, filenameAndExtension);
        }

        public ResultFileOperation CopyPublicBinFileToDevice(string relativeDestinationDirectory, string filenameAndExtension)
        {
            string publicPDABinFolder = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_BIN);
            string publicPDABinFolderExtended = TextUtils.ExpandEnviromentVariable(publicPDABinFolder);
            return CopyPublicFileToDevice(publicPDABinFolderExtended, relativeDestinationDirectory, filenameAndExtension);
        }

        public ResultFileOperation CopyPublicDataFileToDevice(string relativeDestinationDirectory, string filenameAndExtension)
        {
            string publicPDADataFolder = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);
            string publicPDADataFolderExtended = TextUtils.ExpandEnviromentVariable(publicPDADataFolder);
            return CopyPublicFileToDevice(publicPDADataFolderExtended, relativeDestinationDirectory, filenameAndExtension);
        }

        public ResultFileOperation CopyPublicLookUpFileToDevice(string relativeDestinationDirectory, string filenameAndExtension)
        {
            string publicPDALookUpFolder = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_LOOKUP);
            string publicPDALookUpFolderExtended = TextUtils.ExpandEnviromentVariable(publicPDALookUpFolder);
            return CopyPublicFileToDevice(publicPDALookUpFolderExtended, relativeDestinationDirectory, filenameAndExtension);
        }

        private ResultFileOperation CopyPublicFileToDevice(string publicPathExtended, string relativeDestinationDirectory, string filenameAndExtension)
        {
            logger.Debug("obteniendo archivo desde public: " + publicPathExtended + filenameAndExtension);
            FileUtils.VerifyFoldersOrCreate(publicPathExtended);
            logger.Debug("copiando hacia la ruta: " + relativeDestinationDirectory);
            int codigoResultado = MotoApi.copyFileToAppData(publicPathExtended + filenameAndExtension, relativeDestinationDirectory);
            return getResult(codigoResultado);
        }

        public ResultFileOperation DeleteDeviceDataFile(string filename)
        {
            string deviceRelpathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
            string fileToDelete = deviceRelpathData + FileUtils.WrapSlashAndDATExtension(filename);
            logger.Debug("borrando archivo desde el dispositivo: " + fileToDelete);
            int motoApiResult = MotoApi.deleteFileFromAppData(fileToDelete);
            ResultFileOperation result = getResult(motoApiResult);
            logger.Debug("Resultado de borrar: " + result.ToString());
            return result;
        }

        public ResultFileOperation DeletePublicDataFile(string filename)
        {
            string publicFolderPath = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);
            string publicFolderPathExtended = TextUtils.ExpandEnviromentVariable(publicFolderPath);
            string fileToDelete = publicFolderPathExtended + FileUtils.WrapSlashAndDATExtension(filename);
            logger.Debug("verificando la existencia del archivo en: " + fileToDelete);
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

        public ResultFileOperation DeleteDeviceAndPublicDataFiles(string filename)
        {
            ResultFileOperation deleteDevice = DeleteDeviceDataFile(filename);
            ResultFileOperation deletePublic = DeletePublicDataFile(filename);
            return ResultFileOperation.OK;
        }

        private string ReadDefaultContentFromDefaultData()
        {
            string filenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_DEFAULT);

            string userPublicFolder = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_LOOKUP);
            string userPublicFolderExtended = TextUtils.ExpandEnviromentVariable(userPublicFolder);
            FileUtils.VerifyFoldersOrCreate(userPublicFolderExtended);
            logger.Debug("Leyendo el archivo: " + userPublicFolderExtended + filenameAndExtension);

            return FileUtils.ReadFile(userPublicFolderExtended + filenameAndExtension);
        }

        public string ReadSynchronizationStateFromDefaultData()
        {
            string contentDefault = ReadDefaultContentFromDefaultData();
            if(contentDefault != null)
            {
                return TextUtils.GetSynchronizationStateFromDefaultDat(contentDefault);
            }
            else
            {
                return null;
            }
        }

        public string ReadSynchronizationDateFromDefaultData()
        {
            string contentDefault = ReadDefaultContentFromDefaultData();
            if(contentDefault != null)
            {
                return TextUtils.GetSynchronizationDateFromDefaultDat(contentDefault);
            }
            else
            {
                return null;
            }
        }

        public string ReadStoreIdFromDefaultData()
        {
            string contentDefault = ReadDefaultContentFromDefaultData();
            if(contentDefault != null)
            {
                return TextUtils.GetStoreIdFromDefaultDat(contentDefault);
            }
            else
            {
                return null;
            }
        }

        public string ReadVersionDeviceProgramFileFromDefaultData()
        {
            string contentDefault = ReadDefaultContentFromDefaultData();
            if(contentDefault != null)
            {
                contentDefault = TextUtils.RemoveQuotesMarks(contentDefault);
                return TextUtils.GetVersionFromDefaultDat(contentDefault);
            }
            else
            {
                return null;
            }
        }

        public void CreateEmptyDefaultDataFile()
        {
            string fileDefaultDat = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_DEFAULT);
            logger.Debug("Creando el archivo: " + fileDefaultDat);
            string deviceRelPathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_LOOKUP);
            string userPDADocumentFolder = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_LOOKUP);
            string userPDADocumenteFolderExtended = TextUtils.ExpandEnviromentVariable(userPDADocumentFolder);
            FileUtils.VerifyFoldersOrCreate(userPDADocumenteFolderExtended);
            string pdaControl = GetNewDefaultDataContent();
            logger.Debug("Guardando temporalmente en: " + userPDADocumenteFolderExtended);
            FileUtils.WriteFile(userPDADocumenteFolderExtended + fileDefaultDat, pdaControl);
            int copyResult = MotoApi.copyFileToProgramFiles(userPDADocumenteFolderExtended + fileDefaultDat, deviceRelPathData);
            logger.Debug("Moviendo al dispositivo: " + deviceRelPathData);
            logger.Debug("Resultado obtenido: " + getResult(copyResult));
        }

        public string GetLastVersionProgramFileFromServer()
        {
            string programFilename = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_FILENAME);
            string queryParameters = "?nombreDispositivo=" + Constants.MOTO + "&nombreArchivoPrograma=" + programFilename;
            return HttpWebClientUtil.GetLastVersionProgramFileFromServer(queryParameters);
        }

        public string GetNewDefaultDataContent()
        {
            return "0|0|00000000000000|0|0.0.0.0|0";
        }

        public ActionResultDto ControlDeviceLock(long syncId, string storeId)
        {
            ActionResultDto desbloquearPDA = HttpWebClientUtil.ControlDeviceLock(syncId, storeId);
            return desbloquearPDA;
        }

        public void ChangeSynchronizationState(string syncState)
        {
            FileUtils.UpdateDefaultDatFileInPublic(syncState, DeviceMainData.POSITION_ESTADO_SINCRO);
            string slashFilenameAndextension = FileUtils.PrependSlash(Constants.DAT_FILE_DEFAULT);
            CopyPublicLookUpFileToDevice(Constants.DEVICE_RELPATH_LOOKUP, slashFilenameAndextension);
        }

        private string CopyAndGetContentFromFile(string filenameAndExtension)
        {
            ResultFileOperation copyfileResult = CopyDeviceFileToPublicData(filenameAndExtension);
            if (copyfileResult.Equals(ResultFileOperation.OK))
            {
                string destinationDirectory = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);
                string destinationDirectoryExpanded = TextUtils.ExpandEnviromentVariable(destinationDirectory);
                return FileUtils.ReadFile(destinationDirectoryExpanded + filenameAndExtension);
            }
            else
            {
                return null;
            }
        }
        public string ReadPriceControlDataFile()
        {
            string filenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_CONTROL_PRECIO);
            string priceControl = CopyAndGetContentFromFile(filenameAndExtension);
            if(priceControl != null)
            {
                return TextUtils.ParsePriceControlDAT2JsonStr(priceControl);
            }
            else
            {
                return null;
            }
        }

        public bool OverWritePriceControlMade(string newContent)
        {
            return false;
        }

        public string ReadAdjustmentsDataFile()
        {
            string filenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_AJUSTES);
            string adjustments = CopyAndGetContentFromFile(filenameAndExtension);
            if (adjustments != null)
            {
                return TextUtils.ParseAdjustmentDAT2JsonStr(adjustments);
            }
            else
            {
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
                ResultFileOperation result = CopyPublicDataFileToDevice(deviceRelPathData, filenameAndExtension);
                if(ResultFileOperation.OK.Equals(result))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public string ReadReceptionDataFile()
        {
            string filenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_RECEPCIONES);
            string receptions = CopyAndGetContentFromFile(filenameAndExtension);
            if (receptions != null)
            {
                return TextUtils.ParseReceptionsDAT2JsonStr(receptions);
            }
            else
            {
                return null;
            }
        }
        public bool OverWriteReceptionMade(string newContent)
        {
            return false;
        }
        public string ReadLabelDataFile()
        {
            string filenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_ETIQUETAS);
            string labels = CopyAndGetContentFromFile(filenameAndExtension);
            if (labels != null)
            {
                return TextUtils.ParseLabelsDAT2JsonStr(labels);
            }
            else
            {
                return null;
            }
        }
        public bool OverWriteLabelMade(string newContent)
        {
            return false;
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
