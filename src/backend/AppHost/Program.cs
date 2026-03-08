using AppHost;
using Signup_Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer(); // Needed for minimal APIs
builder.Services.AddSwaggerGen();
builder.Services.AddSignupInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();             // Generate Swagger JSON
    app.UseSwaggerUI();           // Serve Swagger UI
}
app.MapRoutes();
app.Run();
