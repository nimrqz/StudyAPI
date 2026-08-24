using StudyAPI.Models;

namespace StudyAPI.DTOs;

public class CreateStudyTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    public DateTime? DueDate { get; set; }
    public int? EstimatedMinutes { get; set; }
    public int CategoryId { get; set; }
}

public class UpdateStudyTaskDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public StudyTaskStatus? Status { get; set; }
    public Priority? Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public int? EstimatedMinutes { get; set; }
    public int? ActualMinutes { get; set; }
    public string? Notes { get; set; }
    public int? CategoryId { get; set; }
}

public class StudyTaskResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public StudyTaskStatus Status { get; set; }
    public Priority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public int? EstimatedMinutes { get; set; }
    public int? ActualMinutes { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public CategorySummaryDto Category { get; set; } = null!;
}

public class CategorySummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class StudyTaskQueryDto
{
    public StudyTaskStatus? Status { get; set; }
    public Priority? Priority { get; set; }
    public int? CategoryId { get; set; }
    public DateTime? DueDateFrom { get; set; }
    public DateTime? DueDateTo { get; set; }
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; }
    public bool Ascending { get; set; } = true;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class PaginatedResponseDto<T>
{
    public List<T> Items { get; set; } = new();
    public int CurrentPage { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}
