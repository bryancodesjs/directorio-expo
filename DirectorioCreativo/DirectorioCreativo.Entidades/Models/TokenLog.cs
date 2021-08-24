using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public class TokenLog
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Token { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public DateTime FechaRegistro { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
