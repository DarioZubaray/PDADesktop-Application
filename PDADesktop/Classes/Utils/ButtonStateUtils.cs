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
            int generalState = dto.idEstadoGeneral;
            long synchronizationId = dto.idSincronizacion;
            MyAppProperties.buttonState_syncId = synchronizationId.ToString();
            string batchId = dto.lote;
            MyAppProperties.buttonState_batchId = batchId;
            MyAppProperties.storeId = MyAppProperties.storeId;
            int activityId = dto.idActividad;
            int actionId = dto.idAccion;
            int genesixState = dto.idEstadoGenesix;
            int deviceState = dto.idEstadoPda;

            switch(generalState)
            {
                case Constants.EGRAL_REINTENTAR_INFORMAR:
                    RetryInformToGenesix(synchronizationId, activityId);
                    break;
                case Constants.EGRAL_REINTENTAR_DESCARGAR:
                    RetryDownloadFromGenesix(synchronizationId, actionId, activityId);
                    break;
                case Constants.EGRAL_REINTENTAR_DESCARGAR_RECEPCIONES:
                    RetryDownloadReceptionsFromGenesix(synchronizationId, batchId);
                    break;
                case Constants.EGRAL_MODIFICAR_AJUSTES:
                    ModifyAdjusments(synchronizationId, batchId);
                    break;
                case Constants.EGRAL_VER_DETALLES_RECEPCIONES:
                    seeReceptionsDetails(synchronizationId, batchId);
                    break;
                case Constants.EGRAL_IMPRIMIR_RECEPCIONES:
                    PrintReceptions(synchronizationId, batchId);
                    break;
                case Constants.EGRAL_OK:
                    stateNeedResolve = SeeAdjustmentsInforms(activityId, deviceState, genesixState, batchId);
                    break;
                default:
                    stateNeedResolve = false;
                    break;
            }
            return stateNeedResolve;
        }

        private static bool SeeAdjustmentsInforms(int activityId, int deviceState, int genesixState, string batchId)
        {
            if ( Constants.EPDA_RECIBIDO.Equals(deviceState)
                 && Constants.EGX_ENVIADO.Equals(genesixState)
                 && Constants.ACTIVIDAD_AJUSTES.Equals(activityId) )
            {
                Uri uriSeeAdjustmentsInform = new Uri(Constants.VER_AJUSTES_INFORMADOS_VIEW, UriKind.Relative);
                var dispatcher = Application.Current.Dispatcher;
                dispatcher.BeginInvoke(new Action(() =>
                {
                    MainWindow window = (MainWindow)Application.Current.MainWindow;
                    window.frame.NavigationService.Navigate(uriSeeAdjustmentsInform);
                }));
                return true;
            }
            else{
                return false;
            }
        }

        private static void PrintReceptions(long syncId, string batchId)
        {
            // Cambia estado de sincro a OK y refresca
            SynchronizationStateUtil.SetSentGenesixState(syncId);
            //lamar a la vista 'ImprimirRecepcionView'
            Uri uriPrintReceptions = new Uri(Constants.VER_IMPRIMIR_RECEPCION_VIEW, UriKind.Relative);
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.BeginInvoke(new Action(() =>
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;
                window.frame.NavigationService.Navigate(uriPrintReceptions);
            }));
        }

        private static void seeReceptionsDetails(long syncId, string batchId)
        {
            MyAppProperties.SeeDetailsReception_syncId = Convert.ToInt32(syncId);
            MyAppProperties.SeeDetailsReception_batchId = Convert.ToInt64(batchId);
            Uri uriSeeReceptionsDetails = new Uri(Constants.VER_DETALLES_RECEPCION_VIEW, UriKind.Relative);
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.BeginInvoke(new Action(() =>
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;
                window.frame.NavigationService.Navigate(uriSeeReceptionsDetails);
            }));
        }

        private static void ModifyAdjusments(long syncId, string batchId)
        {
            MyAppProperties.SeeAdjustmentModify_syncId = syncId;
            MyAppProperties.SeeAdjustmentModify_batchId = batchId;
            Uri uriSeeAdjustmentsModify = new Uri(Constants.VER_AJUSTES_MODIFICAR_VIEW, UriKind.Relative);
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.BeginInvoke(new Action(() =>
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;
                window.frame.NavigationService.Navigate(uriSeeAdjustmentsModify);
            }));
        }

        private static void RetryDownloadReceptionsFromGenesix(long syncId, string batchId)
        {
            IDeviceHandler deviceHandler = App.Instance.deviceHandler;
            bool doControls = ControlStoreIdAndDatetimeSync();
            if (doControls)
            {
                string storeId = MyAppProperties.storeId;
                DownloadFromGenesix(Constants.ACTIVIDAD_INFORMAR_RECEPCIONES, storeId, syncId);

                ActionResultDto unlockDevice = deviceHandler.ControlDeviceLock(syncId, storeId);
                if (unlockDevice.success)
                {
                    deviceHandler.ChangeSynchronizationState(Constants.ESTADO_SINCRO_FIN);
                }
            }
        }

        private static void RetryDownloadFromGenesix(long syncId, int actionId, int activityId)
        {
            IDeviceHandler deviceHandler = App.Instance.deviceHandler;
            bool doControls = ControlStoreIdAndDatetimeSync();
            if (doControls)
            {
                string storeId = MyAppProperties.storeId;
                DownloadFromGenesix(activityId, storeId, syncId);

                ActionResultDto desbloquearPDA = deviceHandler.ControlDeviceLock(syncId, storeId);
                if (desbloquearPDA.success)
                {
                    deviceHandler.ChangeSynchronizationState(Constants.ESTADO_SINCRO_FIN);
                }
            }
        }

        internal static void RetryInformToGenesix(long syncId, int activityId)
        {
            MyAppProperties.SeeDetailsReception_syncId = syncId;
            IDeviceHandler deviceHandler = App.Instance.deviceHandler;
            if (Constants.ACTIVIDAD_INFORMAR_RECEPCIONES.Equals(activityId))
            {
                bool doControls = ControlStoreIdAndDatetimeSync();
                if (doControls)
                {
                    DeleteAllPreviousFiles();
                }
            }

            string username = MyAppProperties.username;
            HttpWebClientUtil.ExecuteInformGenesix(syncId, username);
            string storeId = MyAppProperties.storeId;
            ActionResultDto unlockDevice = deviceHandler.ControlDeviceLock(syncId, storeId);
            if (unlockDevice.success)
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
                    if(synchronizationDefault == null)
                    {
                        return false;
                    }
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

        public static void DownloadFromGenesix(int activityId, string storeId, long syncId)
        {
            string userName = MyAppProperties.username;
            bool downloadSuccess = HttpWebClientUtil.SearchDATsMasterFile(activityId, storeId, userName);

            if (downloadSuccess)
            {
                if (activityId.Equals(Constants.UBICART_CODE))
                {
                    logger.Debug("Ubicart -> creando Archivos PAS");
                    ArchivosDATUtils.createPASFile();
                }
                if (activityId.Equals(Constants.PEDIDOS_CODE))
                {
                    logger.Debug("Pedidos -> creando Archivos Pedidos");
                    ArchivosDATUtils.createOrdersFiles();
                }

                string destinationDirectory = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
                string filename = ArchivosDATUtils.GetDataFileNameByIdActividad(activityId);
                string filenameAndExtension = FileUtils.WrapSlashAndDATExtension(filename);

                ResultFileOperation result = App.Instance.deviceHandler.CopyPublicDataFileToDevice(destinationDirectory, filenameAndExtension);
                logger.Debug("result: " + result);

                SynchronizationStateUtil.SetReceivedFromGenesixState(syncId);
                SynchronizationStateUtil.SetSentDeviceState(syncId);
            }
        }

    }
}
