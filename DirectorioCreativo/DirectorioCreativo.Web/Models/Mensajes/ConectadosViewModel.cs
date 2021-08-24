using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Models.Mensajes
{
    public class ConectadosViewModel
    {        
        public string token { get; set; }
        public int idUsuario { get; set; }
        public string lastIdConexion { get; set; }
        public string idConexion { get; set; }
        public DateTime fechaConexion { get; set; }
        public DateTime ultimaConexion { get; set; }
        public bool Online { get; set; }
    }
}
