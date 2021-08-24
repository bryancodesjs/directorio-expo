using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public class SolicitudPerfilUsuario
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Profesion { get; set; }
        public string Descripcion { get; set; }
        public string Img_perfil { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public bool? Estado_solicitud { get; set; }
        public DateTime Fecha_solicitud { get; set; }
        public int Visitas { get; set; }
        public int Valoraciones { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Youtbe { get; set; }
        public string ImgBanner { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }

    }
}
