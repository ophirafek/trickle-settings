using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppSettings.API.Models
{
    /// <summary>
    /// Represents a general code entry with descriptions in a specific language
    /// </summary>
    public class GeneralCode
    {
        /// <summary>
        /// Unique identifier for the general code record
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Type of code (e.g., 1=Error, 2=Status, etc.)
        /// </summary>
        [Required]
        public int CodeType { get; set; }

        /// <summary>
        /// Numeric code identifier
        /// </summary>
        [Required]
        public int CodeNumber { get; set; }

        /// <summary>
        /// Short description or identifier for the code
        /// </summary>
        [Required]
        [StringLength(100)]
        public string CodeShortDescription { get; set; }

        /// <summary>
        /// Detailed description of the code
        /// </summary>
        public string CodeLongDescription { get; set; }

        /// <summary>
        /// Language code (e.g., 1=English, 2=Spanish, etc.)
        /// </summary>
        [Required]
        public int LanguageCode { get; set; }

        /// <summary>
        /// Date when the code was created
        /// </summary>
        [Required]
        public DateTime OpeningRegDate { get; set; }

        /// <summary>
        /// Date when the code was last modified
        /// </summary>
        public DateTime? ClosingRegDate { get; set; }

        /// <summary>
        /// Flag indicating if the code is active
        /// </summary>
        [Required]
        public bool ActiveFlag { get; set; }
    }
}