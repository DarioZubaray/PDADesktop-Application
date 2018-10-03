using System;
using System.Net;

namespace PDADesktop.Classes.Utils
{
    public class PDAWebClient : WebClient
    {
        #region time in milliseconds
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
        #endregion

        public const string DEFAULT_USER_AGENT = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";

        #region Constructor
        public PDAWebClient()
        {
            this.timeout = 60000;
            this.Headers.Add("user-agent", DEFAULT_USER_AGENT);
        }

        public PDAWebClient(int timeout)
        {
            this.Timeout = timeout;
            this.Headers.Add("user-agent", DEFAULT_USER_AGENT);
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
    }
}
