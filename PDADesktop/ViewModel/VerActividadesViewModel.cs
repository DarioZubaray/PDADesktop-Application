using log4net;
using PDADesktop.Classes;
using PDADesktop.Classes.Devices;
using PDADesktop.Classes.Utils;
using PDADesktop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Windows.Input;

namespace PDADesktop.ViewModel
{
    class VerActividadesViewModel : ViewModelBase
    {
        #region Attributes
        #region Commons Attributes
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDeviceHandler deviceHandler { get; set; }
        #endregion
        #region selectedRow
        public ControlPrecio PriceControlConfirmedSelected { get; set; }
        public ControlPrecio PendingPriceControlSelected { get; set; }
        public Ajustes AdjustmentConfirmedSelected { get; set; }
        public Ajustes PendingAdjustmentSelected { get; set; }
        public Recepcion ReceptionConfirmedSelected { get; set; }
        public Recepcion PendingReceptionSelected { get; set; }
        public Etiqueta LabelConfirmedSelected { get; set; }
        public Etiqueta PendingLabelSelected { get; set; }
        #endregion
        #region List
        private ObservableCollection<ControlPrecio> controlPreciosConfirmados;
        public ObservableCollection<ControlPrecio> ControlPreciosConfirmados
        {
            get
            {
                return controlPreciosConfirmados;
            }
            set
            {
                controlPreciosConfirmados = value;
            }
        }
        private ObservableCollection<ControlPrecio> controlPreciosPendientes;
        public ObservableCollection<ControlPrecio> ControlPreciosPendientes
        {
            get
            {
                return controlPreciosPendientes;
            }
            set
            {
                controlPreciosPendientes = value;
            }
        }

        private ObservableCollection<Ajustes> ajustesConfirmados;
        public ObservableCollection<Ajustes> AjustesConfirmados
        {
            get
            {
                return ajustesConfirmados;
            }
            set
            {
                ajustesConfirmados = value;
            }
        }
        private ObservableCollection<Ajustes> ajustesPendientes;
        public ObservableCollection<Ajustes> AjustesPendientes
        {
            get
            {
                return ajustesPendientes;
            }
            set
            {
                ajustesPendientes = value;
            }
        }

        private ObservableCollection<ArticuloRecepcion> recepcionesConfirmadas;
        public ObservableCollection<ArticuloRecepcion> RecepcionesConfirmadas
        {
            get
            {
                return recepcionesConfirmadas;
            }
            set
            {
                recepcionesConfirmadas = value;
            }
        }
        private ObservableCollection<ArticuloRecepcion> recepcionesPendientes;
        public ObservableCollection<ArticuloRecepcion> RecepcionesPendientes
        {
            get
            {
                return recepcionesPendientes;
            }
            set
            {
                recepcionesPendientes = value;
            }
        }

