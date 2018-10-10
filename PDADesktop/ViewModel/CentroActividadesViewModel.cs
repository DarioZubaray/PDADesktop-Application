using log4net;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
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
        private readonly BackgroundWorker loadCentroActividadesWorker = new BackgroundWorker();
        private readonly BackgroundWorker syncrWorker = new BackgroundWorker();

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

        private Badged badge_verAjustes;
        public Badged Badge_verAjustes
        {
            get
            {
                return badge_verAjustes;
            }
            set
            {
                badge_verAjustes = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public CentroActividadesViewModel()
        {
            BannerApp.PrintActivityCenter();
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
            UltimaCommand = new RelayCommand(GoLastSynchronization, param => this.canExecute);
            EstadoGenesixCommand = new RelayCommand(BotonEstadoGenesix, param => this.canExecute);
            EstadoPDACommand = new RelayCommand(BotonEstadoPDA, param => this.canExecute);
            EstadoGeneralCommand = new RelayCommand(BotonEstadoGeneral, param => this.canExecute);

            loadCentroActividadesWorker.DoWork += loadActivityCenterWorker_DoWork;
            loadCentroActividadesWorker.RunWorkerCompleted += loadCentroActividadesWorker_RunWorkerCompleted;
            
            syncrWorker.DoWork += syncWorker_DoWork;
            syncrWorker.RunWorkerCompleted += syncWorker_RunWorkerCompleted;

            ShowPanelCommand = new RelayCommand(MostrarPanel, param => this.canExecute);
            HidePanelCommand = new RelayCommand(OcultarPanel, param => this.canExecute);
            ChangeMainMessageCommand = new RelayCommand(CambiarMainMensage, param => this.canExecute);
            ChangeSubMessageCommand = new RelayCommand(CambiarSubMensage, param => this.canExecute);
            PanelCloseCommand = new RelayCommand(CerrarPanel, param => this.canExecute);
        }
        #endregion

        #region Workers Method
        #region Loaded Activity Center
        private void loadActivityCenterWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string currentMessage = "Load Activity Center Worker -> doWork";
            logger.Debug(currentMessage);

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
            }
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
                    this.sincronizaciones = SincronizacionDtoDataGrid.RefreshDataGrid(syncList);
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
                buttonSeeAdjustment.Command = this.VerAjustesCommand;

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
                Badge_verAjustes = badge;
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

        private void loadCentroActividadesWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("loadCentroActividades Worker ->runWorkedCompleted");
            PanelLoading = false;
        }
        #endregion

        #region Synchonization
        private void syncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string currentMessage = "sincronizar Worker ->doWork";
            logger.Debug(currentMessage);
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

                    currentMessage = "Controlando recepciones informadas ...";
                    NotifyCurrentMessage(currentMessage);
                    List<Sincronizacion> newSync = CreateNewBatch();

                    currentMessage = "Informando a genesix ...";
                    NotifyCurrentMessage(currentMessage);
                    InformToGenesix(newSync);
                    long syncId = newSync[0].idSincronizacion;
                    ExecuteInformGenesix(syncId);

                    currentMessage = "Borrando remante de archivos ...";
                    NotifyCurrentMessage(currentMessage);
                    DeleteAllPreviousFiles(false);


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
            }

            //legacy:
            if(true)
            {
                List<Actividad> actividades = MyAppProperties.activitiesEnables;
                foreach(Actividad actividad in actividades)
                {
                    if (Constants.DESCARGAR_GENESIX.Equals(actividad.accion.idAccion))
                    {
                        bool descargaMaestroCorrecta = HttpWebClientUtil.BuscarMaestrosDAT((int)actividad.idActividad, "");
                        PanelSubMessage = "Descargando " + actividad.descripcion.ToString();
                        Thread.Sleep(500);
                        if(descargaMaestroCorrecta)
                        {
                            if(actividad.idActividad.Equals(Constants.ubicart))
                            {
                                logger.Debug("Ubicart -> creando Archivos PAS");
                                ArchivosDATUtils.crearArchivoPAS();
                            }
                            if(actividad.idActividad.Equals(Constants.pedidos))
                            {
                                logger.Debug("Pedidos -> creando Archivos Pedidos");
                                ArchivosDATUtils.crearArchivosPedidos();
                            }

                            string destinationDirectory = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
                            string filename = ArchivosDATUtils.GetDataFileNameByIdActividad((int)actividad.idActividad);
                            string filenameAndExtension = FileUtils.WrapSlashAndDATExtension(filename);

                            ResultFileOperation result = App.Instance.deviceHandler.CopyPublicDataFileToDevice(destinationDirectory, filenameAndExtension);
                            logger.Debug("result: " + result);
                            PanelSubMessage = "Moviendo al dispositivo";
                            Thread.Sleep(500);
                        }
                    }
                }
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
            return result.Equals(MessageBoxResult.OK);
        }

        private bool CheckInformedReceptions()
        {
            string sotreId = MyAppProperties.storeId;
            string syncId = HttpWebClientUtil.GetCurrentBatchId(sotreId).ToString();
            bool informedReceptions = HttpWebClientUtil.CheckInformedReceptions(syncId);
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
                long syncId = sync.idSincronizacion;
                int actionId = Convert.ToInt32(sync.actividad.idActividad);
                try
                {
                    string slashFilenameAndExtension = ArchivosDATUtils.GetDataFileNameAndExtensionByIdActividad(actionId);
                    IDeviceHandler deviceHandler = App.Instance.deviceHandler;
                    ResultFileOperation resultCopy = deviceHandler.CopyDeviceFileToPublicData(slashFilenameAndExtension);
                    if(resultCopy.Equals(ResultFileOperation.OK))
                    {
                        UploadFileToServer(slashFilenameAndExtension, actionId, syncId);
                        SynchronizationStateUtil.SetReceivedDeviceState(syncId);
                    }
                    else
                    {
                        SynchronizationStateUtil.SetNoDataDeviceState(syncId);
                    }
                }
                catch
                {
                    SynchronizationStateUtil.SetErrorDeviceState(syncId);
                }
            }
        }

        private void UploadFileToServer(string slashFilenameAndExtension, int actionId, long syncId)
        {
            string filePath = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);
            string parametros = "?idActividad={0}&idSincronizacion={1}&registros={2}";
            var lineCount = FileUtils.CountRegistryWithinFile(filePath + slashFilenameAndExtension);
            parametros = String.Format(parametros, actionId, syncId, lineCount);
            string respuesta = HttpWebClientUtil.UploadFileToServer(filePath + slashFilenameAndExtension, parametros);
            logger.Debug(respuesta);
        }

        private void ExecuteInformGenesix(long syncId)
        {
            HttpWebClientUtil.ExecuteInformGenesix(syncId);
        }

        private void syncWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("sincronizar Worker ->runWorkedCompleted");
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.BeginInvoke(new Action(() =>
            {
                PanelLoading = false;
            }));
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
            syncrWorker.RunWorkerAsync();
        }

        public void InformarGenesix(object obj)
        {
            PanelLoading = true;
            BannerApp.PrintInformGX();
            MyAppProperties.isSynchronizationComplete = false;
            PanelMainMessage = "Informando a genesix, Espere por favor";
            logger.Info("Informando a genesix");
            syncrWorker.RunWorkerAsync();
        }

        public void VerAjustes(object obj)
        {
            logger.Debug("Viendo ajustes realizados");
            bool estadoDevice = App.Instance.deviceHandler.IsDeviceConnected();
            if(estadoDevice)
            {
                string motoApiReadDataFile = App.Instance.deviceHandler.ReadAdjustmentsDataFile();
                //por aca sabemos si hay ajustes realizados y de continuar

                List<Ajustes> ajustes = JsonUtils.GetListAjustes(motoApiReadDataFile);
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
            loadCentroActividadesWorker.RunWorkerAsync();
        }

        public void SincronizacionAnterior(object obj)
        {
            List<Sincronizacion> listaSincronizaciones = null;
            logger.Info("<- Ejecutando la vista anterior a la actual de la grilla");
            string urlSincronizacionAnterior = ConfigurationManager.AppSettings.Get(Constants.API_SYNC_ANTERIOR);
            string _sucursal = MyAppProperties.storeId;
            string _idLote = MyAppProperties.currentBatchId;
            listaSincronizaciones = HttpWebClientUtil.GetHttpWebSynchronizations(urlSincronizacionAnterior, _sucursal, _idLote);
            if(listaSincronizaciones != null && listaSincronizaciones.Count != 0)
            {
                logger.Debug(listaSincronizaciones);
                this.sincronizaciones = SincronizacionDtoDataGrid.RefreshDataGrid(listaSincronizaciones);
                UpdateCurrentBatch(sincronizaciones);
            }
        }

        public void SincronizacionSiguiente(object obj)
        {
            List<Sincronizacion> listaSincronizaciones = null;
            logger.Info("-> Solicitando la vista siguiente de la grilla, si es que la hay...");
            string urlSincronizacionAnterior = ConfigurationManager.AppSettings.Get(Constants.API_SYNC_SIGUIENTE);
            string _sucursal = MyAppProperties.storeId;
            string _idLote = MyAppProperties.currentBatchId;
            listaSincronizaciones = HttpWebClientUtil.GetHttpWebSynchronizations(urlSincronizacionAnterior, _sucursal, _idLote);
            if (listaSincronizaciones != null && listaSincronizaciones.Count != 0)
            {
                logger.Debug(listaSincronizaciones);
                this.sincronizaciones = SincronizacionDtoDataGrid.RefreshDataGrid(listaSincronizaciones);
                UpdateCurrentBatch(sincronizaciones);
            }
        }

        public void BuscarSincronizaciones(object obj)
        {
            logger.Info("Buscando sincronizaciones");
            BuscarLotesView buscarLotesView = new BuscarLotesView();
            buscarLotesView.ShowDialog();
        }

        public void GoLastSynchronization(object obj)
        {
            List<Sincronizacion> synchronizationList = null;
            logger.Info("Llendo a la ultima sincronizacion");
            string urlSincronizacionAnterior = ConfigurationManager.AppSettings.Get(Constants.API_SYNC_ULTIMA);
            string _sucursal = MyAppProperties.storeId;
            string _idLote = MyAppProperties.currentBatchId;
            synchronizationList = HttpWebClientUtil.GetHttpWebSynchronizations(urlSincronizacionAnterior, _sucursal, _idLote);
            if (synchronizationList != null && synchronizationList.Count != 0)
            {
                logger.Debug(synchronizationList);
                this.sincronizaciones = SincronizacionDtoDataGrid.RefreshDataGrid(synchronizationList);
                UpdateCurrentBatch(sincronizaciones);
            }
        }

        public void UpdateCurrentBatch(List<SincronizacionDtoDataGrid> synchronizations)
        {
            if(synchronizations != null && synchronizations.Count != 0)
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
        public static void BotonEstadoGenesix(object obj)
        {
            logger.Info("Boton estado genesix: " + obj);
            logger.Info(MyAppProperties.SelectedSync.actividad);
        }
        public static void BotonEstadoPDA(object obj)
        {
            logger.Info("Boton estado pda");
            logger.Info(MyAppProperties.SelectedSync.actividad);
        }
        public static void BotonEstadoGeneral(object obj)
        {
            logger.Info("Boton estado general");
            logger.Info(MyAppProperties.SelectedSync.actividad);
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
