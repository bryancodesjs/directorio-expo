﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public  class Rango_Edad
    {
        public int Id {  get; set; }
        public string Nombre { get; set; }
        public virtual ICollection<Usuario> IdUsuarioNavigation { get; set; }
    }
}
