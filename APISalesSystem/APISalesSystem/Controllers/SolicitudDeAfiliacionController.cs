using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APISalesSystem;
using System.IO;
using FirebaseAdmin.Auth;
using SQLitePCL;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Authorization;

namespace APISalesSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SolicitudDeAfiliacionController : ControllerBase
    {
        UsuarioFirebaseDecodificado autenticar = new UsuarioFirebaseDecodificado();
        UsuarioFirebase usuario = new UsuarioFirebase();
        private readonly DbSalesSystemContext _context;

        public SolicitudDeAfiliacionController(DbSalesSystemContext context)
        {
            _context = context;
        }

        // GET: api/SolicitudDeAfiliacion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SolicitudDeAfiliacion>>> GetSolicitudDeAfiliacion([FromQuery] int pagina, [FromQuery] int cantidad, [FromQuery] string estado, [FromHeader] string Authorization)
        {
            string idToken = Authorization.Remove(0, 7);
            usuario = await autenticar.obtener_usuario(idToken);

            List<SolicitudDeAfiliacion> solicitud = new List<SolicitudDeAfiliacion>();
                if (usuario.admin)
                {
                if (pagina != 0 && cantidad != 0)
                {
                    if (estado != null) { solicitud = await _context.SolicitudDeAfiliacion.Where(c => c.Estado == estado).Skip((pagina - 1) * cantidad).Take(cantidad).ToListAsync(); }
                    else { solicitud = await _context.SolicitudDeAfiliacion.Skip((pagina - 1) * cantidad).Take(cantidad).ToListAsync(); }
                }
                else
                {
                    if (estado != null) { solicitud = await _context.SolicitudDeAfiliacion.Where(c => c.Estado == estado).ToListAsync(); }
                    else { solicitud = await _context.SolicitudDeAfiliacion.ToListAsync(); }
                }
                }
            else
            {
                solicitud.Add(await _context.SolicitudDeAfiliacion.Where(c => c.IdUsuario == usuario.Uid).OrderByDescending(c => c.Id).FirstOrDefaultAsync());
            }
            return solicitud;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<SolicitudDeAfiliacion>> GetSolicitudDeAfiliacion(int id, [FromHeader] String Authorization)
        {
            string idToken = Authorization.Remove(0, 7);
            usuario = await autenticar.obtener_usuario(idToken);

            if (usuario.admin)
            {
                var solicitud = await _context.SolicitudDeAfiliacion.FindAsync(id);
                return solicitud;
            }
            return NotFound();
        }

        // PUT: api/SolicitudDeAfiliacion/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolicitudDeAfiliacion(int id, SolicitudDeAfiliacion solicitudDeAfiliacion, [FromHeader] string Authorization)
        {
            string idToken = Authorization.Remove(0, 7);
            usuario = await autenticar.obtener_usuario(idToken);

            if (id != solicitudDeAfiliacion.Id)
            {
                return BadRequest();
            }
            if (solicitudDeAfiliacion.Estado == "Aprobado" && solicitudDeAfiliacion.Comentario == "")
            {
                solicitudDeAfiliacion.Comentario = "Bienviendo nuevo vendedor, es un placer contar contigo.";
            }
            string[] valores = imagenes(solicitudDeAfiliacion, 1); ;
            solicitudDeAfiliacion.PagareUrl = valores[0];
            solicitudDeAfiliacion.ReciboAguaUrl = valores[1];
            solicitudDeAfiliacion.ReciboLuzUrl = valores[2];
            solicitudDeAfiliacion.ReciboTelefonoUrl = valores[3];
            solicitudDeAfiliacion.ReferenciaBancariaUrl = valores[4];
            _context.Entry(solicitudDeAfiliacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolicitudDeAfiliacionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        [Route("GestionarSolicitud")]
        public async Task<IActionResult> PostSolicitudDeAfiliacion(int id, string estado, [FromHeader] string Authorization)
        {
            string idToken = Authorization.Remove(0, 7);
            usuario = await autenticar.obtener_usuario(idToken);
            var solicitudDeAfiliacion = new SolicitudDeAfiliacion();
            solicitudDeAfiliacion = await _context.SolicitudDeAfiliacion.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (id != solicitudDeAfiliacion.Id)
            {
                return BadRequest();
            }
            //if (estado == "Aprobado" && solicitudDeAfiliacion.Comentario == "")
            if (estado == "Aprobado" )
            {
                solicitudDeAfiliacion.Estado = "Aprobado";
                solicitudDeAfiliacion.Comentario = "Bienviendo nuevo vendedor, es un placer contar contigo.";
            }
            else if (solicitudDeAfiliacion.Estado == "Denegado" && solicitudDeAfiliacion.Comentario == "")
            {
                solicitudDeAfiliacion.Comentario = "Su solicitud ha sido denegada";
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolicitudDeAfiliacionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

            // POST: api/SolicitudDeAfiliacion
            // To protect from overposting attacks, enable the specific properties you want to bind to, for
            // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
            [HttpPost]
        public async Task<ActionResult<SolicitudDeAfiliacion>> PostSolicitudDeAfiliacion(SolicitudDeAfiliacion solicitudDeAfiliacion, [FromHeader] string Authorization)
        {
            string idToken = Authorization.Remove(0, 7);
            usuario = await autenticar.obtener_usuario(idToken);

            string[] valores = imagenes(solicitudDeAfiliacion, 0); ;
            solicitudDeAfiliacion.PagareUrl = valores[0];
            solicitudDeAfiliacion.ReciboAguaUrl = valores[1];
            solicitudDeAfiliacion.ReciboLuzUrl = valores[2];
            solicitudDeAfiliacion.ReciboTelefonoUrl = valores[3];
            solicitudDeAfiliacion.ReferenciaBancariaUrl = valores[4];
            solicitudDeAfiliacion.IdUsuario = usuario.Uid;
            _context.SolicitudDeAfiliacion.Add(solicitudDeAfiliacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSolicitudDeAfiliacion", new { id = solicitudDeAfiliacion.Id }, solicitudDeAfiliacion);
        }

        // DELETE: api/SolicitudDeAfiliacion/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SolicitudDeAfiliacion>> DeleteSolicitudDeAfiliacion(int id, [FromHeader] string Authorization)
        {
            var solicitudDeAfiliacion = await _context.SolicitudDeAfiliacion.FindAsync(id);
            if (solicitudDeAfiliacion == null)
            {
                return NotFound();
            }

            imagenes(solicitudDeAfiliacion, 2);
            _context.SolicitudDeAfiliacion.Remove(solicitudDeAfiliacion);
            await _context.SaveChangesAsync();

            return solicitudDeAfiliacion;
        }

        private bool SolicitudDeAfiliacionExists(int id)
        {
            return _context.SolicitudDeAfiliacion.Any(e => e.Id == id);
        }


        private string[] imagenes(SolicitudDeAfiliacion solicitud, int modo)
        {

            string filtePath = Path.GetFullPath(@"Images/DocumentosSolicitud");
            if (modo > 0)
            {
                SolicitudDeAfiliacion imagen_cambiar = _context.SolicitudDeAfiliacion.Find(solicitud.Id);
                string[] rutas = new string[] { imagen_cambiar.PagareUrl, imagen_cambiar.ReciboAguaUrl, imagen_cambiar.ReciboLuzUrl, imagen_cambiar.ReciboTelefonoUrl, imagen_cambiar.ReferenciaBancariaUrl };
                for (int i = 0; i < 5; i++)
                {
                    string ruta_seleccionada = rutas[i];
                    if (ruta_seleccionada != "" && ruta_seleccionada != null && ruta_seleccionada.Length > 38)
                    {
                        //Es importante contar cuantos caracteres tiene antes de la ultima pleca en la ruta almacenada en base de datos
                        ruta_seleccionada = ruta_seleccionada.Remove(0, 38);
                        System.IO.File.Delete(Path.Join(filtePath, ruta_seleccionada));
                    }
                }

                _context.Entry(imagen_cambiar).State = EntityState.Detached;
            }
            if (modo < 2)
            {

                //string imagenBase = solicitud.ImagenUrl;
                string[] rutas = new string[] { solicitud.PagareUrl, solicitud.ReciboAguaUrl, solicitud.ReciboLuzUrl, solicitud.ReciboTelefonoUrl, solicitud.ReferenciaBancariaUrl };
                string[] valores_documentos = new string[5];
                for (int i = 0; i < 5; i++)
                {
                    //Agregando imagen a carpeta
                    Guid nombreImagen = Guid.NewGuid();
                    string rutaImagen = Path.Join(filtePath, nombreImagen + ".png");
                    string ruta_base = rutas[i];
                    if (ruta_base != "" && ruta_base != null)
                    {
                        //imagenBase = imagenBase.RutaImagem.Remove(0, 22);
                        byte[] archivoBase64 = Convert.FromBase64String(ruta_base);
                        System.IO.File.WriteAllBytes(rutaImagen, archivoBase64);

                        valores_documentos[i] = "/Images/DocumentosSolicitud/" + nombreImagen + ".png";

                    }
                }
                return valores_documentos;

            }

            string[] retorno = new string[0];
            return retorno;
        }


    }
}
