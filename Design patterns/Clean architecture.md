* Software Architecture: it's how we structure our software.
* Blueprint. 
    - The choices that we make on how to organize the code are costly decisions.

- Architectural patterns:
    - A general, reusable resolution to a commonly occurring problem in software architecture within a given context.
    - A known solution to a common problem.
    - Examples:
        - N-tier/Layered architectural.
        - Hexagonal/Ports and adapters architecture.
        - Microservices architecture.
        - Clean architecture.
        - Service-oriented architecture.
        - Modular monolith architecture.
        - Event-driven architecture.
        - MVC - MVVM

## Layered architecture (N-tier)
- 4 tiers.
- Tier 3 depends on tier 4
- Each tier usually is a .Net project.
- Dependencies (typically references to enforce dependencies direction).
- Three layers:
    - Presentation
    - Business logic
    - Data access layer
- The problem is all the layers have access to the database. You can call the database directly from the presentation layer.

## Domain-centric architecture

- Business logic is independent from DA layer. It is in the center. It should have any dependencies (self-contained). 
    - Clean
    - Hexagonal / Ports and adapters
    - Onion architecture.

## Clean architecture.
    - Domain and Application compose the business logic (layer) of the application.
    - Infrastructure (other things + interacting to DB).
    - All dependencies point towards the core of the application (application and domain layers).
    - Define interfaces in the application layer and in the domain layer and the other layers will interact with the interfaces without caring about the implementation details.
        - The implementation itself will be defined in the Infrastructure layer.
    - Inner layers contain business logic and outer layers contain infrastructure and interaction with the outside world.
        - Inner layers define interfaces and outer layers define implementation.

- Create projects (one project for each layer of clean architecture):

```bash
dotnet new webapi -o GymManagement.Api
dotnet new classlib -o GymManagement.Application
dotnet new classlib -o GymManagement.Infrastructure
dotnet new classlib -o GymManagement.Domain
```    
- Add dependency from the presentation layer to the application project:
```bash
dotnet add GymManagement.Api reference GymManagement.Application/
## Reference `..\GymManagement.Application\GymManagement.Application.csproj` added to the project.
``` 
- Add reference from the Infraestructure to the Application project
```bash
dotnet add GymManagement.Infrastructure/ reference GymManagement.Application/
```
- Add reference from the Application layer to the Domain layer
```bash
dotnet add GymManagement.Application/ reference GymManagement.Domain/
```

[Presentation Layer]                [Infrastructure Layer]
               |                     | 
                [Application Layer]        
                        |
                    [Domain Layer]
                        
- Create the actual project solution:
```bash
dotnet new sln --name "GymManagement"
```
- Add projects to the solution:
```bash
dotnet sln add **/**.csproj
```
- Build project
```bash
dotnet build
```

- Install REST Client extension to send http request through VS code.
- Create foo.http to handle the http requests
- Run webapi project:
```bash
dotnet run --project src/GymManagement.Api/
```
- Modify the foo.http file and send the request to get the default weather info:
```http
GET http://localhost:5215/WeatherForecast
```
## Presentation layer

- Outmost layer. What's presented to the outside world.
- Take data as it arrives from the user and convert it to the language of the actual application.
- The room in the API doesn't neccesary match a room represented in the application layer. 
- Presenting or displaying data.
- Translating data. 

## Implementation layer

- Contracts project is the implementation of the API with the client. Standalone project to publish it as a nuget package. Clients can use this nuget package instead of defining the API themselves. 
- Add new project:
```bash
dotnet new classlib -o GymManagement.Contracts
```
- Add a reference from the Api project to the contracts project. 
```bash
dotnet add GymManagement.Api/ reference GymManagement.Contracts/
```
- Update requests file to represent the CreateSubscriptionRequest:
- requests/Subscriptions/CreateSubscription.http:
    ```http
    @host =..
    @adminId=..
    POST {{host}}/Subscriptions

    Content-Type: application/json
    {
        "SubscriptionType" : "Free", //"Starter", "Pro"
        "AdminId": "{{adminId}}"
    }

    # Content-Type: : application/json
    # {
    #     "Id": {{adminId}}
    #     "SubscriptionType" : "Free", //"Starter", "Pro"
    # }
    ```

Responsabilities of presentation layer:
1. Handling interactions with the outside world.
2. Presenting or displaying data.
3. Translating data from the user and convert it to the language of the application core logic. 
4. Managing UI and framework-related elements.
5. Manipulating the application layer. 

