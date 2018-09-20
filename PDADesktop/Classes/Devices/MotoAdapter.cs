﻿using log4net;
using PDADesktop.Utils;
using System.Configuration;

namespace PDADesktop.Classes.Devices
{
    class MotoAdapter : IDeviceHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string GetName()
        {
            return "MotoAdapter";
        }

        public bool IsDeviceConnected()
        {
            return MotoApi.isDeviceConnected() != 0;
        }

        public DeviceResultName CopyDeviceFileToAppData(string sourceDirectory, string filename)
        {
            string fileRelPath = sourceDirectory + filename;
            logger.Debug("obteniendo archivo desde: " + fileRelPath);
            string clientPathData = ConfigurationManager.AppSettings.Get("CLIENT_PATH_DATA");
            string desDirExpanded = TextUtils.ExpandEnviromentVariable(clientPathData);
            logger.Debug("copiando hacia la ruta: " + desDirExpanded);
            FileUtils.VerifyFoldersOrCreate(desDirExpanded);
            int codigoResultado = MotoApi.downloadFileFromAppData(fileRelPath, desDirExpanded + filename);
            return getResult(codigoResultado);
        }

        public void CreateDefaultDataFile()
        {
            string fileDefaultDat = ConfigurationManager.AppSettings.Get("DEVICE_FILE_DEFAULT");
            logger.Debug("Creando el archivo: " + fileDefaultDat);
            string deviceRelPathData = ConfigurationManager.AppSettings.Get("DEVICE_RELPATH_VERSION");
            string userPDADocumentFolder = ConfigurationManager.AppSettings.Get("CLIENT_PATH_DATA");
            string userPDADocumenteFolderExtended = TextUtils.ExpandEnviromentVariable(userPDADocumentFolder);
            string pdaControl = "0|0|0000000000000|0|0.0.0|0|0";
            logger.Debug("Guardando temporalmente en: " + userPDADocumenteFolderExtended);
            FileUtils.WriteFile(userPDADocumenteFolderExtended + fileDefaultDat, pdaControl);
            int copyResult = MotoApi.copyFileToProgramFiles(userPDADocumenteFolderExtended + fileDefaultDat, deviceRelPathData);
            logger.Debug("Moviendo al dispositivo: " + deviceRelPathData);
            logger.Debug("Resultado obtenido: " + getResult(copyResult));
        }

        public string ReadAdjustmentsDataFile(string destinationDirectory, string filename)
        {
            string deviceRelativePathData = ConfigurationManager.AppSettings.Get("DEVICE_RELPATH_DATA");
            DeviceResultName copyfileResult = CopyDeviceFileToAppData(deviceRelativePathData, filename);
            if (copyfileResult.Equals(DeviceCodeResult.OK))
            {
                string desDirExpanded = TextUtils.ExpandEnviromentVariable(destinationDirectory);
                string adjustments = FileUtils.ReadFile(desDirExpanded + filename);
                return TextUtils.ParseAdjustmentDAT2JsonStr(adjustments);
            }
            else
            {
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
