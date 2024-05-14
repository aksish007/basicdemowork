using TaskManagement.BackgroundServices.DTOs;
using TaskManagement.Common.SharedInterfaces;


namespace TaskManagement.BackgroundServices.EventHandlers
{
    public class TaskCreatedEventHandler
    {
        private readonly IEmailService _emailService;

        public TaskCreatedEventHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Handle(TaskCreatedEvent taskCreatedEvent)
        {
            var task = taskCreatedEvent.Task;
            var subject = "New Task Assigned to You";
            var message = $"You have been assigned a new task: {task.TaskName}";

            await _emailService.SendEmailAsync(task.AssignedTo, subject, message);
        }
    }
}
