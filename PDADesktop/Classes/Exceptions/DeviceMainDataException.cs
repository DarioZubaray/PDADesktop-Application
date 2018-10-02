using System;

namespace PDADesktop.Classes.Exceptions
{
    public class DeviceMainDataException : Exception
    {
        public DeviceMainDataException()
        {
        }

        public DeviceMainDataException(string message) : base(message)
        {
        }

        public DeviceMainDataException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
