using log4net;
using PDADesktop.Classes;
using PDADesktop.Classes.Utils;
using PDADesktop.Model;
using PDADesktop.Model.Dto;
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
        private readonly BackgroundWorker loadSearchBatches = new BackgroundWorker();

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

        private ListView listView;
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

        #region Constructor
        public BuscarLotesViewModel()
        {
            BannerApp.SearchBatches();
            DisplayWaitingPanel("Cargando...");
            PagerLegend = "Buscar Lotes View Model";
            PagerResultLegend = "Mostrando 1 - 10 de 10 resultados";
            loadSearchBatches.DoWork += loadSearchBatches_DoWork;
            loadSearchBatches.RunWorkerCompleted += loadSearchBatches_RunWorkerCompleted;
            ReturnCommand = new RelayCommand(ReturnActivityCenterMethod);
            AcceptCommand = new RelayCommand(AcceptMethod);
            PanelCloseCommand = new RelayCommand(PanelCloseMethod);

            FirstCommand = new RelayCommand(GoFirstPage);
            PreviousCommand = new RelayCommand(GoPreviousPage);
            NextCommand = new RelayCommand(GoNextPage);
            LastCommand = new RelayCommand(GoLastPage);

            LongListResultToDisplay = new long[] { 5, 10, 15, 20 };
            SelectedValueOne = 20;
            int initialPage = 1;
            loadSearchBatches.RunWorkerAsync(argument: initialPage);
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

        public void PanelCloseMethod(object obj)
        {
            HidingWaitingPanel();
        }

        public void GoFirstPage(object obj)
        {
            DisplayWaitingPanel("");
            logger.Debug("Pagina: " + this.listView.page);
            this.listView.page = 1;
            loadSearchBatches.RunWorkerAsync(argument: this.listView.page);
        }

        public void GoPreviousPage(object obj)
        {
            DisplayWaitingPanel("");
            logger.Debug("Pagina: " + this.listView.page);
            this.listView.page -= 1;
            loadSearchBatches.RunWorkerAsync(argument: this.listView.page);
        }

        public void GoNextPage(object obj)
        {
            DisplayWaitingPanel("");
            logger.Debug("Pagina: " + this.listView.page);
            this.listView.page += 1;
            loadSearchBatches.RunWorkerAsync(argument: this.listView.page);
        }

        public void GoLastPage(object obj)
        {
            DisplayWaitingPanel("");
            logger.Debug("Pagina: " + this.listView.page);
            this.listView.page = this.listView.total;
            loadSearchBatches.RunWorkerAsync(argument: this.listView.page);
        }

        #endregion

        #region Worker
        private void loadSearchBatches_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Debug("Load Search Batchs => Do Work");
            System.Threading.Thread.Sleep(1000);
            int page = (int)e.Argument;
            string responseSearchBatch = HttpWebClientUtil.SearchBatches(page, SelectedValueOne);
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

        private void loadSearchBatches_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
