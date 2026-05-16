using Assignment_Api;
using Assignment_Application.Implementations;
using Assignment_Application.Interfaces;
using Assignment_Application.Interfaces.Repositories;
using Assignment_Application.Interfaces.Services;
using Assignment_Infrastructure.DataAccess;
using Assignment_Infrastructure.DataAccess.Fakes;
using Assignment_Infrastructure.Exercise;
using Assignment_Infrastructure.Pdf;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AssignmentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Fake Repositories
builder.Services.AddSingleton<IAssignmentRepository, FakeAssignmentRepository>();
builder.Services.AddSingleton<IAssignmentSetRepository, FakeAssignmentSetRepository>();

// Real Repositories
//builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();
builder.Services.AddScoped<IAssignmentPdfService, AssignmentPdfGenerator>();

builder.Services.AddHttpClient<IExerciseProvider, HttpExerciseProvider>(client =>
{
    var baseUrl = builder.Configuration["ExerciseApi:BaseUrl"]
        ?? throw new InvalidOperationException("ExerciseApi:BaseUrl is not configured.");
    client.BaseAddress = new Uri(baseUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
