using Exercise_Api;
using Exercise_Application.Implementations;
using Exercise_Application.Interfaces.Repositories;
using Exercise_Application.Interfaces.Services;
using Exercise_Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ExerciseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionGateway")));
//TODO: Hardcoded connectionstring, fix for later

builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IKeywordRepository, KeywordRepository>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IKeywordService, KeywordService>();
builder.Services.AddScoped<IExerciseSolutionService, ExerciseSolutionService>();
builder.Services.AddScoped<IExerciseQuestionService, ExerciseQuestionService>();


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
