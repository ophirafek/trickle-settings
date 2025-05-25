using AppSettings.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppSettings.API.Services
{
    /// <summary>
    /// Interface for general code management service
    /// </summary>
    public interface IGeneralCodeService
    {
        /// <summary>
        /// Gets all general codes
        /// </summary>
        /// <returns>Collection of general code DTOs</returns>
        Task<IEnumerable<GeneralCodeDto>> GetAllGeneralCodesAsync();

        /// <summary>
        /// Gets general codes by type
        /// </summary>
        /// <param name="codeType">Code type to filter by</param>
        /// <returns>Collection of general code DTOs with the specified type</returns>
        Task<IEnumerable<GeneralCodeDto>> GetGeneralCodesByTypeAsync(int codeType);

        /// <summary>
        /// Gets general codes by language
        /// </summary>
        /// <param name="languageCode">Language code to filter by</param>
        /// <returns>Collection of general code DTOs with the specified language</returns>
        Task<IEnumerable<GeneralCodeDto>> GetGeneralCodesByLanguageAsync(int languageCode);

        /// <summary>
        /// Gets general codes by type and language
        /// </summary>
        /// <param name="codeType">Code type to filter by</param>
        /// <param name="languageCode">Language code to filter by</param>
        /// <returns>Collection of general code DTOs with the specified type and language</returns>
        Task<IEnumerable<GeneralCodeDto>> GetGeneralCodesByTypeAndLanguageAsync(int codeType, int languageCode);

        /// <summary>
        /// Gets a specific general code by its ID
        /// </summary>
        /// <param name="id">Code ID</param>
        /// <returns>General code DTO if found, null otherwise</returns>
        Task<GeneralCodeDto> GetGeneralCodeByIdAsync(int id);

        /// <summary>
        /// Gets a specific general code by type, number, and language
        /// </summary>
        /// <param name="codeType">Code type</param>
        /// <param name="codeNumber">Code number</param>
        /// <param name="languageCode">Language code</param>
        /// <returns>General code DTO if found, null otherwise</returns>
        Task<GeneralCodeDto> GetGeneralCodeByTypeNumberLanguageAsync(int codeType, int codeNumber, int languageCode);

        /// <summary>
        /// Saves a general code (creates new if Id=0, updates if Id>0)
        /// </summary>
        /// <param name="generalCodeDto">General code data to save</param>
        /// <returns>Saved general code DTO</returns>
        Task<GeneralCodeDto> SaveGeneralCodeAsync(GeneralCodeDto generalCodeDto);

        /// <summary>
        /// Deletes a general code
        /// </summary>
        /// <param name="id">General code ID</param>
        /// <returns>True if general code was deleted, false if not found</returns>
        Task<bool> DeleteGeneralCodeAsync(int id);

        /// <summary>
        /// Checks if a general code with the given type, number, and language already exists
        /// </summary>
        /// <param name="codeType">Code type</param>
        /// <param name="codeNumber">Code number</param>
        /// <param name="languageCode">Language code</param>
        /// <param name="excludeId">Optional ID to exclude from check</param>
        /// <returns>True if a matching general code exists, false otherwise</returns>
        Task<bool> GeneralCodeExistsAsync(int codeType, int codeNumber, int languageCode, int? excludeId = null);
    }
}