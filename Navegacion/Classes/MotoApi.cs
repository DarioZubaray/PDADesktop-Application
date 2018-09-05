using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Navegacion.Classes
{
    class MotoApi
    {
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

        enum codigoResultado : int
        {
            OK = 0,
            ERROR_UNKWON = 50,
            ERROR_INIT = 51,
            ERROR_FILENOTEXISTS = 52,
            ERROR_NOTAFILE = 53,
            ERROR_FILEOPENING = 54,
            ERROR_FILEWRITING = 55,
            ERROR_FILEREADING = 56,
            ERROR_DELETE = 57,
            ERROR_SOURCEFILE_OPENING = 58,
            ERROR_DESTFILE_OPENING = 59
        }
    }
}
