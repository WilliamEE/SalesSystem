using System;
using System.Collections.Generic;

namespace APISalesSystem
{
    public partial class SolicitudDeAfiliacion
    {
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public string ReferenciaBancariaUrl { get; set; }
        public string ReciboLuzUrl { get; set; }
        public string ReciboAguaUrl { get; set; }
        public string ReciboTelefonoUrl { get; set; }
        public string PagareUrl { get; set; }
        public string Estado { get; set; }
        public string IdUsuario { get; set; }
        public string Comentario { get; set; }
    }
}
