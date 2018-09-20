using System.Runtime.InteropServices;

namespace PDADesktop.Classes.Devices
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
    }
}
