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
using System.Windows.Input;
using System.Windows.Threading;

namespace PDADesktop.ViewModel
{
    class VerAjustesInformadosViewModel : ViewModelBase
    {
        #region Attributes
        #region Commons Attributes
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDialogCoordinator dialogCoordinator;

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

        #region Dispatcher Attributes
        private Dispatcher dispatcher;
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
        private ICommand verAjustesInformadosLoadedEvent;
        public ICommand VerAjustesInformadosLoadedEvent
        {
            get
            {
                return verAjustesInformadosLoadedEvent;
            }
            set
            {
                verAjustesInformadosLoadedEvent = value;
            }
        }

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
        #endregion

        #region Constructor
        public VerAjustesInformadosViewModel(IDialogCoordinator instance)
        {
            BannerApp.PrintSeeAdjustmentsInformed();
            DisplayWaitingPanel("Cargando...");
            dialogCoordinator = instance;
            dispatcher = App.Instance.Dispatcher;
            VerAjustesInformadosLoadedEvent = new RelayCommand(VerAjustesInformadosLoadedEventAction);
            ReturnCommand = new RelayCommand(ReturnActivityCenterAction);
        }
        #endregion

        #region Event Method
        public void VerAjustesInformadosLoadedEventAction(object sender)
        {
            logger.Debug("Ver ajustes informados => Loaded Event");
            string batchId = MyAppProperties.buttonState_batchId;
            ListView responseGetAdjustments = HttpWebClientUtil.GetAdjustmentsByBatchId(batchId);

            dispatcher.BeginInvoke(new Action(() =>
            {
                AdjustmentsInformed = ListViewUtils.ParserAjustesInformadosDataGrid(responseGetAdjustments);
                HidingWaitingPanel();
            }));
        }
        #endregion

        #region Action Methods
        public void ReturnActivityCenterAction(object obj)
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
