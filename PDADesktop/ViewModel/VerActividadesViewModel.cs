using log4net;
using PDADesktop.Classes;
using PDADesktop.Classes.Devices;
using PDADesktop.Classes.Utils;
using PDADesktop.Model;
using System;
using System.Collections.Generic;
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

        #region List
        private List<ControlPrecio> controlPreciosConfirmados;
        public List<ControlPrecio> ControlPreciosConfirmados
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
        private List<ControlPrecio> controlPreciosPendientes;
        public List<ControlPrecio> ControlPreciosPendientes
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

        private List<Ajustes> ajustesConfirmados;
        public List<Ajustes> AjustesConfirmados
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
        private List<Ajustes> ajustesPendientes;
        public List<Ajustes> AjustesPendientes
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

        private List<ArticuloRecepcion> recepcionesConfirmadas;
        public List<ArticuloRecepcion> RecepcionesConfirmadas
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
        private List<ArticuloRecepcion> recepcionesPendientes;
        public List<ArticuloRecepcion> RecepcionesPendientes
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

        private List<Etiqueta> etiquetasConfirmadas;
        public List<Etiqueta> EtiquetasConfirmadas
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
        private List<Etiqueta> etiquetasPendientes;
        public List<Etiqueta> EtiquetasPendientes
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

            getDataFromDBCompact();

            //setControlPreciosConfirmadosHardCode();
            //setAjustesConfirmadosHardCode();
            //setAjustesPendientesHardCode();
            //setReceocionesPendientesHardCode();
            //setEtiquetasConfirmadasHardcode();
        }
        #endregion

        #region Methods
        #region Commons Methods
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
                //leer actividades
                ControlPreciosPendientes = SqlCEReaderUtils.leerControlPrecios(@"Data Source=" + publicFolderExtended + sqlceDataBase);
                AjustesPendientes = SqlCEReaderUtils.leerAjustes(@"Data Source=" + publicFolderExtended + sqlceDataBase);
                RecepcionesPendientes = SqlCEReaderUtils.leerRecepciones(@"Data Source=" + publicFolderExtended + sqlceDataBase);
                EtiquetasPendientes = SqlCEReaderUtils.leerEtiquetas(@"Data Source=" + publicFolderExtended + sqlceDataBase);
            }
        }
        #endregion

        #region Methods Hardcode
        private void setControlPreciosConfirmadosHardCode()
        {
            controlPreciosConfirmados = new List<ControlPrecio>();
            ControlPrecio ctrubic2 = new ControlPrecio();
            ctrubic2.EAN = "7791787000422";
            ctrubic2.FechaControl = new DateTime(2019, 1, 10);
            ctrubic2.TipoLectura = 4;
            ctrubic2.Pasillo = "PASIL6";
            ctrubic2.ControlUbicacion = ControlPrecio.TipoControlUbicacion.UbicNueva;
            ctrubic2.IDEtiqueta = "001";
            ctrubic2.CantidadEtiquetas = 5;
            ctrubic2.AlertaStock = false;
            ctrubic2.NumeroSecuencia = "0";
            controlPreciosConfirmados.Add(ctrubic2);
            ControlPrecio ctrubic3 = new ControlPrecio();
            ctrubic3.EAN = "7791787000422";
            ctrubic3.FechaControl = new DateTime(2019, 1, 10);
            ctrubic3.TipoLectura = 2;
            ctrubic3.Pasillo = "PASIL10";
            ctrubic3.ControlUbicacion = ControlPrecio.TipoControlUbicacion.UbicCorrecta;
            ctrubic3.IDEtiqueta = "001";
            ctrubic3.CantidadEtiquetas = 1;
            ctrubic3.AlertaStock = false;
            ctrubic3.NumeroSecuencia = "0";
            controlPreciosConfirmados.Add(ctrubic3);
        }

        private void setAjustesConfirmadosHardCode()
        {
            AjustesConfirmados = new List<Ajustes>();
            Ajustes ajuste1 = new Ajustes(77903792L, "20190110091757", "2", 15);
            AjustesConfirmados.Add(ajuste1);
            Ajustes ajuste2 = new Ajustes(77946805L, "20190110091811", "2", 5);
            AjustesConfirmados.Add(ajuste2);
            Ajustes ajuste3 = new Ajustes(75024956L, "20190110091811", "10", 10);
            AjustesConfirmados.Add(ajuste3);
            Ajustes ajuste4 = new Ajustes(75032715L, "20190110091811", "10", 8);
            AjustesConfirmados.Add(ajuste4);
            Ajustes ajuste5 = new Ajustes(77900302L, "20190110091811", "10", 4);
            AjustesConfirmados.Add(ajuste5);
        }

        private void setAjustesPendientesHardCode()
        {
            AjustesPendientes = new List<Ajustes>();
            Ajustes ajustePendientes1 = new Ajustes(77903792L, "20190110091757", "2", 15);
            AjustesPendientes.Add(ajustePendientes1);
            Ajustes ajustePendientes2 = new Ajustes(77946805L, "20190110091811", "2", 5);
            AjustesPendientes.Add(ajustePendientes2);
            Ajustes ajustePendientes3 = new Ajustes(75024956L, "20190110091811", "10", 10);
            AjustesPendientes.Add(ajustePendientes3);
            Ajustes ajustePendientes4 = new Ajustes(75032715L, "20190110091811", "10", 8);
            AjustesPendientes.Add(ajustePendientes4);
        }

        private void setReceocionesPendientesHardCode()
        {
            RecepcionesPendientes = new List<ArticuloRecepcion>();
            ArticuloRecepcion recep1 = new ArticuloRecepcion();
            Recepcion recepcion = new Recepcion();
            recepcion.fechaRecepcion = new DateTime(2019, 03, 13);
            recepcion.numeroPedido = 15678;
            recepcion.numeroProveedor = 77;
            recepcion.centroEmisor = 005;
            recepcion.numeroRemito = 7654321;
            recepcion.FechaRemito = new DateTime(2019, 03, 07);
            recep1.recepcion = recepcion;
            recep1.EAN = 123;
            recep1.unidadesRecibidas = 15;
            RecepcionesPendientes.Add(recep1);
        }

        private void setEtiquetasConfirmadasHardcode()
        {
            EtiquetasConfirmadas = new List<Etiqueta>();
            Etiqueta etiqueta = new Etiqueta();
            etiqueta.EAN = "7790040719804";
            etiqueta.FechaDate = new DateTime(2019, 03, 13);
            etiqueta.CodigoEtiqueta = "ET2";
            EtiquetasConfirmadas.Add(etiqueta);
        }
        #endregion
        #endregion
    }
}
