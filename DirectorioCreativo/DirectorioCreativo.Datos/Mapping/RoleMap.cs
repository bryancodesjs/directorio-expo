using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("nombre");
        }
    }
}
