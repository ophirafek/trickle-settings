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
    /// Represents a selectable option for select/multi-select custom fields
    /// </summary>
    public class CustomFieldOption
    {
        /// <summary>
        /// Unique identifier for the option
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Reference to the field definition this option belongs to
        /// </summary>
        [Required]
        public int FieldDefinitionId { get; set; }

        /// <summary>
        /// Value of the option (used in code)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Value { get; set; }

        /// <summary>
        /// Display text shown to users
        /// </summary>
        [Required]
        [StringLength(100)]
        public string DisplayText { get; set; }

        /// <summary>
        /// Order in which to display the option
        /// </summary>
        [Required]
        public int SortOrder { get; set; }

        /// <summary>
        /// Flag indicating if the option is active
        /// </summary>
        [Required]
        public bool IsActive { get; set; }

        /// <summary>
        /// Date when the option was created
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the option was last modified
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// User who created the option
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// User who last modified the option
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// Navigation property for the field definition
        /// </summary>
        [ForeignKey("FieldDefinitionId")]
        public virtual CustomFieldDefinition FieldDefinition { get; set; }
    }
}
