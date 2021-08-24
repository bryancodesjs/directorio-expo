using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public class Obra
    {
        public int Id { get; set; }
        public int? IdPerfil { get; set; }
        public string ImgObra { get; set; }
        public string NombreObra { get; set; }
        public string DescripcionObra { get; set; }
        public string Ubicacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int Visitas { get; set; }
        public int Valoraciones { get; set; }
        public bool? EstadoObra { get; set; }

        public virtual PerfilUsuario IdPerfilNavigation { get; set; }
        public virtual ICollection<Valoraciones> valoracion { get; set; }

        public virtual ICollection<DenunciasObras> IdDenunciasObrasNavigation { get; set; }

        public virtual ICollection<Rechazados> IdRechazadosNavigation { get; set; }

    }
}
