using DirectorioCreativo.Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorioCreativo.Datos.Mapping
{
    public class TokenLogMap : IEntityTypeConfiguration<TokenLog>
    {
        public void Configure(EntityTypeBuilder<TokenLog> builder)
        {
            builder.ToTable("token_log");

            builder.Property(e => e.Id).HasColumnName("id");

            builder.Property(e => e.FechaExpiracion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_expiracion");

            builder.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");

            builder.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            builder.Property(e => e.Token)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("token");

            builder.HasOne(d => d.IdUsuarioNavigation)
                .WithMany(p => p.TokenLogs)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TokenLogs_USUARIO");
        }
    }
}
