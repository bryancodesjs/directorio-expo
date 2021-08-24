using DirectorioCreativo.Datos;
using DirectorioCreativo.Entidades.Models;
using DirectorioCreativo.Web.Models.PerfilesUsuario;
using DirectorioCreativo.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;



namespace DirectorioCreativo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilUsuarioController : ControllerBase
    {
        private readonly DbContextDirectorioCreactivo _context;
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;

        public PerfilUsuarioController(DbContextDirectorioCreactivo context, IConfiguration config, IHostingEnvironment env)
        {
            _context = context;
            _config = config;
            _env = env;
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("[action]")]
        public async Task<IActionResult> TicketsPendientes()
        {            
            var tickets = await (from u in _context.Usuarios
                                 join p in _context.PerfilUsuarios on u.Id equals p.IdUsuario
                                 join sp in _context.SolicitudPerfilUsuarios on p.IdUsuario equals sp.IdUsuario
                                 where sp.Estado_solicitud == null
                                 select new
                                 {
                                     artista = u.Nombre + ' ' + u.Apellido,
                                     p.Img_perfil,
                                     solicitud_cambios = sp
                                 }).ToListAsync();

            return Ok( new { tickets });
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> CambiarClave([FromBody] CambiarClaveViewModel model)
        {
            // SE OBTIENE EL ID DEL USUARIO AUTENTICADO
            var userClaims = HttpContext.User.Claims.ToList();
            var userId = userClaims[0].Value.ToString();

            var user = await _context.Usuarios.Where(x => x.Id == Convert.ToInt32( userId) ).FirstOrDefaultAsync();

            // SE CONVIERTE A UN ARRAY DE BYTE LA SALT 
            var token = Convert.FromBase64String(user.SaltClave);

            // SE DESCIFRA LA CLAVE QUE LLEGA POR POST PARA PODER COMPARARLA CON LA CLAVE DE BASE DE DATOS
            var _claveEncryptada = Descifrar._DesifrarClave(model.Actual, token);

            if (_claveEncryptada == user.Clave )
            {
                var infoClave = Encriptar.EncryptarClave( model.Nueva );

                user.SaltClave = Convert.ToBase64String((byte[])infoClave[0]);
                user.Clave = infoClave[1].ToString();

                try
                {
                    _context.Entry(user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return Ok(new { status = 200 });
                }
                catch (Exception ex)
                {
                    return NotFound(ex);
                }                
            }
            else 
            {
                return NotFound(new { message = "Clave incorrecta"} );
            }            
        }

        [Authorize]
        [HttpPut("[action]")]
        public async Task<IActionResult> ActualizarPerfil([FromBody] SolicitudPerfilUsuarioViewModel model)
        {
            // SE OBTIENE EL ID DEL USUARIO AUTENTICADO
            var user = HttpContext.User.Claims.ToList();
            var userId = user[0].Value.ToString();


            var solicitud = new SolicitudPerfilUsuario();
            var nombre = "(" + userId + ")" + "_" + DateTime.Now.ToString("MM-dd-yy H mm ss").Replace(" ", "_") + ".png";

            // PRIMERO SE AGREGA EL ID DEL ARTISTA QUE VA A REALIZAR LA SOLICITUD DEL CAMBIO
            solicitud.IdUsuario = Convert.ToInt32(userId);
            solicitud.Estado_solicitud = null;
            solicitud.Fecha_solicitud = DateTime.Now;

            if (!string.IsNullOrEmpty(model.Img_Banner))
            {
                // CODIGOS PARA ELIMINAR DATOS QUE NO SEAN DEL STRING BASE 64
                var primerIndex = model.Img_Banner.IndexOf(",");
                primerIndex += 1;
                var lastIndex = model.Img_Banner.Length;

                string base64 = model.Img_Banner.Substring(primerIndex, lastIndex - primerIndex);

                try
                {
                    Servicio(base64, nombre);
                    solicitud.ImgBanner = nombre;

                    await _context.SolicitudPerfilUsuarios.AddAsync(solicitud);
                    await _context.SaveChangesAsync();
                    solicitud.ImgBanner = null;
                    solicitud.Id = 0;
                }
                catch (Exception ex)
                {
                    return NotFound(ex);
                }
            }

            if (!string.IsNullOrEmpty(model.Img_Perfil))
            {
                // CODIGOS PARA ELIMINAR DATOS QUE NO SEAN DEL STRING BASE 64
                var primerIndex = model.Img_Perfil.IndexOf(",");
                primerIndex += 1;
                var lastIndex = model.Img_Perfil.Length;

                string base64 = model.Img_Perfil.Substring(primerIndex, lastIndex - primerIndex);

                try
                {
                    Servicio(base64, nombre);
                    solicitud.Img_perfil = nombre;

                    await _context.SolicitudPerfilUsuarios.AddAsync(solicitud);
                    await _context.SaveChangesAsync();
                    solicitud.Img_perfil = null;
                    solicitud.Id = 0;
                }
                catch (Exception ex)
                {
                    return NotFound(ex);
                }
            }

            var infoActual = await (from u in _context.Usuarios
                                    join p in _context.PerfilUsuarios on u.Id equals p.IdUsuario
                                    where u.Id == Convert.ToInt32(userId)
                                    select new
                                    {
                                        u.Nombre,
                                        u.Apellido,
                                        u.Profesion,
                                        u.DescripcionGeneral,
                                        u.Email,
                                        u.Telefono,
                                        p.Facebook,
                                        p.Instagram,
                                        p.Youtbe,
                                        p.Img_perfil,
                                        p.ImgBanner
                                    }).FirstOrDefaultAsync();            

            try
            {

                // SE PROCEDEN A VERIFICAR QUE LOS CAMPOS NO SEAN LOS MISMOS PARA QUE NO SE REPITAN
                if (infoActual.Nombre != model.Nombre && model.Nombre != null)
                {
                    solicitud.Nombre = model.Nombre;

                    await _context.SolicitudPerfilUsuarios.AddAsync(solicitud);
                    await _context.SaveChangesAsync();
                    solicitud.Nombre = null;
                    solicitud.Id = 0;
                }

                if (infoActual.Apellido != model.Apellido && model.Apellido != null)
                {
                    solicitud.Apellido = model.Apellido;

                    await _context.SolicitudPerfilUsuarios.AddAsync(solicitud);
                    await _context.SaveChangesAsync();
                }

                if (infoActual.Profesion != model.Profesion && model.Profesion != null)
                {
                    solicitud.Profesion = model.Profesion;

                    await _context.SolicitudPerfilUsuarios.AddAsync(solicitud);
                    await _context.SaveChangesAsync();
                    //_context.Database.CloseConnection();
                    solicitud.Profesion = null;
                    solicitud.Id = 0;
                }

                if (infoActual.DescripcionGeneral != model.Descripcion && model.Descripcion != null)
                {
                    solicitud.Descripcion = model.Descripcion;

                    await _context.SolicitudPerfilUsuarios.AddAsync(solicitud);
                    await _context.SaveChangesAsync();
                    solicitud.Descripcion = null;
                    solicitud.Id = 0;
                }

                if (infoActual.Facebook != model.Facebook && model.Facebook != null)
                {
                    solicitud.Facebook = model.Facebook;

                    await _context.SolicitudPerfilUsuarios.AddAsync(solicitud);
                    await _context.SaveChangesAsync();
                    solicitud.Facebook = null;
                    solicitud.Id = 0;
                }

                if (infoActual.Instagram != model.Instagram && model.Instagram != null)
                {
                    solicitud.Facebook = model.Instagram;

                    await _context.SolicitudPerfilUsuarios.AddAsync(solicitud);
                    await _context.SaveChangesAsync();
                    solicitud.Instagram = null;
                    solicitud.Id = 0;
                }

                if (infoActual.Youtbe != model.Youtube && model.Youtube != null)
                {
                    solicitud.Youtbe = model.Youtube;

                    await _context.SolicitudPerfilUsuarios.AddAsync(solicitud);
                    await _context.SaveChangesAsync();
                    solicitud.Youtbe = null;
                    solicitud.Id = 0;
                }

                if (infoActual.Telefono != model.Telefono && model.Telefono != null)
                {
                    solicitud.Telefono = model.Telefono;

                    await _context.SolicitudPerfilUsuarios.AddAsync(solicitud);
                    await _context.SaveChangesAsync();
                    solicitud.Telefono = null;
                    solicitud.Id = 0;
                }

                if (infoActual.Email != model.Email && model.Email != null)
                {
                    solicitud.Email = model.Email;

                    await _context.SolicitudPerfilUsuarios.AddAsync(solicitud);
                    await _context.SaveChangesAsync();
                    solicitud.Email = null;
                    solicitud.Id = 0;
                }

                return Ok(new { status = 200 });

            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }

        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("[action]")]
        public async Task<IActionResult> RechazarCambio([FromBody] SolicitudPerfilUsuario_BackOffice model)
        {
            var user = HttpContext.User.Claims.ToList();
            var userId = Convert.ToInt32(user[0].Value.ToString());

            bool _cambioArtista = false;
            bool _cambioPerfil = false;

            //PerfilUsuario perfil = new PerfilUsuario();
            
            var solicitud = await _context.SolicitudPerfilUsuarios.FindAsync(model.Id);
            var perfil = await _context.PerfilUsuarios.Where(u => u.IdUsuario == solicitud.IdUsuario).FirstOrDefaultAsync();
            var artista = await _context.Usuarios.FindAsync(solicitud.IdUsuario);
            solicitud.Estado_solicitud = model.Accion;            

            // SI SE APRUEBAN LOS CAMBIOS
            if (model.Accion)
            {
                // SE VERIFICA QUE EL USUARIO TENGA PERMISOS
                if (await Permisos(userId, "Aceptar Tickets") == false)
                {
                    return Ok(new { status = 203 });
                }

                // SE PROCEDEN A VERIFICAR CUALES CAMPOS TIENEN CAMBIOS Y NO ESTAN VACIOS
                if (solicitud.ImgBanner != null)
                {
                    perfil.ImgBanner = solicitud.ImgBanner;
                    _cambioPerfil = true;
                }

                if (solicitud.Img_perfil != null)
                {
                    perfil.Img_perfil = solicitud.Img_perfil;
                    _cambioPerfil = true;
                }

                if (solicitud.Nombre != null)
                {
                    artista.Nombre = solicitud.Nombre;
                    _cambioArtista = true;
                }

                if (solicitud.Apellido != null)
                {
                    artista.Apellido = solicitud.Apellido;
                    _cambioArtista = true;
                }

                if (solicitud.Profesion != null)
                {
                    artista.Profesion = solicitud.Profesion;
                    _cambioArtista = true;
                }

                if (solicitud.Descripcion != null)
                {
                    artista.DescripcionGeneral = solicitud.Descripcion;
                    _cambioArtista = true;
                }

                if (solicitud.Telefono != null)
                {
                    artista.Telefono = solicitud.Telefono;
                    _cambioArtista = true;
                }

                if (solicitud.Email != null)
                {
                    artista.Email = solicitud.Email;
                    _cambioArtista = true;
                }

                if (solicitud.Facebook != null)
                {
                    perfil.Facebook = solicitud.Facebook;
                    _cambioPerfil = true;
                }

                if (solicitud.Instagram != null)
                {
                    perfil.Instagram = solicitud.Instagram;
                    _cambioPerfil = true;
                }

                if (solicitud.Youtbe != null)
                {
                    perfil.Youtbe = solicitud.Youtbe;
                    _cambioPerfil = true;
                }
            }
            else
            {
                // SE VERIFICA QUE EL USUARIO TENGA PERMISOS
                if (await Permisos(userId, "Rechazar Tickets") == false)
                {
                    return Ok(new { status = 203 });
                }

                var rechazo = new Rechazados
                {
                    Id_Violacion = model.Id_Violacion,
                    Id_Solicitud_Perfil = solicitud.Id,
                    Fecha = DateTime.Now,
                    Detalle = model.Detalle
                };

                await _context.Rechazados.AddAsync(rechazo);
            }

            try
            {
                // SE VERIFICA QUE LOS MODELOS DE PERFIL Y ARTISTA TENGAN ALGUN CAMBIO, PARA PREVENIR ERROR AL GUARDAR EN LA BASE DE DATOS
                if (_cambioPerfil)
                {
                    _context.Entry(perfil).State = EntityState.Modified;
                }

                if (_cambioArtista)
                {
                    _context.Entry(artista).State = EntityState.Modified;
                }

                _context.Entry(solicitud).State = EntityState.Modified;                
                
                await _context.SaveChangesAsync();
                return Ok(new { status = 200});
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }            
        }
        public async Task<bool> Permisos(int idUser, string permiso)
        {
            try
            {
                var acceso = await (from p in _context.UsuarioRols
                                    join urp in _context.UsuarioRolPermisos on p.Id equals urp.Id_UsuarioRol
                                    join perm in _context.Permisos on urp.IdPermiso equals perm.Id
                                    where p.IdUsuario == idUser && perm.Nombre == permiso
                                    select new
                                    {
                                        permiso = perm.Nombre
                                    }).ToListAsync();

                return acceso.Count() > 0 ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Servicio(string base64String, string nombreIMG)
        {
            // SERVIDOR
            var folderPath = Path.Combine(_env.ContentRootPath, @"C:\inetpub\wwwroot\directorio-creativo\assets\img");

            // DESARROLLO
            //var folderPath = Path.Combine(_env.ContentRootPath, @"C:\Users\martin.perez\Desktop\Ministerio de Cultura Oficial\DirectorioCreativo_Cultural\FrontEnd\src\assets\img");

            // DESARROLLO MARTIN
            //var folderPath = Path.Combine(_env.ContentRootPath, @"C:\Users\martin.perez\Desktop\Directiorio_Creativo\DirectorioCreativo_Cultural\FrontEnd\src\assets\img");

            //DESARROLLO BRYAN
            //var folderPath = Path.Combine(_env.ContentRootPath, @"C:\Users\bryan.campos\Documents\GitHub\02-08-2021\FrontEnd\src\assets\img");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            System.IO.File.WriteAllBytes(Path.Combine(folderPath, nombreIMG), Convert.FromBase64String(base64String));
        }


    }
}
