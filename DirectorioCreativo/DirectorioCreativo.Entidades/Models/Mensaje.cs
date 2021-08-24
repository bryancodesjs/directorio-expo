using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public class Mensaje
    {
        public Mensaje()
        {
            DetalleMensajes = new HashSet<DetalleMensaje>();
        }

        public int Id { get; set; }
        public int IdEmisor { get; set; }
        public int IdReceptor { get; set; } 
        
        public int IdTipoSolicitud {  get; set; }

        public virtual Usuario IdEmisorNavigation { get; set; }

        public virtual Usuario IdReceptorNavigation { get; set; }

        public virtual TipoSolicitud IdTipoSolicitudNavigation { get; set; }

        public virtual ICollection<DetalleMensaje> DetalleMensajes { get; set; }
    }
}
