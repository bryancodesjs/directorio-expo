using DirectorioCreativo.Datos;
using DirectorioCreativo.Entidades.Models;
using DirectorioCreativo.Web.Hubs;
using DirectorioCreativo.Web.Models;
using DirectorioCreativo.Web.Models.Mensajes;
using DirectorioCreativo.Web.Models.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
    public class MensajeriaController : ControllerBase
    {
        private readonly DbContextDirectorioCreactivo _context;
        private readonly IHubContext<MensajesHub> _hubContext;

        public MensajeriaController(DbContextDirectorioCreactivo context, IHubContext<MensajesHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;            
        }      

        // POST api/Mensajeria/EnviarMensaje
        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> EnviarMensaje([FromBody] NuevoMensajeViewModel model)
        {
            // SE OBTIENE EL ID DEL USUARIO AUTENTICADO
            var userClaims = HttpContext.User.Claims.ToList();
            var userId = Convert.ToInt32( userClaims[0].Value.ToString() );
            var chat = await _context.Mensajes.Where(m => m.IdEmisor == userId && m.IdReceptor == model.Id_receptor || m.IdReceptor == userId && m.IdEmisor == model.Id_receptor).FirstOrDefaultAsync();

            try
            {
                if ( chat != null )
                {
                    var detalleMensaje = new DetalleMensaje
                    {
                        IdMensaje = chat.Id,
                        IdReceptor = model.Id_receptor,
                        Mensaje = model.detalles,
                        Leido = false,
                        Fecha = DateTime.Now
                    };

                    await _context.DetalleMensajes.AddAsync(detalleMensaje);
                    await _context.SaveChangesAsync();

                    await _hubContext.Clients.All.SendAsync("Update");
                    return Ok(new { status = 200 });
                }
                else
                {
                    var _mensaje = new Mensaje
                    {
                        IdEmisor = userId,
                        IdReceptor = model.Id_receptor,
                        IdTipoSolicitud = model.tipo_solicitud
                    };

                    await _context.Mensajes.AddAsync(_mensaje);
                    await _context.SaveChangesAsync();

                    var _detalleMensaje = new DetalleMensaje
                    {
                        IdMensaje = _mensaje.Id,
                        IdReceptor = model.Id_receptor,
                        Mensaje = model.detalles,
                        Leido = false,
                        Fecha = DateTime.Now
                    };

                    await _context.DetalleMensajes.AddAsync(_detalleMensaje);
                    await _context.SaveChangesAsync();

                    await _hubContext.Clients.All.SendAsync("Update");
                    return Ok(new { status = 200 });
                }
               
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
           
        }

        // GET api/Mensajeria/tipoSolicitud
        [Authorize]
        [HttpGet("[action]")]
        public async Task<IActionResult> tipoSolicitud()
        {
            try
            {
                var _items = await _context.TipoSolicitud.ToListAsync();
                return Ok(new { status = 200, message = _items });
            }
            catch (Exception)
            {
                return NotFound();
            }            
        }

        // GET api/Mensajeria/Chats
        [Authorize]
        [HttpGet("[action]")]
        public async Task<IActionResult> Chats()
        {
            // SE OBTIENE EL ID DEL USUARIO AUTENTICADO
            var userClaims = HttpContext.User.Claims.ToList();
            var userId = Convert.ToInt32(userClaims[0].Value.ToString());

            try
            {
                List<ChatsViewModel> model = new List<ChatsViewModel>();

                var chats1 = await (from m in _context.Mensajes
                                    join u in _context.Usuarios on m.IdReceptor equals u.Id
                                    join p in _context.PerfilUsuarios on u.Id equals p.IdUsuario
                                    where m.IdEmisor == userId
                                    orderby m.Id descending
                                    select new ChatsViewModel
                                    {
                                        idChat = m.Id,
                                        IdReceptor = m.IdReceptor,                                        
                                        artista = u.Nombre + ' ' + u.Apellido,
                                        foto = p.Img_perfil,
                                        noLeido = false,
                                        CantidadnoLeido = 0                                        
                                    }).ToListAsync();

                var chats2 = await (from m in _context.Mensajes
                                    join u in _context.Usuarios on m.IdEmisor equals u.Id
                                    join p in _context.PerfilUsuarios on u.Id equals p.IdUsuario
                                    where m.IdReceptor == userId
                                    orderby m.Id descending
                                    select new ChatsViewModel
                                    {
                                        idChat = m.Id,
                                        IdEmisor = m.IdEmisor,
                                        artista = u.Nombre + ' ' + u.Apellido,
                                        foto = p.Img_perfil,
                                        noLeido = false,
                                        CantidadnoLeido = 0
                                    }).ToListAsync();

                foreach (var _chat1 in chats1)
                {
                    model.Add(_chat1);
                }
                foreach (var _chat2 in chats2)
                {
                    model.Add(_chat2);
                }                

                // SE VERIFICAN TODOS LOS MENSAJES CORRESPONDIENTES A CADA CHAT
                foreach (var chat in model)
                {
                    var mensajes = await (from m in _context.DetalleMensajes
                                          where m.IdMensaje == chat.idChat
                                          orderby m.Fecha 
                                          select new MensajeViewModel
                                          {
                                              idMensaje = m.Id,
                                              IdReceptor = m.IdReceptor,
                                              Mensaje = m.Mensaje,
                                              Leido = m.Leido,
                                              Fecha =  m.Fecha
                                          }).ToListAsync();

                    if (mensajes != null)
                    {
                        for (int i = 0; i < mensajes.Count(); i++)
                        {
                            chat.Fecha = mensajes[ i ].Fecha;

                            if (!mensajes[i].Leido && mensajes[i].IdReceptor == userId)
                            {
                                chat.noLeido = true;
                                chat.CantidadnoLeido += 1;                                
                            }
                        }
                    }

                    chat.Mensajes = mensajes;
                }

                // SE VERIFICA SI ALGUN USUARIO DEL CHAT ESTA CONECTADO
                foreach (var conectados in MensajesHub.conectados)
                {
                    for (int i = 0; i < model.Count; i++)
                    {
                        if (conectados.idUsuario == model[i].IdEmisor)
                        {
                            model[i].EmisorOnline = true;
                        }
                        if (conectados.idUsuario == model[i].IdReceptor)
                        {
                            model[i].ReceptorOnline = true;
                        }
                    }
                }

                return Ok(new { status = 200, chats = model.OrderByDescending(m=>m.Fecha), userId });
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        // GET api/Mensajeria/tipoSolicitud
        [Authorize]
        [HttpPut("[action]")]
        public async Task<IActionResult> MensajeLeido([FromBody] DetalleMensaje model)
        {
            try
            {
                var mensajes = await (from dm in _context.DetalleMensajes
                                      where dm.IdMensaje == model.Id && dm.Leido == false
                                      select new DetalleMensaje
                                      {
                                          Id = dm.Id,
                                          IdMensaje = dm.IdMensaje,
                                          Mensaje = dm.Mensaje,
                                          Leido = dm.Leido,
                                          Fecha = dm.Fecha,
                                          IdReceptor = dm.IdReceptor
                                      }).ToListAsync();

                foreach (var _mensaje in mensajes)
                {
                    _mensaje.Leido = true;
                    _context.Entry(_mensaje).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

                await _hubContext.Clients.All.SendAsync("Update");
                return Ok(new { status = 200 });
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // GET api/Mensajeria/tipoSolicitud
        //[Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> Conectado([FromBody] ConectadosViewModel model)
        {
            //bool existe = false;
            ConectadosViewModel nuevoModel = new ConectadosViewModel();            
            try
            {
                // SE OBTIENE EL ID DEL USUARIO AUTENTICADO
                var userClaims = HttpContext.User.Claims.ToList();                
                if (userClaims.Count == 0) 
                {
                    if ( !string.IsNullOrEmpty(model.token) )
                    {
                        var usuario = MensajesHub.conectados.Where(e => e.token == model.token).FirstOrDefault();
                        if (usuario != null)
                        {
                            usuario.Online = false;
                            usuario.ultimaConexion = DateTime.Now;
                            MensajesHub.conectados.Remove(usuario);
                            await _hubContext.Clients.All.SendAsync("Update");
                        }                        
                    }
                    return Ok(new { status = 201 });
                }

                var userId = Convert.ToInt32(userClaims[0].Value.ToString());
                var result = MensajesHub.conectados.Where( e => e.token == model.token || e.idUsuario == userId ).FirstOrDefault();

                if (result == null)
                {   
                    nuevoModel.idUsuario = Convert.ToInt32(userClaims[0].Value.ToString());
                    nuevoModel.idConexion = model.idConexion;
                    nuevoModel.fechaConexion = DateTime.Now;
                    nuevoModel.ultimaConexion = DateTime.Now;
                    nuevoModel.Online = true;
                    nuevoModel.token = model.token;                    

                    MensajesHub.conectados.Add(nuevoModel);

                    await _hubContext.Clients.All.SendAsync("Update");

                    return Ok(new { status = 200, message = MensajesHub.conectados });
                }
                if(!result.Online)
                {
                    MensajesHub.conectados.Remove(result);
                    result.fechaConexion = DateTime.Now;
                    result.ultimaConexion = DateTime.Now;
                    result.token = model.token;
                    result.Online = true;
                    result.idConexion = model.idConexion;
                    MensajesHub.conectados.Add(result);
                    return Ok(new { status = 200, message = MensajesHub.conectados });
                }

                return Ok(new { status = 200, message = MensajesHub.conectados });
            }
            catch (Exception)
            {
                return NotFound();
            }
        }       
        
        [HttpPost("[action]")]
        public async Task<IActionResult> Desconectado([FromBody] ConectadosViewModel model)
        {
            try
            {
                var usuario = MensajesHub.conectados.Where(e => e.token == model.token).FirstOrDefault();
                if (usuario != null)
                {
                    usuario.Online = false;
                    usuario.ultimaConexion = DateTime.Now;
                    MensajesHub.conectados.Remove(usuario);
                    //MensajesHub.conectados.Add(usuario);

                    await _hubContext.Clients.All.SendAsync("Update");

                    return Ok(new { status = 200, message = model });
                }
                else
                {
                    return Ok(new { status = 201 });
                }
                
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

    }
}
