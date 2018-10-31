using System.Windows.Input;

namespace PDADesktop.Model.Dto
{
    public class ImprimirRecepcionesDtoDataGrid
    {
        public string idRecepcion { get; set; }
        public string fechaRecepcion { get; set; }
        public string remitoCompleto { get; set; }
        public string numeroProveedor { get; set; }
        public string descripcionProveedor { get; set; }
        public string centroPedido { get; set; }
        public string numeroRecepcion { get; set; }
        public string botonImprimir { get; set; }

        private ICommand printCommand;
        public ICommand PrintCommand
        {
            get
            {
                return printCommand;
            }
            set
            {
                printCommand = value;
            }
        }

    }
}
