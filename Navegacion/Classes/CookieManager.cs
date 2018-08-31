using log4net;
using System;
using System.Configuration;
using System.Windows;

namespace Navegacion.Classes
{
    class CookieManager
    {
        #region atributes
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string username;
        public string UserName
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }

        public enum Cookie { usuario, contraseña, ultimoAcceso };
        #endregion

        #region Methods
        public static string getCookie(Cookie galletita)
        {
            string cookie2Return = null;

            Uri uriCookie = getUriCookie(galletita);
            try
            {
                cookie2Return = Application.GetCookie(uriCookie);
            }
            catch (Exception e)
            {
                logger.Error(e.GetBaseException().ToString());
            }
            return parseCookie2String(cookie2Return);
        }

        public static void setCookie(Cookie galletita, string valor)
        {
            Uri uriCookie = getUriCookie(galletita);
            Application.SetCookie(uriCookie, valor);
        }

        private static Uri getUriCookie(Cookie galletita)
        {
            string baseUrlProperties = ConfigurationManager.AppSettings.Get("URL_COOKIE");
            string baseUrlExpanded = Environment.ExpandEnvironmentVariables(baseUrlProperties);
            string completeUrl = null;
            switch (galletita)
            {
                case Cookie.usuario:
                    completeUrl = baseUrlExpanded + "usuario";
                    break;
                case Cookie.contraseña:
                    completeUrl = baseUrlExpanded + "contraseña";
                    break;
                case Cookie.ultimoAcceso:
                    completeUrl = baseUrlExpanded + "ultimoAcceso";
                    break;
                default:
                    return null;
            }
            logger.Info("url cookie: " + completeUrl);
            return new Uri(completeUrl);
        }

        private static string parseCookie2String(string galletita)
        {
            if (galletita != null)
            {
                string llave = galletita.Substring(0, galletita.IndexOf("="));
                string valor = galletita.Substring(galletita.IndexOf("=") + 1);
                logger.Info("nombre obtenido: " + llave + " -> " + valor);
                return valor;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
