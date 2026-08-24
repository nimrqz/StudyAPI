using Microsoft.EntityFrameworkCore;
using StudyAPI.Data;
using StudyAPI.DTOs;
using StudyAPI.Models;

namespace StudyAPI.Repositories;

public class StudyTaskRepository : IStudyTaskRepository
{
    private readonly StudyDbContext _context;

    public StudyTaskRepository(StudyDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<StudyTask> Items, int TotalCount)> GetFilteredAsync(StudyTaskQueryDto query)
    {
        IQueryable<StudyTask> queryable = _context.StudyTasks
            .Include(t => t.Category);

        if (query.Status.HasValue)
            queryable = queryable.Where(t => t.Status == query.Status.Value);

        if (query.Priority.HasValue)
            queryable = queryable.Where(t => t.Priority == query.Priority.Value);

        if (query.CategoryId.HasValue)
            queryable = queryable.Where(t => t.CategoryId == query.CategoryId.Value);

        if (query.DueDateFrom.HasValue)
            queryable = queryable.Where(t => t.DueDate >= query.DueDateFrom.Value);

        if (query.DueDateTo.HasValue)
            queryable = queryable.Where(t => t.DueDate <= query.DueDateTo.Value);

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var searchTerm = query.SearchTerm.ToLower();
            queryable = queryable.Where(t =>
                t.Title.ToLower().Contains(searchTerm) ||
                (t.Description != null && t.Description.ToLower().Contains(searchTerm)));
        }

        queryable = query.SortBy?.ToLower() switch
        {
            "title" => query.Ascending
                ? queryable.OrderBy(t => t.Title)
                : queryable.OrderByDescending(t => t.Title),
            "duedate" => query.Ascending
                ? queryable.OrderBy(t => t.DueDate)
                : queryable.OrderByDescending(t => t.DueDate),
            "priority" => query.Ascending
                ? queryable.OrderBy(t => t.Priority)
                : queryable.OrderByDescending(t => t.Priority),
            "status" => query.Ascending
                ? queryable.OrderBy(t => t.Status)
                : queryable.OrderByDescending(t => t.Status),
            _ => query.Ascending
                ? queryable.OrderBy(t => t.CreatedAt)
                : queryable.OrderByDescending(t => t.CreatedAt)
        };

        var totalCount = await queryable.CountAsync();

        var items = await queryable
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<StudyTask?> GetByIdAsync(int id)
    {
        return await _context.StudyTasks
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<StudyTask> CreateAsync(StudyTask task)
    {
        _context.StudyTasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<bool> UpdateAsync(StudyTask task)
    {
        var existing = await _context.StudyTasks.FindAsync(task.Id);
        if (existing == null)
            return false;

        _context.Entry(existing).CurrentValues.SetValues(task);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _context.StudyTasks.FindAsync(id);
        if (task == null)
            return false;

        _context.StudyTasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.StudyTasks.AnyAsync(t => t.Id == id);
    }

    public async Task<Dictionary<StudyTaskStatus, int>> CountByStatusAsync()
    {
        return await _context.StudyTasks
            .GroupBy(t => t.Status)
            .ToDictionaryAsync(g => g.Key, g => g.Count());
    }

    public async Task<Dictionary<Priority, int>> CountByPriorityAsync()
    {
        return await _context.StudyTasks
            .GroupBy(t => t.Priority)
            .ToDictionaryAsync(g => g.Key, g => g.Count());
    }
}
