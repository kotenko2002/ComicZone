using ComicZone.UserService.BLL.Messaging.Events;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ComicZone.UserService.BLL.Messaging.Publishers
{
    public class UserEventPublisher : IUserEventPublisher
    {
        private readonly RabbitMqConfig _config;
        private readonly ILogger<UserEventPublisher> _logger;

        public UserEventPublisher(
            IOptions<RabbitMqConfig> rabbitOptions,
            ILogger<UserEventPublisher> logger)
        {
            _config = rabbitOptions.Value;
            _logger = logger;
        }

        public async Task PublishUserCreatedAsync(UserCreatedEvent userEvent)
        {
            try
            {
                using var connection = await CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.ExchangeDeclareAsync(
                    exchange: _config.ExchangeName,
                    type: ExchangeType.Topic,
                    durable: true,
                    autoDelete: false,
                    arguments: null
                );

                var message = JsonSerializer.Serialize(userEvent);
                var body = Encoding.UTF8.GetBytes(message);

                await channel.BasicPublishAsync(
                    exchange: _config.ExchangeName,
                    routingKey: "user.created",
                    body: body
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while publishing \"user.created\" event.");
            }
        }

        public async Task PublishUserUpdatedAsync(UserUpdatedEvent userEvent)
        {
            try
            {
                using var connection = await CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.ExchangeDeclareAsync(
                    exchange: _config.ExchangeName,
                    type: ExchangeType.Topic,
                    durable: true,
                    autoDelete: false,
                    arguments: null
                );

                var message = JsonSerializer.Serialize(userEvent);
                var body = Encoding.UTF8.GetBytes(message);

                await channel.BasicPublishAsync(
                    exchange: _config.ExchangeName,
                    routingKey: "user.updated",
                    body: body
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while publishing \"user.updated\" event.");
            }
        }

        private async Task<IConnection> CreateConnectionAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = _config.HostName,
                UserName = _config.UserName,
                Password = _config.Password,
                VirtualHost = _config.VirtualHost,
                Port = _config.Port
            };

            return await factory.CreateConnectionAsync();
        }
    }
}
