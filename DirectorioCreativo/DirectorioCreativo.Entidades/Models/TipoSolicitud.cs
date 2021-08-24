using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public  class TipoSolicitud
    {
        public int Id { get; set; }

        public string Nombre  { get; set; }

        public virtual ICollection<Mensaje> IdMensajeNavigation { get; set; }
    }
}
