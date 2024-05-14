using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Context
{
    public class TaskManagementDbContext : DbContext
    {
        public DbSet<Domain.Entities.Task> Tasks { get; set; }
        public DbSet<Activity> Activities { get; set; }

        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options)
        : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Entities.Task>().Property(x => x.Tags).HasConversion(
                v => string.Join('|', v),
                v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList()
            );

            // Seed data for Tasks
            modelBuilder.Entity<Domain.Entities.Task>().HasData(
                new Domain.Entities.Task
                {
                    Id = 1,
                    TaskName = "Buy Grocery",
                    Tags = new List<string> { "household", "weekly stuff" },
                    DueDate = DateTime.UtcNow.AddDays(7),
                    Color = "beige",
                    AssignedTo = "bala",
                    Status = 0
                },
                new Domain.Entities.Task
                {
                    Id = 2,
                    TaskName = "Complete Assignment",
                    Tags = new List<string> { "work", "urgent" },
                    DueDate = DateTime.UtcNow.AddDays(3),
                    Color = "red",
                    AssignedTo = "john",
                    Status = 1
                },
                new Domain.Entities.Task
                {
                    Id = 3,
                    TaskName = "Prepare Presentation",
                    Tags = new List<string> { "work", "important" },
                    DueDate = DateTime.UtcNow.AddDays(10),
                    Color = "blue",
                    AssignedTo = "jane",
                    Status = 2
                }
            );

        }
    }
}
