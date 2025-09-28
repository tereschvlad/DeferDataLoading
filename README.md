# DeferDataLoading

Defering the execution of the Db query using RabbitMq for saving and performing queries and using mongodb for saving the result. 

DeferDataLoading it is only application which works with other services as RabbitMq, MongoDb and some types of databases as Oracle, PostgreSQL, MySql and MSSQL. The application only read data about query from queue and perform it. After this the result save into mongodb. MongoDb using as storage for saving results. It allows defer performing queries and simplifying working with results. 

# Why it was created?

This project was created for simplifying working with difficalt queries and unificied interface for working with this types of queries. Application gives tools for defering processing of query and after get a result to work with it.

# How it works?