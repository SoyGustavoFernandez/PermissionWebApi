using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API_Models.Models;

public partial class BackendSrContext : DbContext
{
    public BackendSrContext()
    {
    }

    public BackendSrContext(DbContextOptions<BackendSrContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblPermission> TblPermissions { get; set; }

    public virtual DbSet<TblPermissionType> TblPermissionTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Server=GUSTAVO-FERNAND\\LOCALHOST;Database=BackendSr;Integrated Security=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblPermission>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Table where permits are recorded."));

            entity.Property(e => e.Id).HasComment("Unique ID");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Register status.");
            entity.Property(e => e.EmployeeForename)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("Employee Forename.");
            entity.Property(e => e.EmployeeSurename)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("Employee Surename.");
            entity.Property(e => e.PermissionDate)
                .HasComment("Permission granted on Date.")
                .HasColumnType("datetime");
            entity.Property(e => e.PermissionType).HasComment("Permission Type.");

            entity.HasOne(d => d.PermissionTypeNavigation).WithMany(p => p.TblPermissions)
                .HasForeignKey(d => d.PermissionType)
                .HasConstraintName("FK_TblPermissions_TblPermissionTypes");
        });

        modelBuilder.Entity<TblPermissionType>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Table showing the types of permits."));

            entity.Property(e => e.Id).HasComment("Unique ID.");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Register status.");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("Permission description.");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
