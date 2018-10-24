using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PDADesktop.Model.Dto
{
    public class AjustesDTO
    {
        public int page { get; set; }
        public int records { get; set; }
        public Row[] rows { get; set; }
        public int total { get; set; }

        public static ObservableCollection<Ajustes> ParserDataGrid(AjustesDTO ajustesDto)
        {
            ObservableCollection<Ajustes> ajustes = new ObservableCollection<Ajustes>();
            Row[] rows = ajustesDto.rows;
            foreach(Row row in rows)
            {
                long ean = Convert.ToInt64(row.cell[2]);
                string fecha = row.cell[1];
                string motivo = "-";
                long cantidad = Convert.ToInt64(row.cell[3]);
                Ajustes ajuste = new Ajustes(ean, fecha, motivo, cantidad);
                ajustes.Add(ajuste);
            }
            return ajustes;
        }
    }
}
