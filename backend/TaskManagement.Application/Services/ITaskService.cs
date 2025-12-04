using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskResponse>> GetAllTasksAsync();
    Task<IEnumerable<TaskResponse>> GetTasksByStatusAsync(StatusOfTask status);
    Task<TaskResponse?> GetTaskByIdAsync(int id);
    Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request);
    Task<TaskResponse> UpdateTaskAsync(int id, UpdateTaskRequest request);
    Task<bool> DeleteTaskAsync(int id);
}