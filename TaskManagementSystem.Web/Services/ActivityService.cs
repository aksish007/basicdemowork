using System.Net.Http.Json;
using TaskManagement.Domain.Entities;

namespace TaskManagementSystem.Web.Services
{
    public class ActivityService
    {
        private readonly HttpClient _httpClient;

        public ActivityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Activity>> GetActivitiesByTaskIdAsync(int taskId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Activity>>($"api/activities/task/{taskId}");
        }

        public async System.Threading.Tasks.Task AddActivityAsync(Activity activity)
        {
            await _httpClient.PostAsJsonAsync("api/activities", activity);
        }

        public async System.Threading.Tasks.Task AddActivitiesAsync(int taskId, IEnumerable<Activity> activities)
        {
            await _httpClient.PostAsJsonAsync($"api/activities/task/{taskId}", activities);
        }
    }
}
