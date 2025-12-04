using AutoMapper;
using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;

    public TaskService(ITaskRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TaskResponse>> GetAllTasksAsync()
    {
        var tasks = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<TaskResponse>>(tasks);
    }

    public async Task<IEnumerable<TaskResponse>> GetTasksByStatusAsync(StatusOfTask status)
    {
        var tasks = await _repository.GetByStatusAsync(status);
        return _mapper.Map<IEnumerable<TaskResponse>>(tasks);
    }

    public async Task<TaskResponse?> GetTaskByIdAsync(int id)
    {
        var task = await _repository.GetByIdAsync(id);
        return task == null ? null : _mapper.Map<TaskResponse>(task);
    }

    public async Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request)
    {
        // Use domain factory method
        var task = TaskItem.Create(request.Title, request.Description);
        var createdTask = await _repository.CreateAsync(task);
        return _mapper.Map<TaskResponse>(createdTask);
    }

    public async Task<TaskResponse> UpdateTaskAsync(int id, UpdateTaskRequest request)
    {
        var existingTask = await _repository.GetByIdAsync(id);
        if (existingTask == null)
            throw new KeyNotFoundException($"Task with ID {id} not found");

        // Use domain method to update
        existingTask.UpdateDetails(
            title: request.Title,
            description: request.Description,
            status: request.Status
        );

        var updatedTask = await _repository.UpdateAsync(existingTask);
        return _mapper.Map<TaskResponse>(updatedTask);
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}