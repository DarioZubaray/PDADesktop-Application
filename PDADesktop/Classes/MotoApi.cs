using log4net;
using PDADesktop.Utils;
using System.Configuration;
using System.Runtime.InteropServices;

namespace PDADesktop.Classes
{
    class MotoApi
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string pdaMotoCommunication = @"lib\PDAMotoCommunication.dll";
        [DllImport(pdaMotoCommunication, CallingConvention = CallingConvention.Cdecl)]
        public static extern int isDeviceConnected();
        [DllImport(pdaMotoCommunication, CallingConvention = CallingConvention.Cdecl)]
        public static extern int copyFileToAppData(string source, string relativeDest);
        [DllImport(pdaMotoCommunication, CallingConvention = CallingConvention.Cdecl)]
        public static extern int copyFileToProgramFiles(string source, string relativeDest);
        [DllImport(pdaMotoCommunication, CallingConvention = CallingConvention.Cdecl)]
        public static extern int deleteFileFromAppData(string relativeFilePath);
        [DllImport(pdaMotoCommunication, CallingConvention = CallingConvention.Cdecl)]
        public static extern int deleteFileFromProgramFiles(string relativeFilePath);
        [DllImport(pdaMotoCommunication, CallingConvention = CallingConvention.Cdecl)]
        public static extern int downloadFileFromAppData(string relativeSrcPath, string absDestPath);
        [DllImport(pdaMotoCommunication, CallingConvention = CallingConvention.Cdecl)]
        public static extern int downloadFileFromProgramFiles(string relativeSrcPath, string absDestPath);
        [DllImport(pdaMotoCommunication, CallingConvention = CallingConvention.Cdecl)]
        public static extern int setTime(string time);

        public static string ReadDataFile(string desDir, string filename)
        {
            string versionRelPath = ConfigurationManager.AppSettings.Get("DEVICE_RELPATH_DATA");
            string fileRelPath = versionRelPath + filename;
            logger.Debug("descargando archivo desde: " + fileRelPath);
            string desDirExpanded = TextUtils.ExpandEnviromentVariable(desDir);
            logger.Debug("hacia la ruta: " + desDirExpanded);
            FileUtils.VerifyFolders(desDirExpanded);
            CodigoResultado result = getResult(MotoApi.downloadFileFromAppData(fileRelPath, desDirExpanded + filename));
            if (result.Equals(CodigoResultado.OK))
            {
                string ajustes = FileUtils.ReadFile(desDirExpanded + filename);
                return TextUtils.ParseAjusteDAT2Json(ajustes);
            }
            else
            {
                return null;
            }
        }

        private static class MotoResult
        {
            public const int OK = 1;
            public const int ERROR_UNKNOWN = 50;
            public const int ERROR_INIT = 51;
            public const int ERROR_FILENOTEXISTS = 52;
            public const int ERROR_NOTAFILE = 53;
            public const int ERROR_FILEOPENING = 54;
            public const int ERROR_FILEWRITING = 55;
            public const int ERROR_FILEREADING = 56;
            public const int ERROR_DELETE = 57;
        }

        public enum CodigoResultado
        {
            OK,
            NONEXISTENT_FILE,
            UNKNOWN,
            PDA_DELETE_NOTOK,
            PDA_UPLOAD_NOTOK,
            PDA_DOWNLOAD_NOTOK,
            PDA_ERROR,
            CONNECTION_ERROR,
            FILE_DELETE_NOTOK,
            FILE_UPLOAD_NOTOK,
            FILE_DOWNLOAD_NOTOK,
            DIRECTORY_ERROR,
            DELETE_ERROR
        }


        public static CodigoResultado getResult(int intResult)
        {
            CodigoResultado result;
            switch (intResult)
            {

                case MotoResult.OK:
                    result = CodigoResultado.OK;
                    break;

                case MotoResult.ERROR_INIT:
                    result = CodigoResultado.CONNECTION_ERROR;
                    break;

                case MotoResult.ERROR_FILENOTEXISTS:
                    result = CodigoResultado.NONEXISTENT_FILE;
                    break;

                case MotoResult.ERROR_NOTAFILE:
                    result = CodigoResultado.NONEXISTENT_FILE;
                    break;

                case (int)MotoResult.ERROR_FILEOPENING:
                    result = CodigoResultado.UNKNOWN;
                    break;

                case MotoResult.ERROR_FILEWRITING:
                    result = CodigoResultado.UNKNOWN;
                    break;

                case MotoResult.ERROR_FILEREADING:
                    result = CodigoResultado.UNKNOWN;
                    break;

                case MotoResult.ERROR_DELETE:
                    result = CodigoResultado.DELETE_ERROR;
                    break;

                case MotoResult.ERROR_UNKNOWN:
                    result = CodigoResultado.UNKNOWN;
                    break;
                default:
                    result = CodigoResultado.UNKNOWN;
                    break;
            }

            return result;
        }
    }
}