- Create CreateSubscriptionRequest, SubscriptionType and SubscriptionResponse contracts on the contracts project (Subscriptions folder).
- CreateSubscriptionRequest:
```c#
namespace GymManagement.Contracts.Subscriptions;

public record CreateSubscriptionRequest(SubscriptionType SubscriptionType, Guid AdminId);

```
- SubscriptionResponse:
```c#
namespace GymManagement.Contracts.Subscriptions;

public record SubscriptionResponse(Guid Id, SubscriptionType SubscriptionType);
```
- SubscriptionType:
```c#
using System.Text.Json.Serialization;
namespace GymManagement.Contracts.Subscriptions;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SubscriptionType
{
    Free,
    Starter,
    Pro
}
```

- Create a new controller in the Api project (SubscriptionsController):
```c#
using GymManagement.Contracts.Subscriptions;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionsController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateSubscription([FromBody]CreateSubscriptionRequest request)
    {
        return Ok(request);
    }
}
```

- Add a new CreateSubscription http file:
```http
@host=http://localhost:5215
@adminId=6b7be891-d74d-48ff-b0ec-8627eb35eab3
POST {{host}}/Subscriptions
Content-Type: application/json

{
    "SubscriptionType":"Free",
    "AdminId": "{{adminId}}"
}

# {
#     "Id": "{{adminId}}"
#     "SubscriptionType":"Free",
# }
```
 
 ## Application Layer

- Create Subscriptions folder in GymManagement.Application project. 
- Create CreateSubscription class. 

- We're not adding a reference to the Contracts project into the application layer so we cannot simply pass the request from the presentation layer to the application layer as its coming (because the Contracts project is part of the Presentation layer). We must transform into the language wichi is the core of the application (application and domain layers).

* Responsabilities:
- Execute the application's use cases (actions that user can do on the system, features of the application).
    - Fetch domain objects. 
    - Manipulate domain objects (creating the gym domain object and call add gym method are responsability of the application layer).
    - Examples: create subscription, delete subscription, create gym, list gyms, delete gym, etc.
- Define SubscriptionsService.cs class and interface in services folder (this will be refactored later).

- ISubscriptionService.cs
```c#
namespace GymManagement.Application.Services;

public interface ISubscriptionService
{
    Guid CreateSubscription(string subscriptionType, Guid adminId);
}
```
- SubscriptionService
```c#
namespace GymManagement.Application.Services;

public class SubscriptionService : ISubscriptionService
{
    public Guid CreateSubscription(string subscriptionType, Guid adminId)
    {
        return Guid.NewGuid();
    }
}
```
- Notice that you need to translate SubstriptionType from enum to string because there is no dependency from the Application layer to the contracts project (presentation layer).

- SubscriptionsController.cs
```c#
using GymManagement.Application.Services;
using GymManagement.Contracts.Subscriptions;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionsController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpPost]
    public IActionResult CreateSubscription([FromBody]CreateSubscriptionRequest request)
    {
        var subscriptionId = _subscriptionService.CreateSubscription(
            request.SubscriptionType.ToString(), 
            request.AdminId);
        
        var response = new SubscriptionResponse(subscriptionId, request.SubscriptionType);
        return Ok(response);
    }
}
```
- Clean architecture and Dependency Injection:
- We don't want to register the dependencies to the Program.cs / GymManagement.Api project (because it would have to resolve dependencies from the application and the infrastructure layers) so we're going to create an DependencyInjeciton file on the Infraestructure project (layer) and the Application project (layer) which will be invoked during startup from the presentation layer Program.cs file (addInfrastructure and addApplicatin methods). 
- Application layer has the interfaces and Infrastructure has the concrete implementation.

- DependencyInjection.cs (GymManagement.Application layer):
```c#
using Microsoft.Extensions.DependencyInjection;
using GymManagement.Application.Services;

namespace GymManagement.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            return services;
        }
    }
}
```
- Modify api Program.cs:
```c#
using GymManagement.Application;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddApplication();
}
...
```
- Copy DependencyInjection.cs file from Application and paste in the infraestructure layer:

```c#
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services;
        }
    }
}
```
- Modify api Program.cs. This won't compile because there is no reference to the Infrastructure layer from the Presentation layer.
```c#
using GymManagement.Application;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services
        .AddApplication()
        .AddInfrastructure();
}
```
- The only thing needed in the Presentation layer from the Infrastrucuture layer is the DI. Add a reference from the api project to the infrastructure project but making methods internal except for DI which are public.
```bash
dotnet add src/GymManagement.Api reference src/GymManagement.Infrastructure/
```

