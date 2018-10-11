using log4net;

namespace PDADesktop.Classes
{
    class BannerApp
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void PrintAppBanner()
        {
            logger.Debug("\n=================================="
                        +"\n IMAGO - PDA DESKTOP APPLICATION  "
                        +"\n==================================");
        }

        public static void PrintLogin()
        {
            logger.Debug("\n=================================="
                         + "= L O G I N  P D A  D E S K T O P="
                         + "==================================");
        }

        public static void PrintActivityCenter()
        {
            logger.Debug("\n=================================="
                         + "=  A c t i v i t y  C e n t e r  ="
                         + "==================================");
        }

        public static void PrintSynchronization()
        {
            logger.Debug("\n=================================="
                          +"=  S y n c h r o n i z a t i o n ="
                          +"==================================");
        }

        public static void PrintInformGX()
        {
            logger.Debug("\n=================================="
                         +"=  I n f o r m  to  G e n e s i x ="
                         +"==================================");
        }

        public static void PrintSeeAdjustments()
        {
            logger.Debug("\n=================================="
                         + "=      V E R   A J U S T E S     ="
                         + "==================================");
        }
    }
}
