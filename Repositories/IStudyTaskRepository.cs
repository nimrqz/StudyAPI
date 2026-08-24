using StudyAPI.DTOs;
using StudyAPI.Models;

namespace StudyAPI.Repositories;

public interface IStudyTaskRepository
{
    Task<(IEnumerable<StudyTask> Items, int TotalCount)> GetFilteredAsync(StudyTaskQueryDto query);
    Task<StudyTask?> GetByIdAsync(int id);
    Task<StudyTask> CreateAsync(StudyTask task);
    Task<bool> UpdateAsync(StudyTask task);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<Dictionary<StudyTaskStatus, int>> CountByStatusAsync();
    Task<Dictionary<Priority, int>> CountByPriorityAsync();
}
