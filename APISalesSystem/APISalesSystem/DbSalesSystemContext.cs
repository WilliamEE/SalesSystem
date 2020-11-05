using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace APISalesSystem
{
    public partial class DbSalesSystemContext : DbContext
    {
        public DbSalesSystemContext()
        {
        }

        public DbSalesSystemContext(DbContextOptions<DbSalesSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<SolicitudDeAfiliacion> SolicitudDeAfiliacion { get; set; }
        public virtual DbSet<SolicitudProducto> SolicitudProducto { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=Localhost\\dsi215;Initial Catalog=DbSalesSystem;Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("categoria");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdPadre).HasColumnName("id_padre");

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPadreNavigation)
                    .WithMany(p => p.InverseIdPadreNavigation)
                    .HasForeignKey(d => d.IdPadre)
                    .HasConstraintName("FK_categoria_categoria");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("producto");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Activo).HasColumnName("activo");

                entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("id_usuario")
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.ImagenUrl)
                    .HasColumnName("imagenUrl")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Precio)
                    .HasColumnName("precio")
                    .HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.Producto)
                    .HasForeignKey(d => d.IdCategoria)
                    .HasConstraintName("FK_producto_categoria");
            });

            modelBuilder.Entity<SolicitudDeAfiliacion>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Comentario)
                    .HasColumnName("comentario")
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasColumnName("estado")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("date");

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("id_Usuario")
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.PagareUrl)
                    .HasColumnName("pagareUrl")
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.ReciboAguaUrl)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.ReciboLuzUrl)
                    .HasColumnName("reciboLuzUrl")
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.ReciboTelefonoUrl)
                    .HasColumnName("reciboTelefonoUrl")
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.ReferenciaBancariaUrl)
                    .HasColumnName("referenciaBancariaUrl")
                    .HasMaxLength(400)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SolicitudProducto>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Estado)
                    .HasColumnName("estado")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdProducto).HasColumnName("id_producto");

                entity.Property(e => e.IdProductoModificar).HasColumnName("id_producto_modificar");

                entity.Property(e => e.Tipo)
                    .HasColumnName("tipo")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Comentario)
                   .HasColumnName("comentario")
                   .HasMaxLength(100)
                   .IsUnicode(false);

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.SolicitudProductoIdProductoNavigation)
                    .HasForeignKey(d => d.IdProducto)
                    .HasConstraintName("FK_SolicitudProducto_Producto");

                entity.HasOne(d => d.IdProductoModificarNavigation)
                    .WithMany(p => p.SolicitudProductoIdProductoModificarNavigation)
                    .HasForeignKey(d => d.IdProductoModificar)
                    .HasConstraintName("FK_SolicitudProducto_ProductoMod");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
