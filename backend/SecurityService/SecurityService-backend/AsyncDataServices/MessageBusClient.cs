using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System;
using SecurityService.DTOs;

namespace SecurityService.AsyncDataServices
{
  public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration _configuration;
    private IConnection _connection;
    private IModel _channel;

    public MessageBusClient(IConfiguration configuration)
    {
        _configuration = configuration;
        InitializeRabbitMQ();
    }

    private void InitializeRabbitMQ()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMq:Host"],
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        Console.WriteLine("--> Connected to MessageBus");
    }

    public void PublishNewUser(UserRegistrationDto userDto)
    {
        var message = JsonSerializer.Serialize(userDto); 

        if (_connection.IsOpen)
        {
            Console.WriteLine($"--> RabbitMQ Connection Opened. Sending message: {message}");
            SendMessage(message);
        }
        else
        {
            Console.WriteLine("RabbitMQ connection is closed.");
        }
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        

        _channel.BasicPublish(exchange: "trigger",
                              routingKey: "",
                              basicProperties: null,
                              body: body);
    }

    public void Dispose()
    {
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }

    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine("RabbitMQ connection shutdown.");
    }
}

}