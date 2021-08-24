using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Models.Obra
{
    public class ListObrasViewModel
    {
        public int idObra { get; set; }
        public int Id { get; set; }
        public string ImgObra { get; set; }
        public string NombreObra { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int Valoraciones { get; set; }
        public int Visitas { get; set; }
        public string artista { get; set; }
        public int perfilUsuario { get; set; }
        public bool autenticado { get; set; }
        public bool valorado { get; set; }
    }
}
