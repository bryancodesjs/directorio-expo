using DirectorioCreativo.Datos;
using DirectorioCreativo.Entidades.Models;
using DirectorioCreativo.Web.Models.Usuarios;
using DirectorioCreativo.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class LoginController : ControllerBase
    {
        private readonly DbContextDirectorioCreactivo _context;
        private readonly IConfiguration _config;
        //private RoleManager<IdentityRole> roleManager;

        public LoginController(DbContextDirectorioCreactivo context, IConfiguration config)
        {
            _context = context;
            _config = config;
           // roleManager = rolemgr;
        }
        //// PUT: api/Login
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginViewModel model)
        {
            bool? admin = null;

            try
            {
                // SE FILTRA EL USUARIO EN LA BASE DE DATOS
                var user = await _context.Usuarios.Where(x => x.Email == model.email).FirstOrDefaultAsync();               

                // SE VERIFICA QUE EXISTA EL USUARIO
                if (user == null)
                {
                    return NotFound(new { codeStatus = 404, message = "Usuario no existe" });
                }                

                // SE OBTIENEN LOS ROLES DEL USUARIO
                var roles = await (from ur in _context.UsuarioRols
                                   where ur.IdUsuario == user.Id
                                   join r in _context.Roles on ur.IdRol equals r.Id
                                   select new
                                   {
                                       roles = r.Nombre
                                   }).ToListAsync();

                admin = roles.Find(e => e.roles == "Administrador") != null ? true : false;
                

                // SE CONVIERTE A UN ARRAY DE BYTE LA SALT 
                var token = System.Convert.FromBase64String(user.SaltClave);

                // SE DESCIFRA LA CLAVE QUE LLEGA POR POST PARA PODER COMPARARLA CON LA CLAVE DE BASE DE DATOS
                var salt = Descifrar._DesifrarClave(model.clave, token);

                // SE VERIFICA QUE LAS CLAVES SEAN IGUALES
                if (user.Clave == salt)
                {
                    // SE OBTIENE EL CODIGO SECRETO
                    var secretKey = _config.GetValue<string>("SecretKey");
                    var key = Encoding.ASCII.GetBytes(secretKey);

                    // SE GENERAN LOS CLAIMS PARA EL TOKEN
                    var claims = new ClaimsIdentity();
                    claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

                    // SE AGREGAN LOS ROLES DEL USUARIO ROLES 
                    foreach (var _roles in roles)
                    {
                        claims.AddClaim(new Claim(ClaimTypes.Role, _roles.roles ));
                    }
                    
                    var tokenDescription = new SecurityTokenDescriptor
                    {
                        Subject = claims,
                        Expires = DateTime.UtcNow.AddMinutes(45),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenCreated = tokenHandler.CreateToken(tokenDescription);

                    string bearer = tokenHandler.WriteToken(tokenCreated);                   


                    return Ok(new { codeStatus = 200, token = bearer, id = user.Id, usuarioActual = user.Nombre, redirect = admin == true ? "/backoffice" : "/dashboard" });
                }
                else
                {
                    return NotFound(new { codeStatus = 404, message = "Usuario o contraseña invalidos" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize]
        [HttpGet("[action]")]
        public async Task<IActionResult> Acceso()
        {
            var user = HttpContext.User.Claims.ToList();
            if (user.Count == 0)
            {
                return Ok(new { status = 401 });
            }

            bool permisoUsuario_Empresa = false;
            var userId = Convert.ToInt32(user[0].Value.ToString());

            // SE OBTIENEN LOS ROLES DEL USUARIO
            var roles = await (from ur in _context.UsuarioRols
                               where ur.IdUsuario == userId
                               join r in _context.Roles on ur.IdRol equals r.Id
                               select new
                               {
                                   roles = r.Nombre
                               }).ToListAsync();

            var infoUsuario = (from u in _context.Usuarios
                          where u.Id == userId
                          select new
                          {
                              usuario = u.Nombre,
                              tipo = u.Tipo_Registro
                          }).FirstOrDefault();
            if (infoUsuario.tipo != null)
            {
                if (infoUsuario.tipo != "Usuario")
                {
                    permisoUsuario_Empresa = true;
                }
            }
            

            return Ok(new { status = 200, message = roles.Find(e => e.roles == "Administrador") != null ? true : false, usuario = infoUsuario.usuario, permisoProf_Empre = permisoUsuario_Empresa });
        }        


    }
}
