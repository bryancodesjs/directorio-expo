using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class NivelEducativoMap : IEntityTypeConfiguration<NivelEducativo>
    {
        public void Configure(EntityTypeBuilder<NivelEducativo> builder)
        {
            builder.ToTable("nivel_educativo");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.Descripcion)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        }
    }
}
