using log4net;
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
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using ToastNotifications;
using ToastNotifications.Position;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;

namespace PDADesktop.ViewModel
{
    class CentroActividadesViewModel : ViewModelBase
    {
        #region Attributes
        #region Commons Attributes
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDialogCoordinator dialogCoordinator;
        private IDeviceHandler deviceHandler { get; set; }

        private List<SincronizacionDtoDataGrid> sincronizaciones;
        public List<SincronizacionDtoDataGrid> Sincronizaciones
        {
            get
            {
                return sincronizaciones;
            }
            set
            {
                sincronizaciones = value;
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

        private string textBox_sincronizacion;
        public string TextBox_sincronizacion
        {
            get
            {
                return textBox_sincronizacion;
            }
            set
            {
                textBox_sincronizacion = value;
                OnPropertyChanged();
            }
        }
        private string label_version;
        public string Label_version
        {
            get
            {
                return label_version;
            }
            set
            {
                label_version = value;
            }
        }
        private string label_usuario;
        public string Label_usuario
        {
            get
            {
                return label_usuario;
            }
            set
            {
                label_usuario = value;
            }
        }
        private string label_sucursal;
        public string Label_sucursal
        {
            get
            {
                return label_sucursal;
            }
            set
            {
                label_sucursal = value;
            }
        }

        private bool firstGroupPagination;
        public bool FirstGroupPagination
        {
            get
            {
                return firstGroupPagination;
            }
            set
            {
                firstGroupPagination = value;
                OnPropertyChanged();
            }
        }

        private bool lastGroupPagination;
        public bool LastGroupPagination
        {
            get
            {
                return lastGroupPagination;
            }
            set
            {
                lastGroupPagination = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands Attributes
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
        public ICommand CentroActividadesLoadedEvent
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

        private ICommand buscarLoteCommand;
        public ICommand BuscarLoteCommand
        {
            get
            {
                return buscarLoteCommand;
            }
            set
            {
                buscarLoteCommand = value;
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

        private ICommand panelCloseCommand_NC;
        public ICommand PanelCloseCommand_NC
        {
            get
            {
                return panelCloseCommand_NC;
            }
            set
            {
                panelCloseCommand_NC = value;
            }
        }

        private bool canExecute = true;
        #endregion

        #region Workers Attributes
        private readonly BackgroundWorker reloadActivityCenterWorker = new BackgroundWorker();
        private readonly BackgroundWorker syncDataGridWorker = new BackgroundWorker();
        private readonly BackgroundWorker adjustmentWorker = new BackgroundWorker();
        private readonly BackgroundWorker redirectWorker = new BackgroundWorker();
        #endregion

        #region Dispatcher Attributes
        private DispatcherTimer dispatcherTimer { get; set; }
        private Dispatcher dispatcher { get; set; }
        #endregion

        #region Loading Panel Attributes
        private bool panelLoading;
        public bool PanelLoading
        {
            get
            {
                return panelLoading;
            }
            set
            {
                panelLoading = value;
                OnPropertyChanged();
            }
        }
        private string panelMainMessage;
        public string PanelMainMessage
        {
            get
            {
                return panelMainMessage;
            }
            set
            {
                panelMainMessage = value;
                OnPropertyChanged();
            }
        }
        private string panelSubMessage;
        public string PanelSubMessage
        {
            get
            {
                return panelSubMessage;
            }
            set
            {
                panelSubMessage = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Panel No Connection Attributes
        private bool panelLoading_NC;
        public bool PanelLoading_NC
        {
            get
            {
                return panelLoading_NC;
            }
            set
            {
                panelLoading_NC = value;
                OnPropertyChanged();
            }
        }
        private string panelMainMessage_NC;
        public string PanelMainMessage_NC
        {
            get
            {
                return panelMainMessage_NC;
            }
            set
            {
                panelMainMessage_NC = value;
                OnPropertyChanged();
            }
        }
        private string panelSubMessage_NC;
        public string PanelSubMessage_NC
        {
            get
            {
                return panelSubMessage_NC;
            }
            set
            {
                panelSubMessage_NC = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Notifier Attributes
        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.BottomRight,
                offsetX: 10,
                offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(5),
                maximumNotificationCount: MaximumNotificationCount.FromCount(3));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });
        #endregion
        #endregion

        #region Constructor
        public CentroActividadesViewModel(IDialogCoordinator instance)
        {
            BannerApp.PrintActivityCenter();
            PanelLoading_NC = false;
            HidingNoConnectionPanel();
            DisplayWaitingPanel("Cargando...");
            dialogCoordinator = instance;
            dispatcher = App.Instance.Dispatcher;
            deviceHandler = App.Instance.deviceHandler;
            SetGroupPaginator(true, false);
            SetInfoLabels();

            CentroActividadesLoadedEvent = new RelayCommand(ActivityCenterLoadedEventAction, param => this.canExecute);

            ExitButtonCommand = new RelayCommand(ExitPortalApiAction, param => this.canExecute);
            SincronizarCommand = new RelayCommand(SyncAllDataAction, param => this.canExecute);
            InformarCommand = new RelayCommand(InformGenesixAction, param => this.canExecute);
            VerAjustesRealizadosCommand = new RelayCommand(SeeAdjustmentMadeAction, param => this.canExecute);
            AnteriorCommand = new RelayCommand(PreviousSyncAction, param => this.canExecute);
            SiguienteCommand = new RelayCommand(NextSyncAction, param => this.canExecute);
            UltimaCommand = new RelayCommand(LastSyncAction, param => this.canExecute);
            BuscarLoteCommand = new RelayCommand(SearchBatchAction);

            EstadoGeneralCommand = new RelayCommand(GeneralStateAction, param => this.canExecute);

            reloadActivityCenterWorker.DoWork += reloadActivityCenterWorker_DoWork;
            reloadActivityCenterWorker.RunWorkerCompleted += reloadActivityCenterWorker_RunWorkerCompleted;

            syncDataGridWorker.DoWork += syncDataGrid_DoWork;
            syncDataGridWorker.RunWorkerCompleted += syncDataGrid_RunWorkerCompleted;

            adjustmentWorker.DoWork += adjustmentWorker_DoWork;
            adjustmentWorker.RunWorkerCompleted += adjustmentWorker_RunWorkerCompleted;

            redirectWorker.DoWork += redirectWorker_DoWork;
            redirectWorker.RunWorkerCompleted += redirectWorker_RunwWorkerCompleted;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 15);

            ShowPanelCommand = new RelayCommand(ShowPanelAction);
            PanelCloseCommand = new RelayCommand(ClosePanelAction, param => this.canExecute);
            PanelCloseCommand_NC = new RelayCommand(ClosePanelNoConnectionAction);
        }
        #endregion

        #region Loaded Event
        public async void ActivityCenterLoadedEventAction(object obj)
        {
            if (MyAppProperties.loadOnce)
            {
                await Task.Run(async () =>
                {
                    await loadOnceActivityCenter_DoWork();
                });
                loadOnceActivityCenter_RunCompleted();
            }
            else
            {
                reloadActivityCenterWorker.RunWorkerAsync();
            }
        }
        #endregion

        #region dispatcherTimer
        private async void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (!adjustmentWorker.IsBusy || !redirectWorker.IsBusy || !reloadActivityCenterWorker.IsBusy)
            {
                bool deviceStatus = deviceHandler.IsDeviceConnected();

                if (!deviceStatus)
                {
                    MyAppProperties.needReloadActivityCenter = true;
                    await dispatcher.BeginInvoke(new Action(() =>
                    {
                        DisplayDefaultNoConnectionPanel();
                    }));
                }
                else
                {
                    await dispatcher.BeginInvoke(new Action(() =>
                    {
                        HidingNoConnectionPanel();
                    }));
                    if (MyAppProperties.needReloadActivityCenter)
                    {
                        await dispatcher.BeginInvoke(new Action(() =>
                        {
                            DisplayWaitingPanel(String.Empty);
                        }));
                        await loadOnceActivityCenter_DoWork();
                        loadOnceActivityCenter_RunCompleted();
                        MyAppProperties.needReloadActivityCenter = false;
                    }
                }
                CommandManager.InvalidateRequerySuggested();
            }
            else
            {
                logger.Debug("Dispatcher time tick Skipeado");
            }
        }
        #endregion

        #region Workers Method
        #region Load Once Activity Center
        private async Task loadOnceActivityCenter_DoWork()
        {
            string currentMessage = "Load Once Activity Center => Do Work";
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

                if(actionsAvailables == null || actionsAvailables.Count == 0)
                {
                    await dispatcher.BeginInvoke(new Action(() => {
                        AlertUserMetroDialog("No se ha podido obtener actiones disponibles");
                    }));
                    return;
                }

                currentMessage = "Obteniendo actividades de todas las acciones...";
                NotifyCurrentMessage(currentMessage);
                GetActivitiesByActions(actionsAvailables);

                currentMessage = "Obteniendo el id del último lote para sucursal " + storeId + " ...";
                NotifyCurrentMessage(currentMessage);
                int currentBatchId = HttpWebClientUtil.GetLastBatchId(storeId);

                currentMessage = "Obteniendo detalles de sincronización para lote " + currentBatchId + " ...";
                NotifyCurrentMessage(currentMessage);
                GetLastSync(currentBatchId, storeId);
            }
            else
            {
                logger.Info("Server no disponible");
                await dispatcher.BeginInvoke(new Action(() => {
                    notifier.ShowWarning("El servidor PDAExpress no ha respondido a tiempo.");
                }));
            }

            currentMessage = "Checkeando conexión con el dispositivo...";
            NotifyCurrentMessage(currentMessage);
            bool isConneted = CheckDeviceConnected();

            if(isConneted)
            {
                PanelMainMessage = "Espere por favor...";

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

                currentMessage = "Verificando remanente de archivos ...";
                NotifyCurrentMessage(currentMessage);
                bool hasOldData = CheckLastSyncDateFromDefault(filesPreviouslyDeleted);

                if(hasOldData)
                {
                    string messageDeleteAllPreviousFile = "El dispositivo contiene datos antiguos. ¿Confirma que desea descartarlos?";
                    bool continueOrCancel = await AskUserMetroDialog(messageDeleteAllPreviousFile);
                    if (continueOrCancel)
                    {
                        currentMessage = "Borrando datos antiguos ...";
                        NotifyCurrentMessage(currentMessage);
                        DeleteAllPreviousFiles(true);

                        currentMessage = "Actualizando archivo principal de configuración del dispositivo ...";
                        NotifyCurrentMessage(currentMessage);
                        UpdateDeviceMainFile(storeId);
                    }
                    else
                    {
                        logger.Info("El usuario no desea borrar los archivos antiguos");
                    }
                }

                currentMessage = "Leyendo Ajustes realizados...";
                NotifyCurrentMessage(currentMessage);
                CreateBadgeSeeAdjustments();
            }
            else
            {
                logger.Info("Dispositivo no detectado");
                CreateBadgeSeeAdjustments();
                DisplayDefaultNoConnectionPanel();
            }
            MyAppProperties.loadOnce = false;
            DeleteTempFiles();
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
                    this.Sincronizaciones = SincronizacionDtoDataGrid.ParserDataGrid(syncList);
                    UpdateCurrentBatch(Sincronizaciones);
                }
            }
        }

        public void GetActualSync(int currentBatchId, string storeId)
        {
            if (currentBatchId != 0)
            {
                MyAppProperties.currentBatchId = currentBatchId.ToString();
                List<Sincronizacion> syncList = null;
                string urlPathLastSync = ConfigurationManager.AppSettings.Get(Constants.API_SYNC_ACTUAL);
                syncList = HttpWebClientUtil.GetHttpWebSynchronizations(urlPathLastSync, storeId, currentBatchId.ToString());
                if (syncList != null && syncList.Count != 0)
                {
                    this.Sincronizaciones = SincronizacionDtoDataGrid.ParserDataGrid(syncList);
                    UpdateCurrentBatch(Sincronizaciones);
                }
            }
        }

        private bool CheckDeviceConnected()
        {
            bool isDeviceConnected = deviceHandler.IsDeviceConnected();
            logger.Info("Device is connected: " + isDeviceConnected);
            return isDeviceConnected;
        }

        public void CreateBadgeSeeAdjustments()
        {
            dispatcher.BeginInvoke(new Action(() =>
            {
                Button buttonSeeAdjustment = new Button();
                buttonSeeAdjustment.Name = "button_verAjustesRealizados";
                buttonSeeAdjustment.Content = "Ver Ajustes";
                buttonSeeAdjustment.HorizontalAlignment = HorizontalAlignment.Left;
                buttonSeeAdjustment.VerticalAlignment = VerticalAlignment.Top;
                buttonSeeAdjustment.ToolTip = "Ver los ajustes realizados.";
                buttonSeeAdjustment.Command = this.VerAjustesRealizadosCommand;

                Badged badge = new Badged();
                bool estadoDevice = deviceHandler.IsDeviceConnected();
                if (estadoDevice)
                {
                    string DeviceAjusteFile = deviceHandler.ReadAdjustmentsDataFile();
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
            try
            {
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
            catch
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
            if(latestVersionsFromServer != null && latestVersionsFromServer.Count > 1)
            {
                VersionDispositivo lastVersionFromServer = latestVersionsFromServer[0];
                string lastServerVersion = lastVersionFromServer.version.ToString();
                logger.Debug("Ultima versión en el servidor: " + lastServerVersion);

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
                        try
                        {
                            ResultFileOperation copyResult = deviceHandler.CopyPublicBinFileToDevice(deviceRelPathBin, slashFilenameAndExtension);
                            logger.Debug("Resultado de copiar el archivo " + copyResult.ToString());
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex);
                        }
                    }
                    logger.Info("Version obtenida del server: " + lastServerVersion);
                    FileUtils.UpdateDefaultDatFileInPublic(lastServerVersion, DeviceMainData.POSITION_VERSION);
                }
            }
        }

        private bool CheckStoreIdDevice()
        {
            bool needAssociate = false;
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
            string destinationDirectory = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_LOOKUP);
            string filenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_DEFAULT);
            deviceHandler.CopyPublicLookUpFileToDevice(destinationDirectory, filenameAndExtension);
            NotifyCurrentMessage("Borrando remanente de archivos");
            DeleteAllPreviousFiles(true);
        }

        private void DeleteAllPreviousFiles(bool isCompletedSynchronization)
        {
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
                if(isCompletedSynchronization || actividad.accion.idAccion.Equals(Constants.INFORMAR_GENESIX))
                {
                    long idActividadActual = actividad.idActividad;
                    string filename = ArchivosDATUtils.GetDataFileNameByIdActividad((int)idActividadActual);
                    deviceHandler.DeleteDeviceAndPublicDataFiles(filename);
                }
            }
        }

        private bool CheckLastSyncDateFromDefault(bool filesPreviouslyDeleted)
        {
            string synchronizationDefault = deviceHandler.ReadSynchronizationDateFromDefaultData();
            if(synchronizationDefault == null)
            {
                return false;
            }
            bool isGreatherThanToday = DateTimeUtils.IsGreatherThanToday(synchronizationDefault);
            return isGreatherThanToday && !filesPreviouslyDeleted;
        }

        private void UpdateDeviceMainFile(string sucursal)
        {
            FileUtils.UpdateDeviceMainFileInPublic(sucursal);
            string destinationDirectory = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_LOOKUP);
            string filenameAndExtension = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_DEFAULT);
            deviceHandler.CopyPublicLookUpFileToDevice(destinationDirectory, filenameAndExtension);
        }

