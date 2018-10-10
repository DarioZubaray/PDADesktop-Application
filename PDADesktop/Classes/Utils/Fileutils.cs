using log4net;
using System.Configuration;
using System.IO;
using System.Text;
using System;
using PDADesktop.Model;

namespace PDADesktop.Classes.Utils
{
    class FileUtils
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string ReadFile(string source)
        {
            return File.ReadAllText(source, Encoding.UTF8);
        }

        public static void WriteFile(string source, string content)
        {
            File.WriteAllText(source, content);
        }

        public static void CopyFile(string source, string destination)
        {
            File.Copy(source, destination);
        }

        public static void DeleteFile(string source)
        {
            File.Delete(source);
        }

        public static bool VerifyIfExitsFile(string fullFilePath)
        {
            return File.Exists(fullFilePath);
        }

        public static void VerifyFoldersOrCreate(string fullDirectoryPathExtended)
        {
            Directory.CreateDirectory(fullDirectoryPathExtended);
        }

        public static string CountRegistryWithinFile(string filepath)
        {
            var lines = File.ReadAllLines(filepath).Length;
            return lines.ToString();
        }

        public static string WrapSlashAndDATExtension(string filename)
        {
            return PrependSlash(AppendDATExtension(filename));
        }

        public static string PrependSlash(string filename)
        {
            return "/" + filename;
        }

        public static string AppendDATExtension(string filename)
        {
            return filename + ".DAT";
        }

        public static string GetDefaultDatContentByPositionInPublic(string positionContent, int position)
        {
            string defaultPathPublic = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_LOOKUP);
            string defaultPathPublicExtended = TextUtils.ExpandEnviromentVariable(defaultPathPublic);
            string filenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_DEFAULT);
            string currentDefaultContent = ReadFile(defaultPathPublicExtended + filenameAndExtension);
            return TextUtils.BuildDefaultContent(currentDefaultContent, positionContent, position);
        }

        public static void UpdateDefaultDatFileInPublic(string positionContent, int position)
        {
            string content = GetDefaultDatContentByPositionInPublic(positionContent, position);

            string defaultPathPublic = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_LOOKUP);
            string defaultPathPublicExtended = TextUtils.ExpandEnviromentVariable(defaultPathPublic);
            string filenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_DEFAULT);
            string currentDefaultContent = ReadFile(defaultPathPublicExtended + filenameAndExtension);
            WriteFile(defaultPathPublicExtended + filenameAndExtension, content);
        }

        public static void UpdateDeviceMainFileInPublic(string _sucursal)
        {
            string defaultPathPublic = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_LOOKUP);
            string defaultPathPublicExtended = TextUtils.ExpandEnviromentVariable(defaultPathPublic);
            string filenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_DEFAULT);

            string currentDefaultContent = ReadFile(defaultPathPublicExtended + filenameAndExtension);
            String[] defaultArray = currentDefaultContent.Split('|');

            if (defaultArray.Length == DeviceMainData.TOTAL_POSITION_ONE_BASE)
            {
                defaultArray[DeviceMainData.POSITION_ESTADO_ESCUCHA] = "0";
                defaultArray[DeviceMainData.POSITION_ESTADO_SINCRO] = "0";
                defaultArray[DeviceMainData.POSITION_FECHA_SINCO] = DateTime.Now.ToString("yyyyMMddHHmmss");
                defaultArray[DeviceMainData.POSITION_SUCURSAL] = _sucursal;
                defaultArray[DeviceMainData.POSITION_AUTOOFF] = "0";
            }
            string updatedDefaultContent = String.Join("|", defaultArray);
            WriteFile(defaultPathPublicExtended + filenameAndExtension, updatedDefaultContent);
        }
    }
}
