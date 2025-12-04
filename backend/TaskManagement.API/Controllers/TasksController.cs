using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Enums;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponse>>> GetAllTasks()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        return Ok(tasks);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<TaskResponse>>> GetTasksByStatus(StatusOfTask status)
    {
        var tasks = await _taskService.GetTasksByStatusAsync(status);
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskResponse>> GetTaskById(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
            return NotFound();

        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> CreateTask(CreateTaskRequest request)
    {
        try
        {
            var task = await _taskService.CreateTaskAsync(request);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskResponse>> UpdateTask(int id, UpdateTaskRequest request)
    {
        try
        {
            var task = await _taskService.UpdateTaskAsync(id, request);
            return Ok(task);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTask(int id)
    {
        var result = await _taskService.DeleteTaskAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}