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
