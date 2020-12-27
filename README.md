# Microservices in .NET Core
Sample implementation from the book [*Microservices in .NET Core*](https://www.manning.com/books/microservices-in-net-core) 
by Christian Horsdal Gammelgaard, [ISBN 9781617293375](https://en.wikipedia.org/wiki/Special:BookSources?isbn=9781617293375).

Running on a [Raspberry Pi](https://www.raspberrypi.org/products/raspberry-pi-4-model-b/) 
with a [64-bit operating system](https://ubuntu.com/download/raspberry-pi) 
in a [Docker installation](https://www.docker.com/).

## How to build a micro services swarm
1. Start with a [Raspberry Pi](https://www.raspberrypi.org/products/raspberry-pi-4-model-b/)
1. Install a [64-bit operating system](https://ubuntu.com/download/raspberry-pi)
1. Install [Docker](https://docs.docker.com/engine/install/ubuntu/)
	1. Select *arm64* in step three
1. Install [Docker-Compose](https://docs.docker.com/compose/install/)
	1. Use the Alternative Install Options
	1. `pip3 install docker-compose`
1. Clone the repository
	1. `git clone https://github.com/GregEakin/Microservices.git`
1. `cd Microservices`
1. `docker-compose build`
1. `docker-compose up -d`
1. `docker-compose ps`
1. [Manually add](https://www.jetbrains.com/datagrip/) UserIds to Shopping Cart table
1. ProductCatalogSvc: http://host:8086/swagger/index.html
1. ShoppingCartSvc: http://host:8087/swagger/index.html
1. Database: [PostgreSQL](https://www.postgresql.org/) host, port: 5432, user: cartapp, pass: cartpw
1. `docker-compose down --volumes`
1. `docker-compose push`

## Current Shopping Cart Features
* [REST ASP.NET](https://dotnet.microsoft.com/apps/aspnet) interface
* [Persistence storage](https://github.com/StackExchange/Dapper) - Database
* Get cart contents
* Add products to the cart
* Empty the cart
* Ask Product Catalog for information
* Caching Product Catalog information
* Generate event notifications
* Unit Tests

## Current Product Features
* [REST ASP.NET](https://dotnet.microsoft.com/apps/aspnet) interface
* Returns dummy information
* Unit Tests

## Additional Shopping Cart Features
* Get product prices
* Inventory Events
* Customers
* Reservations
* Premotions
* Save for Latter
* Suggestions
* Send reminders

## Additional Services
* Event Logging
* Customers
* Inventory
* Suppliers
* Shippers
* Premotions
* Accounting
* Security
* Performance
* Feedback
* Wish List
* Collections
* Advertising
* Browsing History
* Help
