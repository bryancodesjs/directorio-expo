using DirectorioCreativo.Datos.Mapping;
using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos
{
    public  class DbContextDirectorioCreactivo:DbContext
    {
        public virtual DbSet<DetalleMensaje> DetalleMensajes { get; set; }
        public virtual DbSet<Mensaje> Mensajes { get; set; }
        public virtual DbSet<Obra> Obras { get; set; }
        public virtual DbSet<PerfilUsuario> PerfilUsuarios { get; set; }
        public virtual DbSet<Permiso> Permisos { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UsuarioRolPermiso> UsuarioRolPermisos { get; set; }
        public virtual DbSet<SolicitudPerfilUsuario> SolicitudPerfilUsuarios { get; set; }
        public virtual DbSet<TokenLog> TokenLogs { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<UsuarioRol> UsuarioRols { get; set; }
        public virtual DbSet<Valoraciones> Valoraciones { get; set; }
        public virtual DbSet<DenunciasObras> DenunciasObras { get; set; }
        public virtual DbSet<ViolacionesObra> ViolacionesObras { get; set; }
        public virtual DbSet<Rechazados> Rechazados { get; set; }
        public virtual DbSet<TipoSolicitud> TipoSolicitud { get; set; }
        public virtual DbSet<TipoServicios> TipoServicios { get; set; }
        public virtual DbSet<TipoUsuarios> TipoUsuarios { get; set; }
        public virtual DbSet<Usuario_Tipo_Servicios> Usuario_Tipo_Servicios { get; set; }
        public virtual DbSet<Datos_Enlace_Empresa> Datos_Enlace_Empresa { get; set; }
        public virtual DbSet<NivelEducativo> NivelEducativo { get; set; }
        public virtual DbSet<ServiciosAdicional> ServiciosAdicional { get; set; }
        public virtual DbSet<Usuario_Categoria_Servicios> Usuario_Categoria_Servicios { get; set; }
        public virtual DbSet<CategoriaServicios> CategoriaServicios { get; set; }
        public virtual DbSet<Provincia> Provincia { get; set; }
        public virtual DbSet<Municipio> Municipio { get; set; }
        public virtual DbSet<Rango_Edad> Rango_Edad { get; set; }

        public DbContextDirectorioCreactivo(DbContextOptions<DbContextDirectorioCreactivo> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new DetalleMensajeMap());
            modelBuilder.ApplyConfiguration(new MensajeMap());
            modelBuilder.ApplyConfiguration(new ObraMap());
            modelBuilder.ApplyConfiguration(new PerfilUsuarioMap());
            modelBuilder.ApplyConfiguration(new PermisoMap());
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new UsuarioRolPermisoMap());
            modelBuilder.ApplyConfiguration(new SolicitudPerfilUsuarioMap());
            modelBuilder.ApplyConfiguration(new TokenLogMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new UsuarioRolMap());
            modelBuilder.ApplyConfiguration(new ValoracionesMap());
            modelBuilder.ApplyConfiguration(new DenunciasObraMap());
            modelBuilder.ApplyConfiguration(new ViolacionesObraMap());
            modelBuilder.ApplyConfiguration(new RechazadosMap());
            modelBuilder.ApplyConfiguration(new TipoSolicitudMap());
            modelBuilder.ApplyConfiguration(new TipoServiciosMap());
            modelBuilder.ApplyConfiguration(new TipoUsuariosMap());
            modelBuilder.ApplyConfiguration(new UsuarioTipoServiciosMap());
            modelBuilder.ApplyConfiguration(new Datos_Enlace_EmpresaMap());
            modelBuilder.ApplyConfiguration(new NivelEducativoMap());
            modelBuilder.ApplyConfiguration(new ServiciosAdicionalMap());
            modelBuilder.ApplyConfiguration(new UsuarioCategoriaServiciosMap());
            modelBuilder.ApplyConfiguration(new CategoriaServiciosMap());
            modelBuilder.ApplyConfiguration(new ProvinciaMap());
            modelBuilder.ApplyConfiguration(new MunicipioMap());
            modelBuilder.ApplyConfiguration(new Rango_EdadMap());
        }
    }
}
