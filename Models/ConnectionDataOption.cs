namespace DelayedDataLoading;

internal class ConnectionDataOption
{
    public string PostgreeDbConnection { get; set; }

    public string OracleDbConnection { get; set; }

    public string MySqlConnection { get; set; }

    public string MsSqlConnection { get; set; }

    public string MongoDbConnection { get; set; }

    public string MongoDbName { get; set; }
}