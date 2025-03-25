# CQRS with .NET 9 and MySQL (Separate Read/Write Databases)
This project demonstrates the CQRS (Command Query Responsibility Segregation) pattern using .NET 9 and MySQL, with separate databases for read and write operations.  This architecture optimizes performance and scalability by allowing you to independently scale and tune your read and write data stores.

## Features
- **CQRS Pattern**: Separates command (write) and query (read) operations.

- **.NET 9**: Utilizes the latest features of .NET 9, including init; properties and records.

- **MySQL**: Uses MySQL for data persistence, with separate databases for write and read operations.

- **MediatR**: Implements the CQRS pattern with the help of the MediatR library.

- **RESTful API**: Provides a RESTful API for managing orders.

- **Input Validation**: Uses Data Annotations for request validation.

- **Exception Handling**: Implements robust exception handling with a custom EntityNotFoundException.

- **Response Types**: Uses ProducesResponseType attributes to document API responses.

- **Immutability**: Uses records and init; properties to promote immutability.

- **Separate Read/Write Databases**: Optimizes performance and scalability by using separate MySQL databases for read and write operations.

## Project Structure
The project is organized as follows:

```
├── Controllers             # Contains the API controller
│   └── OrdersController.cs
├── Data                    # Contains the database contexts
│   ├── OrderWriteDbContext.cs # Context for write operations
│   └── OrderReadDbContext.cs  # Context for read operations
├── Models                  # Contains the domain models
│   ├── Order.cs
│   └── OrderItem.cs
├── Commands                # Contains the command definitions
│   ├── CreateOrderCommand.cs
│   └── UpdateOrderStatusCommand.cs
├── Queries                 # Contains the query definitions
│   ├── GetOrderByIdQuery.cs
│   └── GetOrdersByCustomerQuery.cs
├── DTOs                    # Contains the Data Transfer Objects
│   ├── OrderDto.cs
│   └── OrderItemDto.cs
├── Handlers                # Contains the command and query handlers
│   ├── CommandHandlers
│   │   ├── CreateOrderCommandHandler.cs
│   │   └── UpdateOrderStatusCommandHandler.cs
│   ├── QueryHandlers
│   │   ├── GetOrderByIdQueryHandler.cs
│   │   └── GetOrdersByCustomerQueryHandler.cs
├── Program.cs              # Contains the application entry point and configuration
├── appsettings.json        # Contains application settings, including database connection strings
└── [ProjectName].csproj    # Contains the project file
```

## Getting Started
### Prerequisites
- .NET 9 SDK
- MySQL (two instances or databases)

### Installation
1. Clone the repository:

```shell
git clone <repository_url>
```

2. Navigate to the project directory:

```shell
cd <project_directory>
```

3. Update the `appsettings.json` file with your MySQL connection strings. You will need two separate connection strings: one for the write database and one for the read database. Ensure that the read database is populated with data (either through replication, ETL, or other means).

```json
{
  "ConnectionStrings": {
    "OrderWriteDb": "Server=<server-ip>;Port=<server-port>;Database=<write-database>;User=<user>;Password=<password>",
    "OrderReadDb": "Server=<server-ip>;Port=<server-port>;Database=<write-database>;User=<user>;Password=<password>"
  }
}
```

4. Open a terminal in the project directory and run the following commands to apply migrations to the write database:

```shell
dotnet ef database update --context OrderWriteDbContext
```

5. This command creates the schema in the write database.  You may need to create the read database and populate it with data using a separate process (e.g., replication, ETL).  The schema of the read database is defined in the OrderReadDbContext.cs file.

Run the application:

```shell
dotnet run
```

The application will be accessible at https://localhost:<port>, where <port> is specified in your launchSettings.json file.

## API Endpoints
The following API endpoints are available:

```
GET /api/orders/{orderId}
```
Retrieves an order by its ID.  This endpoint reads from the read database.

Response:
- 200 (OK): Returns the order details.
- 404 (Not Found): If the order with the specified ID is not found.

<br>

```
GET /api/orders/customer/{customerName}
```
Retrieves all orders for a specific customer. This endpoint reads from the read database.

Response:
- 200 (OK): Returns an array of order details.

<br>

```
POST /api/orders
```
Creates a new order. This endpoint writes to the write database.

