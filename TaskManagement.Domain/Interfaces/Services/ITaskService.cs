namespace TaskManagement.Domain.Interfaces.Services
{
    public interface ITaskService
    {
        System.Threading.Tasks.Task<Entities.Task> GetTaskByIdAsync(int id);
        System.Threading.Tasks.Task<IEnumerable<Entities.Task>> GetAllTasksAsync();
        System.Threading.Tasks.Task AddTaskAsync(Entities.Task task);
        System.Threading.Tasks.Task AddTasksAsync(IEnumerable<Entities.Task> tasks);
        System.Threading.Tasks.Task UpdateTaskAsync(Entities.Task task);
        System.Threading.Tasks.Task DeleteTaskAsync(int id);
        System.Threading.Tasks.Task<IEnumerable<Entities.Task>> SearchTasksAsync(string taskName, List<string> tags, DateTime? dueDateFrom, DateTime? dueDateTo, List<int> statuses);
    }
}
