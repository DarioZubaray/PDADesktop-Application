using log4net;
using System;
using System.Net;

namespace PDADesktop.Classes.Utils
{
    public class PDAWebClient : WebClient
    {
        #region Attributes
        private int timeout;
        public int Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
            }
        }
        private string url;
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
            }
        }
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        public const string DEFAULT_USER_AGENT = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";

        #region Constructor
        public PDAWebClient()
        {
            this.timeout = 60000;
            this.Headers.Add("user-agent", DEFAULT_USER_AGENT);
        }

        public PDAWebClient(int timeout, string url)
        {
            this.Timeout = timeout;
            this.Headers.Add("user-agent", DEFAULT_USER_AGENT);
            this.Url = url;
            this.DownloadStringCompleted += new DownloadStringCompletedEventHandler(pdaWebClient_DownloadStringCompleted);
            this.DownloadStringAsync(new Uri(this.Url));
        }

        public PDAWebClient(string userAgent)
        {
            this.timeout = 60000;
            this.Headers.Add("user-agent", userAgent);
        }
        #endregion

        protected override WebRequest GetWebRequest(Uri address)
        {
            var result = base.GetWebRequest(address);
            result.Timeout = this.timeout;
            return result;
        }

        private int _retryCount = 0;
        public void pdaWebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            logger.Debug("PDA Web Client => DownloadStringCompleted");
            if (e.Error != null)
            {
                _retryCount++;
                logger.Info("Reintento numero: " + _retryCount);
                if (_retryCount < 3)
                {
                    WebClient pdaWebClient = (WebClient)sender;
                    pdaWebClient.DownloadStringAsync(new Uri(this.Url));
                }
                else
                {
                    _retryCount = 0;
                    logger.Error(e.Error.Message);
                }
            }
            else
            {
                _retryCount = 0;
                logger.Info("PDA Web Client successfull.");
            }
        }
    }
}

