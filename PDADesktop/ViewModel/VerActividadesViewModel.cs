using PDADesktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDADesktop.ViewModel
{
    class VerActividadesViewModel : ViewModelBase
    {
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

        public VerActividadesViewModel()
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

            AjustesPendientes = new List<Ajustes>();
            Ajustes ajustePendientes1 = new Ajustes(77903792L, "20190110091757", "2", 15);
            AjustesPendientes.Add(ajustePendientes1);
            Ajustes ajustePendientes2 = new Ajustes(77946805L, "20190110091811", "2", 5);
            AjustesPendientes.Add(ajustePendientes2);
            Ajustes ajustePendientes3 = new Ajustes(75024956L, "20190110091811", "10", 10);
            AjustesPendientes.Add(ajustePendientes3);
            Ajustes ajustePendientes4 = new Ajustes(75032715L, "20190110091811", "10", 8);
            AjustesPendientes.Add(ajustePendientes4);

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

            EtiquetasConfirmadas = new List<Etiqueta>();
            Etiqueta etiqueta = new Etiqueta();
            etiqueta.EAN = "7790040719804";
            etiqueta.FechaDate = new DateTime(2019, 03, 13);
            etiqueta.CodigoEtiqueta = "ET2";
            EtiquetasConfirmadas.Add(etiqueta);
        }
    }
}
