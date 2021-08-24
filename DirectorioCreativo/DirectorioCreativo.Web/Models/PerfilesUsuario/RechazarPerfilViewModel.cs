using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Models.PerfilesUsuario
{
    public class RechazarPerfilViewModel
    {
        public int Id_Usuario { get; set; }
        public int Id_Violacion { get; set; }
        public string Detalle { get; set; }
    }
}
