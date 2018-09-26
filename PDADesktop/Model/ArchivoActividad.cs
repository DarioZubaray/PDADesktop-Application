namespace PDADesktop.Model
{
    public enum ArchivoActividad
    {
        [ArchivoActividadAttributes("101", "CTRUBIC.DAT", "Control de precios con ubicaciones", false)]
        CTRUBIC = 101,
        [ArchivoActividadAttributes("102", "CTRSUBIC.DAT", "Control Precios sin ubicaciones", false)]
        CTRSUBIC = 102,
        [ArchivoActividadAttributes("103", "AJUSTES.DAT", "Ajustes", false)]
        AJUTES = 103,
        [ArchivoActividadAttributes("104", "RECEP.DAT", "Recepciones", false)]
        RECEP = 104,
        [ArchivoActividadAttributes("105", "ETIQ.DAT", "Impresion de Etiquetas", false)]
        ETIQ = 105,
        [ArchivoActividadAttributes("106", "DEVOLUC.DAT", "Devoluciones", false)]
        DEVOLUC = 106,
        [ArchivoActividadAttributes("201", "ART.DAT", "Artículos", true)]
        ART = 201,
        [ArchivoActividadAttributes("202", "UBIC.DAT", "Ubicaciones", true)]
        UBIC = 202,
        [ArchivoActividadAttributes("203", "UBICART.DAT", "Ubicaciones Artículos", true)]
        UBICART = 203,
        [ArchivoActividadAttributes("204", "PEDIDOS.DAT", "Pedidos", true)]
        PEDIDOS = 204,
        [ArchivoActividadAttributes("205", "TETIQ.DAT", "Tipos de Etiquetas", true)]
        TETIQ = 205,
        [ArchivoActividadAttributes("206", "TAJUS.DAT", "Tipos de Ajustes", true)]
        TAJUS = 206,
        [ArchivoActividadAttributes("207", "PROVEED.DAT", "Proveedores Devolución", true)]
        PROVEED = 207,
        [ArchivoActividadAttributes("208", "PROVART.DAT", "Articulos Proveedores", true)]
        PROVART = 208,
        [ArchivoActividadAttributes("209", "MOTIDEV.DAT", "Motivos Devolución", true)]
        MOTIDEV = 209
    }
}
