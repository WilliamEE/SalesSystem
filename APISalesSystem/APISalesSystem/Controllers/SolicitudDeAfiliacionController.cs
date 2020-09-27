using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APISalesSystem;

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
        public async Task<ActionResult<IEnumerable<SolicitudDeAfiliacion>>> GetSolicitudDeAfiliacion()
        {
            return await _context.SolicitudDeAfiliacion.ToListAsync();
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

            _context.SolicitudDeAfiliacion.Remove(solicitudDeAfiliacion);
            await _context.SaveChangesAsync();

            return solicitudDeAfiliacion;
        }

        private bool SolicitudDeAfiliacionExists(int id)
        {
            return _context.SolicitudDeAfiliacion.Any(e => e.Id == id);
        }
    }
}
