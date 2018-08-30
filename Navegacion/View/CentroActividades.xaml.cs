using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Navegacion.View
{
    /// <summary>
    /// Lógica de interacción para CentroActividades.xaml
    /// </summary>
    public partial class CentroActividades : UserControl
    {
        public CentroActividades()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("View/LoginView.xaml", UriKind.Relative);
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(uri);
        }
    }
}
