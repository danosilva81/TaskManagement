using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs;

public class UpdateTaskRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public StatusOfTask? Status { get; set; }
}
