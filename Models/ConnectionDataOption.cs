namespace DelayedDataLoading;

internal class ConnectionDataOption
{
    public string DbName { get; set; }

    public string DbConnection { get; set; }

    public string MongoDbConnection { get; set; }

    public string MongoDbCollection { get; set; }

    public string QueueName { get; set; }

    public string RabbitMqHostName { get; set; }

    public int RabbitMqPort { get; set; }

    public string RabbitMqUserName { get; set; }

    public string RabbitMqPassword { get; set; }

    public string SeqKey { get; set; }

    public string SeqHost { get; set; }

    public int WorkerDelayed { get; set; }

}