- CQRS: Command Query Responsability Segregation
    - split reads from writes.
    - Commands (writes, f.e: createsubscription, deletesubscription, deleteroom, etc).
        - Write manipulates state and is void.
    - Queries (reads, f.e: getSubscription, getRoom, etc).
        - Read doesn't manipulate state and returns value. 
- Implementing CQRS in the application:
    - Change ISubscriptionService interface name for ISubscriptionWriteService all over the place.
- Mediator Pattern.
    - Interaction between objects is encapsulated through a Mediator.
    - Reduces coupling. Class A doesn't need to know the implementation details of class C.
        - Class A has an instance of Mediator class and Mediator calls implementation in class C.

```bash
dotnet add src/GymManagement.Application/ package MediatR
```

- Subscriptions controller (presentation) is interacting directly with the SubscriptionWriteService on the application layer. Now we're going to use the MediatR to know which logic it will invoke. 
- Replace SubscriptionService with MediatR request and request handlers.
- Change SubscriptionsController to use Mediator:
```c#
using GymManagement.Contracts.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISender _mediator;
    public SubscriptionsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription([FromBody]CreateSubscriptionRequest request)
    {

        var command = new CreateSubscriptionCommand(request.SubscriptionType.ToString(), request.AdminId);
        
        //This will invoke the corresponding request handler
        var subscriptionId = await _mediator.Send(command);
        
        var response = new SubscriptionResponse(subscriptionId, request.SubscriptionType);
        return Ok(response);
    }
}
```
- Remove Services folder from the GymManagement.Application project.
- Create new MediatR request (folder Subscriptions/Commands/CreateSubscription (CreateSubscriptionCommand.cs)):
```c#
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

public record CreateSubscriptionCommand(string SubscriptionType, Guid AdminId) : IRequest<Guid>; //Guid here is to specify the response value 
```
- Create a handler for that request (CreateSubscriptionCommandHandler.cs). 
Request handler:
```c#
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

//First type specifies the command that we're handling (CreateSubscriptionCommand)
//Second type is for the response value.
public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, Guid>
{
    public Task<Guid> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Guid.NewGuid())   ;
    }
}
```
- Fix Dependency injection (wire up MediatR):
    -options: scan the assembly where the DependencyInjection type is (in this case the Application project) for all the IRequest and IRequestHandler interfaces and wire up everything together.


- Splitting by feature vs splitting by type
    - splitting by feature increases the cohesion. Subscriptions feature folder in the application, infrastructure, presentation, etc. 

## Result Pattern.

- Exception handling gives a wrapper around the result. A result object. It returns the actual value or the exception. 
- It's very useful and common in clean architectures. 
- ErrorOr package to implement result pattern.
- Add package:
```bash
dotnet add src/GymManagement.Application/ package ErrorOr
``` 
- Change mediator request CreateSubscriptionCommand to support ErrorOr:
```c#
using ErrorOr;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

public record CreateSubscriptionCommand(
    string SubscriptionType, 
    Guid AdminId) : IRequest<ErrorOr<Guid>> ;
```
- Change mediator request handler to support ErrorOr
```c#
using ErrorOr;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        return Guid.NewGuid();
    }
}
```

- Change the CreateSubscription controller to handle erroror response using MatchFirst method to return successful responses or errors more concisely.
```c#
using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Contracts.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISender _mediator;
    public SubscriptionsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription([FromBody]CreateSubscriptionRequest request)
    {

        var command = new CreateSubscriptionCommand(request.SubscriptionType.ToString(), request.AdminId);
        
        var createSubscriptionResult = await _mediator.Send(command);
        
        return createSubscriptionResult.MatchFirst(
            guid => Ok(new SubscriptionResponse(guid, request.SubscriptionType)),
            error => Problem()
        );
    }
}
```
- Use Match instead of MatchFirst to return the whole list of errors not just the first.
## Repository & Unit of work

- Repository gives us an illusion as if we're working with objects in memory even if we're working with persisted data. 
- Better unit testing. 
- Db is an implementation detail (business point of view should't care which db we're using).
- We create that illusion in the application layer (the interfaces) and the implementation of the repositories is going to be in the infrastucture layer. 
- It is also common to place the repository definition in the domain layer (domai -driving design).

