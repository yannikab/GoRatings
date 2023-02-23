﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GoRatings.DataAccess.Models;

public partial class GoRatingsContext : DbContext
{
    public GoRatingsContext()
    {
    }

    public GoRatingsContext(DbContextOptions<GoRatingsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EntityBase> EntityBases { get; set; } = null!;
    public virtual DbSet<Property> Properties { get; set; } = null!;
    public virtual DbSet<Rating> Ratings { get; set; } = null!;
    public virtual DbSet<RealEstateAgent> RealEstateAgents { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(Settings.Instance.ConnectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EntityBase>(entity =>
        {
            entity.ToTable("EntityBase");

            entity.HasIndex(e => e.PropertyId, "IX_EntityBase_Unique_PropertyId")
                .IsUnique();

            entity.HasIndex(e => e.RealEstateAgentId, "IX_EntityBase_Unique_RealEstateAgentId")
                .IsUnique();

            entity.Property(e => e.Code).HasMaxLength(50);

            entity.Property(e => e.CreatedDt)
                .HasColumnType("datetime")
                .HasColumnName("CreatedDT");

            entity.Property(e => e.Description).HasMaxLength(4000);

            entity.HasOne(d => d.Property)
                .WithOne(p => p.EntityBase)
                .HasForeignKey<EntityBase>(d => d.PropertyId)
                .HasConstraintName("FK_EntityBase_Property");

            entity.HasOne(d => d.RealEstateAgent)
                .WithOne(p => p.EntityBase)
                .HasForeignKey<EntityBase>(d => d.RealEstateAgentId)
                .HasConstraintName("FK_EntityBase_RealEstateAgent");
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.ToTable("Property");

            entity.Property(e => e.Address).HasMaxLength(255);

            entity.Property(e => e.City).HasMaxLength(100);

            entity.Property(e => e.ListingPrice).HasColumnType("decimal(18, 0)");

            entity.Property(e => e.State).HasMaxLength(100);

            entity.Property(e => e.ZipCode).HasMaxLength(50);
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.ToTable("Rating");

            entity.Property(e => e.CreatedDt)
                .HasColumnType("datetime")
                .HasColumnName("CreatedDT");

            entity.Property(e => e.GivenRating).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Entity)
                .WithMany(p => p.Ratings)
                .HasForeignKey(d => d.EntityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rating_EntityBase");
        });

        modelBuilder.Entity<RealEstateAgent>(entity =>
        {
            entity.ToTable("RealEstateAgent");

            entity.Property(e => e.BrokerageFirm).HasMaxLength(100);

            entity.Property(e => e.Email).HasMaxLength(255);

            entity.Property(e => e.FirstName).HasMaxLength(100);

            entity.Property(e => e.LastName).HasMaxLength(100);

            entity.Property(e => e.LicenseNumber).HasMaxLength(50);

            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
