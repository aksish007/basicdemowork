using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces.Repositories;
using TaskManagement.Infrastructure.Context;

namespace TaskManagement.Infrastructure.Repositories
{
    public class ActivityRepository : Repository<Activity>, IActivityRepository
    {
        private readonly TaskManagementDbContext dbContext;
        public ActivityRepository(TaskManagementDbContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<IEnumerable<Activity>> GetActivitiesByTaskIdAsync(int id)
        {
            return await _context.Activities.Where(x => x.TaskId == id).ToListAsync();
        }
    }
}
