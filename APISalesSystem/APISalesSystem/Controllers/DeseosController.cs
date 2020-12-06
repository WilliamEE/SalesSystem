using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace APISalesSystem.Controllers
{
    [EnableCors("AllowWebApp")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeseosController : ControllerBase
    {
        private readonly DbSalesSystemContext _context;
        UsuarioFirebaseDecodificado autenticar = new UsuarioFirebaseDecodificado();
        UsuarioFirebase usuario = new UsuarioFirebase();

        public DeseosController(DbSalesSystemContext context) {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Deseo>> PostDeseo(Deseo deseo, [FromHeader] string Authorization) {
            string idToken = Authorization.Remove(0, 7);
            usuario = await autenticar.obtener_usuario(idToken);
            var deseoExistente = _context.Deseo.Where(d => d.ProductoId == deseo.ProductoId && d.UsuarioId == usuario.Uid).FirstOrDefault();

            if ( deseoExistente != null ) {
                throw new Exception("Este producto ya fue agregado a la lista de deseos");
            }

            _context.Deseo.Add(deseo);
            deseo.UsuarioId = usuario.Uid;
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetDeseo", new { id = deseo.Id }, deseo);
        }


        [HttpPost("{id}")]
        public async Task<ActionResult<Deseo>> GetDeseo(int id) {
            var deseo = _context.Deseo.Where(d => d.Id == id).FirstOrDefault();
            if (deseo == null) {
                return NotFound();
            }
            return deseo;
        }

        // GET: api/Deseos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Deseo>>> GetListDeseos([FromHeader] String Authorization) {
            string idToken = Authorization.Remove(0, 7);
            usuario = await autenticar.obtener_usuario(idToken);
            List<Producto> productos = new List<Producto>();
            var deseos = _context.Deseo.Where(d => d.UsuarioId == usuario.Uid).ToList();
            foreach (Deseo deseo in deseos) {
                Producto producto = _context.Producto.Where(p => p.Id == deseo.ProductoId).FirstOrDefault();
                productos.Add(producto);
            }
            return deseos;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Deseo>> DeleteDeseo(int id) {
            var deseo = await _context.Deseo.FindAsync(id);
            if (deseo == null) {
                return NotFound();
            }

            _context.Deseo.Remove(deseo);
            await _context.SaveChangesAsync();
            return deseo;
        }
    }
}
