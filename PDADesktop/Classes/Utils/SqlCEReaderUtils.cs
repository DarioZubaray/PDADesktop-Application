using log4net;
using PDADesktop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlServerCe;

namespace PDADesktop.Classes.Utils
{
    class SqlCEReaderUtils
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static ObservableCollection<Ajustes> leerAjustes(string cadenaConexion)
        {
            logger.Debug("Leyendo ajustes ...");
            ObservableCollection<Ajustes> listaAjustes = new ObservableCollection<Ajustes>();
            using (SqlCeConnection cnn = new SqlCeConnection(cadenaConexion))
            {
                SqlCeCommand sqlcmd = cnn.CreateCommand();
                sqlcmd.CommandText = "Ajustes";
                sqlcmd.CommandType = CommandType.TableDirect;
                cnn.Open();
                bool emptyTable = true;
                SqlCeDataReader dr = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        emptyTable = false;
                        Ajustes ajuste = new Ajustes();
                        ajuste.ean = Int64.Parse(dr.GetString(0));
                        ajuste.fechaAjuste = dr.GetString(1);
                        ajuste.claveAjuste = dr.GetString(2);
                        ajuste.perfilGenesix = dr.GetString(3);
                        ajuste.cantidad = (long)dr.GetDouble(4);
                        listaAjustes.Add(ajuste);
                    }
                    if (emptyTable)
                    {
                        logger.Debug("No se encontraron registros.");
                    }
                }
            }
            return listaAjustes;
        }

        public static ObservableCollection<ControlPrecio> leerControlPrecios(string cadenaConexion)
        {
            logger.Debug("Leyendo control de precios ...");
            ObservableCollection<ControlPrecio> listaControlPrecios = new ObservableCollection<ControlPrecio>();
            using(SqlCeConnection cnn = new SqlCeConnection(cadenaConexion))
            {
                SqlCeCommand sqlcmd = cnn.CreateCommand();
                sqlcmd.CommandText = "Ctrubic";
                sqlcmd.CommandType = CommandType.TableDirect;
                cnn.Open();
                bool emptyTable = true;
                SqlCeDataReader dr = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        emptyTable = false;
                        ControlPrecio controlPrecio = new ControlPrecio();
                        controlPrecio.EAN = dr.GetString(0);
                        controlPrecio.FechaControl = dr.GetDateTime(1);
                        controlPrecio.TipoLectura = dr.GetInt32(2);
                        controlPrecio.Pasillo = dr.GetString(3);
                        controlPrecio.ControlUbicacion = (ControlPrecio.TipoControlUbicacion)dr.GetInt32(4);
                        controlPrecio.IDEtiqueta = dr.GetString(5);
                        controlPrecio.CantidadEtiquetas = dr.GetInt32(6);
                        controlPrecio.AlertaStock = dr.GetBoolean(7);
                        controlPrecio.NumeroSecuencia = dr.GetString(8);
                        listaControlPrecios.Add(controlPrecio);
                    }
                    if (emptyTable)
                    {
                        logger.Debug("No se encontraron registros");
                    }
                    dr.Close();
                }
            }
            return listaControlPrecios;
        }

        public static ObservableCollection<Etiqueta> leerEtiquetas(string cadenaConexion)
        {
            logger.Debug("Leyendo etiquetas ...");
            ObservableCollection<Etiqueta> listaEtiquetas = new ObservableCollection<Etiqueta>();
            using (SqlCeConnection cnn = new SqlCeConnection(cadenaConexion))
            {
                SqlCeCommand sqlcmd = cnn.CreateCommand();
                sqlcmd.CommandText = "Etiq";
                sqlcmd.CommandType = CommandType.TableDirect;
                cnn.Open();
                bool emptyTable = true;
                SqlCeDataReader dr = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        emptyTable = false;
                        Etiqueta etiqueta = new Etiqueta();
                        etiqueta.EAN = dr.GetString(1);
                        etiqueta.Fecha = dr.GetDateTime(2).ToString();
                        etiqueta.CodigoEtiqueta = dr.GetString(3);
                        listaEtiquetas.Add(etiqueta);
                    }
                    if (emptyTable)
                    {
                        logger.Debug("No se encontraron registros");
                    }
                }
            }
            return listaEtiquetas;
        }

        public static ObservableCollection<ArticuloRecepcion> leerRecepciones(string cadenaConexion)
        {
            logger.Debug("Leyendo recepciones ...");
            ObservableCollection<ArticuloRecepcion> listaArticulosRecepcion = new ObservableCollection<ArticuloRecepcion>();
            using (SqlCeConnection cnn = new SqlCeConnection(cadenaConexion))
            {
                SqlCeCommand sqlcmd = cnn.CreateCommand();
                sqlcmd.CommandText = "Recep";
                sqlcmd.CommandType = CommandType.TableDirect;
                cnn.Open();
                bool emptyTable = true;
                SqlCeDataReader dr = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr.HasRows)
                {
                    while (dr.Read())
                        {
                            emptyTable = false;
                            Recepcion recepcion = new Recepcion();
                            string recepcionString = dr.GetString(0);
                            recepcion.fechaRecepcion = DateTime.ParseExact(recepcionString, "yyyyMMddHHmmss", null);
                            recepcion.numeroPedido = dr.GetInt64(1);
                            recepcion.numeroProveedor = dr.GetInt64(2);
                            //R - campo 3 sin uso
                            recepcion.sucursalRemito = Int64.Parse(dr.GetString(4));
                            recepcion.numeroRemito = Int64.Parse(dr.GetString(5));
                            string remitoString = dr.GetString(6);
                            recepcion.FechaRemito = DateTime.ParseExact(remitoString, "yyyyMMddHHmmss", null);
                            recepcion.descripcionProveedor = dr.GetString(10);
                            ArticuloRecepcion articuloRecepcion = new ArticuloRecepcion();
                            articuloRecepcion.EAN = Int64.Parse(dr.GetString(7));
                            articuloRecepcion.unidadesRecibidas = dr.GetDouble(8);
                            // Clave - campo 9 sin uso
                            articuloRecepcion.recepcion = recepcion;
                            listaArticulosRecepcion.Add(articuloRecepcion);
                        }
                    if (emptyTable)
                    {
                        logger.Debug("No se encontraron registros");
                    }
                }
            }
            return listaArticulosRecepcion;
        }
    }
}
