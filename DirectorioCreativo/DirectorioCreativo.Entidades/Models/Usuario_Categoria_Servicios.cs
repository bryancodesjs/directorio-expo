using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public class Usuario_Categoria_Servicios
    {
        public int Id { get;set;}
        public int Id_Usuario { get;set;}
        public int Id_Categorias_Servicio { get;set;}
        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual CategoriaServicios IdCategoriaServiciosNavigation { get; set; }
    }
}
