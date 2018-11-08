using log4net;
using PDADesktop.Classes;
using PDADesktop.Model;
using PDADesktop.Classes.Utils;
using PDADesktop.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using PDADesktop.Model.Dto;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Threading;

namespace PDADesktop.ViewModel
{
    class VerAjustesModificarViewModel : ViewModelBase
    {
        #region Attributes
        #region Commons Attributes
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDialogCoordinator dialogCoordinator;

        private ObservableCollection<Ajustes> adjustments;
        public ObservableCollection<Ajustes> Adjustments
        {
            get
            {
                return adjustments;
            }
            set
            {
                adjustments = value;
                OnPropertyChanged();
            }
        }

        private Ajustes selectedAdjustment;
        public Ajustes SelectedAdjustment
        {
            get
            {
                return selectedAdjustment;
            }
            set
            {
                selectedAdjustment = value;
                if (selectedAdjustment != null)
                {
                    AdjustmentTypeSelected = selectedAdjustment.motivo;
                    Textbox_amountValue = selectedAdjustment.cantidad.ToString();
                    AdjustmentEnableEdit = true;
                }
                else
                {
                    AdjustmentEnableEdit = false;
                }
                OnPropertyChanged();
            }
        }

        private bool adjustmentEnableEdit;
        public bool AdjustmentEnableEdit
        {
            get
            {
                return adjustmentEnableEdit;
            }
            set
            {
                adjustmentEnableEdit = value;
                OnPropertyChanged();
            }
        }

        private string adjustmentTypeSelected;
        public string AdjustmentTypeSelected
        {
            get
            {
                return adjustmentTypeSelected;
            }
            set
            {
                adjustmentTypeSelected = value;
                SelectedAdjustment.motivo = adjustmentTypeSelected;
                OnPropertyChanged();
            }
        }

        private List<string> adjustmentsTypes;
        public List<string> AdjustmentsTypes
        {
            get
            {
                return adjustmentsTypes;
            }
            set
            {
                adjustmentsTypes = value;
                OnPropertyChanged();
            }
        }

        private string textbox_amountValue;
        public string Textbox_amountValue
        {
            get
            {
                return textbox_amountValue;
            }
            set
            {
                textbox_amountValue = value;
                SelectedAdjustment.cantidad = Convert.ToInt64(textbox_amountValue);
                OnPropertyChanged();
            }
        }
        #endregion

        #region Workers Attributes
        private readonly BackgroundWorker updateAdjustementWorker = new BackgroundWorker();
        #endregion

        #region Dispatcher Attributes
        private Dispatcher dispatcher;
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

        #region Commands Attributes
        private ICommand verAjustesModificarLoadedEvent;
        public ICommand VerAjustesModificarLoadedEvent
        {
            get
            {
                return verAjustesModificarLoadedEvent;
            }
            set
            {
                verAjustesModificarLoadedEvent = value;
            }
        }

        private ICommand deleteAdjustmentCommand;
        public ICommand DeleteAdjustmentCommand
        {
            get
            {
                return deleteAdjustmentCommand;
            }
            set
            {
                deleteAdjustmentCommand = value;
            }
        }

        private ICommand updateAdjustmentCommand;
        public ICommand UpdateAdjustmentCommand
        {
            get
            {
                return updateAdjustmentCommand;
            }
            set
            {
                updateAdjustmentCommand = value;
            }
        }

        private ICommand discardChangesCommand;
        public ICommand DiscardChangesCommand
        {
            get
            {
                return discardChangesCommand;
            }
            set
            {
                discardChangesCommand = value;
            }
        }

