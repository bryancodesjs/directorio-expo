using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Models.Usuarios
{
    public class UsuarioViewModel
    {
        public int id { get; set; }
        public int? id_provincia { get; set; }


        public string nombre { get; set; }

        public string apellido { get; set; }

        public string profesion { get; set; }

        public string descripcion_general { get; set; }

        public string telefono { get; set; }

        public string email { get; set; }

        public string clave { get; set; }

        public string salt_clave { get; set; }

        public DateTime fecha_ingreso { get; set; }
        public bool? habilitado { get; set; }

    }
}
