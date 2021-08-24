using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class DenunciasObraMap : IEntityTypeConfiguration<DenunciasObras>
    {
        public void Configure(EntityTypeBuilder<DenunciasObras> builder)
        {
            builder.ToTable("denuncias_obras");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.Id_Obra).HasColumnName("id_obra");

            builder.Property(e => e.Id_Artista).HasColumnName("id_artista");

            builder.Property(e => e.Id_Violacion).HasColumnName("id_violacion");

            builder.Property(e => e.Detalle)
               .HasMaxLength(95)
               .IsUnicode(false)
               .HasColumnName("detalles");

            builder.Property(e => e.Fecha)
               .HasColumnType("datetime")
               .HasColumnName("fecha");

            builder.HasOne(d => d.IdObraNavigation)
                .WithMany(p => p.IdDenunciasObrasNavigation)
                .HasForeignKey(d => d.Id_Obra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_denuncias_obras_obra");

            builder.HasOne(d => d.IdUsuarioNavigation)
                .WithMany(p => p.IdDenunciasObrasNavigation)
                .HasForeignKey(d => d.Id_Artista)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_denuncias_obras_usuario");

            builder.HasOne(d => d.IdViolacionesObraNavigation)
               .WithMany(p => p.IdDenunciasObrasNavigation)
               .HasForeignKey(d => d.Id_Violacion)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_denuncias_obras_violaciones_obra");
        }
    }
 }

