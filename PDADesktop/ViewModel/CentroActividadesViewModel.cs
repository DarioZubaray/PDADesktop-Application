using log4net;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using PDADesktop.Classes;
using PDADesktop.Classes.Devices;
using PDADesktop.Model;
using PDADesktop.Utils;
using PDADesktop.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Reflection;
using System.Threading;
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
        private readonly BackgroundWorker sincronizarWorker = new BackgroundWorker();

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
        private List<SincronizacionDtoDataGrid> _sincronizaciones;
        public List<SincronizacionDtoDataGrid> sincronizaciones
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

        public Badged badge_verAjustes
        {
            get
            {
                Badged badge = new Badged();

                Button botonVerAjustes = new Button();
                botonVerAjustes.Name = "button_verAjustes";
                botonVerAjustes.Content = "Ver Ajustes";
                botonVerAjustes.HorizontalAlignment = HorizontalAlignment.Left;
                botonVerAjustes.VerticalAlignment = VerticalAlignment.Top;
                botonVerAjustes.ToolTip = "Ver los ajustes realizados.";
                botonVerAjustes.Command = this.VerAjustesCommand;

                bool estadoDevice = App.Instance.deviceHandler.IsDeviceConnected();
                if (estadoDevice)
                {
                    string DeviceAjusteFile = App.Instance.deviceHandler.ReadAdjustmentsDataFile();
                    if (DeviceAjusteFile != null)
                    {
                        ObservableCollection<Ajustes> ajustes = JsonConvert.DeserializeObject<ObservableCollection<Ajustes>>(DeviceAjusteFile);
                        if(ajustes != null && ajustes.Count > 0)
                        {
                            badge.Badge = ajustes.Count;
                        }
                        else
                        {
                            badge.Badge = 0;
                            botonVerAjustes.IsEnabled = false;
                        }
                    }
                    else
                    {
                        badge.Badge = 0;
                        botonVerAjustes.IsEnabled = false;
                    }
                }
                else
                {
                    badge.Badge = "NO PDA";
                    badge.BadgeColorZoneMode = ColorZoneMode.Dark;
                    botonVerAjustes.IsEnabled = false;
                }
                badge.Content = botonVerAjustes;
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

            sincronizaciones = SincronizacionDtoDataGrid.getStaticMockList(new RelayCommand(BotonEstadoGenesix, param => this.canExecute));
            ActualizarLoteActual(sincronizaciones);
            GetIdAccionesAsync();
            sincronizarWorker.DoWork += sincronizarWorker_DoWork;
            sincronizarWorker.RunWorkerCompleted += sincronizarWorker_RunWorkerCompleted;

            ShowPanelCommand = new RelayCommand(MostrarPanel, param => this.canExecute);
            HidePanelCommand = new RelayCommand(OcultarPanel, param => this.canExecute);
            ChangeMainMessageCommand = new RelayCommand(CambiarMainMensage, param => this.canExecute);
            ChangeSubMessageCommand = new RelayCommand(CambiarSubMensage, param => this.canExecute);
            PanelCloseCommand = new RelayCommand(CerrarPanel, param => this.canExecute);
        }
        #endregion

        #region Worker Method
        private void sincronizarWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Debug("sincronizar Worker ->doWork: " + sender);
            // buscar datos antiguos
            /*
             * 1-obtener el default.DAT
             *      si no existe crearlo y construirlo
             * 2-obtener la fecha de ultima sincronizacion
             *      si es mayor a 1 dia, consultar descartar datos antiguos
            */

            // buscar recepciones informadas pendientes
            string idSucursal = MyAppProperties.idSucursal;
            string idSincronizacion = HttpWebClient.GetIdLoteActual(idSucursal).ToString();
            bool recepcionesInformadas = HttpWebClient.CheckRecepcionesInformadas(idSincronizacion);
            logger.Debug("recepciones Informadas pendientes: " + (recepcionesInformadas ? "NO" : "SI"));
            // Consultar por un SI-No si desea continuar
            bool confirmaDescartarRecepciones = true;
            if(!recepcionesInformadas)
            {
                string message = "Existen recepciones pendientes de informar, ¿Desea continuar y descartar las mismas?";
                string caption = "Aviso";
                MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.OKCancel, MessageBoxImage.Asterisk);
                if (result.Equals(MessageBoxResult.Cancel))
                {
                    confirmaDescartarRecepciones = false;
                }
            }
            if(confirmaDescartarRecepciones)
            {
                List<Actividad> actividades = GetIdAcciones();
                foreach(Actividad actividad in actividades)
                {
                    if (Constants.DESCARGAR_GENESIX.Equals(actividad.accion.idAccion))
                    {
                        bool descargaMaestroCorrecta = HttpWebClient.buscarMaestrosDAT((int)actividad.idActividad, idSucursal);
                        PanelSubMessage = "Descargando " + actividad.descripcion.ToString();
                        Thread.Sleep(500);
                        if(descargaMaestroCorrecta)
                        {
                            if(actividad.idActividad.Equals(Constants.ubicart))
                            {
                                logger.Debug("Ubicart -> creando Archivos PAS");
                                MaestrosUtils.crearArchivoPAS();
                            }
                            if(actividad.idActividad.Equals(Constants.pedidos))
                            {
                                logger.Debug("Pedidos -> creando Archivos Pedidos");
                                MaestrosUtils.crearArchivosPedidos();
                            }

                            string destinationDirectory = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
                            string filename = MaestrosUtils.GetMasterFileName((int)actividad.idActividad);
                            string filenameAndExtension = "/" + filename + ".DAT";

                            DeviceResultName result = App.Instance.deviceHandler.CopyAppDataFileToDevice(destinationDirectory, filenameAndExtension);
                            logger.Debug("result: " + result);
                            PanelSubMessage = "Moviendo al dispositivo";
                            Thread.Sleep(500);
                        }
                    }
                }
            }
        }

        private void sincronizarWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("sincronizar Worker ->runWorkedCompleted: " + sender);
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.BeginInvoke(new Action(() =>
            {
                PanelLoading = false;
            }));
        }
        #endregion

        #region Methods
        public void setInfoLabels()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            label_version = assembly.GetName().Version.ToString(3);
            // de donde obtenemos el usuario y sucursal (?)
            label_usuario = "Admin";
            label_sucursal = MyAppProperties.idSucursal;
        }

        public void ExitPortalApi(object obj)
        {
            logger.Info("exit portal api");
            //aca deberia invocar el logout al portal?
            MainWindow window = (MainWindow) Application.Current.MainWindow;
            Uri uri = new Uri(Constants.LOGIN_VIEW, UriKind.Relative);
            window.frame.NavigationService.Navigate(uri);
        }

        public void SincronizarTodosLosDatos(object obj)
        {
            PanelLoading = true;
            PanelMainMessage = "Espere por favor";
            logger.Info("Sincronizando todos los datos");
            sincronizarWorker.RunWorkerAsync();
        }

        public void InformarGenesix(object obj)
        {
            PanelLoading = false;
            logger.Info("Informando a genesix");
        }

        public void VerAjustes(object obj)
        {
            logger.Info("Viendo ajustes realizados");
            bool estadoDevice = App.Instance.deviceHandler.IsDeviceConnected();
            if(estadoDevice)
            {
                string motoApiReadDataFile = App.Instance.deviceHandler.ReadAdjustmentsDataFile();
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
            string _sucursal = MyAppProperties.idSucursal;
            int idLoteActual = HttpWebClient.GetIdLoteActual(_sucursal);
            if (idLoteActual != 0)
            {
                MyAppProperties.idLoteActual = idLoteActual.ToString();
            }
            logger.Debug("Centro de actividades, carga finalizada ocultando el panelLoading");
            PanelLoading = false;
        }

        public void SincronizacionAnterior(object obj)
        {
            List<Sincronizacion> listaSincronizaciones = null;
            logger.Info("<- Ejecutando la vista anterior a la actual de la grilla");
            string urlSincronizacionAnterior = ConfigurationManager.AppSettings.Get(Constants.API_SYNC_ANTERIOR);
            string _sucursal = MyAppProperties.idSucursal;
            string _idLote = MyAppProperties.idLoteActual;
            listaSincronizaciones = HttpWebClient.GetHttpWebSincronizacion(urlSincronizacionAnterior, _sucursal, _idLote);
            if(listaSincronizaciones != null && listaSincronizaciones.Count != 0)
            {
                logger.Debug(listaSincronizaciones);
                this.sincronizaciones = SincronizacionDtoDataGrid.refreshDataGrid(listaSincronizaciones);
                ActualizarLoteActual(sincronizaciones);
            }
        }

        public void SincronizacionSiguiente(object obj)
        {
            List<Sincronizacion> listaSincronizaciones = null;
            logger.Info("-> Solicitando la vista siguiente de la grilla, si es que la hay...");
            string urlSincronizacionAnterior = ConfigurationManager.AppSettings.Get(Constants.API_SYNC_SIGUIENTE);
            string _sucursal = MyAppProperties.idSucursal;
            string _idLote = MyAppProperties.idLoteActual;
            listaSincronizaciones = HttpWebClient.GetHttpWebSincronizacion(urlSincronizacionAnterior, _sucursal, _idLote);
            if (listaSincronizaciones != null && listaSincronizaciones.Count != 0)
            {
                logger.Debug(listaSincronizaciones);
                this.sincronizaciones = SincronizacionDtoDataGrid.refreshDataGrid(listaSincronizaciones);
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
            string urlSincronizacionAnterior = ConfigurationManager.AppSettings.Get(Constants.API_SYNC_ULTIMA);
            string _sucursal = MyAppProperties.idSucursal;
            string _idLote = MyAppProperties.idLoteActual;
            listaSincronizaciones = HttpWebClient.GetHttpWebSincronizacion(urlSincronizacionAnterior, _sucursal, _idLote);
            if (listaSincronizaciones != null && listaSincronizaciones.Count != 0)
            {
                logger.Debug(listaSincronizaciones);
                this.sincronizaciones = SincronizacionDtoDataGrid.refreshDataGrid(listaSincronizaciones);
                ActualizarLoteActual(sincronizaciones);
            }
        }

        public void ActualizarLoteActual(List<SincronizacionDtoDataGrid> sincronizaciones)
        {
            if(sincronizaciones != null && sincronizaciones.Count != 0)
            {
                var s = sincronizaciones[0] as SincronizacionDtoDataGrid;
                string idLoteActual = s.lote;
                MyAppProperties.idLoteActual = idLoteActual;
                ActualizarTxt_Sincronizar(s);
            }
        }

        public void ActualizarTxt_Sincronizar(SincronizacionDtoDataGrid sincronizacion)
        {
            string lote = sincronizacion.lote;
            string fecha = sincronizacion.fecha;
            txt_sincronizacion = " (" + lote + ") " + fecha;
        }
        public void GetIdAccionesAsync()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                GetIdAcciones();
            }).Start();
        }

        public List<Actividad> GetIdAcciones()
        {
            string urlAcciones = ConfigurationManager.AppSettings.Get(Constants.API_GET_ALL_ACCIONES);
            var responseAcciones = HttpWebClient.sendHttpGetRequest(urlAcciones);
            List<Accion> acciones = JsonConvert.DeserializeObject<List<Accion>>(responseAcciones);

            string idAcciones = TextUtils.ParseListAccion2String(acciones);
            string jsonBody = "{ \"idAcciones\": " + idAcciones.ToString() + "}";

            var urlActividades = ConfigurationManager.AppSettings.Get(Constants.API_GET_ACTIVIDADES);
            string responseActividades = HttpWebClient.sendHttpPostRequest(urlActividades, jsonBody);
            logger.Debug(responseActividades);
            List<Actividad> actividades = JsonConvert.DeserializeObject<List<Actividad>>(responseActividades);
            logger.Debug(actividades.ToString());
            return actividades;
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
