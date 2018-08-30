using Navegacion.View;
using System;
using System.Windows;

namespace Navegacion
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow mainWindowView;
        public MainWindow MainWindowView
        {
            get
            {
                return mainWindowView;
            }
            set
            {
                mainWindowView = value;
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Console.WriteLine("Verificando datos guardados...");
            Random rnd = new Random();
            // Genera un numero mayor a 0 y menor a 2 
            int numRandom = rnd.Next(0, 2);
            Console.WriteLine("Numero aleatorio generado: " + numRandom);
            if(numRandom == 1)
            {
                Console.WriteLine("Hemos recordado al usuario, logueando...");
                MainWindowView = new MainWindow();
                Uri uri = new Uri("View/CentroActividades.xaml", UriKind.Relative);
                MainWindowView.frame.NavigationService.Navigate(uri);
                MainWindowView.Show();
            }
            else
            {
                Console.WriteLine("No hay datos guardados con un usuario válido, llendo al login");
                MainWindowView = new MainWindow();
                MainWindowView.Show();
            }
        }
    }
}
