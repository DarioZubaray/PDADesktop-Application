using log4net;
using Newtonsoft.Json;
using PDADesktop.Classes;
using PDADesktop.Model;
using PDADesktop.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
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

        private ICommand centroActividadesLoadedCommand;
        public ICommand CentroActividadesLoadedCommand
        {
            get
            {
                return centroActividadesLoadedCommand;
            }
            set
            {
                centroActividadesLoadedCommand = value;
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

        private ICommand showPanelCommand;
        public ICommand ShowPanelCommand
        {
            get
            {
                return showPanelCommand;
            }
            set
            {
                showPanelCommand = value;
            }
        }

        private ICommand hidePanelCommand;
        public ICommand HidePanelCommand
        {
            get
            {
                return hidePanelCommand;
            }
            set
            {
                hidePanelCommand = value;
            }
        }

        private ICommand changeMainMessageCommand;
        public ICommand ChangeMainMessageCommand
        {
            get
            {
                return changeMainMessageCommand;
            }
            set
            {
                changeMainMessageCommand = value;
            }
        }

        private ICommand changeSubMessageCommand;
        public ICommand ChangeSubMessageCommand
        {
            get
            {
                return changeSubMessageCommand;
            }
            set
            {
                changeSubMessageCommand = value;
            }
        }

        private ICommand panelCloseCommand;
        public ICommand PanelCloseCommand
        {
            get
            {
                return panelCloseCommand;
            }
            set
            {
                panelCloseCommand = value;
            }
        }

        private bool canExecute = true;
        #endregion

        #region Attributes
        private string _txt_sincronizacion;
        public string txt_sincronizacion
        {
            get
            {
                return _txt_sincronizacion;
            }
            set
            {
                _txt_sincronizacion = value;
                OnPropertyChanged();
            }
        }

        private string _label_version;
        public string label_version
        {
            get
            {
                return _label_version;
            }
            set
            {
                _label_version = value;
            }
        }
        private string _label_usuario;
        public string label_usuario
        {
            get
            {
                return _label_usuario;
            }
            set
            {
                _label_usuario = value;
            }
        }
        private string _label_sucursal;
        public string label_sucursal
        {
            get
            {
                return _label_sucursal;
            }
            set
            {
                _label_sucursal = value;
            }
        }

        // Atributos del Spiner
        private bool _panelLoading;
        public bool PanelLoading
        {
            get
            {
                return _panelLoading;
            }
            set
            {
                _panelLoading = value;
                OnPropertyChanged();
            }
        }
        private string _panelMainMessage;
        public string PanelMainMessage
        {
            get
            {
                return _panelMainMessage;
            }
            set
            {
                _panelMainMessage = value;
                OnPropertyChanged();
            }
        }
        private string _panelSubMessage;
        public string PanelSubMessage
        {
            get
            {
                return _panelSubMessage;
            }
            set
            {
                _panelSubMessage = value;
                OnPropertyChanged();
            }
        }

        //Con ObservableCollection no se actualiza la grilla :\
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
            PanelLoading = true;
            PanelMainMessage = "Cargando...";
            setInfoLabels();
            ExitButtonCommand = new RelayCommand(ExitPortalApi, param => this.canExecute);
            SincronizarCommand = new RelayCommand(SincronizarTodosLosDatos, param => this.canExecute);
            InformarCommand = new RelayCommand(InformarGenesix, param => this.canExecute);
            VerAjustesCommand = new RelayCommand(VerAjustes, param => this.canExecute);
            CentroActividadesLoadedCommand = new RelayCommand(CentroActividadesLoaded, param => this.canExecute);
            AnteriorCommand = new RelayCommand(SincronizacionAnterior, param => this.canExecute);
            SiguienteCommand = new RelayCommand(SincronizacionSiguiente, param => this.canExecute);
            BuscarCommand = new RelayCommand(BuscarSincronizaciones, param => this.canExecute);
            UltimaCommand = new RelayCommand(IrUltimaSincronizacion, param => this.canExecute);
            EstadoGenesixCommand = new RelayCommand(BotonEstadoGenesix, param => this.canExecute);
            EstadoPDACommand = new RelayCommand(BotonEstadoPDA, param => this.canExecute);
            EstadoGeneralCommand = new RelayCommand(BotonEstadoGeneral, param => this.canExecute);
            sincronizaciones = SincronizacionPOCO.getStaticMockList(new RelayCommand(BotonEstadoGenesix, param => this.canExecute));
            ActualizarLoteActual(sincronizaciones);

            ShowPanelCommand = new RelayCommand(MostrarPanel, param => this.canExecute);
            HidePanelCommand = new RelayCommand(OcultarPanel, param => this.canExecute);
            ChangeMainMessageCommand = new RelayCommand(CambiarMainMensage, param => this.canExecute);
            ChangeSubMessageCommand = new RelayCommand(CambiarSubMensage, param => this.canExecute);
            PanelCloseCommand = new RelayCommand(CerrarPanel, param => this.canExecute);
        }
        #endregion

        #region Methods
        public void setInfoLabels()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            label_version = assembly.GetName().Version.ToString(3);
            // de donde obtenemos el usaurio y sucursal (?)
            label_usuario = "Admin";
            label_sucursal = "706";
        }

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
            PanelLoading = true;
            PanelMainMessage = "Espere por favor";
            logger.Info("Sincronizando todos los datos");
        }

        public void InformarGenesix(object obj)
        {
            PanelLoading = false;
            logger.Info("Informando a genesix");
        }

        public void VerAjustes(object obj)
        {
            logger.Info("Viendo ajustes realizados");
            int estadoPda = MotoApi.isDeviceConnected();
            if(estadoPda != 0)
            {
                string clientDataDir = ConfigurationManager.AppSettings.Get("CLIENT_PATH_DATA");
                string fileName = ConfigurationManager.AppSettings.Get("DEVICE_FILE_AJUSTES");
                string motoApiReadDataFile = MotoApi.ReadDataFile(clientDataDir, fileName);
                //por aca sabemos si hay ajustes realizados y de continuar

                List<Ajustes> ajustes = JsonConvert.DeserializeObject<List<Ajustes>>(motoApiReadDataFile);
                if(ajustes != null && ajustes.Count > 0)
                {
                    logger.Debug("hay ajustes realizados");
                    logger.Debug("Dato que sera de vital importancia: ajustes.Count es el numnero del badge");

                    bool isWindowOpen = false;
                    foreach (Window w in Application.Current.Windows)
                    {
                        if (w is VerAjustesView)
                        {
                            isWindowOpen = true;
                            w.Activate();
                        }
                    }

                    if (!isWindowOpen)
                    {
                        VerAjustesView newwindow = new VerAjustesView();
                        newwindow.Show();
                    }
                }
                else
                {
                    //else mostramos un mensaje que no hay datos
                    logger.Debug("No, no hay ajustes hecho, para que habran pulsado en ver ajustes, por curiosidad?");
                    AvisoAlUsuario("No se encontraron Ajustes Realizados");
                }

            }
            else
            {
                AvisoAlUsuario("No se detecta conexion con la PDA");
            }
        }

        public void CentroActividadesLoaded(object obj)
        {
            string urlIdLoteActual = ConfigurationManager.AppSettings.Get("API_SYNC_ID_LOTE");
            string _sucursal = MyAppProperties.idSucursal;
            int idLoteActual = HttpWebClient.GetIdLoteActual(urlIdLoteActual, _sucursal);
            if (idLoteActual != 0)
            {
                MyAppProperties.idLoteActual = idLoteActual.ToString();
            }
            PanelLoading = false;
        }

        public void SincronizacionAnterior(object obj)
        {
            List<Sincronizacion> listaSincronizaciones = null;
            logger.Info("<- Ejecutando la vista anterior a la actual de la grilla");
            string urlSincronizacionAnterior = ConfigurationManager.AppSettings.Get("API_SYNC_ANTERIOR");
            string _sucursal = MyAppProperties.idSucursal;
            string _idLote = MyAppProperties.idLoteActual;
            listaSincronizaciones = HttpWebClient.GetHttpWebSincronizacion(urlSincronizacionAnterior, _sucursal, _idLote);
            if(listaSincronizaciones != null && listaSincronizaciones.Count != 0)
            {
                logger.Debug(listaSincronizaciones);
                this.sincronizaciones = SincronizacionPOCO.refreshDataGrid(listaSincronizaciones);
                ActualizarLoteActual(sincronizaciones);
            }
        }

        public void SincronizacionSiguiente(object obj)
        {
            List<Sincronizacion> listaSincronizaciones = null;
            logger.Info("-> Solicitando la vista siguiente de la grilla, si es que la hay...");
            string urlSincronizacionAnterior = ConfigurationManager.AppSettings.Get("API_SYNC_SIGUIENTE");
            string _sucursal = MyAppProperties.idSucursal;
            string _idLote = MyAppProperties.idLoteActual;
            listaSincronizaciones = HttpWebClient.GetHttpWebSincronizacion(urlSincronizacionAnterior, _sucursal, _idLote);
            if (listaSincronizaciones != null && listaSincronizaciones.Count != 0)
            {
                logger.Debug(listaSincronizaciones);
                this.sincronizaciones = SincronizacionPOCO.refreshDataGrid(listaSincronizaciones);
                ActualizarLoteActual(sincronizaciones);
            }
        }

        public void BuscarSincronizaciones(object obj)
        {
            logger.Info("Buscando sincronizaciones");
            BuscarLotesView buscarLotesView = new BuscarLotesView();
            buscarLotesView.ShowDialog();
        }

        public void IrUltimaSincronizacion(object obj)
        {
            List<Sincronizacion> listaSincronizaciones = null;
            logger.Info("Llendo a la ultima sincronizacion");
            string urlSincronizacionAnterior = ConfigurationManager.AppSettings.Get("API_SYNC_ULTIMA");
            string _sucursal = MyAppProperties.idSucursal;
            string _idLote = MyAppProperties.idLoteActual;
            listaSincronizaciones = HttpWebClient.GetHttpWebSincronizacion(urlSincronizacionAnterior, _sucursal, _idLote);
            if (listaSincronizaciones != null && listaSincronizaciones.Count != 0)
            {
                logger.Debug(listaSincronizaciones);
                this.sincronizaciones = SincronizacionPOCO.refreshDataGrid(listaSincronizaciones);
                ActualizarLoteActual(sincronizaciones);
            }
        }

        public void ActualizarLoteActual(List<SincronizacionPOCO> sincronizaciones)
        {
            if(sincronizaciones != null && sincronizaciones.Count != 0)
            {
                var s = sincronizaciones[0] as SincronizacionPOCO;
                string idLoteActual = s.lote;
                MyAppProperties.idLoteActual = idLoteActual;
                ActualizarTxt_Sincronizar(s);
            }
        }

        public void ActualizarTxt_Sincronizar(SincronizacionPOCO sincronizacion)
        {
            string lote = sincronizacion.lote;
            string fecha = sincronizacion.fecha;
            txt_sincronizacion = " (" + lote + ") " + fecha;
        }

        public void BotonEstadoGenesix(object obj)
        {
            logger.Info("Boton estado genesix");
            logger.Debug(this);
            //Sincronizacion row = (Sincronizacion)((Button)e.Source).DataContext;
        }
        public void BotonEstadoPDA(object obj)
        {
            logger.Info("Boton estado pda");
        }
        public void BotonEstadoGeneral(object obj)
        {
            logger.Info("Boton estado general");
        }

        public void MostrarPanel(object obj)
        {
            PanelLoading = true;
        }
        public void OcultarPanel(object obj)
        {
            PanelLoading = false;
        }
        public void CambiarMainMensage(object obj)
        {
            PanelMainMessage = "Espere por favor";
        }
        public void CambiarSubMensage(object obj)
        {
            PanelSubMessage = "";
        }
        public void CerrarPanel(object obj)
        {
            PanelLoading = false;
        }

        public void AvisoAlUsuario(string mensaje)
        {
            string message = mensaje;
            string caption = "Aviso!";
            MessageBoxButton messageBoxButton = MessageBoxButton.OK;
            MessageBoxImage messageBoxImage = MessageBoxImage.Error;
            MessageBoxResult result = MessageBox.Show(message, caption, messageBoxButton, messageBoxImage);
            if (result == MessageBoxResult.OK)
            {
                logger.Debug("Informando al usuario: " + mensaje);
            }
        }
        #endregion
    }
}
