using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APISalesSystem
{
    public class LineaDeOrden
    {
        public int Id { get; set; }
        public int OrdenId { get; set; }
        public int ProductoId { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }

        public virtual Orden Orden { get; set; }
        public virtual Producto Producto { get; set; }
    }
}
