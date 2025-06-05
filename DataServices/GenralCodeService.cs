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
    /// Implementation of the general code service
    /// </summary>
    public class GeneralCodeService : IGeneralCodeService
    {
        private readonly AppSettingsContext _context;

        /// <summary>
        /// Constructor that accepts database context
        /// </summary>
        /// <param name="context">Database context</param>
        public GeneralCodeService(AppSettingsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<GeneralCodeDto>> GetAllGeneralCodesAsync()
        {
            return await _context.GeneralCodes
                .Select(c => MapToDto(c))
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<GeneralCodeDto>> GetGeneralCodesByTypeAsync(int codeType)
        {
            return await _context.GeneralCodes
                .Where(c => c.CodeType == codeType)
                .Select(c => MapToDto(c))
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<GeneralCodeDto>> GetGeneralCodesByLanguageAsync(int languageCode)
        {
            return await _context.GeneralCodes
                .Where(c => c.LanguageCode == languageCode)
                .Select(c => MapToDto(c))
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<GeneralCodeDto>> GetGeneralCodesByTypeAndLanguageAsync(int codeType, int languageCode)
        {
            return await _context.GeneralCodes
                .Where(c => c.CodeType == codeType && c.LanguageCode == languageCode)
                .Select(c => MapToDto(c))
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<GeneralCodeDto> GetGeneralCodeByIdAsync(int id)
        {
            var code = await _context.GeneralCodes.FindAsync(id);
            return code != null ? MapToDto(code) : null;
        }

        /// <inheritdoc/>
        public async Task<GeneralCodeDto> GetGeneralCodeByTypeNumberLanguageAsync(int codeType, int codeNumber, int languageCode)
        {
            var code = await _context.GeneralCodes
                .FirstOrDefaultAsync(c => c.CodeType == codeType
                                    && c.CodeNumber == codeNumber
                                    && c.LanguageCode == languageCode);

            return code != null ? MapToDto(code) : null;
        }

        /// <inheritdoc/>
        public async Task<GeneralCodeDto> SaveGeneralCodeAsync(GeneralCodeDto generalCodeDto)
        {
            if (generalCodeDto == null)
                throw new ArgumentNullException(nameof(generalCodeDto));

            // Check if this is a new or existing code
            bool isNewCode = generalCodeDto.Id == 0;

            // Check if a code with the same combination already exists
            if (await GeneralCodeExistsAsync(generalCodeDto.CodeType, generalCodeDto.CodeNumber, generalCodeDto.LanguageCode, isNewCode ? null : generalCodeDto.Id))
            {
                throw new InvalidOperationException($"A general code with type {generalCodeDto.CodeType}, number {generalCodeDto.CodeNumber}, " +
                    $"and language {generalCodeDto.LanguageCode} already exists.");
            }

            if (isNewCode)
            {
                // Create a new general code
                var generalCode = new GeneralCode
                {
                    CodeType = generalCodeDto.CodeType,
                    CodeNumber = generalCodeDto.CodeNumber,
                    CodeShortDescription = generalCodeDto.CodeShortDescription,
                    CodeLongDescription = generalCodeDto.CodeLongDescription,
                    LanguageCode = generalCodeDto.LanguageCode,
                    ActiveFlag = generalCodeDto.IsActive,
                    OpeningRegDate = DateTime.UtcNow
                };

                _context.GeneralCodes.Add(generalCode);
                await _context.SaveChangesAsync();

                // Update the DTO with the new ID
                generalCodeDto.Id = generalCode.Id;
                return generalCodeDto;
            }
            else
            {
                // Update an existing general code
                var generalCode = await _context.GeneralCodes.FindAsync(generalCodeDto.Id);

                if (generalCode == null)
                    throw new InvalidOperationException($"General code with ID {generalCodeDto.Id} not found.");

                // Update properties
                generalCode.CodeType = generalCodeDto.CodeType;
                generalCode.CodeNumber = generalCodeDto.CodeNumber;
                generalCode.CodeShortDescription = generalCodeDto.CodeShortDescription;
                generalCode.CodeLongDescription = generalCodeDto.CodeLongDescription;
                generalCode.LanguageCode = generalCodeDto.LanguageCode;
                generalCode.ActiveFlag = generalCodeDto.IsActive;
                generalCode.ClosingRegDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return generalCodeDto;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteGeneralCodeAsync(int id)
        {
            var generalCode = await _context.GeneralCodes.FindAsync(id);

            if (generalCode == null)
                return false;

            _context.GeneralCodes.Remove(generalCode);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> GeneralCodeExistsAsync(int codeType, int codeNumber, int languageCode, int? excludeId = null)
        {
            return await _context.GeneralCodes
                .AnyAsync(c => c.CodeType == codeType
                          && c.CodeNumber == codeNumber
                          && c.LanguageCode == languageCode
                          && (excludeId == null || c.Id != excludeId));
        }

        /// <summary>
        /// Maps a GeneralCode entity to a GeneralCodeDto
        /// </summary>
        private static GeneralCodeDto MapToDto(GeneralCode generalCode)
        {
            return new GeneralCodeDto
            {
                Id = generalCode.Id,
                CodeType = generalCode.CodeType,
                CodeNumber = generalCode.CodeNumber,
                CodeShortDescription = generalCode.CodeShortDescription,
                CodeLongDescription = generalCode.CodeLongDescription,
                LanguageCode = generalCode.LanguageCode,
                IsActive = generalCode.ActiveFlag
            };
        }
    }
}