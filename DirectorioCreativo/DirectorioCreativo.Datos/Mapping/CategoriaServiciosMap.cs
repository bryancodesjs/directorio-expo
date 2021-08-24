using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class CategoriaServiciosMap : IEntityTypeConfiguration<CategoriaServicios>
    {
        public void Configure(EntityTypeBuilder<CategoriaServicios> builder)
        {
            builder.ToTable("categoria_servicios")
                   .HasKey(u => u.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Nombre)
                    .HasMaxLength(95)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
        }
    }
}
