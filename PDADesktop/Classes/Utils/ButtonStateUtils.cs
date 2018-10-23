using System;
using PDADesktop.Model.Dto;
using PDADesktop.Classes.Devices;
using PDADesktop.Model;
using System.Collections.Generic;
using log4net;
using System.Configuration;
using PDADesktop.View;
using System.Windows;

namespace PDADesktop.Classes.Utils
{
    public class ButtonStateUtils
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static bool ResolveState()
        {
            bool stateNeedResolve = true;
            SincronizacionDtoDataGrid dto = MyAppProperties.SelectedSync;
            int estadoGeneral = dto.idEstadoGeneral;
            long idSincronizacion = dto.idSincronizacion;
            string idLote = dto.lote;
            int idActividad = dto.idActividad;
            int idAccion = dto.idAccion;
            int egx = dto.idEstadoGenesix;
            int epda = dto.idEstadoPda;

            switch(estadoGeneral)
            {
                case Constants.EGRAL_REINTENTAR_INFORMAR:
                    RetryInformToGenesix(idSincronizacion, idActividad);
                    break;
                case Constants.EGRAL_REINTENTAR_DESCARGA:
                    RetryDownloadFromGenesix(idSincronizacion, idAccion, idActividad);
                    break;
                case Constants.EGRAL_REINTENTAR3:
                    RetryDownloadReceptionsFromGenesix(idSincronizacion, idLote);
                    break;
                case Constants.EGRAL_MODIFICAR_AJUSTE:
                    ModifyAdjusments(idSincronizacion, idLote);
                    break;
                case Constants.EGRAL_VER_DETALLES:
                    seeReceptionsDetails(idSincronizacion, idLote);
                    break;
                case Constants.EGRAL_IMPRIMIR_RECEPCION:
                    PrintReceptions(idSincronizacion, idLote);
                    break;
                case Constants.EGRAL_OK:
                    SeeAdjustmentsInforms(idActividad, epda, egx, idLote);
                    break;
                default:
                    stateNeedResolve = false;
                    break;
            }
            return stateNeedResolve;
        }

