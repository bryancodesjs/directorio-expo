using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Entidades.Models
{
    public  class Usuario
    {
        public Usuario()
        {
            MensajeIdEmisorNavigations = new HashSet<Mensaje>();
            MensajeIdReceptorNavigations = new HashSet<Mensaje>();
            PerfilUsuarios = new HashSet<PerfilUsuario>();
            SolicitudPerfilUsuarios = new HashSet<SolicitudPerfilUsuario>();
            TokenLogs = new HashSet<TokenLog>();
            UsuarioRols = new HashSet<UsuarioRol>();
        }

        public int Id { get; set; }
        public int? IdProvincia { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Profesion { get; set; }
        public string DescripcionGeneral { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
        public string SaltClave { get; set; }
        public DateTime FechaIngreso { get; set; }
        public bool? Habilitado { get; set; }
        public bool? Bloqueado { get; set; }
        public string Nacionalidad {  get; set; }
        public DateTime? Fecha_nacimiento {  get; set; }
        public DateTime? Lugar_nacimiento {  get; set; }
        public int? Id_Rango_edad { get; set; }
        public string Genero { get; set; }
        public int? Cedula_identidad {  get; set; }
        public int? Telefono_fijo {  get; set; } 
        public int? Telefono_celular {  get; set; }
        public string Direccion_residencial {  get; set; }  
        public int? Id_Provincias { get; set; }
        public int? Id_Municipio { get; set; }
        public string Correo_electronico { get; set; }
        public string Correo_empresa { get; set;}
        public int? Id_nivel_educativo {  get; set;}
        public DateTime? Fecha_constituida_empresa { get; set; }
        public string Direccion_empresa { get; set; }
        public string Nombre_empresa { get; set; }
        public string Rnc { get; set; }
        public int? Id_tipo_usuarios { get; set; }
        public int? Id_servicios_adicional {  get; set; }
        public string Tipo_Registro {  get; set; }
        public bool? DatosCompletado {  get; set; }
        public bool? ServiciosCompletado { get; set; }
        public bool? ContactoCompletado { get; set; }

        public virtual ICollection<Mensaje> MensajeIdEmisorNavigations { get; set; }
        public virtual ICollection<Mensaje> MensajeIdReceptorNavigations { get; set; }
        public virtual ICollection<PerfilUsuario> PerfilUsuarios { get; set; }
        public virtual ICollection<SolicitudPerfilUsuario> SolicitudPerfilUsuarios { get; set; }
        public virtual ICollection<TokenLog> TokenLogs { get; set; }
        public virtual ICollection<UsuarioRol> UsuarioRols { get; set; }
        public virtual ICollection<Valoraciones> valoraciones { get; set; }
        public virtual ICollection<DenunciasObras> IdDenunciasObrasNavigation { get; set; }
        public virtual ICollection<Rechazados> IdRechazadosNavigation { get; set; }
        public virtual NivelEducativo IdNivelEducativoNavigation { get; set; }
        public virtual TipoUsuarios IdTipoUsuariosNavigation { get; set; }
        public virtual ServiciosAdicional IdServiciosAdicionalNavigation {  get; set; }
        public virtual Provincia IdProvinciaNavigation { get; set; }
        public virtual Rango_Edad IdRango_EdadNavigation { get; set; }
        public virtual Municipio IdMunicipioNavigation { get; set; }
        public virtual ICollection<Usuario_Categoria_Servicios> IdUsuario_Categoria_ServiciosNavigation { get; set; }
        public virtual ICollection<Usuario_Tipo_Servicios> IdUsuario_Tipo_ServiciosNavigation { get; set; }

    }
}
