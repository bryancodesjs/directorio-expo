using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Models.AdministracionUsuarios
{
    public class NuevoPermisoViewModel
    {
        public int idRolPermiso {  get; set; }
        public int idUsuarioRol { get; set; }
        public int idPermiso { get; set; }
    }
}
