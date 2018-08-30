using Navegacion.Classes;
using Navegacion.Model;
using Navegacion.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Navegacion.ViewModel
{
    class CentroActividadesViewModel
    {
        private ICommand exitButtonCommand;
        public ICommand ExitButtonCommand
        {
            get
            {
                return exitButtonCommand;
            }
            set
            {
                exitButtonCommand = value;
            }
        }
        private bool canExecute = true;

        public List<Sincronizacion> sincronizaciones
        {
            get
            {
                string idLote = "111153";
                string informar = "Informar a Genesix";
                string descargar = "Descargar de Genesix";
                string formato = "dd/MM/yyyy HH:mm \'hs\'";
                List<Sincronizacion> sincros = new List<Sincronizacion>();
                sincros.Add(new Sincronizacion { lote = idLote, accion = informar, fecha = DateTime.Now.ToString(formato), actividad = "Control de precio con ubicaciones", genesix = "OK", pda = "OK", estado = "OK" });
                sincros.Add(new Sincronizacion { lote = idLote, accion = informar, fecha = DateTime.Now.ToString(formato), actividad = "Ajustes", genesix = "OK", pda = "OK", estado = "OK" });
                sincros.Add(new Sincronizacion { lote = idLote, accion = informar, fecha = DateTime.Now.ToString(formato), actividad = "Recepciones", genesix = "OK", pda = "OK", estado = "OK" });
                sincros.Add(new Sincronizacion { lote = idLote, accion = informar, fecha = DateTime.Now.ToString(formato), actividad = "Impresión de etiquetas", genesix = "OK", pda = "OK", estado = "OK" });

                sincros.Add(new Sincronizacion { lote = idLote, accion = descargar, fecha = DateTime.Now.ToString(formato), actividad = "Artículos", genesix = "OK", pda = "OK", estado = "OK" });
                sincros.Add(new Sincronizacion { lote = idLote, accion = descargar, fecha = DateTime.Now.ToString(formato), actividad = "Ubicaciones", genesix = "OK", pda = "OK", estado = "OK" });
                sincros.Add(new Sincronizacion { lote = idLote, accion = descargar, fecha = DateTime.Now.ToString(formato), actividad = "Ubicaciones Artículos", genesix = "OK", pda = "OK", estado = "OK" });
                sincros.Add(new Sincronizacion { lote = idLote, accion = descargar, fecha = DateTime.Now.ToString(formato), actividad = "Pedidos", genesix = "OK", pda = "OK", estado = "OK" });
                sincros.Add(new Sincronizacion { lote = idLote, accion = descargar, fecha = DateTime.Now.ToString(formato), actividad = "Tipos de Etiquetas", genesix = "OK", pda = "OK", estado = "OK" });
                sincros.Add(new Sincronizacion { lote = idLote, accion = descargar, fecha = DateTime.Now.ToString(formato), actividad = "Tipos de Ajustes", genesix = "OK", pda = "OK", estado = "OK" });
                return sincros;
            }
        }

        #region Constructor
        public CentroActividadesViewModel()
        {
            ExitButtonCommand = new RelayCommand(ExitPortalApi, param => this.canExecute);
        }
        #endregion

        public void ExitPortalApi(object obj)
        {
            Console.WriteLine("exit portal api");
            //aca deberia invocar el logout al portal?
            MainWindow window = (MainWindow) Application.Current.MainWindow;
            Uri uri = new Uri("View/LoginView.xaml", UriKind.Relative);
            window.frame.NavigationService.Navigate(uri);
        }
    }
}
