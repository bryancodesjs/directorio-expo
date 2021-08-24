using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class TipoServiciosMap : IEntityTypeConfiguration<TipoServicios>
    {
        public void Configure(EntityTypeBuilder<TipoServicios> builder)
        {
            builder.ToTable("tipo_servicios")
                  .HasKey(u => u.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Nombre)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("nombre");

        }
    }
}
