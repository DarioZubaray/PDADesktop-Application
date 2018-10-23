using log4net;
using PDADesktop.Classes;
using PDADesktop.Model;
using System.Collections.ObjectModel;

namespace PDADesktop.ViewModel
{
    class ImprimirRecepcionViewModel : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ObservableCollection<Recepcion> receptions;
        public ObservableCollection<Recepcion> Receptions
        {
            get
            {
                return receptions;
            }
            set
            {
                receptions = value;
                OnPropertyChanged();
            }
        }

        #region Constructor
        public ImprimirRecepcionViewModel()
        {
            BannerApp.PrintPrintReception();
        }
        #endregion
    }
}
