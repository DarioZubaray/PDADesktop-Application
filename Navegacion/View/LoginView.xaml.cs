using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Navegacion.View
{
    /// <summary>
    /// Lógica de interacción para LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //validar
            string usuario = this.SearchTermTextBox.Text;
            Console.WriteLine("Nombre de usuario: " + usuario);
            if(usuario.Equals("dario"))
            {
                //aca deberia llamar al servicio de login
                //Redirecciona a centroActividades
                Uri uri = new Uri("View/CentroActividades.xaml", UriKind.Relative);
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns.Navigate(uri);
            }
            else
            {
                //marcar como usuario y/o contraseña incorrectos
                this.SearchTermTextBox.BorderBrush = Brushes.Red;
                this.mjsError.Content = "usuario y/o contraseña incorrectos";
                this.mjsError.Visibility = Visibility.Visible;
            }
        }
    }
}
