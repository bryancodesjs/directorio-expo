using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class PerfilUsuarioMap : IEntityTypeConfiguration<PerfilUsuario>
    {
        public void Configure(EntityTypeBuilder<PerfilUsuario> builder)
        {
            builder.ToTable("perfil_usuario");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.Facebook)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("facebook");

            builder.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            builder.Property(e => e.Img_perfil)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("img_perfil");

            builder.Property(e => e.ImgBanner)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("img_banner");

            builder.Property(e => e.Instagram)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("instagram");

            builder.Property(e => e.Valoraciones).HasColumnName("valoraciones");

            builder.Property(e => e.Visitas).HasColumnName("visitas");

            builder.Property(e => e.Youtbe)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("youtbe");

            builder.Property(e => e.Enlace_Paginaweb)
               .HasMaxLength(95)
               .IsUnicode(false)
               .HasColumnName("enlace_paginaweb");

            builder.Property(e => e.Twitter)
              .HasMaxLength(95)
              .IsUnicode(false)
              .HasColumnName("twitter");

            builder.Property(e => e.Linkedin)
              .HasMaxLength(95)
              .IsUnicode(false)
              .HasColumnName("linkedin");

            builder.HasOne(d => d.IdUsuarioNavigation)
                .WithMany(p => p.PerfilUsuarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PerfilUsuario_USUARIO");
        }
    }
}
