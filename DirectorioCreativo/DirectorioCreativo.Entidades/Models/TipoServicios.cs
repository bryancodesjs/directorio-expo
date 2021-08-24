using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public class TipoServicios
    {
        public int Id {  get; set; }
        public string Nombre {get; set;}
        public virtual ICollection<Usuario_Tipo_Servicios> IdUsuario_Tipo_ServiciosNavigation { get; set; }

    }
}
