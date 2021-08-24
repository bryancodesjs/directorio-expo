using DirectorioCreativo.Datos;
using DirectorioCreativo.Entidades.Models;
using DirectorioCreativo.Web.Models.Obra;
using DirectorioCreativo.Web.Models.Usuarios;
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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ObraController : ControllerBase
    {
        private readonly DbContextDirectorioCreactivo _context;
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;

        public ObraController(DbContextDirectorioCreactivo context, IConfiguration config, IHostingEnvironment env)
        {
            _context = context;
            _config = config;
            _env = env;
        }

        // POST: api/Obra/Listar
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Get([FromBody] ParametrosObra model)
        {
            var user = HttpContext.User.Claims.ToList();
            var userId = user.Count > 0 ? user[0].Value.ToString() : null;

            try
            {
                if (model.Filtro == "1")
                {
                    var obras = await (from o in _context.Obras
                                       join pu in _context.PerfilUsuarios on o.IdPerfil equals pu.Id
                                       join u in _context.Usuarios on pu.IdUsuario equals u.Id
                                       where o.EstadoObra == true
                                       orderby o.FechaRegistro descending
                                       select new
                                       {
                                           idObra = o.Id,
                                           u.Id,
                                           o.ImgObra,
                                           o.NombreObra,
                                           o.FechaRegistro,
                                           o.Valoraciones,
                                           o.Visitas,
                                           artista = u.Nombre,
                                           perfilUsuario = pu.Id,
                                           autenticado = userId != null ? true : false,
                                           valorado = false
                                       }).ToListAsync();

                    return Ok(new { codeStatus = 200, message = obras });
                }

                else if (model.Filtro == "2")
                {
                    var obras = await (from o in _context.Obras
                                       join pu in _context.PerfilUsuarios on o.IdPerfil equals pu.Id
                                       join u in _context.Usuarios on pu.IdUsuario equals u.Id
                                       where o.EstadoObra == true
                                       orderby o.Visitas descending
                                       select new
                                       {
                                           idObra = o.Id,
                                           u.Id,
                                           o.ImgObra,
                                           o.NombreObra,
                                           o.FechaRegistro,
                                           o.Valoraciones,
                                           o.Visitas,
                                           artista = u.Nombre,
                                           perfilUsuario = pu.Id,
                                           autenticado = userId != null ? true : false,
                                           valorado = false
                                       }).ToListAsync();

                    return Ok(new { codeStatus = 200, message = obras });
                }
                else
                {
                    var obras = await (from o in _context.Obras
                                       join pu in _context.PerfilUsuarios on o.IdPerfil equals pu.Id
                                       join u in _context.Usuarios on pu.IdUsuario equals u.Id
                                       where o.EstadoObra == true
                                       orderby o.Valoraciones descending
                                       select new ListObrasViewModel
                                       {
                                           idObra = o.Id,
                                           Id = u.Id,
                                           ImgObra = o.ImgObra,
                                           NombreObra = o.NombreObra,
                                           FechaRegistro = o.FechaRegistro,
                                           Valoraciones = o.Valoraciones,
                                           Visitas = o.Visitas,
                                           artista = u.Nombre,
                                           perfilUsuario = pu.Id,
                                           autenticado = userId != null ? true : false,
                                           valorado = false
                                       }).ToListAsync();

                    var valoraciones = await _context.Valoraciones.ToListAsync();

                    foreach (var item in valoraciones)
                    {
                        for (int i = 0; i < obras.Count; i++)
                        {
                            if (item.Id_Obra == obras[i].idObra)
                            {
                                obras[i].valorado = true;
                            }
                        }
                    }

                    return Ok(new { codeStatus = 200, message = obras });
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }          

        }
       
        [HttpGet("[action]")]
        public async Task<IActionResult> Violaciones()
        {
            try
            {
                var _violaciones = await _context.ViolacionesObras.ToListAsync();
                return Ok(new { status = 200, message = _violaciones });
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> GetByUser([FromBody] IdUsuario model)
        {
           var idPerfil = BuscarPefil( Convert.ToInt32(model.Id) );

           var obras = await (from o in _context.Obras
                                 where o.IdPerfil == idPerfil
                                 orderby o.EstadoObra, o.FechaRegistro descending
                                 select o
                             ).ToListAsync();

           var visitas = await (from p in _context.PerfilUsuarios
                                 where p.Id == idPerfil
                                 select new
                                 {
                                     p.Visitas
                                 }).FirstOrDefaultAsync();

            if (obras == null)
            {
                return NotFound();
            }

            return Ok( new { obras, visitas });
        }
        
        [HttpGet("[action]")]
        public async Task<IActionResult> GetByArtista()
        {
            var user = HttpContext.User.Claims.ToList();
            var userId = user[0].Value.ToString();

            int valoraciones = 0;

            var idPerfil = BuscarPefil( Convert.ToInt32( userId ) );

            var obras = await (from o in _context.Obras
                               where o.IdPerfil == idPerfil
                               orderby o.EstadoObra, o.FechaRegistro descending
                               select new ObraViewModel2
                               {
                                Id = o.Id,    
                                IdPerfil = o.IdPerfil,
                                ImgObra = o.ImgObra,
                                NombreObra = o.NombreObra,
                                DescripcionObra = o.DescripcionObra,
                                Ubicacion = o.Ubicacion,
                                FechaRegistro = o.FechaRegistro,
                                Visitas = o.Visitas,
                                Valoraciones = o.Valoraciones,
                                EstadoObra = o.EstadoObra,
                                Motivo = null,
                                Detalles = null,
                                Fecha_rechazo = null
                               }
                              ).ToListAsync();

            if (obras == null)
            {
                return NotFound();
            }

            // OBTENER LAS VALORACIONES
            foreach (var item in obras)
            {
                valoraciones += item.Valoraciones;
            }

            var visitas = await (from p in _context.PerfilUsuarios
                                 where p.Id == idPerfil
                                 select new
                                 {
                                     p.Visitas
                                 }).FirstOrDefaultAsync();

            // SE OBTIENEN LAS PUBLICACIONES RECHAZADAS POR EL ADMINISTRADOR   
            var publicacionesRechazadas = await (from r in _context.Rechazados
                                           join v in _context.ViolacionesObras on r.Id_Violacion equals v.Id
                                           join o in _context.Obras on r.Id_Obras equals o.Id
                                           where o.IdPerfil == idPerfil
                                           select new
                                           {
                                               r.Id_Obras,
                                               r.Fecha,
                                               r.Detalle,
                                               v.Nombre
                                           }).ToListAsync();

            foreach (var item in publicacionesRechazadas)
            {
                for (int i = 0; i < obras.Count; i++)
                {
                    if (obras[i].Id == item.Id_Obras)
                    {
                        obras[i].Motivo = item.Nombre;
                        obras[i].Detalles = item.Detalle;
                        obras[i].Fecha_rechazo = item.Fecha;
                    }
                }
            }            

            return Ok(new { obras, visitas.Visitas, valoraciones });
        }

        // GET: api/Obra/Mostrar/1
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Mostrar([FromRoute] int id)
        {
            var Obra = await _context.Obras.FindAsync(id);

            if (Obra == null)
            {
                return NotFound();
            }

            return Ok(new ObraViewModel
            {
                Id = Obra.Id,
                NombreObra = Obra.NombreObra,
                DescripcionObra = Obra.DescripcionObra,
                Ubicacion = Obra.Ubicacion,
                FechaRegistro = Obra.FechaRegistro,
                Visitas = Obra.Visitas,
                Valoraciones = Obra.Valoraciones,
                EstadoObra = Obra.EstadoObra
            });
        }

        // GET: api/Obra/Pendiente
        [Authorize(Roles = "Administrador")]        
        [HttpGet("[action]")]
        public async Task<IActionResult> Pendiente()
        {            
            var Obra = await (from o in _context.Obras
                        join p in _context.PerfilUsuarios on o.IdPerfil equals p.Id
                        join u in _context.Usuarios on p.IdUsuario equals u.Id
                        where !o.EstadoObra.HasValue
                        orderby o.FechaRegistro descending
                        select new
                        {
                            o.Id,
                            o.NombreObra,
                            o.DescripcionObra,
                            o.ImgObra,
                            o.EstadoObra,
                            artista = u.Nombre + ' ' + u.Apellido,
                            u.Profesion,
                            p.Img_perfil
                        }).ToListAsync();

            if (Obra == null)
            {
                return NotFound( new { status = 404 });
            }

            return Ok( new { status = 200, message = Obra });
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

        [Authorize(Roles = "Administrador")]
        [HttpPut("[action]")]
        public async Task<IActionResult> ActualizarObraPendiente([FromBody] ActualizarObraPendienteViewModel model)
        {
            var user = HttpContext.User.Claims.ToList();
            var userId = Convert.ToInt32(user[0].Value.ToString());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if ( await Permisos( userId, model.Accion ) == false )
            {
                return Ok(new { status = 203 });
            }

            var obra = await _context.Obras.FindAsync(model.Id);

            if (obra == null)
            {
                return NotFound();
            }

            if (model.Accion == "Aceptar Publicacion")
            {
                obra.EstadoObra = true;
                _context.Entry(obra).State = EntityState.Modified;                
            }

            if (model.Accion == "Rechazar Publicacion")
            {
                obra.EstadoObra = false;
                _context.Entry(obra).State = EntityState.Modified;

                var rechazo = new Rechazados
                {
                    Id_Violacion = model.Id_Violacion,
                    Id_Obras = model.Id,
                    Fecha = DateTime.Now,
                    Detalle = model.Detalle
                };

                await _context.Rechazados.AddAsync(rechazo);
            }            

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { status = 200});
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }            
        }

        // PUT: api/Obra/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ObraViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Id <= 0)
            {
                return BadRequest();
            }

            var obra = await _context.Obras.FirstOrDefaultAsync(c => c.Id == model.Id);

            if (obra == null)
            {
                return NotFound();
            }
            obra.Id = model.Id;
            obra.NombreObra = model.NombreObra;
            obra.DescripcionObra = model.DescripcionObra;
            obra.Ubicacion = model.Ubicacion;
            obra.FechaRegistro = model.FechaRegistro;
            obra.Visitas = model.Visitas;
            obra.Valoraciones = model.Valoraciones;
            obra.EstadoObra = model.EstadoObra;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Guardar Excepción
                return BadRequest();
            }

            return Ok();
        }
        
        // POST: api/Obra/Crear
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] ObraViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // CODIGOS PARA ELIMINAR DATOS QUE NO SEAN DEL STRING BASE 64
            var primerIndex = model.ImgObra.IndexOf(",");
            primerIndex += 1;
            var lastIndex = model.ImgObra.Length;
            var base64 = model.ImgObra.Substring(primerIndex, lastIndex - primerIndex);

            // SE OBTIENE EL ID DEL TOKEN
            var user = HttpContext.User.Claims.ToList();
            var userId = Convert.ToInt32(user[0].Value.ToString());

            Obra obra = new Obra
            {
                IdPerfil = BuscarPefil(Convert.ToInt32(userId)),
                ImgObra = "(" + model._Id  + ")_" + model.NombreObra.Replace(" ", "") + "-" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss") + ".png",
                NombreObra = model.NombreObra,
                DescripcionObra = model.DescripcionObra,
                Ubicacion = model.Ubicacion,
                FechaRegistro = DateTime.Now,
                Visitas = 0,
                Valoraciones = 0,
                EstadoObra = null
            };

            _context.Obras.Add(obra);
            try
            {
                SaveImg(base64, obra.ImgObra);
                await _context.SaveChangesAsync();
                return Ok(new { status = 200 });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
           
        }
        public void SaveImg(string base64String, string nombreIMG)
        {
            // SERVIDOR MICM
            var folderPath = Path.Combine(_env.ContentRootPath, @"C:\inetpub\wwwroot\directorio-creativo\assets\img");

            // DESARROLLO MARTIN MICM
            //var folderPath = Path.Combine(_env.ContentRootPath, @"C:\Users\martin.perez\Desktop\Ministerio de Cultura Oficial\DirectorioCreativo_Cultural\FrontEnd\src\assets\img");

            // DESARROLLO MARTIN CASA
            //var folderPath = Path.Combine(_env.ContentRootPath, @"C:\Users\martin.perez\Desktop\Directiorio_Creativo\DirectorioCreativo_Cultural\FrontEnd\src\assets\img");

            // DESARROLLO BRYAN MICM
            //var folderPath = Path.Combine(_env.ContentRootPath, @"C:\Users\bryan.campos\Desktop\pres\DirectorioCreativo_Cultural\FrontEnd\src\assets\img");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            System.IO.File.WriteAllBytes(Path.Combine(folderPath, nombreIMG), Convert.FromBase64String(base64String));
        }

        public int BuscarPefil(int id)
        {
            return _context.PerfilUsuarios.Where(x => x.IdUsuario == id).Select(x => x.Id).FirstOrDefault();
        }

        // DELETE: api/Obra/Eliminar/1
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Eliminar([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var obra = await _context.Obras.FindAsync(id);
            if (obra == null)
            {
                return NotFound();
            }

            _context.Obras.Remove(obra);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(obra);
            }
            catch (Exception ex)
            {
                return BadRequest( ex );
            }            
        }

        // POST: api/Obra/NuevaVisita
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> NuevaVisita([FromBody] ObraViewModel model)
        {
            try
            {
                var obra = await _context.Obras.FindAsync(model.Id);
                obra.Visitas += 1;
                
                _context.Entry(obra).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { status = 200, message = obra }    );
            }
            catch (Exception ex)
            {
                // Retornar Excepción
                return NotFound(ex);
            }            
        }

        // POST: api/Obra/NuevaValoracion  
        [HttpPost("[action]")]
        public async Task<IActionResult> NuevaValoracion([FromBody] ObraViewModel model)
        {
            // SE OBTIENE EL ID DEL USUARIO AUTENTICADO
            var userClaims = HttpContext.User.Claims.ToList();
            var userId = userClaims[0].Value.ToString();

            Valoraciones valoracion = new Valoraciones();

            try
            {
                var obra = await _context.Obras.FindAsync(model.Id);                
                obra.Valoraciones +=  1;

                valoracion.Fecha = DateTime.Now;
                valoracion.Id_Obra = model.Id;
                valoracion.Id_Usuario = Convert.ToInt32(userId);
             
                _context.Valoraciones.Add(valoracion);
                //await _context.SaveChangesAsync();
                _context.Entry(obra).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { status = 200, message = obra });
            }
            catch (Exception ex)
            {
                // Retornar Excepción
                return NotFound(ex);
            }
        }

        // POST: api/Obra/RevisarValoracion                
        [HttpPost("[action]")]
        public async Task<IActionResult> RevisarValoracion([FromBody] ObraViewModel model)
        {
            // SE OBTIENE EL ID DEL USUARIO AUTENTICADO
            var userClaims = HttpContext.User.Claims.ToList();
            var userId = userClaims[0].Value.ToString();            

            try
            {
                var valoracion = await (from v in _context.Valoraciones
                                        where v.Id_Obra == model.Id && v.Id_Usuario == Convert.ToInt32(userId)
                                        select new
                                        {
                                            v
                                        }).FirstOrDefaultAsync() != null ? 404 : 200;

                return Ok(new { status = 200, message = valoracion });
            }
            catch (Exception ex)
            {
                // Retornar Excepción
                return NotFound(ex);
            }
        }

        // POST: api/Obra/Denunciar          
        [HttpPost("[action]")]
        public async Task<IActionResult> DenunciarObra([FromBody] DenunciasObras model)
        {
            // SE OBTIENE EL ID DEL USUARIO AUTENTICADO
            var userClaims = HttpContext.User.Claims.ToList();
            var userId = userClaims[0].Value.ToString();

            try
            {
                var valoracion = await (from v in _context.DenunciasObras
                                        where v.Id_Obra == model.Id_Obra && v.Id_Artista == Convert.ToInt32(userId)
                                        select new
                                        {
                                            v
                                        }).FirstOrDefaultAsync() != null ? 403 : 200;

                if( valoracion == 200 )
                {
                    DenunciasObras denuncia = new DenunciasObras();

                    denuncia.Id_Obra = model.Id_Obra;
                    denuncia.Id_Artista = Convert.ToInt32(userId);
                    denuncia.Id_Violacion = model.Id_Violacion;
                    denuncia.Detalle = model.Detalle;
                    denuncia.Fecha = DateTime.Now;

                     _context.DenunciasObras.Add(denuncia);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { status = valoracion, message = "OK" });
            }
            catch (Exception ex)
            {
                // Retornar Excepción
                return NotFound(ex);
            }
        }


        // PUT: api/Obra/Desactivar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var obra = await _context.Obras.FirstOrDefaultAsync(c => c.Id == id);

            if (obra == null)
            {
                return NotFound();
            }

            obra.EstadoObra = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Guardar Excepción
                return BadRequest();
            }

            return Ok();
        }

        // PUT: api/Obra/Activar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var obra = await _context.Obras.FirstOrDefaultAsync(c => c.Id == id);

            if (obra == null)
            {
                return NotFound();
            }

            obra.EstadoObra = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Guardar Excepción
                return BadRequest();
            }

            return Ok();
        }
    }
}
