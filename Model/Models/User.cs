using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppSettings.API.Models
{
    /// <summary>
    /// Represents a user in the system
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique identifier for the user (manually assigned)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Indicates ID is not auto-generated
        public int Id { get; set; }

        /// <summary>
        /// Username for login
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        /// <summary>
        /// ID number for the user (e.g. national ID, staff ID)
        /// </summary>
        [StringLength(20)]
        public string? IDNumber { get; set; }

        /// <summary>
        /// Date when the user's password expires
        /// </summary>
        public DateTime? PasswordExpiryDate { get; set; }

        /// <summary>
        /// Flag indicating if the user is blocked from accessing the system
        /// </summary>
        public bool IsBlocked { get; set; }

        /// <summary>
        /// Number of unsuccessful login attempts
        /// </summary>
        public int? LoginAttempts { get; set; }

        /// <summary>
        /// Reference to the user's preferred language
        /// </summary>
        public int? PreferredLanguageCode { get; set; }

        /// <summary>
        /// Flag indicating if the user is inactive
        /// </summary>
        public bool IsInactiveFlag { get; set; }

        /// <summary>
        /// Signature for terms of use acceptance
        /// </summary>
        [StringLength(255)]
        public string TermsOfUseSignature { get; set; }

        /// <summary>
        /// Date when the user signed the terms of use
        /// </summary>
        public DateTime? TermsOfUseSignatureDate { get; set; }

        /// <summary>
        /// Mobile number of the user
        /// </summary>
        [StringLength(20)]
        public string MobileNumber { get; set; }

        /// <summary>
        /// Date when the user record was created
        /// </summary>
        [Required]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Date when the user record was last updated
        /// </summary>
        public DateTime? LastUpdateRecDate { get; set; }

        /// <summary>
        /// Flag indicating if the user is active
        /// </summary>
        public bool ActiveFlag { get; set; }
    }
}