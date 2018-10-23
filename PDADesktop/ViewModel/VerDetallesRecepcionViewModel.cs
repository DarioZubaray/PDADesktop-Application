using log4net;
using PDADesktop.Classes;

namespace PDADesktop.ViewModel
{
    class VerDetallesRecepcionViewModel : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public VerDetallesRecepcionViewModel()
        {
            BannerApp.PrintSeeDetailsReception();
        }
    }
}
