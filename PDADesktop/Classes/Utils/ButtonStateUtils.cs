using System;
using PDADesktop.Model.Dto;
using PDADesktop.Classes.Devices;
using PDADesktop.Model;
using System.Collections.Generic;

namespace PDADesktop.Classes.Utils
{
    public class ButtonStateUtils
    {
        public static void ResolveState()
        {
            SincronizacionDtoDataGrid dto = MyAppProperties.SelectedSync;
            int estadoGeneral = dto.idEstadoGeneral;
            long idSincronizacion = dto.idSincronizacion;
            string idLote = dto.lote;
            string accion = dto.accion;
            int idActividad = dto.idActividad;
            int idAccion = dto.idAccion;
            int egx = dto.idEstadoGenesix;
            int epda = dto.idEstadoPda;

            switch(estadoGeneral)
            {
                case Constants.EGRAL_REINTENTAR1:
                    PrimerReintento(idSincronizacion, idActividad);
                    break;
                case Constants.EGRAL_REINTENTAR2:
                    SegundoReintento(idSincronizacion, idAccion, idActividad);
                    break;
                case Constants.EGRAL_REINTENTAR3:
                    TercerReintento(idSincronizacion, idLote);
                    break;
                case Constants.EGRAL_MODIFICAR_AJUSTE:
                    VerAjustes(idSincronizacion, idLote);
                    break;
                case Constants.EGRAL_VER_DETALLES:
                    verDetalles(idSincronizacion, idLote);
                    break;
                case Constants.EGRAL_IMPRIMIR_RECEPCION:
                    Imprimir(idSincronizacion, idLote);
                    break;
                case Constants.EGRAL_OK:
                    verAjustesInformados(idActividad, epda, egx, idLote);
                    break;
                default:
                    break;
            }
        }

        private static void verAjustesInformados(int idActividad, int epda, int egx, string idLote)
        {
            if ( Constants.EPDA_RECIBIDO.Equals(epda) 
                 && Constants.EGX_ENVIADO.Equals(egx)
                 && Constants.ACTIVIDAD_AJUSTES.Equals(idActividad) )
            {
                //llamar a la vista ver 'AjustesView'
            }
        }

        private static void Imprimir(long idSincronizacion, string idLote)
        {
            //lamar a la vista 'ImprimirRecepcionView'
            //la cual puede abrir un PDF
        }

        private static void verDetalles(long idSincronizacion, string idLote)
        {
            //lamar a la vista 'VerDetallesRecepcionView'
        }

        private static void VerAjustes(long idSincronizacion, string idLote)
        {
            //lamar a la vista 'VerAjustesView'
            //que diferencia hay entre verAjustes y verAjustesRealizados y verAjustesInformados(?)
        }

        private static void TercerReintento(long idSincronizacion, string idLote)
        {
            //applet.actualizarPedidos(idSinc, response);
            //applet.controlBloqueoPDA(idSinc);
            //refresca la grilla con el loteActual
        }

        private static void SegundoReintento(long idSincronizacion, int idAccion, int idActividad)
        {
            //applet.descargarDeGX(arrStr);
            //applet.controlBloqueoPDA(arrStr[0]);
            //refresca la grilla con el loteActual
        }

        private static void PrimerReintento(long idSincronizacion, int idActividad)
        {
            IDeviceHandler deviceHandler = App.Instance.deviceHandler;
            if (Constants.ACTIVIDAD_INFORMAR_RECEPCIONES.Equals(idActividad))
            {
                bool isConnected = deviceHandler.IsDeviceConnected();
                if (isConnected)
                {
                    string storeIDFromDevice = deviceHandler.ReadStoreIdFromDefaultData();
                    string storeIdFromLogin = MyAppProperties.storeId;
                    if (!TextUtils.CompareStoreId(storeIDFromDevice, storeIdFromLogin))
                    {
                        string synchronizationDefault = deviceHandler.ReadSynchronizationDateFromDefaultData();
                        bool isGreatherThanToday = DateTimeUtils.IsGreatherThanToday(synchronizationDefault);
                        if (isGreatherThanToday)
                        {
                            //DeleteAllPreviousFiles(true);
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
                    }
                }
            }

            HttpWebClientUtil.ExecuteInformGenesix(idSincronizacion);
            string storeId = MyAppProperties.storeId;
            DesbloquearPDA desbloquearPDA = deviceHandler.ControlBloqueoPDA(idSincronizacion, storeId);
            if (desbloquearPDA.desbloquearPDA)
            {
                deviceHandler.CambiarEstadoSincronizacion(Constants.ESTADO_SINCRO_FIN);
            }
        }
    }
}
