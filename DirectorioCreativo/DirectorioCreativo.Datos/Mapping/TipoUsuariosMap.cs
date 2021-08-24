using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class TipoUsuariosMap : IEntityTypeConfiguration<TipoUsuarios>
    {
        public void Configure(EntityTypeBuilder<TipoUsuarios> builder)
        {
            builder.ToTable("tipo_usuarios")
                  .HasKey(u => u.Id);
            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.Nombre)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("nombre");
        }
    }
}
