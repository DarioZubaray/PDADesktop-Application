using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDADesktop.Classes
{
    class BannerApp
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void printBanner()
        {
            logger.Debug("\n\n");
            logger.Debug("==================================");
            logger.Debug("  IMAGO - PDA DESKTOP APPLICATION ");
            logger.Debug("==================================");
        }
    }
}
