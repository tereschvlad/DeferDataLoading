using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Dapper;
using Npgsql;

namespace DeferDataLoading;

internal class PotgreeReaderService : IDbReaderService
{
    private readonly ILogger<PotgreeReaderService> _logger;
    private readonly ConnectionDataOption _connectionDataOption;
    public PotgreeReaderService(ILogger<PotgreeReaderService> logger, IOptions<ConnectionDataOption> connectionDataOption)
    {
        _logger = logger;
        _connectionDataOption = connectionDataOption.Value;
    }


    public async Task<IEnumerable<dynamic>> ReadDataAsync(string command, Dictionary<string, object> dictParams)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionDataOption.DbConnection))
            {
                var parameters = new DynamicParameters(dictParams);

                return await connection.QueryAsync(command, parameters);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DbRequest error");
        }

        return Enumerable.Empty<dynamic>();
    }
}
