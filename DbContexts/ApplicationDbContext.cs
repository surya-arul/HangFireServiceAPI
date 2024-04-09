using System;
using System.Collections.Generic;
using HangFireServiceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HangFireServiceAPI.DbContexts;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblClaysysEmployee> TblClaysysEmployees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblClaysysEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblClays__3214EC07B27600D7");

            entity.ToTable("tblClaysysEmployees");

            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
