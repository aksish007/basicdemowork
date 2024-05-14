using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces.Services
{
    public interface IActivityService
    {
        System.Threading.Tasks.Task<Activity> GetActivityByIdAsync(int id);
        System.Threading.Tasks.Task<IEnumerable<Activity>> GetAllActivitiesAsync();
        System.Threading.Tasks.Task AddActivityAsync(Activity activity);
        System.Threading.Tasks.Task AddActivitiesAsync(int taskId, IEnumerable<Activity> activities);
        System.Threading.Tasks.Task UpdateActivityAsync(Activity activity);
        System.Threading.Tasks.Task DeleteActivityAsync(int id);
        Task<IEnumerable<Activity>> GetActivitiesByTaskIdAsync(int id);
    }
}
