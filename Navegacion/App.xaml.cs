using log4net;
using Navegacion.Classes;
using Navegacion.Model;
using Navegacion.View;
using System;
using System.Collections.Generic;
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

            /*
             1* check conexion PDAExpress server
             2* check conexion PDAMoto
             */

            //TODO manejar timeout, reconexion
            string urlSincronizacion = "http://localhost:8080/pdaexpress/pdadesktopdemo/getSincronizacionActual.action";
            string queryParam = "?idSucursal=706";
            List<Sincronizacion> sincro = HttpWebClient.GetHttpWebSincronizacion(urlSincronizacion+queryParam);
            logger.Info(sincro);

            int respuestaDll = MotoApi.isDeviceConnected();
            bool boolValue = respuestaDll != 0;
            logger.Info("PDA is connected: " + boolValue);

            logger.Info("Verificando datos guardados...");
            string usuario = VerificarDatosGuardados();
            if(usuario != null)
            {
                logger.Info("cookie de usuario encontrada: " + usuario);
            }

            /* 3* verificar cookies guardadas y loguear ó redirigir al login*/

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

        private string VerificarDatosGuardados()
        {
            return CookieManager.getCookie(CookieManager.Cookie.usuario);
        }

    }
}
