using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Models.Usuarios
{
    public class ActualizarViewModel
    {
        public int id { get; set; }
        public int? id_provincia { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre no debe de tener más de 100 caracteres, ni menos de 3 caracteres.")]
        public string nombre { get; set; }

        public string apellido { get; set; }

        public string profesion { get; set; }

        public string descripcion_general { get; set; }

        public string telefono { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string clave { get; set; }

        public bool act_password { get; set; }
    }
}
