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
        private ControlPrecio PriceControlConfirmedSelected { get; set; }
        private ControlPrecio PendingPriceControlSelect { get; set; }
        private Ajustes AdjustmentConfirmedSelected { get; set; }
        private Ajustes PendingAdjustmentSelected { get; set; }
        private Recepcion ReceptionConfirmedSelected { get; set; }
        private Recepcion PendingReceptionSelected { get; set; }
        private Etiqueta LabelConfirmedSelected { get; set; }
        private Etiqueta PendingLabelSelected { get; set; }
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
        #endregion

        #region Constructor
        public VerActividadesViewModel()
        {
            BannerApp.PrintSeeActivities();
            deviceHandler = App.Instance.deviceHandler;

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
        #endregion
    }
}
