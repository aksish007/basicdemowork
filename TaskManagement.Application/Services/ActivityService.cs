using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces.Repositories;
using TaskManagement.Domain.Interfaces.Services;

namespace TaskManagement.Application.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly ITaskRepository _taskRepository;

        public ActivityService(IActivityRepository activityRepository, ITaskRepository taskRepository)
        {
            _activityRepository = activityRepository;
            _taskRepository = taskRepository;
        }

        public async System.Threading.Tasks.Task<Activity> GetActivityByIdAsync(int id)
        {
            return await _activityRepository.GetByIdAsync(id);
        }

        public async System.Threading.Tasks.Task<IEnumerable<Activity>> GetActivitiesByTaskIdAsync(int id)
        {
            return await _activityRepository.GetActivitiesByTaskIdAsync(id);
        }

        public async System.Threading.Tasks.Task<IEnumerable<Activity>> GetAllActivitiesAsync()
        {
            return await _activityRepository.GetAllAsync();
        }

        public async System.Threading.Tasks.Task AddActivityAsync(Activity activity)
        {
            await _activityRepository.AddAsync(activity);
        }

        public async System.Threading.Tasks.Task AddActivitiesAsync(int taskId, IEnumerable<Activity> activities)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
            {
                throw new KeyNotFoundException("Task not found");
            }

            foreach (var activity in activities)
            {
                activity.TaskId = taskId; // Assuming Activity has a TaskId property to link it to the Task
            }

            await _activityRepository.AddRangeAsync(activities);
        }

        public async System.Threading.Tasks.Task UpdateActivityAsync(Activity activity)
        {
            await _activityRepository.UpdateAsync(activity);
        }

        public async System.Threading.Tasks.Task DeleteActivityAsync(int id)
        {
            var activity = await _activityRepository.GetByIdAsync(id);
            if (activity != null)
            {
                await _activityRepository.DeleteAsync(activity);
            }
        }
    }
}
