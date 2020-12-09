using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APISalesSystem.Controllers
{
    public class UsuarioFirebaseDecodificado
    {
        public async Task<UsuarioFirebase> obtener_usuario(string idToken)
        {
            UsuarioFirebase usuario = new UsuarioFirebase();
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            //Sirve para poner claims a un token
            //var prueba = new Dictionary<string, object>()
            //{
            //{ "admin", true },
            //};
            //await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(decodedToken.Uid, prueba);
            usuario.Uid = decodedToken.Uid;
            var claims = decodedToken.Claims;
            object isAdmin, isSeller;
            if (claims.TryGetValue("admin", out isAdmin))
            {

                if ((bool)isAdmin)
                {
                    usuario.admin = true;
                }
            }
            if (claims.TryGetValue("seller", out isSeller))
            {

                if ((bool)isSeller)
                {
                    usuario.seller = true;
                }
            }
            return usuario;
        }
    }
}
