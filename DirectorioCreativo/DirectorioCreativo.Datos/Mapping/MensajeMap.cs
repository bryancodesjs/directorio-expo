using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class MensajeMap : IEntityTypeConfiguration<Mensaje>
    {
        public void Configure(EntityTypeBuilder<Mensaje> builder)
        {
            builder.ToTable("mensajes");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.IdEmisor).HasColumnName("id_emisor");

            builder.Property(e => e.IdReceptor).HasColumnName("id_receptor");
            builder.Property(e => e.IdTipoSolicitud).HasColumnName("id_tipo_solicitud");

            builder.HasOne(d => d.IdEmisorNavigation)
                .WithMany(p => p.MensajeIdEmisorNavigations)
                .HasForeignKey(d => d.IdEmisor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MENSAJES_USUARIO_Emisor");

            builder.HasOne(d => d.IdReceptorNavigation)
                .WithMany(p => p.MensajeIdReceptorNavigations)
                .HasForeignKey(d => d.IdReceptor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MENSAJES_USUARIO_Receptor");

            builder.HasOne(d => d.IdTipoSolicitudNavigation)
                .WithMany(p => p.IdMensajeNavigation)
                .HasForeignKey(d => d.IdTipoSolicitud)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mensajes_tipo_solicitud");
        }
    }
}
