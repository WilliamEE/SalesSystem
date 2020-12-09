using System;
using System.Collections.Generic;

namespace APISalesSystem
{
    public partial class Producto
    {
        public Producto()
        {
            Deseo = new HashSet<Deseo>();
            SolicitudProducto = new HashSet<SolicitudProducto>();
            LineaDeOrden = new HashSet<LineaDeOrden>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal? Precio { get; set; }
        public string ImagenUrl { get; set; }
        public int IdCategoria { get; set; }
        public string IdUsuario { get; set; }
        public bool? Activo { get; set; }

        public virtual Categoria IdCategoriaNavigation { get; set; }
        public virtual ICollection<Deseo> Deseo { get; set; }
        public virtual ICollection<SolicitudProducto> SolicitudProducto { get; set; }
        public virtual ICollection<LineaDeOrden> LineaDeOrden { get; set; }
    }
}
