using System.ComponentModel.DataAnnotations;

namespace AppSettings.API.DTOs
{
    /// <summary>
    /// Data transfer object for representing general code information
    /// </summary>
    public class GeneralCodeDto
    {
        /// <summary>
        /// Unique identifier for the code (Id=0 indicates a new code)
        /// </summary>
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
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Short description must be between 1 and 100 characters")]
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
        /// Flag indicating if the code is active
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}