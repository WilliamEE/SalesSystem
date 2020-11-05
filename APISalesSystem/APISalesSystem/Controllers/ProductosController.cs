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
using FirebaseAdmin.Auth;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Authorization;

namespace APISalesSystem.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductosController : ControllerBase
    {
        UsuarioFirebaseDecodificado autenticar = new UsuarioFirebaseDecodificado();
        UsuarioFirebase usuario = new UsuarioFirebase();
        int id_cambio;
        private readonly DbSalesSystemContext _context;

        public ProductosController(DbSalesSystemContext context)
        {
            _context = context;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProducto([FromQuery] int pagina, [FromQuery] int cantidad, [FromQuery] int categoria, [FromHeader] String Authorization)
        {
            string idToken = Authorization.Remove(0, 7);
            usuario = await autenticar.obtener_usuario(idToken);
            List<Producto> producto = new List<Producto>();
            //if (usuario.admin)
            //{
            if (pagina != 0 && cantidad != 0)
            {
                producto = await _context.Producto.Where(c => c.Activo == true).Skip((pagina - 1) * cantidad).Take(cantidad).ToListAsync();
            }
            else
            {
                producto = await _context.Producto.Where(c => c.Activo == true).ToListAsync();
            }

            if (categoria != 0)
            {
                producto = producto.Where(data => data.Activo == true && data.IdCategoria.ToString().Contains(categoria.ToString())).ToList();
            }
            return producto;
        }

        [HttpGet]
        [Route("ProductoVendedor")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductoVendedor([FromQuery] int pagina, [FromQuery] int cantidad, [FromQuery] int categoria, [FromHeader] String Authorization)
        {
            string idToken = Authorization.Remove(0, 7);
            usuario = await autenticar.obtener_usuario(idToken);

            List<Producto> producto = new List<Producto>();
            
                if (pagina != 0 && cantidad != 0)
                {
                    producto = await _context.Producto.Where(c => c.IdUsuario == usuario.Uid && c.Activo == true).Skip((pagina - 1) * cantidad).Take(cantidad).ToListAsync();
                }
                else
                {
                    producto = await _context.Producto.Where(c => c.IdUsuario == usuario.Uid && c.Activo == true).ToListAsync();
                }

                if (categoria != 0)
                {
                    producto = producto.Where(data => data.IdUsuario == usuario.Uid && data.Activo == true && data.IdCategoria.ToString().Contains(categoria.ToString())).ToList();
                }
            return producto;
        }

        [HttpGet]
        [Route("ProductosPendientes")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductosPendientes([FromQuery] int pagina, [FromQuery] int cantidad, [FromQuery] int categoria, [FromHeader] String Authorization)
        {
            string idToken = Authorization.Remove(0, 7);
            usuario = await autenticar.obtener_usuario(idToken);

            List<Producto> producto = new List<Producto>();

            if (pagina != 0 && cantidad != 0)
            {
                producto = await _context.Producto.Where(c => c.Activo == false).Skip((pagina - 1) * cantidad).Take(cantidad).ToListAsync();
            }
            else
            {
                producto = await _context.Producto.Where(c => c.Activo == false).ToListAsync();
            }

            if (categoria != 0)
            {
                producto = producto.Where(data => data.Activo == false && data.IdCategoria.ToString().Contains(categoria.ToString())).ToList();
            }
            return producto;
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            _context.Producto.Add(producto);
            imagenes(producto, 0);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction("GetProducto", new { id = producto.Id }, producto);
        }

        [HttpPost]
        [Route("ProcesarProducto")]
        public async Task<ActionResult<Producto>> PostProcesarProducto(int id, string estado)
        {
            var producto = new Producto();
            var productoModificar = new Producto();
            var productoEliminar = new Producto();
            var solicitud = new SolicitudProducto();
            int prodModificar = 0, prodEliminar = 0 ;
            string tipo = "";

            //tipo = producto.SolicitudProductoIdProductoNavigation.First().Tipo;
            solicitud = await actualizarSolicitud(id, estado);
            tipo = solicitud.Tipo;
            
            producto = await _context.Producto.Where(c => c.Activo == false && c.Id == id).FirstOrDefaultAsync();
            if ((bool)producto.Activo == false && tipo == "Agregar" && estado == "Aprobado")
            {
                producto.Activo = true;
                _context.Entry(producto).State = EntityState.Modified;
            }
            else if (tipo == "Modificar" && estado == "Aprobado")
            {
                prodModificar = (int)solicitud.IdProductoModificar;
                productoModificar = await _context.Producto.Where(c => c.Activo == true && c.Id == prodModificar).FirstOrDefaultAsync();

                productoModificar.IdCategoria = producto.IdCategoria;
                productoModificar.ImagenUrl = producto.ImagenUrl;
                productoModificar.Nombre = producto.Nombre;
                productoModificar.Precio = producto.Precio;
                _context.Entry(productoModificar).State = EntityState.Modified;
                imagenes(producto, 2);
                _context.Producto.Remove(producto);
            }
            else if (tipo == "Eliminar" && estado == "Aprobado")
            {
                //cuando soliciten eliminacion debe de mostrarse el producto en el dashboard
                prodEliminar = id_cambio;
                productoEliminar = await _context.Producto.Where(c => c.Activo == true && c.Id == prodEliminar).FirstOrDefaultAsync();
                imagenes(producto, 2);
                imagenes(productoEliminar, 2);
                _context.Producto.Remove(producto);
                _context.Producto.Remove(productoEliminar);
            }
            else if (estado == "Denegado")
            {
                imagenes(producto, 2);
                _context.Producto.Remove(producto);
            }
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

        // GET: api/Productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Producto.Where(c => c.IdUsuario == usuario.Uid && c.Activo == true && c.Id == id).FirstOrDefaultAsync();

            if (producto == null)
            {
                return NotFound();
            }

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

        public async Task<SolicitudProducto> actualizarSolicitud(int id, string estado) 
        {
            
            var solicitud = await _context.SolicitudProducto.Where(c => c.IdProducto == id).FirstAsync();
            id_cambio = (int)solicitud.IdProductoModificar;
            string tipo = solicitud.Tipo;

            solicitud.Estado = estado;

            _context.Entry(solicitud).State = EntityState.Modified;

            if (estado == "Aprobado" && tipo == "Agregar")
            {
                solicitud.Comentario = "Se agregó el producto " + id + " correctamente";
            }
            else if (estado == "Aprobado" && tipo == "Modificar")
            {
                solicitud.IdProducto = null;
                solicitud.Comentario = "Se modificó el producto " + id_cambio + " correctamente";
            }
            else if (estado == "Aprobado" && tipo == "Eliminar")
            {
                solicitud.IdProducto = null;
                solicitud.IdProductoModificar = 0;
                solicitud.Comentario = "Se elimino el producto " + id_cambio + " correctamente";
            }
            else if (estado == "Denegado")
            {
                solicitud.IdProducto = null;
                solicitud.IdProductoModificar = 0;
                if (tipo == "Agregar") { solicitud.Comentario = "Se denegó la acción sobre el producto " + id; }
                if (tipo == "Modificar" || tipo == "Eliminar") { solicitud.Comentario = "Se denegó la acción sobre el producto " + id_cambio; }
            }

            try
            {
                await _context.SaveChangesAsync();
                return solicitud;
            }
            catch (DbUpdateConcurrencyException)
            {
                return solicitud;
            }
        }
    }
}
