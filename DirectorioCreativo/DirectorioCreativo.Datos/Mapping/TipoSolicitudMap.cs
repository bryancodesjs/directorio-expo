using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class TipoSolicitudMap : IEntityTypeConfiguration<TipoSolicitud>
    {
        public void Configure(EntityTypeBuilder<TipoSolicitud> builder)
        {
            builder.ToTable("tipo_solicitud");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.Nombre)
               .HasMaxLength(145)
               .IsUnicode(false)
               .HasColumnName("nombre");
        }
    }
}
