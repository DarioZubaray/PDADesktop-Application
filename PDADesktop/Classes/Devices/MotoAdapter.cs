using log4net;
using PDADesktop.Utils;
using System.Configuration;

namespace PDADesktop.Classes.Devices
{
    class MotoAdapter : IDeviceHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool isDeviceConnected()
        {
            return MotoApi.isDeviceConnected() != 0;
        }

        public string ReadAjustesDataFile(string desDir, string filename)
        {
            string deviceRelativePathData = ConfigurationManager.AppSettings.Get("DEVICE_RELPATH_DATA");
            int moverArchivo = CopyDeviceFile2AppData(deviceRelativePathData, desDir, filename);
            DeviceResultNames result = getResult(moverArchivo);
            if (result.Equals(DeviceCodeResult.OK))
            {
                string desDirExpanded = TextUtils.ExpandEnviromentVariable(desDir);
                string ajustes = FileUtils.ReadFile(desDirExpanded + filename);
                return TextUtils.ParseAjusteDAT2Json(ajustes);
            }
            else
            {
                return null;
            }
        }

        public static int CopyDeviceFile2AppData(string sourceDir, string destinationDir, string filename)
        {
            string fileRelPath = sourceDir + filename;
            logger.Debug("obteniendo archivo desde: " + fileRelPath);
            string desDirExpanded = TextUtils.ExpandEnviromentVariable(destinationDir);
            logger.Debug("copiando hacia la ruta: " + desDirExpanded);
            FileUtils.VerifyFoldersOrCreate(desDirExpanded);
            int codigoResultado = MotoApi.downloadFileFromAppData(fileRelPath, desDirExpanded + filename);
            return codigoResultado;
        }

        public static DeviceResultNames getResult(int intResult)
        {
            DeviceResultNames result;
            switch (intResult)
            {

                case DeviceCodeResult.OK:
                    result = DeviceResultNames.OK;
                    break;

                case DeviceCodeResult.ERROR_INIT:
                    result = DeviceResultNames.CONNECTION_ERROR;
                    break;

                case DeviceCodeResult.ERROR_FILENOTEXISTS:
                    result = DeviceResultNames.NONEXISTENT_FILE;
                    break;

                case DeviceCodeResult.ERROR_NOTAFILE:
                    result = DeviceResultNames.NONEXISTENT_FILE;
                    break;

                case (int)DeviceCodeResult.ERROR_FILEOPENING:
                    result = DeviceResultNames.UNKNOWN;
                    break;

                case DeviceCodeResult.ERROR_FILEWRITING:
                    result = DeviceResultNames.UNKNOWN;
                    break;

                case DeviceCodeResult.ERROR_FILEREADING:
                    result = DeviceResultNames.UNKNOWN;
                    break;

                case DeviceCodeResult.ERROR_DELETE:
                    result = DeviceResultNames.DELETE_ERROR;
                    break;

                case DeviceCodeResult.ERROR_UNKNOWN:
                    result = DeviceResultNames.UNKNOWN;
                    break;
                default:
                    result = DeviceResultNames.UNKNOWN;
                    break;
            }

            return result;
        }
    }
}
