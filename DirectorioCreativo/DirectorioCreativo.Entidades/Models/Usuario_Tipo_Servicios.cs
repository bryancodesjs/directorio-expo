using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public class Usuario_Tipo_Servicios
    {
        public int Id { get; set; }
        public int Id_Usuario { get; set; }
        public int Id_Tipo_Servicio { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual TipoServicios IdTipoServiciosNavigation { get; set; }

    }
}
