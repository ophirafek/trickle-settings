using AppSettings.API.Models;
using Microsoft.EntityFrameworkCore;

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
        /// DbSet for GeneralCodes
        /// </summary>
        public DbSet<GeneralCode> GeneralCodes { get; set; }

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

            // Configure GeneralCode entity
            modelBuilder.Entity<GeneralCode>(entity =>
            {
                entity.HasIndex(e => new { e.CodeType, e.CodeNumber, e.LanguageCode })
                    .IsUnique()
                    .HasName("UQ_GeneralCodes_TypeNumber_Language");

                entity.HasIndex(e => new { e.CodeType, e.LanguageCode })
                    .HasName("IX_GeneralCodes_CodeType_LanguageCode");

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);
            });
        }
    }
}