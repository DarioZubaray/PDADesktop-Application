using log4net;
using PDADesktop.Classes;
using PDADesktop.Model;
using PDADesktop.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PDADesktop.ViewModel
{
    class CentroActividadesViewModel : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region Commands
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

        private ICommand sincronizarCommand;
        public ICommand SincronizarCommand
        {
            get
            {
                return sincronizarCommand;
            }
            set
            {
                sincronizarCommand = value;
            }
        }

        private ICommand informarCommand;
        public ICommand InformarCommand
        {
            get
            {
                return informarCommand;
            }
            set
            {
                informarCommand = value;
            }
        }

        private ICommand verAjustesCommand;
        public ICommand VerAjustesCommand
        {
            get
            {
                return verAjustesCommand;
            }
            set
            {
                verAjustesCommand = value;
            }
        }

        private ICommand sincronizacionActualCommand;
        public ICommand SincronizacionActualCommand
        {
            get
            {
                return sincronizacionActualCommand;
            }
            set
            {
                sincronizacionActualCommand = value;
            }
        }

        private ICommand anteriorCommand;
        public ICommand AnteriorCommand
        {
            get
            {
                return anteriorCommand;
            }
            set
            {
                anteriorCommand = value;
            }
        }

        private ICommand siguienteCommand;
        public ICommand SiguienteCommand
        {
            get
            {
                return siguienteCommand;
            }
            set
            {
                siguienteCommand = value;
            }
        }

        private ICommand buscarCommand;
        public ICommand BuscarCommand
        {
            get
            {
                return buscarCommand;
            }
            set
            {
                buscarCommand = value;
            }
        }

        private ICommand ultimaCommand;
        public ICommand UltimaCommand
        {
            get
            {
                return ultimaCommand;
            }
            set
            {
                ultimaCommand = value;
            }
        }

        private bool canExecute = true;
        #endregion

        #region Attributes
        private List<SincronizacionPOCO> _sincronizaciones;
        public List<SincronizacionPOCO> sincronizaciones
        {
            get
            {
                return _sincronizaciones;
            }
            set
            {
                _sincronizaciones = value;
                OnPropertyChanged();
            }
        }

        public MaterialDesignThemes.Wpf.Badged badge_informar
        {
            get
            {
                MaterialDesignThemes.Wpf.Badged badge = new MaterialDesignThemes.Wpf.Badged();
                badge.Badge = new Random().Next(5, 15);
                Button botonInformar = new Button();
                botonInformar.Name = "button_informar";
                botonInformar.Content = "Informar a Genesix todos los datos";
                botonInformar.HorizontalAlignment = HorizontalAlignment.Left;
                botonInformar.VerticalAlignment = VerticalAlignment.Top;
                botonInformar.ToolTip = "Informar a genesix todos los datos realizados.";
                botonInformar.Command = this.InformarCommand;
                badge.Content = botonInformar;
                return badge;
            }
        }
        #endregion

        #region Constructor
        public CentroActividadesViewModel()
        {
            ExitButtonCommand = new RelayCommand(ExitPortalApi, param => this.canExecute);
            SincronizarCommand = new RelayCommand(SincronizarTodosLosDatos, param => this.canExecute);
            InformarCommand = new RelayCommand(InformarGenesix, param => this.canExecute);
            VerAjustesCommand = new RelayCommand(VerAjustes, param => this.canExecute);
            SincronizacionActualCommand = new RelayCommand(SincronizacionActual, param => this.canExecute);
            AnteriorCommand = new RelayCommand(SincronizacionAnterior, param => this.canExecute);
            SiguienteCommand = new RelayCommand(SincronizacionSiguiente, param => this.canExecute);
            BuscarCommand = new RelayCommand(BuscarSincronizaciones, param => this.canExecute);
            UltimaCommand = new RelayCommand(IrUltimaSincronizacion, param => this.canExecute);
            sincronizaciones = SincronizacionPOCO.getStaticMockList(new RelayCommand(BotonEstadoGenesix, param => this.canExecute));
        }
        #endregion

        #region Methods
        public void ExitPortalApi(object obj)
        {
            logger.Info("exit portal api");
            //aca deberia invocar el logout al portal?
            MainWindow window = (MainWindow) Application.Current.MainWindow;
            Uri uri = new Uri("View/LoginView.xaml", UriKind.Relative);
            window.frame.NavigationService.Navigate(uri);
        }

        public void SincronizarTodosLosDatos(object obj)
        {
            logger.Info("Sincronizando todos los datos");
        }

        public void InformarGenesix(object obj)
        {
            logger.Info("Informando a genesix");
        }

        public void VerAjustes(object obj)
        {
            logger.Info("Viendo ajustes realizados");
        }

        public void SincronizacionActual(object obj)
        {
            string urlIdLoteActual = ConfigurationManager.AppSettings.Get("API_SYNC_ID_LOTE");
            string _sucursal = MyAppProperties.idSucursal;
            int idLoteActual = HttpWebClient.GetIdLoteActual(urlIdLoteActual, _sucursal);
            if (idLoteActual != 0)
            {
                MyAppProperties.idLoteActual = idLoteActual.ToString();
            }
        }

        public void SincronizacionAnterior(object obj)
        {
            List<Sincronizacion> sincronizaciones = null;
            logger.Info("<- Ejecutando la vista anterior a la actual de la grilla");
            string urlSincronizacionAnterior = ConfigurationManager.AppSettings.Get("API_SYNC_ANTERIOR");
            string _sucursal = MyAppProperties.idSucursal;
            string _idLote = MyAppProperties.idLoteActual;
            sincronizaciones = HttpWebClient.GetHttpWebSincronizacion(urlSincronizacionAnterior, _sucursal, _idLote);
            if(sincronizaciones.Count != 0)
            {
                logger.Debug(sincronizaciones);
                this.sincronizaciones = SincronizacionPOCO.refreshDataGrid(sincronizaciones);
                //dataGrid.ItemSource = sincronizaciones;
            }

        }

        public void SincronizacionSiguiente(object obj)
        {
            logger.Info("-> Solicitando la vista siguiente de la grilla, si es que la hay...");
        }

        public void BuscarSincronizaciones(object obj)
        {
            logger.Info("Buscando sincronizaciones");
        }

        public void IrUltimaSincronizacion(object obj)
        {
            logger.Info("Llendo a la ultima sincronizacion");
        }

        public void BotonEstadoGenesix(object obj)
        {
            logger.Info("Boton estado genesix");
            logger.Debug(this);
            //Sincronizacion row = (Sincronizacion)((Button)e.Source).DataContext;
        }
        #endregion
    }
}
