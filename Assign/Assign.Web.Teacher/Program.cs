using Assign.Web.Teacher.Components;
using Assign.Web.Teacher.Services.ApiClients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<Assign_Frontend.Services.ExerciseApiClient>(c =>
{
    c.BaseAddress = new Uri(
        builder.Configuration["ExerciseApi:BaseUrl"]
        ?? throw new InvalidOperationException("ExerciseApi:BaseUrl is not configured."));
});
builder.Services.AddHttpClient<Assign_Frontend.Services.KeywordApiClient>(c =>
{
    c.BaseAddress = new Uri(
        builder.Configuration["ExerciseApi:BaseUrl"]
        ?? throw new InvalidOperationException("ExerciseApi:BaseUrl is not configured."));
});
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(o =>
{
    o.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});
builder.Services.AddHttpClient<AssignmentApiClient>(c =>
{
    c.BaseAddress = new Uri(
        builder.Configuration["AssignmentApi:BaseUrl"] 
        ?? throw new InvalidOperationException("ExerciseApi:BaseUrl is not configured.")); 
});
builder.Services.AddHttpClient<AssignmentSetApiClient>(c =>
{
    c.BaseAddress = new Uri(
        builder.Configuration["AssignmentApi:BaseUrl"]
        ?? throw new InvalidOperationException("AssignmentApi:BaseUrl is not configured."));
});
builder.Services.AddHttpClient<SubmittedAssignmentApiClient>(c =>
{
    c.BaseAddress = new Uri(
        builder.Configuration["AssignmentApi:BaseUrl"]
        ?? throw new InvalidOperationException("ExerciseApi:BaseUrl is not configured."));
});
builder.Services.AddHttpClient<ErrorTypeApiClient>(c =>
{
    c.BaseAddress = new Uri(
        builder.Configuration["AssignmentApi:BaseUrl"]
        ?? throw new InvalidOperationException("AssignmentApi:BaseUrl is not configured."));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
