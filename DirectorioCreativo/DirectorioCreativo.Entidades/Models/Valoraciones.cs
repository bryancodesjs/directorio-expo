using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public  class Valoraciones
    {
        public int Id { get; set; }
        public int Id_Usuario { get; set; }
        public int Id_Obra { get; set; }
        public DateTime Fecha { get; set; }

        public virtual Obra IdObraNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }

    }
}
