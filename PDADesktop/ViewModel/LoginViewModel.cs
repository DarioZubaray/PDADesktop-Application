using log4net;
using MaterialDesignThemes.Wpf;
using PDADesktop.Classes;
using PDADesktop.View;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace PDADesktop.ViewModel
{
    class LoginViewModel : ViewModelBase
    {
        #region attributes
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly BackgroundWorker loginWorker = new BackgroundWorker();
        public string usernameText { get; set; }
        public string FloatingPasswordBox { get; set; }

        private bool remembermeCheck;
        public bool RemembermeCheck
        {
            get
            {
                return remembermeCheck;
            }
            set
            {
                remembermeCheck = value;
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
        private ICommand flipLoginCommand;
        public ICommand FlipLoginCommand
        {
            get
            {
                return flipLoginCommand;
            }
            set
            {
                flipLoginCommand = value;
            }
        }

        private bool canExecute = true;
        #endregion
        
        #region Constructor
        public LoginViewModel()
        {
            BannerApp.PrintLogin();
            MyAppProperties.window = (MainWindow)Application.Current.MainWindow;
            LoginButtonCommand = new RelayCommand(LoginPortalApi, param => this.canExecute);
            ShowPanelCommand = new RelayCommand(ShowPanel, param => this.canExecute);
            HidePanelCommand = new RelayCommand(HidePanel, param => this.canExecute);
            ChangeMainMessageCommand = new RelayCommand(ChangeMainMensage, param => this.canExecute);
            ChangeSubMessageCommand = new RelayCommand(ChangeSubMensage, param => this.canExecute);
            PanelCloseCommand = new RelayCommand(ClosePanel, param => this.canExecute);
            FlipLoginCommand = new RelayCommand(FlipLoginMethod);
            loginWorker.DoWork += worker_DoWork;
            loginWorker.RunWorkerCompleted += worker_RunWorkerCompleted;
            RemembermeCheck = true;
        }
        #endregion

        #region Worker Method
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // run all background tasks here
            logger.Debug("login worker -> doWork");
            Thread.Sleep(1200);
            MainWindow window = MyAppProperties.window;
            if ("juli".Equals(usernameText))
            {
                logger.Debug("recuerdame: " + RemembermeCheck);
                logger.Debug("Nombre no null: " + usernameText);
                /*
                DateTime fechaExpiracion = new DateTime(2020, 12, 31);
                string userCookie = "username=" + usernameText + ";expires=" + fechaExpiracion.ToString("r");
                string urlFromProperties = ConfigurationManager.AppSettings.Get("URL_COOKIE");
                string variablePublic = Environment.ExpandEnvironmentVariables(urlFromProperties);
                Uri cookieUri1 = new Uri(variablePublic);
                Application.SetCookie(cookieUri1, userCookie);

                string cookie = Application.GetCookie(cookieUri1);
                logger.Info("Cookie recibida: " + cookie);
                */
                //aca deberia llamar al servicio de login
                //Redirecciona a centroActividades
                Uri uriActivityCenter = new Uri(Constants.CENTRO_ACTIVIDADES_VIEW, UriKind.Relative);

                var dispatcher = Application.Current.Dispatcher;
                dispatcher.BeginInvoke(new Action(() =>
                {
                    window.frame.NavigationService.Navigate(uriActivityCenter);
                }));
                
            }
            else
            {
                //marcar como usuario y/o contraseña incorrectos
                logger.Info("usuario y/o contraseña incorrectos");
                var dispatcher = Application.Current.Dispatcher;
                dispatcher.BeginInvoke(new Action(() =>
                {
                    LoginView loginview = (LoginView)window.frame.Content;
                    loginview.msgbar.Clear();
                    loginview.msgbar.SetDangerAlert("usuario y/o contraseña incorrectos", 3);
                    loginview.usernameText.Text = String.Empty;
                    loginview.FloatingPasswordBox.Clear();
                    loginview.usernameText.Focus();
                    PanelLoading = false;
                }));
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //update ui once worker complete his work
            logger.Debug("login Worker ->runWorkedCompleted");
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.BeginInvoke(new Action(() =>
            {
                PanelLoading = false;
            }));
        }
        #endregion

        #region methods
        public void LoginPortalApi(object obj)
        {
            logger.Info("login portal api");
            logger.Debug("Usuario: " + usernameText);
            logger.Debug("Constraseña: " + FloatingPasswordBox + ", para fines de desarrollo");
            PanelMainMessage = "Espere por favor ...";
            loginWorker.RunWorkerAsync();
        }
        #endregion

        #region panel methods
        public void ShowPanel(object obj)
        {
            logger.Debug("Mostrando panel de carga");
            PanelLoading = true;
        }

        public void HidePanel(object obj)
        {
            logger.Debug("Ocultando panel de carga");
            PanelLoading = false;
        }
        public void ChangeMainMensage(object obj)
        {
            PanelMainMessage = "Espere por favor";
        }

        public void ChangeSubMensage(object obj)
        {
            PanelSubMessage = "";
        }

        public void ClosePanel(object obj)
        {
            PanelLoading = false;
        }

        public void FlipLoginMethod(object obj)
        {
            logger.Debug("Invocando a flip command");
            MainWindow window = MyAppProperties.window;
            //if ( usernameText?.Length > 3)
            //{
                Flipper.FlipCommand.Execute(null, null);
                LoginView loginview = (LoginView)window.frame.Content;
                loginview.FloatingPasswordBox.Focus();
            //}
            //else
            //{
            //    LoginView loginview = (LoginView)window.frame.Content;
            //    loginview.msgbar.Clear();
            //    loginview.msgbar.SetWarningAlert("Especifique un usuario de por lo menos 4 caracteres", 3);
            //    loginview.usernameText.Text = String.Empty;
            //    loginview.FloatingPasswordBox.Clear();
            //    loginview.usernameText.Focus();
            //}
        }
        #endregion
    }
}
