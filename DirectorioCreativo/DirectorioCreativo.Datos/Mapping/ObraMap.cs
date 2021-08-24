using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class ObraMap : IEntityTypeConfiguration<Obra>
    {
        public void Configure(EntityTypeBuilder<Obra> builder)
        {
            builder.ToTable("obra");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.DescripcionObra)
                .IsRequired()
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("descripcion_obra");

            builder.Property(e => e.EstadoObra).HasColumnName("estado_obra");

            builder.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");

            builder.Property(e => e.IdPerfil).HasColumnName("id_perfil");

            builder.Property(e => e.ImgObra)
                .IsRequired()
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("img_obra");

            builder.Property(e => e.NombreObra)
                .IsRequired()
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("nombre_obra");

            builder.Property(e => e.Ubicacion)
                .IsRequired()
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("ubicacion");

            builder.Property(e => e.Valoraciones).HasColumnName("valoraciones");

            builder.Property(e => e.Visitas).HasColumnName("visitas");

            builder.HasOne(d => d.IdPerfilNavigation)
                .WithMany(p => p.Obras)
                .HasForeignKey(d => d.IdPerfil)
                .HasConstraintName("FK_OBRA_PerfilUsuario");
        }
    }
}
