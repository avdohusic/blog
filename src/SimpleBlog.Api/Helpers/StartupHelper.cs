using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleBlog.Api.Helpers;

public static class StartupHelper
{
    public static IServiceCollection AddAuthenticationDefault(this IServiceCollection services)
    {
        services
            .AddAuthentication("BasicAuthentication")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

        return services;
    }

    public static IServiceCollection AddControllersDefault(this IServiceCollection services)
    {
        services.AddControllers(config =>
        {
            config.ReturnHttpNotAcceptable = true;
            config.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
            config.OutputFormatters.Add(new XmlSerializerOutputFormatter());

            // remove text/json as it isn't the approved media type
            // for working with JSON at API level
            var jsonOutputFormatter = config.OutputFormatters.OfType<SystemTextJsonOutputFormatter>().FirstOrDefault();
            if (jsonOutputFormatter is not null && jsonOutputFormatter.SupportedMediaTypes.Contains("text/json"))
            {
                jsonOutputFormatter.SupportedMediaTypes.Remove("text/json");
            }
        }).AddJsonOptions(config =>
        {
            config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            config.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        return services;
    }

    public static IServiceCollection AddSwaggerGenDefault(this IServiceCollection services)
    {
        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("simpleblog", new OpenApiInfo
            {
                Title = "SimpleBlog Api",
                Version = "v1",
                Description = "The official SimpleBlog Api documentation."
            });
            config.AddSecurityDefinition("basic", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                In = ParameterLocation.Header,
                Description = "Basic Authorization header using the Bearer scheme."
            });
            config.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "basic"
                        }
                    },
                    new string[] {}
                }
            });
            config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Api.xml"));
        });

        return services;
    }
}