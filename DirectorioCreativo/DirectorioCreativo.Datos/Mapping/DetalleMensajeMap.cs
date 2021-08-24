using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class DetalleMensajeMap : IEntityTypeConfiguration<DetalleMensaje>
    {
        public void Configure(EntityTypeBuilder<DetalleMensaje> builder)
        {
            builder.ToTable("detalle_mensaje");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");

            builder.Property(e => e.IdMensaje).HasColumnName("id_mensaje");

            builder.Property(e => e.Leido).HasColumnName("leido");

            builder.Property(e => e.Mensaje)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("mensaje");

            builder.Property(e => e.IdReceptor).HasColumnName("id_receptor");  

            builder.HasOne(d => d.IdMensajeNavigation)
                .WithMany(p => p.DetalleMensajes)
                .HasForeignKey(d => d.IdMensaje)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleMensaje_MENSAJES");
        }
    }
}
