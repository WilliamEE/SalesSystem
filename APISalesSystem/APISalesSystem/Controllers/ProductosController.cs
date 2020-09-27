using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APISalesSystem;
using System.IO;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace APISalesSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly DbSalesSystemContext _context;

        public ProductosController(DbSalesSystemContext context)
        {
            _context = context;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProducto([FromQuery] int pagina, [FromQuery] int cantidad)
        {
            //return await _context.Producto.ToListAsync();

            List<Producto> documentoLegal;
            if (pagina != 0 && cantidad != 0)
            {
                documentoLegal = await _context.Producto.Skip((pagina - 1) * cantidad).Take(cantidad).ToListAsync();
            }
            else
            {
                documentoLegal = await _context.Producto.ToListAsync();
            }
            return documentoLegal;
        }

        // GET: api/Productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Producto.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }

        // PUT: api/Productos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest();
            }

            producto.ImagenUrl = imagenes(producto, 1);
            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
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

        // POST: api/Productos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            producto.ImagenUrl = imagenes(producto, 0);
            _context.Producto.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProducto", new { id = producto.Id }, producto);
        }

        // DELETE: api/Productos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Producto>> DeleteProducto(int id)
        {
            var producto = await _context.Producto.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            imagenes(producto, 2);
            _context.Producto.Remove(producto);
            await _context.SaveChangesAsync();

            return producto;
        }

        private bool ProductoExists(int id)
        {
            return _context.Producto.Any(e => e.Id == id);
        }

        private string imagenes(Producto producto, int modo)
        {

            string filtePath = Path.GetFullPath(@"Images/Productos");
            if (modo > 0)
            {
                Producto imagen_cambiar = _context.Producto.Find(producto.Id);
                string ruta_imagen_eliminar = imagen_cambiar.ImagenUrl;
                if (ruta_imagen_eliminar != "" && ruta_imagen_eliminar != null && ruta_imagen_eliminar.Length > 38)
                {
                    //Es importante contar cuantos caracteres tiene antes de la ultima pleca en la ruta almacenada en base de datos
                    ruta_imagen_eliminar = ruta_imagen_eliminar.Remove(0, 38);
                    System.IO.File.Delete(filtePath + "\\" + ruta_imagen_eliminar);
                }
                _context.Entry(imagen_cambiar).State = EntityState.Detached;
            }
            if (modo < 2)
            {
                //Agregando imagen a carpeta
                //string nombreImagen = producto.Nombre.Replace(" ", "");
                Guid nombreImagen = Guid.NewGuid();
                string rutaImagen = filtePath + "\\" + nombreImagen + ".png";
                string imagenBase = producto.ImagenUrl;
                if (imagenBase != "" && imagenBase != null)
                {
                    //imagenBase = imagenBase.RutaImagem.Remove(0, 22);
                    byte[] archivoBase64 = Convert.FromBase64String(imagenBase);
                    System.IO.File.WriteAllBytes(rutaImagen, archivoBase64);

                    return "/Images/Productos/" + nombreImagen + ".png";
                }
            }

            return "";
        }
    }
}
