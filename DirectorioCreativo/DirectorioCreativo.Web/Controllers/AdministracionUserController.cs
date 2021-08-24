using DirectorioCreativo.Datos;
using DirectorioCreativo.Entidades.Models;
using DirectorioCreativo.Web.Models.AdministracionUsuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DirectorioCreativo.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdministracionUserController : ControllerBase
    {

        private readonly DbContextDirectorioCreactivo _context;
        private readonly IConfiguration _config;

        public AdministracionUserController(DbContextDirectorioCreactivo context, IConfiguration config)
        {
            _context = context;
            _config = config;
            // roleManager = rolemgr;
        }

        // GET: api/<AdministracionUserController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var obras = await _context.Obras.ToListAsync();

                var artistas = await (from u in _context.Usuarios
                                      join p in _context.PerfilUsuarios on u.Id equals p.IdUsuario
                                      join ru in _context.UsuarioRols on u.Id equals ru.IdUsuario
                                      join r in _context.Roles on ru.IdRol equals r.Id
                                      where r.Nombre == "Usuario"
                                      select new UsuariosViewModel
                                      {
                                          Id = u.Id,
                                          IdPerfil = p.Id,
                                          artista = u.Nombre + ' ' + u.Apellido,
                                          email = u.Email,
                                          ingreso = u.FechaIngreso,
                                          Profesion = u.Profesion == "" ? "Sin profesion" : u.Profesion,
                                          publicaciones = 0,
                                          Visitas = p.Visitas,
                                          Valoraciones = p.Valoraciones,
                                          Bloqueado = u.Bloqueado
                                      }).ToListAsync();

                var administradores = await (from u in _context.Usuarios
                                             join ru in _context.UsuarioRols on u.Id equals ru.IdUsuario
                                             join r in _context.Roles on ru.IdRol equals r.Id
                                             where r.Nombre == "Administrador"
                                             select new UsuariosViewModel
                                             {
                                                 Id = u.Id,
                                                 artista = u.Nombre + ' ' + u.Apellido,
                                                 email = u.Email,
                                                 ingreso = u.FechaIngreso,
                                                 Bloqueado = u.Bloqueado
                                             }).ToListAsync();

                foreach (var _o in obras)
                {
                    for (int i = 0; i < artistas.Count(); i++)
                    {
                        if (artistas[i].IdPerfil == _o.IdPerfil)
                        {
                            artistas[i].publicaciones += 1;
                        }
                    }
                }

                return Ok(new { artistas, administradores });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex });
            }
        }

        // PUT api/Habilitar
        [HttpPut("[action]")]
        public async Task<IActionResult> Habilitar([FromBody] UsuariosViewModel model)
        {
            var user = HttpContext.User.Claims.ToList();
            var userId = Convert.ToInt32(user[0].Value.ToString());

            try
            {
                if (await RevisarPermiso(userId, "Habilitar Artista") == false)
                {
                    return Ok(new { status = 203 });
                }

                var artista = await _context.Usuarios.FindAsync(model.Id);
                artista.Bloqueado = false;
                _context.Entry(artista).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { status = 200 });
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
        // PUT api/AdministracionUser/Deshabilitar
        [HttpPut("[action]")]
        public async Task<IActionResult> Deshabilitar([FromBody] UsuariosViewModel model)
        {
            var user = HttpContext.User.Claims.ToList();
            var userId = Convert.ToInt32(user[0].Value.ToString());

            try
            {
                if (await RevisarPermiso(userId, "Deshabilitar Artista") == false)
                {
                    return Ok(new { status = 203 });
                }

                var artista = await _context.Usuarios.FindAsync(model.Id);
                artista.Bloqueado = true;
                _context.Entry(artista).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { status = 200 });
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        // GET api/AdministracionUser/Permisos
        [HttpPost("[action]")]
        public async Task<IActionResult> Permisos([FromBody] UsuariosViewModel model)
        {
            try
            {
                var permisosUsuario = await (from p in _context.UsuarioRols
                                             join r in _context.Roles on p.IdRol equals r.Id
                                             join urp in _context.UsuarioRolPermisos on p.Id equals urp.Id_UsuarioRol
                                             join perm in _context.Permisos on urp.IdPermiso equals perm.Id
                                             where p.IdUsuario == model.Id
                                             select new
                                             {
                                                 idUsuario = p.IdUsuario,
                                                 idRolPermiso = urp.Id,
                                                 idUsuarioRol = p.Id,
                                                 rol = r.Nombre,
                                                 idPermiso = perm.Id,
                                                 permiso = perm.Nombre
                                             }).ToListAsync();

                var permisosRestantes = await (from p in _context.Permisos
                                               select new PermisosViewModel
                                               {
                                                   idPermiso = p.Id,
                                                   permiso = p.Nombre
                                               }).ToListAsync();

                if (permisosUsuario.Count() > 0)
                {
                    foreach (var item in permisosUsuario)
                    {
                        for (int i = 0; i < permisosRestantes.Count(); i++)
                        {
                            if (permisosRestantes[i].idPermiso == item.idPermiso)
                            {
                                permisosRestantes.RemoveAt(i);
                            }
                            else
                            {
                                permisosRestantes[i].idUsuarioRol = item.idUsuarioRol;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < permisosRestantes.Count(); i++)
                    {
                        permisosRestantes[i].idUsuarioRol = await BuscarIDRolUsuario(model.Id);
                    }
                }

                return Ok(new { permisosUsuario, permisosRestantes, rol = permisosUsuario.Count() > 0 ? permisosUsuario[0].rol : await BuscarRolUsuario(model.Id) });
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        private async Task<string> BuscarRolUsuario(int id)
        {
            var permisosUsuario = await (from p in _context.UsuarioRols
                                         join r in _context.Roles on p.IdRol equals r.Id
                                         where p.IdUsuario == id
                                         select new
                                         {
                                             r.Nombre
                                         }).FirstOrDefaultAsync();

            return permisosUsuario.Nombre;
        }

        private async Task<int> BuscarIDRolUsuario(int id)
        {
            var permisosUsuario = await (from p in _context.UsuarioRols                                        
                                        where p.IdUsuario == id
                                        select new
                                        {                                            
                                            idUsuarioRol = p.Id                     
                                        }).FirstOrDefaultAsync(); 
            
            return permisosUsuario.idUsuarioRol;
        }

        // POST api/AdministracionUser/AgregarPermiso
        [HttpPost("[action]")]
        public async Task<IActionResult> AgregarPermiso([FromBody] NuevoPermisoViewModel model)
        {
            try
            {
                UsuarioRolPermiso permiso = new UsuarioRolPermiso();
                permiso.IdPermiso = model.idPermiso;
                permiso.Id_UsuarioRol = model.idUsuarioRol;
                await _context.UsuarioRolPermisos.AddAsync(permiso);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        // POST api/AdministracionUser/QuitarPermiso
        [HttpPost("[action]")]
        public async Task<IActionResult> QuitarPermiso([FromBody] NuevoPermisoViewModel model)
        {
            try
            {
                var permiso = await _context.UsuarioRolPermisos.FindAsync(model.idRolPermiso);
                _context.Entry(permiso).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        // POST api/AdministracionUser/Revisar
        [Authorize(Roles = "Administrador")]
        [HttpGet("[action]")]
        public async Task<IActionResult> Check()
        {
            var user = HttpContext.User.Claims.ToList();
            var userId = Convert.ToInt32(user[0].Value.ToString());

            try
            {
                var acceso = await (from p in _context.UsuarioRols
                                    join urp in _context.UsuarioRolPermisos on p.Id equals urp.Id_UsuarioRol
                                    join perm in _context.Permisos on urp.IdPermiso equals perm.Id
                                    where p.IdUsuario == userId && perm.Nombre == "Super Admin"
                                    select new
                                    {
                                        permiso = perm.Nombre
                                    }).ToListAsync();

                return acceso.Count() > 0 ? Ok(new { message = true}) : Ok(new { message = false });
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        public async Task<bool> RevisarPermiso(int idUser, string permiso)
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
