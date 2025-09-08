using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace DelayedDataLoading;

internal class ReaderService : IReaderService
{
    private readonly ILogger<ReaderService> _logger;
    private readonly IDbReaderService _dbReaderService;
    private readonly IMongoDbWriterService _mongoDbService;
    private readonly ConnectionDataOption _connectionDataOption;

    public ReaderService(ILogger<ReaderService> logger,
        IDbReaderService dbReaderService,
        IMongoDbWriterService mongoDbService, 
        IOptions<ConnectionDataOption> connectionDataOption)
    {
        _logger = logger;
        _dbReaderService = dbReaderService;
        _mongoDbService = mongoDbService;
        _connectionDataOption = connectionDataOption.Value;
    }

    public async Task ReadDataAsync()
    {
        try
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, Password = "test", UserName = "test" };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            string queueName = string.Empty;

            queueName = "postgree_queue";
            //queueName = "oracle_queue";
            //queueName = "mysql_queue";
            //queueName = "mssql_queue";

            var message = await channel.BasicGetAsync(queueName, false);
            while (message != null)
            {
                try
                {
                    var body = message.Body.ToArray();
                    var msg = System.Text.Encoding.UTF8.GetString(body);

                    var requestData = JsonSerializer.Deserialize<RequestDataModel>(msg);

                    var data = await _dbReaderService.ReadDataAsync(requestData.Request, requestData.Parameters);

                    var resultRequestData = new ResultRequestDataModel
                    {
                        Request = requestData.Request,
                        Parameters = requestData.Parameters,
                        ResultJson = JsonSerializer.Serialize(data),
                        CreateDate = DateTime.UtcNow,
                        Application = requestData.Application,
                        UserName = requestData.UserName,
                        MongoCollectionName = requestData.MongoCollectionName
                    };

                    await _mongoDbService.WriteDataAsync(resultRequestData);

                    await channel.BasicAckAsync(message.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                    await channel.BasicNackAsync(message.DeliveryTag, false, false);
                }

                message = await channel.BasicGetAsync(queueName, false);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error of reading data");
        }
    }
}
