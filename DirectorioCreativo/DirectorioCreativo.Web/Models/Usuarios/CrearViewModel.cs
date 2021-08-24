using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Models.Usuarios
{
    public class CrearViewModel
    {        
        public string tipo_registro { get; set; }
        public string RNC { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }       
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
        public DateTime FechaIngreso { get; set; }
        public bool Habilitado { get; set; }
    }
}
