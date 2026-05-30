using System.Text;
using DayFlowAPI.Data;
using DayFlowAPI.Repositories;
using DayFlowAPI.Repositories.Interfaces;
using DayFlowAPI.Services;
using DayFlowAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace DayFlowAPI.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServiceExtensions(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "DayFlow API", Version = "v1" });

            c.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description =
                        "Insira apenas o token JWT. O Swagger adiciona o prefixo Bearer automaticamente.",
                }
            );

            c.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        Array.Empty<string>()
                    },
                }
            );

            // c.OperationFilter<AuthorizeCheckOperationFilter>();
        });

        // PostgreSQL + Entity Framework Core
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );

        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowAngular",
                policy =>
                {
                    policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
                }
            );
        });

        // JWT Authentication
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
                    ),
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var authHeader = context.Request.Headers.Authorization.ToString();
                        Console.WriteLine(
                            $"[JWT DEBUG] Authorization header received: {(string.IsNullOrEmpty(authHeader) ? "MISSING" : authHeader[..Math.Min(80, authHeader.Length)] + "...")}"
                        );

                        if (string.IsNullOrEmpty(authHeader))
                        {
                            var token = context.Request.Query["access_token"].FirstOrDefault();
                            if (!string.IsNullOrEmpty(token))
                            {
                                context.Token = token;
                                Console.WriteLine("[JWT DEBUG] Token extracted from query string.");
                            }
                        }

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("[JWT DEBUG] Token validated successfully.");
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine(
                            $"[JWT DEBUG] Authentication FAILED: {context.Exception.GetType().Name} - {context.Exception.Message}"
                        );
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        Console.WriteLine(
                            $"[JWT DEBUG] Challenge issued. Error: {context.Error}, Description: {context.ErrorDescription}"
                        );
                        return Task.CompletedTask;
                    },
                };
            });

        services.AddAuthorization();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IToDoService, ToDoService>();
        services.AddScoped<IToDoRepository, ToDoRepository>();

        return services;
    }
}
