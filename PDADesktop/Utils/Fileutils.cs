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

        public static bool VerifyIfExitsFile(string desDirExpanded)
        {
            return File.Exists(desDirExpanded);
        }

        public static void VerifyFoldersOrCreate(string desDirExpanded)
        {
            Directory.CreateDirectory(desDirExpanded);
        }
    }
}
