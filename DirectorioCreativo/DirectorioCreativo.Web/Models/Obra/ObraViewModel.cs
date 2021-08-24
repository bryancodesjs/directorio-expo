using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Models.Obra
{
    public class ObraViewModel
    {
        public int _Id { get; set; }
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


    }
}
