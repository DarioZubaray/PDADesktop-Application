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
            string text = File.ReadAllText(@"c:\file.txt", Encoding.UTF8);
            return text;
        }
    }
}
