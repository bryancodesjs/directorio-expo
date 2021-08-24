using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Models.Mensajes
{
    public class MensajeViewModel
    {
        public int idMensaje { get; set; }        
        public int IdReceptor { get; set; }
        public string Mensaje { get; set; }
        public bool Leido { get; set; }
        public DateTime Fecha { get; set; }
    }
}
