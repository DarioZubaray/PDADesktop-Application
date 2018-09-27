using PDADesktop.Classes;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace PDADesktop.Model
{
    public class SincronizacionDtoDataGrid
    {
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

        public static List<SincronizacionDtoDataGrid> refreshDataGrid(List<Sincronizacion> sincro)
        {
            List<SincronizacionDtoDataGrid> dataGridRefresh = new List<SincronizacionDtoDataGrid>();
            foreach(Sincronizacion s in sincro)
            {
                SincronizacionDtoDataGrid sPoco = new SincronizacionDtoDataGrid();
                sPoco.lote = s.lote.idLote.ToString();
                sPoco.accion = s.actividad.accion.descripcion;
                sPoco.fecha = s.lote.fecha.ToString();
                sPoco.actividad = s.actividad.descripcion;
                sPoco.genesix = s.egx.descripcion;
                sPoco.pda = s.epda.descripcion;
                sPoco.estado = s.egral.descripcion;
                dataGridRefresh.Add(sPoco);
            }
            return dataGridRefresh;
        }

    }
}
