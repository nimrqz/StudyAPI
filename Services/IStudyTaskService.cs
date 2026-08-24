using StudyAPI.DTOs;
using StudyAPI.Models;

namespace StudyAPI.Services;

public interface IStudyTaskService
{
    Task<PaginatedResponseDto<StudyTaskResponseDto>> GetFilteredAsync(StudyTaskQueryDto query);
    Task<StudyTaskResponseDto?> GetByIdAsync(int id);
    Task<StudyTaskResponseDto> CreateAsync(CreateStudyTaskDto dto);
    Task<StudyTaskResponseDto?> UpdateAsync(int id, UpdateStudyTaskDto dto);
    Task<bool> DeleteAsync(int id);
    Task<Dictionary<StudyTaskStatus, int>> GetCountByStatusAsync();
    Task<Dictionary<Priority, int>> GetCountByPriorityAsync();
}
