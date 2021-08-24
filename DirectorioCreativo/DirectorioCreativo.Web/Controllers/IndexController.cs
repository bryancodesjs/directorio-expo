using DirectorioCreativo.Datos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DirectorioCreativo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly DbContextDirectorioCreactivo context;
        public IndexController(DbContextDirectorioCreactivo _context)
        {
            context = _context;
        }


        // GET: api/<IndexController>
        [HttpGet]
        public async Task<IActionResult> Get()
        { 
            try
            {
                var result = await context.Usuarios.ToListAsync();

                return Ok(new { API = "API Directorio Creativo" ,version = 1.0, statusDB = result.Count() > 0 ? "Ok" : "False" });
            }
            catch (Exception ex)
            {
                return NotFound(new { API = "API Directorio Creativo", version = 1.0, status = 404, message = ex });
            }
            
        }        
    }
}
