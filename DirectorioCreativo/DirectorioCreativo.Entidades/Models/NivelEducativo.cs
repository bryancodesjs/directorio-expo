using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public   class NivelEducativo
    {
        public int Id {  get; set; }        
        public string Descripcion { get; set; }
        public virtual ICollection<Usuario> IdUsuarioNavigation { get; set; }
    }
}
