using AutoMapper;
using FluentAssertions;
using Moq;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Mappings;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Tests.Services;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _mockRepository;
    private readonly IMapper _mapper;
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        _mockRepository = new Mock<ITaskRepository>();
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = config.CreateMapper();
        
        _service = new TaskService(_mockRepository.Object, _mapper);
    }

    [Fact]
    public async Task GetAllTasksAsync_ShouldReturnAllTasks()
    {
        // Arrange
        var tasks = new List<TaskItem>
        {
            TaskItem.Create("Task 1", "Description 1", StatusOfTask.Todo),
            TaskItem.Create("Task 2", "Description 2", StatusOfTask.InProgress)
        };
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

        // Act
        var result = await _service.GetAllTasksAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.Title == "Task 1");
    }

    [Fact]
    public async Task GetTasksByStatusAsync_ShouldReturnFilteredTasks()
    {
        // Arrange
        var tasks = new List<TaskItem>
        {
            TaskItem.Create("Task 1", "Description", StatusOfTask.Todo)
        };
        _mockRepository.Setup(r => r.GetByStatusAsync(StatusOfTask.Todo)).ReturnsAsync(tasks);

        // Act
        var result = await _service.GetTasksByStatusAsync(StatusOfTask.Todo);

        // Assert
        result.Should().HaveCount(1);
        result.First().Status.Should().Be("Todo");
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldCreateAndReturnTask()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "New Task",
            Description = "Description"
        };

        var createdTask = TaskItem.Create("New Task", "Description", StatusOfTask.Todo);

        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<TaskItem>())).ReturnsAsync(createdTask);

        // Act
        var result = await _service.CreateTaskAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("New Task");
        result.Status.Should().Be("Todo");
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldThrowException_WhenTaskNotFound()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((TaskItem?)null);

        var request = new UpdateTaskRequest { Title = "Updated" };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateTaskAsync(999, request));
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldThrowException_WhenTitleIsEmpty()
    {
        // Arrange
        var request = new CreateTaskRequest
        {
            Title = "",
            Description = "Description"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateTaskAsync(request));
    }
}