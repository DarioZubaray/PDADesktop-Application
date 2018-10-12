using log4net;
using Newtonsoft.Json;
using PDADesktop.Classes;
using PDADesktop.Model;
using PDADesktop.Classes.Utils;
using PDADesktop.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace PDADesktop.ViewModel
{
    class VerAjustesViewModel : ViewModelBase
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
                if(selectedAdjustment != null)
                {
                    AdjustmentTypeSelected = selectedAdjustment.motivo;
                    Textbox_amountValue = selectedAdjustment.cantidad.ToString();
                    Combobox_adjustmentTypeEnabled = true;
                    Textbox_amountEnabled = true;
                }
                else
                {
                    Combobox_adjustmentTypeEnabled = false;
                    Textbox_amountEnabled = false;
                }
                OnPropertyChanged();
            }
        }

        private bool textbox_eanEnabled;
        public bool Textbox_eanEnabled
        {
            get
            {
                return textbox_eanEnabled;
            }
            set
            {
                textbox_eanEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool combobox_adjustmentTypeEnabled;
        public bool Combobox_adjustmentTypeEnabled
        {
            get
            {
                return combobox_adjustmentTypeEnabled;
            }
            set
            {
                combobox_adjustmentTypeEnabled = value;
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
                selectedAdjustment.motivo = adjustmentTypeSelected;
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

        private bool textbox_amountEnabled;
        public bool Textbox_amountEnabled
        {
            get
            {
                return textbox_amountEnabled;
            }
            set
            {
                textbox_amountEnabled = value;
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
                selectedAdjustment.cantidad = Convert.ToInt64(textbox_amountValue);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructor
        public VerAjustesViewModel()
        {
            BannerApp.PrintSeeAdjustments();
            MyAppProperties.isSeeAdjustmentsWindowClosed = false;
            var dispatcher = App.Instance.MainWindow.Dispatcher;
            bool deviceStatus = App.Instance.deviceHandler.IsDeviceConnected();
            if (deviceStatus)
            {
                string deviceReadAdjustmentDataFile = App.Instance.deviceHandler.ReadAdjustmentsDataFile();
                if(deviceReadAdjustmentDataFile != null)
                {
                    Adjustments = JsonUtils.GetObservableCollectionAjustes(deviceReadAdjustmentDataFile);

                    AdjustmentsTypes = HttpWebClientUtil.GetAdjustmentsTypes();
                    logger.Debug(AdjustmentsTypes.ToString());
                }
                else
                {
                    dispatcher.BeginInvoke(
                        new Action(() => UserNotify("No se encotraron ajustes!")),
                        DispatcherPriority.ApplicationIdle);
                }
            }
            else
            {
                dispatcher.BeginInvoke(
                    new Action(() => UserNotify("No se detecta conexion con la PDA")),
                    DispatcherPriority.ApplicationIdle);
            }

            Textbox_eanEnabled = false;
            Combobox_adjustmentTypeEnabled = false;
            Textbox_amountEnabled = false;

            DeleteAdjustmentCommand = new RelayCommand(DeleteAdjustmentMethod);
            UpdateAdjustmentCommand = new RelayCommand(UpdateAdjustmentMethod);
            DiscardChangesCommand = new RelayCommand(DiscardChangesMethod);
            SaveChangesCommand = new RelayCommand(SaveChangesMethod);
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
            foreach(Ajustes adjustment in Adjustments)
            {
                if (adjustment.ean.Equals(SelectedAdjustment.ean))
                {
                    adjustment.motivo = SelectedAdjustment.motivo;
                    adjustment.cantidad = SelectedAdjustment.cantidad;
                    OnPropertyChanged();
                }
            }
            SelectedAdjustment = null;
        }
        public void DiscardChangesMethod(object obj)
        {
            logger.Debug("DescartarCambiosButton");
            string pregunta = "¿Desea descartar los cambios?";
            if (PreguntarAlUsuario(pregunta))
            {
                CloseVerAjustesWindow();
            }
            else
            {
                // revertir el estado del archivo?
                // cual es esta de la lista?
            }

        }
        public void SaveChangesMethod(object obj)
        {
            logger.Debug("GuardarCambiosButton");
            string newAdjustmentContent = TextUtils.ParseCollectionToAdjustmentDAT(Adjustments);
            logger.Debug("Nuevos ajustes: " + newAdjustmentContent);

            if (App.Instance.deviceHandler.OverWriteAdjustmentMade(newAdjustmentContent))
            {
                CloseVerAjustesWindow();
            }
            else
            {
                UserNotify("ERROR");
            }
        }

        private void CloseVerAjustesWindow()
        {
            foreach (Window w in App.Instance.Windows)
            {
                if (w is VerAjustesView)
                {
                    MyAppProperties.isSeeAdjustmentsWindowClosed = true;
                    w.Close();
                    break;
                }
            }
        }

        public void UserNotify(string mensaje)
        {
            string message = mensaje;
            string caption = "Aviso!";
            MessageBoxButton messageBoxButton = MessageBoxButton.OK;
            MessageBoxImage messageBoxImage = MessageBoxImage.Error;
            MessageBoxResult result = MessageBox.Show(message, caption, messageBoxButton, messageBoxImage);
            if(result == MessageBoxResult.OK)
            {
                logger.Debug("Informando al usuario: " + mensaje);
            }
        }

        public bool PreguntarAlUsuario(string pregunta)
        {
            string message = pregunta;
            string caption = "Aviso!";
            MessageBoxButton messageBoxButton = MessageBoxButton.OKCancel;
            MessageBoxImage messageBoxImage = MessageBoxImage.Question;
            MessageBoxResult result = MessageBox.Show(message, caption, messageBoxButton, messageBoxImage);
            logger.Debug("Preguntando al usuario: " + pregunta);
            if (result == MessageBoxResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
