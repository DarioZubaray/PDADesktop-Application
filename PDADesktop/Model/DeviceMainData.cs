using PDADesktop.Classes.Exceptions;
using System;

namespace PDADesktop.Model
{
    class DeviceMainData
    {
        #region Constants
        public const int POSITION_ESTADO_ESCUCHA = 0;
        public const int POSITION_ESTADO_SINCRO = 1;
        public const int POSITION_FECHA_SINCO = 2;
        public const int POSITION_SUCURSAL = 3;
        public const int POSITION_VERSION = 4;
        public const int POSITION_AUTOOFF = 5;
        public const int TOTAL_POSITION = 5;
        #endregion

        #region Attributes
        private string estadoEscucha { get; set; }
        private string estadoSincronizacion { get; set; }
        private string fechaUltimaSincronizacion { get; set; }
        private string sucursal { get; set; }
        private string version { get; set; }
        private string autoOff { get; set; }
        #endregion

        #region Static Methods
        public static DeviceMainData build(string mainData)
        {
            DeviceMainData deviceMainData = new DeviceMainData();
            try
            {
                String[] values = mainData.Split('|');
                deviceMainData.estadoEscucha = values[POSITION_ESTADO_ESCUCHA];
                deviceMainData.estadoSincronizacion = values[POSITION_ESTADO_SINCRO];
                deviceMainData.fechaUltimaSincronizacion = values[POSITION_FECHA_SINCO];
                deviceMainData.sucursal = values[POSITION_SUCURSAL];
                deviceMainData.version = values[POSITION_VERSION];
                deviceMainData.autoOff = values[POSITION_AUTOOFF];
            }
            catch (Exception e)
            {
                ThrowDeviceMainDataException(e);
            }
            return deviceMainData;
        }

        private static void ThrowDeviceMainDataException(Exception e)
        {
            string message = "No se pudo leer archivo de configuración del dispositivo";
            throw new DeviceMainDataException(message, e);
        }
        #endregion
    }
}
