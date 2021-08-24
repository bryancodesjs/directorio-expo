using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public class UsuarioRolPermiso
    {
        public int Id { get; set; }
        public int Id_UsuarioRol { get; set; }
        public int IdPermiso { get; set; }

        public virtual Permiso IdPermisoNavigation { get; set; }
        public virtual Role IdRolesNavigation { get; set; }
    }
}
