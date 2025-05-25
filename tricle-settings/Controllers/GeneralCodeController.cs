using AppSettings.API.DTOs;
using AppSettings.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AppSettings.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralCodesController : ControllerBase
    {
        private readonly IGeneralCodeService _generalCodeService;

        public GeneralCodesController(IGeneralCodeService generalCodeService)
        {
            _generalCodeService = generalCodeService;
        }

        /// <summary>
        /// Gets all general codes
        /// </summary>
        /// <returns>List of all general codes</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllGeneralCodes()
        {
            var codes = await _generalCodeService.GetAllGeneralCodesAsync();
            return Ok(codes);
        }

        /// <summary>
        /// Gets general codes by type
        /// </summary>
        /// <param name="type">Code type to filter by</param>
        /// <returns>List of general codes with the specified type</returns>
        [HttpGet("type/{type}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGeneralCodesByType(int type)
        {
            var codes = await _generalCodeService.GetGeneralCodesByTypeAsync(type);
            return Ok(codes);
        }

        /// <summary>
        /// Gets general codes by language
        /// </summary>
        /// <param name="languageCode">Language code to filter by</param>
        /// <returns>List of general codes with the specified language</returns>
        [HttpGet("language/{languageCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGeneralCodesByLanguage(int languageCode)
        {
            var codes = await _generalCodeService.GetGeneralCodesByLanguageAsync(languageCode);
            return Ok(codes);
        }

        /// <summary>
        /// Gets general codes by type and language
        /// </summary>
        /// <param name="type">Code type to filter by</param>
        /// <param name="languageCode">Language code to filter by</param>
        /// <returns>List of general codes with the specified type and language</returns>
        [HttpGet("type/{type}/language/{languageCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGeneralCodesByTypeAndLanguage(int type, int languageCode)
        {
            var codes = await _generalCodeService.GetGeneralCodesByTypeAndLanguageAsync(type, languageCode);
            return Ok(codes);
        }

        /// <summary>
        /// Gets a specific general code by ID
        /// </summary>
        /// <param name="id">General code ID</param>
        /// <returns>General code details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetGeneralCode(int id)
        {
            var code = await _generalCodeService.GetGeneralCodeByIdAsync(id);

            if (code == null)
                return NotFound();

            return Ok(code);
        }

        /// <summary>
        /// Gets a specific general code by type, number, and language
        /// </summary>
        /// <param name="type">Code type</param>
        /// <param name="number">Code number</param>
        /// <param name="languageCode">Language code</param>
        /// <returns>General code details</returns>
        [HttpGet("type/{type}/number/{number}/language/{languageCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetGeneralCodeByTypeNumberLanguage(int type, int number, int languageCode)
        {
            var code = await _generalCodeService.GetGeneralCodeByTypeNumberLanguageAsync(type, number, languageCode);

            if (code == null)
                return NotFound();

            return Ok(code);
        }

        /// <summary>
        /// Saves a general code (creates new if Id=0, updates if Id>0)
        /// </summary>
        /// <param name="generalCodeDto">General code data</param>
        /// <returns>Saved general code details</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveGeneralCode([FromBody] GeneralCodeDto generalCodeDto)
        {
            try
            {
                bool isNewCode = generalCodeDto.Id == 0;
                var savedCode = await _generalCodeService.SaveGeneralCodeAsync(generalCodeDto);

                if (isNewCode)
                {
                    return CreatedAtAction(nameof(GetGeneralCode), new { id = savedCode.Id }, savedCode);
                }

                return Ok(savedCode);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a general code
        /// </summary>
        /// <param name="id">General code ID</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteGeneralCode(int id)
        {
            var result = await _generalCodeService.DeleteGeneralCodeAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}