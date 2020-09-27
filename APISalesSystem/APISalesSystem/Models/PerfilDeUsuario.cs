using System;
using System.Collections.Generic;

namespace APISalesSystem
{
    public partial class PerfilDeUsuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string FotoDePerfil { get; set; }
        public int? IdUsuario { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
