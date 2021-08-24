using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuario")
                   .HasKey(u => u.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.IdProvincia).HasColumnName("id_provincia");
            builder.Property(e => e.Nombre)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("nombre");

            builder.Property(e => e.Apellido)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("apellido");
            builder.Property(e => e.Profesion)
                .HasMaxLength(190)
                .IsUnicode(false)
                .HasColumnName("profesion");

            builder.Property(e => e.DescripcionGeneral)
               .HasColumnType("text")
               .HasColumnName("descripcion_general");
            builder.Property(e => e.Telefono)
               .HasMaxLength(20)
               .IsUnicode(false)
               .HasColumnName("telefono");

            builder.Property(e => e.Email)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("email");
            builder.Property(e => e.Clave)
               .HasColumnName("clave");

            builder.Property(e => e.SaltClave)
                .HasColumnName("salt_clave");

            builder.Property(e => e.FechaIngreso)
                .HasColumnType("datetime")
                .HasColumnName("fecha_ingreso");

            builder.Property(e => e.Habilitado).HasColumnName("habilitado");

            builder.Property(e => e.Bloqueado).HasColumnName("bloqueado");

            builder.Property(e => e.Nacionalidad)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("nacionalidad");

            builder.Property(e => e.Fecha_nacimiento)
                .HasColumnType("datetime")
                .HasColumnName("fecha_nacimiento");

            builder.Property(e => e.Lugar_nacimiento)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("lugar_nacimiento");

            builder.Property(e => e.Id_Rango_edad).HasColumnName("id_rango_edad");

            builder.Property(e => e.Genero)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("genero");

            builder.Property(e => e.Telefono_fijo).HasColumnName("telefono_fijo");

            builder.Property(e => e.Telefono_celular).HasColumnName("telefono_celular");

            builder.Property(e => e.Direccion_residencial)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("direccion_residencial");

            builder.Property(e => e.Id_Provincias).HasColumnName("id_provincias");

            builder.Property(e => e.Id_Municipio).HasColumnName("id_municipio");

            builder.Property(e => e.Correo_empresa)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("correo_empresa");

            builder.Property(e => e.Id_nivel_educativo).HasColumnName("id_nivel_educativo");

            builder.Property(e => e.Fecha_constituida_empresa)
                .HasColumnType("datetime")
                .HasColumnName("fecha_constituida_empresa");

            builder.Property(e => e.Direccion_empresa)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("direccion_empresa");

            builder.Property(e => e.Nombre_empresa)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("nombre_empresa");

            builder.Property(e => e.Rnc)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("rnc");

            builder.Property(e => e.Id_tipo_usuarios).HasColumnName("id_tipo_usuarios");

            builder.Property(e => e.Id_servicios_adicional).HasColumnName("id_servicios_adicional");
            builder.Property(e => e.Tipo_Registro)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("tipo_registro");

            builder.Property(e => e.DatosCompletado).HasColumnName("datosCompletado");

            builder.Property(e => e.ServiciosCompletado).HasColumnName("serviciosCompletado");

            builder.Property(e => e.ContactoCompletado).HasColumnName("contactoCompletado");

            builder.HasOne(d => d.IdNivelEducativoNavigation)
                .WithMany(p => p.IdUsuarioNavigation)
                .HasForeignKey(d => d.Id_nivel_educativo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_usuario_nivel_educativo");

            builder.HasOne(d => d.IdTipoUsuariosNavigation)
                .WithMany(p => p.IdUsuarioNavigation)
                .HasForeignKey(d => d.Id_tipo_usuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_usuario_tipo_usuario");

            builder.HasOne(d => d.IdServiciosAdicionalNavigation)
                .WithMany(p => p.IdUsuarioNavigation)
                .HasForeignKey(d => d.Id_servicios_adicional)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_usuario_servicios_adicional");

            builder.HasOne(d => d.IdRango_EdadNavigation)
                .WithMany(p => p.IdUsuarioNavigation)
                .HasForeignKey(d => d.Id_Rango_edad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_usuario_rango_edad");

            builder.HasOne(d => d.IdProvinciaNavigation)
               .WithMany(p => p.IdUsuarioNavigation)
               .HasForeignKey(d => d.Id_Provincias)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_usuario_provincia");

            builder.HasOne(d => d.IdMunicipioNavigation)
               .WithMany(p => p.IdUsuarioNavigation)
               .HasForeignKey(d => d.Id_Municipio)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_usuario_municipio");

        }
    }
}
