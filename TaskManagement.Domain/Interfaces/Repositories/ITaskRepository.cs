namespace TaskManagement.Domain.Interfaces.Repositories
{
    public interface ITaskRepository : IRepository<Domain.Entities.Task>
    {
        Task<IEnumerable<Domain.Entities.Task>> SearchTasksAsync(string taskName, List<string> tags, DateTime? dueDateFrom, DateTime? dueDateTo, List<int> statuses);
    }
}
