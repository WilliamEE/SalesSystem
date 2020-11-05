using System;
using System.Collections.Generic;

namespace APISalesSystem
{
    public partial class Producto
    {
        public Producto()
        {
            SolicitudProductoIdProductoModificarNavigation = new HashSet<SolicitudProducto>();
            SolicitudProductoIdProductoNavigation = new HashSet<SolicitudProducto>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal? Precio { get; set; }
        public string ImagenUrl { get; set; }
        public int IdCategoria { get; set; }
        public string IdUsuario { get; set; }
        public bool? Activo { get; set; }

        public virtual Categoria IdCategoriaNavigation { get; set; }
        public virtual ICollection<SolicitudProducto> SolicitudProductoIdProductoModificarNavigation { get; set; }
        public virtual ICollection<SolicitudProducto> SolicitudProductoIdProductoNavigation { get; set; }
    }
}
