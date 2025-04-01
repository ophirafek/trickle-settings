using System.ComponentModel.DataAnnotations;

namespace AppSettings.API.DTOs
{
    /// <summary>
    /// Data transfer object for representing country information
    /// Used for all CRUD operations (Id=0 indicates a new country)
    /// </summary>
    public class CountryDto
    {
        /// <summary>
        /// Unique identifier for the country
        /// Id=0 indicates a new country to be created
        /// </summary>
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
        /// Flag indicating if the country is active
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}