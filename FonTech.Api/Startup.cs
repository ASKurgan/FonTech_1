﻿using Microsoft.OpenApi.Models;
using System.Reflection;

namespace FonTech.Api
{
    public static class Startup
    {
        /// <summary>
        /// Подключение Swagger
        /// </summary>
        /// <param name="services"></param>
      public static void  AddSwagger(this IServiceCollection services)
        {
            services.AddApiVersioning()
                .AddApiExplorer(options =>
                {
                    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options => 
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                  Version = "v1",
                  Title = "FonTech.Api",
                  Description = "This is version 1.0",
                  TermsOfService = new Uri("https://www.youtube.com/watch?v=u7bWzy8IBbk&list=RDu7bWzy8IBbk&start_radio=1"),
                  Contact = new OpenApiContact() 
                  {
                   Name  = "Test contact",
                   Url = new Uri("https://www.youtube.com/watch?v=u7bWzy8IBbk&list=RDu7bWzy8IBbk&start_radio=1"),
                  }
                  });

                options.SwaggerDoc("v2", new OpenApiInfo()
                {
                    Version = "v2",
                    Title = "FonTech.Api",
                    Description = "This is version 2.0",
                    TermsOfService = new Uri("https://www.youtube.com/watch?v=u7bWzy8IBbk&list=RDu7bWzy8IBbk&start_radio=1"),
                    Contact = new OpenApiContact()
                    {
                        Name = "Test contact",
                        Url = new Uri("https://www.youtube.com/watch?v=u7bWzy8IBbk&list=RDu7bWzy8IBbk&start_radio=1"),
                    }
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Введите пожалуйста валидный токен",
                    Name = "Авторизация",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        { 
                             Reference = new OpenApiReference ()
                             {
                               Type = ReferenceType.SecurityScheme,
                               Id = "Bearer"
                             },
                             Name = "Bearer",
                             In = ParameterLocation.Header,
                        },
                       Array.Empty<string>()
                    }
                });

                string? xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
            });
        }
    }
}
