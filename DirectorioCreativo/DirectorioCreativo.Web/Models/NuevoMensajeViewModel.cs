using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Models
{
    public class NuevoMensajeViewModel
    {
        public int Id_receptor {  get; set; }
        public string asunto { get; set; }
        public int tipo_solicitud { get; set; }
        public string detalles { get; set; }
    }
}
