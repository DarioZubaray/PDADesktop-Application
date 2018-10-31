using log4net;
using MahApps.Metro.Controls.Dialogs;
using PDADesktop.Classes;
using PDADesktop.Classes.Utils;
using PDADesktop.Model;
using PDADesktop.Model.Dto;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace PDADesktop.ViewModel
{
    class ImprimirRecepcionViewModel : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Attributes
        private IDialogCoordinator dialogCoordinator;

        private readonly BackgroundWorker loadPrintReceptionWorker = new BackgroundWorker();

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

            loadPrintReceptionWorker.DoWork += loadPrintReceptionWorker_DoWork;
            loadPrintReceptionWorker.RunWorkerCompleted += loadPrintReceptionWorker_RunWorkerCompleted;

            PrintCommand = new RelayCommand(PrintMethod);
            loadPrintReceptionWorker.RunWorkerAsync();
        }
        #endregion

        #region Workers

        #region Load Print Reception
        private void loadPrintReceptionWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Debug("Load Print Reception -> Do Work");
            ListView listView = HttpWebClientUtil.GetGrillaRECEP();
            var dispatcher = App.Instance.Dispatcher;
            dispatcher.BeginInvoke(new Action(() =>
            {
                Receptions = ListViewUtils.ParserImprimirRecepcionDataGrid(listView);
            }));

        }
        private void loadPrintReceptionWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("Load Print Reception -> Run Work Completed");
        }
        #endregion

        #endregion

        #region Command Methods
        private void PrintMethod(object sender)
        {
            logger.Debug("Print Method");
            if(sender is Recepcion)
            {
                logger.Debug("Recepcion " + sender.ToString());
            }
        }
        #endregion
    }
}
