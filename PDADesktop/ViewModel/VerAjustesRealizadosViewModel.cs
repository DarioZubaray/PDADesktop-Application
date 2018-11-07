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
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.ComponentModel;
using PDADesktop.Classes.Devices;

namespace PDADesktop.ViewModel
{
    class VerAjustesRealizadosViewModel : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Attributes
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
                if(selectedAdjustment != null)
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
            }set
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

        private readonly BackgroundWorker loadVerAjustesRealizadosWorker = new BackgroundWorker();

        #region Constructor
        public VerAjustesRealizadosViewModel(IDialogCoordinator instance)
        {
            BannerApp.PrintSeeAdjustmentsRealized();
            dialogCoordinator = instance;
            DisplayWaitingPanel("Cargando", "Espere por favor...");

            loadVerAjustesRealizadosWorker.DoWork += loadVerAjustesRealizadosWorker_DoWork;
            loadVerAjustesRealizadosWorker.RunWorkerCompleted += loadVerAjustesRealizadosWorker_RunWorkerCompleted;

            AdjustmentEnableEdit = false;

            DeleteAdjustmentCommand = new RelayCommand(DeleteAdjustmentMethod);
            UpdateAdjustmentCommand = new RelayCommand(UpdateAdjustmentMethod);
            DiscardChangesCommand = new RelayCommand(DiscardChangesMethod);
            SaveChangesCommand = new RelayCommand(SaveChangesMethod);
            PanelCloseCommand = new RelayCommand(PanelCloseMethod);

            loadVerAjustesRealizadosWorker.RunWorkerAsync();
        }
        #endregion

        #region Workers
        private async void loadVerAjustesRealizadosWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Debug("Load ver ajustes realizados => Do Work");
            var deviceHandler = App.Instance.deviceHandler;
            bool deviceStatus = deviceHandler.IsDeviceConnected();
            if (deviceStatus)
            {
                string deviceReadAdjustmentDataFile = null;
                try
                {
                    deviceReadAdjustmentDataFile = deviceHandler.ReadAdjustmentsDataFile();
                }
                catch (Exception ex)
                {
                    logger.Debug(ex.Message);
                    await AlertUserMetroDialog("Ocurrio un error en la lectura del archivo de ajuste.");
                }
                if (deviceReadAdjustmentDataFile != null)
                {
                    UpdateAjustesGrid(deviceReadAdjustmentDataFile);
                }
                else
                {
                    await AlertUserMetroDialog("No se encotraron ajustes!");
                }
            }
            else
            {
                await AlertUserMetroDialog("No se detecta conexion con la PDA");
            }
        }

        private void UpdateAjustesGrid(string deviceReadAdjustmentDataFile)
        {
            Adjustments = JsonUtils.GetObservableCollectionAjustes(deviceReadAdjustmentDataFile);

            AdjustmentsTypes = HttpWebClientUtil.GetAdjustmentsTypes();
            logger.Debug(AdjustmentsTypes.ToString());
        }

        private void loadVerAjustesRealizadosWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("Load ver ajustes reliazados => Run Worker Completed");
            HidingWaitingPanel();
        }
        #endregion

        #region Methods
        public void DeleteAdjustmentMethod(object obj)
        {
            logger.Debug("EliminarAjusteButton");
            Ajustes parametro = obj as Ajustes;
            if(parametro != null)
                logger.Debug("Parametro: " + parametro.ToString());
            if(SelectedAdjustment != null)
                logger.Debug("AjusteSeleccionado : " + SelectedAdjustment.ToString());

            Adjustments.Remove(SelectedAdjustment);
            SelectedAdjustment = null;
        }
        public void UpdateAdjustmentMethod(object obj)
        {
            logger.Debug("ActualizarAjusteButton");
            SelectedAdjustment = null;
        }
        public async void DiscardChangesMethod(object obj)
        {
            logger.Debug("DescartarCambiosButton");
            string pregunta = "¿Desea descartar los cambios?";
            bool discardChanges = await AskToUserMahappDialog(pregunta);
            if (discardChanges)
            {
                RedirectToActivityCenterView();
            }
        }

        public async void SaveChangesMethod(object obj)
        {
            logger.Debug("GuardarCambiosButton");
            string newAdjustmentContent = TextUtils.ParseCollectionToAdjustmentDAT(Adjustments);
            logger.Debug("Nuevos ajustes: " + newAdjustmentContent);
            IDeviceHandler deviceHandler = App.Instance.deviceHandler;
            try
            {
                bool overWriteSuccess = deviceHandler.OverWriteAdjustmentMade(newAdjustmentContent);
                if (overWriteSuccess)
                {
                    RedirectToActivityCenterView();
                }
                else
                {
                    await AlertUserMetroDialog("No se ha podido sobreescribir el ajuste deseado.");
                }
            }catch (Exception e)
            {
                logger.Error(e.Message);
                await AlertUserMetroDialog("Ocurrió un error al escribir el archivo de ajuste");
            }
        }

        private void PanelCloseMethod(object sender)
        {
            HidingWaitingPanel();
        }

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

        private async Task<bool> AlertUserMetroDialog(string message, string title = "Aviso")
        {
            MetroDialogSettings settings = new MetroDialogSettings();
            settings.AffirmativeButtonText = "Aceptar";
            Task<MessageDialogResult> showMessageAsync = dialogCoordinator.ShowMessageAsync(this, title, message, MessageDialogStyle.Affirmative, settings);
            MessageDialogResult messsageDialogResult = await showMessageAsync;
            bool resultAffirmative = messsageDialogResult.Equals(MessageDialogResult.Affirmative);
            return resultAffirmative;
        }
        #endregion
    }
}
