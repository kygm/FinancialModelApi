﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using FinancialAPI.Models;

namespace FinancialAPI.Data
{
    public partial class FinancialModelContext : DbContext
    {
        public FinancialModelContext()
        {
        }

        public FinancialModelContext(DbContextOptions<FinancialModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<HistoricalSector> HistoricalSectors { get; set; }
        public virtual DbSet<HistoricalSecurity> HistoricalSecurities { get; set; }
        public virtual DbSet<Sector> Sectors { get; set; }
        public virtual DbSet<Security> Securities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HistoricalSector>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("HistoricalSector");

                entity.Property(e => e.DateCalculated).HasColumnType("datetime");

                entity.Property(e => e.ID).ValueGeneratedOnAdd();

                entity.Property(e => e.SectorMovement).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Sector)
                    .WithMany()
                    .HasForeignKey(d => d.SectorID)
                    .HasConstraintName("FK__Historica__Secto__2A4B4B5E");
            });

            modelBuilder.Entity<HistoricalSecurity>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("HistoricalSecurity");

                entity.Property(e => e.DateCalculated).HasColumnType("datetime");

                entity.Property(e => e.ID).ValueGeneratedOnAdd();

                entity.Property(e => e.SecurityMovement).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Security)
                    .WithMany()
                    .HasForeignKey(d => d.SecurityID)
                    .HasConstraintName("FK__Historica__Secur__286302EC");
            });

            modelBuilder.Entity<Sector>(entity =>
            {
                entity.Property(e => e.SectorName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Security>(entity =>
            {
                entity.Property(e => e.SecurityDate).HasColumnType("datetime");

                entity.Property(e => e.SecurityName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SecurityPrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.SecurityTicker)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Sector)
                    .WithMany(p => p.Securities)
                    .HasForeignKey(d => d.SectorID)
                    .HasConstraintName("FK__Securitie__Secto__267ABA7A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}