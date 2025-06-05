using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppSettings.API.Models;

namespace Model.Models.CustomFields
{
    public class CustomFieldDefinition
    {
        /// <summary>
        /// Unique identifier for the custom field definition
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Entity type this field belongs to (e.g., 'lead', 'company', etc.)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string EntityType { get; set; }

        /// <summary>
        /// Unique name for the field (used as a reference in code)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Display name shown to users
        /// </summary>
        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Optional description for the field
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Type of the field (text, number, date, etc.)
        /// </summary>
        [Required]
        [StringLength(20)]
        public string FieldType { get; set; }

        /// <summary>
        /// Flag indicating if the field is required
        /// </summary>
        [Required]
        public bool IsRequired { get; set; }

        /// <summary>
        /// Flag indicating if the field is active
        /// </summary>
        [Required]
        public bool IsActive { get; set; }

        /// <summary>
        /// Order in which to display the field
        /// </summary>
        [Required]
        public int SortOrder { get; set; }

        /// <summary>
        /// Default value for the field (stored as JSON)
        /// </summary>
        public string? DefaultValue { get; set; }

        /// <summary>
        /// Minimum value (for number fields)
        /// </summary>
        public decimal? MinValue { get; set; }

        /// <summary>
        /// Maximum value (for number fields)
        /// </summary>
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// Maximum length (for text fields)
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// Regular expression for validation (for text fields)
        /// </summary>
        [StringLength(500)]
        public string? Regex { get; set; }

        /// <summary>
        /// Reference to general code type (for general code fields)
        /// </summary>
        public int? GeneralCodeType { get; set; }

        /// <summary>
        /// Group ID for organizing fields
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// Group name for backward compatibility and simple grouping
        /// </summary>
        [StringLength(50)]
        public string GroupName { get; set; }

        /// <summary>
        /// Flag indicating if the field is visible in the UI
        /// </summary>
        [Required]
        public bool IsVisible { get; set; }

        /// <summary>
        /// Date when the field was created
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the field was last modified
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// User who created the field
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// User who last modified the field
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// Navigation property for the custom field group
        /// </summary>
        [ForeignKey("GroupId")]
        public virtual CustomFieldGroup Group { get; set; }

        /// <summary>
        /// Navigation property for the field options
        /// </summary>
        public virtual ICollection<CustomFieldOption> Options { get; set; } = new List<CustomFieldOption>();

        /// <summary>
        /// Navigation property for field values
        /// </summary>
        public virtual ICollection<CustomFieldValue> Values { get; set; } = new List<CustomFieldValue>();
    }
}
