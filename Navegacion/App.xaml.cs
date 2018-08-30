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
        private MainWindow mainWindow;
        public MainWindow MainWindowView
        {
            get
            {
                return mainWindow;
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Console.WriteLine("Verificando datos guardados...");
            Random rnd = new Random();
            int numRandom = rnd.Next(0, 2); // Genera un numero mayor a 0 y menor a 2 
            Console.WriteLine("Numero aleatorio generado: " + numRandom);
            if(numRandom == 1)
            {
                Console.WriteLine("Hemos recordado al usuario, logueando...");
                mainWindow = new MainWindow();
                Uri uri = new Uri("View/CentroActividades.xaml", UriKind.Relative);
                //NavigationService ns = NavigationService.GetNavigationService(window.frame.NavigationService);
                mainWindow.frame.NavigationService.Navigate(uri);
                //ns.Navigate(uri);
                mainWindow.Show();
            }
            else
            {
                Console.WriteLine("No hay datos guardados con un usuario válido, llendo al login");
                mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }
    }
}
