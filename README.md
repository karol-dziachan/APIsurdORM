
# Documentation for APIsurdORM

This documentation provides a comprehensive overview of the APIsurdORM library, including configuration details, functionality, and structure. The application is written in C# and is designed to generate a complete API for entities from a specified directory based on provided templates.

## Table of Contents

1.  Configuration
2.  Functionality
3.  Template Structure
4.  Generated Project Structure
5.  CQRS Compliance
6.  Controllers and Mediator
7.  Database Connections
8.  Logging
9.  Middleware
10.  Entity Attributes

## Configuration

### appsettings.json

To configure the application, the `appsettings.json` file must contain the following section:

```json
{
  "Settings": {
    "TemplatesPath": "C:\\source\\APIsurdORM\\Templates",
    "DestinationPath": "C:\\temp\\generate",
    "EntitiesDirPath": "C:\\source\\APIsurdORM\\Examples\\Pr0t0k07.APIsurdORM.Examples\\Entities\\",
    "ProjetName": "Pr0t0k07.GenerateExamples"
  }
} 
```
-   `TemplatesPath`: Path to the directory containing the template files.
-   `DestinationPath`: Path to the directory where the generated project will be stored.
-   `EntitiesDirPath`: Path to the directory containing the entity classes.
-   `ProjetName`: Name of the generated project.

## Functionality

The application generates a complete API based on specified templates and entities from a given directory. It follows the CQRS (Command Query Responsibility Segregation) pattern, ensuring a clear separation between read and write operations.

## Template Structure

The templates used by the application are structured as follows:
```Templates
├── CORE
│   ├── Application
│   ├── Common
│   └── Domain
├── DATABASE
│   └── Project containing generated DDL
├── PRESENTATION
│   └── Api (endpoints)
└── INFRASTRUCTURE
 ├── Infrastructure
    └── Persistence
 ``` 

### CORE

-   Application: Contains application-level logic, including commands and queries.
-   Common: Common utilities and helpers used across the application.
-   Domain: Domain entities and business logic.

### DATABASE

-   Project containing generated DDL: Contains the generated Data Definition Language scripts for the database.

### PRESENTATION

-   Api (endpoints): Contains the API endpoints.

### INFRASTRUCTURE

-   Infrastructure: General infrastructure-related code.
-   Persistence: Database context and repositories.

## Generated Project Structure

The generated project adheres to the following structure, which aligns with the CQRS pattern:
```mathematica
GeneratedProject
├── Application
│   ├── Commands
│   └── Queries
├── Domain
│   └── Entities
├── Infrastructure
│   ├── Repositories
│   ├── DatabaseContext
│   └── Migrations
├── Presentation
│   └── Controllers
└── Logs
```

## CQRS Compliance

The generated project is fully compliant with the CQRS pattern. Commands and queries are separated into different folders within the `Application` layer. This separation ensures that read and write operations are handled distinctly, promoting clean architecture principles.

## Controllers and Mediator

Controllers in the generated project use the Mediator pattern, leveraging the MediatR library. This approach decouples the controllers from the business logic, enhancing maintainability and testability.

Example controller snippet:
```csharp
[ApiController]
[Route("api/[controller]")]
public class ExampleController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExampleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var query = new GetExampleQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateExampleCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }
}
``` 

## Database Connections

Database connections in the generated project are managed through repositories that use Dapper, a simple object mapper for .NET. This provides high-performance data access with a focus on raw SQL execution.

Example repository snippet:

```csharp
public class ExampleRepository : IExampleRepository
{
    private readonly IDbConnection _dbConnection;

    public ExampleRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<ExampleEntity> GetByIdAsync(int id)
    {
        var query = "SELECT * FROM Examples WHERE Id = @Id";
        return await _dbConnection.QuerySingleOrDefaultAsync<ExampleEntity>(query, new { Id = id });
    }

    public async Task<int> AddAsync(ExampleEntity entity)
    {
        var query = "INSERT INTO Examples (Name, Description) VALUES (@Name, @Description); SELECT CAST(SCOPE_IDENTITY() as int)";
        return await _dbConnection.ExecuteScalarAsync<int>(query, entity);
    }
}
``` 

## Logging

The generated project includes extensive logging capabilities, divided into different levels:

-   Trace: Detailed information, typically of interest only when diagnosing problems.
-   Debug: Information useful to developers for debugging the application.
-   Information: Information about the application's general flow.

Example logging setup in `appsettings.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "System": "Warning"
    }
  }
}
```

Example usage in code:
```csharp
public class ExampleService
{
    private readonly ILogger<ExampleService> _logger;

    public ExampleService(ILogger<ExampleService> logger)
    {
        _logger = logger;
    }

    public void Process()
    {
        _logger.LogInformation("Processing started.");
        // Processing logic
        _logger.LogInformation("Processing finished.");
    }
}
``` 

## Middleware

The generated project includes middleware to handle exceptions and return appropriate HTTP responses. This middleware catches exceptions, logs them, and returns a `BadRequest` response if necessary.

Example middleware:
```csharp
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var result = new RequestResult(false, $"An error occurred while processing the request. Exception message: {exception.Message}");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return context.Response.WriteAsync(JsonConvert.SerializeObject(result));
    }
}
``` 
To use the middleware, add it to the pipeline in `Startup.cs` or `Program.cs`:

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // Other middleware registrations

    app.UseRouting();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}
``` 

## Entity Attributes

Entities in the generated project have attributes that define their behavior in the database. These attributes help configure the database schema and ensure proper mapping between the database and the application.

Example entity with attributes:
```csharp
    [Entity(nameof(TestEntity))]
    [PluralNameEntity("TestEntities")]
    public class TestEntity
    {
        [PrimaryKey]
        [Unique]
        [RequiredProperty]
        [DefaultValue("default NEWID()")]
        [SqlType("UNIQUEIDENTIFIER")]
        public Guid Id {  get; set; }

        [MaxLength(50)]
        [RequiredProperty]
        [Unique]
        [SqlType("NVARCHAR(50)")]
        public string FirstName { get; set; }

        [SqlType("NVARCHAR(50)")]
        [RequiredProperty]
        public string LastName { get; set; }

        [ForeignKey("RelatedEntity", "Id")]
        [RequiredProperty]
        [SqlType("UNIQUEIDENTIFIER")]
        public RelatedEntity RelatedEntityId { get; set; }

    }
``` 
Common attributes used:

-   `Key`: Specifies the primary key.
-   `Required`: Marks a property as required.
-   `MaxLength`: Sets the maximum length for a string property.

By following these guidelines and utilizing the provided configuration and templates, the APIsurdORM library facilitates the generation of a well-structured, maintainable, and scalable API project.