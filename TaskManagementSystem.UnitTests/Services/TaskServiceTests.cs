using Moq;
using TaskManagement.Application.Services;
using TaskManagement.BackgroundServices.DTOs;
using TaskManagement.BackgroundServices.TaskBGSVC;
using TaskManagement.Domain.Interfaces.Repositories;

namespace TaskManagementSystem.UnitTests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly Mock<TaskBackgroundService> _taskBackgroundServiceMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _taskBackgroundServiceMock = new Mock<TaskBackgroundService>(null);
            _taskService = new TaskService(_taskRepositoryMock.Object, _taskBackgroundServiceMock.Object);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ReturnsTask()
        {
            // Arrange
            var taskId = 1;
            var task = new TaskManagement.Domain.Entities.Task { Id = taskId, TaskName = "Test Task" };
            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(task);

            // Act
            var result = await _taskService.GetTaskByIdAsync(taskId);

            // Assert
            Assert.Equal(task, result);
        }

        [Fact]
        public async Task GetAllTasksAsync_ReturnsAllTasks()
        {
            // Arrange
            var tasks = new List<TaskManagement.Domain.Entities.Task> { new TaskManagement.Domain.Entities.Task { Id = 1, TaskName = "Test Task 1" }, new TaskManagement.Domain.Entities.Task { Id = 2, TaskName = "Test Task 2" } };
            _taskRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _taskService.GetAllTasksAsync();

            // Assert
            Assert.Equal(tasks, result);
        }

        [Fact]
        public async Task AddTaskAsync_AddsTaskAndEnqueuesEvent()
        {
            // Arrange
            var task = new TaskManagement.Domain.Entities.Task { TaskName = "Test Task" };

            // Act
            await _taskService.AddTaskAsync(task);

            // Assert
            _taskRepositoryMock.Verify(repo => repo.AddAsync(task), Times.Once);
            _taskBackgroundServiceMock.Verify(bg => bg.EnqueueTask(It.IsAny<TaskCreatedEvent>()), Times.Once);
        }

        [Fact]
        public async Task AddTasksAsync_AddsTasksAndEnqueuesEvents()
        {
            // Arrange
            var tasks = new List<TaskManagement.Domain.Entities.Task>
            {
                new TaskManagement.Domain.Entities.Task { TaskName = "Test Task 1" },
                new TaskManagement.Domain.Entities.Task { TaskName = "Test Task 2" }
            };

            // Act
            await _taskService.AddTasksAsync(tasks);

            // Assert
            _taskRepositoryMock.Verify(repo => repo.AddRangeAsync(tasks), Times.Once);
            _taskBackgroundServiceMock.Verify(bg => bg.EnqueueTask(It.IsAny<TaskCreatedEvent>()), Times.Exactly(tasks.Count));
        }

        [Fact]
        public async Task UpdateTaskAsync_UpdatesTaskAndEnqueuesEvent()
        {
            // Arrange
            var task = new TaskManagement.Domain.Entities.Task { Id = 1, TaskName = "Updated Task" };

            // Act
            await _taskService.UpdateTaskAsync(task);

            // Assert
            _taskRepositoryMock.Verify(repo => repo.UpdateAsync(task), Times.Once);
            _taskBackgroundServiceMock.Verify(bg => bg.EnqueueTask(It.IsAny<TaskCreatedEvent>()), Times.Once);
        }

        [Fact]
        public async Task DeleteTaskAsync_DeletesTask()
        {
            // Arrange
            var taskId = 1;
            var task = new TaskManagement.Domain.Entities.Task { Id = taskId, TaskName = "Test Task" };
            _taskRepositoryMock.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(task);

            // Act
            await _taskService.DeleteTaskAsync(taskId);

            // Assert
            _taskRepositoryMock.Verify(repo => repo.DeleteAsync(task), Times.Once);
        }

        [Fact]
        public async Task SearchTasksAsync_ReturnsFilteredTasks()
        {
            // Arrange
            var taskName = "Test";
            var tags = new List<string> { "Tag1", "Tag2" };
            var dueDateFrom = DateTime.Now;
            var dueDateTo = DateTime.Now.AddDays(1);
            var statuses = new List<int> { 1, 2 };
            var tasks = new List<TaskManagement.Domain.Entities.Task> { new TaskManagement.Domain.Entities.Task { TaskName = "Test Task" } };

            _taskRepositoryMock.Setup(repo => repo.SearchTasksAsync(taskName, tags, dueDateFrom, dueDateTo, statuses)).ReturnsAsync(tasks);

            // Act
            var result = await _taskService.SearchTasksAsync(taskName, tags, dueDateFrom, dueDateTo, statuses);

            // Assert
            Assert.Equal(tasks, result);
        }
    }
}
