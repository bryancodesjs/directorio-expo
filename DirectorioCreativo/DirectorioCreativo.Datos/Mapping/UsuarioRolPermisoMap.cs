using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class UsuarioRolPermisoMap : IEntityTypeConfiguration<UsuarioRolPermiso>
    {
        public void Configure(EntityTypeBuilder<UsuarioRolPermiso> builder)
        {
            builder.ToTable("usuarioRol_permiso");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.IdPermiso).HasColumnName("id_permiso");

            builder.Property(e => e.Id_UsuarioRol).HasColumnName("id_usuarioRol");

            builder.HasOne(d => d.IdPermisoNavigation)
                .WithMany(p => p.RolesPermisos)
                .HasForeignKey(d => d.IdPermiso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolesPermiso_PERMISOS");

            builder.HasOne(d => d.IdRolesNavigation)
                .WithMany(p => p.RolesPermisos)
                .HasForeignKey(d => d.Id_UsuarioRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_usuarioRol_permiso_usuario_rol");
        }
    }
}
