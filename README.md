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
```

## More information how work with it
[Link for image in dockerhub](https://hub.docker.com/r/vladteresch/deferdataloading) 

