# Microservices in .NET Core
Sample impelementation from the book [*Microservices in .NET Core*](https://www.manning.com/books/microservices-in-net-core) 
by Christian Horsdal Gammelgaard, ISBN 9781617293375.

Running a [Raspberry Pi](https://www.raspberrypi.org/products/raspberry-pi-4-model-b/) 
with a [64-bit operating system](https://ubuntu.com/download/raspberry-pi) 
under a [Docker swarm](https://www.docker.com/).

## How to build a micro services swarm
1. Clone the repository
1. cd Misrosovers
1. [Manually add](https://www.jetbrains.com/datagrip/) UserIds to Shopping Cart table
1. docker-compose build
1. docker-compose up -d
1. docker-compose ps
1. ProductCatalogSvc: http://host:8086/swagger/index.html
1. ShoppingCartSvc: http://host:8087/swagger/index.html
1. Database: [PostgreSQL](https://www.postgresql.org/) host, port: 5432, user: cartapp, pass: cartpw
1. docker-compose down --volumes
1. docker-compose push

## Current Shoping Cart Features
* [REST ASP.NET](https://dotnet.microsoft.com/apps/aspnet) interface
* [Persistance storage](https://github.com/StackExchange/Dapper) - Database
* Get cart contents
* Add products to the cart
* Empty the cart
* Ask Product Catalog for information
* Caching Product Catalog information
* Generate event notifications

## Current Product Features
* [REST ASP.NET](https://dotnet.microsoft.com/apps/aspnet) interface
* Returns dummy information

## Additional Shopping Cart Features
* Get product prices
* Inventory Events
* Cusomters
* Reseverations
* Permotions
* Save for Latter
* Suggestions

## Additional Services
* Event Logging
* Customers
* Inventory
* Suppliers
* Shippers
* Promotons
* Accounting
* Security
* Performance
* Feedback
* Whish List
* Collections
* Advertising
* Broswing History
* Help
