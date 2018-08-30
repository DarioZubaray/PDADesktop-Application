using Navegacion.Classes;
using Navegacion.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Navegacion.ViewModel
{
    class CentroActividadesViewModel
    {
        private ICommand exitButtonCommand;
        public ICommand ExitButtonCommand
        {
            get
            {
                return exitButtonCommand;
            }
            set
            {
                exitButtonCommand = value;
            }
        }
        private bool canExecute = true;

        #region Constructor
        public CentroActividadesViewModel()
        {
            ExitButtonCommand = new RelayCommand(ExitPortalApi, param => this.canExecute);
        }
        #endregion

        public void ExitPortalApi(object obj)
        {
            Console.WriteLine("exit portal api");
            //aca deberia invocar el logout al portal?
            MainWindow window = (MainWindow) Application.Current.MainWindow;
            Uri uri = new Uri("View/LoginView.xaml", UriKind.Relative);
            window.frame.NavigationService.Navigate(uri);
        }
    }
}
