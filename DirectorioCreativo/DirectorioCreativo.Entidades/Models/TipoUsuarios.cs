using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public class TipoUsuarios
    {
        public int Id { get; set; }
        public string Nombre {  get; set; }
        public virtual ICollection <Usuario> IdUsuarioNavigation { get; set; }
 
    }
}
