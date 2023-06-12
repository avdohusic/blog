using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleBlog.Api.Helpers;

public static class StartupHelper
{
    public static IServiceCollection AddAuthenticationDefault(this IServiceCollection services, IConfiguration configuration)
    {
        var validIssuer = configuration.GetSection(key: "Jwt:ValidIssuer").Get<string>();
        var validAudience = configuration.GetSection(key: "Jwt:ValidAudience").Get<string>();
        var jwtSecretKey = configuration.GetSection(key: "Jwt:SecretKey").Get<string>();

        if (string.IsNullOrWhiteSpace(value: validIssuer) || string.IsNullOrWhiteSpace(value: validAudience) || string.IsNullOrWhiteSpace(jwtSecretKey))
        {
            throw new Exception(message: "The configuration value 'Jwt:ValidIssuer', 'Jwt:ValidAudience' and 'Jwt:SecretKey' cannot be null or empty.");
        }

        services.AddAuthentication(config =>
        {
            config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(config =>
        {
            config.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = false,
                ValidIssuer = validIssuer,
                ValidAudience = validAudience,
                IssuerSigningKey = JwtSecurityKey.Create(jwtSecretKey)
            };
        });

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
            config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                In = ParameterLocation.Header,
                BearerFormat = "JWT",
                Description = "Authorization header using the Bearer scheme."
            });
            config.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
               {
                 new OpenApiSecurityScheme
                 {
                   Reference = new OpenApiReference
                   {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                   }
                  },
                  new string[] {}
                }
              });
            config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Api.xml"));
        });

        return services;
    }

    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var origins = configuration.GetSection(key: "Cors:Origins").Get<string>();
        if (string.IsNullOrWhiteSpace(value: origins))
        {
            throw new Exception(message: "The configuration value 'Cors:Origins' cannot be null or empty.");
        }

        var urls = origins.Split(separator: ";");

        services
            .AddCors(setupAction: o => o.AddPolicy(name: "DefaultPolicy", configurePolicy: builder =>
            {
                builder
                    .WithOrigins(origins: urls)
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

        return services;
    }
}