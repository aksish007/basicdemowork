using Microsoft.AspNetCore.Mvc;
using TaskManagement.Domain.Interfaces.Services;

namespace TaskManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IActivityService _activityService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService taskService, ILogger<TasksController> logger, IActivityService activityService)
        {
            _taskService = taskService;
            _logger = logger;
            _activityService = activityService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskManagement.Domain.Entities.Task>> GetTask(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                if (task == null) return NotFound();
                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return StatusCode(500, "An error occurred while retrieving the task.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskManagement.Domain.Entities.Task>>> GetAllTasks()
        {
            try
            {
                var tasks = await _taskService.GetAllTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return StatusCode(500, "An error occurred while retrieving tasks.");
            }
        }

        [HttpPost("CreateTask")]
        public async Task<ActionResult<TaskManagement.Domain.Entities.Task>> CreateTask([FromBody] TaskManagement.Domain.Entities.Task task)
        {
            try
            {
                await _taskService.AddTaskAsync(task);
                return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return StatusCode(500, "An error occurred while creating the task.");
            }
        }

        [HttpPost("CreateTasks")]
        public async Task<ActionResult<IEnumerable<TaskManagement.Domain.Entities.Task>>> CreateTasks([FromBody] List<TaskManagement.Domain.Entities.Task> tasks)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _taskService.AddTasksAsync(tasks);
                return Ok(tasks);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Internal server error while creating tasks.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskManagement.Domain.Entities.Task task)
        {
            if (id != task.Id)
                return BadRequest("Task ID mismatch");

            try
            {
                await _taskService.UpdateTaskAsync(task);
                if (task.Activities.Count > 0)
                {
                    foreach (var activity in task.Activities)
                        await _activityService.UpdateActivityAsync(activity);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return StatusCode(500, "An error occurred while updating the task.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                await _taskService.DeleteTaskAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return StatusCode(500, "An error occurred while deleting the task.");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<TaskManagement.Domain.Entities.Task>>> SearchTasks([FromQuery] string taskName, [FromQuery] List<string> tags, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] List<int> statuses)
        {
            try
            {
                var tasks = await _taskService.SearchTasksAsync(taskName, tags, startDate, endDate, statuses);
                return Ok(tasks);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Internal server error while searching tasks.");
            }
        }
    }
}
