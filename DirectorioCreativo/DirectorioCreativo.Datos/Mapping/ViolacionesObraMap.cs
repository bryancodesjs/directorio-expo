using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class ViolacionesObraMap : IEntityTypeConfiguration<ViolacionesObra>
    {
        public void Configure(EntityTypeBuilder<ViolacionesObra> builder)
        {
            builder.ToTable("violaciones_obra");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("nombre");

        }
    }
}
