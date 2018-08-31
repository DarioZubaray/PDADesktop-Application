using log4net;
using Navegacion.View;
using System;
using System.Configuration;
using System.Globalization;
using System.Windows;

namespace Navegacion
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
            logger.Info("Verificando datos guardados...");
            VerificarDatosGuardados();
            Random rnd = new Random();
            // Genera un numero mayor a 0 y menor a 2 
            int numRandom = rnd.Next(0, 2);


            logger.Debug("Numero aleatorio generado: " + numRandom);
            if(numRandom == 1)
            {
                logger.Info("Hemos recordado al usuario, logueando...");

                MainWindowView = new MainWindow();
                Uri uri = new Uri("View/CentroActividades.xaml", UriKind.Relative);
                MainWindowView.frame.NavigationService.Navigate(uri);
                MainWindowView.Show();
            }
            else
            {
                logger.Info("No hay datos guardados con un usuario válido, llendo al login");
                MainWindowView = new MainWindow();
                MainWindowView.Show();
            }
        }

        private void VerificarDatosGuardados()
        {
            string urlFromProperties = ConfigurationManager.AppSettings.Get("URL_COOKIE");
            string urlCookie = Environment.ExpandEnvironmentVariables(urlFromProperties);
            Uri uriCookie = new Uri(urlCookie);
            logger.Info(urlCookie);
            try
            {
                string cookie = Application.GetCookie(uriCookie);

                logger.Info("Cookie encontrada: " + cookie);
                string nombre = cookie.Substring(cookie.IndexOf("=")+1);
                logger.Info("nombre obtenido: " + nombre);
            }
            catch(Exception e)
            {
                logger.Error(e.GetBaseException().ToString());
            }
        }

    }
}
