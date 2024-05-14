using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using TaskManagement.BackgroundServices.DTOs;
using TaskManagement.BackgroundServices.EventHandlers;

namespace TaskManagement.BackgroundServices.TaskBGSVC
{
    public class TaskBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly BlockingCollection<TaskCreatedEvent> _taskQueue = new BlockingCollection<TaskCreatedEvent>();

        public TaskBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void EnqueueTask(TaskCreatedEvent taskEvent)
        {
            _taskQueue.Add(taskEvent);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var taskEvent = _taskQueue.Take(stoppingToken);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var handler = scope.ServiceProvider.GetRequiredService<TaskCreatedEventHandler>();
                    await handler.Handle(taskEvent);
                }
            }
        }
    }
}
