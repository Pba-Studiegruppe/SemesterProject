using Assign_Frontend.Components;
using Assign_Frontend.Services;

var builder = WebApplication.CreateBuilder(args);

// Razor components + interactive server rendering.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// API clients. Base URL comes from appsettings -> "ExerciseApi:BaseUrl".
var exerciseApiBaseUrl = builder.Configuration["ExerciseApi:BaseUrl"]
    ?? throw new InvalidOperationException(
        "ExerciseApi:BaseUrl is not configured. Add it to appsettings.json.");

builder.Services.AddHttpClient<ExerciseApiClient>(client =>
{
    client.BaseAddress = new Uri(exerciseApiBaseUrl);
});

builder.Services.AddHttpClient<KeywordApiClient>(client =>
{
    client.BaseAddress = new Uri(exerciseApiBaseUrl);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();
