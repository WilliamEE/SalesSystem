using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APISalesSystem.Controllers
{
    public class UsuarioFirebase
    {
        public UsuarioFirebase()
        {
            Uid = "";
            email = "";
            admin = false;
            seller = false;
        }

        public string Uid { get; set; }
        public string email { get; set; }
        public bool admin { get; set; }
        public bool seller { get; set; }
    }
}
