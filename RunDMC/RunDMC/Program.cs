using Microsoft.EntityFrameworkCore;
using RunDMC.Data;
using RunDMC.Mappings;
using RunDMC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<FitnessDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<FitnessProfile>());
builder.Services.AddScoped<IWorkoutService, WorkoutService>();
builder.Services.AddScoped<IStatsService, StatsService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();
