﻿using log4net;
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

namespace PDADesktop.ViewModel
{
    class VerDetallesRecepcionViewModel : ViewModelBase
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

        private Recepcion selectedReception;
        public Recepcion SelectedReception
        {
            get
            {
                return selectedReception;
            }
            set
            {
                selectedReception = value;
                if (selectedReception != null)
                {
                    ReceptionEnableEdit = true;
                }
                else
                {
                    ReceptionEnableEdit = false;
                }
                OnPropertyChanged();
            }
        }

        private bool receptionEnableEdit;
        public bool ReceptionEnableEdit
        {
            get
            {
                return receptionEnableEdit;
            }
            set
            {
                receptionEnableEdit = value;
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

        #region Worker
        private readonly BackgroundWorker loadSeeDetailsWorker = new BackgroundWorker();
        #endregion

        #region Commands
        private ICommand discardAllCommand;
        public ICommand DiscardAllCommand
        {
            get
            {
                return discardAllCommand;
            }
            set
            {
                discardAllCommand = value;
            }
        }
        private ICommand cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return cancelCommand;
            }
            set
            {
                cancelCommand = value;
            }
        }
        private ICommand retryCommand;
        private ICommand RetryCommand
        {
            get
            {
                return retryCommand;
            }
            set
            {
                retryCommand = value;
            }
        }
        #endregion

        #region Constructor
        public VerDetallesRecepcionViewModel(IDialogCoordinator instance)
        {
            BannerApp.PrintSeeDetailsReception();
            dialogCoordinator = instance;
            DisplayWaitingPanel("Cargando...");
            ReceptionEnableEdit = false;

            loadSeeDetailsWorker.DoWork += loadSeeDetailsWorker_DoWork;
            loadSeeDetailsWorker.RunWorkerCompleted += loadSeeDetailsWorker_RunWorkerCompleted;

            DiscardAllCommand = new RelayCommand(DiscardAllMethod);
            CancelCommand = new RelayCommand(CancelMethod);
            RetryCommand = new RelayCommand(RetryMethod);

            loadSeeDetailsWorker.RunWorkerAsync();
        }
        #endregion

        #region Workers

        #region Load See Details Worker
        private void loadSeeDetailsWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            logger.Debug("Load See Details Receptions -> Do Work");
            ListView listView = HttpWebClientUtil.LoadReceptionsGrid();
            Receptions = ListView.ParserRecepcionDataGrid(listView);
        }
        private void loadSeeDetailsWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logger.Debug("Load See Details Receptions -> Run Work Completed");
        }
        #endregion

        #endregion

        #region Command Methods
        private void DiscardAllMethod(object sender)
        {

        }
        private void CancelMethod(object sender)
        {
            RedirectToActivityCenterView();
        }
        private void RetryMethod(object sender)
        {

        }
        #endregion

        #region Methods
        private void RedirectToActivityCenterView()
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

        public void CerrarPanel(object obj)
        {
            PanelLoading = false;
        }
        #endregion
    }
}