        private ObservableCollection<Etiqueta> etiquetasConfirmadas;
        public ObservableCollection<Etiqueta> EtiquetasConfirmadas
        {
            get
            {
                return etiquetasConfirmadas;
            }
            set
            {
                etiquetasConfirmadas = value;
            }
        }
        private ObservableCollection<Etiqueta> etiquetasPendientes;
        public ObservableCollection<Etiqueta> EtiquetasPendientes
        {
            get
            {
                return etiquetasPendientes;
            }
            set
            {
                etiquetasPendientes = value;
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

        private ICommand removeAllPriceControlCommand;
        public ICommand RemoveAllPriceControlCommand
        {
            get
            {
                return removeAllPriceControlCommand;
            }
            set
            {
                removeAllPriceControlCommand = value;
            }
        }
        private ICommand removeOnePriceControlCommand;
        public ICommand RemoveOnePriceControlCommand
        {
            get
            {
                return removeOnePriceControlCommand;
            }
            set
            {
                removeOnePriceControlCommand = value;
            }
        }
        private ICommand addOnePriceControlCommand;
        public ICommand AddOnePriceControlCommand
        {
            get
            {
                return addOnePriceControlCommand;
            }
            set
            {
                addOnePriceControlCommand = value;
            }
        }
        private ICommand addAllPriceControlCommand;
        public ICommand AddAllPriceControlCommand
        {
            get
            {
                return addAllPriceControlCommand;
            }
            set
            {
                addAllPriceControlCommand = value;
            }
        }

        private ICommand removeAllAdjustmentCommand;
        public ICommand RemoveAllAdjustmentCommand
        {
            get
            {
                return removeAllAdjustmentCommand;
            }
            set
            {
                removeAllAdjustmentCommand = value;
            }
        }
        private ICommand removeOneAdjustmentCommand;
        public ICommand RemoveOneAdjustmentCommand
        {
            get
            {
                return removeOneAdjustmentCommand;
            }
            set
            {
                removeOneAdjustmentCommand = value;
            }
        }
        private ICommand addOneAdjustmentCommand;
        public ICommand AddOneAdjustmentCommand
        {
            get
            {
                return addOneAdjustmentCommand;
            }
            set
            {
                addOneAdjustmentCommand = value;
            }
        }
        private ICommand addAllAdjustmentCommand;
        public ICommand AddAllAdjustmentCommand
        {
            get
            {
                return addAllAdjustmentCommand;
            }
            set
            {
                addAllAdjustmentCommand = value;
            }
        }

        private ICommand removeAllReceptionCommand;
        public ICommand RemoveAllReceptionCommand
        {
            get
            {
                return removeAllReceptionCommand;
            }
            set
            {
                removeAllReceptionCommand = value;
            }
        }
        private ICommand removeOneReceptionCommand;
        public ICommand RemoveOneReceptionCommand
        {
            get
            {
                return removeOneReceptionCommand;
            }
            set
            {
                removeOneReceptionCommand = value;
            }
        }
        private ICommand addOneReceptionCommand;
        public ICommand AddOneReceptionCommand
        {
            get
            {
                return addOneReceptionCommand;
            }
            set
            {
                addOneReceptionCommand = value;
            }
        }
        private ICommand addAllReceptionCommand;
        public ICommand AddAllReceptionCommand
        {
            get
            {
                return addAllReceptionCommand;
            }
            set
            {
                addAllReceptionCommand = value;
            }
        }

        private ICommand removeAllLabelCommand;
        public ICommand RemoveAllLabelCommand
        {
            get
            {
                return removeAllLabelCommand;
            }
            set
            {
                removeAllLabelCommand = value;
            }
        }
        private ICommand removeOneLabelCommand;
        public ICommand RemoveOneLabelCommand
        {
            get
            {
                return removeOneLabelCommand;
            }
            set
            {
                removeOneLabelCommand = value;
            }
        }
        private ICommand addOneLabelCommand;
        public ICommand AddOneLabelCommand
        {
            get
            {
                return addOneLabelCommand;
            }
            set
            {
                addOneLabelCommand = value;
            }
        }
        private ICommand addAllLabelCommand;
        public ICommand AddAllLabelCommand
        {
            get
            {
                return addAllLabelCommand;
            }
            set
            {
                addAllLabelCommand = value;
            }
        }
        #endregion

        #endregion

        #region Constructor
        public VerActividadesViewModel()
        {
            BannerApp.PrintSeeActivities();
            deviceHandler = App.Instance.deviceHandler;

            ReturnCommand = new RelayCommand(ReturnAction);
            AcceptCommand = new RelayCommand(AcceptAction);

            RemoveAllPriceControlCommand = new RelayCommand(RemoveAllPricecontrolAction);
            RemoveOnePriceControlCommand = new RelayCommand(RemoveOnePricecontrolAction);
            AddOnePriceControlCommand = new RelayCommand(AddOnePricecontrolAction);
            AddAllPriceControlCommand = new RelayCommand(AddAllPricecontrolAction);

            RemoveAllAdjustmentCommand = new RelayCommand(RemoveAllAdjustmentAction);
            RemoveOneAdjustmentCommand = new RelayCommand(RemoveOneAdjustmentAction);
            AddOneAdjustmentCommand = new RelayCommand(AddOneAdjustmentAction);
            AddAllAdjustmentCommand = new RelayCommand(AddAllAdjustmentAction);

            RemoveAllReceptionCommand = new RelayCommand(RemoveAllReceptionAction);
            RemoveOneReceptionCommand = new RelayCommand(RemoveOneReceptionAction);
            AddOneReceptionCommand = new RelayCommand(AddOneReceptionAction);
            AddAllReceptionCommand = new RelayCommand(AddAllReceptionAction);

            RemoveAllLabelCommand = new RelayCommand(RemoveAllLabelAction);
            RemoveOneLabelCommand = new RelayCommand(RemoveOneLabelAction);
            AddOneLabelCommand = new RelayCommand(AddOneLabelAction);
            AddAllLabelCommand = new RelayCommand(AddAllLabelAction);

            getDataFromPlainFiles();
            getDataFromDBCompact();
        }
        #endregion

        #region Methods
        #region Commons Methods
        private void getDataFromPlainFiles()
        {
            string priceControlfileContent = deviceHandler.ReadPriceControlDataFile();
            if(priceControlfileContent != null)
            {
                controlPreciosConfirmados = JsonUtils.GetObservableCollectionControlPrecios(priceControlfileContent);
                
            }
            string adjustmentFilecontent = deviceHandler.ReadAdjustmentsDataFile();
            if(adjustmentFilecontent != null)
            {
                AjustesConfirmados = JsonUtils.GetObservableCollectionAjustes(adjustmentFilecontent);
            }

            string receptionFilecontent = deviceHandler.ReadReceptionDataFile();
            if(receptionFilecontent != null)
            {
                RecepcionesConfirmadas = JsonUtils.GetObservableCollectionRecepciones(receptionFilecontent);
            }
            string labelFileContent = deviceHandler.ReadLabelDataFile();
            if(labelFileContent != null)
            {
                EtiquetasConfirmadas = JsonUtils.GetObservableCollectionEtiquetas(labelFileContent);
            }
        }

        private void getDataFromDBCompact()
        {
            string sqlceDataBase = "/" + ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATABASE);
            string deviceFolder = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_ROOT);
            logger.Info("copiando desde " + deviceFolder + sqlceDataBase);

            string publicFolder = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_PROGRAM);
            string publicFolderExtended = TextUtils.ExpandEnviromentVariable(publicFolder);
            logger.Info("hacia " + publicFolderExtended + sqlceDataBase);

