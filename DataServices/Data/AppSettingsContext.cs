using AppSettings.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace AppSettings.API.Data
{
    /// <summary>
    /// Database context for the application settings
    /// </summary>
    public class AppSettingsContext : DbContext
    {
        /// <summary>
        /// Constructor that accepts DbContextOptions
        /// </summary>
        /// <param name="options">Database context options</param>
        public AppSettingsContext(DbContextOptions<AppSettingsContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// DbSet for Countries
        /// </summary>
        public DbSet<Country> Countries { get; set; }

        /// <summary>
        /// Configure model relationships and constraints
        /// </summary>
        /// <param name="modelBuilder">Model builder instance</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Country entity
            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasIndex(e => e.CountryCode)
                    .IsUnique();

                entity.HasIndex(e => e.CountryName)
                    .IsUnique();

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed some common countries
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    CountryCode = "US",
                    CountryName = "United States",
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                },
                new Country
                {
                    Id = 2,
                    CountryCode = "CA",
                    CountryName = "Canada",
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                },
                new Country
                {
                    Id = 3,
                    CountryCode = "UK",
                    CountryName = "United Kingdom",
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                },
                new Country
                {
                    Id = 4,
                    CountryCode = "DE",
                    CountryName = "Germany",
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                },
                new Country
                {
                    Id = 5,
                    CountryCode = "FR",
                    CountryName = "France",
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                }
            );
        }
    }
}