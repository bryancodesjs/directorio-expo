using DirectorioCreativo.Datos;
using DirectorioCreativo.Entidades.Models;
using DirectorioCreativo.Web.Models.PerfilesUsuario;
using DirectorioCreativo.Web.Models.Usuarios;
using DirectorioCreativo.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly DbContextDirectorioCreactivo _context;
        private readonly IConfiguration _config;

        public UsuariosController(DbContextDirectorioCreactivo context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: api/Usuarios/Listar
        //[Authorize(Roles = "Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<UsuarioViewModel>> Listar()
        {
            var usuario = await _context.Usuarios.Include(u => u.UsuarioRols).ToListAsync();

            return usuario.Select(u => new UsuarioViewModel
            {
                id = u.Id,
                id_provincia = u.IdProvincia,
                nombre = u.Nombre,
                apellido = u.Apellido,
                profesion = u.Profesion,
                descripcion_general = u.DescripcionGeneral,
                telefono = u.Telefono,
                email = u.Email,
                clave = u.Clave,
                salt_clave = u.SaltClave,
                habilitado = u.Habilitado
            });
        }

     
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PerfilUsuario perfil = new PerfilUsuario();

            var email = model.Email.ToLower();
            if (await _context.Usuarios.AnyAsync(u => u.Email == email))
            {
                return BadRequest("El email ya existe");
            }

            var _clave = Encriptar.EncryptarClave(model.Clave);
            var _salt = Convert.ToBase64String((byte[])_clave[0]);

            Usuario usuario = new Usuario
            {    
                Nombre = model.Nombre,
                Apellido = model.Apellido, 
                Telefono = model.Telefono,
                Email = model.Email.ToLower(),
                Rnc = model.RNC,
                Clave = _clave[1].ToString(),
                SaltClave = _salt,
                FechaIngreso = DateTime.Now,
                Habilitado = null,
                Bloqueado = false,
                DatosCompletado = false,
                ServiciosCompletado = false,
                ContactoCompletado = false,
                Tipo_Registro = model.tipo_registro
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            if (model.tipo_registro != "Usuario")
            {
                perfil.IdUsuario = usuario.Id;
                perfil.Visitas = 0;
                perfil.Valoraciones = 0;         

                _context.PerfilUsuarios.Add(perfil);
            } 

            UsuarioRol rol = new UsuarioRol
            {
                IdUsuario = usuario.Id,
                IdRol = 2
            };            

            try
            {                
                _context.UsuarioRols.Add(rol);
                await _context.SaveChangesAsync();

                var secretKey = _config.GetValue<string>("SecretKey");
                var key = Encoding.ASCII.GetBytes(secretKey);

                var claims = new ClaimsIdentity();
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()));

                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(10),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenCreated = tokenHandler.CreateToken(tokenDescription);

                string bearer = tokenHandler.WriteToken(tokenCreated);
                string usuarioActual = usuario.Nombre + ' ' + usuario.Apellido;

                return Ok(new { codeStatus = 200, token = bearer, id = usuario.Id, usuarioActual, redirect = model.tipo_registro == "Usuario" ? "/" : "/dashboard" });

            }
            catch (Exception ex)
            {
                return BadRequest(new { codeStatus = 400, message = ex });
            }
        }

        
        [HttpPost("[action]")]
        public async Task<IActionResult> InfoUser([FromBody] IdUsuario model)
        {
            var userInfo = from u in _context.Usuarios
                           join p in _context.PerfilUsuarios on u.Id equals p.IdUsuario
                           where u.Id == Convert.ToInt32(model.Id)
                           select new
                           {
                               artista = u.Nombre + ' ' + u.Apellido,
                               nombre = u.Nombre,
                               u.Profesion,
                               u.DescripcionGeneral,
                               u.FechaIngreso,
                               u.Habilitado,
                               p.Visitas,
                               p.Valoraciones,
                               p.Instagram,
                               p.Facebook,
                               p.Youtbe,                               
                               p.Img_perfil,
                               p.ImgBanner
                           };

            return Ok(await userInfo.FirstOrDefaultAsync());
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<IActionResult> InfoArtista()
        {
            var user = HttpContext.User.Claims.ToList();
            var userId = Convert.ToInt32(user[0].Value.ToString());

            // SE OBTIENE TODA LA INFORMACION DEL ARTISTA PARA MOSTRARLA EN EL MODULO DE CONFIGURACION
            var userInfo = await (from u in _context.Usuarios
                                  join p in _context.PerfilUsuarios on u.Id equals p.IdUsuario
                                  where u.Id == userId
                                  select new
                                  {
                                      u.Id,
                                      artista = u.Nombre + ' ' + u.Apellido,
                                      u.Id_Provincias,
                                      u.Id_Municipio,
                                      u.Nombre,
                                      u.Apellido,
                                      u.DescripcionGeneral,
                                      u.Nacionalidad,
                                      u.Lugar_nacimiento,
                                      u.Id_Rango_edad,
                                      u.Genero,
                                      u.Cedula_identidad,
                                      u.Direccion_residencial,                                     
                                      u.Email,
                                      telefonoEnlaceEmpresa = u.Telefono,
                                      u.Telefono_celular,
                                      u.Telefono_fijo,
                                      u.Rnc,                                      
                                      u.FechaIngreso,
                                      u.Habilitado,
                                      p.Instagram,
                                      p.Facebook,
                                      p.Youtbe,
                                      p.Twitter,
                                      p.Linkedin,
                                      p.Enlace_Paginaweb,
                                      p.Img_perfil,
                                      p.ImgBanner,
                                      u.Bloqueado
                                  }).FirstOrDefaultAsync();

            var motivosRechazo = await (from r in _context.Rechazados
                                        join v in _context.ViolacionesObras on r.Id_Violacion equals v.Id
                                        where r.Id_Usuario == userId
                                        select new
                                        {
                                            r.Fecha,
                                            r.Detalle,
                                            v.Nombre
                                        }).FirstOrDefaultAsync();

            // SE OBTIENEN LAS SOLICITUDES DE CAMBIOS REALIZADAS POR EL ARTISTA            
            var solicitudes = await (from s in _context.SolicitudPerfilUsuarios
                                     join p in _context.PerfilUsuarios on s.IdUsuario equals p.IdUsuario
                               where s.IdUsuario == userId
                               select new SolicitudPerfilUsuario2ViewModel
                               {
                                   Id = s.Id,
                                   Id_Perfil = p.Id,
                                   Nombre = s.Nombre,
                                   Apellido = s.Apellido,
                                   Profesion = s.Profesion,
                                   Descripcion = s.Descripcion,
                                   Email = s.Email,
                                   Telefono = s.Telefono,
                                   Instagram = s.Instagram,
                                   Facebook = s.Facebook,
                                   Youtube = s.Youtbe,
                                   Img_Banner = s.ImgBanner,
                                   Img_Perfil = s.Img_perfil,
                                   Fecha_solicitud = s.Fecha_solicitud,
                                   Estado_solicitud = s.Estado_solicitud
                                   
                               }).ToListAsync();

            // SE OBTIENEN LAS SOLICITUDES DE CAMBIOS RECHAZADAS POR EL ADMINISTRADOR   
            var cambiosRechazados = await (from r in _context.Rechazados
                                           join v in _context.ViolacionesObras on r.Id_Violacion equals v.Id
                                           join s in _context.SolicitudPerfilUsuarios on r.Id_Solicitud_Perfil equals s.Id
                                           where s.IdUsuario == userId
                                           select new
                                           {
                                               r.Id_Solicitud_Perfil,
                                               r.Fecha,
                                               r.Detalle,
                                               v.Nombre
                                           }).ToListAsync();

            foreach (var item in cambiosRechazados)
            {
                for (int i = 0; i < solicitudes.Count; i++)
                {
                    if(solicitudes[i].Id == item.Id_Solicitud_Perfil)
                    {
                        solicitudes[i].Motivo = item.Nombre;
                        solicitudes[i].Detalles = item.Detalle;
                        solicitudes[i].Fecha_rechazo = item.Fecha;
                    }
                }
            }

            // SE OBTIENEN LAS CATEGORIA DE SERVICIO DEL USUARIO
            var categoriaServicios = await (from ucs in _context.Usuario_Categoria_Servicios
                                           join cs in _context.CategoriaServicios on ucs.Id_Categorias_Servicio equals cs.Id
                                           where ucs.Id_Usuario == userId
                                           select new
                                           {
                                              cs.Id,
                                              categoriaServicio = cs.Nombre
                                           }).ToListAsync();

            // SE OBTIENEN LOS TIPOS DE SERVICIO DEL USUARIO
            var tipoServicios = await (from uts in _context.Usuario_Tipo_Servicios
                                       join cs in _context.TipoServicios on uts.Id_Tipo_Servicio equals cs.Id
                                       where uts.Id_Usuario == userId
                                       select new
                                       {
                                           cs.Id,
                                           tipoServicio = cs.Nombre
                                       }).ToListAsync();

            return Ok(new { userInfo, solicitudes, rechazo = userInfo.Habilitado == true ? null : motivosRechazo, categoriaServicios, tipoServicios } );
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("[action]")]
        public async Task<IActionResult> ArtistasPendientes()
        {
            var pendientes = await (from u in _context.Usuarios
                                    join p in _context.PerfilUsuarios on u.Id equals p.IdUsuario
                                    where u.Habilitado == null
                                    select new
                                    {
                                        u.Id,
                                        artista = u.Nombre + ' ' + u.Apellido,
                                        p.Img_perfil
                                    }).ToListAsync(); 

            return Ok(pendientes);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("[action]")]
        public async Task<IActionResult> AprobarArtista([FromBody] Usuario model)
        {
            var user = HttpContext.User.Claims.ToList();
            var userId = Convert.ToInt32(user[0].Value.ToString());

            if (await Permisos(userId, "Aceptar Artistas") == false)
            {
                return Ok(new { status = 203 });
            }

            var artista = await _context.Usuarios.Where( u => u.Id == model.Id).FirstOrDefaultAsync();

            artista.Habilitado = true;

            _context.Entry(artista).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                return Ok( new { status = 200 } );
            }
            catch (Exception ex)
            {
                return NotFound(ex); 
            }
            
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("[action]")]
        public async Task<IActionResult> RechazarArtista([FromBody] RechazarPerfilViewModel model)
        {
            var user = HttpContext.User.Claims.ToList();
            var userId = Convert.ToInt32(user[0].Value.ToString());

            if (await Permisos(userId, "Rechazar Artistas") == false)
            {
                return Ok(new { status = 203 });
            }

            var artista = await _context.Usuarios.Where(u => u.Id == model.Id_Usuario).FirstOrDefaultAsync();
            artista.Habilitado = false;

            _context.Entry(artista).State = EntityState.Modified;

            var rechazo = new Rechazados
            {
                Id_Violacion = model.Id_Violacion,
                Id_Usuario = model.Id_Usuario,
                Fecha = DateTime.Now,
                Detalle = model.Detalle
            };

            await _context.Rechazados.AddAsync(rechazo);            

            try
            {
                await _context.SaveChangesAsync();

                return Ok(new { status = 200 });
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

    }
}