- Modify Command and Command classes to implement Repository Pattern:

- CreateSubscriptionCommand.cs
```c#
using ErrorOr;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

public record CreateSubscriptionCommand(
    string SubscriptionType, 
    Guid AdminId) : IRequest<ErrorOr<Subscription>> ;
```
- CreateSubscriptionCommandHandler.cs:
```c#
using ErrorOr;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, ErrorOr<Subscription>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;

    public CreateSubscriptionCommandHandler(ISubscriptionsRepository subscriptionsRepository)
    {
        _subscriptionsRepository = subscriptionsRepository;
    }
  
    public async Task<ErrorOr<Subscription>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        //Create a Subscription
        var subscription = new Subscription{
            Id = Guid.NewGuid()
        };
        //Add it to the db
        await _subscriptionsRepository.AddSubscriptionAsync(subscription);

        return subscription;
    }
}
```
- Add subscription model to the Domain layer (Subscriptions/Subscription.cs):
```c#
namespace GymManagement.Domain.Subscriptions;

public class Subscription
{
    public Guid Id { get; set;}
}
```
- Create new folder in Application layer Common/Interfaces for all the interfaces that are gonna be implemented by Infrastructure layer
- Add ISubscriptionsRepository interface:
```c#
using GymManagement.Domain.Subscriptions;

namespace GymManagement.Application.Common.Interfaces;

public interface ISubscriptionsRepository
{
    Task AddSubscriptionAsync(Subscription subscription);
}
```
- Small change in SubscriptionsController to receive Subscription instead of Guid:
```c#
...
  return createSubscriptionResult.MatchFirst(
            subscription => Ok(new SubscriptionResponse(subscription.Id, request.SubscriptionType)),
            error => Problem()
        );
...
```
## Unit of work pattern.

- Unit of work starts a transaction and commits with CommitChangesAsync()
    - Implement transaction in CommandHandler to make sure all changes or none are applied to the db.
- Implement Unit of work pattern:
- Application/Common/Interfaces/IUnitOfWork.cs:
```c#
namespace GymManagement.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task CommitChangesAsync();
}
```
- Implement UnitOfWork in CreateSubscriptionCommandHandler.cs:
```c#
using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, ErrorOr<Subscription>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSubscriptionCommandHandler(ISubscriptionsRepository subscriptionsRepository,
        IUnitOfWork unitOfWork)
    {
        _subscriptionsRepository = subscriptionsRepository;
        _unitOfWork = unitOfWork;
    }
  
    public async Task<ErrorOr<Subscription>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        //Create a Subscription
        var subscription = new Subscription{
            Id = Guid.NewGuid()
        };
        //Add it to the db
        await _subscriptionsRepository.AddSubscriptionAsync(subscription);
        await _unitOfWork.CommitChangesAsync();
        return subscription;
    }
}
```
## Infrastructure layer:
- It is time to implement the interfaces defined in the application layer in the Infrastructure layer.
- Interacting with the persistence solution.
- Interacting with other services (web clients, message brokers, etc).
- Interacting with the underlying machine (system clock, files, etc).
- Identity concerns.
- Implementing the Repository pattern in the infrastructure layer:
- Create folder Subscriptions/Persistence
- Implement SubscriptionRepository.cs:
```c#
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;

namespace GymManagement.Infrastructure.Subscriptions.Persistence;

public class SubscriptionsRepository : ISubscriptionsRepository
{
    private readonly List<Subscription> _subscriptions = new();

    public Task AddSubscriptionAsync(Subscription subscription)
    {
        //Add the subscription to the database
        _subscriptions.Add(subscription);
        return Task.CompletedTask;
    }
}
```
- Wire up everything together in the DependencyInjection file:
```c#
using GymManagement.Application.Common.Interfaces;
using GymManagement.Infrastructure.Subscriptions.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();
            return services;
        }
    }
}
```
    - Notice that the Interface comes from Application layer whereas the implementation comes from the Infrastructure layer (usings).
- Comment out the Unit of work bit just for testing (CreateSubscriptionCommandHandler.cs)

