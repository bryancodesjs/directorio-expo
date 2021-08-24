using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public class UsuarioRol
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }

        public virtual Role IdRolNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }

    }
}
