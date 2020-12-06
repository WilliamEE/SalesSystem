using System;
using System.Collections.Generic;

namespace APISalesSystem
{
    public partial class Deseo
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int ProductoId { get; set; }

        public virtual Producto Producto { get; set; }
    }
}
