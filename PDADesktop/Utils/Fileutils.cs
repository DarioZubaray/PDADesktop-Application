using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDADesktop.Utils
{
    class FileUtils
    {
        public static string ReadFile(string source)
        {
            string text = File.ReadAllText(source, Encoding.UTF8);
            return text;
        }

        internal static void VerifyFolders(string desDirExpanded)
        {
            //http://msdn.microsoft.com/en-us/library/54a0at6s.aspx
            System.IO.Directory.CreateDirectory(desDirExpanded);
        }
    }
}
