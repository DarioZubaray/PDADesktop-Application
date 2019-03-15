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
            string fileReaded = null;
            try
            {
                fileReaded = File.ReadAllText(source, Encoding.UTF8);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
            }
            return fileReaded;
        }

        public static void WriteFile(string path, string contents)
        {
            try
            {
                File.WriteAllText(path, contents);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
            }
        }

        public static void CopyFile(string source, string destination)
        {
            try
            {
                File.Copy(source, destination);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
            }
        }

        public static void DeleteFile(string source)
        {
            try
            {
                File.Delete(source);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
            }
        }

        public static bool VerifyIfExitsFile(string fullFilePath)
        {
            return File.Exists(fullFilePath);
        }

        public static void VerifyFoldersOrCreate(string fullDirectoryPathExtended)
        {
            try
            {
                Directory.CreateDirectory(fullDirectoryPathExtended);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
            }
        }

        public static string CountRegistryWithinFile(string filepath)
        {
            var totalLines = 0;
            try
            {
                totalLines = File.ReadAllLines(filepath).Length;
                return totalLines.ToString();
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                return totalLines.ToString();
            }
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
            if(currentDefaultContent != null)
            {
                return TextUtils.BuildDefaultContent(currentDefaultContent, positionContent, position);
            }
            else
            {
                return null;
            }
        }

        public static void UpdateDefaultDatFileInPublic(string positionContent, int position)
        {
            string content = GetDefaultDatContentByPositionInPublic(positionContent, position);

            string defaultPathPublic = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_LOOKUP);
            string defaultPathPublicExtended = TextUtils.ExpandEnviromentVariable(defaultPathPublic);
            string filenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_DEFAULT);
            string currentDefaultContent = ReadFile(defaultPathPublicExtended + filenameAndExtension);
            if(currentDefaultContent != null)
            {
                WriteFile(defaultPathPublicExtended + filenameAndExtension, content);
            }
        }

        public static void UpdateDeviceMainFileInPublic(string _sucursal)
        {
            string defaultPathPublic = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_LOOKUP);
            string defaultPathPublicExtended = TextUtils.ExpandEnviromentVariable(defaultPathPublic);
            string filenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_DEFAULT);

            string currentDefaultContent = ReadFile(defaultPathPublicExtended + filenameAndExtension);
            if(currentDefaultContent == null)
            {
                logger.Error("No se pudo obtener el contenido por defecto");
                return;
            }
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

        public static string GetTempFileName(string pattern)
        {
            Int32 unixTimestamp = DateTimeUtils.GetUnixTimeFromUTCNow();
            return String.Format(pattern, unixTimestamp);
        }

        internal static void DeleteTempFiles()
        {
            string tempFilePath = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_TEMP);
            string tempFilePathExtended = TextUtils.ExpandEnviromentVariable(tempFilePath);
            try
            {
                if (Directory.Exists(@tempFilePathExtended))
                {
                    Directory.Delete(@tempFilePathExtended, true);
                }
            }
            catch (IOException ex)
            {
               logger.Error(ex.Message, ex);
            }
        }
    }
}
