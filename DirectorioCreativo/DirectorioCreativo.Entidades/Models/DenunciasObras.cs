using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public class DenunciasObras
    {
        public int Id { get; set; }

        public int? Id_Obra { get; set; }

        public int? Id_Artista { get; set; }

        public int Id_Violacion { get; set; }
        public string Detalle { get; set; }

        public DateTime Fecha { get; set; }


        public virtual Obra IdObraNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }

        public virtual ViolacionesObra IdViolacionesObraNavigation { get; set; }
    }
}
