using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using SecurityService.Models;
using AutoMapper;
using SecurityServiceBackend.Configurations;
using Microsoft.OpenApi.Models;
using SecurityServiceBackend.Repositories;
using SecurityServiceBackend.Interfaces;
using SecurityServiceBackend.Services;
using SecurityService.Helpers;
using SecurityServiceBackend.Models;
using System.Security.Cryptography;
using SecurityService.AsyncDataServices;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Configure Services
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure Middleware
ConfigureMiddleware(app);

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    const int maxRequestLimit = 209715200;
    services.Configure<IISServerOptions>(options =>
    {
        options.MaxRequestBodySize = maxRequestLimit;
    });
    services.Configure<KestrelServerOptions>(options =>
    {
        options.Limits.MaxRequestBodySize = maxRequestLimit;
    });
    services.Configure<FormOptions>(options =>
    {
        options.ValueLengthLimit = maxRequestLimit;
        options.MultipartBodyLengthLimit = maxRequestLimit;
        options.MultipartHeadersLengthLimit = maxRequestLimit;
    });
    // Configure MySQL
    services.AddDbContext<ApplicationDbContext>(options =>
    {
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 25));
        var connection = configuration.GetConnectionString("DefaultConnection");
        options.UseMySql(connection, serverVersion);
    });

    // Configure Identity
    services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

    // AutoMapper configuration
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Configure JWT Authentication by public and private keys and the public key path ./public.key
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(RSA.Create()),
                ValidateIssuer = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

    // Register Repositories and Services for Dependency Injection
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IAuthService, AuthService>();
    services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
    services.AddSingleton<IMessageBusClient, MessageBusClient>();

    // Add Authorization
    services.AddAuthorization();

    // Add Controllers
    services.AddControllers();

    // Add Swagger services
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        // Add JWT Authentication to Swagger
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }});
    });

    // Add CORS services
    services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder => builder.WithOrigins("http://localhost:3000")
                              .AllowAnyHeader()
                              .AllowAnyMethod());
    });
}

void ConfigureMiddleware(WebApplication app)
{
    // Middleware configuration
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseHsts();
    }
    app.UseRouting();

    // Enable CORS
    app.UseCors("AllowSpecificOrigin");

    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });

    app.MapControllers();
}