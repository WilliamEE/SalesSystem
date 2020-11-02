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
        public async Task<ActionResult<IEnumerable<SolicitudDeAfiliacion>>> GetSolicitudDeAfiliacion([FromQuery] int pagina, [FromQuery] int cantidad, [FromHeader] string Authorization)
        {
            string idToken = Authorization.Remove(0, 7);
            usuario = await autenticar.obtener_usuario(idToken);

            List<SolicitudDeAfiliacion> solicitud = new List<SolicitudDeAfiliacion>();
                if (usuario.admin)
                {
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
                else
                {
                //solicitud = await _context.SolicitudDeAfiliacion.Where(c =>c.id_Usuario == usuario.Uid).FirstOrDefaultAsync();
                return solicitud;
                }
        }
        // GET: api/SolicitudDeAfiliacion/5
        [HttpGet("porId")]
        public async Task<ActionResult<SolicitudDeAfiliacion>> GetSolicitudDeAfiliacionId([FromQuery] string token)
        {
            string idToken = token;
            //string idToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjFlNjYzOGY4NDlkODVhNWVkMGQ1M2NkNDI1MzE0Y2Q1MGYwYjY1YWUiLCJ0eXAiOiJKV1QifQ.eyJuYW1lIjoiQ2FybG9zIiwiaXNzIjoiaHR0cHM6Ly9zZWN1cmV0b2tlbi5nb29nbGUuY29tL2RzaTIxNSIsImF1ZCI6ImRzaTIxNSIsImF1dGhfdGltZSI6MTYwMTE0ODAwOSwidXNlcl9pZCI6IjFieVVuMnV2WFhNWTdJeWpMZExBNDVNT2hMUzIiLCJzdWIiOiIxYnlVbjJ1dlhYTVk3SXlqTGRMQTQ1TU9oTFMyIiwiaWF0IjoxNjAxMjQzNzYzLCJleHAiOjE2MDEyNDczNjMsImVtYWlsIjoibW9yYW5fa3Jsb3NAaG90bWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZW1haWwiOlsibW9yYW5fa3Jsb3NAaG90bWFpbC5jb20iXX0sInNpZ25faW5fcHJvdmlkZXIiOiJwYXNzd29yZCJ9fQ.UkvIe3U4insA6MyA4bqXamggpdjEAfMJANxlC4_tC2fKGn2nPnFW9uirzNZ6j7bfHUVoF0usryVmV1C_Nfia3esboZTyRvoPMW2_9tdeSwl_ah4pQTef8FpAjqX1xtKRRv2UX7zaJOvWboKaL8OhEcdwhrYdeOF2AfrBkBOIHYYgInmVjs3m2EWgRVFtSuhbX7EJ8qRRdg31Y2c-GaKjg_CpXpy5XRkdLqwuhWZWOH9ZtvdkTkExj2xrnkfMbinOqOohrl1zJDNl5nBiDyCHYDLI_hnNT57KUbgNbNkPcW-e5k2OsnvbZHCGc39XVlkBZLwvchoV4Huj0RGAJVFnFA";
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            string uid = decodedToken.Uid;

            //var solicitudDeAfiliacion = await _context.SolicitudDeAfiliacion.FindAsync(id);
            var solicitudDeAfiliacion = await _context.SolicitudDeAfiliacion.Where(c => c.id_Usuario == uid).FirstOrDefaultAsync();
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
            string idToken = solicitudDeAfiliacion.id_Usuario;
            //string idToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjFlNjYzOGY4NDlkODVhNWVkMGQ1M2NkNDI1MzE0Y2Q1MGYwYjY1YWUiLCJ0eXAiOiJKV1QifQ.eyJuYW1lIjoiQ2FybG9zIiwiaXNzIjoiaHR0cHM6Ly9zZWN1cmV0b2tlbi5nb29nbGUuY29tL2RzaTIxNSIsImF1ZCI6ImRzaTIxNSIsImF1dGhfdGltZSI6MTYwMTE0ODAwOSwidXNlcl9pZCI6IjFieVVuMnV2WFhNWTdJeWpMZExBNDVNT2hMUzIiLCJzdWIiOiIxYnlVbjJ1dlhYTVk3SXlqTGRMQTQ1TU9oTFMyIiwiaWF0IjoxNjAxMjQzNzYzLCJleHAiOjE2MDEyNDczNjMsImVtYWlsIjoibW9yYW5fa3Jsb3NAaG90bWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZW1haWwiOlsibW9yYW5fa3Jsb3NAaG90bWFpbC5jb20iXX0sInNpZ25faW5fcHJvdmlkZXIiOiJwYXNzd29yZCJ9fQ.UkvIe3U4insA6MyA4bqXamggpdjEAfMJANxlC4_tC2fKGn2nPnFW9uirzNZ6j7bfHUVoF0usryVmV1C_Nfia3esboZTyRvoPMW2_9tdeSwl_ah4pQTef8FpAjqX1xtKRRv2UX7zaJOvWboKaL8OhEcdwhrYdeOF2AfrBkBOIHYYgInmVjs3m2EWgRVFtSuhbX7EJ8qRRdg31Y2c-GaKjg_CpXpy5XRkdLqwuhWZWOH9ZtvdkTkExj2xrnkfMbinOqOohrl1zJDNl5nBiDyCHYDLI_hnNT57KUbgNbNkPcW-e5k2OsnvbZHCGc39XVlkBZLwvchoV4Huj0RGAJVFnFA";
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            string uid = decodedToken.Uid;

            string[] valores = imagenes(solicitudDeAfiliacion, 0); ;
            solicitudDeAfiliacion.PagareUrl = valores[0];
            solicitudDeAfiliacion.ReciboAguaUrl = valores[1];
            solicitudDeAfiliacion.ReciboLuzUrl = valores[2];
            solicitudDeAfiliacion.ReciboTelefonoUrl = valores[3];
            solicitudDeAfiliacion.ReferenciaBancariaUrl = valores[4];
            solicitudDeAfiliacion.id_Usuario = uid;
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

        // DELETE: api/SolicitudDeAfiliacion/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SolicitudDeAfiliacion>> AprobarDenegarSolicitud(int id)
        {
            //string idToken = solicitudDeAfiliacion.id_Usuario;
            string idToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjFlNjYzOGY4NDlkODVhNWVkMGQ1M2NkNDI1MzE0Y2Q1MGYwYjY1YWUiLCJ0eXAiOiJKV1QifQ.eyJuYW1lIjoiQ2FybG9zIiwiaXNzIjoiaHR0cHM6Ly9zZWN1cmV0b2tlbi5nb29nbGUuY29tL2RzaTIxNSIsImF1ZCI6ImRzaTIxNSIsImF1dGhfdGltZSI6MTYwMTE0ODAwOSwidXNlcl9pZCI6IjFieVVuMnV2WFhNWTdJeWpMZExBNDVNT2hMUzIiLCJzdWIiOiIxYnlVbjJ1dlhYTVk3SXlqTGRMQTQ1TU9oTFMyIiwiaWF0IjoxNjAxMjQzNzYzLCJleHAiOjE2MDEyNDczNjMsImVtYWlsIjoibW9yYW5fa3Jsb3NAaG90bWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZW1haWwiOlsibW9yYW5fa3Jsb3NAaG90bWFpbC5jb20iXX0sInNpZ25faW5fcHJvdmlkZXIiOiJwYXNzd29yZCJ9fQ.UkvIe3U4insA6MyA4bqXamggpdjEAfMJANxlC4_tC2fKGn2nPnFW9uirzNZ6j7bfHUVoF0usryVmV1C_Nfia3esboZTyRvoPMW2_9tdeSwl_ah4pQTef8FpAjqX1xtKRRv2UX7zaJOvWboKaL8OhEcdwhrYdeOF2AfrBkBOIHYYgInmVjs3m2EWgRVFtSuhbX7EJ8qRRdg31Y2c-GaKjg_CpXpy5XRkdLqwuhWZWOH9ZtvdkTkExj2xrnkfMbinOqOohrl1zJDNl5nBiDyCHYDLI_hnNT57KUbgNbNkPcW-e5k2OsnvbZHCGc39XVlkBZLwvchoV4Huj0RGAJVFnFA";
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            string uid = decodedToken.Uid;
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
            return retorno;
        }


    }
}
