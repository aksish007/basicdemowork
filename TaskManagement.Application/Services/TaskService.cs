using TaskManagement.BackgroundServices.DTOs;
using TaskManagement.BackgroundServices.TaskBGSVC;
using TaskManagement.Domain.Interfaces.Repositories;
using TaskManagement.Domain.Interfaces.Services;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly TaskBackgroundService _taskBackgroundService;

        public TaskService(ITaskRepository taskRepository, TaskBackgroundService taskBackgroundService)
        {
            _taskRepository = taskRepository;
            _taskBackgroundService = taskBackgroundService;
        }

        public async Task<Domain.Entities.Task> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAllAsync();
        }

        public async System.Threading.Tasks.Task AddTaskAsync(Domain.Entities.Task task)
        {
            await _taskRepository.AddAsync(task);
            if (task.Id == 0)
            {
                _taskBackgroundService.EnqueueTask(new TaskCreatedEvent(task));
            }
        }

        public async System.Threading.Tasks.Task AddTasksAsync(IEnumerable<Domain.Entities.Task> tasks)
        {
            await _taskRepository.AddRangeAsync(tasks);
            foreach (var task in tasks)
            {
                if (task.Id == 0)
                {
                    _taskBackgroundService.EnqueueTask(new TaskCreatedEvent(task));
                }
            }
        }

        public async System.Threading.Tasks.Task UpdateTaskAsync(Domain.Entities.Task task)
        {
            await _taskRepository.UpdateAsync(task);

            if (task.Id == 0)
            {
                _taskBackgroundService.EnqueueTask(new TaskCreatedEvent(task));
            }
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task != null)
            {
                await _taskRepository.DeleteAsync(task);
            }
        }

        public async Task<IEnumerable<Domain.Entities.Task>> SearchTasksAsync(string taskName, List<string> tags, DateTime? dueDateFrom, DateTime? dueDateTo, List<int> statuses)
        {
            return await _taskRepository.SearchTasksAsync(taskName, tags, dueDateFrom, dueDateTo, statuses);
        }
    }
}