        private static void SeeAdjustmentsInforms(int idActividad, int epda, int egx, string idLote)
        {
            if ( Constants.EPDA_RECIBIDO.Equals(epda)
                 && Constants.EGX_ENVIADO.Equals(egx)
                 && Constants.ACTIVIDAD_AJUSTES.Equals(idActividad) )
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;
                Uri uri = new Uri(Constants.VER_AJUSTES_INFORMADOS_VIEW, UriKind.Relative);
                window.frame.NavigationService.Navigate(uri);
            }
        }

        private static void PrintReceptions(long idSincronizacion, string idLote)
        {
            //lamar a la vista 'ImprimirRecepcionView'
            //la cual puede abrir un PDF
        }

        private static void seeReceptionsDetails(long idSincronizacion, string idLote)
        {
            //lamar a la vista 'VerDetallesRecepcionView'
        }

        private static void ModifyAdjusments(long idSincronizacion, string idLote)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            Uri uri = new Uri(Constants.VER_AJUSTES_MODIFICAR_VIEW, UriKind.Relative);
            window.frame.NavigationService.Navigate(uri);
        }

        private static void RetryDownloadReceptionsFromGenesix(long idSincronizacion, string idLote)
        {
            IDeviceHandler deviceHandler = App.Instance.deviceHandler;
            bool doControls = ControlStoreIdAndDatetimeSync();
            if (doControls)
            {
                string storeId = MyAppProperties.storeId;
                DownloadFromGenesix(Constants.ACTIVIDAD_INFORMAR_RECEPCIONES, storeId, idSincronizacion);

                ControlBloqueoPDA desbloquearPDA = deviceHandler.ControlDeviceLock(idSincronizacion, storeId);
                if (desbloquearPDA.desbloquearPDA)
                {
                    deviceHandler.ChangeSynchronizationState(Constants.ESTADO_SINCRO_FIN);
                }
            }
        }

        private static void RetryDownloadFromGenesix(long idSincronizacion, int idAccion, int idActividad)
        {
            IDeviceHandler deviceHandler = App.Instance.deviceHandler;
            bool doControls = ControlStoreIdAndDatetimeSync();
            if (doControls)
            {
                string storeId = MyAppProperties.storeId;
                DownloadFromGenesix(idActividad, storeId, idSincronizacion);

                ControlBloqueoPDA desbloquearPDA = deviceHandler.ControlDeviceLock(idSincronizacion, storeId);
                if (desbloquearPDA.desbloquearPDA)
                {
                    deviceHandler.ChangeSynchronizationState(Constants.ESTADO_SINCRO_FIN);
                }
            }
        }

        private static void RetryInformToGenesix(long idSincronizacion, int idActividad)
        {
            IDeviceHandler deviceHandler = App.Instance.deviceHandler;
            if (Constants.ACTIVIDAD_INFORMAR_RECEPCIONES.Equals(idActividad))
            {
                bool doControls = ControlStoreIdAndDatetimeSync();
                if (doControls)
                {
                    DeleteAllPreviousFiles();
                }
            }

            HttpWebClientUtil.ExecuteInformGenesix(idSincronizacion);
            string storeId = MyAppProperties.storeId;
            ControlBloqueoPDA unlockDevice = deviceHandler.ControlDeviceLock(idSincronizacion, storeId);
            if (unlockDevice.desbloquearPDA)
            {
                deviceHandler.ChangeSynchronizationState(Constants.ESTADO_SINCRO_FIN);
            }
        }

        public static bool ControlStoreIdAndDatetimeSync()
        {
            IDeviceHandler deviceHandler = App.Instance.deviceHandler;
            bool isConnected = deviceHandler.IsDeviceConnected();
            if (isConnected)
            {
                string storeIDFromDevice = deviceHandler.ReadStoreIdFromDefaultData();
                string storeIdFromLogin = MyAppProperties.storeId;
                bool areEqualsStoresIds = TextUtils.CompareStoreId(storeIDFromDevice, storeIdFromLogin);
                if (!areEqualsStoresIds)
                {
                    string synchronizationDefault = deviceHandler.ReadSynchronizationDateFromDefaultData();
                    bool isGreatherThanToday = DateTimeUtils.IsGreatherThanToday(synchronizationDefault);
                    if (isGreatherThanToday)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void DeleteAllPreviousFiles()
        {
            IDeviceHandler deviceHandler = App.Instance.deviceHandler;
            List<Actividad> actividades = MyAppProperties.activitiesEnables;

            deviceHandler.DeleteDeviceAndPublicDataFiles(Constants.LPEDIDOS);
            deviceHandler.DeleteDeviceAndPublicDataFiles(Constants.APEDIDOS);
            deviceHandler.DeleteDeviceAndPublicDataFiles(Constants.EPEDIDOS);
            deviceHandler.DeleteDeviceAndPublicDataFiles(Constants.RPEDIDOS);

            foreach (Actividad actividad in actividades)
            {
                long idActividadActual = actividad.idActividad;
                string filename = ArchivosDATUtils.GetDataFileNameByIdActividad((int)idActividadActual);
                deviceHandler.DeleteDeviceAndPublicDataFiles(filename);
            }
        }

        public static void DownloadFromGenesix(int idActividad, string storeId, long syncId)
        {
            bool descargaMaestroCorrecta = HttpWebClientUtil.SearchDATsMasterFile(idActividad, storeId);

            if (descargaMaestroCorrecta)
            {
                if (idActividad.Equals(Constants.ubicart))
                {
                    logger.Debug("Ubicart -> creando Archivos PAS");
                    ArchivosDATUtils.crearArchivoPAS();
                }
                if (idActividad.Equals(Constants.pedidos))
                {
                    logger.Debug("Pedidos -> creando Archivos Pedidos");
                    ArchivosDATUtils.crearArchivosPedidos();
                }

                string destinationDirectory = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
                string filename = ArchivosDATUtils.GetDataFileNameByIdActividad(idActividad);
                string filenameAndExtension = FileUtils.WrapSlashAndDATExtension(filename);

                ResultFileOperation result = App.Instance.deviceHandler.CopyPublicDataFileToDevice(destinationDirectory, filenameAndExtension);
                logger.Debug("result: " + result);

                SynchronizationStateUtil.SetReceivedFromGenesixState(syncId);
                SynchronizationStateUtil.SetSentDeviceState(syncId);
            }
        }

    }
}
