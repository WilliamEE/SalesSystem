using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APISalesSystem
{
    public class Orden
    {
        public Orden()
        {
            LineaDeOrden = new HashSet<LineaDeOrden>();
        }

        public int Id { get; set; }
        public string Fecha { get; set; }
        public string Estado { get; set; }
        public string UsuarioId { get; set; }

        public virtual ICollection<LineaDeOrden> LineaDeOrden { get; set; }
    }
}

