using log4net;
using Newtonsoft.Json;
using PDADesktop.Classes;
using PDADesktop.Model;
using System;
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
        private ObservableCollection<Ajustes> ajustes;
        public ObservableCollection<Ajustes> Ajustes
        {
            get
            {
                return ajustes;
            }
            set
            {
                ajustes = value;
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
                OnPropertyChanged();
                if(selectedAdjustment != null)
                {
                    Textbox_cantidadEnabled = true;
                }
                else
                {
                    Textbox_cantidadEnabled = false;
                }
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
        private bool textbox_motivoEnabled;
        public bool Textbox_motivoEnabled
        {
            get
            {
                return textbox_motivoEnabled;
            }
            set
            {
                textbox_motivoEnabled = value;
                OnPropertyChanged();
            }
        }
        private bool textbox_cantidadEnabled;
        public bool Textbox_cantidadEnabled
        {
            get
            {
                return textbox_cantidadEnabled;
            }
            set
            {
                textbox_cantidadEnabled = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public VerAjustesViewModel()
        {
            int estadoPda = MotoApi.isDeviceConnected();
            var dispatcher = Application.Current.MainWindow.Dispatcher;
            if (estadoPda != 0)
            {
                string clientDataDir = ConfigurationManager.AppSettings.Get("CLIENT_PATH_DATA");
                string fileName = ConfigurationManager.AppSettings.Get("DEVICE_FILE_AJUSTES");
                string motoApiReadDataFile = MotoApi.ReadDataFile(clientDataDir, fileName);
                if(motoApiReadDataFile != null)
                {
                    Ajustes = JsonConvert.DeserializeObject<ObservableCollection<Ajustes>>(motoApiReadDataFile);
                }
                else
                {
                    dispatcher.BeginInvoke(
                        new Action(() => AvisoAlUsuario("No se encotraron ajustes!")), 
                        DispatcherPriority.ApplicationIdle);
                }
            }
            else
            {
                dispatcher.BeginInvoke(
                    new Action(() => AvisoAlUsuario("No se detecta conexion con la PDA")),
                    DispatcherPriority.ApplicationIdle);
            }

            Textbox_eanEnabled = false;
            Textbox_motivoEnabled = false;
            Textbox_cantidadEnabled = false;

            EliminarAjusteCommand = new RelayCommand(EliminarAjusteButton);
            ActualizarAjusteCommand = new RelayCommand(ActualizarAjusteButton);
            DescartarCambiosCommand = new RelayCommand(DescartarCambiosButton);
            GuardarCambiosCommand = new RelayCommand(GuardarCambiosButton);
        }
        #endregion

        #region Commands
        private ICommand eliminarAjusteCommand;
        public ICommand EliminarAjusteCommand
        {
            get
            {
                return eliminarAjusteCommand;
            }
            set
            {
                eliminarAjusteCommand = value;
            }
        }

        private ICommand actualizarAjusteCommand;
        public ICommand ActualizarAjusteCommand
        {
            get
            {
                return actualizarAjusteCommand;
            }
            set
            {
                actualizarAjusteCommand = value;
            }
        }

        private ICommand descartarCambiosCommand;
        public ICommand DescartarCambiosCommand
        {
            get
            {
                return descartarCambiosCommand;
            }
            set
            {
                descartarCambiosCommand = value;
            }
        }

        private ICommand guardarAjustesCommand;
        public ICommand GuardarCambiosCommand
        {
            get
            {
                return guardarAjustesCommand;
            }
            set
            {
                guardarAjustesCommand = value;
            }
        }
        #endregion

        #region Methods
        public void EliminarAjusteButton(object obj)
        {
            logger.Debug("EliminarAjusteButton");
            Ajustes parametro = obj as Ajustes;
            logger.Debug("Parametro: " + parametro.ToString());
            logger.Debug("AjusteSeleccionado : " + SelectedAdjustment.ToString());

            Ajustes.Remove(SelectedAdjustment);
            SelectedAdjustment = null;
        }
        public void ActualizarAjusteButton(object obj)
        {
            logger.Debug("ActualizarAjusteButton");
        }
        public void DescartarCambiosButton(object obj)
        {
            logger.Debug("DescartarCambiosButton");
        }
        public void GuardarCambiosButton(object obj)
        {
            logger.Debug("GuardarCambiosButton");
        }

        public void AvisoAlUsuario(string mensaje)
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
        #endregion
    }
}
