using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public StatusOfTask Status { get; set; }

    // Parameterless constructor for EF Core
    public TaskItem()
    {
        Title = string.Empty;
        Status = StatusOfTask.Todo;
        CreatedAt = DateTime.UtcNow;
    }

    // Factory method for creating new tasks with validation
    public static TaskItem Create(string title, string? description = null, StatusOfTask status = StatusOfTask.Todo)
    {
        ValidateTitle(title);
        
        return new TaskItem
        {
            Title = title.Trim(),
            Description = description?.Trim(),
            Status = status,
            CreatedAt = DateTime.UtcNow
        };
    }

    // Method for updating with validation
    public void UpdateDetails(string? title = null, string? description = null, StatusOfTask? status = null)
    {
        if (title != null)
        {
            ValidateTitle(title);
            Title = title.Trim();
        }

        if (description != null)
            Description = description.Trim();

        if (status.HasValue)
            Status = status.Value;

        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        if (title.Trim().Length > 250)
            throw new ArgumentException("Title cannot exceed 250 characters.", nameof(title));
    }
}