using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Models.Usuarios
{
    public class LoginViewModel
    {

        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string clave { get; set; }

    }
}
