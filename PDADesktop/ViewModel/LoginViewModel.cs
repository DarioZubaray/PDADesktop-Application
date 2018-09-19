using log4net;
using PDADesktop.Classes;
using PDADesktop.View;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace PDADesktop.ViewModel
{
    class LoginViewModel : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region attributes
        private readonly BackgroundWorker worker = new BackgroundWorker();
        public string usernameText { get; set; }
        public string FloatingPasswordBox { get; set; }

        private bool recuerdameCheck;
        public bool RecuerdameCheck
        {
            get
            {
                return recuerdameCheck;
            }
            set
            {
                recuerdameCheck = value;
            }
        }

        // Atributos del Spiner
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

        #region Command
        private ICommand loginButtonCommand;
        public ICommand LoginButtonCommand
        {
            get
            {
                return loginButtonCommand;
            }
            set
            {
                loginButtonCommand = value;
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

        private ICommand hidePanelCommand;
        public ICommand HidePanelCommand
        {
            get
            {
                return hidePanelCommand;
            }
            set
            {
                hidePanelCommand = value;
            }
        }

        private ICommand changeMainMessageCommand;
        public ICommand ChangeMainMessageCommand
        {
            get
            {
                return changeMainMessageCommand;
            }
            set
            {
                changeMainMessageCommand = value;
            }
        }

        private ICommand changeSubMessageCommand;
        public ICommand ChangeSubMessageCommand
        {
            get
            {
                return changeSubMessageCommand;
            }
            set
            {
                changeSubMessageCommand = value;
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

        private bool canExecute = true;
        #endregion
        
        #region Constructor
        public LoginViewModel()
        {
            LoginButtonCommand = new RelayCommand(LoginPortalApi, param => this.canExecute);
            ShowPanelCommand = new RelayCommand(MostrarPanel, param => this.canExecute);
            HidePanelCommand = new RelayCommand(OcultarPanel, param => this.canExecute);
            ChangeMainMessageCommand = new RelayCommand(CambiarMainMensage, param => this.canExecute);
            ChangeSubMessageCommand = new RelayCommand(CambiarSubMensage, param => this.canExecute);
            PanelCloseCommand = new RelayCommand(CerrarPanel, param => this.canExecute);
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            RecuerdameCheck = true;
        }
        #endregion

        #region Worker Method
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // run all background tasks here
            logger.Debug("worker doWork: " + sender);
            Thread.Sleep(1200);
            MainWindow window = MyAppProperties.window;
            if ("juli".Equals(usernameText))
            {
                logger.Debug("Nombre no null: " + usernameText);
                logger.Debug("recuerdame: " + RecuerdameCheck);
                DateTime fechaExpiracion = new DateTime(2020, 12, 31);
                string userCookie = "username=" + usernameText + ";expires=" + fechaExpiracion.ToString("r");
                string urlFromProperties = ConfigurationManager.AppSettings.Get("URL_COOKIE");
                string variablePublic = Environment.ExpandEnvironmentVariables(urlFromProperties);
                Uri cookieUri1 = new Uri(variablePublic);
                Application.SetCookie(cookieUri1, userCookie);

                string cookie = Application.GetCookie(cookieUri1);
                logger.Info("Cookie recibida: " + cookie);
                //aca deberia llamar al servicio de login
                //Redirecciona a centroActividades
                Uri uri = new Uri("View/CentroActividadesView.xaml", UriKind.Relative);

                var dispatcher = Application.Current.Dispatcher;
                dispatcher.BeginInvoke(new Action(() =>
                {
                    window.frame.NavigationService.Navigate(uri);
                }));
                
            }
            else
            {
                //marcar como usuario y/o contraseña incorrectos
                logger.Error("usuario y/o contraseña incorrectos");
                var dispatcher = Application.Current.Dispatcher;
                dispatcher.BeginInvoke(new Action(() =>
                {
                    LoginView loginview = (LoginView)window.frame.Content;
                    loginview.msgbar.Clear();
                    loginview.msgbar.SetDangerAlert("usuario y/o contraseña incorrectos", 3);
                    loginview.usernameText.Text = "";
                    loginview.FloatingPasswordBox.Clear();
                    loginview.usernameText.Focus();
                    PanelLoading = false;
                }));
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //update ui once worker complete his work
            logger.Debug("Worker runWorkedCompleted: " + sender);
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.BeginInvoke(new Action(() =>
            {
                PanelLoading = false;
            }));
        }
        #endregion

        public void LoginPortalApi(object obj)
        {
            logger.Info("login portal api");
            logger.Debug("Usuario: " + usernameText);
            logger.Debug("Constraseña: " + FloatingPasswordBox + ", para fines de desarrollo");
            MyAppProperties.window = (MainWindow) Application.Current.MainWindow;
            PanelMainMessage = "Espere por favor ...";
            worker.RunWorkerAsync();
        }
        public void MostrarPanel(object obj)
        {
            logger.Debug("Mostrando panel de carga");
            PanelLoading = true;
        }
        public void OcultarPanel(object obj)
        {
            logger.Debug("Ocultando panel de carga");
            PanelLoading = false;
        }
        public void CambiarMainMensage(object obj)
        {
            PanelMainMessage = "Espere por favor";
        }
        public void CambiarSubMensage(object obj)
        {
            PanelSubMessage = "";
        }
        public void CerrarPanel(object obj)
        {
            PanelLoading = false;
        }
    }
}