```c#
using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, ErrorOr<Subscription>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    //private readonly IUnitOfWork _unitOfWork;

    public CreateSubscriptionCommandHandler(ISubscriptionsRepository subscriptionsRepository)//,
        //IUnitOfWork unitOfWork)
    {
        _subscriptionsRepository = subscriptionsRepository;
        //_unitOfWork = unitOfWork;
    }
  
    public async Task<ErrorOr<Subscription>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        //Create a Subscription
        var subscription = new Subscription{
            Id = Guid.NewGuid()
        };
        //Add it to the db
        await _subscriptionsRepository.AddSubscriptionAsync(subscription);
        //await _unitOfWork.CommitChangesAsync();
        return subscription;
    }
}
```
- Retrieve subscription:

    - Modify SubscriptionsController.cs in the presentation project:
    ```c#
    using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
    using GymManagement.Application.Subscriptions.Queries.GetSubscription;
    using GymManagement.Contracts.Subscriptions;
    using GymManagement.Domain.Subscriptions;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    namespace GymManagement.Api.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISender _mediator;
        public SubscriptionsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody]CreateSubscriptionRequest request)
        {

            var command = new CreateSubscriptionCommand(request.SubscriptionType.ToString(), request.AdminId);
            
            var createSubscriptionResult = await _mediator.Send(command);
            
            return createSubscriptionResult.MatchFirst(
                subscription => Ok(new SubscriptionResponse(subscription.Id, request.SubscriptionType)),
                error => Problem()
            );
        }

        [HttpGet("{subscriptionId:guid}")]
        public async Task<IActionResult> GetSubscription(Guid subscriptionId)
        {
            var query = new GetSubscriptionQuery(subscriptionId);

            var getSubscriptionResult = await _mediator.Send(query);

            return getSubscriptionResult.MatchFirst(
                subscription => Ok(new SubscriptionResponse(
                    subscription.Id, 
                    Enum.Parse<SubscriptionType>(subscription.SubscriptionType))),
                    error => Problem()
            );
        }
    }
    ```
    - Modify Application layer to add new command query and command query handler:
    - Application/Queries/GetSubscriptionQuery.cs
    ```c#
    using ErrorOr;
    using GymManagement.Domain.Subscriptions;
    using MediatR;

    namespace GymManagement.Application.Subscriptions.Queries.GetSubscription;

    public record GetSubscriptionQuery(Guid SubscriptionId) : IRequest<ErrorOr<Subscription>>;
    ```
    - Application/Queries/GetSubscriptionQueryHandler.cs
    ```c#
    using ErrorOr;
    using GymManagement.Application.Common.Interfaces;
    using GymManagement.Domain.Subscriptions;
    using MediatR;

    namespace GymManagement.Application.Subscriptions.Queries.GetSubscription;

    public class GetSubscriptionQueryHandler : IRequestHandler<GetSubscriptionQuery, ErrorOr<Subscription>>
    {
        private readonly ISubscriptionsRepository _subscriptionsRepository;
        public GetSubscriptionQueryHandler(ISubscriptionsRepository subscriptionsRepository)
        {
            _subscriptionsRepository = subscriptionsRepository;
        }

        public async Task<ErrorOr<Subscription>> Handle(GetSubscriptionQuery query, CancellationToken cancellationToken)
        {
            var subscription = await _subscriptionsRepository.GetByIdAsync(query.SubscriptionId);
            
            return subscription is null 
                ? Error.NotFound(description:"Subscription not found.") 
                : subscription;
        }
    }   
    ```
    - Entity framework implementation to persist and retrieve subscription data.
    - Add EntityFrameworkCore the the infrastructure layer:
    ```bash
    dotnet add GymManagement.Infrastructure/ package Microsoft.EntityFrameworkCore
    ```
    - Create Common/Persistence/ folder in the Infrastructure layer and create the dbcontext there (GymManagementDbContext.cs):
    ```c#
    using GymManagement.Domain.Subscriptions;
    using Microsoft.EntityFrameworkCore;

    namespace GymManagement.Infrastructure.Common.Persistence;

    public class GymManagementDbContext : DbContext
    {
        public DbSet<Subscription> Subscriptions { get; set; } = null!;
        
        public GymManagementDbContext(DbContextOptions<GymManagementDbContext> options) : base(options)
        {
            
        }
    }
    ```
    - Modify the SubscriptionRepository (Infrastructure layer) and change the static list of Subscriptions for dbContext:
    ```c#
    using GymManagement.Application.Common.Interfaces;
    using GymManagement.Domain.Subscriptions;
    using GymManagement.Infrastructure.Common.Persistence;

    namespace GymManagement.Infrastructure.Subscriptions.Persistence;

    public class SubscriptionsRepository : ISubscriptionsRepository
    {
        private readonly GymManagementDbContext _dbContext;

        public SubscriptionsRepository(GymManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    
        public async Task AddSubscriptionAsync(Subscription subscription)
        {
            //Add the subscription to the database
            await _dbContext.Subscriptions.AddAsync(subscription);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Subscription?> GetByIdAsync(Guid subscriptionId)
        {
            //Get the subscription from the database
            var subscription = await _dbContext.Subscriptions.FindAsync(subscriptionId);
            return subscription;
        }
    }
    ```
    - Configure the dbcontext in the dependency injection of the infrastructure layer:
    ```c#
    using GymManagement.Application.Common.Interfaces;
    using GymManagement.Infrastructure.Common.Persistence;
    using GymManagement.Infrastructure.Subscriptions.Persistence;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    namespace GymManagement.Infrastructure
    {
        public static class DependencyInjection
        {
            public static IServiceCollection AddInfrastructure(this IServiceCollection services)
            {
                services.AddDbContext<GymManagementDbContext>(options =>
                    options.UseSqlite("Data Source=GymManagement.db"));
                    
                services.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();
                return services;
            }
        }
    }
    ```
    - Add sqllite to infrastructure layer:
    ```bash
    dotnet add GymManagement.Infrastructure/ package Microsoft.EntityFrameworkCore.Sqlite
    ```
    - To create migrations you need to install Design to the Api:
    ```bash
    dotnet add GymManagement.Api/ package Microsoft.EntityFrameworkCore.Design
    ```

    - Create a new migration (-p: project that contains the dbcontext, -s: entry point of the project)
    ```bash
    dotnet ef migrations add InitialCreate -p GymManagement.Infrastructure/ -s GymManagement.Api/
    ```
    - Execute the migration:
    ```bash
    dotnet ef database update -p GymManagement.Infrastructure/ -s GymManagement.Api/
    ```
    - Note: you can install Sqllite extension in VS Code to show database (check Sqllite explorer and open GymManagement.db)

