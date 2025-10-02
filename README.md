# DeferDataLoading

DeferDataLoading is a small worker service that defers execution of database queries using RabbitMQ and persists the results to MongoDB. You publish a message to a queue with the SQL (plus parameters); the worker reads it, runs the query against the configured database engine, and stores the result set in a MongoDB collection. Logs can be shipped to Seq.

Supports PostgreSQL, MySQL, Oracle, and SQL Server (selected by configuration).

# Why?
Complex or long‑running queries can block your application and tie up resources. Offloading them to a background worker allows you to:
- decouple the request/response path,
- run queries asynchronously or on a schedule,
- centralize result storage for later retrieval/analysis.

# How it works
- Producer (any app) publishes a JSON message to a RabbitMQ queue specifying the query and parameters.
- Worker (this service) consumes messages, selects the appropriate DB reader based on Connections:DbName, executes the query, and converts rows to BsonDocuments.
- A ResultRequestDataModel document containing your original request metadata and the returned rows is written to MongoDB.
- The worker runs continuously; a Quartz job triggers reads roughly every 120 seconds by default, also have variable for worker delay WorkerDelayed.
- MongoDB documents are limited to ~16 MB. If a query result exceeds this, you need to page or chunk the output.

## Example of message format for RabbitMq (queue payload)
Send a UTF‑8 JSON object to the queue named by Connections__QueueName:
``` json
{
	"Request":"select * from test_table where id = @id", 
	"Parameters":
	[
		{"id": 12}
	],
	"RequestName":"Test_Request",
	"Application":"TestApp",
	"UserName":"TestUser",
	"MongoCollectionName":"example_collection"
}
```
- Request - Query to be executed in DB
- Parameters - Parameters for query
- RequestName - Name of the query to simplify finding results in MongoDB collection
- Application - Name of the application that sent the query for simplify finding in mongodb collection
- UserName - User name which send query
- MongoCollectionName - Name of mongodb collection where will be saved result

## Example of data result in MongoDb
``` json
{
  "Request": "select name, age, salary from test_table",
  "Parameters": [
  ],
  "RequestName": "Test_Request",
  "Application": "TestApp",
  "UserName": "TestUser",
  "Rows": [
  ],
  "CreateDate": {
    "$date": "2025-01-01T01:01:01.001Z"
  }
}
```
- Request - Query that was executed
- Parameters - Parameters for query
- RequestName - Name of query for simplify finding in mongodb collection
- Application - Name of application which send query for simplify finding in mongodb collection
- UserName - User name that sent the query
- Rows - Result of executing the query
- CreateDate - Query completion date

### Example of bash script for creating container

``` bash
# (optional) create a shared network for your stack
docker network create app-net

docker run -d \
  --name deferdataloading \
  --network app-net \
  --restart unless-stopped \
  -e Connections__DbName=PostgreSql \
  -e "Connections__DbConnection=Host=postgres;Database=mydatabase;Username=test_user;Password=test_password" \
  -e "Connections__MongoDbConnection=mongodb://test:test@mongodb:27017" \
  -e Connections__MongoDbName=MongoDb_collection_name \
  -e Connections__RabbitMqHostName=rabbitmq \
  -e Connections__RabbitMqPort=5672 \
  -e Connections__RabbitMqUser=test \
  -e Connections__RabbitMqPassword=test \
  -e Connections__QueueName=example_rabbitmq_queue \
  -e Connections__SeqKey=example_seqkey \
  -e Connections__SeqHost=http://seq:5341 \
  -e Connections__WorkerDelayed=180 \
  vladteresch/deferdataloading:latest
```

- Connections__DbName - target DB engine (PostgreSql  MySql | Oracle | SqlServer)
- Connections__DbConnection - connection string for selected DB
- Connections__MongoDbConnection - MongoDB connection string
- Connections__MongoDbName - MongoDB collection name
- Connections__RabbitMqHostName - RabbitMQ host name
- Connections__RabbitMqPort - RabbitMQ port
- Connections__RabbitMqUser - RabbitMQ user name
- Connections__RabbitMqPassword - RabbitMQ user password
- Connections__QueueName - queue name for query requests
- Connections__SeqKey - Seq API key
- Connections__SeqHost - Seq server URL
- Connections__WorkerDelayed - worker delay in seconds (how often it polls)
- app-net - network required to connect with other containers

### More information how work with it you can see there [dockerhub](https://hub.docker.com/r/vladteresch/deferdataloading) 


