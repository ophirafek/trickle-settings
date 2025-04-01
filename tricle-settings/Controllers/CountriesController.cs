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
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        /// <summary>
        /// Gets all countries
        /// </summary>
        /// <returns>List of all countries</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCountries()
        {
            var countries = await _countryService.GetAllCountriesAsync();
            return Ok(countries);
        }

        /// <summary>
        /// Gets a specific country by id
        /// </summary>
        /// <param name="id">Country ID</param>
        /// <returns>Country details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCountry(int id)
        {
            var country = await _countryService.GetCountryByIdAsync(id);

            if (country == null)
                return NotFound();

            return Ok(country);
        }

        /// <summary>
        /// Saves a country (creates new if Id=0, updates if Id>0)
        /// </summary>
        /// <param name="countryDto">Country data</param>
        /// <returns>Saved country details</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveCountry([FromBody] CountryDto countryDto)
        {
            try
            {
                bool isNewCountry = countryDto.Id == 0;
                var savedCountry = await _countryService.SaveCountryAsync(countryDto);

                if (isNewCountry)
                {
                    return CreatedAtAction(nameof(GetCountry), new { id = savedCountry.Id }, savedCountry);
                }

                return Ok(savedCountry);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a country
        /// </summary>
        /// <param name="id">Country ID</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var result = await _countryService.DeleteCountryAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}