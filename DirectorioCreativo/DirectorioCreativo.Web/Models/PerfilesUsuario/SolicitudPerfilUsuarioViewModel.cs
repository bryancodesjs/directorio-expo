using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Models.PerfilesUsuario
{
    public class SolicitudPerfilUsuarioViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Profesion { get; set; }
        public string Descripcion { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public int Visitas { get; set; }
        public int Valoraciones { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Youtube { get; set; }
        public string Img_Banner { get; set; }
        public string Img_Perfil { get; set; }
    }
}
