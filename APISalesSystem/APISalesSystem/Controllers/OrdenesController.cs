using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APISalesSystem.Controllers
{
    [EnableCors("AllowWebApp")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdenesController : ControllerBase
    {
        private readonly DbSalesSystemContext _context;
        UsuarioFirebaseDecodificado autenticar = new UsuarioFirebaseDecodificado();
        UsuarioFirebase usuario = new UsuarioFirebase();
        public OrdenesController(DbSalesSystemContext context) {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Orden>> PostOrden(Orden orden, [FromHeader] string authorization) {
            string idToken = authorization.Remove(0, 7);
            usuario = await autenticar.obtener_usuario(idToken);
            orden.UsuarioId = usuario.Uid;
            orden.Estado = "Pendiente";
            orden.Fecha = DateTime.Now.ToString();
            _context.Orden.Add(orden);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetOrden", new { id = orden.Id }, orden);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Orden>> GetOrden(int id) {
            var orden = _context.Orden.Where(o => o.Id == id).FirstOrDefault();
            if (orden == null)
            {
                return NotFound();
            }
            orden.LineaDeOrden = _context.LineaDeOrden.Where(l => l.OrdenId == id).ToList();
            return orden;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orden>>> GetOrdenes([FromHeader] string authorization) {
            string idToken = authorization.Remove(0, 7);
            usuario = await autenticar.obtener_usuario(idToken);

            var ordenes = _context.Orden.Where(o => o.UsuarioId == usuario.Uid).ToList();
            foreach (var orden in ordenes) {
                orden.LineaDeOrden = _context.LineaDeOrden.Where(l => l.OrdenId == orden.Id).ToList();
            }
            return ordenes;
        }
    }
}
