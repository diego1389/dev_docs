- Inverting the control and allow whoever uses the class to provide the implementation details by depending on abstraction; 
- It also makes class unit testable. 
- Before D.I (you cannot apply unit testing to this method):
```csharp
string CreateGreetMessage()
{
    var dateTimeNow = DateTime.Now;
    switch (dateTimeNow.Hour)
    {
        case >= 5 and < 12:
            return "Good morning";
        case >= 12 and < 18:
            return "Good afternoon";
        default:
            return "Good evening";
    }
}
```
- Refactor using Dependency Injection:

```csharp
// See https://aka.ms/new-console-template for more information

public interface IDateTimeProvider
{
    public DateTime DateTimeNow { get; }
}

public class SystemDateProvider : IDateTimeProvider
{
    public DateTime DateTimeNow => DateTime.Now;
}

public class Greeter
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public Greeter(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public string CreateGreetMessage()
    {
        var dateTimeNow = _dateTimeProvider.DateTimeNow;
        switch (dateTimeNow.Hour)
        {
            case >= 5 and < 12:
                return "Good morning";
            case >= 12 and < 18:
                return "Good afternoon";
            default:
                return "Good evening";
        }
    }
}

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Greeter g = new Greeter(new SystemDateProvider());
            Console.WriteLine(g.CreateGreetMessage());
        }
    }
}
```

- We use interfaces instead of abstract classes because with abstract classes you will still need an implementation (no Dependency injection).
- People prefer composition over inheritance.
- Service container (how you want your service to be resolved) and the container will resolve them for you. 
- service provider container resolves IWeatherService dependency in Application with OpenWeatherService implementation:
- Application.cs
```csharp
using System.Text.Json;
using Weather.ConsoleApp.Weather;

namespace Weather.ConsoleApp;

public class Application
{
    private readonly IWeatherService _weatherService;

    public Application(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public async Task RunAsync(string[] args)
    {
        var city = args[0];
        var weather = await _weatherService.GetCurrentWeatherAsync(city);
        var serializedWeather = JsonSerializer.Serialize(weather, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        Console.WriteLine(serializedWeather);
    }
}
```
- Program.cs
```csharp
using Microsoft.Extensions.DependencyInjection;
using Weather.ConsoleApp;
using Weather.ConsoleApp.Weather;

if (args.Length == 0)
{
    args = new string[1];
    args[0] = "London";
}

var services = new ServiceCollection();

services.AddSingleton<IWeatherService, OpenWeatherService>();
services.AddSingleton<Application>();

var serviceProvider = services.BuildServiceProvider();

var app = serviceProvider.GetService<Application>();
var application = serviceProvider.GetRequiredService<Application>();

await application.RunAsync(args);
```
- The ServiceCollection class is an Enumerable of ServiceDescriptors (describe how the services have to be resolved by the Dependency Injection framework).  
- The Service provider is the actual container that will help you resolve the services. The ServiceCollection describe the services and the ServiceProvider resolves them.  
- Result
```Json
{
  "coord": null,
  "weather": null,
  "base": null,
  "main": null,
  "visibility": 0,
  "wind": null,
  "clouds": null,
  "dt": 0,
  "sys": null,
  "timezone": 0,
  "id": 0,
  "name": null,
  "cod": 401
}
```
- Starting in .Net 6 .dot net core will not use Startup.cs file to configure Dependency Injection. Now we only use Program.cs

- Program.cs
```csharp
using Weather.Api.Service;
using Weather.Api.Weather;

var builder = WebApplication.CreateBuilder(args); //Builder creation

// ConfigureServices Starts
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddTransient<IWeatherService, OpenWeatherService>();
builder.Services.AddScoped<IdGenerator>();
// ConfigureService Ends

var app = builder.Build(); //Application creation

// Configure Starts
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//Configure End

app.Run();
```
- Dependency lifetimes:

