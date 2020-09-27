using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APISalesSystem;
using System.IO;

namespace APISalesSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudDeAfiliacionController : ControllerBase
    {
        private readonly DbSalesSystemContext _context;

        public SolicitudDeAfiliacionController(DbSalesSystemContext context)
        {
            _context = context;
        }

        // GET: api/SolicitudDeAfiliacion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SolicitudDeAfiliacion>>> GetSolicitudDeAfiliacion([FromQuery] int pagina, [FromQuery] int cantidad)
        {
            List<SolicitudDeAfiliacion> solicitud;
            if (pagina != 0 && cantidad != 0)
            {
                solicitud = await _context.SolicitudDeAfiliacion.Skip((pagina - 1) * cantidad).Take(cantidad).ToListAsync();
            }
            else
            {
                solicitud = await _context.SolicitudDeAfiliacion.ToListAsync();
            }
            return solicitud;
        }

        // GET: api/SolicitudDeAfiliacion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SolicitudDeAfiliacion>> GetSolicitudDeAfiliacion(int id)
        {
            var solicitudDeAfiliacion = await _context.SolicitudDeAfiliacion.FindAsync(id);

            if (solicitudDeAfiliacion == null)
            {
                return NotFound();
            }

            return solicitudDeAfiliacion;
        }

        // PUT: api/SolicitudDeAfiliacion/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolicitudDeAfiliacion(int id, SolicitudDeAfiliacion solicitudDeAfiliacion)
        {
            if (id != solicitudDeAfiliacion.Id)
            {
                return BadRequest();
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

        // POST: api/SolicitudDeAfiliacion
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SolicitudDeAfiliacion>> PostSolicitudDeAfiliacion(SolicitudDeAfiliacion solicitudDeAfiliacion)
        {
            string[] valores = imagenes(solicitudDeAfiliacion, 1); ;
            solicitudDeAfiliacion.PagareUrl = valores[0];
            solicitudDeAfiliacion.ReciboAguaUrl = valores[1];
            solicitudDeAfiliacion.ReciboLuzUrl = valores[2];
            solicitudDeAfiliacion.ReciboTelefonoUrl = valores[3];
            solicitudDeAfiliacion.ReferenciaBancariaUrl = valores[4];
            _context.SolicitudDeAfiliacion.Add(solicitudDeAfiliacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSolicitudDeAfiliacion", new { id = solicitudDeAfiliacion.Id }, solicitudDeAfiliacion);
        }

        // DELETE: api/SolicitudDeAfiliacion/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SolicitudDeAfiliacion>> DeleteSolicitudDeAfiliacion(int id)
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
                        System.IO.File.Delete(filtePath + "\\" + ruta_seleccionada);
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
                    string rutaImagen = filtePath + "\\" + nombreImagen + ".png";
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
            return retorno ;
        }


    }
}
