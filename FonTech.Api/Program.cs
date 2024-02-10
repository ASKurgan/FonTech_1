using FonTech.DAL.DependencyInjection;
using FonTech.Application.DependencyInjection;
using Serilog;
using FonTech.DAL;
using Microsoft.EntityFrameworkCore;
using FonTech.Api;
using FonTech.Domain.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics;
using FonTech.Api.Middlewares;
var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.DefaultSection));

// builder.Services.AddSingleton(new ServiceCollection());
builder.Services.AddControllers();

builder.Services.AddAuthenticationAndAuthorization(builder);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSwagger();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddApplication();
var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FonTech Swagger v1.0");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "FonTech Swagger v2.0");
       // c.RoutePrefix = string.Empty;
    });
}

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.MapControllers();

app.Run();


