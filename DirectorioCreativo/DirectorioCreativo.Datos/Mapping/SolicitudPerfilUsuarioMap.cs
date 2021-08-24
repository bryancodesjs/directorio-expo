using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class SolicitudPerfilUsuarioMap : IEntityTypeConfiguration<SolicitudPerfilUsuario>
    {
        public void Configure(EntityTypeBuilder<SolicitudPerfilUsuario> builder)
        {
            builder.ToTable("solicitud_perfil_usuario");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.Facebook)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("facebook");

            builder.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            builder.Property(e => e.ImgBanner)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("img_banner");

            builder.Property(e => e.Instagram)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("instagram");

            builder.Property(e => e.Nombre)
               .HasMaxLength(95)
               .IsUnicode(false)
               .HasColumnName("nombre");

            builder.Property(e => e.Apellido)
               .HasMaxLength(95)
               .IsUnicode(false)
               .HasColumnName("apellido");

            builder.Property(e => e.Profesion)
               .HasMaxLength(95)
               .IsUnicode(false)
               .HasColumnName("profesion");

            builder.Property(e => e.Descripcion)
               .HasMaxLength(95)
               .IsUnicode(false)
               .HasColumnName("descripcion");

            builder.Property(e => e.Img_perfil)
             .HasMaxLength(95)
             .IsUnicode(false)
             .HasColumnName("img_perfil");

            builder.Property(e => e.Email)
              .HasMaxLength(95)
              .IsUnicode(false)
              .HasColumnName("email");

            builder.Property(e => e.Telefono)
              .HasMaxLength(95)
              .IsUnicode(false)
              .HasColumnName("telefono");

            builder.Property(e => e.Img_perfil)
              .HasMaxLength(95)
              .IsUnicode(false)
              .HasColumnName("img_perfil");

            builder.Property(e => e.Estado_solicitud).HasColumnName("estado_solicitud");

            builder.Property(e => e.Fecha_solicitud)
                .HasColumnType("datetime")
                .HasColumnName("fecha_solicitud");


            builder.Property(e => e.Valoraciones).HasColumnName("valoraciones");

            builder.Property(e => e.Visitas).HasColumnName("visitas");

            builder.Property(e => e.Youtbe)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("youtbe");

            builder.HasOne(d => d.IdUsuarioNavigation)
                .WithMany(p => p.SolicitudPerfilUsuarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SOLICITUD_PerfilUSUARIO");
        }
    }
}
