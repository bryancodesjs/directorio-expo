using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Models.AdministracionUsuarios
{
    public class PermisosViewModel
    {
        public int idRolPermiso {get; set;}
        public int idUsuarioRol {get; set;}
        public string rol {get; set;}
        public int idPermiso {get; set;}
        public string permiso {get; set;}
        public bool? Acceso { get; set; }
    }
}
