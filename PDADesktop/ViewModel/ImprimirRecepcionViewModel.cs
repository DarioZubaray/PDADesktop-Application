using log4net;
using MahApps.Metro.Controls.Dialogs;
using PDADesktop.Classes;
using PDADesktop.Classes.Utils;
using PDADesktop.Model.Dto;
using PDADesktop.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace PDADesktop.ViewModel
{
    class ImprimirRecepcionViewModel : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Attributes
        private IDialogCoordinator dialogCoordinator;

        private readonly BackgroundWorker loadPrintReceptionWorker = new BackgroundWorker();
        private readonly BackgroundWorker printerReceptionsWorker = new BackgroundWorker();

        private ObservableCollection<ImprimirRecepcionesDtoDataGrid> printReceptions;
        public ObservableCollection<ImprimirRecepcionesDtoDataGrid> PrintReceptions
        {
            get
            {
                return printReceptions;
            }
            set
            {
                printReceptions = value;
                OnPropertyChanged();
            }
        }

        private ImprimirRecepcionesDtoDataGrid selectedReception;
        public ImprimirRecepcionesDtoDataGrid SelectedReception
        {
            get
            {
                return selectedReception;
            }
            set
            {
                selectedReception = value;
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
        #endregion

        #region Constructor
        public ImprimirRecepcionViewModel(IDialogCoordinator instance)
        {
            DisplayWaitingPanel("Espere por favor");
            BannerApp.PrintPrintReception();
            dialogCoordinator = instance;

            loadPrintReceptionWorker.DoWork += loadPrintReceptionWorker_DoWork;
            loadPrintReceptionWorker.RunWorkerCompleted += loadPrintReceptionWorker_RunWorkerCompleted;

            printerReceptionsWorker.DoWork += printerReceptionsWorker_DoWork;
            printerReceptionsWorker.RunWorkerCompleted += printerReceptionsWorker_RunWorkerCompleted;

            ReturnCommand = new RelayCommand(ReturnMethod);
            PanelCloseCommand = new RelayCommand(PanelCloseMethod);

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
                PrintReceptions = ListViewUtils.ParserImprimirRecepcionDataGrid(listView);
                AddPrintCommand(PrintReceptions);
                HidingWaitingPanel();
            }));

        }
        private void loadPrintReceptionWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("Load Print Reception -> Run Work Completed");
        }
        #endregion

        #region Printer Reception Worker
        private void printerReceptionsWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Debug("Printer Receptions => Do Worker");
            logger.Debug(this.SelectedReception);

            logger.Debug("Numero de recepcion: " + SelectedReception.numeroRecepcion);
            logger.Debug("Codigo de Tienda: " + MyAppProperties.storeId);

            string temp = HttpWebClientUtil.PrintReception(SelectedReception.numeroRecepcion, MyAppProperties.storeId);

            try
            {
                if(FileUtils.VerifyIfExitsFile(temp))
                {
                    System.Diagnostics.Process.Start(temp);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error al abrir el archivo temporal: " + ex.Message);
            }
            //borrar los temporales (?)
        }

        private void printerReceptionsWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("Printer Receptions => Run Work Completed");
            var dispatcher = App.Instance.Dispatcher;
            dispatcher.BeginInvoke(new Action(() => {
                HidingWaitingPanel();
            }));
        }
        #endregion
        #endregion

        #region Command Methods
        private void PrintMethod(object sender)
        {
            logger.Debug("Print Method");
            DisplayWaitingPanel("Imprimiendo...");

            printerReceptionsWorker.RunWorkerAsync();
        }

        private void ReturnMethod(object sender)
        {
            DisplayWaitingPanel("Volviendo...");
            logger.Debug("return Method");
            RedirectToActivityCenterView();
            HidingWaitingPanel();
        }

        private void PanelCloseMethod(object sender)
        {
            HidingWaitingPanel();
        }
        #endregion

       #region Methods
        public void AddPrintCommand(ObservableCollection<ImprimirRecepcionesDtoDataGrid> imprimirRecepciones)
        {
            foreach(ImprimirRecepcionesDtoDataGrid imprimirRecepcion in imprimirRecepciones)
            {
                imprimirRecepcion.PrintCommand = new RelayCommand(PrintMethod, param => true);
            }
        }

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

        private void RedirectToActivityCenterView()
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            Uri uri = new Uri(Constants.CENTRO_ACTIVIDADES_VIEW, UriKind.Relative);
            window.frame.NavigationService.Navigate(uri);
        }

        #endregion
    }
}