- **Transient:**  anytime you require a new instance of that class will be created (a new IdGenerator is created anytime you call it).
- Program.cs (register IdGenerator as transient)
```csharp
using WeatherApiTest.Filter;
using WeatherApiTest.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<LifetimeIndicatorFilter>();
builder.Services.AddTransient<IdGenerator>();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();
```
- LifetimeController.cs
```csharp
using System;
using Microsoft.AspNetCore.Mvc;
using WeatherApiTest.Filter;
using WeatherApiTest.Services;

namespace WeatherApiTest.Controllers
{
    [ApiController]
    public class LifetimeController : ControllerBase
    {
        private readonly IdGenerator _idGenerator;

        public LifetimeController(IdGenerator idGenerator)
        {
            this._idGenerator = idGenerator;
        }

        [HttpGet("lifetime")]
        [ServiceFilter(typeof(LifetimeIndicatorFilter))]
        public IActionResult GetId()
        {
            var id = _idGenerator.Id;
            return Ok(id);
        }
    }
}
```
- IdGenerator.cs (new service).
```csharp
using System;
namespace WeatherApiTest.Services
{
    public class IdGenerator
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}
```
- LifetimeIndicatorFilter.cs (new filter that works as middleware of the http request). First calls OnActionExecuting, then the Get method and finally the OnActionExecuted method. 
```csharp
using System;
using Microsoft.AspNetCore.Mvc.Filters;
using WeatherApiTest.Services;

namespace WeatherApiTest.Filter
{
    public class LifetimeIndicatorFilter : IActionFilter
    {
        private readonly IdGenerator _idGenerator;
        private readonly ILogger<LifetimeIndicatorFilter> _logger;

        public LifetimeIndicatorFilter(IdGenerator idGenerator, ILogger<LifetimeIndicatorFilter> logger)
        {
            this._idGenerator = idGenerator;
            this._logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var id = _idGenerator.Id;
            _logger.LogInformation($"{nameof(OnActionExecuted)} id was: {id}");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var id = _idGenerator.Id;
            _logger.LogInformation($"{nameof(OnActionExecuting)} id was: {id}");
        }
    }
}
```
- Since you registered the IdGenerator as transient when you call the Get method in Postman it returns a new Guid and the OnActionExecuting and ActionExecutedContext get a different Guid since it creates a new instance of IdGenerator for the filter class and another one for the get request (lifetimecontroller class). 
- **Singleton lifetime:** a single instance of the class through the lifetime of the application. Thread safety issues. 
- Program.cs
    ```csharp
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddScoped<LifetimeIndicatorFilter>();
    builder.Services.AddSingleton<IdGenerator>();
    ```
- We get the same Guid from the LifetimeController class and the LifetimeIndicatorFilter class (even if we make the call from Postman multiple times). It uses the same instance for each request. 

- **Scoped lifetime:** 
- It creates a new instance each time you make a request. You get the same IdGenerator intance in LifetimeIndicatorFilter and LifetimeController classes but if you make a new request you get a different Guid (but still the same for both classes). 

- Difference between GetService and GetRequiredService is that the latest throws an exception if you don't specify the service implementation (services.AddSingleton<Application>() for example):
- Program.cs
```c#
var services = new ServiceCollection();

services.AddSingleton<IWeatherService, OpenWeatherService>();
//services.AddSingleton<Application>();

var serviceProvider = services.BuildServiceProvider();

var app = serviceProvider.GetService<Application>();
var application = serviceProvider.GetRequiredService<Application>();

await application.RunAsync(args);
```
- It is recommended to use GetRequiredService.
## Registration approaches:
- Program.cs
- Explicit resolving dependencies (it is preferred to use the generic version when possible):
```c#
//generic version
//builder.Services.AddScoped<LifetimeIndicatorFilter>();

//is the same as:
builder.Services.AddScoped(provider =>
{
    var idGenerator = provider.GetRequiredService<IdGenerator>();
    var logger = provider.GetRequiredService<ILogger<LifetimeIndicatorFilter>>();

    return new LifetimeIndicatorFilter(idGenerator, logger);
});

//generic
//builder.Services.AddScoped<IdGenerator>();

//is the same as: 
builder.Services.AddScoped(_ => new IdGenerator());

var app = builder.Build();
```
- In netcore 3.1 instead of builder.Services you have services in the ConfigureServices method. 

## Resolving dependencies

