using DirectorioCreativo.Datos;
using DirectorioCreativo.Entidades.Models;
using DirectorioCreativo.Web.Models.Permiso;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DirectorioCreativo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        private readonly DbContextDirectorioCreactivo _context;
        private readonly IConfiguration _config;

        public PermisosController(DbContextDirectorioCreactivo context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        // GET: api/Permisos/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<PermisoViewModel>> Listar()
        {
            var Permiso = await _context.Permisos.ToListAsync();

            return Permiso.Select(c => new PermisoViewModel
            {
                Id = c.Id,
                Nombre = c.Nombre
            });

        }

        // GET: api/Permisos/Mostrar/1
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Mostrar([FromRoute] int id)
        {

            var Permiso = await _context.Permisos.FindAsync(id);

            if (Permiso == null)
            {
                return NotFound();
            }

            return Ok(new PermisoViewModel
            {
                Id = Permiso.Id,
                Nombre = Permiso.Nombre

            });
        }

        // PUT: api/Permisos/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] PermisoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Id <= 0)
            {
                return BadRequest();
            }

            var Permiso = await _context.Permisos.FirstOrDefaultAsync(c => c.Id == model.Id);

            if (Permiso == null)
            {
                return NotFound();
            }

            Permiso.Id = model.Id;
            Permiso.Nombre = model.Nombre;

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

        // POST: api/Permisos/Crear
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] PermisoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Permiso permiso = new Permiso
            {
                Id = model.Id,
                Nombre = model.Nombre,

            };

            _context.Permisos.Add(permiso);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok();
        }

        // DELETE: api/Permisos/Eliminar/1
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Eliminar([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }

            _context.Permisos.Remove(permiso);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok(permiso);
        }



        private bool PermisosExists(int id)
        {
            return _context.Obras.Any(e => e.Id == id);
        }
    }
}
