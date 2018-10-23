﻿using log4net;
using MaterialDesignThemes.Wpf;
using PDADesktop.Classes;
using PDADesktop.Classes.Devices;
using PDADesktop.Model;
using PDADesktop.Classes.Utils;
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
using PDADesktop.Model.Dto;
using System.Windows.Threading;

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

        private ICommand verAjustesRealizadosCommand;
        public ICommand VerAjustesRealizadosCommand
        {
            get
            {
                return verAjustesRealizadosCommand;
            }
            set
            {
                verAjustesRealizadosCommand = value;
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
        private readonly BackgroundWorker loadOnceCentroActividadesWorker = new BackgroundWorker();
        private readonly BackgroundWorker reloadCentroActividadesWorker = new BackgroundWorker();
        private readonly BackgroundWorker syncWorker = new BackgroundWorker();
        private readonly BackgroundWorker syncDataGridWorker = new BackgroundWorker();
        private readonly BackgroundWorker adjustmentWorker = new BackgroundWorker();

        private DispatcherTimer dispatcherTimer { get; set; }

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

        #region Loading panel
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
        #endregion
        #region Panel no connection
        private bool _panelLoading_NC;
        public bool PanelLoading_NC
        {
            get
            {
                return _panelLoading_NC;
            }
            set
            {
                _panelLoading_NC = value;
                OnPropertyChanged();
            }
        }
        private string _panelMainMessage_NC;
        public string PanelMainMessage_NC
        {
            get
            {
                return _panelMainMessage_NC;
            }
            set
            {
                _panelMainMessage_NC = value;
                OnPropertyChanged();
            }
        }
        private string _panelSubMessage_NC;
        public string PanelSubMessage_NC
        {
            get
            {
                return _panelSubMessage_NC;
            }
            set
            {
                _panelSubMessage_NC = value;
                OnPropertyChanged();
            }
        }
        #endregion

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
        private SincronizacionDtoDataGrid selectedSynchronization;
        public SincronizacionDtoDataGrid SelectedSynchronization
        {
            get
            {
                return selectedSynchronization;
            }
            set
            {
                selectedSynchronization = value;
                MyAppProperties.SelectedSync = selectedSynchronization;
                OnPropertyChanged();
            }
        }

        private Badged badge_verAjustesRealizados;
        public Badged Badge_verAjustesRealizados
        {
            get
            {
                return badge_verAjustesRealizados;
            }
            set
            {
                badge_verAjustesRealizados = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public CentroActividadesViewModel()
        {
            BannerApp.PrintActivityCenter();
            PanelLoading_NC = false;
            PanelMainMessage_NC = "PDA SIN CONEXION";
            PanelLoading = true;
            PanelMainMessage = "Cargando...";
            setInfoLabels();
            ExitButtonCommand = new RelayCommand(ExitPortalApi, param => this.canExecute);
            SincronizarCommand = new RelayCommand(SincronizarTodosLosDatos, param => this.canExecute);
            InformarCommand = new RelayCommand(InformarGenesix, param => this.canExecute);
            VerAjustesRealizadosCommand = new RelayCommand(VerAjustesRealizados, param => this.canExecute);
            CentroActividadesLoadedCommand = new RelayCommand(CentroActividadesLoaded, param => this.canExecute);
            AnteriorCommand = new RelayCommand(SincronizacionAnterior, param => this.canExecute);
            SiguienteCommand = new RelayCommand(SincronizacionSiguiente, param => this.canExecute);
            BuscarCommand = new RelayCommand(BuscarSincronizaciones, param => this.canExecute);
            UltimaCommand = new RelayCommand(GoLastSynchronization, param => this.canExecute);
            EstadoGenesixCommand = new RelayCommand(BotonEstadoGenesix, param => this.canExecute);
            EstadoPDACommand = new RelayCommand(BotonEstadoPDA, param => this.canExecute);
            EstadoGeneralCommand = new RelayCommand(BotonEstadoGeneral, param => this.canExecute);

            loadOnceCentroActividadesWorker.DoWork += loadOnceActivityCenterWorker_DoWork;
            loadOnceCentroActividadesWorker.RunWorkerCompleted += loadOnceCentroActividadesWorker_RunWorkerCompleted;

            reloadCentroActividadesWorker.DoWork += reloadCentroActividadesWorker_DoWork;
            reloadCentroActividadesWorker.RunWorkerCompleted += reloadCentroActividadesWorker_RunWorkerCompleted;

            syncWorker.DoWork += syncWorker_DoWork;
            syncWorker.RunWorkerCompleted += syncWorker_RunWorkerCompleted;

            syncDataGridWorker.DoWork += syncDataGrid_DoWork;
            syncDataGridWorker.RunWorkerCompleted += syncDataGrid_RunWorkerCompleted;

            adjustmentWorker.DoWork += adjustmentWorker_DoWork;
            adjustmentWorker.RunWorkerCompleted += adjustmentWorker_RunWorkerCompleted;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 15);
            dispatcherTimer.Start();

            ShowPanelCommand = new RelayCommand(MostrarPanel, param => this.canExecute);
            HidePanelCommand = new RelayCommand(OcultarPanel, param => this.canExecute);
            ChangeMainMessageCommand = new RelayCommand(CambiarMainMensage, param => this.canExecute);
            ChangeSubMessageCommand = new RelayCommand(CambiarSubMensage, param => this.canExecute);
            PanelCloseCommand = new RelayCommand(CerrarPanel, param => this.canExecute);
        }
        #endregion

        #region dispatcherTimer
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (!loadOnceCentroActividadesWorker.IsBusy || syncWorker.IsBusy)
            {
                bool deviceStatus = App.Instance.deviceHandler.IsDeviceConnected();
                logger.Debug("disptachertimer tick => Device status: " + deviceStatus);

                var dispatcher = App.Instance.Dispatcher;
                if (!deviceStatus)
                {
                    MyAppProperties.needReloadActivityCenter = true;
                    dispatcher.BeginInvoke(new Action(() =>
                    {
                        ShowPanelNoConnection();
                    }));
                }
                else
                {
                    if (MyAppProperties.needReloadActivityCenter)
                    {
                        dispatcher.BeginInvoke(new Action(() =>
                        {
                            PanelLoading_NC = false;
                            PanelMainMessage_NC = "La conexión ha vuelto! que bien!";
                            PanelLoading = true;
                        }));
                        loadOnceCentroActividadesWorker.RunWorkerAsync();
                        MyAppProperties.needReloadActivityCenter = false;
                    }
                }

                // Forcing the CommandManager to raise the RequerySuggested event
                CommandManager.InvalidateRequerySuggested();
            }
            else
            {
                logger.Debug("se esta cargando el centro de actividades...");
            }
        }

        private void ShowPanelNoConnection()
        {
            PanelLoading_NC = true;
            PanelMainMessage_NC = "Se ha perdido la conexión con el Dispositivo " + App.Instance.deviceHandler.GetNameToDisplay();
            PanelSubMessage_NC = "Reintentando...";
        }
        #endregion

        #region Workers Method
        #region Loaded Activity Center Worker
        private void loadOnceActivityCenterWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string currentMessage = "Load Activity Center Worker -> doWork";
            logger.Debug(currentMessage);
            if(dispatcherTimer != null)
            {
                dispatcherTimer.Stop();
            }

            currentMessage = "Verificando conexión con el servidor PDAExpress...";
            NotifyCurrentMessage(currentMessage);
            bool serverStatus = CheckServerStatus();

            string storeId = MyAppProperties.storeId;
            if(serverStatus)
            {
                currentMessage = "Obteniendo acciones disponibles...";
                NotifyCurrentMessage(currentMessage);
                List<Accion> actionsAvailables = GetAllActions();

                currentMessage = "Obteniendo actividades de todas las acciones...";
                NotifyCurrentMessage(currentMessage);
                GetActivitiesByActions(actionsAvailables);

                currentMessage = "Obteniendo el id del último lote para sucursal " + storeId + " ...";
                NotifyCurrentMessage(currentMessage);
                int currentBatchId = HttpWebClientUtil.GetCurrentBatchId(storeId);

                currentMessage = "Obteniendo detalles de sincronización para lote " + currentBatchId + " ...";
                NotifyCurrentMessage(currentMessage);
                GetLastSync(currentBatchId, storeId);
            }
            else
            {
                logger.Info("Server no disponible");
            }

            currentMessage = "Checkeando conexión con el dispositivo...";
            NotifyCurrentMessage(currentMessage);
            bool isConneted = CheckDeviceConnected();

            if(isConneted)
            {
                PanelMainMessage = "Espere por favor...";
                currentMessage = "Leyendo Ajustes realizados...";
                NotifyCurrentMessage(currentMessage);
                CreateBadgeSeeAdjustments();

                currentMessage = "Leyendo la configuración del dispositivo...";
                NotifyCurrentMessage(currentMessage);
                CopyDeviceMainDataFileToPublic();

                currentMessage = "Controlando componentes del dispositivo...";
                NotifyCurrentMessage(currentMessage);
                ControlDevicePrograms();

                currentMessage = "Verificando sucursal configurada en el dispositivo...";
                NotifyCurrentMessage(currentMessage);
                bool filesPreviouslyDeleted = false;
                bool needAssociateStoreId = CheckStoreIdDevice();
                if (needAssociateStoreId)
                {
                    currentMessage = "Asociando sucursal del dispositivo al n°" + storeId + " ...";
                    NotifyCurrentMessage(currentMessage);
                    AssociateCurrentStoreId(storeId);
                    filesPreviouslyDeleted = true;
                }

                currentMessage = "Borrando datos antiguos ...";
                NotifyCurrentMessage(currentMessage);
                CheckLastSyncDateFromDefault(filesPreviouslyDeleted);

                currentMessage = "Actualizando archivo principal de configuración del dispositivo ...";
                NotifyCurrentMessage(currentMessage);
                UpdateDeviceMainFile(storeId);
            }
            else
            {
                logger.Info("Dispositivo no detectado");
                CreateBadgeSeeAdjustments();
                ShowPanelNoConnection();
            }
            MyAppProperties.loadOnce = false;
            NotifyCurrentMessage("Todo listo!");
        }

        public void NotifyCurrentMessage(string currentMessage)
        {
            logger.Debug(currentMessage);
            PanelSubMessage = currentMessage;
            Thread.Sleep(300);
        }

        private bool CheckServerStatus()
        {
            return HttpWebClientUtil.GetHttpWebServerConexionStatus();
        }

        public List<Accion> GetAllActions()
        {
            List<Accion> actionsAvailables = HttpWebClientUtil.GetAllActions();
            MyAppProperties.actionsEnables = actionsAvailables;
            return actionsAvailables;
        }

        public void GetActivitiesByActions(List<Accion> actionsAvailable)
        {
            List<Actividad> activitiesAvailables = HttpWebClientUtil.GetActivitiesByActionId(actionsAvailable);
            MyAppProperties.activitiesEnables = activitiesAvailables;
        }

        public void GetLastSync(int currentBatchId, string storeId)
        {
            if (currentBatchId != 0)
            {
                MyAppProperties.currentBatchId = currentBatchId.ToString();
                List<Sincronizacion> syncList = null;
                string urlPathLastSync = ConfigurationManager.AppSettings.Get(Constants.API_SYNC_ULTIMA);
                syncList = HttpWebClientUtil.GetHttpWebSynchronizations(urlPathLastSync, storeId, currentBatchId.ToString());
                if (syncList != null && syncList.Count != 0)
                {
                    this.sincronizaciones = SincronizacionDtoDataGrid.ParserDataGrid(syncList);
                    UpdateCurrentBatch(sincronizaciones);
                }
            }
        }

        private bool CheckDeviceConnected()
        {
            bool isDeviceConnected = App.Instance.deviceHandler.IsDeviceConnected();
            logger.Info("Device is connected: " + isDeviceConnected);
            return isDeviceConnected;
        }

        public void CreateBadgeSeeAdjustments()
        {
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.BeginInvoke(new Action(() =>
            {
                Button buttonSeeAdjustment = new Button();
                buttonSeeAdjustment.Name = "button_verAjustes";
                buttonSeeAdjustment.Content = "Ver Ajustes";
                buttonSeeAdjustment.HorizontalAlignment = HorizontalAlignment.Left;
                buttonSeeAdjustment.VerticalAlignment = VerticalAlignment.Top;
                buttonSeeAdjustment.ToolTip = "Ver los ajustes realizados.";
                buttonSeeAdjustment.Command = this.VerAjustesRealizadosCommand;

                Badged badge = new Badged();
                bool estadoDevice = App.Instance.deviceHandler.IsDeviceConnected();
                if (estadoDevice)
                {
                    string DeviceAjusteFile = App.Instance.deviceHandler.ReadAdjustmentsDataFile();
                    if (DeviceAjusteFile != null)
                    {
                        ObservableCollection<Ajustes> ajustes = JsonUtils.GetObservableCollectionAjustes(DeviceAjusteFile);
                        if (ajustes != null && ajustes.Count > 0)
                        {
                            badge.Badge = ajustes.Count;
                        }
                        else
                        {
                            badge.Badge = 0;
                            buttonSeeAdjustment.IsEnabled = false;
                        }
                    }
                    else
                    {
                        badge.Badge = 0;
                        buttonSeeAdjustment.IsEnabled = false;
                    }
                }
                else
                {
                    badge.Badge = "NO PDA";
                    badge.BadgeColorZoneMode = ColorZoneMode.Dark;
                    buttonSeeAdjustment.IsEnabled = false;
                }
                badge.Content = buttonSeeAdjustment;
                Badge_verAjustesRealizados = badge;
            }));
        }

        private void CopyDeviceMainDataFileToPublic()
        {
            string slashFilenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_DEFAULT);
            IDeviceHandler deviceHandler = App.Instance.deviceHandler;
            ResultFileOperation deviceCopyResult = deviceHandler.CopyDeviceFileToPublicLookUp(slashFilenameAndExtension);
            if(deviceCopyResult.Equals(ResultFileOperation.OK))
            {
                logger.Debug("Archivo copiado con éxito.");
            }
            else
            {
                string errorCopyDeviceFile = "Error al leer el archivo DEFAULT, Regenerándolo...";
                NotifyCurrentMessage(errorCopyDeviceFile);
                deviceHandler.CreateEmptyDefaultDataFile();
            }
        }

        private void ControlDevicePrograms()
        {
            int device = 2;
            bool enabled = true;
            List<VersionDispositivo> latestVersionsFromServer =  HttpWebClientUtil.GetInfoVersions(device, enabled);
            if(latestVersionsFromServer != null)
            {
                VersionDispositivo lastVersionFromServer = latestVersionsFromServer[0];
                string lastServerVersion = lastVersionFromServer.version.ToString();
                logger.Debug("Ultima versión en el servidor: " + lastServerVersion);

                IDeviceHandler deviceHandler = App.Instance.deviceHandler;
                string deviceMainFileVersion =  deviceHandler.ReadVersionDeviceProgramFileFromDefaultData();
                logger.Debug("Versión del dispositivo: " + deviceMainFileVersion);

                if(!lastServerVersion.Equals(deviceMainFileVersion))
                {
                    NotifyCurrentMessage("Descargando componentes del dispositivo...");
                    List<VersionArchivo> filesVersions = lastVersionFromServer.versiones;
                    foreach(VersionArchivo fileVersion in filesVersions)
                    {
                        string name = fileVersion.nombre;
                        logger.Debug("Descargando " + name);
                        string fileVersionId = fileVersion.idVersion;
                        HttpWebClientUtil.DownloadDevicePrograms(fileVersionId, name);

                        string deviceRelPathBin = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_BIN);
                        string slashFilenameAndExtension = FileUtils.PrependSlash(name);
                        ResultFileOperation copyResult = deviceHandler.CopyPublicBinFileToDevice(deviceRelPathBin, slashFilenameAndExtension);
                        logger.Debug("Resultado de copiar el archivo " + copyResult.ToString());
                    }
                    logger.Info("Version obtenida del server: " + lastServerVersion);
                    FileUtils.UpdateDefaultDatFileInPublic(lastServerVersion, DeviceMainData.POSITION_VERSION);
                }
            }
        }

        private bool CheckStoreIdDevice()
        {
            bool needAssociate = false;
            IDeviceHandler deviceHandler = App.Instance.deviceHandler;
            string storeIDFromDevice = deviceHandler.ReadStoreIdFromDefaultData();
            string storeIdFromLogin = MyAppProperties.storeId;
            if(!TextUtils.CompareStoreId(storeIDFromDevice, storeIdFromLogin))
            {
                needAssociate = true;
            }
            return needAssociate;
        }

        private void AssociateCurrentStoreId(string sucursal)
        {
            FileUtils.UpdateDefaultDatFileInPublic(sucursal, DeviceMainData.POSITION_SUCURSAL);
            DeleteAllPreviousFiles(true);
        }

        private void DeleteAllPreviousFiles(bool isCompletedSynchronization)
        {
            IDeviceHandler deviceHandler = App.Instance.deviceHandler;
            List<Actividad> actividades = MyAppProperties.activitiesEnables;
            if(isCompletedSynchronization)
            {
                deviceHandler.DeleteDeviceAndPublicDataFiles(Constants.LPEDIDOS);
                deviceHandler.DeleteDeviceAndPublicDataFiles(Constants.APEDIDOS);
                deviceHandler.DeleteDeviceAndPublicDataFiles(Constants.EPEDIDOS);
                deviceHandler.DeleteDeviceAndPublicDataFiles(Constants.RPEDIDOS);
            }
            foreach(Actividad actividad in actividades)
            {
                long idActividadActual = actividad.idActividad;
                string filename = ArchivosDATUtils.GetDataFileNameByIdActividad((int)idActividadActual);
                deviceHandler.DeleteDeviceAndPublicDataFiles(filename);
            }
        }

        private void CheckLastSyncDateFromDefault(bool filesPreviouslyDeleted)
        {
            IDeviceHandler deviceHandler = App.Instance.deviceHandler;
            string synchronizationDefault = deviceHandler.ReadSynchronizationDateFromDefaultData();
            bool isGreatherThanToday = DateTimeUtils.IsGreatherThanToday(synchronizationDefault);
            if(isGreatherThanToday && !filesPreviouslyDeleted)
            {
                DeleteAllPreviousFiles(true);
            }
        }

        private void UpdateDeviceMainFile(string sucursal)
        {
            FileUtils.UpdateDeviceMainFileInPublic(sucursal);
            IDeviceHandler deviceHandler = App.Instance.deviceHandler;
            string destinationDirectory = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_LOOKUP);
            string filenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_DEFAULT);
            deviceHandler.CopyPublicLookUpFileToDevice(destinationDirectory, filenameAndExtension);
        }

        private void loadOnceCentroActividadesWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("loadOnceCentroActividades Worker ->runWorkedCompleted");
            PanelLoading = false;
            if (dispatcherTimer != null)
            {
                dispatcherTimer.Start();
            }
            MyAppProperties.needReloadActivityCenter = false;
        }
        #endregion

        #region reload Activity Center Worker
        private void reloadCentroActividadesWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Debug("Reload Activity Center Worker -> doWork");
            if (dispatcherTimer != null)
            {
                dispatcherTimer.Stop();
            }
            string currentMessage = "Checkeando conexión con el dispositivo...";
            NotifyCurrentMessage(currentMessage);
            bool isConneted = CheckDeviceConnected();

            if (isConneted)
            {
                PanelMainMessage = "Espere por favor...";
                currentMessage = "Leyendo Ajustes realizados...";
                NotifyCurrentMessage(currentMessage);
                CreateBadgeSeeAdjustments();
            }

            currentMessage = "Verificando conexión con el servidor PDAExpress...";
            NotifyCurrentMessage(currentMessage);
            bool serverStatus = CheckServerStatus();

            string storeId = MyAppProperties.storeId;
            string currentBatchId = MyAppProperties.currentBatchId;
            if (serverStatus)
            {
                currentMessage = "Obteniendo detalles de sincronización para lote " + currentBatchId + " ...";
                NotifyCurrentMessage(currentMessage);
                GetLastSync(Convert.ToInt32(currentBatchId), storeId);
            }
        }
        private void reloadCentroActividadesWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("ReloadCentroActividades Worker ->runWorkedCompleted");
            PanelLoading = false;
            if (dispatcherTimer != null)
            {
                dispatcherTimer.Start();
            }
        }
        #endregion

        #region Synchonization Worker
        private void syncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string currentMessage = "sincronizar Worker ->doWork";
            logger.Debug(currentMessage);

            if (dispatcherTimer != null)
            {
                dispatcherTimer.Stop();
            }

            currentMessage = "Checkeando conexión con el dispositivo...";
            NotifyCurrentMessage(currentMessage);
            bool isConneted = CheckDeviceConnected();

            if (isConneted)
            {
                currentMessage = "Leyendo la configuración del dispositivo...";
                NotifyCurrentMessage(currentMessage);
                CopyDeviceMainDataFileToPublic();

                currentMessage = "Actualizando el estado de la sincronización...";
                NotifyCurrentMessage(currentMessage);
                ChangeSyncState();

                currentMessage = "Verificando conexión con el servidor PDAExpress...";
                NotifyCurrentMessage(currentMessage);
                bool serverStatus = CheckServerStatus();

                string storeId = MyAppProperties.storeId;
                if (serverStatus)
                {
                    currentMessage = "Controlando nuevo lote ...";
                    NotifyCurrentMessage(currentMessage);
                    bool needAskDiscarOldBatch = VerifyNewBatch();
                    if(needAskDiscarOldBatch)
                    {
                        string messageOldBatch = "Aun no se han finalizado todas las Actividades de la ultima Sincronización. Si genera una nueva Sincronización no podrá continuar con las Actividades pendientes. ¿Desesa continuar de todos modos?";
                        bool continueOrCancel = AskCancelCurrentSynchronization(messageOldBatch);
                        if(continueOrCancel)
                        {
                            logger.Info("Cancelando operacion por tener lotes antiguos");
                            return;
                        }
                    }

                    currentMessage = "Controlando recepciones informadas ...";
                    NotifyCurrentMessage(currentMessage);
                    bool discardReceptionsIfThereWas = CheckInformedReceptions();
                    if(discardReceptionsIfThereWas)
                    {
                        string messageInformedReceptions = "Existen recepciones pendientes de informar, ¿Desea continuar y descartar las mismas?";
                        bool continueOrCancel = AskCancelCurrentSynchronization(messageInformedReceptions);
                        if (continueOrCancel)
                        {
                            logger.Info("Cancelando operacion por tener recepciones pendientes");
                            return;
                        }
                        
                    }

                    currentMessage = "Creando nuevo lote de sincronización ...";
                    NotifyCurrentMessage(currentMessage);
                    List<Sincronizacion> newSync = CreateNewBatch();

                    currentMessage = "Informando a genesix ...";
                    NotifyCurrentMessage(currentMessage);
                    InformToGenesix(newSync);
                    ExecuteInformGenesix(newSync);

                    currentMessage = "Borrando remante de archivos ...";
                    NotifyCurrentMessage(currentMessage);
                    DeleteAllPreviousFiles(false);

                    currentMessage = "Descargando archivos maestros...";
                    NotifyCurrentMessage(currentMessage);
                    DownloadMasterFile(newSync);

                    currentMessage = "Refrescando los datos de la tabla de sincronizaciones ...";
                    NotifyCurrentMessage(currentMessage);
                    UpdateDataGrid(storeId, newSync);

                    currentMessage = "Actualizando archivo principal de configuración del dispositivo ...";
                    NotifyCurrentMessage(currentMessage);
                    UpdateDeviceMainFile(storeId);

                    NotifyCurrentMessage("Todo listo!");
                }
                else
                {
                    logger.Debug("Servidor no disponible");
                }
            }
            else
            {
                logger.Debug("Dispositivo no conectado");
                ShowPanelNoConnection();
            }
        }

        private void ChangeSyncState()
        {
            string syncState = DeviceMainData.ESTADO_SINCRO_INICIADO.ToString();
            int syncPosition = DeviceMainData.POSITION_ESTADO_SINCRO;
            FileUtils.UpdateDefaultDatFileInPublic(syncState, syncPosition);
        }

        private bool VerifyNewBatch()
        {
            string idSucursal = MyAppProperties.storeId;
            ActionResultDto actionResult = HttpWebClientUtil.VerifyNewBatch(idSucursal);
            return !actionResult.success;
        }

        private bool AskCancelCurrentSynchronization(string message)
        {
            string caption = "Confirme";
            MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.OKCancel, MessageBoxImage.Error);
            return result.Equals(MessageBoxResult.Cancel);
        }

        private bool CheckInformedReceptions()
        {
            string sotreId = MyAppProperties.storeId;
            string batchId = HttpWebClientUtil.GetCurrentBatchId(sotreId).ToString();
            bool informedReceptions = HttpWebClientUtil.CheckInformedReceptions(batchId);
            logger.Info("recepciones Informadas pendientes: " + (informedReceptions ? "NO" : "SI"));

            return !informedReceptions;
        }

        private List<Sincronizacion> CreateNewBatch()
        {
            string storeId = MyAppProperties.storeId;
            bool isCompleted = MyAppProperties.isSynchronizationComplete;
            List<Sincronizacion> newSync = HttpWebClientUtil.CreateNewBatch(storeId, isCompleted);
            return newSync;
        }

        private void InformToGenesix(List<Sincronizacion> newSync)
        {
            foreach(Sincronizacion sync in newSync)
            {
                if (sync.actividad.accion.idAccion.Equals(Constants.INFORMAR_GENESIX))
                {
                    long syncId = sync.idSincronizacion;
                    int actionId = Convert.ToInt32(sync.actividad.idActividad);
                    try
                    {
                        string slashFilenameAndExtension = ArchivosDATUtils.GetDataFileNameAndExtensionByIdActividad(actionId);
                        IDeviceHandler deviceHandler = App.Instance.deviceHandler;
                        ResultFileOperation resultCopy = deviceHandler.CopyDeviceFileToPublicData(slashFilenameAndExtension);
                        if (resultCopy.Equals(ResultFileOperation.OK))
                        {
                            UploadFileToServer(slashFilenameAndExtension, actionId, syncId);
                            SynchronizationStateUtil.SetReceivedDeviceState(syncId);
                        }
                        else
                        {
                            SynchronizationStateUtil.SetNoDataDeviceState(syncId);
                            SynchronizationStateUtil.SetNoDataGenesixState(syncId);
                        }
                    }
                    catch
                    {
                        SynchronizationStateUtil.SetErrorDeviceState(syncId);
                    }
                }
            }
        }

        private void UploadFileToServer(string slashFilenameAndExtension, int actionId, long syncId)
        {
            try
            {
                string filePath = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);
                string parametros = "?idActividad={0}&idSincronizacion={1}&registros={2}";
                var lineCount = FileUtils.CountRegistryWithinFile(filePath + slashFilenameAndExtension);
                parametros = String.Format(parametros, actionId, syncId, lineCount);
                string respuesta = HttpWebClientUtil.UploadFileToServer(filePath + slashFilenameAndExtension, parametros);
                logger.Debug(respuesta);

                SynchronizationStateUtil.SetSentGenesixState(syncId);
            }
            catch
            {
                SynchronizationStateUtil.SetErrorGenesixGeneralState(syncId);
            }
        }

        private void ExecuteInformGenesix(List<Sincronizacion> newSync)
        {
            foreach (Sincronizacion sync in newSync)
            {
                if (sync.actividad.accion.idAccion.Equals(Constants.INFORMAR_GENESIX))
                {
                    long syncId = sync.idSincronizacion;
                    HttpWebClientUtil.ExecuteInformGenesix(syncId);
                }
            }
        }

        private void DownloadMasterFile(List<Sincronizacion> newSync)
        {
            foreach (Sincronizacion sync in newSync)
            {
                if (Constants.DESCARGAR_GENESIX.Equals(sync.actividad.accion.idAccion))
                {
                    long syncId = sync.idSincronizacion;
                    try
                    {
                        string storeId = MyAppProperties.storeId;
                        bool descargaMaestroCorrecta = HttpWebClientUtil.BuscarMaestrosDAT((int)sync.actividad.idActividad, storeId);
                        NotifyCurrentMessage("Descargando " + sync.actividad.descripcion.ToString());
                        Thread.Sleep(500);

                        if (descargaMaestroCorrecta)
                        {
                            if (sync.actividad.idActividad.Equals(Constants.ubicart))
                            {
                                logger.Debug("Ubicart -> creando Archivos PAS");
                                ArchivosDATUtils.crearArchivoPAS();
                            }
                            if (sync.actividad.idActividad.Equals(Constants.pedidos))
                            {
                                logger.Debug("Pedidos -> creando Archivos Pedidos");
                                ArchivosDATUtils.crearArchivosPedidos();
                            }

                            string destinationDirectory = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
                            string filename = ArchivosDATUtils.GetDataFileNameByIdActividad((int)sync.actividad.idActividad);
                            string filenameAndExtension = FileUtils.WrapSlashAndDATExtension(filename);

                            ResultFileOperation result = App.Instance.deviceHandler.CopyPublicDataFileToDevice(destinationDirectory, filenameAndExtension);
                            logger.Debug("result: " + result);
                            NotifyCurrentMessage("Moviendo al dispositivo");

                            SetReceivedFromGenesix(syncId);
                            SynchronizationStateUtil.SetSentDeviceState(syncId);
                            Thread.Sleep(500);
                        }

                    }catch (Exception e)
                    {
                        logger.Error(e.Message);
                        SetErrorGenesixGeneral(syncId);
                    }
                }
            }
        }

        private void SetReceivedFromGenesix(long syncId)
        {
            SynchronizationStateUtil.SetReceivedFromGenesixState(syncId);
        }

        private void SetErrorGenesixGeneral(long syncId)
        {
            SynchronizationStateUtil.SetErrorGenesixGeneralState(syncId);
        }

        private void UpdateDataGrid(string storeId, List<Sincronizacion> newSync)
        {
            string batchId = newSync[0].lote.idLote.ToString();
            string urlLastSync = ConfigurationManager.AppSettings.Get(Constants.API_SYNC_ULTIMA);
            List<Sincronizacion> lastSync = HttpWebClientUtil.GetHttpWebSynchronizations(urlLastSync, storeId, batchId);
            this.sincronizaciones = SincronizacionDtoDataGrid.ParserDataGrid(lastSync);
            UpdateCurrentBatch(sincronizaciones);
        }

        private void syncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("sincronizar Worker ->runWorkedCompleted");
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.BeginInvoke(new Action(() =>
            {
                PanelLoading = false;
            }));
            if (dispatcherTimer != null)
            {
                dispatcherTimer.Start();
            }
        }
        #endregion

        #region sync Worker
        private void syncDataGrid_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Info("<- Cargando la grilla");
            string urlSincronizacionAnterior = MyAppProperties.currentUrlSync;
            string _sucursal = MyAppProperties.storeId;
            string _idLote = MyAppProperties.currentBatchId;
            List<Sincronizacion> listaSincronizaciones = null;
            listaSincronizaciones = HttpWebClientUtil.GetHttpWebSynchronizations(urlSincronizacionAnterior, _sucursal, _idLote);
            if (listaSincronizaciones != null && listaSincronizaciones.Count != 0)
            {
                logger.Debug(listaSincronizaciones);
                this.sincronizaciones = SincronizacionDtoDataGrid.ParserDataGrid(listaSincronizaciones);
                UpdateCurrentBatch(sincronizaciones);
            }
        }
        private void syncDataGrid_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            PanelLoading = false;
            PanelMainMessage = "";
            PanelSubMessage = "";
        }
        #endregion

        #region Adjustment Worker
        private void adjustmentWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Debug("AdjustmentWorker -> Do worker started");
            bool estadoDevice = App.Instance.deviceHandler.IsDeviceConnected();
            if (estadoDevice)
            {
                MainWindow window = MyAppProperties.window;
                Uri uriSeeAdjustments = new Uri(Constants.VER_AJUSTES_REALIZADOS_VIEW, UriKind.Relative);

                var dispatcher = Application.Current.Dispatcher;
                dispatcher.BeginInvoke(new Action(() =>
                {
                    window.frame.NavigationService.Navigate(uriSeeAdjustments);
                }));
            }
            else
            {
                PanelLoading = true;
                AvisoAlUsuario("No se detecta conexion con la PDA");
            }
        }

        private void adjustmentWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("AdjustmentWorker -> Run worker completed");
        }
        #endregion

        #endregion

        #region Methods
        public void setInfoLabels()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            label_version = assembly.GetName().Version.ToString(3);
            // de donde obtenemos el usuario y sucursal (?)
            label_usuario = "Admin";
            label_sucursal = MyAppProperties.storeId;
            logger.Debug("setInfoLabels[version: " + label_version
                + ", usuario: " + label_usuario + ", sucursal: " + label_sucursal + "]");
        }

        public void ExitPortalApi(object obj)
        {
            logger.Info("exit portal api");
            //aca deberia invocar el logout al portal?
            MyAppProperties.loadOnce = true;
            MainWindow window = (MainWindow) Application.Current.MainWindow;
            Uri uri = new Uri(Constants.LOGIN_VIEW, UriKind.Relative);
            window.frame.NavigationService.Navigate(uri);
        }

        public void SincronizarTodosLosDatos(object obj)
        {
            PanelLoading = true;
            BannerApp.PrintSynchronization();
            MyAppProperties.isSynchronizationComplete = true;
            PanelMainMessage = "Sincronizando todos los datos, Espere por favor";
            logger.Info("Sincronizando todos los datos");
            syncWorker.RunWorkerAsync();
        }

        public void InformarGenesix(object obj)
        {
            PanelLoading = true;
            BannerApp.PrintInformGX();
            MyAppProperties.isSynchronizationComplete = false;
            PanelMainMessage = "Informando a genesix, Espere por favor";
            logger.Info("Informando a genesix");
            syncWorker.RunWorkerAsync();
        }

        public void VerAjustesRealizados(object obj)
        {
            PanelLoading = true;
            PanelMainMessage = "Cargando ajustes realizados, Espere por favor";
            PanelSubMessage = null;
            logger.Debug("Viendo ajustes realizados");
            adjustmentWorker.RunWorkerAsync();
        }

        public void CentroActividadesLoaded(object obj)
        {
            if (MyAppProperties.loadOnce)
            {
                loadOnceCentroActividadesWorker.RunWorkerAsync();
            }
            else
            {
                reloadCentroActividadesWorker.RunWorkerAsync();
            }
        }

        public void SincronizacionAnterior(object obj)
        {
            PanelLoading = true;
            PanelMainMessage = "Obteniendo sincronizaciones";
            PanelSubMessage = "Espere por favor";
            MyAppProperties.currentUrlSync = ConfigurationManager.AppSettings.Get(Constants.API_SYNC_ANTERIOR);
            syncDataGridWorker.RunWorkerAsync();
        }

        public void SincronizacionSiguiente(object obj)
        {
            PanelLoading = true;
            PanelMainMessage = "Obteniendo sincronizaciones";
            PanelSubMessage = "Espere por favor";
            MyAppProperties.currentUrlSync = ConfigurationManager.AppSettings.Get(Constants.API_SYNC_SIGUIENTE);
            syncDataGridWorker.RunWorkerAsync();
        }

        public void BuscarSincronizaciones(object obj)
        {
            logger.Info("Buscando sincronizaciones");
            BuscarLotesView buscarLotesView = new BuscarLotesView();
            buscarLotesView.ShowDialog();
        }

        public void GoLastSynchronization(object obj)
        {
            PanelLoading = true;
            PanelMainMessage = "Obteniendo sincronizaciones";
            PanelSubMessage = "Espere por favor";
            MyAppProperties.currentUrlSync = ConfigurationManager.AppSettings.Get(Constants.API_SYNC_ULTIMA);
            syncDataGridWorker.RunWorkerAsync();
        }

        public void AddCommandForButtonsState(List<SincronizacionDtoDataGrid> sincronizaciones)
        {
            foreach (SincronizacionDtoDataGrid sync in sincronizaciones)
            {
                sync.EstadoGeneralCommand = new RelayCommand(BotonEstadoGeneral, param => true);
                sync.EstadoGenesixCommand = new RelayCommand(BotonEstadoGenesix, param => true);
                sync.EstadoPDACommand = new RelayCommand(BotonEstadoPDA, param => true);
            }
        }

        public void UpdateCurrentBatch(List<SincronizacionDtoDataGrid> synchronizations)
        {
            AddCommandForButtonsState(sincronizaciones);
            if (synchronizations != null && synchronizations.Count != 0)
            {
                var s = synchronizations[0] as SincronizacionDtoDataGrid;
                string currentBatchId = s.lote;
                MyAppProperties.currentBatchId = currentBatchId;
                UpdateTxt_Synchronization(s);
            }
        }

        public void UpdateTxt_Synchronization(SincronizacionDtoDataGrid synchronization)
        {
            string currentBatch = synchronization.lote;
            string currentDate = synchronization.fecha;
            txt_sincronizacion = String.Format(" ({0}) {1}", currentBatch, currentDate);
        }

        #region Button States
        public void BotonEstadoGenesix(object obj)
        {
            logger.Info("Boton estado genesix: " + obj);
            logger.Info(MyAppProperties.SelectedSync.actividad);
        }
        public void BotonEstadoPDA(object obj)
        {
            logger.Info("Boton estado pda");
            logger.Info(MyAppProperties.SelectedSync.actividad);
        }
        public void BotonEstadoGeneral(object obj)
        {
            logger.Info("Boton estado general");
            logger.Info(MyAppProperties.SelectedSync.actividad);
            bool stateNeedResolve = ButtonStateUtils.ResolveState();
            if (stateNeedResolve)
            {
                MyAppProperties.currentUrlSync = ConfigurationManager.AppSettings.Get(Constants.API_SYNC_ULTIMA);
                syncDataGridWorker.RunWorkerAsync();
            }
        }
        #endregion


        #region Panel Methods
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
        #endregion

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
