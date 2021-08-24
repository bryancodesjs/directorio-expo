using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class RechazadosMap : IEntityTypeConfiguration<Rechazados>
    {
        public void Configure(EntityTypeBuilder<Rechazados> builder)
        {
            builder.ToTable("rechazados");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.Id_Violacion).HasColumnName("id_violacion");

            builder.Property(e => e.Id_Solicitud_Perfil).HasColumnName("id_solicitud_perfil");

            builder.Property(e => e.Id_Usuario).HasColumnName("id_usuario");

            builder.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");

            builder.Property(e => e.Detalle)
           .HasMaxLength(95)
           .IsUnicode(false)
           .HasColumnName("detalle");

            builder.HasOne(d => d.IdViolacionesObraNavigation)
                .WithMany(p => p.IdRechazadosNavigation)
                .HasForeignKey(d => d.Id_Violacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_rechazados_violaciones_obra");

            builder.HasOne(d => d.IdUsuarioNavigation)
              .WithMany(p => p.IdRechazadosNavigation)
              .HasForeignKey(d => d.Id_Usuario)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_rechazados_usuario");

            builder.HasOne(d => d.IdObraNavigation)
              .WithMany(p => p.IdRechazadosNavigation)
              .HasForeignKey(d => d.Id_Obras)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_rechazados_obra");

        }
    }
}

