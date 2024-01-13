using FonTech.DAL.DependencyInjection;
using FonTech.Application.DependencyInjection;
using Serilog;
using FonTech.DAL;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddSingleton(new ServiceCollection());
builder.Services.AddControllers();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddApplication();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();


