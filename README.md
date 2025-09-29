# DeferDataLoading

Defering the execution of the Db query using RabbitMq for saving and performing queries and using mongodb for saving the result. 

DeferDataLoading it is only application which works with other services as RabbitMq, MongoDb and some types of databases as Oracle, PostgreSQL, MySql and MSSQL. The application only read data about query from queue and perform it. After this the result save into mongodb. MongoDb using as storage for saving results. It allows defer performing queries and simplifying working with results. 

# Why it was created?

This project was created for simplifying working with difficalt queries and unificied interface for working with this types of queries. Application gives tools for defering processing of query and after get a result to work with it.

# How it works?
You need to have this application for work, you need also RabbitMq and MongoDb. Loging system which are used [Seq](https://datalust.co/), so you should have it too. An equally important element is database. Application suport only PostgreSQL, MSSQL, MySql and Oracle. For start this application RabbitMq with queue has already exist MongoDb should be exist too. 

## Example of data for RabbitMq 
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

## Example of docker-compose
``` yml
services:
  deferdataloading:
    container_name: container-name
    image: vladteresch/deferdataloading:latest
    networks:
      - test-network
    environment:
      - Connections__DbName=postgree
      - "Connections__DbConnection=Host=my_potree;Database=mydatabase;Username=test_user;Password=test_password"
      - "Connections__MongoDbConnection=mongodb://test:test@mongodb:27017/"
      - Connections__MongoDbName=MongoDb_collection_name
      - Connections__RabbitMqHostName=my_rabbitmq
      - Connections__RabbitMqPort=5672
      - Connections__RabbitMqUser=test
      - Connections__RabbitMqPassword=test
      - Connections__QueueName=example_rabbitmq_queue
      - Connections__SeqKey=example_seqkey
      - "Connections__SeqHost=http://seq:5341"

networks:
  test-network:
    external: true
    name: test-network   
```

## Exa,ple of bash script
``` bash
docker run -d \
  --name test-network \
  --network test-network \
  --restart unless-stopped \
  -e Connections__DbName=postgree \
  -e "Connections__DbConnection=Host=my_potree;Database=mydatabase;Username=test_user;Password=test_password" \
  -e "Connections__MongoDbConnection=mongodb://test:test@mongodb:27017/" \
  -e Connections__MongoDbName=MongoDb_collection_name \
  -e Connections__RabbitMqHostName=my_rabbitmq \
  -e Connections__RabbitMqPort=5672 \
  -e Connections__RabbitMqUser=test \
  -e Connections__RabbitMqPassword=test \
  -e Connections__QueueName=example_rabbitmq_queue \
  -e Connections__SeqKey=example_seqkey \
  -e Connections__SeqHost=http://seq:5341 \
  vladteresch/deferdataloading:latest
```

- Connections__DbName - name of database which you connect
- Connections__DbConnection - connection string for database
- Connections__MongoDbConnection - connection string for mongodb
- Connections__MongoDbName - comgodb collection name
- Connections__RabbitMqHostName - rabbitmq host name
- Connections__RabbitMqPort - rabbitmq port
- Connections__RabbitMqUser - rabbitmq user name 
- Connections__RabbitMqPassword - rabbitmq user password 
- Connections__QueueName - rabbitmq queue name for saving data for queries
- Connections__SeqKey - seq authorize key
- Connections__SeqHost - seq connection
- test-network - network need for connecting with others containers


## More information how work with it
[Link for image in dockerhub](https://hub.docker.com/r/vladteresch/deferdataloading) 

