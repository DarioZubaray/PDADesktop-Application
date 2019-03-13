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
                         + "=  A C T I V I T Y  C E N T E R  ="
                         + "==================================");
        }

        public static void PrintSynchronization()
        {
            logger.Debug("\n=================================="
                          +"=  S Y N C H R O N I Z A T I O N ="
                          +"==================================");
        }

        public static void PrintInformGX()
        {
            logger.Debug("\n=================================="
                         +"=  I N F O R M  TO  G E N E S I X ="
                         +"==================================");
        }

        public static void PrintSeeActivities()
        {
            logger.Debug("\n=================================="
                         + "=  VER  ACTIVIDADES  REALIZADOS  ="
                         + "==================================");
        }

        public static void PrintSeeAdjustmentsRealized()
        {
            logger.Debug("\n=================================="
                         + "=   VER   AJUSTES   REALIZADOS   ="
                         + "==================================");
        }

        public static void PrintSeeAdjustmentsInformed()
        {
            logger.Debug("\n=================================="
                         + "=   VER   AJUSTES   INFORMADOS   ="
                         + "==================================");
        }

        public static void PrintSeeAdjustmentsModify()
        {
            logger.Debug("\n=================================="
                         + "=   VER   AJUSTES   MODIFICAR   ="
                         + "==================================");
        }

        public static void PrintPrintReception()
        {
            logger.Debug("\n=================================="
                         + "=    IMPRIMIR    RECEPCIONES    ="
                         + "==================================");
        }

        public static void PrintSeeDetailsReception()
        {
            logger.Debug("\n=================================="
                         + "=    VER DETALLES RECEPCIONES    ="
                         + "==================================");
        }

        public static void SearchBatches()
        {
            logger.Debug("\n=================================="
                         + "=    B U S C A R   L O T E S    ="
                         + "==================================");
        }
    }
}
