using log4net;
using MahApps.Metro.Controls.Dialogs;
using PDADesktop.Classes;
using PDADesktop.Classes.Utils;
using PDADesktop.Model;
using PDADesktop.Model.Dto;
using PDADesktop.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PDADesktop.ViewModel
{
    class VerDetallesRecepcionViewModel : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Attributes
        private IDialogCoordinator dialogCoordinator;

        private ObservableCollection<Recepcion> receptions;
        public ObservableCollection<Recepcion> Receptions
        {
            get
            {
                return receptions;
            }
            set
            {
                receptions = value;
                OnPropertyChanged();
            }
        }

        private Recepcion selectedReception;
        public Recepcion SelectedReception
        {
            get
            {
                return selectedReception;
            }
            set
            {
                selectedReception = value;
                if (selectedReception != null)
                {
                    ReceptionEnableEdit = true;
                }
                else
                {
                    ReceptionEnableEdit = false;
                }
                OnPropertyChanged();
            }
        }

        private bool receptionEnableEdit;
        public bool ReceptionEnableEdit
        {
            get
            {
                return receptionEnableEdit;
            }
            set
            {
                receptionEnableEdit = value;
                OnPropertyChanged();
            }
        }
        #endregion

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

        #region Worker
        private readonly BackgroundWorker loadSeeDetailsWorker = new BackgroundWorker();
        private readonly BackgroundWorker discardReceptionsWorker = new BackgroundWorker();
        private readonly BackgroundWorker retryInformWorker = new BackgroundWorker();
        #endregion

        #region Commands
        private ICommand discardAllCommand;
        public ICommand DiscardAllCommand
        {
            get
            {
                return discardAllCommand;
            }
            set
            {
                discardAllCommand = value;
            }
        }
        private ICommand cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return cancelCommand;
            }
            set
            {
                cancelCommand = value;
            }
        }
        private ICommand retryCommand;
        public ICommand RetryCommand
        {
            get
            {
                return retryCommand;
            }
            set
            {
                retryCommand = value;
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
        #endregion

        #region Constructor
        public VerDetallesRecepcionViewModel(IDialogCoordinator instance)
        {
            BannerApp.PrintSeeDetailsReception();
            dialogCoordinator = instance;
            DisplayWaitingPanel("Cargando...");
            ReceptionEnableEdit = false;

            loadSeeDetailsWorker.DoWork += loadSeeDetailsWorker_DoWork;
            loadSeeDetailsWorker.RunWorkerCompleted += loadSeeDetailsWorker_RunWorkerCompleted;
            discardReceptionsWorker.DoWork += discardReceptionsWorker_DoWork;
            discardReceptionsWorker.RunWorkerCompleted += discardReceptionsWorker_RunWorkerCompleted;
            retryInformWorker.DoWork += retryInformWorker_DoWork;
            retryInformWorker.RunWorkerCompleted += retryInformWorker_RunWorkerCompleted;

            DiscardAllCommand = new RelayCommand(DiscardAllMethod);
            CancelCommand = new RelayCommand(CancelMethod);
            RetryCommand = new RelayCommand(RetryMethod);
            PanelCloseCommand = new RelayCommand(PanelCloseMethod);

            loadSeeDetailsWorker.RunWorkerAsync();
        }
        #endregion

        #region Workers

        #region Load See Details Worker
        private void loadSeeDetailsWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Debug("Load See Details Receptions -> Do Work");
            ListView listView = HttpWebClientUtil.LoadReceptionsGrid();
            Receptions = ListViewUtils.ParserRecepcionDataGrid(listView);
        }
        private void loadSeeDetailsWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("Load See Details Receptions -> Run Work Completed");
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.BeginInvoke(new Action( () =>
            {
                HidingWaitingPanel();
            }));
        }
        #endregion

        #region Discard Reception Worker
        private void discardReceptionsWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Debug("Discard Receptions -> Do Work");
            long syncId = MyAppProperties.SeeDetailsReception_syncId;
            long batchId = MyAppProperties.SeeDetailsReception_batchId;
            Dictionary<string, string> responseDiscard = HttpWebClientUtil.DiscardReceptions(batchId.ToString(), syncId.ToString());
            if(responseDiscard.ContainsKey("descargarPedidos"))
            {
                var descargarPedidos = responseDiscard["descargarPedidos"];
                if (descargarPedidos.Equals("true"))
                {
                    bool thereAreInformed = responseDiscard["hayInformadas"].Equals("true");
                    var syncIdResponse = responseDiscard["idSincronizacion"] as string;
                    UpdateOrder(syncIdResponse, thereAreInformed);
                }
            }
            var deviceHandler = App.Instance.deviceHandler;
            string storeId = MyAppProperties.storeId;
            ActionResultDto unlockDevice = deviceHandler.ControlDeviceLock(syncId, storeId);
            if (unlockDevice.success)
            {
                deviceHandler.ChangeSynchronizationState(Constants.ESTADO_SINCRO_FIN);
            }
        }

        private void UpdateOrder(string syncId, bool thereAreInformed)
        {
            try
            {
                string storeId = MyAppProperties.storeId;
                bool downloadOrders = HttpWebClientUtil.SearchDATsMasterFile(Convert.ToInt32(Constants.PEDIDOS_CODE), storeId);
                if (downloadOrders)
                {
                    ArchivosDATUtils.createOrdersFiles();
                }

                if (thereAreInformed)
                {
                    SynchronizationStateUtil.SetPrintReceptions(Convert.ToInt64(syncId));
                }
                else
                {
                    SynchronizationStateUtil.SetSentGenesixState(Convert.ToInt64(syncId));
                }
            }
            catch
            {
                SynchronizationStateUtil.SetRetry3GeneralState(Convert.ToInt64(syncId));
            }
        }

        private void discardReceptionsWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("Discard Receptions -> Run Work Completed");
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.BeginInvoke(new Action(() =>
            {
                HidingWaitingPanel();
                RedirectToActivityCenterView();
            }));
        }
        #endregion

        #region Retry Inform Worker
        private void retryInformWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Debug("Retry Inform Receptions -> Do Work");
            long syncId = MyAppProperties.SeeDetailsReception_syncId;
            int activityId = Convert.ToInt32(Constants.RECEP_CODE);
            ButtonStateUtils.RetryInformToGenesix(syncId, activityId);
        }
        private void retryInformWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("Retry Inform Receptions -> Run Work Completed");
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.BeginInvoke(new Action(() =>
            {
                HidingWaitingPanel();
                RedirectToActivityCenterView();
            }));
        }
        #endregion

        #endregion

        #region Command Methods
        private async void DiscardAllMethod(object sender)
        {
            string messageToAskToUser = "Se descartarán todas las Recepciones pendientes de informar. ¿Desea continuar?";
            bool userAnswer = await AskToUserMahappDialog(messageToAskToUser);

            if (userAnswer)
            {
                DisplayWaitingPanel("Espere por favor", "Descartando recepciones");
                discardReceptionsWorker.RunWorkerAsync();
            }
        }
        private void CancelMethod(object sender)
        {
            DisplayWaitingPanel("Espere por favor");
            RedirectToActivityCenterView();
        }
        private void RetryMethod(object sender)
        {
            DisplayWaitingPanel("Espere por favor", "Reintentando informar recepciones");
            retryInformWorker.RunWorkerAsync();
        }

        private void PanelCloseMethod(object sender)
        {
            HidingWaitingPanel();
        }
        #endregion

        #region Methods
        private void RedirectToActivityCenterView()
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            Uri uri = new Uri(Constants.CENTRO_ACTIVIDADES_VIEW, UriKind.Relative);
            window.frame.NavigationService.Navigate(uri);
        }

        private async Task<bool> AskToUserMahappDialog(string message, string title = "Aviso")
        {
            MetroDialogSettings settings = new MetroDialogSettings();
            settings.AffirmativeButtonText = "Aceptar";
            settings.NegativeButtonText = "Cancelar";
            Task<MessageDialogResult> showMessageAsync = dialogCoordinator.ShowMessageAsync(this, title, message, MessageDialogStyle.AffirmativeAndNegative, settings);
            MessageDialogResult messsageDialogResult = await showMessageAsync;
            bool resultAffirmative = messsageDialogResult.Equals(MessageDialogResult.Affirmative);
            return resultAffirmative;
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
            PanelMainMessage = "";
            PanelSubMessage = "";
        }

        #endregion
    }
}
