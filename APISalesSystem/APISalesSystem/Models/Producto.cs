using System;
using System.Collections.Generic;

namespace APISalesSystem
{
    public partial class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal? Precio { get; set; }
        public string ImagenUrl { get; set; }
        public int? IdCategoria { get; set; }
        public int? IdPerfil { get; set; }

        public virtual Categoria IdCategoriaNavigation { get; set; }
    }
}
