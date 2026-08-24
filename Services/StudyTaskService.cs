using StudyAPI.DTOs;
using StudyAPI.Models;
using StudyAPI.Repositories;

namespace StudyAPI.Services;

public class StudyTaskService : IStudyTaskService
{
    private readonly IStudyTaskRepository _taskRepository;
    private readonly ICategoryRepository _categoryRepository;

    public StudyTaskService(
        IStudyTaskRepository taskRepository,
        ICategoryRepository categoryRepository)
    {
        _taskRepository = taskRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<PaginatedResponseDto<StudyTaskResponseDto>> GetFilteredAsync(StudyTaskQueryDto query)
    {
        if (query.PageSize > 100)
            query.PageSize = 100;

        var (items, totalCount) = await _taskRepository.GetFilteredAsync(query);
        var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);
        var responseDtos = items.Select(MapToResponseDto).ToList();

        return new PaginatedResponseDto<StudyTaskResponseDto>
        {
            Items = responseDtos,
            CurrentPage = query.Page,
            TotalItems = totalCount,
            TotalPages = totalPages,
            HasNextPage = query.Page < totalPages,
            HasPreviousPage = query.Page > 1
        };
    }

    public async Task<StudyTaskResponseDto?> GetByIdAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        return task == null ? null : MapToResponseDto(task);
    }

    public async Task<StudyTaskResponseDto> CreateAsync(CreateStudyTaskDto dto)
    {
        if (!await _categoryRepository.ExistsAsync(dto.CategoryId))
            throw new KeyNotFoundException($"Categoria com ID {dto.CategoryId} não encontrada.");

        var task = new StudyTask
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            DueDate = dto.DueDate,
            EstimatedMinutes = dto.EstimatedMinutes,
            CategoryId = dto.CategoryId,
            Status = StudyTaskStatus.Pending
        };

        var created = await _taskRepository.CreateAsync(task);
        var taskWithCategory = await _taskRepository.GetByIdAsync(created.Id);
        return MapToResponseDto(taskWithCategory!);
    }

    public async Task<StudyTaskResponseDto?> UpdateAsync(int id, UpdateStudyTaskDto dto)
    {
        var existing = await _taskRepository.GetByIdAsync(id);
        if (existing == null)
            return null;

        if (dto.CategoryId.HasValue && dto.CategoryId != existing.CategoryId)
        {
            if (!await _categoryRepository.ExistsAsync(dto.CategoryId.Value))
                throw new KeyNotFoundException($"Categoria com ID {dto.CategoryId} não encontrada.");
        }

        if (dto.Title != null)
            existing.Title = dto.Title;
        if (dto.Description != null)
            existing.Description = dto.Description;
        if (dto.Status.HasValue)
            existing.Status = dto.Status.Value;
        if (dto.Priority.HasValue)
            existing.Priority = dto.Priority.Value;
        if (dto.DueDate.HasValue)
            existing.DueDate = dto.DueDate;
        if (dto.EstimatedMinutes.HasValue)
            existing.EstimatedMinutes = dto.EstimatedMinutes;
        if (dto.ActualMinutes.HasValue)
            existing.ActualMinutes = dto.ActualMinutes;
        if (dto.Notes != null)
            existing.Notes = dto.Notes;
        if (dto.CategoryId.HasValue)
            existing.CategoryId = dto.CategoryId.Value;

        existing.UpdatedAt = DateTime.UtcNow;

        await _taskRepository.UpdateAsync(existing);

        var updated = await _taskRepository.GetByIdAsync(id);
        return MapToResponseDto(updated!);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _taskRepository.DeleteAsync(id);
    }

    public async Task<Dictionary<StudyTaskStatus, int>> GetCountByStatusAsync()
    {
        return await _taskRepository.CountByStatusAsync();
    }

    public async Task<Dictionary<Priority, int>> GetCountByPriorityAsync()
    {
        return await _taskRepository.CountByPriorityAsync();
    }

    private static StudyTaskResponseDto MapToResponseDto(StudyTask task)
    {
        return new StudyTaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            EstimatedMinutes = task.EstimatedMinutes,
            ActualMinutes = task.ActualMinutes,
            Notes = task.Notes,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            Category = new CategorySummaryDto
            {
                Id = task.Category.Id,
                Name = task.Category.Name
            }
        };
    }
}
