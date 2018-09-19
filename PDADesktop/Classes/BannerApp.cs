using log4net;

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
