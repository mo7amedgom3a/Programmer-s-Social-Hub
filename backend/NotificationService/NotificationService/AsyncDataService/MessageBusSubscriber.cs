using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using NotificationService.EventProcessing;

namespace NotificationService.AsyncDataService
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IEventProcessor _eventProcessor;
        private const string QueueName = "PostEventsQueue";

        public MessageBusSubscriber(IServiceProvider serviceProvider)
        {
            // Using service provider to resolve the event processor dependency
            using var scope = serviceProvider.CreateScope();
            _eventProcessor = scope.ServiceProvider.GetRequiredService<IEventProcessor>();

            var factory = new ConnectionFactory() 
            { 
                HostName = "localhost", 
                Port = 5672,
                Password = "guest",
                UserName = "guest",
                DispatchConsumersAsync = true 
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare and bind the queue to the same exchange used by the PostService
            _channel.ExchangeDeclare(exchange: "post_events", type: ExchangeType.Fanout);
            _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: QueueName, exchange: "post_events", routingKey: "");

            Console.WriteLine("--> Listening for events from RabbitMQ on queue: PostEventsQueue");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Message received from queue: {message}");

                await _eventProcessor.ProcessEventAsync(message);
            };

            _channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
            base.Dispose();
        }
    }
}
