using AppSettings.API.Data;
using AppSettings.API.DTOs;
using AppSettings.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppSettings.API.Services
{
    /// <summary>
    /// Implementation of the country service
    /// </summary>
    public class CountryService : ICountryService
    {
        private readonly AppSettingsContext _context;

        /// <summary>
        /// Constructor that accepts database context
        /// </summary>
        /// <param name="context">Database context</param>
        public CountryService(AppSettingsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CountryDto>> GetAllCountriesAsync()
        {
            return await _context.Countries
                .Select(c => new CountryDto
                {
                    Id = c.Id,
                    CountryCode = c.CountryCode,
                    CountryName = c.CountryName,
                    IsActive = c.IsActive
                })
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<CountryDto> GetCountryByIdAsync(int id)
        {
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
                return null;

            return new CountryDto
            {
                Id = country.Id,
                CountryCode = country.CountryCode,
                CountryName = country.CountryName,
                IsActive = country.IsActive
            };
        }

        /// <inheritdoc/>
        public async Task<CountryDto> SaveCountryAsync(CountryDto countryDto)
        {
            if (countryDto == null)
                throw new ArgumentNullException(nameof(countryDto));

            // Determine if this is a create or update operation
            bool isNewCountry = countryDto.Id == 0;

            // Validate country code and name uniqueness
            if (await CountryCodeExistsAsync(countryDto.CountryCode, isNewCountry ? null : countryDto.Id))
                throw new InvalidOperationException($"Country code '{countryDto.CountryCode}' already exists.");

            if (await CountryNameExistsAsync(countryDto.CountryName, isNewCountry ? null : countryDto.Id))
                throw new InvalidOperationException($"Country name '{countryDto.CountryName}' already exists.");

            if (isNewCountry)
            {
                // Create new country
                var country = new Country
                {
                    CountryCode = countryDto.CountryCode.ToUpperInvariant(),
                    CountryName = countryDto.CountryName,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = countryDto.IsActive
                };

                _context.Countries.Add(country);
                await _context.SaveChangesAsync();

                countryDto.Id = country.Id;
                return countryDto;
            }
            else
            {
                // Update existing country
                var country = await _context.Countries.FindAsync(countryDto.Id);

                if (country == null)
                    throw new InvalidOperationException($"Country with ID {countryDto.Id} not found.");

                // Update properties
                country.CountryCode = countryDto.CountryCode.ToUpperInvariant();
                country.CountryName = countryDto.CountryName;
                country.IsActive = countryDto.IsActive;
                country.ModifiedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return countryDto;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteCountryAsync(int id)
        {
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
                return false;

            _context.Countries.Remove(country);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> CountryCodeExistsAsync(string countryCode, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
                return false;

            countryCode = countryCode.ToUpperInvariant();

            return await _context.Countries
                .AnyAsync(c => c.CountryCode == countryCode && (excludeId == null || c.Id != excludeId));
        }

        /// <inheritdoc/>
        public async Task<bool> CountryNameExistsAsync(string countryName, int? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(countryName))
                return false;

            return await _context.Countries
                .AnyAsync(c => c.CountryName == countryName && (excludeId == null || c.Id != excludeId));
        }
    }
}