        private void DeleteTempFiles()
        {
            FileUtils.DeleteTempFiles();
        }

        private void loadOnceActivityCenter_RunCompleted()
        {
            logger.Debug("load once centro Actividades => Run Worker Completed");
            MyAppProperties.needReloadActivityCenter = false;
            HidingWaitingPanel();
            if (dispatcherTimer != null && !dispatcherTimer.IsEnabled)
            {
                dispatcherTimer.Start();
            }
        }
        #endregion

        #region Reload Activity Center Worker
        private void reloadActivityCenterWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Debug("reload activity center => Do Work");
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
            if (serverStatus)
            {
                string storeId = MyAppProperties.storeId;
                currentMessage = "Obteniendo el id del último lote para sucursal " + storeId + " ...";
                NotifyCurrentMessage(currentMessage);
                string lastBatchId = HttpWebClientUtil.GetLastBatchId(storeId).ToString();

                string currentBatchId = MyAppProperties.currentBatchId;
                if(currentBatchId == null)
                {
                    currentBatchId = lastBatchId;
                    SetGroupPaginator(true, false);
                }
                else
                {
                    SetGroupPaginator(true, true);
                }

                currentMessage = "Obteniendo detalles de sincronización para lote " + currentBatchId + " ...";
                NotifyCurrentMessage(currentMessage);
                GetActualSync(Convert.ToInt32(currentBatchId), storeId);

            }
            else
            {
                dispatcher.BeginInvoke(new Action(() => {
                    notifier.ShowWarning("El servidor PDAExpress no ha respondido a tiempo.");
                }));
            }
        }
        private void reloadActivityCenterWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("reload activity center => Run Worker Completed");
            HidingWaitingPanel();
            if (dispatcherTimer != null)
            {
                dispatcherTimer.Start();
            }
        }
        #endregion

        #region Synchronization
        private async Task synchronization_DoWork()
        {
            string currentMessage = "sync => Do Work";
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

                currentMessage = "Verificando remanente de archivos ...";
                NotifyCurrentMessage(currentMessage);
                bool hasOldData = CheckLastSyncDateFromDefault(false);

                if (hasOldData)
                {
                    string messageDeleteAllPreviousFile = "El dispositivo contiene datos antiguos. ¿Confirma que desea descartarlos?";
                    bool continueOrCancel = await AskUserMetroDialog(messageDeleteAllPreviousFile);
                    if (continueOrCancel)
                    {
                        currentMessage = "Borrando datos antiguos ...";
                        NotifyCurrentMessage(currentMessage);
                        DeleteAllPreviousFiles(true);

                        currentMessage = "Actualizando el estado de la sincronización...";
                        NotifyCurrentMessage(currentMessage);
                        ChangeSyncState();
                    }
                    else
                    {
                        logger.Info("El usuario no desea borrar los archivos antiguos");
                        return;
                    }
                }

                currentMessage = "Verificando conexión con el servidor PDAExpress...";
                NotifyCurrentMessage(currentMessage);
                bool serverStatus = CheckServerStatus();

                string storeId = MyAppProperties.storeId;
                string username = MyAppProperties.username;
                if (serverStatus)
                {
                    currentMessage = "Controlando nuevo lote ...";
                    NotifyCurrentMessage(currentMessage);
                    bool needAskDiscarOldBatch = VerifyNewBatch();
                    if(needAskDiscarOldBatch)
                    {
                        string messageOldBatch = "Aun no se han finalizado todas las Actividades de la ultima Sincronización. Si genera una nueva Sincronización no podrá continuar con las Actividades pendientes. ¿Desesa continuar de todos modos?";
                        bool continueOrCancel = await AskUserMetroDialog(messageOldBatch);
                        if (!continueOrCancel)
                        {
                            logger.Info("Cancelando operacion por tener lotes antiguos");
                            return;
                        }
                    }

                    currentMessage = "Controlando recepciones informadas ...";
                    NotifyCurrentMessage(currentMessage);
                    bool discardReceptionsIfThereWas = HasNotInformedAnNotRuledOutReceptions();
                    if(discardReceptionsIfThereWas)
                    {
                        string messageInformedReceptions = "Existen recepciones pendientes de informar, ¿Desea continuar y descartar las mismas?";
                        bool continueOrCancel = await AskUserMetroDialog(messageInformedReceptions);
                        if (!continueOrCancel)
                        {
                            logger.Info("Cancelando operacion por tener recepciones pendientes");
                            return;
                        }
                        
                    }

                    currentMessage = "Creando nuevo lote de sincronización ...";
                    NotifyCurrentMessage(currentMessage);
                    List<Sincronizacion> newSync = CreateNewBatch();

                    if(newSync == null)
                    {
                        await dispatcher.BeginInvoke(new Action(() => {
                            AlertUserMetroDialog("No se ha podido crear una nueva sincronizacion");
                        }));
                        return;
                    }

                    currentMessage = "Informando a genesix ...";
                    NotifyCurrentMessage(currentMessage);
                    InformToGenesix(newSync);
                    ExecuteInformGenesix(newSync, username);

                    currentMessage = "Borrando remanente de archivos ...";
                    NotifyCurrentMessage(currentMessage);
                    DeleteAllPreviousFiles(false);

                    if(MyAppProperties.isSynchronizationComplete)
                    {
                        currentMessage = "Descargando archivos maestros...";
                        NotifyCurrentMessage(currentMessage);
                        DownloadMasterFile(newSync);
                    }

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
                    await dispatcher.BeginInvoke(new Action(() => {
                        notifier.ShowWarning("El servidor PDAExpress no ha respondido a tiempo.");
                    }));
                }
            }
            else
            {
                logger.Debug("Dispositivo no conectado");
                DisplayDefaultNoConnectionPanel();
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

        private bool HasNotInformedAnNotRuledOutReceptions()
        {
            string sotreId = MyAppProperties.storeId;
            string batchId = HttpWebClientUtil.GetLastBatchId(sotreId).ToString();
            bool informedReceptions = HttpWebClientUtil.HasNotInformedAndNotRuledOutReceptions(batchId);
            logger.Info("recepciones no-Informadas y/o no-descartadas: " + (informedReceptions ? "SI" : "NO"));

            return informedReceptions;
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
                        ResultFileOperation resultCopy = deviceHandler.CopyDeviceFileToPublicData(slashFilenameAndExtension);
                        if (resultCopy.Equals(ResultFileOperation.OK))
                        {
                            UploadFileToServer(slashFilenameAndExtension, actionId, syncId);
                            SynchronizationStateUtil.SetReceivedDeviceState(syncId);

                            logger.Debug("action subida con exito, id: " + actionId);
                            if(actionId.Equals(Constants.ACTIVIDAD_IMPRIMIR_ETIQUETAS))
                            {
                                SynchronizationStateUtil.SetPrintReceptions(syncId);
                            }
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
                string filePathExtended = TextUtils.ExpandEnviromentVariable(filePath);
                string parametros = "?idActividad={0}&idSincronizacion={1}&registros={2}";
                var lineCount = FileUtils.CountRegistryWithinFile(filePathExtended + slashFilenameAndExtension);
                parametros = String.Format(parametros, actionId, syncId, lineCount);
                string respuesta = HttpWebClientUtil.UploadFileToServer(filePathExtended + slashFilenameAndExtension, parametros);
                logger.Debug(respuesta);

                SynchronizationStateUtil.SetSentGenesixState(syncId);
            }
            catch
            {
                SynchronizationStateUtil.SetErrorGenesixGeneralState(syncId);
            }
        }

        private void ExecuteInformGenesix(List<Sincronizacion> newSync, string username)
        {
            foreach (Sincronizacion sync in newSync)
            {
                if (sync.actividad.accion.idAccion.Equals(Constants.INFORMAR_GENESIX))
                {
                    long syncId = sync.idSincronizacion;
                    HttpWebClientUtil.ExecuteInformGenesix(syncId, username);
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
                        string userName = MyAppProperties.username;
                        bool descargaMaestroCorrecta = HttpWebClientUtil.SearchDATsMasterFile((int)sync.actividad.idActividad, storeId, userName);
                        NotifyCurrentMessage("Descargando " + sync.actividad.descripcion.ToString());
                        Thread.Sleep(500);

                        if (descargaMaestroCorrecta)
                        {
                            if (sync.actividad.idActividad.Equals(Constants.UBICART_CODE))
                            {
                                logger.Debug("Ubicart -> creando Archivos PAS");
                                ArchivosDATUtils.createPASFile();
                            }
                            if (sync.actividad.idActividad.Equals(Constants.PEDIDOS_CODE))
                            {
                                logger.Debug("Pedidos -> creando Archivos Pedidos");
                                ArchivosDATUtils.createOrdersFiles();
                            }

                            string destinationDirectory = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
                            string filename = ArchivosDATUtils.GetDataFileNameByIdActividad((int)sync.actividad.idActividad);
                            string filenameAndExtension = FileUtils.WrapSlashAndDATExtension(filename);

                            ResultFileOperation result = deviceHandler.CopyPublicDataFileToDevice(destinationDirectory, filenameAndExtension);
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
            this.Sincronizaciones = SincronizacionDtoDataGrid.ParserDataGrid(lastSync);
            UpdateCurrentBatch(Sincronizaciones);
        }

        private void synchronization_RunCompleted()
        {
            logger.Debug("sync => Run Worker Completed");
            dispatcher.BeginInvoke(new Action(() =>
            {
                HidingWaitingPanel();
            }));
            if (dispatcherTimer != null)
            {
                dispatcherTimer.Start();
            }
        }
        #endregion

        #region Sync Data Grid Worker
        private void syncDataGrid_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Info("sync data grid => Do Work");
            Thread.Sleep(500);
            string urlSincronizacionAnterior = MyAppProperties.currentUrlSync;
            string _sucursal = MyAppProperties.storeId;
            string _idLote = MyAppProperties.currentBatchId;
            List<Sincronizacion> listaSincronizaciones = null;
            listaSincronizaciones = HttpWebClientUtil.GetHttpWebSynchronizations(urlSincronizacionAnterior, _sucursal, _idLote);
            if (listaSincronizaciones != null && listaSincronizaciones.Count != 0)
            {
                logger.Debug(listaSincronizaciones);
                this.Sincronizaciones = SincronizacionDtoDataGrid.ParserDataGrid(listaSincronizaciones);
                UpdateCurrentBatch(Sincronizaciones);
            }
            else
            {
                dispatcher.BeginInvoke(new Action(() => {
                    notifier.ShowWarning("El servidor PDAExpress no ha respondido como se esperaba.");
                }));
            }
        }
        private void syncDataGrid_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Info("sync data grid => Run Worker Completed");
            HidingWaitingPanel();
        }
        #endregion

        #region Adjustment Worker
        private void adjustmentWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Debug("adjustment => Do Work");
            bool estadoDevice = deviceHandler.IsDeviceConnected();
            if (estadoDevice)
            {
                MainWindow window = MyAppProperties.window;
                Uri uriSeeAdjustments = new Uri(Constants.VER_AJUSTES_REALIZADOS_VIEW, UriKind.Relative);

                dispatcher.BeginInvoke(new Action(() =>
                {
                    window.frame.NavigationService.Navigate(uriSeeAdjustments);
                }));
            }
            else
            {
                AlertUserMetroDialog("No se detecta conexion con la PDA");
            }
        }

        private void adjustmentWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("adjustment => Run Worker Completed");
        }
        #endregion

        #region Redirect Worker
        private void redirectWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            dispatcher.BeginInvoke(new Action(() =>
            {
                DisplayWaitingPanel("Espere por favor");
            }));
            Thread.Sleep(800);
            logger.Debug("Redirect => Do Work");

            bool stateNeedResolve = ButtonStateUtils.ResolveState();
            if (stateNeedResolve)
            {
                logger.Debug("Resolucion de estado positiva..");
                MyAppProperties.currentUrlSync = ConfigurationManager.AppSettings.Get(Constants.API_SYNC_ULTIMA);
                syncDataGridWorker.RunWorkerAsync();
            }
        }

        private void redirectWorker_RunwWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("adjustment => Run Worker Completed");
            HidingWaitingPanel();
        }
        #endregion
        #endregion

        #region Commons Methods
        public void SetGroupPaginator(bool firstGroup, bool lastGroup)
        {
            SetFirstGroupPaginator(firstGroup);
            SetLastGroupPaginator(lastGroup);
        }

        private void SetFirstGroupPaginator(bool firstGroup)
        {
            FirstGroupPagination = firstGroup;
        }
        private void SetLastGroupPaginator(bool lastGroup)
        {
            LastGroupPagination = lastGroup;
        }

        public void SetInfoLabels()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Label_version = assembly.GetName().Version.ToString(3);
            // de donde obtenemos el usuario y sucursal (?)
            Label_usuario = MyAppProperties.username;
            Label_sucursal = MyAppProperties.storeId;
            logger.Debug("setInfoLabels[version: " + Label_version
                + ", usuario: " + Label_usuario + ", sucursal: " + Label_sucursal + "]");
        }

        public void AddCommandForButtonsState(List<SincronizacionDtoDataGrid> sincronizaciones)
        {
            foreach (SincronizacionDtoDataGrid sync in sincronizaciones)
            {
                sync.EstadoGeneralCommand = new RelayCommand(GeneralStateAction, param => true);
            }
        }

        public void UpdateCurrentBatch(List<SincronizacionDtoDataGrid> synchronizations)
        {
            AddCommandForButtonsState(Sincronizaciones);
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
            TextBox_sincronizacion = String.Format(" ({0}) {1}", currentBatch, currentDate);
        }
        #endregion

        #region Actions Methods
        public void ExitPortalApiAction(object obj)
        {
            logger.Info("exit portal api");

            MyAppProperties.loadOnce = true;
            MainWindow window = (MainWindow) Application.Current.MainWindow;
            Uri uri = new Uri(Constants.LOGIN_VIEW, UriKind.Relative);
            window.frame.NavigationService.Navigate(uri);
        }

        public async void SyncAllDataAction(object obj)
        {
            DisplayWaitingPanel("Sincronizando todos los datos, Espere por favor");
            BannerApp.PrintSynchronization();
            MyAppProperties.isSynchronizationComplete = true;
            logger.Info("Sincronizando todos los datos");

            await Task.Run( async () => {
                await synchronization_DoWork();
            });
            synchronization_RunCompleted();
        }

        public async void InformGenesixAction(object obj)
        {
            DisplayWaitingPanel("Informando a genesix, Espere por favor");
            BannerApp.PrintInformGX();
            MyAppProperties.isSynchronizationComplete = false;
            logger.Info("Informando a genesix");
            await Task.Run(async () => {
                await synchronization_DoWork();
            });
            synchronization_RunCompleted();
        }

        public void SeeAdjustmentMadeAction(object obj)
        {
            DisplayWaitingPanel("Cargando ajustes realizados", "Espere por favor");
            logger.Debug("Viendo ajustes realizados");
            adjustmentWorker.RunWorkerAsync();
        }

        public void PreviousSyncAction(object obj)
        {
            DisplayWaitingPanel("Obteniendo sincronizaciones", "Espere por favor");
            SetLastGroupPaginator(true);
            MyAppProperties.currentUrlSync = ConfigurationManager.AppSettings.Get(Constants.API_SYNC_ANTERIOR);
            if (!syncDataGridWorker.IsBusy)
            {
                syncDataGridWorker.RunWorkerAsync();
            }
        }

        public void NextSyncAction(object obj)
        {
            DisplayWaitingPanel("Obteniendo sincronizaciones", "Espere por favor");
            SetFirstGroupPaginator(true);
            MyAppProperties.currentUrlSync = ConfigurationManager.AppSettings.Get(Constants.API_SYNC_SIGUIENTE);
            if (!syncDataGridWorker.IsBusy)
            {
                syncDataGridWorker.RunWorkerAsync();
            }
        }

        public void LastSyncAction(object obj)
        {
            DisplayWaitingPanel("Obteniendo sincronizaciones", "Espere por favor");
            SetFirstGroupPaginator(true);
            MyAppProperties.currentUrlSync = ConfigurationManager.AppSettings.Get(Constants.API_SYNC_ULTIMA);
            if(!syncDataGridWorker.IsBusy)
            {
                syncDataGridWorker.RunWorkerAsync();
            }
        }

        public void SearchBatchAction(object obj)
        {
            DisplayWaitingPanel("Buscando Lotes...");
            Thread.Sleep(400);
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            Uri uri = new Uri(Constants.BUSCAR_LOTES_VIEW, UriKind.Relative);
            window.frame.NavigationService.Navigate(uri);
        }
        #endregion

        #region States Action Methods
        public void GeneralStateAction(object obj)
        {
            logger.Info("Boton estado general");
            logger.Info(MyAppProperties.SelectedSync.actividad);
            redirectWorker.RunWorkerAsync();
        }
        #endregion

        #region Panel Methods
        public void DisplayWaitingPanel(string mainMessage, string subMessage = "")
        {
            PanelLoading = true;
            PanelMainMessage = mainMessage;
            PanelSubMessage = subMessage;
        }
        public void HidingWaitingPanel()
        {
            PanelLoading = false;
            PanelMainMessage = String.Empty;
            PanelSubMessage = String.Empty;
        }

        public void ShowPanelAction(object obj)
        {
            DisplayWaitingPanel(String.Empty);
        }

        public void ClosePanelAction(object obj)
        {
            HidingWaitingPanel();
        }

        public void DisplayDefaultNoConnectionPanel()
        {
            logger.Debug("Displat Default NoConnection Panel <- llamado");
            string deviceName = deviceHandler.GetNameToDisplay();
            string mainMessage = "Se ha perdido la conexión con " + deviceName;
            string subMessage = "Reintentando..";
            DisplayNoConnectionPanel(mainMessage, subMessage);
        }
        public void DisplayNoConnectionPanel(string mainMessage, string subMessage = "")
        {
            PanelLoading_NC = true;
            PanelMainMessage_NC = mainMessage;
            PanelSubMessage_NC = subMessage;
        }
        public void HidingNoConnectionPanel()
        {
            PanelLoading_NC = false;
            PanelMainMessage_NC = String.Empty;
            PanelSubMessage_NC = String.Empty;
        }

        public void ClosePanelNoConnectionAction(object obj)
        {
            PanelLoading_NC = false;
        }
        #endregion

        #region Metro Dialog Methods
        private async Task<bool> AskUserMetroDialog(string message, string title = "Aviso")
        {
            MessageDialogStyle messageDialogStyle = MessageDialogStyle.AffirmativeAndNegative;
            bool userResponse = await ShowMetroDialog(messageDialogStyle, message, title);
            return userResponse;
        }

        private async void AlertUserMetroDialog(string message)
        {
            MessageDialogStyle messageDialogStyle = MessageDialogStyle.Affirmative;
            await ShowMetroDialog(messageDialogStyle, message);
        }

        private async Task<bool> ShowMetroDialog(MessageDialogStyle messageDialogStyle, string message, string title = "Aviso")
        {
            MetroDialogSettings metroDialogSettings = new MetroDialogSettings();
            metroDialogSettings.AffirmativeButtonText = "Aceptar";
            metroDialogSettings.NegativeButtonText = "Cancelar";
            MessageDialogResult userResponse = await dialogCoordinator.ShowMessageAsync(this, title, message, messageDialogStyle, metroDialogSettings);
            return userResponse == MessageDialogResult.Affirmative;
        }
        #endregion
    }
}
