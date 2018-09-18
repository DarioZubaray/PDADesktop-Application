﻿using log4net;
using Newtonsoft.Json;
using PDADesktop.Classes;
using PDADesktop.Model;
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

                    // Buscar los tipos de ajustes de pda express
                    // Me preocupa el timeout y el host inalcanzable
                    string urlTiposAjustes = ConfigurationManager.AppSettings.Get("API_GET_TIPOS_AJUSTES");
                    TiposDeAjustes = HttpWebClient.GetTiposDeAjustes(urlTiposAjustes);
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
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is VerAjustesView)
                    {
                        w.Close();
                        break;
                    }
                }
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
