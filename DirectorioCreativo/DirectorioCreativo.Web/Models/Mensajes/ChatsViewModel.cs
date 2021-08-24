using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Models.Mensajes
{
    public class ChatsViewModel
    {
        public int idChat {  get; set; }
        public int IdReceptor { get; set; }
        public int IdEmisor { get; set; }
        public string artista { get; set; }
        public string foto { get; set; }
        public bool noLeido {  get; set; }
        public int CantidadnoLeido { get; set; }
        public List<MensajeViewModel> Mensajes { get; set; }
        public DateTime Fecha { get; set; } 
        public bool EmisorOnline { get; set; }
        public bool ReceptorOnline { get; set; }

    }
}
