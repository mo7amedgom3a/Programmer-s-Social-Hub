using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using PostService.AsyncDataService.Models;

namespace PostService.AsyncDataService
{
    public class MessageBusClient : IMessageBusClient, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost", // Ensure this matches your RabbitMQ server setup
                Port = 5672,
                UserName = "guest",
                Password = "guest",
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "post_events", type: ExchangeType.Fanout);
               

                Console.WriteLine("--> Connected to RabbitMQ and declared exchange");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the message bus: {ex.Message}");
                throw;
            }
        }

public void PublishEvent<T>(T eventMessage, string eventType)
{
    var wrappedMessage = new
    {
        EventType = eventType,
        Data = eventMessage
    };

    var message = JsonSerializer.Serialize(wrappedMessage);

    if (_connection.IsOpen)
    {
        Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
        SendMessage(message);
    }
    else
    {
        Console.WriteLine("--> RabbitMQ Connection is closed, not sending");
    }
}
        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                exchange: "post_events",
                routingKey: "",
                basicProperties: null,
                body: body);

            Console.WriteLine($"--> We have sent a message: {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("--> MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}
