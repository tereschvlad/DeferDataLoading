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


    public async Task ReadDataAsync(string command, Dictionary<string, string> dictParams)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionDataOption.DbConnection))
            {
                var t2 = new Dictionary<string, object>()
                {
                    { "Id", 1}
                };

                var parameters = new DynamicParameters(t2);

                var t1 = await connection.QueryAsync(command, parameters);

                
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DbRequest error");
        }
    }
}
