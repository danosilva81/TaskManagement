using FluentAssertions;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Tests.Domain;

public class TaskItemTests
{
    [Fact]
    public void Create_ShouldCreateTaskWithValidData()
    {
        // Act
        var task = TaskItem.Create("Test Task", "Description", StatusOfTask.Todo);

        // Assert
        task.Should().NotBeNull();
        task.Title.Should().Be("Test Task");
        task.Description.Should().Be("Description");
        task.Status.Should().Be(StatusOfTask.Todo);
        task.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Create_ShouldThrowException_WhenTitleIsEmpty()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => TaskItem.Create("", "Description"));
    }

    [Fact]
    public void Create_ShouldThrowException_WhenTitleIsTooLong()
    {
        // Arrange
        var longTitle = new string('A', 251);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => TaskItem.Create(longTitle));
    }

    [Fact]
    public void UpdateDetails_ShouldUpdateTitle()
    {
        // Arrange
        var task = TaskItem.Create("Original Title");

        // Act
        task.UpdateDetails(title: "Updated Title");

        // Assert
        task.Title.Should().Be("Updated Title");
        task.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void UpdateDetails_ShouldUpdateStatus()
    {
        // Arrange
        var task = TaskItem.Create("Task", status: StatusOfTask.Todo);

        // Act
        task.UpdateDetails(status: StatusOfTask.Done);

        // Assert
        task.Status.Should().Be(StatusOfTask.Done);
    }

    [Fact]
    public void UpdateDetails_ShouldTrimWhitespace()
    {
        // Arrange
        var task = TaskItem.Create("Task");

        // Act
        task.UpdateDetails(title: "  Updated Title  ");

        // Assert
        task.Title.Should().Be("Updated Title");
    }
}