using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public  class CategoriaServicios
    {
        public int Id {  get; set; }    
        public string Nombre { get; set; }
        public virtual ICollection<Usuario_Categoria_Servicios> IdUsuario_Categoria_ServiciosNavigation { get; set; }

    }
}
