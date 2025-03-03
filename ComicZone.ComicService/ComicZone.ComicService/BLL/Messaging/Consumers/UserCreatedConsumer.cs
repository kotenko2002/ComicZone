using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using ComicZone.ComicService.BLL.Messaging.Events;

namespace ComicZone.ComicService.BLL.Messaging.Consumers
{
    public class UserCreatedConsumer : BackgroundService
    {
        private readonly ConnectionFactory _factory;
        private IConnection _connection;
        private IChannel _channel;

        private readonly RabbitMqConfig _config;
        private readonly ILogger<UserCreatedConsumer> _logger;
        private const string QueueName = "user-created-queue";

        public UserCreatedConsumer(IOptions<RabbitMqConfig> options, ILogger<UserCreatedConsumer> logger)
        {
            _config = options.Value;
            _logger = logger;

            _factory = new ConnectionFactory
            {
                HostName = _config.HostName,
                UserName = _config.UserName,
                Password = _config.Password,
                VirtualHost = _config.VirtualHost,
                Port = _config.Port
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _connection = await _factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(exchange: _config.ExchangeName, type: ExchangeType.Topic, durable: true, autoDelete: false, arguments: null);
            await _channel.QueueDeclareAsync(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            await _channel.QueueBindAsync(queue: QueueName, exchange: _config.ExchangeName, routingKey: "user.created");

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var userCreatedEvent = JsonSerializer.Deserialize<UserCreatedEvent>(message);
                    _logger.LogInformation("Received UserCreatedEvent for user {UserId}", userCreatedEvent.Id);

                    // Викликаємо бізнес-логіку обробки події, наприклад:
                    // await ProcessUserCreatedAsync(userCreatedEvent);

                    // крч тут конекшн вже закритий, тому і падає
                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while consuming \"user.created\" event.");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }

                await Task.Yield();
            };

            await _channel.BasicConsumeAsync(queue: QueueName, autoAck: false, consumer: consumer);
        }
    }
}
