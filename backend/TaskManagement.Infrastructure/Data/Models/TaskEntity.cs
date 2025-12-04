using TaskManagement.Domain.Enums;

namespace TaskManagement.Infrastructure.Data.Models;

public class TaskEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public StatusOfTask Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}