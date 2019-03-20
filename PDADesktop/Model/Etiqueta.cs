using PDADesktop.Classes.Utils;
using System;
using System.Globalization;

namespace PDADesktop.Model
{
    public class Etiqueta
    {

        public static readonly string DATE_FORMAT = "yyyyMMddHHmmss";

        public long ID { get; set; }
        public string EAN { get; set; }
        public string CodigoEtiqueta { get; set; }

        public string Fecha
        {
            get
            {
                return DateTimeUtils.GetActivityStringDate(FechaDate);
            }
            set
            {
                FechaDate = DateTimeUtils.GetActivityDatetime(value);
            }
        }

        public DateTime FechaDate { get; set; }
    }
}
