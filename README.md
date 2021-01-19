# Cashless Registration

Short microservice to generate tokens for cashless registration.

## Description
There are basically just two endpoints: One to save card data, which will return a token, and another one where you can validate the token to check if it is valid. All data is saved in memory, using CQRS to keep different models for saving and validating.

## Tech stack
The API was developed using .NET 5, using Swagger for documentation and Docker support for Linux coverage. FluentValidation is being used to make sure the requests are valid, AutoMapper to create mappings for the models, and MediatR to handle commands and queries. A very simple authentication using JWT was included just as an example. For saving data, there is a Dapper + Sqlite combination. There are unit and integration tests, developed using xUnit.

## Coming next
Ideally, it would be nice to introduce BDD (with Specflow) to better explain features, and bind them to the tests. It would also be nice to introduce a NoSQL database (like LiteDB) to enhance the CQRS pattern.
Ideas and suggestions are very welcome ! :)