### Implement the Unit of work pattern:

    - Implement IUnitOfWork interface in the dbcontext:
    (EntityFrameworkCore already implements the unit of work pattern)

    ```c#
    using GymManagement.Application.Common.Interfaces;
    using GymManagement.Domain.Subscriptions;
    using Microsoft.EntityFrameworkCore;

    namespace GymManagement.Infrastructure.Common.Persistence;

    public class GymManagementDbContext : DbContext, IUnitOfWork
    {
        public DbSet<Subscription> Subscriptions { get; set; } = null!;
        
        public GymManagementDbContext(DbContextOptions<GymManagementDbContext> options) : base(options)
        {
            
        }

        public async Task CommitChangesAsync()
        {
            await base.SaveChangesAsync();
        }
    }
    ```
    - Remove savechanges from the repository:

    ```c#
    //...
    public async Task AddSubscriptionAsync(Subscription subscription)
    {
        //Add the subscription to the database
        await _dbContext.Subscriptions.AddAsync(subscription);
    } 
    //...
    ```
    - Uncomment unitofwork implementation from Application/.../CreateSubscriptionCommandHandler.cs:

    ```c#
    using ErrorOr;
    using GymManagement.Application.Common.Interfaces;
    using GymManagement.Domain.Subscriptions;
    using MediatR;

    namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

    public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, ErrorOr<Subscription>>
    {
        private readonly ISubscriptionsRepository _subscriptionsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSubscriptionCommandHandler(ISubscriptionsRepository subscriptionsRepository,
            IUnitOfWork unitOfWork)
        {
            _subscriptionsRepository = subscriptionsRepository;
            _unitOfWork = unitOfWork;
        }
    
        public async Task<ErrorOr<Subscription>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            //Create a Subscription
            var subscription = new Subscription{
                Id = Guid.NewGuid(),
                SubscriptionType = request.SubscriptionType
            };
            //Add it to the db
            await _subscriptionsRepository.AddSubscriptionAsync(subscription);
            await _unitOfWork.CommitChangesAsync();
            return subscription;
        }
    }
    ```
- Specify in the dependencyinjection (infrastructure) to use dbcontext as unit of work:
    
```c#
using GymManagement.Application.Common.Interfaces;
using GymManagement.Infrastructure.Common.Persistence;
using GymManagement.Infrastructure.Subscriptions.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<GymManagementDbContext>(options =>
                options.UseSqlite("Data Source=GymManagement.db"));

            services.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();
            services.AddScoped<IUnitOfWork>(services => services.GetRequiredService<GymManagementDbContext>());
            return services;
        }
    }
}
```
### EF Core and repository pattern:

