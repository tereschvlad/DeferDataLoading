using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Npgsql;

namespace DelayedDataLoading.Services
{
    internal class MySqlReaderService : IDbReaderService
    {
        private readonly ILogger<MySqlReaderService> _logger;
        private readonly ConnectionDataOption _connectionDataOption;
        public MySqlReaderService(ILogger<MySqlReaderService> logger, IOptions<ConnectionDataOption> connectionDataOption)
        {
            _logger = logger;
            _connectionDataOption = connectionDataOption.Value;
        }


        public async Task<IEnumerable<object>> ReadDataAsync(string command, Dictionary<string, object> dictParams)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionDataOption.PostgreeDbConnection))
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
