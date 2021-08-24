using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class UsuarioCategoriaServiciosMap : IEntityTypeConfiguration<Usuario_Categoria_Servicios>
    {
        public void Configure(EntityTypeBuilder<Usuario_Categoria_Servicios> builder)
        {
            builder.ToTable("usuario_categoria_servicios")
                   .HasKey(u => u.Id);
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Id_Usuario).HasColumnName("id_usuario");
            builder.Property(e => e.Id_Categorias_Servicio).HasColumnName("id_categorias_servicio");

            builder.HasOne(d => d.IdUsuarioNavigation)
               .WithMany(p => p.IdUsuario_Categoria_ServiciosNavigation)
               .HasForeignKey(d => d.Id_Usuario)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_usuario_categoria_servicios_usuario");

            builder.HasOne(d => d.IdCategoriaServiciosNavigation)
                .WithMany(p => p.IdUsuario_Categoria_ServiciosNavigation)
                .HasForeignKey(d => d.Id_Categorias_Servicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioRol_USUARIO");
        }
    }
}
