using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Infrastructure.Data.Models;

namespace TaskManagement.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        var entities = await _context.Tasks.ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<TaskItem>> GetByStatusAsync(StatusOfTask status)
    {
        var entities = await _context.Tasks
            .Where(t => t.Status == status)
            .ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        var entity = await _context.Tasks.FindAsync(id);
        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<TaskItem> CreateAsync(TaskItem task)
    {
        var entity = MapToEntity(task);
        entity.CreatedAt = DateTime.UtcNow;
        
        _context.Tasks.Add(entity);
        await _context.SaveChangesAsync();
        
        return MapToDomain(entity);
    }

    public async Task<TaskItem> UpdateAsync(TaskItem task)
    {
        var entity = await _context.Tasks.FindAsync(task.Id);
        if (entity == null)
            throw new KeyNotFoundException($"Task with ID {task.Id} not found");

        entity.Title = task.Title;
        entity.Description = task.Description;
        entity.Status = task.Status;
        entity.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return MapToDomain(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Tasks.FindAsync(id);
        if (entity == null)
            return false;

        _context.Tasks.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    // Mapping methods
    private static TaskItem MapToDomain(TaskEntity entity)
    {
        return new TaskItem
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    private static TaskEntity MapToEntity(TaskItem domain)
    {
        return new TaskEntity
        {
            Id = domain.Id,
            Title = domain.Title,
            Description = domain.Description,
            Status = domain.Status,
            CreatedAt = domain.CreatedAt,
            UpdatedAt = domain.UpdatedAt
        };
    }
}