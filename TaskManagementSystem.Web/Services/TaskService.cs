using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;
namespace TaskManagementSystem.Web.Services
{
    public class TaskService
    {
        private readonly HttpClient _httpClient;

        public TaskService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<TaskManagement.Domain.Entities.Task>> GetTasksAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<TaskManagement.Domain.Entities.Task>>("api/tasks");
        }

        public async Task<TaskManagement.Domain.Entities.Task> GetTaskByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<TaskManagement.Domain.Entities.Task>($"api/tasks/{id}");
        }

        public async Task AddTaskAsync(TaskManagement.Domain.Entities.Task task)
        {
            await _httpClient.PostAsJsonAsync("api/tasks", task);
        }

        public async Task AddTasksAsync(IEnumerable<TaskManagement.Domain.Entities.Task> tasks)
        {
            await _httpClient.PostAsJsonAsync("api/tasks", tasks);
        }

        public async Task UpdateTaskAsync(TaskManagement.Domain.Entities.Task task)
        {
            await _httpClient.PutAsJsonAsync($"api/tasks/{task.Id}", task);
        }

        public async Task DeleteTaskAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/tasks/{id}");
        }

        public async Task<IEnumerable<TaskManagement.Domain.Entities.Task>> SearchTasksAsync(string taskName, List<string> tags, DateTime? startDate, DateTime? endDate, List<string> statuses)
        {
            // Build the query parameters for the search
            var query = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(taskName))
                query["taskName"] = taskName;

            if (tags != null && tags.Count > 0)
                query["tags"] = string.Join(",", tags);

            if (startDate.HasValue)
                query["startDate"] = startDate.Value.ToString("yyyy-MM-dd");

            if (endDate.HasValue)
                query["endDate"] = endDate.Value.ToString("yyyy-MM-dd");

            if (statuses != null && statuses.Count > 0)
                query["statuses"] = string.Join(",", statuses);

            var uri = QueryHelpers.AddQueryString("api/tasks/search", query);

            return await _httpClient.GetFromJsonAsync<IEnumerable<TaskManagement.Domain.Entities.Task>>(uri);
        }
    }
}
