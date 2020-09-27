using System;
using System.Collections.Generic;

namespace APISalesSystem
{
    public partial class Usuario
    {
        public int Id { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public int? IdRol { get; set; }

        public virtual Rol IdRolNavigation { get; set; }
    }
}
