using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class ServiciosAdicionalMap : IEntityTypeConfiguration<ServiciosAdicional>
    {
        public void Configure(EntityTypeBuilder<ServiciosAdicional> builder)
        {
            builder.ToTable("servicios_adicional");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.Descripcion)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("descripcion");

        }
    }
}
