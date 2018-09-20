namespace PDADesktop.Classes.Devices
{
    class MotoAdapter : DeviceHandler
    {
        public bool isDeviceConnected()
        {
            return MotoApi.isDeviceConnected() != 0;
        }
    }
}
