using StudyAPI.DTOs;
using StudyAPI.Models;
using StudyAPI.Repositories;

namespace StudyAPI.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
    {
        var categories = await _repository.GetAllAsync();
        return categories.Select(MapToResponseDto);
    }

    public async Task<CategoryResponseDto?> GetByIdAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        return category == null ? null : MapToResponseDto(category);
    }

    public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto)
    {
        var existing = await _repository.GetByNameAsync(dto.Name);
        if (existing != null)
            throw new InvalidOperationException($"Já existe uma categoria com o nome '{dto.Name}'.");

        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            Color = dto.Color
        };

        var created = await _repository.CreateAsync(category);
        return MapToResponseDto(created);
    }

    public async Task<CategoryResponseDto?> UpdateAsync(int id, UpdateCategoryDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            return null;

        if (!string.IsNullOrEmpty(dto.Name) && dto.Name != existing.Name)
        {
            var duplicate = await _repository.GetByNameAsync(dto.Name);
            if (duplicate != null)
                throw new InvalidOperationException($"Já existe uma categoria com o nome '{dto.Name}'.");
        }

        if (!string.IsNullOrEmpty(dto.Name))
            existing.Name = dto.Name;
        if (dto.Description != null)
            existing.Description = dto.Description;
        if (dto.Color != null)
            existing.Color = dto.Color;

        existing.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(existing);
        return MapToResponseDto(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _repository.ExistsAsync(id);
    }

    public async Task<int> CountAsync()
    {
        return await _repository.CountAsync();
    }

    private static CategoryResponseDto MapToResponseDto(Category category)
    {
        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Color = category.Color,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt,
            TaskCount = category.StudyTasks?.Count ?? 0
        };
    }
}