- Wrapping entityframework repository implementation with a custom repository in the infrastructure layer. 
- Sometime the EF queries are complex so it is best to keep them in a wrapper in the infrastructure layer instead of calling the Add and SaveChanges from EF directly in the application layer.

### Domain-Driven Design vs Clean Architecture:

- Developing Complex systems. 
- Complex ideas (domains) and break them up on smaller chunks. 
- A set of practices, guidelines and concepts. 
- Makes the code base easier to work with, more maintable.
- Domain models:
    - Object that contain properties and behaviour. Model model: Gym.
    - Rich vs anemic domain models:
        - Anemic: expose data and rely on external manipulation of this data (anti pattern)
        - Example:
        - Gym.cs
        ```c#
        public class Gym{
            public List<Room> Rooms{get; set;}
        }
        ```
        - GymHandler.cs
        ```c#
        public void AddRoomHandler(Gym gym, Room room){
            gym.Rooms.Add(room);
        }
        ```

        - Rich: contain inside not only the data but the behaviour. The more behaviour the richer:
        Example: Gym.cs
        ```c#
        public class Gym{
            private readonly int _maxRooms = 3;
            private readonly List<Guid> _roomsIds = new();

            public ErrorOr<Sucess> AddRoom(Room room){
                if(_rooms.Cotnains(room.Id))
                    return Error.Conflict();
                if(_rooms.Count > _maxRooms)
                    return Error.Validation();
                
                _roomsIds.Add(room.Id);

                return Result.Success;
            }

        }
        ```
    - Rich Domain Modeling Guidelines:
        - Private fields and properties by default.
        - Expose only when needed.
        - Expose only what's needed. 
    - Always valid Domain Models:
        - Always in a valid state. 
    - Persistence Ignorance:
        - Modeling the domain without taking into account how the domain objects will be persisted.
        - Repository pattern (abstract behind the repository the db implementation details)

## Domain layer:

- In the future you might want to start your application from the Domain layer instead of the presentation. 
Responsabilities:
    - Defining domain models.
    - Defining domain errors (business errors, business rules to enforce). Well defined to identify what went wrong.
    - Executing business logic. 
    - Enforcing business rules.
- Implementing strongly typed enums (we want to control the data). We dont want the value to be anything:
    - Install smartenum nuget package in the domain layer:
    ```bash
    dotnet add GymManagement.Domain/ package Ardalis.SmartEnum
    ```
    - Create /Domain/Subscriptions/SubscriptionType.cs
    ```c#   
    using Ardalis.SmartEnum;

    namespace GymManagement.Domain.Subscriptions;

    public class SubscriptionType : SmartEnum<SubscriptionType>

    {
        public static readonly SubscriptionType Free = new(nameof(Free), 0);
        public static readonly SubscriptionType Starter = new(nameof(Starter), 1);
        public static readonly SubscriptionType Pro = new(nameof(Pro), 2);
        public SubscriptionType(string name, int value) : base(name, value)
        {
        }
    }
    ```
    - Change the Subscription.cs to use SubscriptionType instead of string:
    ```c#
    namespace GymManagement.Domain.Subscriptions;

    public class Subscription
    {
        public Guid Id { get; set;}
        public SubscriptionType SubscriptionType { get; set; } = null!;
    }
    ```

