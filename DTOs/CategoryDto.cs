namespace StudyAPI.DTOs;

public class CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Color { get; set; }
}

public class UpdateCategoryDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; }
}

public class CategoryResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Color { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int TaskCount { get; set; }
}
