using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppSettings.API.DTOs
{
    /// <summary>
    /// Data transfer object for custom field group
    /// </summary>
    public class CustomFieldGroupDto
    {
        /// <summary>
        /// Unique identifier for the group
        /// </summary>
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
        [RegularExpression("^[a-zA-Z0-9_]+$", ErrorMessage = "Name can only contain letters, numbers, and underscores")]
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
        public int SortOrder { get; set; }

        /// <summary>
        /// Flag indicating if the group is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Fields in this group
        /// </summary>
        public List<CustomFieldDefinitionDto> Fields { get; set; } = new List<CustomFieldDefinitionDto>();

        /// <summary>
        /// Fields with their values
        /// </summary>
        public List<CustomFieldWithValueDto> FieldsWithValues { get; set; } = new List<CustomFieldWithValueDto>();
    }

    /// <summary>
    /// Data transfer object for custom field option
    /// </summary>
    public class CustomFieldOptionDto
    {
        /// <summary>
        /// Unique identifier for the option
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID of the field definition this option belongs to
        /// </summary>
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
        public int SortOrder { get; set; }

        /// <summary>
        /// Flag indicating if the option is active
        /// </summary>
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// Data transfer object for custom field definition
    /// </summary>
    public class CustomFieldDefinitionDto
    {
        /// <summary>
        /// Unique identifier for the custom field definition
        /// </summary>
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
        [RegularExpression("^[a-zA-Z0-9_]+$", ErrorMessage = "Name can only contain letters, numbers, and underscores")]
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
        public bool IsRequired { get; set; }

        /// <summary>
        /// Flag indicating if the field is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Order in which to display the field
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Default value for the field
        /// </summary>
        public object DefaultValue { get; set; }

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
        public string Regex { get; set; }

        /// <summary>
        /// Options for select fields
        /// </summary>
        public List<CustomFieldOptionDto> Options { get; set; } = new List<CustomFieldOptionDto>();

        /// <summary>
        /// Reference to general code type (for general code fields)
        /// </summary>
        public int? GeneralCodeType { get; set; }

        /// <summary>
        /// ID of the group this field belongs to
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// Group name for backward compatibility and simple grouping
        /// </summary>
        [StringLength(50)]
        public string GroupName { get; set; } = "General";

        /// <summary>
        /// Flag indicating if the field is visible in the UI
        /// </summary>
        public bool IsVisible { get; set; } = true;
    }

    /// <summary>
    /// Data transfer object for custom field value
    /// </summary>
    public class CustomFieldValueDto
    {
        /// <summary>
        /// Unique identifier for the field value
        /// </summary>
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
        /// Multi-select values (list of selected option IDs)
        /// </summary>
        public List<int> SelectedOptionIds { get; set; } = new List<int>();
    }

    /// <summary>
    /// Data transfer object for custom field with its value
    /// </summary>
    public class CustomFieldWithValueDto
    {
        /// <summary>
        /// Field definition
        /// </summary>
        public CustomFieldDefinitionDto Definition { get; set; }

        /// <summary>
        /// Field value (null if no value exists)
        /// </summary>
        public CustomFieldValueDto Value { get; set; }
    }
}