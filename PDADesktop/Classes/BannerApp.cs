using log4net;

namespace PDADesktop.Classes
{
    class BannerApp
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void printBanner()
        {
            logger.Debug("==================================");
            logger.Debug("  IMAGO - PDA DESKTOP APPLICATION ");
            logger.Debug("==================================");
        }

        public static void printSynchronization()
        {
            logger.Debug("==================================");
            logger.Debug("=  S y n c h r o n i z a t i o n =");
            logger.Debug("==================================");
        }

        public static void printInformGX()
        {
            logger.Debug("==================================");
            logger.Debug("=  I n f o r m ==  G e n e s i x =");
            logger.Debug("==================================");
        }
    }
}
