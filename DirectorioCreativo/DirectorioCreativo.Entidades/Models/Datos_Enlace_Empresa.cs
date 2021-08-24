using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public class Datos_Enlace_Empresa
    {
        public int Id {  get; set; }
        public string Nombre  { get; set; }
        public string Apellido { get; set; }
        public DateTime Fecha_nacimiento { get; set; }
        public string Lugar_nacimiento { get; set; }
        public string Nacionalidad  { get; set; }
        public string Edad  { get; set; }
        public string Genero { get; set; }
        public string Cedula_Identidad { get; set; }
        public string Telefono_celular { get; set; }
        public string Telefono_fijo { get; set; }
        public string Direccion { get; set; }
        public int Provincia { get; set; }
        public int Municipio { get; set; }
        public string Correo_electronico { get; set; }
        public int Id_usuario { get; set; }

    }
}
