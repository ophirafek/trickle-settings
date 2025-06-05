using AppSettings.API.Data;
using AppSettings.API.DTOs;
using AppSettings.API.Models;
using Microsoft.EntityFrameworkCore;
using Model.Models.CustomFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppSettings.API.Services
{
    /// <summary>
    /// Implementation of the custom field service
    /// </summary>
    public class CustomFieldService : ICustomFieldService
    {
        private readonly AppSettingsContext _context;

        /// <summary>
        /// Constructor that accepts database context
        /// </summary>
        /// <param name="context">Database context</param>
        public CustomFieldService(AppSettingsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region Group Operations

        /// <inheritdoc/>
        public async Task<IEnumerable<CustomFieldGroupDto>> GetAllCustomFieldGroupsAsync()
        {
            var groups = await _context.CustomFieldGroups
                .Where(g => g.IsActive)
                .OrderBy(g => g.SortOrder)
                .ToListAsync();

            return groups.Select(MapGroupToDto);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CustomFieldGroupDto>> GetCustomFieldGroupsByEntityTypeAsync(string entityType)
        {
            var groups = await _context.CustomFieldGroups
                .Where(g => g.EntityType.ToLower() == entityType.ToLower() && g.IsActive)
                .OrderBy(g => g.SortOrder)
                .ToListAsync();

            return groups.Select(MapGroupToDto);
        }

        /// <inheritdoc/>
        public async Task<CustomFieldGroupDto> GetCustomFieldGroupByIdAsync(int id)
        {
            var group = await _context.CustomFieldGroups
                .Include(g => g.Fields.Where(f => f.IsActive).OrderBy(f => f.SortOrder))
                .FirstOrDefaultAsync(g => g.Id == id);

            return group != null ? MapGroupToDto(group) : null;
        }

        /// <inheritdoc/>
        public async Task<CustomFieldGroupDto> SaveCustomFieldGroupAsync(CustomFieldGroupDto groupDto)
        {
            if (groupDto == null)
                throw new ArgumentNullException(nameof(groupDto));

            // Check if this is a new or existing group
            bool isNewGroup = groupDto.Id == 0;

            // Validate that the group name is unique for this entity type
            if (await GroupNameExistsAsync(groupDto.EntityType, groupDto.Name, isNewGroup ? null : groupDto.Id))
            {
                throw new InvalidOperationException($"A group with name '{groupDto.Name}' already exists for entity type '{groupDto.EntityType}'.");
            }

            if (isNewGroup)
            {
                // Create a new group
                var group = new CustomFieldGroup
                {
                    EntityType = groupDto.EntityType,
                    Name = groupDto.Name,
                    DisplayName = groupDto.DisplayName,
                    Description = groupDto.Description,
                    SortOrder = groupDto.SortOrder,
                    IsActive = groupDto.IsActive,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = 1 // TODO: Get from current user
                };

                _context.CustomFieldGroups.Add(group);
                await _context.SaveChangesAsync();

                // Update the DTO with the new ID
                groupDto.Id = group.Id;
                return groupDto;
            }
            else
            {
                // Update an existing group
                var group = await _context.CustomFieldGroups.FindAsync(groupDto.Id);

                if (group == null)
                    throw new InvalidOperationException($"Custom field group with ID {groupDto.Id} not found.");

                // Update properties
                group.EntityType = groupDto.EntityType;
                group.Name = groupDto.Name;
                group.DisplayName = groupDto.DisplayName;
                group.Description = groupDto.Description;
                group.SortOrder = groupDto.SortOrder;
                group.IsActive = groupDto.IsActive;
                group.ModifiedDate = DateTime.UtcNow;
                group.ModifiedBy = 1; // TODO: Get from current user

                await _context.SaveChangesAsync();
                return groupDto;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteCustomFieldGroupAsync(int id)
        {
            var group = await _context.CustomFieldGroups
                .Include(g => g.Fields)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
                return false;

            // Check if there are any fields in this group
            if (group.Fields.Any())
            {
                // Instead of deleting, just mark as inactive
                group.IsActive = false;
                group.ModifiedDate = DateTime.UtcNow;
                group.ModifiedBy = 1; // TODO: Get from current user
            }
            else
            {
                // No fields, so we can safely delete
                _context.CustomFieldGroups.Remove(group);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateCustomFieldGroupSortOrderAsync(IEnumerable<int> groupIds)
        {
            if (groupIds == null)
                throw new ArgumentNullException(nameof(groupIds));

            // Get all groups that need to be updated
            var groupIdArray = groupIds.ToArray();
            var groups = await _context.CustomFieldGroups
                .Where(g => groupIdArray.Contains(g.Id))
                .ToListAsync();

            // Update sort orders
            for (int i = 0; i < groupIdArray.Length; i++)
            {
                var group = groups.FirstOrDefault(g => g.Id == groupIdArray[i]);
                if (group != null)
                {
                    group.SortOrder = i * 10; // Use increments of 10 for easier future insertions
                    group.ModifiedDate = DateTime.UtcNow;
                    group.ModifiedBy = 1; // TODO: Get from current user
                }
            }

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> GroupNameExistsAsync(string entityType, string name, int? excludeId = null)
        {
            return await _context.CustomFieldGroups
                .AnyAsync(g => g.EntityType.ToLower() == entityType.ToLower() &&
                               g.Name.ToLower() == name.ToLower() &&
                               (excludeId == null || g.Id != excludeId));
        }

        #endregion

        #region Definition Operations

        /// <inheritdoc/>
        public async Task<IEnumerable<CustomFieldDefinitionDto>> GetAllCustomFieldDefinitionsAsync()
        {
            var definitions = await _context.CustomFieldDefinitions
                .Include(d => d.Options.Where(o => o.IsActive).OrderBy(o => o.SortOrder))
                .Where(d => d.IsActive)
                .OrderBy(d => d.SortOrder)
                .ToListAsync();

            return definitions.Select(MapDefinitionToDto);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CustomFieldDefinitionDto>> GetCustomFieldDefinitionsByEntityTypeAsync(string entityType)
        {
            var definitions = await _context.CustomFieldDefinitions
                .Include(d => d.Options.Where(o => o.IsActive).OrderBy(o => o.SortOrder))
                .Where(d => d.EntityType.ToLower() == entityType.ToLower() && d.IsActive)
                .OrderBy(d => d.SortOrder)
                .ToListAsync();

            return definitions.Select(MapDefinitionToDto);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CustomFieldGroupDto>> GetCustomFieldDefinitionsByEntityTypeGroupedAsync(string entityType)
        {
            // First get all groups for this entity type
            var groups = await _context.CustomFieldGroups
                .Where(g => g.EntityType.ToLower() == entityType.ToLower() && g.IsActive)
                .OrderBy(g => g.SortOrder)
                .ToListAsync();

            // Create a result list with all existing groups
            var result = groups.Select(g => new CustomFieldGroupDto
            {
                Id = g.Id,
                EntityType = g.EntityType,
                Name = g.Name,
                DisplayName = g.DisplayName,
                Description = g.Description,
                SortOrder = g.SortOrder,
                IsActive = g.IsActive,
                Fields = new List<CustomFieldDefinitionDto>()
            }).ToList();

            // Add a default group for fields without a group
            var defaultGroup = new CustomFieldGroupDto
            {
                Id = 0,
                EntityType = entityType,
                Name = "General",
                DisplayName = "General",
                SortOrder = int.MaxValue, // Put at the end
                IsActive = true,
                Fields = new List<CustomFieldDefinitionDto>()
            };

            // Get all fields for this entity type
            var definitions = await _context.CustomFieldDefinitions
                .Include(d => d.Options.Where(o => o.IsActive).OrderBy(o => o.SortOrder))
                .Where(d => d.EntityType.ToLower() == entityType.ToLower() && d.IsActive)
                .OrderBy(d => d.SortOrder)
                .ToListAsync();

            // Distribute fields to their groups
            foreach (var definition in definitions)
            {
                var definitionDto = MapDefinitionToDto(definition);

                if (definition.GroupId.HasValue)
                {
                    // Find the group for this field
                    var group = result.FirstOrDefault(g => g.Id == definition.GroupId.Value);
                    if (group != null)
                    {
                        group.Fields.Add(definitionDto);
                    }
                    else
                    {
                        // If group not found (might have been deleted), add to default group
                        defaultGroup.Fields.Add(definitionDto);
                    }
                }
                else
                {
                    // Fields without a group go to the default group
                    defaultGroup.Fields.Add(definitionDto);
                }
            }

            // Only add the default group if it has fields
            if (defaultGroup.Fields.Count > 0)
            {
                result.Add(defaultGroup);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CustomFieldDefinitionDto>> GetCustomFieldDefinitionsByGroupIdAsync(int groupId)
        {
            var definitions = await _context.CustomFieldDefinitions
                .Include(d => d.Options.Where(o => o.IsActive).OrderBy(o => o.SortOrder))
                .Where(d => d.GroupId == groupId && d.IsActive)
                .OrderBy(d => d.SortOrder)
                .ToListAsync();

            return definitions.Select(MapDefinitionToDto);
        }

        /// <inheritdoc/>
        public async Task<CustomFieldDefinitionDto> GetCustomFieldDefinitionByIdAsync(int id)
        {
            var definition = await _context.CustomFieldDefinitions
                .Include(d => d.Options.Where(o => o.IsActive).OrderBy(o => o.SortOrder))
                .FirstOrDefaultAsync(d => d.Id == id);

            return definition != null ? MapDefinitionToDto(definition) : null;
        }

        /// <inheritdoc/>
        public async Task<CustomFieldDefinitionDto> SaveCustomFieldDefinitionAsync(CustomFieldDefinitionDto definitionDto)
        {
            if (definitionDto == null)
                throw new ArgumentNullException(nameof(definitionDto));

            // Check if this is a new or existing definition
            bool isNewDefinition = definitionDto.Id == 0;

            // Validate that the field name is unique for this entity type
            if (await FieldNameExistsAsync(definitionDto.EntityType, definitionDto.Name, isNewDefinition ? null : definitionDto.Id))
            {
                throw new InvalidOperationException($"A field with name '{definitionDto.Name}' already exists for entity type '{definitionDto.EntityType}'.");
            }

            if (isNewDefinition)
            {
                // Create a new definition
                var definition = new CustomFieldDefinition
                {
                    EntityType = definitionDto.EntityType,
                    Name = definitionDto.Name,
                    DisplayName = definitionDto.DisplayName,
                    Description = definitionDto.Description,
                    FieldType = definitionDto.FieldType,
                    IsRequired = definitionDto.IsRequired,
                    IsActive = definitionDto.IsActive,
                    SortOrder = definitionDto.SortOrder,
                    DefaultValue = definitionDto.DefaultValue != null ? System.Text.Json.JsonSerializer.Serialize(definitionDto.DefaultValue) : null,
                    MinValue = definitionDto.MinValue,
                    MaxValue = definitionDto.MaxValue,
                    MaxLength = definitionDto.MaxLength,
                    Regex = definitionDto.Regex,
                    GeneralCodeType = definitionDto.GeneralCodeType,
                    GroupId = definitionDto.GroupId,
                    GroupName = definitionDto.GroupName,
                    IsVisible = definitionDto.IsVisible,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = 1 // TODO: Get from current user
                };

                _context.CustomFieldDefinitions.Add(definition);
                await _context.SaveChangesAsync();

                // Update the DTO with the new ID
                definitionDto.Id = definition.Id;

                // Save options if provided
                if (definitionDto.Options != null && definitionDto.Options.Count > 0)
                {
                    await SaveCustomFieldOptionsAsync(definition.Id, definitionDto.Options);
                }

                return definitionDto;
            }
            else
            {
                // Update an existing definition
                var definition = await _context.CustomFieldDefinitions.FindAsync(definitionDto.Id);

                if (definition == null)
                    throw new InvalidOperationException($"Custom field definition with ID {definitionDto.Id} not found.");

                // Update properties
                definition.EntityType = definitionDto.EntityType;
                definition.Name = definitionDto.Name;
                definition.DisplayName = definitionDto.DisplayName;
                definition.Description = definitionDto.Description;
                definition.FieldType = definitionDto.FieldType;
                definition.IsRequired = definitionDto.IsRequired;
                definition.IsActive = definitionDto.IsActive;
                definition.SortOrder = definitionDto.SortOrder;
                definition.DefaultValue = definitionDto.DefaultValue != null ? System.Text.Json.JsonSerializer.Serialize(definitionDto.DefaultValue) : null;
                definition.MinValue = definitionDto.MinValue;
                definition.MaxValue = definitionDto.MaxValue;
                definition.MaxLength = definitionDto.MaxLength;
                definition.Regex = definitionDto.Regex;
                definition.GeneralCodeType = definitionDto.GeneralCodeType;
                definition.GroupId = definitionDto.GroupId;
                definition.GroupName = definitionDto.GroupName;
                definition.IsVisible = definitionDto.IsVisible;
                definition.ModifiedDate = DateTime.UtcNow;
                definition.ModifiedBy = 1; // TODO: Get from current user

                await _context.SaveChangesAsync();

                // Update options if provided
                if (definitionDto.Options != null && definitionDto.Options.Count > 0)
                {
                    await SaveCustomFieldOptionsAsync(definition.Id, definitionDto.Options);
                }

                return definitionDto;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteCustomFieldDefinitionAsync(int id)
        {
            var definition = await _context.CustomFieldDefinitions.FindAsync(id);

            if (definition == null)
                return false;

            // Check if there are any values for this definition
            var hasValues = await _context.CustomFieldValues.AnyAsync(v => v.FieldDefinitionId == id);

            if (hasValues)
            {
                // Instead of deleting, just mark as inactive
                definition.IsActive = false;
                definition.ModifiedDate = DateTime.UtcNow;
                definition.ModifiedBy = 1; // TODO: Get from current user
            }
            else
            {
                // No values, so we can safely delete
                _context.CustomFieldDefinitions.Remove(definition);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateCustomFieldSortOrderAsync(IEnumerable<int> fieldIds)
        {
            if (fieldIds == null)
                throw new ArgumentNullException(nameof(fieldIds));

            // Get all fields that need to be updated
            var fieldIdArray = fieldIds.ToArray();
            var fields = await _context.CustomFieldDefinitions
                .Where(d => fieldIdArray.Contains(d.Id))
                .ToListAsync();

            // Update sort orders
            for (int i = 0; i < fieldIdArray.Length; i++)
            {
                var field = fields.FirstOrDefault(f => f.Id == fieldIdArray[i]);
                if (field != null)
                {
                    field.SortOrder = i * 10; // Use increments of 10 for easier future insertions
                    field.ModifiedDate = DateTime.UtcNow;
                    field.ModifiedBy = 1; // TODO: Get from current user
                }
            }

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> FieldNameExistsAsync(string entityType, string name, int? excludeId = null)
        {
            return await _context.CustomFieldDefinitions
                .AnyAsync(d => d.EntityType.ToLower() == entityType.ToLower() &&
                               d.Name.ToLower() == name.ToLower() &&
                               (excludeId == null || d.Id != excludeId));
        }

        #endregion

        #region Option Operations

        /// <inheritdoc/>
        public async Task<IEnumerable<CustomFieldOptionDto>> GetCustomFieldOptionsAsync(int fieldDefinitionId)
        {
            var options = await _context.CustomFieldOptions
                .Where(o => o.FieldDefinitionId == fieldDefinitionId && o.IsActive)
                .OrderBy(o => o.SortOrder)
                .ToListAsync();

            return options.Select(MapOptionToDto);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CustomFieldOptionDto>> SaveCustomFieldOptionsAsync(int fieldDefinitionId, IEnumerable<CustomFieldOptionDto> optionDtos)
        {
            if (optionDtos == null)
                throw new ArgumentNullException(nameof(optionDtos));

            // Verify the field definition exists
            var definition = await _context.CustomFieldDefinitions.FindAsync(fieldDefinitionId);
            if (definition == null)
                throw new InvalidOperationException($"Custom field definition with ID {fieldDefinitionId} not found.");

            // Get existing options
            var existingOptions = await _context.CustomFieldOptions
                .Where(o => o.FieldDefinitionId == fieldDefinitionId)
                .ToListAsync();

            var results = new List<CustomFieldOptionDto>();
            var optionsArray = optionDtos.ToArray();

            // Process each option
            for (int i = 0; i < optionsArray.Length; i++)
            {
                var optionDto = optionsArray[i];

                // Ensure the option is associated with the correct field
                optionDto.FieldDefinitionId = fieldDefinitionId;

                // Set the sort order if not specified
                if (optionDto.SortOrder == 0)
                {
                    optionDto.SortOrder = i * 10; // Use increments of 10 for easier future insertions
                }

                // Check if this is a new or existing option
                bool isNewOption = optionDto.Id == 0;

                if (isNewOption)
                {
                    // Create a new option
                    var option = new CustomFieldOption
                    {
                        FieldDefinitionId = fieldDefinitionId,
                        Value = optionDto.Value,
                        DisplayText = optionDto.DisplayText,
                        SortOrder = optionDto.SortOrder,
                        IsActive = optionDto.IsActive,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = 1 // TODO: Get from current user
                    };

                    _context.CustomFieldOptions.Add(option);
                    await _context.SaveChangesAsync();

                    // Update the DTO with the new ID
                    optionDto.Id = option.Id;
                    results.Add(optionDto);
                }
                else
                {
                    // Update an existing option
                    var option = existingOptions.FirstOrDefault(o => o.Id == optionDto.Id);

                    if (option == null)
                    {
                        // Option with this ID not found, create a new one
                        option = new CustomFieldOption
                        {
                            FieldDefinitionId = fieldDefinitionId,
                            Value = optionDto.Value,
                            DisplayText = optionDto.DisplayText,
                            SortOrder = optionDto.SortOrder,
                            IsActive = optionDto.IsActive,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = 1 // TODO: Get from current user
                        };

                        _context.CustomFieldOptions.Add(option);
                    }
                    else
                    {
                        // Update properties
                        option.Value = optionDto.Value;
                        option.DisplayText = optionDto.DisplayText;
                        option.SortOrder = optionDto.SortOrder;
                        option.IsActive = optionDto.IsActive;
                        option.ModifiedDate = DateTime.UtcNow;
                        option.ModifiedBy = 1; // TODO: Get from current user
                    }

                    await _context.SaveChangesAsync();
                    results.Add(optionDto);
                }
            }

            // Handle deletion of options not in the provided list
            var optionIdsToKeep = optionsArray.Where(o => o.Id > 0).Select(o => o.Id).ToArray();
            var optionsToDelete = existingOptions.Where(o => !optionIdsToKeep.Contains(o.Id)).ToList();

            foreach (var option in optionsToDelete)
            {
                // Check if there are any values using this option
                var hasValues = await _context.CustomFieldValues
                    .AnyAsync(v => v.FieldDefinitionId == fieldDefinitionId &&
                                  (v.NumberValue == option.Id ||
                                   (v.SelectedOptions != null && v.SelectedOptions.Contains(option.Id.ToString()))));

                if (hasValues)
                {
                    // Just mark as inactive
                    option.IsActive = false;
                    option.ModifiedDate = DateTime.UtcNow;
                    option.ModifiedBy = 1; // TODO: Get from current user
                }
                else
                {
                    // No values, so we can safely delete
                    _context.CustomFieldOptions.Remove(option);
                }
            }

            await _context.SaveChangesAsync();
            return results;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateCustomFieldOptionSortOrderAsync(IEnumerable<int> optionIds)
        {
            if (optionIds == null)
                throw new ArgumentNullException(nameof(optionIds));

            // Get all options that need to be updated
            var optionIdArray = optionIds.ToArray();
            var options = await _context.CustomFieldOptions
                .Where(o => optionIdArray.Contains(o.Id))
                .ToListAsync();

            // Update sort orders
            for (int i = 0; i < optionIdArray.Length; i++)
            {
                var option = options.FirstOrDefault(o => o.Id == optionIdArray[i]);
                if (option != null)
                {
                    option.SortOrder = i * 10; // Use increments of 10 for easier future insertions
                    option.ModifiedDate = DateTime.UtcNow;
                    option.ModifiedBy = 1; // TODO: Get from current user
                }
            }

            return await _context.SaveChangesAsync() > 0;
        }

        #endregion

        #region Value Operations

        /// <inheritdoc/>
        public async Task<IEnumerable<CustomFieldValueDto>> GetCustomFieldValuesByEntityAsync(string entityType, int entityId)
        {
            var values = await _context.CustomFieldValues
                .Where(v => v.EntityType.ToLower() == entityType.ToLower() && v.EntityId == entityId)
                .ToListAsync();

            return values.Select(MapValueToDto);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CustomFieldWithValueDto>> GetCustomFieldsWithValuesByEntityAsync(string entityType, int entityId)
        {
            // Get all active field definitions for this entity type
            var definitions = await _context.CustomFieldDefinitions
                .Include(d => d.Options.Where(o => o.IsActive).OrderBy(o => o.SortOrder))
                .Where(d => d.EntityType.ToLower() == entityType.ToLower() && d.IsActive)
                .OrderBy(d => d.SortOrder)
                .ToListAsync();

            // If this is a new entity (ID = 0), return definitions with null values
            if (entityId == 0)
            {
                return definitions.Select(d => new CustomFieldWithValueDto
                {
                    Definition = MapDefinitionToDto(d),
                    Value = null
                });
            }

            // Get all values for this entity
            var values = await _context.CustomFieldValues
                .Where(v => v.EntityType.ToLower() == entityType.ToLower() && v.EntityId == entityId)
                .ToListAsync();

            // Create a dictionary for quick lookup
            var valuesByDefinitionId = values.ToDictionary(v => v.FieldDefinitionId);

            // Combine definitions with their values
            return definitions.Select(d => new CustomFieldWithValueDto
            {
                Definition = MapDefinitionToDto(d),
                Value = valuesByDefinitionId.TryGetValue(d.Id, out var value) ? MapValueToDto(value) : null
            });
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CustomFieldGroupDto>> GetCustomFieldsWithValuesByEntityGroupedAsync(string entityType, int entityId)
        {
            // First get the fields with values
            var fieldsWithValues = await GetCustomFieldsWithValuesByEntityAsync(entityType, entityId);

            // Get all groups for this entity type
            var groups = await _context.CustomFieldGroups
                .Where(g => g.EntityType.ToLower() == entityType.ToLower() && g.IsActive)
                .OrderBy(g => g.SortOrder)
                .ToListAsync();

            // Create a result list with all existing groups
            var result = groups.Select(g => new CustomFieldGroupDto
            {
                Id = g.Id,
                EntityType = g.EntityType,
                Name = g.Name,
                DisplayName = g.DisplayName,
                Description = g.Description,
                SortOrder = g.SortOrder,
                IsActive = g.IsActive,
                FieldsWithValues = new List<CustomFieldWithValueDto>()
            }).ToList();

            // Add a default group for fields without a group
            var defaultGroup = new CustomFieldGroupDto
            {
                Id = 0,
                EntityType = entityType,
                Name = "General",
                DisplayName = "General",
                SortOrder = int.MaxValue, // Put at the end
                IsActive = true,
                FieldsWithValues = new List<CustomFieldWithValueDto>()
            };

            // Distribute fields to their groups
            foreach (var field in fieldsWithValues)
            {
                if (field.Definition.GroupId.HasValue)
                {
                    // Find the group for this field
                    var group = result.FirstOrDefault(g => g.Id == field.Definition.GroupId.Value);
                    if (group != null)
                    {
                        group.FieldsWithValues.Add(field);
                    }
                    else
                    {
                        // If group not found (might have been deleted), add to default group
                        defaultGroup.FieldsWithValues.Add(field);
                    }
                }
                else
                {
                    // Fields without a group go to the default group
                    defaultGroup.FieldsWithValues.Add(field);
                }
            }

            // Only add the default group if it has fields
            if (defaultGroup.FieldsWithValues.Count > 0)
            {
                result.Add(defaultGroup);
            }

            // Remove empty groups
            result.RemoveAll(g => g.FieldsWithValues.Count == 0);

            return result;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CustomFieldValueDto>> SaveCustomFieldValuesAsync(IEnumerable<CustomFieldValueDto> valueDtos)
        {
            if (valueDtos == null)
                throw new ArgumentNullException(nameof(valueDtos));

            var results = new List<CustomFieldValueDto>();

            foreach (var valueDto in valueDtos)
            {
                // Verify the field definition exists
                var definition = await _context.CustomFieldDefinitions.FindAsync(valueDto.FieldDefinitionId);
                if (definition == null)
                    throw new InvalidOperationException($"Custom field definition with ID {valueDto.FieldDefinitionId} not found.");

                // Check if this is a new or existing value
                CustomFieldValue value;

                // Try to find an existing value
                value = await _context.CustomFieldValues
                    .FirstOrDefaultAsync(v => v.EntityType.ToLower() == valueDto.EntityType.ToLower() &&
                                             v.EntityId == valueDto.EntityId &&
                                             v.FieldDefinitionId == valueDto.FieldDefinitionId);

                if (value == null)
                {
                    // Create a new value
                    value = new CustomFieldValue
                    {
                        EntityId = valueDto.EntityId,
                        EntityType = valueDto.EntityType,
                        FieldDefinitionId = valueDto.FieldDefinitionId,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = 1 // TODO: Get from current user
                    };
                    _context.CustomFieldValues.Add(value);
                }

                // Update properties based on field type
                switch (definition.FieldType.ToLower())
                {
                    case "text":
                    case "textarea":
                        value.TextValue = valueDto.TextValue;
                        break;
                    case "number":
                    case "select":
                    case "general-code":
                        value.NumberValue = valueDto.NumberValue;
                        break;
                    case "date":
                        value.DateValue = valueDto.DateValue;
                        break;
                    case "boolean":
                        value.BooleanValue = valueDto.BooleanValue;
                        break;
                    case "multi-select":
                        value.SelectedOptions = valueDto.SelectedOptionIds != null && valueDto.SelectedOptionIds.Count > 0
                            ? System.Text.Json.JsonSerializer.Serialize(valueDto.SelectedOptionIds)
                            : null;
                        break;
                }

                value.ModifiedDate = DateTime.UtcNow;
                value.ModifiedBy = 1; // TODO: Get from current user

                await _context.SaveChangesAsync();

                // Update the DTO with the ID
                valueDto.Id = value.Id;
                results.Add(valueDto);
            }

            return results;
        }

        #endregion

        #region Mapping Methods

        /// <summary>
        /// Maps a CustomFieldGroup entity to a CustomFieldGroupDto
        /// </summary>
        private CustomFieldGroupDto MapGroupToDto(CustomFieldGroup group)
        {
            var dto = new CustomFieldGroupDto
            {
                Id = group.Id,
                EntityType = group.EntityType,
                Name = group.Name,
                DisplayName = group.DisplayName,
                Description = group.Description,
                SortOrder = group.SortOrder,
                IsActive = group.IsActive,
                Fields = new List<CustomFieldDefinitionDto>()
            };

            // Map fields if loaded
            if (group.Fields != null)
            {
                dto.Fields = group.Fields
                    .Where(f => f.IsActive)
                    .OrderBy(f => f.SortOrder)
                    .Select(MapDefinitionToDto)
                    .ToList();
            }

            return dto;
        }

        /// <summary>
        /// Maps a CustomFieldDefinition entity to a CustomFieldDefinitionDto
        /// </summary>
        private CustomFieldDefinitionDto MapDefinitionToDto(CustomFieldDefinition definition)
        {
            var dto = new CustomFieldDefinitionDto
            {
                Id = definition.Id,
                EntityType = definition.EntityType,
                Name = definition.Name,
                DisplayName = definition.DisplayName,
                Description = definition.Description,
                FieldType = definition.FieldType,
                IsRequired = definition.IsRequired,
                IsActive = definition.IsActive,
                SortOrder = definition.SortOrder,
                MinValue = definition.MinValue,
                MaxValue = definition.MaxValue,
                MaxLength = definition.MaxLength,
                Regex = definition.Regex,
                GeneralCodeType = definition.GeneralCodeType,
                GroupId = definition.GroupId,
                GroupName = definition.GroupName ?? "General",
                IsVisible = definition.IsVisible,
                Options = new List<CustomFieldOptionDto>()
            };

            // Deserialize default value
            if (!string.IsNullOrEmpty(definition.DefaultValue))
            {
                try
                {
                    dto.DefaultValue = System.Text.Json.JsonSerializer.Deserialize<object>(definition.DefaultValue);
                }
                catch (System.Text.Json.JsonException)
                {
                    // If deserialization fails, use the raw string
                    dto.DefaultValue = definition.DefaultValue;
                }
            }

            // Map options if loaded
            if (definition.Options != null)
            {
                dto.Options = definition.Options
                    .Where(o => o.IsActive)
                    .OrderBy(o => o.SortOrder)
                    .Select(MapOptionToDto)
                    .ToList();
            }

            return dto;
        }

        /// <summary>
        /// Maps a CustomFieldOption entity to a CustomFieldOptionDto
        /// </summary>
        private CustomFieldOptionDto MapOptionToDto(CustomFieldOption option)
        {
            return new CustomFieldOptionDto
            {
                Id = option.Id,
                FieldDefinitionId = option.FieldDefinitionId,
                Value = option.Value,
                DisplayText = option.DisplayText,
                SortOrder = option.SortOrder,
                IsActive = option.IsActive
            };
        }

        /// <summary>
        /// Maps a CustomFieldValue entity to a CustomFieldValueDto
        /// </summary>
        private CustomFieldValueDto MapValueToDto(CustomFieldValue value)
        {
            var dto = new CustomFieldValueDto
            {
                Id = value.Id,
                EntityId = value.EntityId,
                EntityType = value.EntityType,
                FieldDefinitionId = value.FieldDefinitionId,
                TextValue = value.TextValue,
                NumberValue = value.NumberValue,
                DateValue = value.DateValue,
                BooleanValue = value.BooleanValue,
                SelectedOptionIds = new List<int>()
            };

            // Deserialize selected options
            if (!string.IsNullOrEmpty(value.SelectedOptions))
            {
                try
                {
                    dto.SelectedOptionIds = System.Text.Json.JsonSerializer.Deserialize<List<int>>(value.SelectedOptions);
                }
                catch (System.Text.Json.JsonException)
                {
                    // If deserialization fails, use an empty list
                    dto.SelectedOptionIds = new List<int>();
                }
            }

            return dto;
        }

        #endregion
    }
}