using log4net;
using PDADesktop.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;

namespace PDADesktop.Classes.Utils
{
    class SqlCEReaderUtils
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static List<Ajustes> leerAjustes(SqlCeConnection con)
        {
            logger.Debug("Leyendo ajustes ...");
            List<Ajustes> listaAjustes = new List<Ajustes>();
            string query = "SELECT * FROM AJUSTES";
            using (SqlCeCommand com = new SqlCeCommand(query, con))
            {
                bool emptyTable = true;
                SqlCeDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    emptyTable = false;
                    Ajustes ajuste = new Ajustes();
                    ajuste.ean = Int64.Parse(reader.GetString(0));
                    ajuste.fechaAjuste = reader.GetString(1);
                    ajuste.claveAjuste = reader.GetString(2);
                    ajuste.perfilGenesix = reader.GetString(3);
                    ajuste.cantidad = (long)reader.GetDouble(4);
                    listaAjustes.Add(ajuste);
                }
                if (emptyTable)
                {
                    logger.Debug("No se encontraron registros.");
                }
            }
            return listaAjustes;
        }

        public static List<ControlPrecio> leerControlPrecios(SqlCeConnection con)
        {
            logger.Debug("Leyendo control de precios ...");
            List<ControlPrecio> listaControlPrecios = new List<ControlPrecio>();
            string query = "SELECT * FROM CTRUBIC";
            using (SqlCeCommand com = new SqlCeCommand(query, con))
            {
                bool emptyTable = true;
                SqlCeDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    emptyTable = false;
                    ControlPrecio controlPrecio = new ControlPrecio();
                    controlPrecio.EAN = reader.GetString(0);
                    controlPrecio.FechaControl = reader.GetDateTime(1);
                    controlPrecio.TipoLectura = reader.GetInt32(2);
                    controlPrecio.Pasillo = reader.GetString(3);
                    controlPrecio.ControlUbicacion = (ControlPrecio.TipoControlUbicacion)reader.GetInt32(4);
                    controlPrecio.IDEtiqueta = reader.GetString(5);
                    controlPrecio.CantidadEtiquetas = reader.GetInt32(6);
                    controlPrecio.AlertaStock = reader.GetBoolean(7);
                    controlPrecio.NumeroSecuencia = reader.GetString(8);
                    listaControlPrecios.Add(controlPrecio);
                }
                if (emptyTable)
                {
                    logger.Debug("No se encontraron registros");
                }
            }
            return listaControlPrecios;
        }

        public static List<Etiqueta> leerEtiquetas(SqlCeConnection con)
        {
            logger.Debug("Leyendo etiquetas ...");
            List<Etiqueta> listaEtiquetas = new List<Etiqueta>();
            string query = "SELECT * FROM ETIQ";
            using (SqlCeCommand com = new SqlCeCommand(query, con))
            {
                bool emptyTable = true;
                SqlCeDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    emptyTable = false;
                    Etiqueta etiqueta = new Etiqueta();
                    etiqueta.EAN = reader.GetString(0);
                    etiqueta.Fecha = reader.GetString(1);
                    etiqueta.CodigoEtiqueta = reader.GetString(2);
                    listaEtiquetas.Add(etiqueta);
                }
                if (emptyTable)
                {
                    logger.Debug("No se encontraron registros");
                }
            }
            return listaEtiquetas;
        }

        public static List<ArticuloRecepcion> leerRecepciones(SqlCeConnection con)
        {
            logger.Debug("Leyendo recepciones ...");
            List<ArticuloRecepcion> listaArticulosRecepcion = new List<ArticuloRecepcion>();
            string query = "SELECT * FROM RECEP";
            using (SqlCeCommand com = new SqlCeCommand(query, con))
            {
                bool emptyTable = true;
                SqlCeDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    emptyTable = false;
                    Recepcion recepcion = new Recepcion();
                    string recepcionString = reader.GetString(0);
                    recepcion.fechaRecepcion = DateTime.ParseExact(recepcionString, "yyyyMMddHHmmss", null);
                    recepcion.numeroPedido = reader.GetInt64(1);
                    recepcion.numeroProveedor = reader.GetInt64(2);
                    //R - campo 3 sin uso
                    recepcion.sucursalRemito = Int64.Parse(reader.GetString(4));
                    recepcion.numeroRemito = Int64.Parse(reader.GetString(5));
                    string remitoString = reader.GetString(6);
                    recepcion.FechaRemito = DateTime.ParseExact(remitoString, "yyyyMMddHHmmss", null);
                    recepcion.descripcionProveedor = reader.GetString(10);
                    ArticuloRecepcion articuloRecepcion = new ArticuloRecepcion();
                    articuloRecepcion.EAN = Int64.Parse(reader.GetString(7));
                    articuloRecepcion.unidadesRecibidas = reader.GetDouble(8);
                    // Clave - campo 9 sin uso
                    articuloRecepcion.recepcion = recepcion;
                    listaArticulosRecepcion.Add(articuloRecepcion);
                }
                if (emptyTable)
                {
                    logger.Debug("No se encontraron registros");
                }
            }
            return listaArticulosRecepcion;
        }
    }
}
