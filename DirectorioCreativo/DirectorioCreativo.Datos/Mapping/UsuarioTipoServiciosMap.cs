using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class UsuarioTipoServiciosMap : IEntityTypeConfiguration<Usuario_Tipo_Servicios>
    {
        public void Configure(EntityTypeBuilder<Usuario_Tipo_Servicios> builder)
        {
            builder.ToTable("usuario_tipo_servicios")
                   .HasKey(u => u.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Id_Usuario).HasColumnName("id_usuario");
            builder.Property(e => e.Id_Tipo_Servicio).HasColumnName("id_tipo_servicio");

            builder.HasOne(d => d.IdUsuarioNavigation)
                .WithMany(p => p.IdUsuario_Tipo_ServiciosNavigation)
                .HasForeignKey(d => d.Id_Usuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_usuario_tipo_servicios_usuario");

            builder.HasOne(d => d.IdTipoServiciosNavigation)
                .WithMany(p => p.IdUsuario_Tipo_ServiciosNavigation)
                .HasForeignKey(d => d.Id_Tipo_Servicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_usuario_tipo_servicio_tipo_servicio");


        }
    }
}
