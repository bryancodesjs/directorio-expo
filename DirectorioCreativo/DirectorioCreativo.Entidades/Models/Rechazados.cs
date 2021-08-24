using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public class Rechazados
    { 
        public int Id { get; set; }
        public int? Id_Violacion { get; set; }
        public int? Id_Solicitud_Perfil { get; set; }
        public int? Id_Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Detalle { get; set; }
        public int?  Id_Obras { get; set; }
        public virtual ViolacionesObra IdViolacionesObraNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }

        public virtual Obra IdObraNavigation { get; set; }

    }
}
