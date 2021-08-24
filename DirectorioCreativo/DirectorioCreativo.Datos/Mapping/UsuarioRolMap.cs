using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class UsuarioRolMap : IEntityTypeConfiguration<UsuarioRol>
    {
        public void Configure(EntityTypeBuilder<UsuarioRol> builder)
        {
            builder.ToTable("usuario_rol");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.IdRol).HasColumnName("id_rol");

            builder.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            builder.HasOne(d => d.IdRolNavigation)
                .WithMany(p => p.UsuarioRols)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioRol_ROLES");

            builder.HasOne(d => d.IdUsuarioNavigation)
                .WithMany(p => p.UsuarioRols)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioRol_USUARIO");
        }
    }
}
