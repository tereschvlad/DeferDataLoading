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

## Example of message format for RabbitMq (queue payload)
Send a UTF‑8 JSON object to the queue named by Connections__QueueName:
``` json
{
	// Query which sould be carried in DB
	"Request":"select * from test_table where id = @id", 
	// Parameters for query
	"Parameters":
	{
		{"id", 12}
	},
	// Name of query for simplify finding in mongodb collection
	"RequestName":"Test_Request",
	// Name of application which send query for simplify finding in mongodb collection
	"Application":"TestApp",
	// User name which send query
	"UserName":"TestUser",
	// Name of mongodb collection where will be saved result
	"MongoCollectionName":"example_collection"
}
```

## Example of data result in MongoDb
``` json
{
  // Query which was carried
  "Request": "select name, age, salary from test_table",
  // Parameters for query
  "Parameters": {

  },
  // Name of query for simplify finding in mongodb collection
  "RequestName": "Test_Request",
  // Name of application which send query for simplify finding in mongodb collection
  "Application": "TestApp",
  // User name which send query
  "UserName": "TestUser",
  // Result of carring query
  "Rows": [

  ],
  // Query completion date
  "CreateDate": {
    "$date": "2025-01-01T01:01:01.001Z"
  }
}
```

### More information how work with it you can see there [Link for dockerhub](https://hub.docker.com/r/vladteresch/deferdataloading) 


