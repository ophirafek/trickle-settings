using AppSettings.API.DTOs;
using AppSettings.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppSettings.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomFieldsController : ControllerBase
    {
        private readonly ICustomFieldService _customFieldService;

        public CustomFieldsController(ICustomFieldService customFieldService)
        {
            _customFieldService = customFieldService ?? throw new ArgumentNullException(nameof(customFieldService));
        }

        #region Groups

        /// <summary>
        /// Gets all custom field groups
        /// </summary>
        /// <returns>List of all custom field groups</returns>
        [HttpGet("groups")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllGroups()
        {
            var groups = await _customFieldService.GetAllCustomFieldGroupsAsync();
            return Ok(groups);
        }

        /// <summary>
        /// Gets custom field groups by entity type
        /// </summary>
        /// <param name="entityType">Entity type to filter by</param>
        /// <returns>List of custom field groups for the specified entity type</returns>
        [HttpGet("groups/entity/{entityType}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGroupsByEntityType(string entityType)
        {
            var groups = await _customFieldService.GetCustomFieldGroupsByEntityTypeAsync(entityType);
            return Ok(groups);
        }

        /// <summary>
        /// Gets a specific custom field group by ID
        /// </summary>
        /// <param name="id">Custom field group ID</param>
        /// <returns>Custom field group details</returns>
        [HttpGet("groups/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetGroup(int id)
        {
            var group = await _customFieldService.GetCustomFieldGroupByIdAsync(id);

            if (group == null)
                return NotFound();

            return Ok(group);
        }

        /// <summary>
        /// Saves a custom field group (creates new if Id=0, updates if Id>0)
        /// </summary>
        /// <param name="groupDto">Custom field group data</param>
        /// <returns>Saved custom field group details</returns>
        [HttpPost("groups")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveGroup([FromBody] CustomFieldGroupDto groupDto)
        {
            try
            {
                bool isNewGroup = groupDto.Id == 0;
                var savedGroup = await _customFieldService.SaveCustomFieldGroupAsync(groupDto);

                if (isNewGroup)
                {
                    return CreatedAtAction(nameof(GetGroup), new { id = savedGroup.Id }, savedGroup);
                }

                return Ok(savedGroup);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a custom field group
        /// </summary>
        /// <param name="id">Custom field group ID</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("groups/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var result = await _customFieldService.DeleteCustomFieldGroupAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Updates the sort order of custom field groups
        /// </summary>
        /// <param name="groupIds">Ordered list of group IDs</param>
        /// <returns>No content if successful</returns>
        [HttpPut("groups/sort-order")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateGroupSortOrder([FromBody] IEnumerable<int> groupIds)
        {
            try
            {
                var result = await _customFieldService.UpdateCustomFieldGroupSortOrderAsync(groupIds);

                if (!result)
                    return BadRequest("Failed to update sort order");

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Checks if a group name already exists for an entity type
        /// </summary>
        /// <param name="entityType">Entity type</param>
        /// <param name="name">Group name</param>
        /// <param name="excludeId">Optional ID to exclude from the check</param>
        /// <returns>True if the name exists, false otherwise</returns>
        [HttpGet("groups/check-name/{entityType}/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckGroupNameExists(string entityType, string name, [FromQuery] int? excludeId = null)
        {
            var exists = await _customFieldService.GroupNameExistsAsync(entityType, name, excludeId);
            return Ok(exists);
        }

        #endregion

        #region Definitions

        /// <summary>
        /// Gets all custom field definitions
        /// </summary>
        /// <returns>List of all custom field definitions</returns>
        [HttpGet("definitions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllDefinitions()
        {
            var definitions = await _customFieldService.GetAllCustomFieldDefinitionsAsync();
            return Ok(definitions);
        }

        /// <summary>
        /// Gets custom field definitions by entity type
        /// </summary>
        /// <param name="entityType">Entity type to filter by</param>
        /// <returns>List of custom field definitions for the specified entity type</returns>
        [HttpGet("definitions/entity/{entityType}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDefinitionsByEntityType(string entityType)
        {
            var definitions = await _customFieldService.GetCustomFieldDefinitionsByEntityTypeAsync(entityType);
            return Ok(definitions);
        }

        /// <summary>
        /// Gets custom field definitions by entity type, grouped by group
        /// </summary>
        /// <param name="entityType">Entity type to filter by</param>
        /// <returns>List of custom field groups for the specified entity type</returns>
        [HttpGet("definitions/entity/{entityType}/grouped")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDefinitionsByEntityTypeGrouped(string entityType)
        {
            var groups = await _customFieldService.GetCustomFieldDefinitionsByEntityTypeGroupedAsync(entityType);
            return Ok(groups);
        }

        /// <summary>
        /// Gets custom field definitions by group ID
        /// </summary>
        /// <param name="groupId">Group ID to filter by</param>
        /// <returns>List of custom field definitions for the specified group</returns>
        [HttpGet("definitions/group/{groupId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDefinitionsByGroupId(int groupId)
        {
            var definitions = await _customFieldService.GetCustomFieldDefinitionsByGroupIdAsync(groupId);
            return Ok(definitions);
        }

        /// <summary>
        /// Gets a specific custom field definition by ID
        /// </summary>
        /// <param name="id">Custom field definition ID</param>
        /// <returns>Custom field definition details</returns>
        [HttpGet("definitions/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDefinition(int id)
        {
            var definition = await _customFieldService.GetCustomFieldDefinitionByIdAsync(id);

            if (definition == null)
                return NotFound();

            return Ok(definition);
        }

        /// <summary>
        /// Saves a custom field definition (creates new if Id=0, updates if Id>0)
        /// </summary>
        /// <param name="definitionDto">Custom field definition data</param>
        /// <returns>Saved custom field definition details</returns>
        [HttpPost("definitions")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveDefinition([FromBody] CustomFieldDefinitionDto definitionDto)
        {
            try
            {
                bool isNewDefinition = definitionDto.Id == 0;
                var savedDefinition = await _customFieldService.SaveCustomFieldDefinitionAsync(definitionDto);

                if (isNewDefinition)
                {
                    return CreatedAtAction(nameof(GetDefinition), new { id = savedDefinition.Id }, savedDefinition);
                }

                return Ok(savedDefinition);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a custom field definition
        /// </summary>
        /// <param name="id">Custom field definition ID</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("definitions/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDefinition(int id)
        {
            var result = await _customFieldService.DeleteCustomFieldDefinitionAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Updates the sort order of custom field definitions
        /// </summary>
        /// <param name="fieldIds">Ordered list of field IDs</param>
        /// <returns>No content if successful</returns>
        [HttpPut("definitions/sort-order")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateSortOrder([FromBody] IEnumerable<int> fieldIds)
        {
            try
            {
                var result = await _customFieldService.UpdateCustomFieldSortOrderAsync(fieldIds);

                if (!result)
                    return BadRequest("Failed to update sort order");

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Checks if a field name already exists for an entity type
        /// </summary>
        /// <param name="entityType">Entity type</param>
        /// <param name="name">Field name</param>
        /// <param name="excludeId">Optional ID to exclude from the check</param>
        /// <returns>True if the name exists, false otherwise</returns>
        [HttpGet("definitions/check-name/{entityType}/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckNameExists(string entityType, string name, [FromQuery] int? excludeId = null)
        {
            var exists = await _customFieldService.FieldNameExistsAsync(entityType, name, excludeId);
            return Ok(exists);
        }

        #endregion

        #region Options

        /// <summary>
        /// Gets options for a custom field
        /// </summary>
        /// <param name="fieldDefinitionId">Field definition ID</param>
        /// <returns>List of custom field options for the specified field</returns>
        [HttpGet("options/field/{fieldDefinitionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOptionsByField(int fieldDefinitionId)
        {
            var options = await _customFieldService.GetCustomFieldOptionsAsync(fieldDefinitionId);
            return Ok(options);
        }

        /// <summary>
        /// Saves custom field options for a field
        /// </summary>
        /// <param name="fieldDefinitionId">Field definition ID</param>
        /// <param name="options">Custom field options to save</param>
        /// <returns>Saved custom field options</returns>
        [HttpPost("options/field/{fieldDefinitionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveOptions(int fieldDefinitionId, [FromBody] IEnumerable<CustomFieldOptionDto> options)
        {
            try
            {
                var savedOptions = await _customFieldService.SaveCustomFieldOptionsAsync(fieldDefinitionId, options);
                return Ok(savedOptions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates the sort order of custom field options
        /// </summary>
        /// <param name="optionIds">Ordered list of option IDs</param>
        /// <returns>No content if successful</returns>
        [HttpPut("options/sort-order")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOptionSortOrder([FromBody] IEnumerable<int> optionIds)
        {
            try
            {
                var result = await _customFieldService.UpdateCustomFieldOptionSortOrderAsync(optionIds);

                if (!result)
                    return BadRequest("Failed to update sort order");

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Values

        /// <summary>
        /// Gets custom field values for a specific entity
        /// </summary>
        /// <param name="entityType">Type of entity</param>
        /// <param name="entityId">Entity ID</param>
        /// <returns>List of custom field values for the specified entity</returns>
        [HttpGet("values/entity/{entityType}/{entityId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetValuesByEntity(string entityType, int entityId)
        {
            var values = await _customFieldService.GetCustomFieldValuesByEntityAsync(entityType, entityId);
            return Ok(values);
        }

        /// <summary>
        /// Gets custom fields with their values for a specific entity
        /// </summary>
        /// <param name="entityType">Type of entity</param>
        /// <param name="entityId">Entity ID</param>
        /// <returns>List of custom fields with values for the specified entity</returns>
        [HttpGet("entity/{entityType}/{entityId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFieldsWithValuesByEntity(string entityType, int entityId)
        {
            var fieldsWithValues = await _customFieldService.GetCustomFieldsWithValuesByEntityAsync(entityType, entityId);
            return Ok(fieldsWithValues);
        }

        /// <summary>
        /// Gets custom fields with their values for a specific entity, grouped by group
        /// </summary>
        /// <param name="entityType">Type of entity</param>
        /// <param name="entityId">Entity ID</param>
        /// <returns>List of custom field groups with values for the specified entity</returns>
        [HttpGet("entity/{entityType}/{entityId}/grouped")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFieldsWithValuesByEntityGrouped(string entityType, int entityId)
        {
            var groups = await _customFieldService.GetCustomFieldsWithValuesByEntityGroupedAsync(entityType, entityId);
            return Ok(groups);
        }

        /// <summary>
        /// Saves custom field values for an entity
        /// </summary>
        /// <param name="values">Custom field values to save</param>
        /// <returns>Saved custom field values</returns>
        [HttpPost("values")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveValues([FromBody] IEnumerable<CustomFieldValueDto> values)
        {
            try
            {
                var savedValues = await _customFieldService.SaveCustomFieldValuesAsync(values);
                return Ok(savedValues);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}