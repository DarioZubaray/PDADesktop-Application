using PDADesktop.Model;
using System.Collections.ObjectModel;
using System.Data.SqlServerCe;

namespace PDADesktop.Classes.Utils
{
    public class SqlCEUpdaterUtils
    {
        public static void EmptyAjustes(string strConn)
        {
            string command = "DELETE FROM Ajustes";
            using (SqlCeConnection con = new SqlCeConnection(strConn))
            {
                con.Open();

                using (SqlCeCommand cmd = new SqlCeCommand(command, con))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();
                    }
                con.Close();
            }
        }

        public static void GuardarAjustes(ObservableCollection<Ajustes> ajustes, string strConn)
        {
            string command = "insert into Ajustes(ean, fecha, codigo_ajuste, perfil_gx, cant) values (@Val1, @val2, @val3, @val4, @val5)";
            using (SqlCeConnection con = new SqlCeConnection(strConn))
            {
                con.Open();
                foreach(Ajustes ajuste in ajustes)
                {
                    using (SqlCeCommand cmd = new SqlCeCommand(command, con))
                    {
                        cmd.Parameters.AddWithValue("@Val1", ajuste.ean);
                        cmd.Parameters.AddWithValue("@Val2", ajuste.fechaAjuste);
                        cmd.Parameters.AddWithValue("@Val3", ajuste.motivo);
                        cmd.Parameters.AddWithValue("@Val4", ajuste.perfilGenesix);
                        cmd.Parameters.AddWithValue("@Val5", ajuste.cantidad);
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }
                con.Close();
            }
        }

        public static void EmptyControlPrecios(string strConn)
        {
            string command = "DELETE FROM CTRUBIC";
            using (SqlCeConnection con = new SqlCeConnection(strConn))
            {
                con.Open();

                using (SqlCeCommand cmd = new SqlCeCommand(command, con))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        public static void GuardarControlPrecios(ObservableCollection<ControlPrecio> controlPrecios, string strConn)
        {
            string columnas = "ean, fecha, tipo_lectura, ubic_id, control_ubic, etiqueta_id, cantidad_etiquetas, alerta, num_secuencia";
            string command = "insert into CTRUBIC(" + columnas + ") values (@Val1, @val2, @val3, @val4, @val5, @val6, @val7, @val8, @val9)";
            using (SqlCeConnection con = new SqlCeConnection(strConn))
            {
                con.Open();
                foreach (ControlPrecio control in controlPrecios)
                {
                    using (SqlCeCommand cmd = new SqlCeCommand(command, con))
                    {
                        cmd.Parameters.AddWithValue("@Val1", control.EAN);
                        cmd.Parameters.AddWithValue("@Val2", control.FechaControl);
                        cmd.Parameters.AddWithValue("@Val3", control.TipoLectura);
                        cmd.Parameters.AddWithValue("@Val4", control.Pasillo);
                        cmd.Parameters.AddWithValue("@Val5", control.ControlUbicacion);
                        cmd.Parameters.AddWithValue("@Val6", control.IDEtiqueta);
                        cmd.Parameters.AddWithValue("@Val7", control.CantidadEtiquetas);
                        cmd.Parameters.AddWithValue("@Val8", control.AlertaStock);
                        cmd.Parameters.AddWithValue("@Val9", control.NumeroSecuencia);
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }
                con.Close();
            }
        }

        public static void EmptyRecepciones(string strConn)
        {
            string command = "DELETE FROM Recep";
            using (SqlCeConnection con = new SqlCeConnection(strConn))
            {
                con.Open();

                using (SqlCeCommand cmd = new SqlCeCommand(command, con))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        public static void GuardarRecepciones(ObservableCollection<ArticuloRecepcion> articuloRecepciones, string strConn)
        {
            string columnas = "fecha_control, num_pedido, num_proveedor, ce_remito, num_remito, fecha_remito, ean, uni_ingresadas, desc_proveedor";
            string command = "insert into Recep(" + columnas + ") values (@Val1, @val2, @val3, @val4, @val5, @val6, @val7, @val8, @val9)";
            using (SqlCeConnection con = new SqlCeConnection(strConn))
            {
                con.Open();
                foreach (ArticuloRecepcion artRecepcion in articuloRecepciones)
                {
                    Recepcion recepcion = artRecepcion.recepcion;
                    using (SqlCeCommand cmd = new SqlCeCommand(command, con))
                    {
                        cmd.Parameters.AddWithValue("@Val1", recepcion.fechaRecep);
                        cmd.Parameters.AddWithValue("@Val2", recepcion.numeroPedido);
                        cmd.Parameters.AddWithValue("@Val3", recepcion.numeroProveedor);
                        cmd.Parameters.AddWithValue("@Val4", recepcion.centroEmisor);
                        cmd.Parameters.AddWithValue("@Val5", recepcion.numeroRemito);
                        cmd.Parameters.AddWithValue("@Val6", recepcion.fechaRem);
                        cmd.Parameters.AddWithValue("@Val7", artRecepcion.EAN);
                        cmd.Parameters.AddWithValue("@Val8", artRecepcion.unidadesRecibidas);
                        cmd.Parameters.AddWithValue("@Val9", recepcion.descripcionProveedor);
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }
                con.Close();
            }
        }

        public static void EmptyEtiquetas(string strConn)
        {
            string command = "DELETE FROM Etiq";
            using (SqlCeConnection con = new SqlCeConnection(strConn))
            {
                con.Open();

                using (SqlCeCommand cmd = new SqlCeCommand(command, con))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        public static void GuardarEtiquetas(ObservableCollection<Etiqueta> etiquetas, string strConn)
        {
            string columnas = "ean, fecha, codigo_etiq";
            string command = "insert into Etiq(" + columnas + ") values (@Val1, @val2, @val3)";
            using (SqlCeConnection con = new SqlCeConnection(strConn))
            {
                con.Open();
                foreach (Etiqueta etiq in etiquetas)
                {
                    using (SqlCeCommand cmd = new SqlCeCommand(command, con))
                    {
                        cmd.Parameters.AddWithValue("@Val1", etiq.EAN);
                        cmd.Parameters.AddWithValue("@Val2", etiq.FechaDate);
                        cmd.Parameters.AddWithValue("@Val3", etiq.CodigoEtiqueta);
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }
                con.Close();
            }
        }
    }
}
