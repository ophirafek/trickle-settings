using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppSettings.API.Models
{
    /// <summary>
    /// Represents a country in the system
    /// </summary>
    public class Country
    {
        /// <summary>
        /// Unique identifier for the country
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// ISO 2-letter country code
        /// </summary>
        [Required]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Country code must be exactly 2 characters")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Full name of the country
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Country name must be between 2 and 100 characters")]
        public string CountryName { get; set; }

        /// <summary>
        /// Date when the country was created in the system
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the country was last modified
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Flag indicating if the country is active in the system
        /// </summary>
        [Required]
        public bool IsActive { get; set; }
    }
}