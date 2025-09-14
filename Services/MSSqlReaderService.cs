using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DelayedDataLoading.Services
{
    internal class MSSqlReaderService : IDbReaderService
    {
        private readonly ILogger<MSSqlReaderService> _logger;
        private readonly ConnectionDataOption _connectionDataOption;
        public MSSqlReaderService(ILogger<MSSqlReaderService> logger, IOptions<ConnectionDataOption> connectionDataOption)
        {
            _logger = logger;
            _connectionDataOption = connectionDataOption.Value;
        }

        public async Task<IEnumerable<dynamic>> ReadDataAsync(string command, Dictionary<string, object> dictParams)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionDataOption.DbConnection))
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
}
