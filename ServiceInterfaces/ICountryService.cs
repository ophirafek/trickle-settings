using AppSettings.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppSettings.API.Services
{
    /// <summary>
    /// Interface for country management service
    /// </summary>
    public interface ICountryService
    {
        /// <summary>
        /// Gets all countries
        /// </summary>
        /// <returns>Collection of country DTOs</returns>
        Task<IEnumerable<CountryDto>> GetAllCountriesAsync();

        /// <summary>
        /// Gets a country by its ID
        /// </summary>
        /// <param name="id">Country ID</param>
        /// <returns>Country DTO if found, null otherwise</returns>
        Task<CountryDto> GetCountryByIdAsync(int id);

        /// <summary>
        /// Saves a country (creates new if Id=0, updates existing otherwise)
        /// </summary>
        /// <param name="countryDto">Country data to save</param>
        /// <returns>Saved country DTO</returns>
        Task<CountryDto> SaveCountryAsync(CountryDto countryDto);

        /// <summary>
        /// Deletes a country
        /// </summary>
        /// <param name="id">Country ID</param>
        /// <returns>True if country was deleted, false if not found</returns>
        Task<bool> DeleteCountryAsync(int id);

        /// <summary>
        /// Checks if a country code already exists
        /// </summary>
        /// <param name="countryCode">Country code to check</param>
        /// <param name="excludeId">Optional ID to exclude from check</param>
        /// <returns>True if country code exists, false otherwise</returns>
        Task<bool> CountryCodeExistsAsync(string countryCode, int? excludeId = null);

        /// <summary>
        /// Checks if a country name already exists
        /// </summary>
        /// <param name="countryName">Country name to check</param>
        /// <param name="excludeId">Optional ID to exclude from check</param>
        /// <returns>True if country name exists, false otherwise</returns>
        Task<bool> CountryNameExistsAsync(string countryName, int? excludeId = null);
    }
}