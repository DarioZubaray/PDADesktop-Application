using log4net;
using MahApps.Metro.Controls.Dialogs;
using PDADesktop.Classes;
using PDADesktop.Classes.Utils;
using PDADesktop.Model;
using PDADesktop.Model.Dto;
using PDADesktop.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PDADesktop.ViewModel
{
    class BuscarLotesViewModel : ViewModelBase
    {
        #region Attributes
        #region Commons Attributes
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDialogCoordinator dialogCoordinator;

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

        private string pagerLegend;
        public string PagerLegend
        {
            get
            {
                return pagerLegend;
            }
            set
            {
                pagerLegend = value;
                OnPropertyChanged();
            }
        }

        private string pagerResultLegend;
        public string PagerResultLegend
        {
            get
            {
                return pagerResultLegend;
            }
            set
            {
                pagerResultLegend = value;
                OnPropertyChanged();
            }
        }

        private long[] longListResultToDisplay;
        public long[] LongListResultToDisplay
        {
            get
            {
                return longListResultToDisplay;
            }
            set
            {
                longListResultToDisplay = value;
                OnPropertyChanged();
            }
        }

        private long selectedValueOne;
        public long SelectedValueOne
        {
            get
            {
                return selectedValueOne;
            }
            set
            {
                selectedValueOne = value;
                OnPropertyChanged();
            }
        }

        private bool firstButtonEnabled;
        public bool FirstButtonEnabled
        {
            get
            {
                return firstButtonEnabled;
            }
            set
            {
                firstButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool previousButtonEnabled;
        public bool PreviousButtonEnabled
        {
            get
            {
                return previousButtonEnabled;
            }
            set
            {
                previousButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool nextButtonEnabled;
        public bool NextButtonEnabled
        {
            get
            {
                return nextButtonEnabled;
            }
            set
            {
                nextButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool lastButtonEnabled;
        public bool LastButtonEnabled
        {
            get
            {
                return lastButtonEnabled;
            }
            set
            {
                lastButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        private Model.Dto.ListView listView;
        #endregion

        #region Workers Attributes
        private readonly BackgroundWorker loadSearchBatchesWorker = new BackgroundWorker();
        #endregion

        #region Loading Panel Attributes
        private bool panelLoading;
        public bool PanelLoading
        {
            get
            {
                return panelLoading;
            }
            set
            {
                panelLoading = value;
                OnPropertyChanged();
            }
        }
        private string panelMainMessage;
        public string PanelMainMessage
        {
            get
            {
                return panelMainMessage;
            }
            set
            {
                panelMainMessage = value;
                OnPropertyChanged();
            }
        }
        private string panelSubMessage;
        public string PanelSubMessage
        {
            get
            {
                return panelSubMessage;
            }
            set
            {
                panelSubMessage = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands Attributes
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

        private ICommand panelCloseCommand;
        public ICommand PanelCloseCommand
        {
            get
            {
                return panelCloseCommand;
            }
            set
            {
                panelCloseCommand = value;
            }
        }

        private ICommand firstCommand;
        public ICommand FirstCommand
        {
            get
            {
                return firstCommand;
            }
            set
            {
                firstCommand = value;
            }
        }

        private ICommand previousCommand;
        public ICommand PreviousCommand
        {
            get
            {
                return previousCommand;
            }
            set
            {
                previousCommand = value;
            }
        }

        private ICommand nextCommand;
        public ICommand NextCommand
        {
            get
            {
                return nextCommand;
            }
            set
            {
                nextCommand = value;
            }
        }

        private ICommand lastCommand;
        public ICommand LastCommand
        {
            get
            {
                return lastCommand;
            }
            set
            {
                lastCommand = value;
            }
        }
        #endregion
        #endregion

        #region Constructor
        public BuscarLotesViewModel(IDialogCoordinator instance)
        {
            BannerApp.SearchBatches();
            DisplayWaitingPanel("Cargando...");
            dialogCoordinator = instance;
            PagerLegend = "Buscar Lotes View Model";
            PagerResultLegend = "Mostrando 1 - 10 de 10 resultados";

            loadSearchBatchesWorker.DoWork += loadSearchBatchesWorker_DoWork;
            loadSearchBatchesWorker.RunWorkerCompleted += loadSearchBatchesWorker_RunWorkerCompleted;

            ReturnCommand = new RelayCommand(ReturnActivityCenterAction);
            AcceptCommand = new RelayCommand(AcceptAction);
            PanelCloseCommand = new RelayCommand(PanelCloseAction);

            FirstButtonEnabled = false;
            PreviousButtonEnabled = false;
            NextButtonEnabled = true;
            LastButtonEnabled = true;

            FirstCommand = new RelayCommand(GoFirstPageAction);
            PreviousCommand = new RelayCommand(GoPreviousPageAction);
            NextCommand = new RelayCommand(GoNextPageAction);
            LastCommand = new RelayCommand(GoLastPageAction);

            LongListResultToDisplay = new long[] { 5, 10, 15, 20 };
            SelectedValueOne = 20;
            int initialPage = 1;
            loadSearchBatchesWorker.RunWorkerAsync(argument: initialPage);
        }
        #endregion

        #region Action Methods
        public void ReturnActivityCenterAction(object obj)
        {
            MyAppProperties.currentBatchId = obj?.ToString();
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            Uri uri = new Uri(Constants.CENTRO_ACTIVIDADES_VIEW, UriKind.Relative);
            window.frame.NavigationService.Navigate(uri);
        }

        public void AcceptAction(object obj)
        {
            logger.Debug("Accept Method");
            if(this.SelectedBatch != null)
            {
                logger.Debug("SelectedBatch: " + this.SelectedBatch.idLote);
                ReturnActivityCenterAction(this.SelectedBatch.idLote);
            }
        }

        public void PanelCloseAction(object obj)
        {
            HidingWaitingPanel();
        }
        #endregion

        #region Paginator Methods
        public void GoFirstPageAction(object obj)
        {
            DisplayWaitingPanel("");
            this.listView.page = 1;
            FirstButtonEnabled = false;
            PreviousButtonEnabled = false;
            NextButtonEnabled = true;
            LastButtonEnabled = true;
            loadSearchBatchesWorker.RunWorkerAsync(argument: this.listView.page);
        }

        public void GoPreviousPageAction(object obj)
        {
            DisplayWaitingPanel("");
            if(this.listView.page == 2)
            {
                this.listView.page = 1;
                FirstButtonEnabled = false;
                PreviousButtonEnabled = false;
            }
            else
            {
                this.listView.page -= 1;
                FirstButtonEnabled = true;
                PreviousButtonEnabled = true;
            }
            NextButtonEnabled = true;
            LastButtonEnabled = true;
            loadSearchBatchesWorker.RunWorkerAsync(argument: this.listView.page);
        }

        public void GoNextPageAction(object obj)
        {
            DisplayWaitingPanel("");
            if(this.listView.page == this.listView.total - 1)
            {
                this.listView.page = this.listView.total;
                NextButtonEnabled = false;
                LastButtonEnabled = false;
            }
            else
            {
                this.listView.page += 1;
                NextButtonEnabled = true;
                LastButtonEnabled = true;
            }
            FirstButtonEnabled = true;
            PreviousButtonEnabled = true;
            loadSearchBatchesWorker.RunWorkerAsync(argument: this.listView.page);
        }

        public void GoLastPageAction(object obj)
        {
            DisplayWaitingPanel("");
            this.listView.page = this.listView.total;
            NextButtonEnabled = false;
            LastButtonEnabled = false;
            FirstButtonEnabled = true;
            PreviousButtonEnabled = true;
            loadSearchBatchesWorker.RunWorkerAsync(argument: this.listView.page);
        }
        #endregion

        #region Workers Methods
        #region Load Search Batches Worker
        private void loadSearchBatchesWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Debug("load search batches => Do Work");
            System.Threading.Thread.Sleep(1000);
            int page = (int)e.Argument;
            string storeId = MyAppProperties.storeId;
            string responseSearchBatch = HttpWebClientUtil.SearchBatches(storeId, page, SelectedValueOne);
            listView = JsonUtils.GetListView(responseSearchBatch);
            var dispatcher = App.Instance.Dispatcher;
            dispatcher.BeginInvoke(new Action(() =>
            {
                SearchBatch = ListViewUtils.ParserSearchBatchesDataGrid(listView);
                PagerLegend = "Página " + listView.page + " de " + listView.total;
                long inicio = (listView.page * SelectedValueOne) - (SelectedValueOne - 1);
                long fin = inicio + (SelectedValueOne - 1);
                int totalRecords = listView.records;
                PagerResultLegend = String.Format("Mostrando {0} - {1} de {2} resultados.", inicio, fin, totalRecords);
            }));
        }

        private void loadSearchBatchesWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("load search batches => Run Worker Completed");
            var dispatcher = App.Instance.Dispatcher;
            dispatcher.BeginInvoke(new Action(() =>
            {
                HidingWaitingPanel();
            }));
        }
        #endregion
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
