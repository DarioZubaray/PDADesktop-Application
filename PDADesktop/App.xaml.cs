using log4net;
using PDADesktop.Classes;
using PDADesktop.Classes.Devices;
using PDADesktop.View;
using Squirrel;
using StructureMap;
using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace PDADesktop
{
    public partial class App : Application
    {
        #region attributes
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private UpdateManager updateManager { get; set; }
        public static App Instance { get; private set; }
        public Container Container { get; private set; }
        private static Mutex _mutex = null;
        public IDeviceHandler deviceHandler { get; private set; }
        #endregion

        #region constructor
        public App()
        {
            BannerApp.PrintAppBanner();
            Instance = this;
            Container = new Container(c =>
            {
                c.AddRegistry(new MyContainerInitializer());
            });
            deviceHandler = this.Container.GetInstance<IDeviceHandler>();
            logger.Info("Adaptador: " + deviceHandler.GetAdapterName());
        }
        #endregion

        #region startup
        protected override void OnStartup(StartupEventArgs e)
        {
            CheckApplicationRunning();
            base.OnStartup(e);

            MyAppProperties.loadOnce = true;

            logger.Debug("Verificando en segundo plano actualizaciones con squirrel.window");
            UpdateApp();

            logger.Debug("Verificando datos guardados...");
            string isUserReminded = VerifySavedData();

            if (isUserReminded != null)
            {
                bool check = isUserReminded == "true";
                if(check)
                {
                    // user reminded

                    // getUserCredentials();
                    // AttemptAutoLoginPortalImagoSur();
                    if (GenerandoAleatoriedadDeCasosLogueados() == 1)
                    {
                        RedirectToActivitiesCenter();
                    }
                    else
                    {
                        // fail to autoLogin
                        RedirectLogin();
                    }
                }
                else
                {
                    // no reminded user
                    RedirectLogin();
                }
           }
           else
           {
               RedirectLogin();
            }
        }
        #endregion

        #region unhandledException
        void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            logger.Error(e.ToString());
        }
        #endregion

        #region methods
        private void CheckApplicationRunning()
        {
            const string appName = "PDADesktop";
            bool createdNew;

            _mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                //app is already running! Exiting the application
                Application.Current.Shutdown();
            }
        }

        private int GenerandoAleatoriedadDeCasosLogueados()
        {
            Random rnd = new Random();
            int numRandom = rnd.Next(0, 2);
            logger.Debug("Numero aleatorio generado: " + numRandom);
            return numRandom;
        }

        async void UpdateApp()
        {
            string hostIpPort = ConfigurationManager.AppSettings.Get(Constants.PDAEXPRESS_SERVER_HOST);
            string urlOrPath = hostIpPort + ConfigurationManager.AppSettings.Get(Constants.URL_APPLICATION_UPDATE);
            try
            {
                using (updateManager = new UpdateManager(urlOrPath))
                {
                    logger.Info("buscando actualización");
                    UpdateInfo updateInfo = await updateManager.CheckForUpdate();
                    if (updateInfo.ReleasesToApply.Any())
                    {
                        var versionCount = updateInfo.ReleasesToApply.Count;
                        logger.Info($"{versionCount} update(s) found.");

                        var versionWord = versionCount > 1 ? "versions" : "version";
                        var message = new StringBuilder().AppendLine($"PDA Desktop App está {versionCount} {versionWord} por detrás de la última versión.").
                                                          AppendLine("Se descargará e instalará una actualización, los cambios surgirán efecto una vez el programa sea reiniciado.").
                                                          ToString();

                        var result = MessageBox.Show(message, "App Update", MessageBoxButton.OK);
                        logger.Info("Descargando actualizaciones");
                        var updateResult = await updateManager.UpdateApp();
                        logger.Info($"Descarga completa. Version {updateResult.Version} tomará efecto cuando la App sea reiniciada.");
                    }
                    else
                    {
                        logger.Info("No se detectaron actualizaciones");
                    }
                    logger.Info("actualización finalizada");
                }
            }
            catch (Exception e)
            {
                logger.Error("Error al buscar actualizaciones a " + urlOrPath);
                logger.Error(e.GetBaseException().ToString());
            }
        }
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            logger.Info("dispose update manager");
            updateManager.Dispose();
        }

        private string VerifySavedData()
        {
            return null;
        }

        private void RedirectToActivitiesCenter()
        {
            logger.Info("Redireccionando al centro de actividades...");
            var mainWindow = this.Container.GetInstance<MainWindow>();
            Uri uri = new Uri(Constants.CENTRO_ACTIVIDADES_VIEW, UriKind.Relative);
            mainWindow.frame.NavigationService.Navigate(uri);
            mainWindow.Show();
        }

        private void RedirectLogin()
        {
            logger.Info("No hay datos guardados con un usuario válido, redireccionando al login");
            var mainWindow = this.Container.GetInstance<MainWindow>();
            mainWindow.Show();
        }
        #endregion 
    }
}
