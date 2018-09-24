using PDADesktop.Classes;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace PDADesktop.Model
{
    class SincronizacionDtoDataGrid
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

        public static List<SincronizacionDtoDataGrid> getStaticMockList(RelayCommand command)
        {
            string idLote = "111153";
            string informar = "Informar a Genesix";
            string descargar = "Descargar de Genesix";
            string formato = "dd/MM/yyyy HH:mm \'hs\'";
            List<SincronizacionDtoDataGrid> sincros = new List<SincronizacionDtoDataGrid>();
            sincros.Add(new SincronizacionDtoDataGrid
            {
                lote = idLote,
                accion = informar,
                fecha = DateTime.Now.ToString(formato),
                actividad = "Control de precios con ubicaciones",
                genesix = "OK",
                pda = "OK",
                estado = "OK",
                EstadoGenesixCommand = command
            });
            sincros.Add(new SincronizacionDtoDataGrid
            {
                lote = idLote,
                accion = informar,
                fecha = DateTime.Now.ToString(formato),
                actividad = "Ajustes",
                genesix = "OK",
                pda = "OK",
                estado = "OK",
                EstadoGenesixCommand = command
            });
            sincros.Add(new SincronizacionDtoDataGrid
            {
                lote = idLote,
                accion = informar,
                fecha = DateTime.Now.ToString(formato),
                actividad = "Recepciones",
                genesix = "OK",
                pda = "OK",
                estado = "OK",
                EstadoGenesixCommand = command
            });
            sincros.Add(new SincronizacionDtoDataGrid
            {
                lote = idLote,
                accion = informar,
                fecha = DateTime.Now.ToString(formato),
                actividad = "Impresión de etiquetas",
                genesix = "OK",
                pda = "OK",
                estado = "OK",
                EstadoGenesixCommand = command
            });

            sincros.Add(new SincronizacionDtoDataGrid
            {
                lote = idLote,
                accion = descargar,
                fecha = DateTime.Now.ToString(formato),
                actividad = "Articulos",
                genesix = "OK",
                pda = "OK",
                estado = "OK",
                EstadoGenesixCommand = command
            });
            sincros.Add(new SincronizacionDtoDataGrid
            {
                lote = idLote,
                accion = descargar,
                fecha = DateTime.Now.ToString(formato),
                actividad = "Ubicaciones",
                genesix = "OK",
                pda = "OK",
                estado = "OK",
                EstadoGenesixCommand = command
            });
            sincros.Add(new SincronizacionDtoDataGrid
            {
                lote = idLote,
                accion = descargar,
                fecha = DateTime.Now.ToString(formato),
                actividad = "Ubicacion Articulos",
                genesix = "OK",
                pda = "OK",
                estado = "OK",
                EstadoGenesixCommand = command
            });
            sincros.Add(new SincronizacionDtoDataGrid
            {
                lote = idLote,
                accion = descargar,
                fecha = DateTime.Now.ToString(formato),
                actividad = "Pedidos",
                genesix = "OK",
                pda = "OK",
                estado = "OK",
                EstadoGenesixCommand = command
            });
            sincros.Add(new SincronizacionDtoDataGrid
            {
                lote = idLote,
                accion = descargar,
                fecha = DateTime.Now.ToString(formato),
                actividad = "Tipos de Etiquetas",
                genesix = "OK",
                pda = "OK",
                estado = "OK",
                EstadoGenesixCommand = command
            });
            sincros.Add(new SincronizacionDtoDataGrid
            {
                lote = idLote,
                accion = descargar,
                fecha = DateTime.Now.ToString(formato),
                actividad = "Tipos de Ajustes",
                genesix = "OK",
                pda = "OK",
                estado = "OK",
                EstadoGenesixCommand = command
            });
            return sincros;
        }
    }
}
