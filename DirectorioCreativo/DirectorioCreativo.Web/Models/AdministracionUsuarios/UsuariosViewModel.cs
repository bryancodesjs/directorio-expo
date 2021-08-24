using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Models.AdministracionUsuarios
{
    public class UsuariosViewModel
    {
        public int Id { get; set; }
        public int IdPerfil { get; set; }
        public string artista { get; set; }
        public string email { get; set; }
        public DateTime ingreso { get; set; }
        public string Profesion { get; set; }
        public int? publicaciones { get; set; }
        public int? Visitas { get; set; }
        public int? Valoraciones { get; set; }
        public bool? Bloqueado { get; set; }
    }
}
