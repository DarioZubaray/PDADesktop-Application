using log4net;
using MahApps.Metro.Controls.Dialogs;
using PDADesktop.Classes;
using PDADesktop.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PDADesktop.ViewModel
{
    class ImprimirRecepcionViewModel : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Attributes
        private IDialogCoordinator dialogCoordinator;

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
        #endregion

        #region Command
        private ICommand printCommand;
        public ICommand PrintCommand
        {
            get
            {
                return printCommand;
            }
            set
            {
                printCommand = value;
            }
        }
        #endregion

        #region Constructor
        public ImprimirRecepcionViewModel(IDialogCoordinator instance)
        {
            BannerApp.PrintPrintReception();
            dialogCoordinator = instance;

            PrintCommand = new RelayCommand(PrintMethod);
        }
        #endregion

        #region Command Methods
        private void PrintMethod(object sender)
        {
            logger.Debug("Print Method");
        }
        #endregion
    }
}
