using log4net;
using System.Configuration;
using System.IO;
using System.Text;

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

        public static void UpdateDefaultDatFileInPublic(string positionContent, int position)
        {
            string defaultPathPublic = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);
            string defaultPathPublicExtended = TextUtils.ExpandEnviromentVariable(defaultPathPublic);
            string filenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_DEFAULT);
            string currentDefaultContent = ReadFile(defaultPathPublicExtended + filenameAndExtension);
            string content = TextUtils.BuildDefaultContent(currentDefaultContent, positionContent, position);
            WriteFile(defaultPathPublic, content);
        }
    }
}