Request body:
```json
{
  "customerName": "string",
  "items": [
    {
      "productId": "string",
      "productName": "string",  // Get from product service
      "quantity": 0,
      "price": 0
    }
  ]
}
```
Response:
- 201 (Created): Returns the ID of the created order.
- 400 (Bad Request): If the request is invalid.

<br>

```
PUT /api/orders/{orderId}/status
```

Updates the status of an order. This endpoint writes to the write database.

Request body:

```json
{
  "status": "string"
}
```

Response:
- 204 (No Content): If the status is updated successfully.
- 400 (Bad Request): If the request is invalid.
- 404 (Not Found): If the order with the specified ID is not found.

## Key Concepts
- **Commands**: Represent write operations that change the state of the system (e.g., CreateOrderCommand, UpdateOrderStatusCommand).  Commands are handled by the write database context.

- **Queries**: Represent read operations that retrieve data from the system (e.g., GetOrderByIdQuery, GetOrdersByCustomerQuery).  Queries are handled by the read database context.

- **Handlers**: Classes that handle commands and queries.  Command handlers modify data in the write database, while query handlers retrieve data from the read database.

- **Data Transfer Objects (DTOs)**: Used to transfer data between the application and the client.  DTOs define the structure of the data that is sent over the network.  In this example, DTOs also define the structure of the data read from the read database.

- **MediatR**: A library that simplifies the implementation of the CQRS pattern by providing a mechanism for routing commands and queries to their respective handlers.

- **Entity Framework Core**: An ORM (Object-Relational Mapper) used to interact with the database.  This project uses two separate EF Core contexts:
    + OrderWriteDbContext: For write operations.
    + OrderReadDbContext: For read operations.

- **Read and Write Databases**: This project uses separate MySQL databases for read and write operations.  This allows for independent scaling and optimization of each database.

    + The write database is optimized for transactional consistency and data integrity.

    + The read database is optimized for query performance and may be denormalized.

## Best Practices
This project follows several best practices:

- **Immutability**: Uses records and init; properties to ensure that data models and commands/queries are immutable.

- **Explicit Validation**: Uses Data Annotations for request validation.

- **Clear Separation of Concerns**: Separates command and query handling logic into distinct classes and uses separate database contexts for read and write operations.

- **Robust Error Handling**: Implements a custom exception (EntityNotFoundException) and handles exceptions in the controller to provide meaningful error responses.

- **Documented API**: Uses ProducesResponseType attributes to document the possible HTTP status codes and response types for each API endpoint.

- **Separate Read/Write Databases**: Uses separate databases for read and write operations to improve performance and scalability.

## Scalability Considerations
- **Independent Scaling**: CQRS with separate read/write databases allows you to scale read and write operations independently.  You can scale the read database (or read replicas) to handle a high volume of read traffic without affecting write performance.  Similarly, you can scale the write database to handle a high volume of write operations without affecting read performance.

- **Read Database Optimization**: The read database can be optimized for query performance through denormalization, indexing, and other techniques.  This optimization does not affect the write database.

- **Data Synchronization**: In a real-world application, you will need to implement a mechanism to synchronize data between the write and read databases.  This is typically done using asynchronous techniques such as eventing or Change Data Capture (CDC).  This project does not include the synchronization logic.

- **Eventual Consistency**:  Be aware that when using separate databases, the read database will be eventually consistent with the write database.

## Future Enhancements
- **Implement Data Synchronization:** Implement a mechanism to synchronize data between the write and read databases (e.g., using events, Change Data Capture).

- **Implement a Product Service**: The CreateOrderCommandHandler currently uses hardcoded product names. In a real-world application, you would integrate with a product service to retrieve product information.

- **Add Integration Events**: You could add integration events to publish events when an order is created or updated. This would allow other services to subscribe to these events and react accordingly. (e.g., sending an email, updating inventory).

- **Implement Saga Pattern**: For more complex order management scenarios (e.g., order cancellation, payment processing), you could implement the Saga pattern to manage long-running transactions across multiple services.

- **Add Authentication and Authorization**: Implement authentication and authorization to secure the API endpoints.

- **Add Unit Tests and Integration Tests**: Implement comprehensive tests to ensure the application's functionality and prevent regressions.