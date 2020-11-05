using System;
using System.Collections.Generic;

namespace APISalesSystem
{
    public partial class SolicitudProducto
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public int? IdProducto { get; set; }
        public int? IdProductoModificar { get; set; }
        public string Estado { get; set; }
        public string Comentario { get; set; }

        public virtual Producto IdProductoNavigation { get; set; }
    }
}
