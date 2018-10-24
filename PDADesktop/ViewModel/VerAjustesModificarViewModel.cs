﻿using log4net;
using PDADesktop.Classes;
using PDADesktop.Model;
using PDADesktop.Classes.Utils;
using PDADesktop.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using PDADesktop.Model.Dto;

namespace PDADesktop.ViewModel
{
    class VerAjustesModificarViewModel : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Attributes
        private ObservableCollection<Ajustes> adjustments;
        public ObservableCollection<Ajustes> Adjustments
        {
            get
            {
                return adjustments;
            }
            set
            {
                adjustments = value;
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
                if (selectedAdjustment != null)
                {
                    AdjustmentTypeSelected = selectedAdjustment.motivo;
                    Textbox_amountValue = selectedAdjustment.cantidad.ToString();
                    AdjustmentEnableEdit = true;
                }
                else
                {
                    AdjustmentEnableEdit = false;
                }
                OnPropertyChanged();
            }
        }

        private bool adjustmentEnableEdit;
        public bool AdjustmentEnableEdit
        {
            get
            {
                return adjustmentEnableEdit;
            }
            set
            {
                adjustmentEnableEdit = value;
                OnPropertyChanged();
            }
        }

        private string adjustmentTypeSelected;
        public string AdjustmentTypeSelected
        {
            get
            {
                return adjustmentTypeSelected;
            }
            set
            {
                adjustmentTypeSelected = value;
                SelectedAdjustment.motivo = adjustmentTypeSelected;
                OnPropertyChanged();
            }
        }

        private List<string> adjustmentsTypes;
        public List<string> AdjustmentsTypes
        {
            get
            {
                return adjustmentsTypes;
            }
            set
            {
                adjustmentsTypes = value;
                OnPropertyChanged();
            }
        }

        private string textbox_amountValue;
        public string Textbox_amountValue
        {
            get
            {
                return textbox_amountValue;
            }
            set
            {
                textbox_amountValue = value;
                SelectedAdjustment.cantidad = Convert.ToInt64(textbox_amountValue);
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public VerAjustesModificarViewModel()
        {
            BannerApp.PrintSeeAdjustmentsModify();
            AjustesDTO ajustes = HttpWebClientUtil.LoadAdjustmentsGrid();
            adjustments = AjustesDTO.ParserDataGrid(ajustes);

            AdjustmentEnableEdit = false;

            DeleteAdjustmentCommand = new RelayCommand(DeleteAdjustmentMethod);
            UpdateAdjustmentCommand = new RelayCommand(UpdateAdjustmentMethod);
            DiscardChangesCommand = new RelayCommand(DiscardChangesMethod);
            SaveChangesCommand = new RelayCommand(SaveChangesMethod);
        }

        ~VerAjustesModificarViewModel()
        {
            MyAppProperties.SeeAdjustmentModify_syncId = 0L;
            MyAppProperties.SeeAdjustmentModify_batchId = null;
        }
        #endregion

        #region Commands
        private ICommand deleteAdjustmentCommand;
        public ICommand DeleteAdjustmentCommand
        {
            get
            {
                return deleteAdjustmentCommand;
            }
            set
            {
                deleteAdjustmentCommand = value;
            }
        }

        private ICommand updateAdjustmentCommand;
        public ICommand UpdateAdjustmentCommand
        {
            get
            {
                return updateAdjustmentCommand;
            }
            set
            {
                updateAdjustmentCommand = value;
            }
        }

        private ICommand discardChangesCommand;
        public ICommand DiscardChangesCommand
        {
            get
            {
                return discardChangesCommand;
            }
            set
            {
                discardChangesCommand = value;
            }
        }

        private ICommand saveChangesCommand;
        public ICommand SaveChangesCommand
        {
            get
            {
                return saveChangesCommand;
            }
            set
            {
                saveChangesCommand = value;
            }
        }
        #endregion

        #region Methods
        public void DeleteAdjustmentMethod(object obj)
        {
            logger.Debug("EliminarAjusteButton");
            string question = "¿Está seguro que desea eliminar el ajuste? Esta acción no se puede deshacer";
            if (AskToUser(question))
            {
                Ajustes parametro = obj as Ajustes;
                if (parametro != null)
                    logger.Debug("Parametro: " + parametro.ToString());
                if (SelectedAdjustment != null)
                    logger.Debug("AjusteSeleccionado : " + SelectedAdjustment.ToString());

                Adjustments.Remove(SelectedAdjustment);
                long adjustmentId = SelectedAdjustment.id;
                string batchId = MyAppProperties.SeeAdjustmentModify_batchId;
                long syncId = MyAppProperties.SeeAdjustmentModify_syncId;

                HttpWebClientUtil.DeleteAdjustment(adjustmentId, batchId, syncId);
                SelectedAdjustment = null;
            }
        }

        public void UpdateAdjustmentMethod(object obj)
        {
            logger.Debug("ActualizarAjusteButton");
            SelectedAdjustment = null;
        }

        public void DiscardChangesMethod(object obj)
        {
            logger.Debug("DescartarCambiosButton");
            string pregunta = "¿Desea descartar los cambios?";
            if (AskToUser(pregunta))
            {
                RedirectToActivityCenterView();
            }
            else
            {
                // revertir el estado del archivo?
                // cual es esta de la lista?
            }

        }
        public void SaveChangesMethod(object obj)
        {
            logger.Debug("GuardarCambiosButton");
            string newAdjustmentContent = TextUtils.ParseCollectionToAdjustmentDAT(Adjustments);
            logger.Debug("Nuevos ajustes: " + newAdjustmentContent);

            if (App.Instance.deviceHandler.OverWriteAdjustmentMade(newAdjustmentContent))
            {
                RedirectToActivityCenterView();
            }
            else
            {
                UserNotify("ERROR");
            }
        }

        private void RedirectToActivityCenterView()
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            Uri uri = new Uri(Constants.CENTRO_ACTIVIDADES_VIEW, UriKind.Relative);
            window.frame.NavigationService.Navigate(uri);
        }

        public void UserNotify(string message)
        {
            string caption = "Aviso!";
            MessageBoxButton messageBoxButton = MessageBoxButton.OK;
            MessageBoxImage messageBoxImage = MessageBoxImage.Error;
            MessageBoxResult result = MessageBox.Show(message, caption, messageBoxButton, messageBoxImage);
            if (result == MessageBoxResult.OK)
            {
                logger.Debug("Informando al usuario: " + message);
            }
        }

        public bool AskToUser(string question)
        {
            string message = question;
            string caption = "Aviso!";
            MessageBoxButton messageBoxButton = MessageBoxButton.OKCancel;
            MessageBoxImage messageBoxImage = MessageBoxImage.Question;
            MessageBoxResult result = MessageBox.Show(message, caption, messageBoxButton, messageBoxImage);
            logger.Debug("Preguntando al usuario: " + question);
            if (result == MessageBoxResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
