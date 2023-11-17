* Architecture: it's how we structure our software.
* Blueprint. 
    - Costly decisions.

- Architectural patterns:
    - A general, reusable resolution to a commonly occurring problem in software architecture within a given context.
    - Examples:
        - N-tier/Layered architectural.
        - Hexagonal/Ports and adapters architecture.
        - Microservices architecture.
        - Clean architecture.
        - Service-oriented architecture.
        - Modular monolith architecture.
        - Event-driven architecture.
        - MVC - MVVC

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

- Business logic is independent from DA layer. It is in the center. It should have any dependencies. 
    - Clean
    - Hexagonal / Ports and adapters
    - Onion architecture.

## Clean architecture.
    - Domain and application compose the business logic.
    - Infrastructure (other things + interacting to DB).
    - All dependencies point towards the core of the application (application and domain layers).
    - Define interface in the application layer and in the domain layer and the other layers will interact with the interfaces without caring about the implementation details.
    - Inner layers contain business logic and outer layers contain infrastructure and interaction with the outside world.

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
                
                [Application Layer]        
                
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
Responsabilities of presentation layer:
1. Handling interactions with the outside world.
2. Presenting or displaying data.
3. Translating data from the user and convert it to the language of the application core logic. 
4. Managing UI and framework-related elements.
5. Manipulating the application layer. 

- Create CreateSubscriptionRequest, SubscriptionType and SubscriptionResponse contracts on the contracts project.

- Create a new controller in the Api project:
```c#
using GymManagement.Contracts.Subscriptions;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class SubscriptionsController : ControllerBase{
    [HttpPost]
    public IActionResult CreateSubscription(CreateSubscriptionRequest request){
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
 
- Create Subscriptions folder in GymManagement.Application project. 
- Create CreateSubscription class. 

- We're not adding a reference to the Contracts project into the application layer so we cannot simply pass the request from the presentation layer to the application layer as its coming. We must transform into the language wichi is the core of the application (application and domain layers).

* Responsabilities:
- Execute the application's use cases (actions that user can do on the system, features of the application).
    - Fetch domain objects. 
    - Manipulate domain objects.

- We don't want to register the dependencies to the Program.cs / GymManagement.Api project so we're going to create an DependencyInjeciton file on the Infraestructure project (layer) and the Application project (layer) which will be invoked during startup (addInfrastructure and addApplicatin methods). 
- Application layer has the interfaces and Infrastructure has the concrete implementation.
- Add a reference from the api project to the infrastructure project but making methods internal except for DI which are public.
```bash
dotnet add src/GymManagement.Api reference src/GymManagement.Infrastructure/
```

- CQRS: Command Query Responsability Segregation
    - split reads from writes.
    - Commands (writes, f.e: createsubscription, deletesubscription, deleteroom, etc).
        - Write manipulates state and is void.
    - Queries (reads, f.e: getSubscription, getRoom, etc).
        - Read doesn't manipulate state and returns value. 

- Mediator Pattern.
    - Interaction between objects is encapsulated through a Mediator.
    - Reduces coupling. 

```bash
dotnet add src/GymManagement.Application/ package MediatR
```

- Subscriptions controller is intercting directly with the subscription service on the application layer. Now we're going to use the MediatR to know which logic it will invoke. 
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

## Repository & Unit of work

- 