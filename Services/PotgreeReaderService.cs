using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Dapper;
using Npgsql;

namespace DelayedDataLoading;

internal class PotgreeReaderService : IDbReaderService
{
    private readonly ILogger<PotgreeReaderService> _logger;
    private readonly ConnectionDataOption _connectionDataOption;
    public PotgreeReaderService(ILogger<PotgreeReaderService> logger, IOptions<ConnectionDataOption> connectionDataOption)
    {
        _logger = logger;
        _connectionDataOption = connectionDataOption.Value;
    }


    public async Task ReadDataAsync(string command, string parameters)
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionDataOption.DbConnection);

            var t1 = await connection.QueryAsync(command);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DbRequest error");
        }
    }
}
