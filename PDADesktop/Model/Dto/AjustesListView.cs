using System;
using System.Collections.ObjectModel;

namespace PDADesktop.Model.Dto
{
    public class ListView
    {
        public int page { get; set; }
        public int records { get; set; }
        public Row[] rows { get; set; }
        public int total { get; set; }

        public static ObservableCollection<Ajustes> ParserAjustesDataGrid(ListView ajustesListView)
        {
            ObservableCollection<Ajustes> ajustes = new ObservableCollection<Ajustes>();
            Row[] rows = ajustesListView.rows;
            foreach(Row row in rows)
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
            foreach(Row row in rows)
            {
                Recepcion recepcion = new Model.Recepcion();
                recepcion.idRecepcion = Convert.ToInt64(row.cell[0]);
                recepcion.fechaRecepcion = DateTime.ParseExact(row.cell[1], "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                recepcion.remitoCompleto = row.cell[2];
                recepcion.numeroProveedor = Convert.ToInt64(row.cell[3]);
                EstadoRecepcion estadoRecepcion = new EstadoRecepcion();
                estadoRecepcion.idEstado = Convert.ToInt64(row.cell[4]);
                estadoRecepcion.descripcion = row.cell[5];
                recepcion.estado = estadoRecepcion;
                recepciones.Add(recepcion);
            }
            return recepciones;
        }
    }
}
