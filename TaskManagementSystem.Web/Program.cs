using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using TaskManagementSystem.Web;
using TaskManagementSystem.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7175") });
builder.Services.AddMudServices();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<ActivityService>();
await builder.Build().RunAsync();