            ResultFileOperation result = deviceHandler.CopyFromDeviceToPublicFolder(sqlceDataBase, deviceFolder, publicFolderExtended);
            if (ResultFileOperation.OK.Equals(result))
            {
                ControlPreciosPendientes = SqlCEReaderUtils.leerControlPrecios(@"Data Source=" + publicFolderExtended + sqlceDataBase);
                AjustesPendientes = SqlCEReaderUtils.leerAjustes(@"Data Source=" + publicFolderExtended + sqlceDataBase);
                RecepcionesPendientes = SqlCEReaderUtils.leerRecepciones(@"Data Source=" + publicFolderExtended + sqlceDataBase);
                EtiquetasPendientes = SqlCEReaderUtils.leerEtiquetas(@"Data Source=" + publicFolderExtended + sqlceDataBase);
            }
        }
        #endregion

        #region Command Methods
        public void ReturnAction(object sender)
        {
            logger.Debug("ReturnAction");
        }
        public void AcceptAction(object sender)
        {
            logger.Debug("AcceptAction");
        }

        #region PriceControl Action
        public void RemoveAllPricecontrolAction(object sender)
        {
            logger.Debug("Remove All => Price control");
            foreach(ControlPrecio ctrubic in ControlPreciosConfirmados)
            {
                ControlPreciosPendientes.Add(ctrubic);
            }
            ControlPreciosConfirmados.Clear();
        }
        public void RemoveOnePricecontrolAction(object sender)
        {
            logger.Debug("Remove one => Price control");
            if(PriceControlConfirmedSelected != null)
            {
                ControlPreciosPendientes.Add(PriceControlConfirmedSelected);
                ControlPreciosConfirmados.Remove(PriceControlConfirmedSelected);
            }
        }
        public void AddOnePricecontrolAction(object sender)
        {
            logger.Debug("Add one => Price control");
            if (PendingPriceControlSelected != null)
            {
                ControlPreciosConfirmados.Add(PendingPriceControlSelected);
                ControlPreciosPendientes.Remove(PendingPriceControlSelected);
            }
        }
        public void AddAllPricecontrolAction(object sender)
        {
            logger.Debug("Add All => Price control");
            foreach(ControlPrecio ctrubic in ControlPreciosPendientes)
            {
                ControlPreciosConfirmados.Add(ctrubic);
            }
            ControlPreciosPendientes.Clear();
        }
        #endregion

        #region Adjustment Action
        public void RemoveAllAdjustmentAction(object sender)
        {
            logger.Debug("Remove All => Adjustment");
        }
        public void RemoveOneAdjustmentAction(object sender)
        {
            logger.Debug("Remove one => Adjustment");
        }
        public void AddOneAdjustmentAction(object sender)
        {
            logger.Debug("Add one => Adjustment");
        }
        public void AddAllAdjustmentAction(object sender)
        {
            logger.Debug("Add All => Adjustment");
        }
        #endregion

        #region Reception Action
        public void RemoveAllReceptionAction(object sender)
        {
            logger.Debug("Remove All => Reception");
        }
        public void RemoveOneReceptionAction(object sender)
        {
            logger.Debug("Remove one => Reception");
        }
        public void AddOneReceptionAction(object sender)
        {
            logger.Debug("Add one => Reception");
        }
        public void AddAllReceptionAction(object sender)
        {
            logger.Debug("Add All => Reception");
        }
        #endregion

        #region Label Action
        public void RemoveAllLabelAction(object sender)
        {
            logger.Debug("Remove All => Label");
        }
        public void RemoveOneLabelAction(object sender)
        {
            logger.Debug("Remove one => Label");
        }
        public void AddOneLabelAction(object sender)
        {
            logger.Debug("Add one => Label");
        }
        public void AddAllLabelAction(object sender)
        {
            logger.Debug("Add All => Label");
        }
        #endregion

        #endregion
        #endregion
    }
}
