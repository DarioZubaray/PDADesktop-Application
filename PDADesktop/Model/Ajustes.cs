namespace PDADesktop.Model
{
    public class Ajustes
    {
        public long id { get; set; }
        public long ean { get; set; }
        public string fechaAjuste { get; set; }
        public string motivo { get; set; }
        public string perfilGenesix { get; set; }
        public long cantidad { get; set; }
        public string claveAjuste { get; set; }

        public Ajustes(long ean, string fechaAjuste, string motivo, long cantidad)
        {
            this.ean = ean;
            this.fechaAjuste = fechaAjuste;
            this.motivo = motivo;
            this.cantidad = cantidad;
        }

        public Ajustes() { }


        public override string ToString()
        {
            return "Ajuste[ean: " + this.ean
                + ", fechaAjuste: " + this.fechaAjuste
                + ", motivo: " + this.motivo
                + ", cantidad: " + this.cantidad
                + " ]";
        }
    }
}
