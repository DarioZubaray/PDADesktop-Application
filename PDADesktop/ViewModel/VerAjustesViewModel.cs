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
                if(selectedAdjustment != null)
                {
                    TipoDeAjusteSelected = selectedAdjustment.motivo;
                    Textbox_cantidadValue = selectedAdjustment.cantidad.ToString();
                    Combobox_tipoAjusteEnabled = true;
                    Textbox_cantidadEnabled = true;
                }
                else
                {
                    Combobox_tipoAjusteEnabled = false;
                    Textbox_cantidadEnabled = false;
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

        private bool combobox_tipoAjusteEnabled;
        public bool Combobox_tipoAjusteEnabled
        {
            get
            {
                return combobox_tipoAjusteEnabled;
            }
            set
            {
                combobox_tipoAjusteEnabled = value;
                OnPropertyChanged();
            }
        }

        private string tipoDeAjusteSelected;
        public string TipoDeAjusteSelected
        {
            get
            {
                return tipoDeAjusteSelected;
            }
            set
            {
                tipoDeAjusteSelected = value;
                selectedAdjustment.motivo = tipoDeAjusteSelected;
                OnPropertyChanged();
            }
        }

        private List<string> tiposDeAjustes;
        public List<string> TiposDeAjustes
        {
            get
            {
                return tiposDeAjustes;
            }set
            {
                tiposDeAjustes = value;
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

        private string textbox_cantidadValue;
        public string Textbox_cantidadValue
        {
            get
            {
                return textbox_cantidadValue;
            }
            set
            {
                textbox_cantidadValue = value;
                selectedAdjustment.cantidad = Convert.ToInt64(textbox_cantidadValue);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructor
        public VerAjustesViewModel()
        {
            var dispatcher = Application.Current.MainWindow.Dispatcher;
            bool estadoDevice = App.Instance.deviceHandler.IsDeviceConnected();
            if (estadoDevice)
            {
                string deviceReadDataFile = App.Instance.deviceHandler.ReadAdjustmentsDataFile();
                if(deviceReadDataFile != null)
                {
                    Ajustes = JsonConvert.DeserializeObject<ObservableCollection<Ajustes>>(deviceReadDataFile);

                    // Buscar los tipos de ajustes de pda express
                    // Me preocupa el timeout y el host inalcanzable
                    TiposDeAjustes = HttpWebClient.GetTiposDeAjustes();
                    logger.Debug(TiposDeAjustes.ToString());
                }
                else
                {
                    dispatcher.BeginInvoke(
                        new Action(() => AvisarAlUsuario("No se encotraron ajustes!")),
                        DispatcherPriority.ApplicationIdle);
                }
            }
            else
            {
                dispatcher.BeginInvoke(
                    new Action(() => AvisarAlUsuario("No se detecta conexion con la PDA")),
                    DispatcherPriority.ApplicationIdle);
            }

            Textbox_eanEnabled = false;
            Combobox_tipoAjusteEnabled = false;
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
            if(parametro != null)
                logger.Debug("Parametro: " + parametro.ToString());
            if(SelectedAdjustment != null)
                logger.Debug("AjusteSeleccionado : " + SelectedAdjustment.ToString());

            Ajustes.Remove(SelectedAdjustment);
            SelectedAdjustment = null;
        }
        public void ActualizarAjusteButton(object obj)
        {
            logger.Debug("ActualizarAjusteButton");
            SelectedAdjustment = null;
        }
        public void DescartarCambiosButton(object obj)
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
        public void GuardarCambiosButton(object obj)
        {
            logger.Debug("GuardarCambiosButton");
            string newAdjustmentContent = TextUtils.ParseCollectionToAdjustmentDAT(Ajustes);
            logger.Debug("Nuevos ajustes: " + newAdjustmentContent);

            if (App.Instance.deviceHandler.OverWriteAdjustmentMade(newAdjustmentContent))
            {
                CloseVerAjustesWindow();
            }
            else
            {
                AvisarAlUsuario("ERROR");
            }
        }

        private void CloseVerAjustesWindow()
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w is VerAjustesView)
                {
                    w.Close();
                    break;
                }
            }
        }

        public void AvisarAlUsuario(string mensaje)
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
