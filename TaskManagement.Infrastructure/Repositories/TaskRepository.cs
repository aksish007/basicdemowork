using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Interfaces.Repositories;
using TaskManagement.Infrastructure.Context;

namespace TaskManagement.Infrastructure.Repositories
{
    public class TaskRepository : Repository<Domain.Entities.Task>, ITaskRepository
    {
        public TaskRepository(TaskManagementDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Domain.Entities.Task>> SearchTasksAsync(string taskName, List<string> tags, DateTime? dueDateFrom, DateTime? dueDateTo, List<int> statuses)
        {
            var query = _context.Tasks.AsQueryable();

            if (!string.IsNullOrEmpty(taskName))
            {
                query = query.Where(t => t.TaskName.Contains(taskName, StringComparison.OrdinalIgnoreCase));
            }

            if (tags != null && tags.Count > 0)
            {
                query = query.Where(t => t.Tags.Any(tag => tags.Contains(tag)));
            }

            if (dueDateFrom.HasValue)
            {
                query = query.Where(t => t.DueDate >= dueDateFrom.Value);
            }

            if (dueDateTo.HasValue)
            {
                query = query.Where(t => t.DueDate <= dueDateTo.Value);
            }

            if (statuses != null && statuses.Count > 0)
            {
                query = query.Where(t => statuses.Contains(t.Status));
            }

            return await query.ToListAsync();
        }
    }
}
