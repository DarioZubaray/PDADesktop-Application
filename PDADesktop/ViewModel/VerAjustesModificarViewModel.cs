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

namespace PDADesktop.ViewModel
{
    class VerAjustesModificarViewModel : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Attributes
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

        private IDialogCoordinator dialogCoordinator;

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

        #endregion

        #region BackgroundWorkers
        private readonly BackgroundWorker updateAdjustementWorker = new BackgroundWorker();
        #endregion

        #region Constructor
        public VerAjustesModificarViewModel(IDialogCoordinator instance)
        {
            BannerApp.PrintSeeAdjustmentsModify();
            dialogCoordinator = instance;
            DisplayWaitingPanel("Cargando...");

            AjustesListView ajustes = HttpWebClientUtil.LoadAdjustmentsGrid();
            adjustments = AjustesListView.ParserDataGrid(ajustes);

            AdjustmentEnableEdit = false;

            updateAdjustementWorker.DoWork += UpdateAdjustementWorker_DoWork;
            updateAdjustementWorker.RunWorkerCompleted += UpdateAdjustmentWorker_RunWorkerCompleted;

            DeleteAdjustmentCommand = new RelayCommand(DeleteAdjustmentMethod);
            UpdateAdjustmentCommand = new RelayCommand(UpdateAdjustmentMethod);
            DiscardChangesCommand = new RelayCommand(DiscardChangesMethod);
            SaveChangesCommand = new RelayCommand(SaveChangesMethod);

            PanelCloseCommand = new RelayCommand(CerrarPanel);
            HidingWaitingPanel();
        }

        ~VerAjustesModificarViewModel()
        {
            MyAppProperties.SeeAdjustmentModify_syncId = 0L;
            MyAppProperties.SeeAdjustmentModify_batchId = null;
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
            var dispatcher = App.Current.Dispatcher;
            dispatcher.BeginInvoke(new Action(() =>
            {
                RedirectToActivityCenterView();
            }));
        }
        #endregion

        #region Commands
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

        #region Methods
        public async void DeleteAdjustmentMethod(object obj)
        {
            logger.Debug("EliminarAjusteButton");
            string message = "¿Está seguro que desea eliminar el ajuste? Esta acción no se puede deshacer";
            bool userAnswer = await AskToUserMahappDialog(message);

            if (userAnswer)
            {
                Ajustes parametro = obj as Ajustes;
                if (parametro != null)
                    logger.Debug("Parametro: " + parametro.ToString());
                if (SelectedAdjustment != null)
                    logger.Debug("AjusteSeleccionado : " + SelectedAdjustment.ToString());

                Adjustments.Remove(SelectedAdjustment);
                long adjustmentId = SelectedAdjustment.id;
                string batchId = MyAppProperties.SeeAdjustmentModify_batchId;
                long syncId = MyAppProperties.SeeAdjustmentModify_syncId;
                ActionResultDto responseDeleteAdjustment = HttpWebClientUtil.DeleteAdjustment(adjustmentId, batchId, syncId);
                logger.Debug("resultado de borrar ajuste: " + responseDeleteAdjustment.success);
                SelectedAdjustment = null;
            }
        }

        public void UpdateAdjustmentMethod(object obj)
        {
            logger.Debug("ActualizarAjusteButton");
            SelectedAdjustment = null;
        }

        public async void DiscardChangesMethod(object obj)
        {
            logger.Debug("DescartarCambiosButton");
            string message = "¿Desea descartar los cambios?";
            bool userAnswer = await AskToUserMahappDialog(message);

            if (userAnswer)
            {
                RedirectToActivityCenterView();
            }
        }

        public void SaveChangesMethod(object obj)
        {
            logger.Debug("GuardarCambiosButton");
            string batchId = MyAppProperties.SeeAdjustmentModify_batchId;
            string title = "Actualizando cantidades de ajustes";
            string message = "Espere por favor mientras se informan los ajustes modificados.";
            DisplayWaitingPanel(title, message);
            updateAdjustementWorker.RunWorkerAsync();
        }

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

        public void CerrarPanel(object obj)
        {
            PanelLoading = false;
        }
        #endregion

        #endregion
    }
}
