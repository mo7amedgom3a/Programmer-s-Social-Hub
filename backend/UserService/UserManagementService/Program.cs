using MongoDB.Driver;
using UserManagementService.Repositories;
using UserManagementService.Services;
using UserManagementService.Interfaces;
using UserManagementService.Models;
using Microsoft.OpenApi.Models;
using UserManagementService.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using UserManagementService.EventProcessing;
using ZstdSharp.Unsafe;

var builder = WebApplication.CreateBuilder(args);

// MongoDB connection setup
var mongoDBSettings = new MongoDBSettings(builder.Configuration);
var client = new MongoClient(mongoDBSettings.GetConnectionString());
var database = client.GetDatabase(mongoDBSettings.GetDatabaseName());

// Add MongoDB as a singleton service
builder.Services.AddSingleton(database);
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(mongoDBSettings.GetConnectionString()));
builder.Services.AddSingleton(sp => client.GetDatabase(mongoDBSettings.GetDatabaseName()));

builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
    options.MaxReceiveMessageSize = 16 * 1024 * 1024; // 16 MB
});

// Repositories and services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddHostedService<MessageBusSubscriber>();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Swagger configuration with lifetime for API documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "User Management Service API",
        Version = "v1",
        Description = "API documentation for User Management Service",
        Contact = new OpenApiContact
        {
            Name = "mohamed gomaa",
            Email = "mohamed@gmail.com",
        },
        License = new OpenApiLicense
        {
            Name = "Use under XYZ",
            Url = new Uri("https://license-url.com"),
        }
    });

    // Enable support for JWT bearer tokens (if needed)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Add controllers support
builder.Services.AddControllers();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

string publicKey = File.ReadAllText("public.key");
RSA rsa = RSA.Create();
rsa.ImportFromPem(publicKey.ToCharArray());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            IssuerSigningKey = new RsaSecurityKey(rsa),
            ClockSkew = TimeSpan.Zero // remove delay of token when expire
        };
    });

// Build the app
var app = builder.Build();

// Enable Swagger and Swagger UI in all environments
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Management Service API v1");
    c.DocumentTitle = "User Management Service API Docs";
    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});

app.UseCors("AllowSpecificOrigin");

app.MapGrpcService<GrpcUserService>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
