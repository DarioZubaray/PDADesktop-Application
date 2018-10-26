using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PDADesktop.Model.Dto
{
    public struct AjusteDto
    {
        public string idAJUSTES { get; set; }
        public string lote { get; set; }
        public string EAN { get; set; }
        public string fechaSolicitud { get; set; }
        public string codigoAjuste { get;set; }
        public string usuario { get; set; }
        public string unidadesAjustar { get; set; }
        public string modificarAjuste { get; set; }
        public string informado { get; set; }

        public static List<AjusteDto> ParseObservableCollectionToAjusteDto(ObservableCollection<Ajustes> adjustments, string batchId)
        {
            List<AjusteDto> ajustesDto = new List<AjusteDto>();
            foreach(Ajustes ajuste in adjustments)
            {
                AjusteDto ajusteDto = new AjusteDto();
                ajusteDto.idAJUSTES = ajuste.id.ToString();
                ajusteDto.lote = batchId;
                ajusteDto.EAN = ajuste.ean.ToString();
                string fechaAjuste = ajuste.fechaAjuste;
                DateTime fechaSolicitud = DateTime.ParseExact(fechaAjuste, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                ajusteDto.fechaSolicitud = fechaSolicitud.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fff\\Z");
                ajusteDto.codigoAjuste = ajuste.motivo;
                ajusteDto.unidadesAjustar = ajuste.cantidad.ToString();
                ajusteDto.modificarAjuste = "0";
                ajusteDto.informado = "1";
                ajustesDto.Add(ajusteDto);
            }
            return ajustesDto;
        }
    }
}
