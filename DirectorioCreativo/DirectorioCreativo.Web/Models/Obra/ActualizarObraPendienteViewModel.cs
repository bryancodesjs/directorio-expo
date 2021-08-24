using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioCreativo.Web.Models.Obra
{
    public class ActualizarObraPendienteViewModel
    {
        public int Id { get; set; }
        public string Accion { get; set; }
        public int? Id_Violacion { get; set; }
        public string Detalle { get; set; }        
    }
}
