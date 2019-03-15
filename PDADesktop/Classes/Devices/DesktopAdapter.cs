using log4net;
using PDADesktop.Classes.Utils;
using PDADesktop.Model;
using PDADesktop.Model.Dto;
using System;
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

        public string GetNameToDisplay()
        {
            return Environment.MachineName;
        }

        public string GetAdapterName()
        {
            return Constants.DESKTOP_ADAPTER;
        }

        public bool IsDeviceConnected()
        {
            return true;
        }

        public ResultFileOperation CopyFromDeviceToPublicFolder(string filename, string deviceFolder, string publicFolder)
        {
            return ResultFileOperation.UNKNOWN;
        }

        public ResultFileOperation CopyDeviceFileToPublicLookUp(string filenameAndExtension)
        {
            string desktopFolder = ConfigurationManager.AppSettings.Get(Constants.DESKTOP_FOLDER);
            string desktopFolderExtended = TextUtils.ExpandEnviromentVariable(desktopFolder);
            string deviceRelPathLookup = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_LOOKUP);
            string sourceDirectory = desktopFolderExtended + deviceRelPathLookup;
            if (FileUtils.VerifyIfExitsFile(sourceDirectory + filenameAndExtension))
            {
                string clientPathLookup = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_LOOKUP);
                string clientPathLookupExtended = TextUtils.ExpandEnviromentVariable(clientPathLookup);
                logger.Debug("obteniendo archivo desde dispositivo: " + sourceDirectory + filenameAndExtension);
                logger.Debug("copiando hacia la ruta public: " + clientPathLookupExtended + filenameAndExtension);
                FileUtils.CopyFile(sourceDirectory + filenameAndExtension, clientPathLookupExtended + filenameAndExtension);
                if (FileUtils.VerifyIfExitsFile(clientPathLookupExtended + filenameAndExtension))
                {
                    return ResultFileOperation.OK;
                }
                else
                {
                    return ResultFileOperation.FILE_DOWNLOAD_NOTOK;
                }
            }
            return ResultFileOperation.NONEXISTENT_FILE;
        }

        public ResultFileOperation CopyDeviceFileToPublicData(string filenameAndExtension)
        {
            string desktopFolder = ConfigurationManager.AppSettings.Get(Constants.DESKTOP_FOLDER);
            string desktopFolderExtended = TextUtils.ExpandEnviromentVariable(desktopFolder);
            string deviceRelPathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
            string sourceDirectory = desktopFolderExtended + deviceRelPathData;
            if (FileUtils.VerifyIfExitsFile(sourceDirectory + filenameAndExtension))
            {
                string clientPathData = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);
                string clientPathDataExtended = TextUtils.ExpandEnviromentVariable(clientPathData);
                logger.Debug("obteniendo archivo desde dispositivo: " + sourceDirectory + filenameAndExtension);
                logger.Debug("copiando hacia la ruta public: " + clientPathDataExtended + filenameAndExtension);
                FileUtils.CopyFile(sourceDirectory + filenameAndExtension, clientPathDataExtended + filenameAndExtension);
                if(FileUtils.VerifyIfExitsFile(clientPathDataExtended + filenameAndExtension))
                {
                    return ResultFileOperation.OK;
                }
                else
                {
                    return ResultFileOperation.FILE_DOWNLOAD_NOTOK;
                }
            }
            return ResultFileOperation.NONEXISTENT_FILE;
        }

        public ResultFileOperation CopyPublicRootFileToDevice(string destinationDirectory, string filenameAndExtension)
        {
            return ResultFileOperation.UNKNOWN;
        }

        public ResultFileOperation CopyPublicBinFileToDevice(string destinationDirectory, string filenameAndExtension)
        {
            string publicPathBin = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_BIN);
            string publicPathBinExtended = TextUtils.ExpandEnviromentVariable(publicPathBin);
            return CopyPublicFileToDevice(publicPathBinExtended, destinationDirectory, filenameAndExtension);
        }

        public ResultFileOperation CopyPublicDataFileToDevice(string destinationDirectory, string filenameAndExtension)
        {
            string publicPathData = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);
            string publicPathDataExtended = TextUtils.ExpandEnviromentVariable(publicPathData);
            return CopyPublicFileToDevice(publicPathDataExtended, destinationDirectory, filenameAndExtension);
        }

        public ResultFileOperation CopyPublicLookUpFileToDevice(string destinationDirectory, string filenameAndExtension)
        {
            string publicPathLookup = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_LOOKUP);
            string publicPathLookupExtended = TextUtils.ExpandEnviromentVariable(publicPathLookup);
            return CopyPublicFileToDevice(publicPathLookupExtended, destinationDirectory, filenameAndExtension);
        }

        private ResultFileOperation CopyPublicFileToDevice(string publicPathExtended, string destinationDirectory, string filenameAndExtension)
        {
            logger.Debug("obteniendo archivo desde public: " + publicPathExtended);
            FileUtils.VerifyFoldersOrCreate(publicPathExtended);
            string desktopFolder = ConfigurationManager.AppSettings.Get(Constants.DESKTOP_FOLDER);
            string desktopFolderExtended = TextUtils.ExpandEnviromentVariable(desktopFolder);
            string clientPathData = desktopFolderExtended + destinationDirectory + filenameAndExtension;
            logger.Debug("copiando hacia la ruta: " + clientPathData);
            FileUtils.CopyFile(publicPathExtended + filenameAndExtension, clientPathData);
            return ResultFileOperation.OK;
        }

        public ResultFileOperation DeleteDeviceDataFile(string filename)
        {
            string publicFolder = ConfigurationManager.AppSettings.Get(Constants.DESKTOP_FOLDER);
            string publicFolderExtended = TextUtils.ExpandEnviromentVariable(publicFolder);
            string deviceRelPathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
            string fileToDelete = publicFolderExtended + deviceRelPathData + FileUtils.WrapSlashAndDATExtension(filename);
            logger.Debug("obteniendo archivo desde public: " + fileToDelete);
            bool verificationPreviousExistenceFile = FileUtils.VerifyIfExitsFile(fileToDelete);
            if (!verificationPreviousExistenceFile)
            {
                logger.Debug("Archivo solicitado para borrar no existe");
                return ResultFileOperation.NONEXISTENT_FILE;
            }
            FileUtils.DeleteFile(fileToDelete);
            bool verifySuccessfullDeleteFile = FileUtils.VerifyIfExitsFile(fileToDelete);
            if(verifySuccessfullDeleteFile)
            {
                return ResultFileOperation.DELETE_ERROR;
            }
            else
            {
                return ResultFileOperation.OK;
            }
        }

        public ResultFileOperation DeletePublicDataFile(string filename)
        {
            string publicFolderPath = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);
            string publicFolderPathExtended = TextUtils.ExpandEnviromentVariable(publicFolderPath);
            string fileToDelete = publicFolderPathExtended + FileUtils.WrapSlashAndDATExtension(filename);
            logger.Debug("obteniendo archivo desde public: " + fileToDelete);
            bool verificationPreviousInexistenceFile = FileUtils.VerifyIfExitsFile(fileToDelete);
            if(!verificationPreviousInexistenceFile)
            {
                logger.Debug("Archivo solicitado para borrar no existe");
                return ResultFileOperation.NONEXISTENT_FILE;
            }
            logger.Debug("borrando el siguiente archivo: " + fileToDelete);
            FileUtils.DeleteFile(fileToDelete);
            bool verificationExistenceFile = FileUtils.VerifyIfExitsFile(fileToDelete);
            if(verificationExistenceFile)
            {
                logger.Info("No se pudo borrar el archivo " + filename );
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
            if (contentDefault != null)
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
            string deviceRelPathVersion = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_LOOKUP);
            string userDesktopPdaTestFolderPath = GetUserDesktopPDATestFolderPath(deviceRelPathVersion);
            FileUtils.VerifyFoldersOrCreate(userDesktopPdaTestFolderPath);
            string pdaControl = GetNewDefaultDataContent();
            logger.Debug("Guardando en: " + userDesktopPdaTestFolderPath + fileDefaultDat);
            FileUtils.WriteFile(userDesktopPdaTestFolderPath + fileDefaultDat, pdaControl);
            logger.Debug("Resultado de crear archivo: " + FileUtils.VerifyIfExitsFile(userDesktopPdaTestFolderPath + fileDefaultDat));
        }

        public string GetLastVersionProgramFileFromServer()
        {
            string programFilename = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_FILENAME);
            string queryParams = "?nombreDispositivo=" + Constants.DESKTOP + "&nombreArchivoPrograma=" + programFilename;
            return HttpWebClientUtil.GetLastVersionProgramFileFromServer(queryParams);
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

        public string ReadPriceControlDataFile()
        {
            return null;
        }

        public bool OverWritePriceControlMade(string newContent)
        {
            return false;
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
            string clientPathData = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);
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

        public string ReadReceptionDataFile()
        {
            return null;
        }
        public bool OverWriteReceptionMade(string newContent)
        {
            return false;
        }
        public string ReadLabelDataFile()
        {
            return null;
        }
        public bool OverWriteLabelMade(string newContent)
        {
            return false;
        }

    }
}
