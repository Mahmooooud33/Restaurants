using Microsoft.OpenApi.Models;
using Restaurants.API.Middlewares;
using Serilog;

namespace Restaurants.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddPresentationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();

        builder.Services.AddControllers();

        #region Swagger
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference{ Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                    },
                    []
                }
            });
        });
        #endregion

        builder.Services.AddEndpointsApiExplorer();

        #region Custom Middlewares
        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddScoped<RequestTimeLoggingMiddleware>();
        #endregion

        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));
    }
}
