using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models.CustomFields
{
    /// <summary>
    /// Represents a custom field value for an entity
    /// </summary>
    public class CustomFieldValue
    {
        /// <summary>
        /// Unique identifier for the field value
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// ID of the entity this value belongs to
        /// </summary>
        [Required]
        public int EntityId { get; set; }

        /// <summary>
        /// Type of entity this value belongs to
        /// </summary>
        [Required]
        [StringLength(50)]
        public string EntityType { get; set; }

        /// <summary>
        /// Reference to the field definition
        /// </summary>
        [Required]
        public int FieldDefinitionId { get; set; }

        /// <summary>
        /// Text value (for text, textarea fields)
        /// </summary>
        public string TextValue { get; set; }

        /// <summary>
        /// Numeric value (for number, select, general-code fields)
        /// </summary>
        public decimal? NumberValue { get; set; }

        /// <summary>
        /// Date value (for date fields)
        /// </summary>
        public DateTime? DateValue { get; set; }

        /// <summary>
        /// Boolean value (for boolean fields)
        /// </summary>
        public bool? BooleanValue { get; set; }

        /// <summary>
        /// Multi-select values (stored as JSON array)
        /// </summary>
        public string SelectedOptions { get; set; }

        /// <summary>
        /// Date when the value was created
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the value was last modified
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// User who created the value
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// User who last modified the value
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// Navigation property for the field definition
        /// </summary>
        [ForeignKey("FieldDefinitionId")]
        public virtual CustomFieldDefinition FieldDefinition { get; set; }
    }
}
