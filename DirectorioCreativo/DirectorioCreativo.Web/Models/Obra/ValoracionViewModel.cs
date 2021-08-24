using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Models.Obra
{
    public class ValoracionViewModel
    {
        public DateTime Fecha { get; set; }
        public int Id_Obra { get; set; }
        public int IdUsuario { get; set; }
    }
}
