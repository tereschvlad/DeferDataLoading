using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DelayedDataLoading;

internal class ReaderService : IReaderService
{
    private readonly ILogger<ReaderService> _logger;
    private readonly IDbReaderService _dbReaderService;
    public ReaderService(ILogger<ReaderService> logger,
        IDbReaderService dbReaderService)
    {
        _logger = logger;
        _dbReaderService = dbReaderService;
    }

    public async Task ReadDataAsync()
    {
        try
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, Password = "test", UserName = "test" };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.BasicQosAsync(0, 1, false);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = System.Text.Encoding.UTF8.GetString(body);

                    var requestData = JsonSerializer.Deserialize<RequestDataModel>(message);

                    var data = await _dbReaderService.ReadDataAsync(requestData.Request, requestData.Parameters);

                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                };

                await channel.BasicConsumeAsync(queue: "postgree_queue", autoAck: false, consumer: consumer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error of reading data");
        }
    }
}
