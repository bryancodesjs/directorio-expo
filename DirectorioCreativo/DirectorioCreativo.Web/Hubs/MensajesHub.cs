using DirectorioCreativo.Web.Models.Mensajes;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Hubs
{
    public class MensajesHub: Hub
    {
        public static List<ConectadosViewModel> conectados = new List<ConectadosViewModel>();
        
    }
}