* Resolving dependencies from constructor:
- WeatherForecastController.cs
```c# 
private readonly ILogger<WeatherForecastController> _logger;

public WeatherForecastController(ILogger<WeatherForecastController> logger)
{
    _logger = logger;
}
```
- Program.cs
```c#
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(provider =>
{
    var logger = provider.GetRequiredService<ILogger<DurationLoggerFilter>>();
    ret
```
- Resolving dependencies from the method (not recommended unless you really only need the dependency for that method):
- WeatherForecastController.cs
```c#
[HttpGet(Name = "GetWeatherForecast")]
public IEnumerable<WeatherForecast> Get([FromServices] ILogger<WeatherForecastController> logger)
{
    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    })
    .ToArray();
}
```
- You can resolve the dependencies from the HttpContext (not recommended better use constructor).
```c#
 [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var serviceProvider = HttpContext.RequestServices;
        var result = serviceProvider.GetRequiredService<ILogger<WeatherForecastController>>();


        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
```
- Resolving dependencies from Action Filters as Attributes (they cannot have constructor). //The bad way
- DurationLoggerAttribute.cs
```c#
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ResolvingDeps.WebApi.Attributes;

public class DurationLoggerAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            await next();
        }
        finally
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var logger = serviceProvider.GetRequiredService<ILogger<DurationLoggerAttribute>>();
            var text = $"Request completed in {sw.ElapsedMilliseconds}ms";
            logger.LogInformation(text);
            //Console.WriteLine(text);
        }
    }
}
```
Resolving dependencies from Action Filters as Attributes (they cannot have constructor). //The good way using Service Filters:
1. Change Program.cs to register the filter:
```c#
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(provider =>
{
    var logger = provider.GetRequiredService<ILogger<DurationLoggerFilter>>();
    return new DurationLoggerFilter(logger);
});
```
2. Change the Controller method:
```c#
   [HttpGet("weather")]
    [ServiceFilter(typeof(DurationLoggerFilter))]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}
```
3. Define the filter:
```c#
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ResolvingDeps.WebApi.Filters;

public class DurationLoggerFilter : IAsyncActionFilter
{
    private readonly ILogger<DurationLoggerFilter> _logger;

    public DurationLoggerFilter(ILogger<DurationLoggerFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            await next();
        }
        finally
        {
            var text = $"Request completed in {sw.ElapsedMilliseconds}ms";
            _logger.LogInformation(text);
        }
    }
}
```
- Resolving dependencies from middleware:
- DurationLoggerMiddleware.cs
```c#
using System.Diagnostics;

namespace ResolvingDeps.WebApi.Middlewares;

public class DurationLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<DurationLoggerMiddleware> _logger;

    public DurationLoggerMiddleware(RequestDelegate next,
        ILogger<DurationLoggerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            await _next(context);
        }
        finally
        {
            var text = $"Request completed in {sw.ElapsedMilliseconds}ms";
            _logger.LogInformation(text);
        }
    }
}
```
- Register middleware in Program.cs
```c#
app.UseMiddleware<DurationLoggerMiddleware>();
```
- Resolving dependencies in Minimal Api
- PRogram.cs
```c#
using ResolvingDeps.MinimalApi;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("weather", (ILogger<Program> logger) =>
{
    var weatherSummaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    var weather = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = weatherSummaries[Random.Shared.Next(weatherSummaries.Length)]
        }).ToArray();
    logger.LogInformation("Hi from logger.");
    return Results.Ok(weather);
});

app.Run();
```
- Resolve dependencies in Razor Views & Pages
- ServiceToInject.cs
```c#
namespace ResolvingDeps.Mvc;

public class ServiceToInject
{
    public string Message => "I was injected!";
}
```
- Index.cshtml
```html
@{
    ViewData["Title"] = "Home Page";
}
@inject ServiceToInject ServiceToInject

<div class="text-center">
    <h1 class="display-4">Welcome</h1>
    <p>Learn about <a href="https://docs.microsoft.com/aspnet/core">building Web apps with ASP.NET Core</a>.</p>
    <p>@ServiceToInject.Message</p>
</div>
```