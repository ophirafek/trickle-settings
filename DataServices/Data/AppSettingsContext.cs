using AppSettings.API.Models;
using Microsoft.EntityFrameworkCore;
using Model.Models.CustomFields;

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
        /// DbSet for CustomFieldGroups
        /// </summary>
        public DbSet<CustomFieldGroup> CustomFieldGroups { get; set; }

        /// <summary>
        /// DbSet for CustomFieldDefinitions
        /// </summary>
        public DbSet<CustomFieldDefinition> CustomFieldDefinitions { get; set; }

        /// <summary>
        /// DbSet for CustomFieldOptions
        /// </summary>
        public DbSet<CustomFieldOption> CustomFieldOptions { get; set; }

        /// <summary>
        /// DbSet for CustomFieldValues
        /// </summary>
        public DbSet<CustomFieldValue> CustomFieldValues { get; set; }

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
                    .HasDatabaseName("UQ_GeneralCodes_TypeNumber_Language");

                entity.HasIndex(e => new { e.CodeType, e.LanguageCode })
                    .HasDatabaseName("IX_GeneralCodes_CodeType_LanguageCode");

                entity.Property(e => e.OpeningRegDate)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.ActiveFlag)
                    .HasDefaultValue(true);
            });

            // Configure CustomFieldGroup entity
            modelBuilder.Entity<CustomFieldGroup>(entity =>
            {
                entity.HasIndex(e => new { e.EntityType, e.Name })
                    .IsUnique()
                    .HasDatabaseName("UQ_CustomFieldGroups_EntityType_Name");

                entity.HasIndex(e => e.EntityType)
                    .HasDatabaseName("IX_CustomFieldGroups_EntityType");

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);
            });

            // Configure CustomFieldDefinition entity
            modelBuilder.Entity<CustomFieldDefinition>(entity =>
            {
                entity.HasIndex(e => new { e.EntityType, e.Name })
                    .IsUnique()
                    .HasDatabaseName("UQ_CustomFieldDefinitions_EntityType_Name");

                entity.HasIndex(e => e.EntityType)
                    .HasDatabaseName("IX_CustomFieldDefinitions_EntityType");

                entity.HasIndex(e => e.GroupId)
                    .HasDatabaseName("IX_CustomFieldDefinitions_GroupId");

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.IsVisible)
                    .HasDefaultValue(true);

                // Configure relationship with CustomFieldGroup
                entity.HasOne(d => d.Group)
                    .WithMany(g => g.Fields)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure CustomFieldOption entity
            modelBuilder.Entity<CustomFieldOption>(entity =>
            {
                entity.HasIndex(e => new { e.FieldDefinitionId, e.Value })
                    .IsUnique()
                    .HasDatabaseName("UQ_CustomFieldOptions_FieldDefinition_Value");

                entity.HasIndex(e => e.FieldDefinitionId)
                    .HasDatabaseName("IX_CustomFieldOptions_FieldDefinitionId");

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                // Configure relationship with CustomFieldDefinition
                entity.HasOne(o => o.FieldDefinition)
                    .WithMany(d => d.Options)
                    .HasForeignKey(o => o.FieldDefinitionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure CustomFieldValue entity
            modelBuilder.Entity<CustomFieldValue>(entity =>
            {
                entity.HasIndex(e => new { e.EntityType, e.EntityId, e.FieldDefinitionId })
                    .IsUnique()
                    .HasDatabaseName("UQ_CustomFieldValues_Entity_FieldDefinition");

                entity.HasIndex(e => new { e.EntityType, e.EntityId })
                    .HasDatabaseName("IX_CustomFieldValues_Entity");

                entity.HasIndex(e => e.FieldDefinitionId)
                    .HasDatabaseName("IX_CustomFieldValues_FieldDefinition");

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETDATE()");

                // Configure relationship with CustomFieldDefinition
                entity.HasOne(v => v.FieldDefinition)
                    .WithMany(d => d.Values)
                    .HasForeignKey(v => v.FieldDefinitionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}