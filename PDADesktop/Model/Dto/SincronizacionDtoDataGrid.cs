using PDADesktop.Classes;
using PDADesktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace PDADesktop.Model.Dto
{
    public class SincronizacionDtoDataGrid
    {
        public long idSincronizacion { get; set; }
        public string lote { get; set; }
        public int idAccion { get; set; }
        public string accion { get; set; }
        public string fecha { get; set; }
        public int idActividad { get; set; }
        public string actividad { get; set; }
        public int idEstadoGenesix { get; set; }
        public string genesix { get; set; }
        public int idEstadoPda { get; set; }
        public string pda { get; set; }
        public int idEstadoGeneral { get; set; }
        public string estado { get; set; }

        private ICommand estadoGenesixCommand;
        public ICommand EstadoGenesixCommand
        {
            get
            {
                return estadoGenesixCommand;
            }
            set
            {
                estadoGenesixCommand = value;
            }
        }
        private ICommand estadoPDACommand;
        public ICommand EstadoPDACommand
        {
            get
            {
                return estadoPDACommand;
            }
            set
            {
                estadoPDACommand = value;
            }
        }
        private ICommand estadoGeneralCommand;
        public ICommand EstadoGeneralCommand
        {
            get
            {
                return estadoGeneralCommand;
            }
            set
            {
                estadoGeneralCommand = value;
            }
        }

        public static List<SincronizacionDtoDataGrid> ParserDataGrid(List<Sincronizacion> sincro)
        {
            List<SincronizacionDtoDataGrid> dataGridRefresh = new List<SincronizacionDtoDataGrid>();
            foreach(Sincronizacion s in sincro)
            {
                SincronizacionDtoDataGrid sincroDTOGrid = new SincronizacionDtoDataGrid();
                sincroDTOGrid.idSincronizacion = s.idSincronizacion;
                sincroDTOGrid.lote = s.lote.idLote.ToString();
                sincroDTOGrid.accion = s.actividad.accion.descripcion.Split(' ').First();
                sincroDTOGrid.idAccion = Convert.ToInt32(s.actividad.accion.idAccion);
                sincroDTOGrid.fecha = s.lote.fecha.ToString("dd/MM/yyyy HH:mm");
                sincroDTOGrid.actividad = s.actividad.descripcion;
                sincroDTOGrid.idActividad = Convert.ToInt32(s.actividad.idActividad);
                sincroDTOGrid.genesix = s.egx.descripcion;
                sincroDTOGrid.idEstadoGenesix = Convert.ToInt32(s.egx.idEstado);
                sincroDTOGrid.pda = s.epda.descripcion;
                sincroDTOGrid.idEstadoPda = Convert.ToInt32(s.epda.idEstado);
                sincroDTOGrid.estado = GetButtonStateName(s, s.egral.descripcion);
                sincroDTOGrid.idEstadoGeneral = Convert.ToInt32(s.egral.idEstado);
                dataGridRefresh.Add(sincroDTOGrid);
            }
            return dataGridRefresh;
        }

        private static string GetButtonStateName(Sincronizacion sync, string defaultName)
        {
            string buttonStateName = defaultName;
            long stateGeneralId = sync.egral.idEstado;
            switch (stateGeneralId)
            {
                case Constants.EGRAL_REINTENTAR_INFORMAR:
                    buttonStateName = "Informar";
                    break;
                case Constants.EGRAL_REINTENTAR_DESCARGAR:
                    buttonStateName = "Descargar";
                    break;
                case Constants.EGRAL_REINTENTAR_DESCARGAR_RECEPCIONES:
                    buttonStateName = "Descargar Recep";
                    break;
                case Constants.EGRAL_MODIFICAR_AJUSTES:
                    buttonStateName = "Modificar Ajustes";
                    break;
                case Constants.EGRAL_VER_DETALLES_RECEPCIONES:
                    buttonStateName = "Ver Detalles";
                    break;
                case Constants.EGRAL_IMPRIMIR_RECEPCIONES:
                    buttonStateName = "Imprimir";
                    break;
                case Constants.EGRAL_OK:
                    if(Constants.AJUSTES_CODE.Equals(sync.actividad.idActividad))
                    {
                        int epdaReceived = Convert.ToInt32(sync.epda.idEstado);
                        int egx = Convert.ToInt32(sync.egx.idEstado);
                        if(Constants.EPDA_RECIBIDO.Equals(epdaReceived) && 
                            Constants.EGX_ENVIADO.Equals(egx))
                        {
                            buttonStateName = "Ver Ajustes";
                        }
                    }
                    break;
                default:
                    buttonStateName = defaultName;
                    break;
            }


            return buttonStateName;
        }

    }
}
