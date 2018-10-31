using PDADesktop.Model;
using PDADesktop.Model.Dto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDADesktop.Classes.Utils
{
    public class ListViewUtils
    {

        public static ObservableCollection<Ajustes> ParserAjustesDataGrid(ListView ajustesListView)
        {
            ObservableCollection<Ajustes> ajustes = new ObservableCollection<Ajustes>();
            Row[] rows = ajustesListView.rows;
            foreach (Row row in rows)
            {
                Ajustes ajuste = new Ajustes();
                ajuste.id = Convert.ToInt64(row.cell[0]);
                ajuste.fechaAjuste = row.cell[1];
                ajuste.ean = Convert.ToInt64(row.cell[2]);
                ajuste.motivo = "-";
                ajuste.cantidad = Convert.ToInt64(row.cell[3]);
                ajustes.Add(ajuste);
            }
            return ajustes;
        }

        public static ObservableCollection<Recepcion> ParserRecepcionDataGrid(ListView recepcionListView)
        {
            ObservableCollection<Recepcion> recepciones = new ObservableCollection<Recepcion>();
            Row[] rows = recepcionListView.rows;
            foreach (Row row in rows)
            {
                Recepcion recepcion = new Model.Recepcion();
                recepcion.idRecepcion = Convert.ToInt64(row.cell[0]);

                DateTime datetime;
                if(DateTime.TryParse(row.cell[1], out datetime))
                {
                    recepcion.fechaRecepcion = datetime;
                }
                recepcion.remitoCompleto = row.cell[2];

                long providerNumber = 0;
                if(Int64.TryParse(row.cell[3], out providerNumber))
                {
                    recepcion.numeroProveedor = providerNumber;
                }

                EstadoRecepcion estadoRecepcion = new EstadoRecepcion();
                long stateId = 0;
                if(Int64.TryParse(row.cell[4], out stateId))
                {
                    estadoRecepcion.idEstado = stateId;
                }
                estadoRecepcion.descripcion = row.cell[5];
                recepcion.estado = estadoRecepcion;
                recepciones.Add(recepcion);
            }
            return recepciones;
        }

        public static ObservableCollection<Recepcion> ParserImprimirRecepcionDataGrid(ListView recepcionListView)
        {
            ObservableCollection<Recepcion> recepciones = new ObservableCollection<Recepcion>();
            Row[] rows = recepcionListView.rows;
            foreach (Row row in rows)
            {
                Recepcion recepcion = new Model.Recepcion();
                recepcion.idRecepcion = Convert.ToInt64(row.cell[0]);

                DateTime datetime;
                if (DateTime.TryParse(row.cell[1], out datetime))
                {
                    recepcion.fechaRecepcion = datetime;
                }
                recepcion.remitoCompleto = row.cell[2];

                long providerNumber = 0;
                if (Int64.TryParse(row.cell[3], out providerNumber))
                {
                    recepcion.numeroProveedor = providerNumber;
                }

                recepcion.descripcionProveedor = row.cell[4];

                long transmitterCenter = 0;
                if(Int64.TryParse(row.cell[5].Substring(0,3), out transmitterCenter))
                {
                    recepcion.centroEmisor = transmitterCenter;
                }

                long orderNumber = 0;
                if(Int64.TryParse(row.cell[5].Substring(3), out orderNumber))
                {
                    recepcion.numeroPedido = orderNumber;
                }

                recepcion.numeroRecepcion = Convert.ToInt64(row.cell[6]);
                recepciones.Add(recepcion);
            }
            return recepciones;
        }
    }
}
