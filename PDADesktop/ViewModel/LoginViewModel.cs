﻿using log4net;
using MahApps.Metro.Controls.Dialogs;
using MaterialDesignThemes.Wpf;
using PDADesktop.Classes;
using PDADesktop.Classes.Utils;
using PDADesktop.Model.Portal;
using PDADesktop.View;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace PDADesktop.ViewModel
{
    class LoginViewModel : ViewModelBase
    {
        #region Attributes
        #region Commons Attributes
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDialogCoordinator dialogCoordinator;

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
        #endregion

        #region Worker Attributes
        private readonly BackgroundWorker loadLoginWorker = new BackgroundWorker();
        private readonly BackgroundWorker loginWorker = new BackgroundWorker();
        #endregion

        #region Dispatcher Attributes
        private Dispatcher dispatcher { get; set; }
        #endregion

        #region Panel Loading Attributes
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

        #region Command Attributes
        private ICommand loginLoadedEvent;
        public ICommand LoginLoadedEvent
        {
            get
            {
                return loginLoadedEvent;
            }
            set
            {
                loginLoadedEvent = value;
            }
        }

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

        #region Notifier Attributes
        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.BottomRight,
                offsetX: 10,
                offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(5),
                maximumNotificationCount: MaximumNotificationCount.FromCount(3));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });
        #endregion

        #endregion

        #region Constructor
        public LoginViewModel(IDialogCoordinator instance)
        {
            BannerApp.PrintLogin();
            MyAppProperties.window = (MainWindow)Application.Current.MainWindow;
            dialogCoordinator = instance;
            dispatcher = App.Instance.Dispatcher;
            DisplayWaitingPanel("Inicializando PDA Desktop Application");

            LoginLoadedEvent = new RelayCommand(LoginLoadedEventAction);
            LoginButtonCommand = new RelayCommand(LoginPortalApiAction, param => this.canExecute);
            ShowPanelCommand = new RelayCommand(ShowPanelAction, param => this.canExecute);
            HidePanelCommand = new RelayCommand(HidePanelAction, param => this.canExecute);
            ChangeMainMessageCommand = new RelayCommand(ChangeMainMensageAction, param => this.canExecute);
            ChangeSubMessageCommand = new RelayCommand(ChangeSubMensageAction, param => this.canExecute);
            PanelCloseCommand = new RelayCommand(ClosePanelAction, param => this.canExecute);
            FlipLoginCommand = new RelayCommand(FlipLoginAction);

            loadLoginWorker.DoWork += loadLoginWorker_DoWork;
            loadLoginWorker.RunWorkerCompleted += loadLoginWorker_RunWorkerCompleted;
            loginWorker.DoWork += loginWorker_DoWork;
            loginWorker.RunWorkerCompleted += loginWorker_RunWorkerCompleted;

            RemembermeCheck = true;
        }
        #endregion

        #region Event Method
        public void LoginLoadedEventAction(object sender)
        {
            logger.Debug("Login => Loaded Event");
            loadLoginWorker.RunWorkerAsync();
        }
        #endregion

        #region Workers
        #region Load Login Worker
        private void loadLoginWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool portalStatus = HttpWebClientUtil.GetHttpWebPortalServerConexionStatus();
            if (!portalStatus)
            {
                dispatcher.BeginInvoke(new Action(() => {
                    notifier.ShowError("No se a podido conectar con el Portal de Aplicaciones - ImagoSur");
                }));
            }

            bool serverStatus = HttpWebClientUtil.GetHttpWebServerConexionStatus();
            if (!serverStatus)
            {
                dispatcher.BeginInvoke(new Action(() => {
                    notifier.ShowWarning("El servidor PDAExpress no ha respondido a tiempo.");
                }));
            }
        }

        private void loadLoginWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcher.BeginInvoke(new Action(() => {
                HidingWaitingPanel();
            }));
        }
        #endregion

        #region Login Worker
        private void loginWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Debug("login => Do Work");
            Thread.Sleep(1200);
            MainWindow window = MyAppProperties.window;

            bool portalStatus = HttpWebClientUtil.GetHttpWebPortalServerConexionStatus();
            if (portalStatus)
            {
                UserKey userKey = HttpWebClientUtil.AttemptAuthenticatePortalImagoSur(usernameText, FloatingPasswordBox);
                logger.Debug(userKey);
                if (userKey != null)
                {
                    if (AuthorizeUserKey(userKey))
                    {
                        logger.Info("userKey " + userKey.key);
                        MyAppProperties.storeId = userKey.user.sucursal.ToString();
                        MyAppProperties.username = userKey.user.userName;

                        Uri uriActivityCenter = new Uri(Constants.CENTRO_ACTIVIDADES_VIEW, UriKind.Relative);
                        dispatcher.BeginInvoke(new Action(() =>
                        {
                            window.frame.NavigationService.Navigate(uriActivityCenter);
                        }));
                    }
                    else
                    {
                        logger.Info("Sin perfil PDAExpress");
                        dispatcher.BeginInvoke(new Action(() =>
                        {
                            LoginView loginview = (LoginView)window.frame.Content;
                            ShowMSGBar(loginview, "No posee el perfil PDAExpress para acceder", "danger");
                            ResetLoginForm(loginview);
                            PanelLoading = false;
                        }));
                    }
                }
                else
                {
                    logger.Info("usuario y/o contraseña incorrectos");
                    dispatcher.BeginInvoke(new Action(() =>
                    {
                        LoginView loginview = (LoginView)window.frame.Content;
                        ShowMSGBar(loginview, "usuario y/o contraseña incorrectos");
                        ResetLoginForm(loginview);
                        PanelLoading = false;
                    }));
                }
            }
            else
            {
                dispatcher.BeginInvoke(new Action(() => {
                    notifier.ShowError("No se a podido conectar con el Portal de Aplicaciones - ImagoSur");
                }));
            }
        }

        private bool AuthorizeUserKey(UserKey userKey)
        {
            try
            {
                var perfilesSet = userKey.user.perfilesSet;
                foreach (var perfil in perfilesSet)
                {
                    string desc = perfil.descripcion.Replace(" ", "").ToLower();
                    if (desc.Equals("pdaexpress", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
            }
            return false;
        }

        private void ShowMSGBar(LoginView loginview, string message, string type = "warning")
        {
            loginview.msgbar.Clear();
            if(type.Equals("warning"))
            {
                loginview.msgbar.SetWarningAlert(message, 3);
            }
            else
            {
                loginview.msgbar.SetDangerAlert(message, 3);
            }
        }

        private void ResetLoginForm(LoginView loginview)
        {
            loginview.usernameText.Text = String.Empty;
            loginview.FloatingPasswordBox.Clear();
            loginview.usernameText.Focus();
        }

        private void loginWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("login => Run Worker Completed");
            dispatcher.BeginInvoke(new Action(() =>
            {
                PanelLoading = false;
            }));
        }
        #endregion
        #endregion

        #region Commons Methods
        public void LoginPortalApiAction(object obj)
        {
            DisplayWaitingPanel("Espere por favor ...");
            FlipLoginAction(null);
            logger.Info("login portal api");
            logger.Debug("Usuario: " + usernameText);
            logger.Debug("Constraseña: " + FloatingPasswordBox + ", para fines de desarrollo");
            PanelMainMessage = "Espere por favor ...";
            loginWorker.RunWorkerAsync();
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

        #region Panel Methods
        public void ShowPanelAction(object obj)
        {
            logger.Debug("Mostrando panel de carga");
            DisplayWaitingPanel(String.Empty);
        }

        public void HidePanelAction(object obj)
        {
            logger.Debug("Ocultando panel de carga");
            HidingWaitingPanel();
        }
        public void ChangeMainMensageAction(object obj)
        {
            PanelMainMessage = "Espere por favor";
        }

        public void ChangeSubMensageAction(object obj)
        {
            PanelSubMessage = "";
        }

        public void ClosePanelAction(object obj)
        {
            PanelLoading = false;
        }
        #endregion

        #region Action Methods
        public void FlipLoginAction(object obj)
        {
            logger.Debug("Invocando a flip command");
            MainWindow window = MyAppProperties.window;

            Flipper.FlipCommand.Execute(null, null);
            LoginView loginview = (LoginView)window.frame.Content;
            loginview.FloatingPasswordBox.Focus();
            Keyboard.Focus(loginview.FloatingPasswordBox);
        }
        #endregion
    }
}
