using log4net;
using MahApps.Metro.Controls.Dialogs;
using PDADesktop.Classes;
using PDADesktop.Classes.Devices;
using PDADesktop.Classes.Utils;
using PDADesktop.Model;
using PDADesktop.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace PDADesktop.ViewModel
{
    class VerActividadesViewModel : ViewModelBase
    {
        #region Attributes
        #region Commons Attributes
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeviceHandler deviceHandler { get; set; }
        private Dispatcher dispatcher { get; set; }
        private IDialogCoordinator dialogCoordinator;
        private readonly object _lock = new object();
        #endregion

        #region selectedRow
        public ControlPrecio PriceControlConfirmedSelected { get; set; }
        public ControlPrecio PendingPriceControlSelected { get; set; }
        public Ajustes AdjustmentConfirmedSelected { get; set; }
        public Ajustes PendingAdjustmentSelected { get; set; }
        public ArticuloRecepcion ReceptionConfirmedSelected { get; set; }
        public ArticuloRecepcion PendingReceptionSelected { get; set; }
        public Etiqueta LabelConfirmedSelected { get; set; }
        public Etiqueta PendingLabelSelected { get; set; }
        #endregion

        #region List
        private ObservableCollection<ControlPrecio> controlPreciosConfirmados;
        public ObservableCollection<ControlPrecio> ControlPreciosConfirmados
        {
            get
            {
                return controlPreciosConfirmados;
            }
            set
            {
                controlPreciosConfirmados = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<ControlPrecio> controlPreciosPendientes;
        public ObservableCollection<ControlPrecio> ControlPreciosPendientes
        {
            get
            {
                return controlPreciosPendientes;
            }
            set
            {
                controlPreciosPendientes = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Ajustes> ajustesConfirmados;
        public ObservableCollection<Ajustes> AjustesConfirmados
        {
            get
            {
                return ajustesConfirmados;
            }
            set
            {
                ajustesConfirmados = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Ajustes> ajustesPendientes;
        public ObservableCollection<Ajustes> AjustesPendientes
        {
            get
            {
                return ajustesPendientes;
            }
            set
            {
                ajustesPendientes = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ArticuloRecepcion> recepcionesConfirmadas;
        public ObservableCollection<ArticuloRecepcion> RecepcionesConfirmadas
        {
            get
            {
                return recepcionesConfirmadas;
            }
            set
            {
                recepcionesConfirmadas = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<ArticuloRecepcion> recepcionesPendientes;
        public ObservableCollection<ArticuloRecepcion> RecepcionesPendientes
        {
            get
            {
                return recepcionesPendientes;
            }
            set
            {
                recepcionesPendientes = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Etiqueta> etiquetasConfirmadas;
        public ObservableCollection<Etiqueta> EtiquetasConfirmadas
        {
            get
            {
                return etiquetasConfirmadas;
            }
            set
            {
                etiquetasConfirmadas = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Etiqueta> etiquetasPendientes;
        public ObservableCollection<Etiqueta> EtiquetasPendientes
        {
            get
            {
                return etiquetasPendientes;
            }
            set
            {
                etiquetasPendientes = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Workers Attributes
        private readonly BackgroundWorker loadSeeActivitiesWorker = new BackgroundWorker();
        #endregion

        #region Commands
        private ICommand verActividadesLoadedEvent;
        public ICommand VerActividadesLoadedEvent
        {
            get
            {
                return verActividadesLoadedEvent;
            }
            set
            {
                verActividadesLoadedEvent = value;
            }
        }
        private ICommand returnCommand;
        public ICommand ReturnCommand
        {
            get
            {
                return returnCommand;
            }
            set
            {
                returnCommand = value;
            }
        }
        private ICommand acceptCommand;
        public ICommand AcceptCommand
        {
            get
            {
                return acceptCommand;
            }
            set
            {
                acceptCommand = value;
            }
        }

        private ICommand removeAllPriceControlCommand;
        public ICommand RemoveAllPriceControlCommand
        {
            get
            {
                return removeAllPriceControlCommand;
            }
            set
            {
                removeAllPriceControlCommand = value;
            }
        }
        private ICommand removeOnePriceControlCommand;
        public ICommand RemoveOnePriceControlCommand
        {
            get
            {
                return removeOnePriceControlCommand;
            }
            set
            {
                removeOnePriceControlCommand = value;
            }
        }
        private ICommand addOnePriceControlCommand;
        public ICommand AddOnePriceControlCommand
        {
            get
            {
                return addOnePriceControlCommand;
            }
            set
            {
                addOnePriceControlCommand = value;
            }
        }
        private ICommand addAllPriceControlCommand;
        public ICommand AddAllPriceControlCommand
        {
            get
            {
                return addAllPriceControlCommand;
            }
            set
            {
                addAllPriceControlCommand = value;
            }
        }

        private ICommand removeAllAdjustmentCommand;
        public ICommand RemoveAllAdjustmentCommand
        {
            get
            {
                return removeAllAdjustmentCommand;
            }
            set
            {
                removeAllAdjustmentCommand = value;
            }
        }
        private ICommand removeOneAdjustmentCommand;
        public ICommand RemoveOneAdjustmentCommand
        {
            get
            {
                return removeOneAdjustmentCommand;
            }
            set
            {
                removeOneAdjustmentCommand = value;
            }
        }
        private ICommand addOneAdjustmentCommand;
        public ICommand AddOneAdjustmentCommand
        {
            get
            {
                return addOneAdjustmentCommand;
            }
            set
            {
                addOneAdjustmentCommand = value;
            }
        }
        private ICommand addAllAdjustmentCommand;
        public ICommand AddAllAdjustmentCommand
        {
            get
            {
                return addAllAdjustmentCommand;
            }
            set
            {
                addAllAdjustmentCommand = value;
            }
        }

        private ICommand removeAllReceptionCommand;
        public ICommand RemoveAllReceptionCommand
        {
            get
            {
                return removeAllReceptionCommand;
            }
            set
            {
                removeAllReceptionCommand = value;
            }
        }
        private ICommand removeOneReceptionCommand;
        public ICommand RemoveOneReceptionCommand
        {
            get
            {
                return removeOneReceptionCommand;
            }
            set
            {
                removeOneReceptionCommand = value;
            }
        }
        private ICommand addOneReceptionCommand;
        public ICommand AddOneReceptionCommand
        {
            get
            {
                return addOneReceptionCommand;
            }
            set
            {
                addOneReceptionCommand = value;
            }
        }
        private ICommand addAllReceptionCommand;
        public ICommand AddAllReceptionCommand
        {
            get
            {
                return addAllReceptionCommand;
            }
            set
            {
                addAllReceptionCommand = value;
            }
        }

        private ICommand removeAllLabelCommand;
        public ICommand RemoveAllLabelCommand
        {
            get
            {
                return removeAllLabelCommand;
            }
            set
            {
                removeAllLabelCommand = value;
            }
        }
        private ICommand removeOneLabelCommand;
        public ICommand RemoveOneLabelCommand
        {
            get
            {
                return removeOneLabelCommand;
            }
            set
            {
                removeOneLabelCommand = value;
            }
        }
        private ICommand addOneLabelCommand;
        public ICommand AddOneLabelCommand
        {
            get
            {
                return addOneLabelCommand;
            }
            set
            {
                addOneLabelCommand = value;
            }
        }
        private ICommand addAllLabelCommand;
        public ICommand AddAllLabelCommand
        {
            get
            {
                return addAllLabelCommand;
            }
            set
            {
                addAllLabelCommand = value;
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
        #endregion

        #region Constructor
        public VerActividadesViewModel(IDialogCoordinator instance)
        {
            BannerApp.PrintSeeActivities();
            DisplayWaitingPanel("Cargando...");
            deviceHandler = App.Instance.deviceHandler;
            dispatcher = App.Instance.Dispatcher;
            dialogCoordinator = instance;

            VerActividadesLoadedEvent = new RelayCommand(SeeActivitiesLoadedEventAction);

            ReturnCommand = new RelayCommand(ReturnAction);
            AcceptCommand = new RelayCommand(AcceptAction);

            RemoveAllPriceControlCommand = new RelayCommand(RemoveAllPricecontrolAction);
            RemoveOnePriceControlCommand = new RelayCommand(RemoveOnePricecontrolAction);
            AddOnePriceControlCommand = new RelayCommand(AddOnePricecontrolAction);
            AddAllPriceControlCommand = new RelayCommand(AddAllPricecontrolAction);

            RemoveAllAdjustmentCommand = new RelayCommand(RemoveAllAdjustmentAction);
            RemoveOneAdjustmentCommand = new RelayCommand(RemoveOneAdjustmentAction);
            AddOneAdjustmentCommand = new RelayCommand(AddOneAdjustmentAction);
            AddAllAdjustmentCommand = new RelayCommand(AddAllAdjustmentAction);

            RemoveAllReceptionCommand = new RelayCommand(RemoveAllReceptionAction);
            RemoveOneReceptionCommand = new RelayCommand(RemoveOneReceptionAction);
            AddOneReceptionCommand = new RelayCommand(AddOneReceptionAction);
            AddAllReceptionCommand = new RelayCommand(AddAllReceptionAction);

            RemoveAllLabelCommand = new RelayCommand(RemoveAllLabelAction);
            RemoveOneLabelCommand = new RelayCommand(RemoveOneLabelAction);
            AddOneLabelCommand = new RelayCommand(AddOneLabelAction);
            AddAllLabelCommand = new RelayCommand(AddAllLabelAction);

            ShowPanelCommand = new RelayCommand(ShowPanelAction);
            PanelCloseCommand = new RelayCommand(ClosePanelAction);
            PanelCloseCommand_NC = new RelayCommand(ClosePanelNoConnectionAction);
        }
        #endregion

        #region Methods
        #region LoadEvent
        public async void SeeActivitiesLoadedEventAction(object obj)
        {
            await Task.Run(async () =>
            {
                await loadSeeActivitiesWorker_DoWork();
            });
            loadSeeActivitiesWorker_RunWorkerCompleted();
        }
        #endregion

        #region Workers Method
        private async Task loadSeeActivitiesWorker_DoWork()
        {
            logger.Debug("load see activities worker => Do Work");
            await getDataFromPlainFiles();
            getDataFromDBCompact();
        }
        private void loadSeeActivitiesWorker_RunWorkerCompleted()
        {
            logger.Debug("load see activities worker => Run Worker Completed");
            logger.Debug(ControlPreciosConfirmados);

            HidingWaitingPanel();
        }
        #endregion

        #region Commons Methods
        public void NotifyCurrentMessage(string currentMessage)
        {
            logger.Debug(currentMessage);
            PanelSubMessage = currentMessage;
            Thread.Sleep(300);
        }

        private async Task getDataFromPlainFiles()
        {
            String currentMessage = "Obteniendo Control de precios confirmados... ";
            NotifyCurrentMessage(currentMessage);
            string priceControlfileContent = deviceHandler.ReadPriceControlDataFile();
            if(priceControlfileContent != null)
            {
                this.ControlPreciosConfirmados = JsonUtils.GetObservableCollectionControlPrecios(priceControlfileContent);
            }
            currentMessage = "Obteniendo Ajustes confirmados... ";
            NotifyCurrentMessage(currentMessage);
            string adjustmentFilecontent = deviceHandler.ReadAdjustmentsDataFile();
            if(adjustmentFilecontent != null)
            {
                await dispatcher.BeginInvoke(new Action(() => {
                    AjustesConfirmados = JsonUtils.GetObservableCollectionAjustes(adjustmentFilecontent);
                }));
            }

            currentMessage = "Obteniendo Recepciones confirmados... ";
            NotifyCurrentMessage(currentMessage);
            string receptionFilecontent = deviceHandler.ReadReceptionDataFile();
            if(receptionFilecontent != null)
            {
                await dispatcher.BeginInvoke(new Action(() => {
                    RecepcionesConfirmadas = JsonUtils.GetObservableCollectionRecepciones(receptionFilecontent);
                }));
            }

            currentMessage = "Obteniendo Impresión de Etiquetas confirmados... ";
            NotifyCurrentMessage(currentMessage);
            string labelFileContent = deviceHandler.ReadLabelDataFile();
            if(labelFileContent != null)
            {
                await dispatcher.BeginInvoke(new Action(() => {
                    EtiquetasConfirmadas = JsonUtils.GetObservableCollectionEtiquetas(labelFileContent);
                }));
            }
        }

        private void getDataFromDBCompact()
        {
            string currentMessage = "Obteniendo otros datos pendientes... ";
            NotifyCurrentMessage(currentMessage);

            string sqlceDataBase = "/" + ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATABASE);
            string deviceFolder = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_ROOT);
            logger.Info("copiando desde " + deviceFolder + sqlceDataBase);

            string publicFolder = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_PROGRAM);
            string publicFolderExtended = TextUtils.ExpandEnviromentVariable(publicFolder);
            logger.Info("hacia " + publicFolderExtended + sqlceDataBase);

            ResultFileOperation result = deviceHandler.CopyFromDeviceToPublicFolder(sqlceDataBase, deviceFolder, publicFolderExtended);
            if (ResultFileOperation.OK.Equals(result))
            {
                currentMessage = "Leyendo control de precios sin confirmar ... ";
                NotifyCurrentMessage(currentMessage);
                dispatcher.BeginInvoke(new Action(() => {
                    ControlPreciosPendientes = SqlCEReaderUtils.leerControlPrecios(@"Data Source=" + publicFolderExtended + sqlceDataBase);
                }));

                currentMessage = "Leyendo ajustes sin confirmar... ";
                NotifyCurrentMessage(currentMessage);
                dispatcher.BeginInvoke(new Action(() => {
                    AjustesPendientes = SqlCEReaderUtils.leerAjustes(@"Data Source=" + publicFolderExtended + sqlceDataBase);
                }));

                currentMessage = "Leyendo recepcione sin confirmar... ";
                NotifyCurrentMessage(currentMessage);
                dispatcher.BeginInvoke(new Action(() => {
                    RecepcionesPendientes = SqlCEReaderUtils.leerRecepciones(@"Data Source=" + publicFolderExtended + sqlceDataBase);
                }));

                currentMessage = "Leyendo etiquetas sin confirmar ... ";
                NotifyCurrentMessage(currentMessage);
                dispatcher.BeginInvoke(new Action(() => {
                    EtiquetasPendientes = SqlCEReaderUtils.leerEtiquetas(@"Data Source=" + publicFolderExtended + sqlceDataBase);
                }));
            }
        }
        #endregion

        #region Command Methods
        public async void ReturnAction(object sender)
        {
            logger.Debug("ReturnAction");
            string returnMsg = "¿Desea regresar sin efectuar cambios?";
            bool continueOrCancel = await AskUserMetroDialog(returnMsg);
            if (continueOrCancel)
            {
                MainWindow window = (MainWindow)Application.Current.MainWindow;
                Uri uri = new Uri(Constants.CENTRO_ACTIVIDADES_VIEW, UriKind.Relative);
                window.frame.NavigationService.Navigate(uri);
            }
        }
        public void AcceptAction(object sender)
        {
            logger.Debug("AcceptAction");
            //TODO logica de inserts y escritura en los archivos
            ReturnAction(null);
        }

        #region PriceControl Action
        public void RemoveAllPricecontrolAction(object sender)
        {
            logger.Debug("Remove All => Price control");
            foreach(ControlPrecio ctrubic in ControlPreciosConfirmados)
            {
                ControlPreciosPendientes.Add(ctrubic);
            }
            ControlPreciosConfirmados.Clear();
        }
        public void RemoveOnePricecontrolAction(object sender)
        {
            logger.Debug("Remove one => Price control");
            if(PriceControlConfirmedSelected != null)
            {
                ControlPreciosPendientes.Add(PriceControlConfirmedSelected);
                ControlPreciosConfirmados.Remove(PriceControlConfirmedSelected);
            }
        }
        public void AddOnePricecontrolAction(object sender)
        {
            logger.Debug("Add one => Price control");
            if (PendingPriceControlSelected != null)
            {
                ControlPreciosConfirmados.Add(PendingPriceControlSelected);
                ControlPreciosPendientes.Remove(PendingPriceControlSelected);
            }
        }
        public void AddAllPricecontrolAction(object sender)
        {
            logger.Debug("Add All => Price control");
            foreach(ControlPrecio ctrubic in ControlPreciosPendientes)
            {
                ControlPreciosConfirmados.Add(ctrubic);
            }
            ControlPreciosPendientes.Clear();
        }
        #endregion

        #region Adjustment Action
        public void RemoveAllAdjustmentAction(object sender)
        {
            logger.Debug("Remove All => Adjustment");
            foreach (Ajustes ajuste in AjustesConfirmados)
            {
                AjustesPendientes.Add(ajuste);
            }
            AjustesConfirmados.Clear();
        }
        public void RemoveOneAdjustmentAction(object sender)
        {
            logger.Debug("Remove one => Adjustment");
            if(AdjustmentConfirmedSelected != null)
            {
                AjustesPendientes.Add(AdjustmentConfirmedSelected);
                AjustesConfirmados.Remove(AdjustmentConfirmedSelected);
            }
        }
        public void AddOneAdjustmentAction(object sender)
        {
            logger.Debug("Add one => Adjustment");
            if(PendingAdjustmentSelected != null)
            {
                AjustesConfirmados.Add(PendingAdjustmentSelected);
                AjustesPendientes.Remove(PendingAdjustmentSelected);
            }
        }
        public void AddAllAdjustmentAction(object sender)
        {
            logger.Debug("Add All => Adjustment");
            foreach (Ajustes ajustes in AjustesPendientes)
            {
                AjustesConfirmados.Add(ajustes);
            }
            AjustesPendientes.Clear();
        }
        #endregion

        #region Reception Action
        public void RemoveAllReceptionAction(object sender)
        {
            logger.Debug("Remove All => Reception");
            foreach (ArticuloRecepcion artRecep in RecepcionesConfirmadas)
            {
                RecepcionesPendientes.Add(artRecep);
            }
            RecepcionesConfirmadas.Clear();
        }
        public void RemoveOneReceptionAction(object sender)
        {
            logger.Debug("Remove one => Reception");
            if (ReceptionConfirmedSelected != null)
            {
                RecepcionesPendientes.Add(ReceptionConfirmedSelected);
                RecepcionesConfirmadas.Remove(ReceptionConfirmedSelected);
            }
        }
        public void AddOneReceptionAction(object sender)
        {
            logger.Debug("Add one => Reception");
            if (PendingReceptionSelected != null)
            {
                RecepcionesConfirmadas.Add(PendingReceptionSelected);
                RecepcionesPendientes.Remove(PendingReceptionSelected);
            }
        }
        public void AddAllReceptionAction(object sender)
        {
            logger.Debug("Add All => Reception");
            foreach (ArticuloRecepcion artRecep in RecepcionesPendientes)
            {
                RecepcionesConfirmadas.Add(artRecep);
            }
            RecepcionesPendientes.Clear();
        }
        #endregion

        #region Label Action
        public void RemoveAllLabelAction(object sender)
        {
            logger.Debug("Remove All => Label");
            foreach (Etiqueta etiqueta in EtiquetasConfirmadas)
            {
                EtiquetasPendientes.Add(etiqueta);
            }
            EtiquetasConfirmadas.Clear();
        }
        public void RemoveOneLabelAction(object sender)
        {
            logger.Debug("Remove one => Label");
            if(LabelConfirmedSelected != null)
            {
                EtiquetasPendientes.Add(LabelConfirmedSelected);
                EtiquetasConfirmadas.Remove(LabelConfirmedSelected);
            }
        }
        public void AddOneLabelAction(object sender)
        {
            logger.Debug("Add one => Label");
            if(PendingLabelSelected != null)
            {
                EtiquetasConfirmadas.Add(PendingLabelSelected);
                EtiquetasPendientes.Remove(PendingLabelSelected);
            }
        }
        public void AddAllLabelAction(object sender)
        {
            logger.Debug("Add All => Label");
            foreach (Etiqueta etiqueta in EtiquetasPendientes)
            {
                EtiquetasConfirmadas.Add(etiqueta);
            }
            EtiquetasPendientes.Clear();
        }
        #endregion

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

        private async Task<bool> ShowMetroDialog(MessageDialogStyle messageDialogStyle, string message, string title = "Aviso")
        {
            MetroDialogSettings metroDialogSettings = new MetroDialogSettings();
            metroDialogSettings.AffirmativeButtonText = "Aceptar";
            metroDialogSettings.NegativeButtonText = "Cancelar";
            MessageDialogResult userResponse = await dialogCoordinator.ShowMessageAsync(this, title, message, messageDialogStyle, metroDialogSettings);
            return userResponse == MessageDialogResult.Affirmative;
        }
        #endregion
        #endregion
    }
}
