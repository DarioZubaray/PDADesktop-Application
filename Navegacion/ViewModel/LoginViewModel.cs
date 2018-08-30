using Navegacion.Classes;
using Navegacion.View;
using System;
using System.Windows;
using System.Windows.Input;

namespace Navegacion.ViewModel
{
    class LoginViewModel
    {
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

        #region Constructor
        public LoginViewModel()
        {
            LoginButtonCommand = new RelayCommand(LoginPortalApi, param => this.canExecute);
        }
        #endregion

        public void LoginPortalApi(object obj)
        {
            Console.WriteLine("login portal api");
            Uri uri = new Uri("View/CentroActividades.xaml", UriKind.Relative);
            MainWindow window = (MainWindow) Application.Current.MainWindow;
            window.frame.NavigationService.Navigate(uri);
        }
    }
}
