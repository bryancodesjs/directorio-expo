using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public  class ViolacionesObra
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public virtual ICollection<DenunciasObras> IdDenunciasObrasNavigation { get; set; }
        public virtual ICollection<Rechazados> IdRechazadosNavigation { get; set; }

    }
}
