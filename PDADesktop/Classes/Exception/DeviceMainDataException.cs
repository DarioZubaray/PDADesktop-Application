using System;

namespace PDADesktop.Classes.Exception
{
    public class DeviceMainDataException : System.Exception
    {
        public DeviceMainDataException()
        {
        }

        public DeviceMainDataException(string message) : base(message)
        {
        }

        public DeviceMainDataException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}
