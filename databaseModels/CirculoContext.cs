using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Database
{
    public partial class CirculoContext : DbContext
    {
        public CirculoContext()
        {
        }

        public CirculoContext(DbContextOptions<CirculoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PharmacyNearby> PharmacyNearby { get; set; } = null!;
        public virtual DbSet<PharmacyVisited> PharmacyVisited { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<PharmacyNearby>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v1()");

                entity.Property(e => e.PharmacyName).HasMaxLength(100);
            });

            modelBuilder.Entity<PharmacyVisited>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v1()");

                entity.Property(e => e.PharmacyName).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
