using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.Data.Models;

namespace TaskManagement.Infrastructure.Data;

public static class DbSeeder
{
    public static void SeedData(AppDbContext context)
    {
        // Check if database is already seeded
        if (context.Tasks.Any())
        {
            return;
        }

        var tasks = new[]
        {
            new TaskEntity
            {
                Title = "Complete project documentation",
                Description = "Write comprehensive README and API documentation",
                Status = StatusOfTask.InProgress,
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new TaskEntity
            {
                Title = "Review pull requests",
                Description = "Review and merge pending PRs from team members",
                Status = StatusOfTask.Todo,
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new TaskEntity
            {
                Title = "Fix authentication bug",
                Description = "Resolve the JWT token expiration issue",
                Status = StatusOfTask.Done,
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                UpdatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new TaskEntity
            {
                Title = "Prepare demo presentation",
                Description = "Create slides for stakeholder meeting",
                Status = StatusOfTask.InProgress,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new TaskEntity
            {
                Title = "Update dependencies",
                Description = "Update all NuGet packages to latest stable versions",
                Status = StatusOfTask.Todo,
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new TaskEntity
            {
                Title = "Write unit tests",
                Description = "Increase code coverage to 80%",
                Status = StatusOfTask.Todo,
                CreatedAt = DateTime.UtcNow
            },
            new TaskEntity
            {
                Title = "Deploy to staging",
                Description = "Deploy latest build to staging environment",
                Status = StatusOfTask.Done,
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = DateTime.UtcNow.AddDays(-8)
            },
            new TaskEntity
            {
                Title = "Design new feature mockups",
                Description = "Create UI mockups for the new dashboard feature",
                Status = StatusOfTask.InProgress,
                CreatedAt = DateTime.UtcNow.AddDays(-4)
            }
        };

        context.Tasks.AddRange(tasks);
        context.SaveChanges();
    }
}