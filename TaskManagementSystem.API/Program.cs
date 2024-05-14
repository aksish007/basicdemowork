using Microsoft.EntityFrameworkCore;
using TaskManagement.Adapters.Email;
using TaskManagement.Application.Services;
using TaskManagement.BackgroundServices.EventHandlers;
using TaskManagement.BackgroundServices.TaskBGSVC;
using TaskManagement.Common.SharedInterfaces;
using TaskManagement.Domain.Interfaces.Repositories;
using TaskManagement.Domain.Interfaces.Services;
using TaskManagement.Infrastructure.Context;
using TaskManagement.Infrastructure.Repositories;
using TaskManagementSystem.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging();

// Database Registrations
builder.Services.AddDbContext<TaskManagementDbContext>(options =>
        options.UseSqlite("Data Source=TaskManagement.db"));

builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod())
);

builder.Services.AddScoped<TaskManagementDbContext, TaskManagementDbContext>();

// Repository Registrations
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();

// Service Registrations
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IActivityService, ActivityService>();

builder.Services.AddScoped<IEmailService, SendGridEmailService>();
builder.Services.AddScoped<TaskCreatedEventHandler>();

builder.Services.AddSingleton<TaskBackgroundService>();
//builder.Services.AddHostedService(provider => provider.GetRequiredService<TaskBackgroundService>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
