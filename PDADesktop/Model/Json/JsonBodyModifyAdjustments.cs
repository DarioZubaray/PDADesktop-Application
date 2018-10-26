using PDADesktop.Model.Dto;
using System.Collections.Generic;

namespace PDADesktop.Model.Json
{
    public struct JsonBodyModifyAdjustments
    {
        public long idSincronizacion { get; set; }
        public List<AjusteDto> modificarAjustes { get; set; }
    }
}
