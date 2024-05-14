using Microsoft.AspNetCore.Mvc;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces.Services;

namespace TaskManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivitiesController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        // GET: api/Activity/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(int id)
        {
            try
            {
                var activity = await _activityService.GetActivityByIdAsync(id);
                if (activity == null)
                    return NotFound();
                return Ok(activity);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Internal server error while retrieving activity.");
            }
        }

        // GET: api/Activity/task/{id}
        [HttpGet("task/{id}")]
        public async Task<ActionResult<Activity>> GetActivitiesByTaskId(int id)
        {
            try
            {
                var activities = await _activityService.GetActivitiesByTaskIdAsync(id);
                if (activities == null || activities.Count() == 0)
                    return NotFound();
                return Ok(activities);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Internal server error while retrieving activity.");
            }
        }

        // GET: api/Activity
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activity>>> GetAllActivities()
        {
            try
            {
                var activities = await _activityService.GetAllActivitiesAsync();
                return Ok(activities);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Internal server error while retrieving all activities.");
            }
        }

        // POST: api/Activity
        [HttpPost]
        public async Task<ActionResult<Activity>> CreateActivity([FromBody] Activity activity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _activityService.AddActivityAsync(activity);
                return CreatedAtAction(nameof(GetActivity), new { id = activity.Id }, activity);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Internal server error while creating activity.");
            }
        }

        [HttpPost("task/{taskId}")]
        public async Task<ActionResult<IEnumerable<Activity>>> CreateActivities(int taskId, [FromBody] List<Activity> activities)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _activityService.AddActivitiesAsync(taskId, activities);
                return Ok(activities);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Task not found");
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Internal server error while creating activities.");
            }
        }

        // PUT: api/Activity/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActivity(int id, [FromBody] Activity activity)
        {
            if (id != activity.Id)
                return BadRequest("Activity ID mismatch");

            try
            {
                await _activityService.UpdateActivityAsync(activity);
                return NoContent();
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Internal server error while updating activity.");
            }
        }

        // DELETE: api/Activity/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            try
            {
                await _activityService.DeleteActivityAsync(id);
                return NoContent();
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Internal server error while deleting activity.");
            }
        }
    }
}
