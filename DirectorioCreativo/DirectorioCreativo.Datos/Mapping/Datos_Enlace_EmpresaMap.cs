using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class Datos_Enlace_EmpresaMap : IEntityTypeConfiguration<Datos_Enlace_Empresa>
    {
        public void Configure(EntityTypeBuilder<Datos_Enlace_Empresa> builder)
        {
            builder.ToTable("datosEnlace_Empresa");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.Nombre)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("nombre");

            builder.Property(e => e.Apellido)
                .HasMaxLength(95)
                .IsUnicode(false)
                .HasColumnName("apellido");

            builder.Property(e => e.Fecha_nacimiento)
                .HasColumnType("datetime")
                .HasColumnName("fecha_nacimiento"); ;

            builder.Property(e => e.Lugar_nacimiento)
               .HasMaxLength(95)
               .IsUnicode(false)
               .HasColumnName("lugar_nacimiento");

            builder.Property(e => e.Nacionalidad)
             .HasMaxLength(95)
             .IsUnicode(false)
             .HasColumnName("nacionalidad");

            builder.Property(e => e.Edad).HasColumnName("edad");

            builder.Property(e => e.Genero)
            .HasMaxLength(95)
            .IsUnicode(false)
            .HasColumnName("genero");

            builder.Property(e => e.Cedula_Identidad)
            .HasMaxLength(95)
            .IsUnicode(false)
            .HasColumnName("cedula_identidad");

            builder.Property(e => e.Telefono_celular)
            .HasMaxLength(95)
            .IsUnicode(false)
            .HasColumnName("telefono_celular");


            builder.Property(e => e.Telefono_fijo)
            .HasMaxLength(95)
            .IsUnicode(false)
            .HasColumnName("telefono_fijo");

            builder.Property(e => e.Direccion)
               .HasColumnType("text")
               .HasColumnName("direccion");

            builder.Property(e => e.Provincia).HasColumnName("provincia");

            builder.Property(e => e.Municipio).HasColumnName("municipio");

            builder.Property(e => e.Correo_electronico)
            .HasMaxLength(95)
            .IsUnicode(false)
            .HasColumnName("correo_electronico");
        }
    }
}
