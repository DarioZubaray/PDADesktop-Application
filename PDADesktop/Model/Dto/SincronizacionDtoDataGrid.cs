using PDADesktop.Classes;
using PDADesktop.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace PDADesktop.Model.Dto
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

        public static List<SincronizacionDtoDataGrid> ParserDataGrid(List<Sincronizacion> sincro)
        {
            List<SincronizacionDtoDataGrid> dataGridRefresh = new List<SincronizacionDtoDataGrid>();
            foreach(Sincronizacion s in sincro)
            {
                SincronizacionDtoDataGrid sincroDTOGrid = new SincronizacionDtoDataGrid();
                sincroDTOGrid.lote = s.lote.idLote.ToString();
                sincroDTOGrid.accion = s.actividad.accion.descripcion.Split(' ').First();
                sincroDTOGrid.fecha = s.lote.fecha.ToString("dd/MM/yyyy HH:mm");
                sincroDTOGrid.actividad = s.actividad.descripcion;
                sincroDTOGrid.genesix = s.egx.descripcion;
                sincroDTOGrid.pda = s.epda.descripcion;
                sincroDTOGrid.estado = s.egral.descripcion;
                sincroDTOGrid.EstadoGeneralCommand = new RelayCommand(CentroActividadesViewModel.BotonEstadoGeneral, param => true);
                sincroDTOGrid.EstadoGenesixCommand = new RelayCommand(CentroActividadesViewModel.BotonEstadoGenesix, param => true);
                sincroDTOGrid.EstadoPDACommand = new RelayCommand(CentroActividadesViewModel.BotonEstadoPDA, param => true);
                dataGridRefresh.Add(sincroDTOGrid);
            }
            return dataGridRefresh;
        }

    }
}
