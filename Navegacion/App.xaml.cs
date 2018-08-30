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
                var window = new MainWindow();
                Uri uri = new Uri("View/CentroActividades.xaml", UriKind.Relative);
                //NavigationService ns = NavigationService.GetNavigationService(window.frame.NavigationService);
                window.frame.NavigationService.Navigate(uri);
                //ns.Navigate(uri);
                window.Show();
            }
            else
            {
                Console.WriteLine("No hay datos guardados con un usuario válido, llendo al login");
                var window = new MainWindow();
                window.Show();
            }
        }
    }
}
