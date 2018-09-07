using log4net;
using PDADesktop.Classes;
using PDADesktop.View;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Input;

namespace PDADesktop.ViewModel
{
    class LoginViewModel
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string usernameText { get; set; }
        public string FloatingPasswordBox { get; set; }

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
        private bool canExecute = true;

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

        #region Constructor
        public LoginViewModel()
        {
            LoginButtonCommand = new RelayCommand(LoginPortalApi, param => this.canExecute);
            RecuerdameCheck = true;
        }
        #endregion

        public void LoginPortalApi(object obj)
        {
            logger.Info("login portal api");
            logger.Debug("Usuario: " + usernameText);
            logger.Debug("Constraseña: " + FloatingPasswordBox + ", para fines de desarrollo");

            MainWindow window = (MainWindow) Application.Current.MainWindow;
            if ("dariojz".Equals(usernameText))
            {
                logger.Debug("Nombre no null: " + usernameText);
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
                Uri uri = new Uri("View/CentroActividades.xaml", UriKind.Relative);
                window.frame.NavigationService.Navigate(uri);
            }
            else
            {
                //marcar como usuario y/o contraseña incorrectos
                LoginView loginview = (LoginView) window.frame.Content;
                logger.Error("usuario y/o contraseña incorrectos");
                loginview.msgbar.Clear();
                loginview.msgbar.SetDangerAlert("usuario y/o contraseña incorrectos", 3);
                loginview.usernameText.Text = "";
                loginview.FloatingPasswordBox.Clear();
                loginview.usernameText.Focus();
            }
        }

    }
}
