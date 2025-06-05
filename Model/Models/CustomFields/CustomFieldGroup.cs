using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Models.CustomFields;

namespace AppSettings.API.Models
{
    /// <summary>
    /// Represents a group of custom fields for organizational purposes
    /// </summary>
    public class CustomFieldGroup
    {
        /// <summary>
        /// Unique identifier for the custom field group
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Entity type this group belongs to (e.g., 'lead', 'company', etc.)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string EntityType { get; set; }

        /// <summary>
        /// Name of the group (used as a reference in code)
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
        /// Optional description for the group
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Order in which to display the group
        /// </summary>
        [Required]
        public int SortOrder { get; set; }

        /// <summary>
        /// Flag indicating if the group is active
        /// </summary>
        [Required]
        public bool IsActive { get; set; }

        /// <summary>
        /// Date when the group was created
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the group was last modified
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// User who created the group
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// User who last modified the group
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// Navigation property for fields in this group
        /// </summary>
        public virtual ICollection<CustomFieldDefinition> Fields { get; set; } = new List<CustomFieldDefinition>();
    }

}