- Implementing (Rich) Domain models:
- Change Subscription.cs class:
```c#
namespace GymManagement.Domain.Subscriptions;

public class Subscription
{
    private readonly Guid _adminId;
    public Guid Id { get; }
    public SubscriptionType SubscriptionType { get; }

    public Subscription(
        SubscriptionType subscriptionType,
        Guid adminId,
        Guid? id = null)
    {
        SubscriptionType = subscriptionType;
        _adminId = adminId;
        Id = id ?? Guid.NewGuid();
    }
}
```
- Change CreateSubscriptionCommand to use enum instead of string:
```c#
using ErrorOr;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

public record CreateSubscriptionCommand(
    SubscriptionType SubscriptionType, 
    Guid AdminId) : IRequest<ErrorOr<Subscription>> ;

```
- Modify the CreateSubscriptionCommandHandler to use the constructor to get the Subscription object from the Domain.
```c#
    //...
    public async Task<ErrorOr<Subscription>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        //Create a Subscription
        var subscription = new Subscription(
            request.SubscriptionType,
            request.AdminId
        );
        //Add it to the db
        await _subscriptionsRepository.AddSubscriptionAsync(subscription);
        await _unitOfWork.CommitChangesAsync();
        return subscription;
    }
    //...
``` 
- Need to transform the Controller subscription type because the Contacts subscriptiontype enum is different than the domain subscriptiontype. 
    - Presentation layer is responsible for converting data from the presentation language to the internal language of the application (Domain layer).
    ```c#
    using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
    using GymManagement.Application.Subscriptions.Queries.GetSubscription;
    using GymManagement.Contracts.Subscriptions;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using DomainSubscriptionType = GymManagement.Domain.Subscriptions.SubscriptionType;
    using SubscriptionType = GymManagement.Contracts.Subscriptions.SubscriptionType;

    namespace GymManagement.Api.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISender _mediator;
        public SubscriptionsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody]CreateSubscriptionRequest request)
        {

            if(!DomainSubscriptionType.TryFromName(
                request.SubscriptionType.ToString(), out var subscriptionType))
                {
                    return Problem(statusCode: StatusCodes.Status400BadRequest, detail: "Invalid subscription type");
                }

            var command = new CreateSubscriptionCommand(
                    subscriptionType, 
                    request.AdminId);
            
            var createSubscriptionResult = await _mediator.Send(command);
            
            return createSubscriptionResult.MatchFirst(
                subscription => Ok(new SubscriptionResponse(subscription.Id, request.SubscriptionType)),
                error => Problem()
            );
        }

        [HttpGet("{subscriptionId:guid}")]
        public async Task<IActionResult> GetSubscription(Guid subscriptionId)
        {
            var query = new GetSubscriptionQuery(subscriptionId);

            var getSubscriptionResult = await _mediator.Send(query);

            return getSubscriptionResult.MatchFirst(
                subscription => Ok(new SubscriptionResponse(
                    subscription.Id, 
                    Enum.Parse<SubscriptionType>(subscription.SubscriptionType.Name))),
                    error => Problem()
            );
        }
    }


    ```
## Implementing domain model EF core config
- Create Infrastructure/Subscriptions/SubscriptionConfiguration.cs
```c#
using GymManagement.Domain.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Subscriptions.Persistence;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(s => s.Id).ValueGeneratedNever();
        builder.Property("_adminId")
            .HasColumnName("AdminId");
        
        builder.Property(s => s.SubscriptionType)
            .HasConversion(
                subscriptionType => subscriptionType.Value,
                value => SubscriptionType.FromValue(value));

    }
}
```

- Update dbcontext, create new migration and update db:
```c#
using System.Reflection;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Common.Persistence;

public class GymManagementDbContext : DbContext, IUnitOfWork
{
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    
    public GymManagementDbContext(DbContextOptions<GymManagementDbContext> options) : base(options)
    {
        
    }

    public async Task CommitChangesAsync()
    {
        await base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
```

## Error handling

- Different types of errors for different layers.
    - Presentation:
        - Convertion errors (invalid data). Return an error without invoking the core of the application.
        - For example: invalid subscription type cast.
        - Client trying to interact with endpoint that doesnt exists or navigate to a page that doesnt exists.
        - Authentication 401 error in the presentation layer.
        - Take errors and present them to the user (converting application & domain errors to presentation errors).
        - What's the actual response?
            - View
            - Error code
            - Depends of the technology.

    - Application:
        - Domain object cannot be found.
        - Data passed from the presentation to the application layer is valid.
        - Manipulation on domain object but current user is not authorized. 
            - Take errors from the domain layer and convert them to a different error that the application wants to propagate to the presentation layer.
    - Domain:
        - Invariants: things that need to be true all times in our object (no more than 3 rooms allowed for example).
        - Propagate upwards that a business rule was applied.
        - Requested action will violate a business rule.
        - Unexpected manipulation on domain objects.
    
    - Error handling approaches in Clean architecture:
        1. Exceptions: when something goes wrong we throw an exception.
            - Exceptions defined in the domain layer (business rules).
            - Some exceptions from the domain layer we want to capture in the application layer and change them before sending them to the presentation layer.
        2. The result pattern:
            - Explicit about the errors and the specific flow.
            - For things that are not supposed to happen just throw an exception.
            - When it is expected use result pattern. 

# Implement errors in the domain layer:

```c#

```