using NotificationService.Hubs;
using NotificationService.MappingProfiles;
using Microsoft.OpenApi.Models;
using NotificationService.Interfaces;
using NotificationService.Repositories;
using NotificationService.Data;
using Microsoft.EntityFrameworkCore;
using NotificationService.EventProcessing;
using NotificationService.AsyncDataService;
using NotificationService.GrpcClients;
using Grpc.Net.Client;
using NotificationService.Grpc;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
});

builder.Services.AddDbContext<NotificationDbContext>(options =>
{
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 25));
    var connection = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connection, serverVersion);
});

// Configure gRPC client
var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
};
var channel = GrpcChannel.ForAddress("http://localhost:5173", new GrpcChannelOptions { HttpHandler = handler });
var client = new UserService.UserServiceClient(channel);

builder.Services.AddSingleton(client);
builder.Services.AddScoped<GrpcUserClientService>();

// Add services to the container.
builder.Services.AddAutoMapper(typeof(NotificationMappingProfile).Assembly);
builder.Services.AddControllers();
builder.Services.AddScoped<INotificationService, NotificationService.Services.NotificationService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddHostedService<MessageBusSubscriber>();

// Add SignalR service to the container
builder.Services.AddSignalR();

// Add Swagger service to the container
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NotificationService API", Version = "v1" });
});

// Register your dependencies here
// Example: builder.Services.AddTransient<IMyService, MyService>();

var app = builder.Build();

// Middleware configuration
app.UseRouting();

// Enable CORS
app.UseCors("AllowLocalhost3000");

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NotificationService API V1");
    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});

// Map SignalR hubs and controllers
app.MapHub<NotificationHub>("/hubs/notification");
app.MapControllers();

app.Run();
