using FluentValidation.AspNetCore;
using Unirota.API.Configurations;
using Unirota.Application;
using Unirota.Infrastructure;
using Unirota.Migrations;

var builder = WebApplication.CreateBuilder(args);
builder.Host.AddConfigurations();
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddMigrations(builder.Configuration);

builder.Services.AddSwaggerGen();
var app = builder.Build();

//migration runner
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    services.AddMigrationRunner();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
