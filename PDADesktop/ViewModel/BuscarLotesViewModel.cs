using log4net;
using PDADesktop.Classes;
using PDADesktop.Model;
using PDADesktop.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace PDADesktop.ViewModel
{
    class BuscarLotesViewModel : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Attributes
        private readonly BackgroundWorker loadSearchBatch = new BackgroundWorker();

        private ObservableCollection<Lote> searchBatch;
        public ObservableCollection<Lote> SearchBatch
        {
            get
            {
                return searchBatch;
            }
            set
            {
                searchBatch = value;
                OnPropertyChanged();
            }
        }

        private Lote selectedBatch;
        public Lote SelectedBatch
        {
            get
            {
                return selectedBatch;
            }
            set
            {
                selectedBatch = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Loading panel
        private bool _panelLoading;
        public bool PanelLoading
        {
            get
            {
                return _panelLoading;
            }
            set
            {
                _panelLoading = value;
                OnPropertyChanged();
            }
        }
        private string _panelMainMessage;
        public string PanelMainMessage
        {
            get
            {
                return _panelMainMessage;
            }
            set
            {
                _panelMainMessage = value;
                OnPropertyChanged();
            }
        }
        private string _panelSubMessage;
        public string PanelSubMessage
        {
            get
            {
                return _panelSubMessage;
            }
            set
            {
                _panelSubMessage = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        private ICommand returnCommand;
        public ICommand ReturnCommand
        {
            get
            {
                return returnCommand;
            }
            set
            {
                returnCommand = value;
            }
        }

        private ICommand acceptCommand;
        public ICommand AcceptCommand
        {
            get
            {
                return acceptCommand;
            }
            set
            {
                acceptCommand = value;
            }
        }
        #endregion

        #region Constructor
        public BuscarLotesViewModel()
        {
            DisplayWaitingPanel("Cargando...");
            loadSearchBatch.DoWork += loadloadSearchBatch_DoWork;
            loadSearchBatch.RunWorkerCompleted += loadloadSearchBatch_RunWorkerCompleted;
            ReturnCommand = new RelayCommand(ReturnActivityCenterMethod);
            AcceptCommand = new RelayCommand(AcceptMethod);

            loadSearchBatch.RunWorkerAsync();
        }
        #endregion

        #region RelayCommand Methods
        public void ReturnActivityCenterMethod(object obj)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            Uri uri = new Uri(Constants.CENTRO_ACTIVIDADES_VIEW, UriKind.Relative);
            window.frame.NavigationService.Navigate(uri);
        }

        public void AcceptMethod(object obj)
        {
            logger.Debug("Accept Method");
        }
        #endregion

        #region Worker
        private void loadloadSearchBatch_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Debug("Load Search Batchs => Do Work");
            SearchBatch = new ObservableCollection<Lote>();
            Lote l1 = new Lote(1, DateTime.Now, 706);
            Lote l2 = new Lote(2, DateTime.Now, 706);
            SearchBatch.Add(l1);
            SearchBatch.Add(l2);
        }

        private void loadloadSearchBatch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("Load Search Batchs => Run Worker Completed");
            var dispatcher = App.Instance.Dispatcher;
            dispatcher.BeginInvoke(new Action(() =>
            {
                HidingWaitingPanel();
            }));
        }
        #endregion


        #region Panel Methods
        public void DisplayWaitingPanel(string mainMessage, string subMessage = "")
        {
            PanelLoading = true;
            PanelMainMessage = mainMessage;
            PanelSubMessage = subMessage;
        }
        public void HidingWaitingPanel()
        {
            PanelLoading = false;
            PanelMainMessage = "";
            PanelSubMessage = "";
        }
        #endregion
    }
}
