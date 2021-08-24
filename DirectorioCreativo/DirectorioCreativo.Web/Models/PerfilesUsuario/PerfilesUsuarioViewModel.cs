using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Models.PerfilesUsuario
{
    public class PerfilesUsuarioViewModel
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int Visitas { get; set; }
        public int Valoraciones { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Youtbe { get; set; }
        public string ImgBanner { get; set; }
    }
}
