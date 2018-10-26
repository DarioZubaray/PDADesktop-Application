using System;
using System.Collections.ObjectModel;

namespace PDADesktop.Model.Dto
{
    public class AjustesListView
    {
        public int page { get; set; }
        public int records { get; set; }
        public Row[] rows { get; set; }
        public int total { get; set; }

        public static ObservableCollection<Ajustes> ParserDataGrid(AjustesListView ajustesListView)
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
    }
}
