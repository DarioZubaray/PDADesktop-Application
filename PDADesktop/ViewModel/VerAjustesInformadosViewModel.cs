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
using System.Windows.Threading;

namespace PDADesktop.ViewModel
{
    class VerAjustesInformadosViewModel : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Attributes
        private readonly BackgroundWorker loadAdjustmentInformed = new BackgroundWorker();

        private ObservableCollection<Ajustes> adjustmentsInformed;
        public ObservableCollection<Ajustes> AdjustmentsInformed
        {
            get
            {
                return adjustmentsInformed;
            }
            set
            {
                adjustmentsInformed = value;
                OnPropertyChanged();
            }
        }

        private Ajustes selectedAdjustment;
        public Ajustes SelectedAdjustment
        {
            get
            {
                return selectedAdjustment;
            }
            set
            {
                selectedAdjustment = value;
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

        
        #endregion

        #region Constructor
        public VerAjustesInformadosViewModel()
        {
            BannerApp.PrintSeeAdjustmentsInformed();
            DisplayWaitingPanel("Cargando...");
            var dispatcher = App.Instance.MainWindow.Dispatcher;

            loadAdjustmentInformed.DoWork += loadAdjustmentInformed_DoWork;
            loadAdjustmentInformed.RunWorkerCompleted += loadAdjustmentInformed_RunWorkerCompleted;
            ReturnCommand = new RelayCommand(ReturnActivitycenterMethod);

            loadAdjustmentInformed.RunWorkerAsync();
        }
        #endregion

        #region Worker
        private void loadAdjustmentInformed_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Debug("Load adjustments informed => Do Work");
            string batchId = MyAppProperties.buttonState_batchId;
            ListView responseGetAdjustments = HttpWebClientUtil.GetAdjustmentsByBatchId(batchId);

            var dispatcher = App.Instance.Dispatcher;
            dispatcher.BeginInvoke(new Action(() =>
           {
               AdjustmentsInformed = ListViewUtils.ParserAjustesInformadosDataGrid(responseGetAdjustments);
           }));
        }

        private void loadAdjustmentInformed_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("Load adjustments informed => Run Worker Completed");
            var dispatcher = App.Instance.Dispatcher;
            dispatcher.BeginInvoke(new Action(() =>
          {
              HidingWaitingPanel();
          }));
        }
        #endregion

        #region RelayCommand Methods
        public void ReturnActivitycenterMethod(object obj)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            Uri uri = new Uri(Constants.CENTRO_ACTIVIDADES_VIEW, UriKind.Relative);
            window.frame.NavigationService.Navigate(uri);
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
