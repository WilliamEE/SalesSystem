using System;
using System.Collections.Generic;

namespace APISalesSystem
{
    public partial class Categoria
    {
        public Categoria()
        {
            InverseIdPadreNavigation = new HashSet<Categoria>();
            Producto = new HashSet<Producto>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public int? IdPadre { get; set; }

        public virtual Categoria IdPadreNavigation { get; set; }
        public virtual ICollection<Categoria> InverseIdPadreNavigation { get; set; }
        public virtual ICollection<Producto> Producto { get; set; }
    }
}
