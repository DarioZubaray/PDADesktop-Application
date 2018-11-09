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
using System.Windows.Threading;

namespace PDADesktop.ViewModel
{
    class VerAjustesRealizadosViewModel : ViewModelBase
    {
        #region Attributes
        #region Common Attributes
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDialogCoordinator dialogCoordinator;
        private IDeviceHandler deviceHandler { get; set; }

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
        private ICommand verAjustesRealizadosLoadedEvent;
        public ICommand VerAjustesRealizadosLoadedEvent
        {
            get
            {
                return verAjustesRealizadosLoadedEvent;
            }
            set
            {
                verAjustesRealizadosLoadedEvent = value;
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

        #region Dispatcher Attributes
        Dispatcher dispatcher { get; set; }
        #endregion
        #endregion

        #region Constructor
        public VerAjustesRealizadosViewModel(IDialogCoordinator instance)
        {
            BannerApp.PrintSeeAdjustmentsRealized();
            dialogCoordinator = instance;
            deviceHandler = App.Instance.deviceHandler;
            DisplayWaitingPanel("Cargando", "Espere por favor...");

            VerAjustesRealizadosLoadedEvent = new RelayCommand(VerAjustesRealizadosLoadedEventAction);
            AdjustmentEnableEdit = false;

            DeleteAdjustmentCommand = new RelayCommand(DeleteAdjustmentAction);
            UpdateAdjustmentCommand = new RelayCommand(UpdateAdjustmentAction);
            DiscardChangesCommand = new RelayCommand(DiscardChangesAction);
            SaveChangesCommand = new RelayCommand(SaveChangesAction);
            PanelCloseCommand = new RelayCommand(PanelCloseAction);
        }
        #endregion

        #region Event Method
        public void VerAjustesRealizadosLoadedEventAction(object sender)
        {
            logger.Debug("Ver ajustes Realizados => Loaded Event");
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
                    AlertUserMetroDialog("Ocurrió un error en la lectura del archivo de ajuste.");
                }
                if (deviceReadAdjustmentDataFile != null)
                {
                    UpdateAjustesGrid(deviceReadAdjustmentDataFile);
                }
                else
                {
                    AlertUserMetroDialog("No se encotraron ajustes!");
                }
            }
            else
            {
                AlertUserMetroDialog("No se detecta conexión con la PDA");
            }
            dispatcher.BeginInvoke(new Action(() => HidingWaitingPanel()));
        }

        private void UpdateAjustesGrid(string deviceReadAdjustmentDataFile)
        {
            Adjustments = JsonUtils.GetObservableCollectionAjustes(deviceReadAdjustmentDataFile);

            AdjustmentsTypes = HttpWebClientUtil.GetAdjustmentsTypes();
            logger.Debug(AdjustmentsTypes.ToString());
        }
        #endregion

        #region Action Methods
        public void DeleteAdjustmentAction(object obj)
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
        public void UpdateAdjustmentAction(object obj)
        {
            logger.Debug("ActualizarAjusteButton");
            SelectedAdjustment = null;
        }
        public async void DiscardChangesAction(object obj)
        {
            logger.Debug("DescartarCambiosButton");
            string pregunta = "¿Desea descartar los cambios?";
            bool discardChanges = await AskUserMetroDialog(pregunta);
            if (discardChanges)
            {
                RedirectToActivityCenterView();
            }
        }

        public void SaveChangesAction(object obj)
        {
            logger.Debug("GuardarCambiosButton");
            string newAdjustmentContent = TextUtils.ParseCollectionToAdjustmentDAT(Adjustments);
            logger.Debug("Nuevos ajustes: " + newAdjustmentContent);
            try
            {
                bool overWriteSuccess = deviceHandler.OverWriteAdjustmentMade(newAdjustmentContent);
                if (overWriteSuccess)
                {
                    RedirectToActivityCenterView();
                }
                else
                {
                    AlertUserMetroDialog("No se ha podido sobreescribir el ajuste deseado.");
                }
            }catch (Exception e)
            {
                logger.Error(e.Message);
                AlertUserMetroDialog("Ocurrió un error al escribir el archivo de ajuste");
            }
        }
        #endregion

        #region Panel Methods
        private void PanelCloseAction(object sender)
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

        private async void AlertUserMetroDialog(string message, string title = "Aviso")
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
