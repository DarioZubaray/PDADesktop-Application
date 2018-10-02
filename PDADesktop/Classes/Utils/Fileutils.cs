using PDADesktop.Classes.Devices;
using System.IO;
using System.Text;

namespace PDADesktop.Classes.Utils
{
    class FileUtils
    {
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
            return "/" + filename + ".DAT";
        }
    }
}
