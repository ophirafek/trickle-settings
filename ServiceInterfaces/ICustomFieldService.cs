using AppSettings.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppSettings.API.Services
{
    /// <summary>
    /// Interface for custom field management service
    /// </summary>
    public interface ICustomFieldService
    {
        #region Custom Field Groups

        /// <summary>
        /// Gets all custom field groups
        /// </summary>
        /// <returns>Collection of custom field group DTOs</returns>
        Task<IEnumerable<CustomFieldGroupDto>> GetAllCustomFieldGroupsAsync();

        /// <summary>
        /// Gets custom field groups by entity type
        /// </summary>
        /// <param name="entityType">Entity type to filter by</param>
        /// <returns>Collection of custom field group DTOs for the specified entity type</returns>
        Task<IEnumerable<CustomFieldGroupDto>> GetCustomFieldGroupsByEntityTypeAsync(string entityType);

        /// <summary>
        /// Gets a custom field group by ID
        /// </summary>
        /// <param name="id">Custom field group ID</param>
        /// <returns>Custom field group DTO if found, null otherwise</returns>
        Task<CustomFieldGroupDto> GetCustomFieldGroupByIdAsync(int id);

        /// <summary>
        /// Saves a custom field group (creates new if Id=0, updates existing otherwise)
        /// </summary>
        /// <param name="groupDto">Custom field group data to save</param>
        /// <returns>Saved custom field group DTO</returns>
        Task<CustomFieldGroupDto> SaveCustomFieldGroupAsync(CustomFieldGroupDto groupDto);

        /// <summary>
        /// Deletes a custom field group
        /// </summary>
        /// <param name="id">Custom field group ID</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> DeleteCustomFieldGroupAsync(int id);

        /// <summary>
        /// Updates the sort order of custom field groups
        /// </summary>
        /// <param name="groupIds">Ordered list of group IDs</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> UpdateCustomFieldGroupSortOrderAsync(IEnumerable<int> groupIds);

        /// <summary>
        /// Checks if a group name already exists for an entity type
        /// </summary>
        /// <param name="entityType">Entity type</param>
        /// <param name="name">Group name</param>
        /// <param name="excludeId">Optional ID to exclude from the check</param>
        /// <returns>True if the name exists, false otherwise</returns>
        Task<bool> GroupNameExistsAsync(string entityType, string name, int? excludeId = null);

        #endregion

        #region Custom Field Definitions

        /// <summary>
        /// Gets all custom field definitions
        /// </summary>
        /// <returns>Collection of custom field definition DTOs</returns>
        Task<IEnumerable<CustomFieldDefinitionDto>> GetAllCustomFieldDefinitionsAsync();

        /// <summary>
        /// Gets custom field definitions by entity type
        /// </summary>
        /// <param name="entityType">Entity type to filter by</param>
        /// <returns>Collection of custom field definition DTOs for the specified entity type</returns>
        Task<IEnumerable<CustomFieldDefinitionDto>> GetCustomFieldDefinitionsByEntityTypeAsync(string entityType);

        /// <summary>
        /// Gets custom field definitions by entity type, grouped by group ID
        /// </summary>
        /// <param name="entityType">Entity type to filter by</param>
        /// <returns>Collection of custom field group DTOs with their fields for the specified entity type</returns>
        Task<IEnumerable<CustomFieldGroupDto>> GetCustomFieldDefinitionsByEntityTypeGroupedAsync(string entityType);

        /// <summary>
        /// Gets custom field definitions by group ID
        /// </summary>
        /// <param name="groupId">Group ID to filter by</param>
        /// <returns>Collection of custom field definition DTOs for the specified group</returns>
        Task<IEnumerable<CustomFieldDefinitionDto>> GetCustomFieldDefinitionsByGroupIdAsync(int groupId);

        /// <summary>
        /// Gets a custom field definition by ID
        /// </summary>
        /// <param name="id">Custom field definition ID</param>
        /// <returns>Custom field definition DTO if found, null otherwise</returns>
        Task<CustomFieldDefinitionDto> GetCustomFieldDefinitionByIdAsync(int id);

        /// <summary>
        /// Saves a custom field definition (creates new if Id=0, updates existing otherwise)
        /// </summary>
        /// <param name="definitionDto">Custom field definition data to save</param>
        /// <returns>Saved custom field definition DTO</returns>
        Task<CustomFieldDefinitionDto> SaveCustomFieldDefinitionAsync(CustomFieldDefinitionDto definitionDto);

        /// <summary>
        /// Deletes a custom field definition
        /// </summary>
        /// <param name="id">Custom field definition ID</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> DeleteCustomFieldDefinitionAsync(int id);

        /// <summary>
        /// Updates the sort order of custom field definitions
        /// </summary>
        /// <param name="fieldIds">Ordered list of field IDs</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> UpdateCustomFieldSortOrderAsync(IEnumerable<int> fieldIds);

        /// <summary>
        /// Checks if a field name already exists for an entity type
        /// </summary>
        /// <param name="entityType">Entity type</param>
        /// <param name="name">Field name</param>
        /// <param name="excludeId">Optional ID to exclude from the check</param>
        /// <returns>True if the name exists, false otherwise</returns>
        Task<bool> FieldNameExistsAsync(string entityType, string name, int? excludeId = null);

        #endregion

        #region Custom Field Options

        /// <summary>
        /// Gets options for a custom field
        /// </summary>
        /// <param name="fieldDefinitionId">Field definition ID</param>
        /// <returns>Collection of custom field option DTOs for the specified field</returns>
        Task<IEnumerable<CustomFieldOptionDto>> GetCustomFieldOptionsAsync(int fieldDefinitionId);

        /// <summary>
        /// Saves custom field options for a field
        /// </summary>
        /// <param name="fieldDefinitionId">Field definition ID</param>
        /// <param name="options">Collection of custom field option DTOs to save</param>
        /// <returns>Saved custom field option DTOs</returns>
        Task<IEnumerable<CustomFieldOptionDto>> SaveCustomFieldOptionsAsync(int fieldDefinitionId, IEnumerable<CustomFieldOptionDto> options);

        /// <summary>
        /// Updates the sort order of custom field options
        /// </summary>
        /// <param name="optionIds">Ordered list of option IDs</param>
        /// <returns>True if successful, false otherwise</returns>
        Task<bool> UpdateCustomFieldOptionSortOrderAsync(IEnumerable<int> optionIds);

        #endregion

        #region Custom Field Values

        /// <summary>
        /// Gets custom field values for a specific entity
        /// </summary>
        /// <param name="entityType">Type of entity</param>
        /// <param name="entityId">Entity ID</param>
        /// <returns>Collection of custom field value DTOs for the specified entity</returns>
        Task<IEnumerable<CustomFieldValueDto>> GetCustomFieldValuesByEntityAsync(string entityType, int entityId);

        /// <summary>
        /// Gets custom fields with their values for a specific entity
        /// </summary>
        /// <param name="entityType">Type of entity</param>
        /// <param name="entityId">Entity ID</param>
        /// <returns>Collection of custom fields with values for the specified entity</returns>
        Task<IEnumerable<CustomFieldWithValueDto>> GetCustomFieldsWithValuesByEntityAsync(string entityType, int entityId);

        /// <summary>
        /// Gets custom fields with their values for a specific entity, grouped by group
        /// </summary>
        /// <param name="entityType">Type of entity</param>
        /// <param name="entityId">Entity ID</param>
        /// <returns>Collection of custom field groups with values for the specified entity</returns>
        Task<IEnumerable<CustomFieldGroupDto>> GetCustomFieldsWithValuesByEntityGroupedAsync(string entityType, int entityId);

        /// <summary>
        /// Saves custom field values for an entity
        /// </summary>
        /// <param name="values">Collection of custom field values to save</param>
        /// <returns>Saved custom field value DTOs</returns>
        Task<IEnumerable<CustomFieldValueDto>> SaveCustomFieldValuesAsync(IEnumerable<CustomFieldValueDto> values);

        #endregion
    }
}