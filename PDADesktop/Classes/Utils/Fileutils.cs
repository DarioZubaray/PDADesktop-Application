using PDADesktop.Classes.Devices;
using System.IO;
using System.Text;

namespace PDADesktop.Utils
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

        public static bool VerifyIfExitsFile(string desDirExpanded)
        {
            return File.Exists(desDirExpanded);
        }

        public static void VerifyFoldersOrCreate(string desDirExpanded)
        {
            Directory.CreateDirectory(desDirExpanded);
        }

        public static string CountRegistryWithinFile(string filepath)
        {
            var lines = File.ReadAllLines(filepath).Length;
            return lines.ToString();
        }
    }
}
