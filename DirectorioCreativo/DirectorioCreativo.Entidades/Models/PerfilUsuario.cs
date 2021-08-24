using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public  class PerfilUsuario
    {
        public PerfilUsuario()
        {
            Obras = new HashSet<Obra>();
        }

        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Img_perfil { get; set; }
        public int Visitas { get; set; }
        public int Valoraciones { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Youtbe { get; set; }        
        public string Enlace_Paginaweb { get; set; }
        public string Twitter { get; set; }
        public string Linkedin { get; set; }
        public string ImgBanner { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<Obra> Obras { get; set; }

    }
}
