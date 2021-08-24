using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class ValoracionesMap : IEntityTypeConfiguration<Valoraciones>
    {
        public void Configure(EntityTypeBuilder<Valoraciones> builder)
        {
            builder.ToTable("valoraciones");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.Id_Usuario).HasColumnName("id_usuario");

            builder.Property(e => e.Id_Obra).HasColumnName("id_obra");

            builder.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");

            builder.HasOne(d => d.IdObraNavigation)
               .WithMany(p => p.valoracion)
               .HasForeignKey(d => d.Id_Obra)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_valoraciones_obra");

            builder.HasOne(d => d.IdUsuarioNavigation)
               .WithMany(p => p.valoraciones)
               .HasForeignKey(d => d.Id_Usuario)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_valoraciones_usuario");

        }
    }
}
