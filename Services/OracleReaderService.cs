using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;

namespace DelayedDataLoading.Services
{
    internal class OracleReaderService : IDbReaderService
    {
        private readonly ILogger<OracleReaderService> _logger;
        private readonly ConnectionDataOption _connectionDataOption;
        public OracleReaderService(ILogger<OracleReaderService> logger, IOptions<ConnectionDataOption> connectionDataOption)
        {
            _logger = logger;
            _connectionDataOption = connectionDataOption.Value;
        }

        public async Task<IEnumerable<object>> ReadDataAsync(string command, Dictionary<string, object> dictParams)
        {
            try
            {
                using (var connection = new OracleConnection(_connectionDataOption.OracleDbConnection))
                {
                    var parameters = new DynamicParameters(dictParams);

                    return await connection.QueryAsync<object>(command, parameters);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DbRequest error");
            }

            return Enumerable.Empty<object>();
        }
    }
}
