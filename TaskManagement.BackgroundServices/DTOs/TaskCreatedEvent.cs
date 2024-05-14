namespace TaskManagement.BackgroundServices.DTOs
{
    public class TaskCreatedEvent
    {
        public Domain.Entities.Task Task { get; set; }

        public TaskCreatedEvent(Domain.Entities.Task task)
        {
            Task = task;
        }
    }
}