        private ICommand saveChangesCommand;
        public ICommand SaveChangesCommand
        {
            get
            {
                return saveChangesCommand;
            }
            set
            {
                saveChangesCommand = value;
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
        #endregion

        #region Constructor
        public VerAjustesModificarViewModel(IDialogCoordinator instance)
        {
            BannerApp.PrintSeeAdjustmentsModify();
            dialogCoordinator = instance;
            dispatcher = App.Instance.Dispatcher;
            DisplayWaitingPanel("Cargando...");

            VerAjustesModificarLoadedEvent = new RelayCommand(VerAjustesModificarLoadedEventAction);

            //TODO mover esta llama a un metodo de un worker en segundo plano
            string batchId = MyAppProperties.SeeAdjustmentModify_batchId;
            string estadoInformado = "true";
            ListView ajustes = HttpWebClientUtil.LoadAdjustmentsGrid(batchId, estadoInformado);
            adjustments = ListViewUtils.ParserAjustesDataGrid(ajustes);

            AdjustmentEnableEdit = false;

            updateAdjustementWorker.DoWork += UpdateAdjustementWorker_DoWork;
            updateAdjustementWorker.RunWorkerCompleted += UpdateAdjustmentWorker_RunWorkerCompleted;

            DeleteAdjustmentCommand = new RelayCommand(DeleteAdjustmentAction);
            UpdateAdjustmentCommand = new RelayCommand(UpdateAdjustmentAction);
            DiscardChangesCommand = new RelayCommand(DiscardChangesAction);
            SaveChangesCommand = new RelayCommand(SaveChangesAction);

            PanelCloseCommand = new RelayCommand(ClosePanelAction);
            HidingWaitingPanel();
        }

        ~VerAjustesModificarViewModel()
        {
            MyAppProperties.SeeAdjustmentModify_syncId = 0L;
            MyAppProperties.SeeAdjustmentModify_batchId = null;
        }
        #endregion

        #region Event Method
        public void VerAjustesModificarLoadedEventAction(object sender)
        {
            logger.Debug("Ver ajustes Modificar => Loaded Event");
        }
        #endregion

        #region Update Adjustment Worker
        private void UpdateAdjustementWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Debug("Update Adjustment Worker -> doWork");
            long syncId = MyAppProperties.SeeAdjustmentModify_syncId;
            logger.Debug("syncId: " + syncId);
            logger.Debug("Ajustes: " + adjustments);
            string batchId = MyAppProperties.SeeAdjustmentModify_batchId;
            string responseUpdateModifyAdjustments = HttpWebClientUtil.UpdateModifiedAdjustments(batchId, Adjustments, syncId);
            logger.Debug(responseUpdateModifyAdjustments);
        }

        private void UpdateAdjustmentWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("Update Adjustment Worker -> runWorker Completed");
            dispatcher.BeginInvoke(new Action(() =>
            {
                RedirectToActivityCenterView();
            }));
        }
        #endregion

        #region Actions Methods
        public async void DeleteAdjustmentAction(object obj)
        {
            logger.Debug("EliminarAjusteButton");
            string message = "¿Está seguro que desea eliminar el ajuste? Esta acción no se puede deshacer";
            bool userAnswer = await AskUserMetroDialog(message);

            if (userAnswer)
            {
                Ajustes parametro = obj as Ajustes;
                if (parametro != null)
                    logger.Debug("Parametro: " + parametro.ToString());
                if (SelectedAdjustment != null)
                    logger.Debug("AjusteSeleccionado : " + SelectedAdjustment.ToString());

                long adjustmentId = SelectedAdjustment.id;
                string batchId = MyAppProperties.SeeAdjustmentModify_batchId;
                long syncId = MyAppProperties.SeeAdjustmentModify_syncId;
                ActionResultDto responseDeleteAdjustment = HttpWebClientUtil.DeleteAdjustment(adjustmentId, batchId, syncId);
                logger.Debug("resultado de borrar ajuste: " + responseDeleteAdjustment.success);
                Adjustments.Remove(SelectedAdjustment);
                SelectedAdjustment = null;
            }
        }

        public void UpdateAdjustmentAction(object obj)
        {
            logger.Debug("ActualizarAjusteButton");
            SelectedAdjustment = null;
        }

        public async void DiscardChangesAction(object obj)
        {
            logger.Debug("DescartarCambiosButton");
            string message = "¿Desea descartar los cambios?";
            bool userAnswer = await AskUserMetroDialog(message);

            if (userAnswer)
            {
                RedirectToActivityCenterView();
            }
        }

        public async void SaveChangesAction(object obj)
        {
            logger.Debug("GuardarCambiosButton");
            string messageToAskToUser = "¿Está seguro que desea continuar, confirmando la modificación del ajuste?";
            bool userAnswer = await AskUserMetroDialog(messageToAskToUser);

            if (userAnswer)
            {
                string batchId = MyAppProperties.SeeAdjustmentModify_batchId;
                string title = "Actualizando cantidades de ajustes";
                string messageToDisplayWaitingPanel = "Espere por favor mientras se informan los ajustes modificados.";
                DisplayWaitingPanel(title, messageToDisplayWaitingPanel);
                updateAdjustementWorker.RunWorkerAsync();
            }
        }
        #endregion

        #region Commons Methods
        private void RedirectToActivityCenterView()
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            Uri uri = new Uri(Constants.CENTRO_ACTIVIDADES_VIEW, UriKind.Relative);
            window.frame.NavigationService.Navigate(uri);
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

        public void ClosePanelAction(object obj)
        {
            PanelLoading = false;
        }
        #endregion
    }
}
