using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RoyalPrestige_API.Models;

namespace RoyalPrestige_API.Data;

public partial class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Contrato> Contratos { get; set; }

    public virtual DbSet<Demostracione> Demostraciones { get; set; }

    public virtual DbSet<EstadosDemostracion> EstadosDemostracions { get; set; }

    public virtual DbSet<Reprogramacione> Reprogramaciones { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Vendedores> Vendedores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=royaldb;User Id=postgres;Password=root;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_clientes");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Apellido)
                .HasMaxLength(22)
                .IsFixedLength();
            entity.Property(e => e.Direccion)
                .HasMaxLength(35)
                .IsFixedLength();
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsFixedLength();
            entity.Property(e => e.Nombre)
                .HasMaxLength(22)
                .IsFixedLength();
            entity.Property(e => e.Observaciones)
                .HasMaxLength(255)
                .IsFixedLength();

            entity.HasOne(d => d.Vendedor).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.VendedorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_vendedor_id");
        });

        modelBuilder.Entity<Contrato>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_contratos");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Contrato1)
                .HasMaxLength(255)
                .IsFixedLength()
                .HasColumnName("Contrato");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Contratos)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cliente_id");

            entity.HasOne(d => d.Vendedor).WithMany(p => p.Contratos)
                .HasForeignKey(d => d.VendedorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_vendedor_id");
        });

        modelBuilder.Entity<Demostracione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_demostraciones");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Cliente).WithMany(p => p.Demostraciones)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_clientes_id");

            entity.HasOne(d => d.EstadoVentaNavigation).WithMany(p => p.DemostracioneEstadoVentaNavigations)
                .HasForeignKey(d => d.EstadoVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_estados_venta_id");

            entity.HasOne(d => d.EstadoVisita).WithMany(p => p.DemostracioneEstadoVisita)
                .HasForeignKey(d => d.EstadoVisitaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_estado_visita_id");
        });

        modelBuilder.Entity<EstadosDemostracion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_estados");

            entity.ToTable("EstadosDemostracion");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Estado)
                .HasMaxLength(22)
                .IsFixedLength();
        });

        modelBuilder.Entity<Reprogramacione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_reprogramaciones");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Observaciones)
                .HasMaxLength(255)
                .IsFixedLength();

            entity.HasOne(d => d.Demostracion).WithMany(p => p.Reprogramaciones)
                .HasForeignKey(d => d.DemostracionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_demostracion_id");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_role");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Rol)
                .HasMaxLength(15)
                .IsFixedLength();
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_usuarios");

            entity.HasIndex(e => e.Email, "Email").IsUnique();

            entity.HasIndex(e => e.Username, "Username").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Apellido)
                .HasMaxLength(22)
                .IsFixedLength();
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsFixedLength();
            entity.Property(e => e.Nombre)
                .HasMaxLength(22)
                .IsFixedLength();
            entity.Property(e => e.Password)
                .HasMaxLength(265)
                .IsFixedLength();
            entity.Property(e => e.ProfilePic)
                .HasMaxLength(265)
                .IsFixedLength();
            entity.Property(e => e.Username)
                .HasMaxLength(22)
                .IsFixedLength();

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .HasConstraintName("fk_rol_id");
        });

        modelBuilder.Entity<Vendedores>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_vendedores");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");

            entity.HasOne(d => d.Vendedor).WithMany(p => p.Vendedores)
                .HasForeignKey(d => d.VendedorId)
                .HasConstraintName("fk_vendedor_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
