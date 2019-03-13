using System;
using System.Globalization;

namespace PDADesktop.Model
{
    class Etiqueta
    {
        private string _fecha;
        private DateTime _fechaDate;

        public static readonly string DATE_FORMAT = "yyyyMMddHHmmss";

        public long ID { get; set; }
        public string EAN { get; set; }
        public string CodigoEtiqueta { get; set; }
        public string Fecha
        {
            get
            {
                return _fecha;
            }
            set
            {
                _fechaDate = DateTime.ParseExact(value, DATE_FORMAT, CultureInfo.InvariantCulture);
                _fecha = value;
            }
        }

        public DateTime FechaDate
        {
            get
            {
                return _fechaDate;
            }
            set
            {
                _fechaDate = value;
                var dateString = value.ToString(DATE_FORMAT, CultureInfo.InvariantCulture);
                _fecha = dateString;
            }
        }
    }
}
