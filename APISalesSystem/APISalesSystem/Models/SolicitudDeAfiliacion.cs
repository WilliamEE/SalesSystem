using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APISalesSystem
{
    public partial class SolicitudDeAfiliacion
    {
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
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
