using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public class DetalleMensaje
    {
        public int Id { get; set; }
        public int IdMensaje { get; set; }
        public string Mensaje { get; set; }
        public bool Leido { get; set; }
        public DateTime Fecha { get; set; }
        public int IdReceptor {  get; set; }

        public virtual Mensaje IdMensajeNavigation { get; set; }
    }
}
