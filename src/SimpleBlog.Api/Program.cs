using Microsoft.EntityFrameworkCore;
using SimpleBlog.Api.Helpers;
using SimpleBlog.Infrastructure.Data;
using SimpleBlog.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCorsConfiguration(builder.Configuration)
    .AddControllersDefault()
    .AddEndpointsApiExplorer()
    .AddSwaggerGenDefault()
    .AddAuthenticationDefault(builder.Configuration)
    .AddApplicationServices()
    .AddInfrastructureConfig(builder.Configuration);


var app = builder.Build();

// Ensure that database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SimpleBlogDbContext>();
    await context.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(config => { config.RouteTemplate = "api-docs/{documentName}/openapi.json"; });
    app.UseSwaggerUI(config =>
    {
        config.DocumentTitle = "SimpleBlog Api Documentation";
        config.RoutePrefix = "api-docs";
        config.SwaggerEndpoint("simpleblog/openapi.json", "SimpleBlog Api");
    });
}

//app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseCors(policyName: "DefaultPolicy");

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();