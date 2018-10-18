using System;
using PDADesktop.Model.Dto;

namespace PDADesktop.Classes.Utils
{
    public class ButtonStateUtils
    {
        public static void ResolveState()
        {
            SincronizacionDtoDataGrid dto = MyAppProperties.SelectedSync;
            int estadoGeneral = dto.idEstadoGeneral;
            string accion = dto.accion;
            string actividad = dto.actividad;
            //

            switch(estadoGeneral)
            {
                case Constants.EGRAL_REINTENTAR1:
                    PrimerReintento();
                    break;
                case Constants.EGRAL_REINTENTAR2:
                    SegundoReintento();
                    break;
                case Constants.EGRAL_REINTENTAR3:
                    TercerReintento();
                    break;
                case Constants.EGRAL_MODIFICAR_AJUSTE:
                    VerAjustes();
                    break;
                case Constants.EGRAL_VER_DETALLES:
                    verDetalles();
                    break;
                case Constants.EGRAL_IMPRIMIR_RECEPCION:
                    Imprimir();
                    break;
                case Constants.EGRAL_OK:
                    verAjustesInformados();
                    break;
                default:
                    break;
            }
        }

        private static void verAjustesInformados()
        {
            throw new NotImplementedException();
        }

        private static void Imprimir()
        {
            throw new NotImplementedException();
        }

        private static void verDetalles()
        {
            throw new NotImplementedException();
        }

        private static void VerAjustes()
        {
            throw new NotImplementedException();
        }

        private static void TercerReintento()
        {
            throw new NotImplementedException();
        }

        private static void SegundoReintento()
        {
            throw new NotImplementedException();
        }

        private static void PrimerReintento()
        {
            throw new NotImplementedException();
        }
    }
}
