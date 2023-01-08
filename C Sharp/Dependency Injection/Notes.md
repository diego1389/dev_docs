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
- Resolving dependencies in Hosted Services (it uses the constructor).
- Program.cs
```c#
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<BackgroundTicker>();
```
- Background Ticker
```c#
namespace ResolvingDeps.WebApi.HostedServices;

public class BackgroundTicker : BackgroundService
{
    private readonly ILogger<BackgroundTicker> _logger;

    public BackgroundTicker(ILogger<BackgroundTicker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"Hi from {nameof(BackgroundTicker)}");
            await Task.Delay(1000, stoppingToken);
        }
    }
}
```
## Deep dive

- You don't everything needs to be dependency injected. Only the dependencies need to use interfaces. 
- Don't hide business logic inside the mappers. 
- Mappers should not be injected and should not use an interface. Use that only for required dependencies. 
- Program.cs
```c#
builder.Services.AddSingleton<SingletonService>();
builder.Services.AddScoped<ScopedService>();
builder.Services.AddTransient<TransientService>();
```
- SingletonService.cs
```c#
using System;
namespace WeatherApiTest.Services
{
	public class SingletonService
	{
		private readonly TransientService _transientService;

		public SingletonService(TransientService transientService)
		{
			_transientService = transientService;
        }

		//public Guid Id { get; } = Guid.NewGuid();
		public Guid Id => _transientService.Id;
	}
}
```
- Even though the Singleton service inject a transient service it still will return the same instance every single time you call it in the controller (the same id). The constructor will only be called once. 
- lifetimecontroller.cs
```c#
using System;
using Microsoft.AspNetCore.Mvc;
using WeatherApiTest.Filter;
using WeatherApiTest.Services;

namespace WeatherApiTest.Controllers
{
    [ApiController]
    public class LifetimeController : ControllerBase
    {
        //private readonly IdGenerator _idGenerator;

        //public LifetimeController(IdGenerator idGenerator)
        //{
        //    this._idGenerator = idGenerator;
        //}

        private readonly SingletonService _singletonService;
        private readonly TransientService _transientService;
        private readonly ScopedService _scopedService;

        public LifetimeController(ScopedService scopedService, TransientService transientService, SingletonService singletonService)
        {
            _scopedService = scopedService;
            _transientService = transientService;
            _singletonService = singletonService;
        }

        [HttpGet("lifetime")]
        //[ServiceFilter(typeof(LifetimeIndicatorFilter))]
        public IActionResult Get()
        {
            //var id = _idGenerator.Id;
            var ids = new
            {
                SingleonId = _singletonService.Id,
                TransientId = _transientService.Id,
                ScopedId = _scopedService.Id
            };

            return Ok(ids);
        }
    }
}
```
- A circular dependency: ServiceA is injected in constructor in ServiceB and ServiceB is injected in constructor in ServiceA.
- Registering open generics (for example ILoggerAddapter<TType>):
```c#
builder.Services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAddapter<>));
```
- Registering multiple interface implementations. If I register multiple interface implementations it will get the latest registered because it is a queue.
- Program.cs
```c#
builder.Services.AddTransient<IWeatherService, OpenWeatherService>();
builder.Services.AddTransient<IWeatherService, InMemoryWeatherService>();
```
- We can also get an IEnumerable in the constructor to decide which implementation we want to use:
- WeatherForecastController
    ```c#
    [HttpGet(Name = "GetWeatherForecast")]
    public async IEnumerable<WeatherForecast> Get()
    {
        var first = _weatherServices.First();
        WeatherForecast weather = await first.GetCurrentWeatherAsync("London");
    }
    ```
- The ServiceDescriptor describes how a service should be registered. 
```c#
builder.Services.AddTransient<IWeatherService, OpenWeatherService>();

/*Same as: */
var weatherServiceDescriptor =
    new ServiceDescriptor(typeof(IWeatherService), typeof(OpenWeatherService), ServiceLifetime.Transient);

builder.Services.Add(weatherServiceDescriptor);
```
- Add and TryAdd. With TryAdd, if you already something that resolves an interface it will not add it. For example if you already have an OpenWeatherService for IWeatherService it will not add the InMemoryWeatherService implementation. You can also use TryAddTransient, TryAddScoped, TryAddSingleton.

- TryAddEnumerable. 
- Replacing dependencies:
    - Before the builder.Build() you can manipulate the service collection anyway you want. 
    - Remove all dependencies from one type:
    ```c#
    builder.Services.AddTransient<IWeatherService, OpenWeatherService>();
    builder.Services.RemoveAll(typeof(IWeatherService));
    ```
    - You can also remove from an specific index:
    ```c#
    builder.Services.RemoveAt(190);//don't use an specific number
    ```

- Cleaning up service registration
    - Create a new class (Weather Service Registration)
    ```c#
	public static class WeatherServiceRegistration
	{
		public static IServiceCollection AddWeatherServices(this IServiceCollection services)
		{
            //services.AddTransient<IWeatherService, OpenWeatherService>();
            var openWeatherServiceDescriptor =
                new ServiceDescriptor(typeof(IWeatherService), typeof(OpenWeatherService), ServiceLifetime.Transient);

            var weatherServiceDescriptor =
                new ServiceDescriptor(typeof(IWeatherService), typeof(OpenWeatherService), ServiceLifetime.Transient);


            services.Add(weatherServiceDescriptor);
            services.Add(openWeatherServiceDescriptor);

            return services;
        }
    }
    ```
    - Program.cs:
    ```c#
    builder.Services.AddWeatherServices();
    ```
## Advanced techniques

- Creating a custom scope
    - Scope-less services defined as Scoped are treated as singletons. In this example it returns the same Id value:
    ```c#
    using CustomScope.ConsoleApp;
    using Microsoft.Extensions.DependencyInjection;

    var services = new ServiceCollection();

    services.AddScoped<ExampleService>();

    var serviceProvider = services.BuildServiceProvider();

    var exampleService1 = serviceProvider.GetRequiredService<ExampleService>();
    var exampleService2 = serviceProvider.GetRequiredService<ExampleService>();

    Console.WriteLine(exampleService1.Id);
    Console.WriteLine(exampleService2.Id);
    ```
    - To create a new scope:
    ```c#
    using CustomScope.ConsoleApp;
    using Microsoft.Extensions.DependencyInjection;

    var services = new ServiceCollection();

    services.AddScoped<ExampleService>();

    var serviceProvider = services.BuildServiceProvider();
    var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

    using (var serviceScope = serviceScopeFactory.CreateScope())
    {
        var exampleService1 = serviceScope.ServiceProvider.GetRequiredService<ExampleService>();
        Console.WriteLine(exampleService1.Id);
    }

    using (var serviceScope = serviceScopeFactory.CreateScope())
    {
        var exampleService2 = serviceScope.ServiceProvider.GetRequiredService<ExampleService>();
        Console.WriteLine(exampleService2.Id);
    }
    ```
- Service locator anti-pattern.
    - Do not inject the service provider to locate services, instead of that inject the services that you need in each class. 

- When service locator makes sense:
- Create handlers folder:
- IHandler interface
```c#
using System;
namespace MultiFunction.ConsoleApp.Handlers
{
	public interface IHandler
	{
		Task HandleAsync();
	}
}
```
- GetCurrentLondonWeatherHandler
```c#
using System;
using MultiFunction.ConsoleApp.Console;
using MultiFunction.ConsoleApp.Weather;

namespace MultiFunction.ConsoleApp.Handlers
{
    [CommandName("weather")]
    public class GetCurrentLondonWeatherHandler : IHandler
    {
        private readonly IConsoleWriter _consoleWriter;
        private readonly IWeatherService _weatherService;

        public GetCurrentLondonWeatherHandler(IConsoleWriter consoleWriter,
            IWeatherService weatherService)
        {
            _consoleWriter = consoleWriter;
            _weatherService = weatherService;
        }


        public async Task HandleAsync()
        {
            var weather = await _weatherService.GetCurrentWeatherAsync("London");
            double? value = (weather?.Main?.Temp != null) ? weather?.Main?.Temp : 100;

            _consoleWriter.WriteLine($"The temperature in London is {value} C");
        }
    }
}
```
- GetCurrentTimeHandler
```c#
using System;
using MultiFunction.ConsoleApp.Console;
using MultiFunction.ConsoleApp.Time;

namespace MultiFunction.ConsoleApp.Handlers
{
    [CommandName("time")]
    public class GetCurrentTimeHandler : IHandler
    {
        private readonly IConsoleWriter _consoleWriter;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetCurrentTimeHandler(IConsoleWriter consoleWriter,
            IDateTimeProvider dateTimeProvider)
        {
            _consoleWriter = consoleWriter;
            _dateTimeProvider = dateTimeProvider;
        }

        public Task HandleAsync()
        {
            var timeNow = _dateTimeProvider.DateTimeNow;
            _consoleWriter.WriteLine($"The current time is {timeNow:O}");
            return Task.CompletedTask;
        }
    }
}
```
- HandlerOrchestator
```c#
using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MultiFunction.ConsoleApp.Handlers
{
	public class HandlerOrchestator
	{
		private readonly Dictionary<string, Type> _handlerTypes = new();

		private readonly IServiceScopeFactory _serviceScopeFactory;

		public HandlerOrchestator(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
			RegisterCommandHandler();
		}

        public IHandler? GetHandlerForCommandName(string command)
		{
			var handlerType = _handlerTypes.GetValueOrDefault(command);
			if (handlerType is null)
				return null;

			using var serviceScope = _serviceScopeFactory.CreateScope();
			return (IHandler) serviceScope.ServiceProvider.GetRequiredService(handlerType);
		}

        private void RegisterCommandHandler()
        {
			//Assembly scanning...
			var handlerTypes = HandlerExtensions.GetHandlerTypesForAssembly(typeof(IHandler).Assembly);

			foreach (var handlerType in handlerTypes)
			{
				var commandNameAttribute = handlerType.GetCustomAttribute<CommandNameAttribute>();
				if(commandNameAttribute is null)
				{
					continue;
				}
				var commandName = commandNameAttribute.CommandName;
				_handlerTypes[commandName] = handlerType;
			}
        }
    }
}
```
- CommandNameAttribute
```c#
using System;
namespace MultiFunction.ConsoleApp.Handlers
{
	[AttributeUsage(AttributeTargets.Class)]
	public class CommandNameAttribute :Attribute
	{
		public string CommandName { get; set; }

		public CommandNameAttribute(string commandName)
		{
			CommandName = commandName;
		}
	}
}
```
- HandlerExtensions
```c#
using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MultiFunction.ConsoleApp.Handlers
{
	public static class HandlerExtensions
	{
		public static void AddCommandHandlers(this IServiceCollection services, Assembly assembly)
		{
            var handlerTypes = GetHandlerTypesForAssembly(assembly);

            foreach (var handlerType in handlerTypes)
            {
                services.TryAddTransient(handlerType);
            }
		}

        public static IEnumerable<TypeInfo> GetHandlerTypesForAssembly(Assembly assembly)
        {
            var handlerTypes = assembly.DefinedTypes
                .Where(x => !x.IsInterface && !x.IsAbstract
                && typeof(IHandler).IsAssignableFrom(x));

            return handlerTypes;
        }
    }
}
```
- Application.cs
```c#
using MultiFunction.ConsoleApp.Console;
using MultiFunction.ConsoleApp.Handlers;

namespace MultiFunction.ConsoleApp;

public class Application
{
    private readonly IConsoleWriter _consoleWriter;
    private readonly HandlerOrchestator _handlerOrchestator;

    public Application(IConsoleWriter consoleWriter,
        HandlerOrchestator handlerOrchestator)
    {
        _consoleWriter = consoleWriter;
        _handlerOrchestator = handlerOrchestator;
    }

    public async Task RunAsync(string[] args)
    {
        var command = args[0];

        var handler = _handlerOrchestator.GetHandlerForCommandName(command);
        if (handler == null)
        {
            _consoleWriter.WriteLine($"No handler found for command name {command}");
            return;
        }
        await handler.HandleAsync();
    }
}
```
- Program.cs
```c#
using MultiFunction.ConsoleApp.Console;
using MultiFunction.ConsoleApp.Handlers;

namespace MultiFunction.ConsoleApp;

public class Application
{
    private readonly IConsoleWriter _consoleWriter;
    private readonly HandlerOrchestator _handlerOrchestator;

    public Application(IConsoleWriter consoleWriter,
        HandlerOrchestator handlerOrchestator)
    {
        _consoleWriter = consoleWriter;
        _handlerOrchestator = handlerOrchestator;
    }

    public async Task RunAsync(string[] args)
    {
        var command = args[0];

        var handler = _handlerOrchestator.GetHandlerForCommandName(command);
        if (handler == null)
        {
            _consoleWriter.WriteLine($"No handler found for command name {command}");
            return;
        }
        await handler.HandleAsync();
    }
}
```
- Avoid capturing dependencies:
    - Even if IWeatherService was registered as Transient if you call it outside of the MapGet scope it will be resolved as a singleton. TO handle it as trasient inject IWeatherService in the MapGetScope:
    ```c#
    //var weatherService = app.Services.GetRequiredService<IWeatherService>();

    app.MapGet("weather/{city}",
        async ([FromRoute] string city, IWeatherService weatherService) =>
    {
        var weather = await weatherService.GetCurrentWeatherAsync(city);
        return weather == null ? Results.NotFound() : Results.Ok(weather);
    });
    ```
- Avoid multiple service providers. 
- Creating decorators. Refactor the following code using DI and decorator pattern (it breaks single responsability principle).
```c#
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace MultiFunction.ConsoleApp.Weather;

public class OpenWeatherService : IWeatherService
{
    private const string OpenWeatherApiKey = "f539ebbe9ad5228403f6c267b7b7743c";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<OpenWeatherService> _logger;

    public OpenWeatherService(IHttpClientFactory httpClientFactory,
        ILogger<OpenWeatherService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<WeatherResponse?> GetCurrentWeatherAsync(string city)
    {
        var sw = Stopwatch.StartNew();

        try
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={OpenWeatherApiKey}&units=metric";
            var _httpClient = _httpClientFactory.CreateClient();
            var weatherResponse = await _httpClient.GetAsync(url);
            if (weatherResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            var weather = await weatherResponse.Content.ReadFromJsonAsync<WeatherResponse>();
            return weather;
        }
        finally
        {
            sw.Stop();
            _logger.LogInformation("Weather retrieval for city {0}, {1} ms", city, sw.ElapsedMilliseconds);
        }      
    }
}
```
- Add decorator class:
    - LoggedWeatherService.cs
    ```c#
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using Microsoft.Extensions.Logging;
    using Weather.Api.Weather;

    namespace Weather.Api.Weather;

    public class LoggedWeatherService : IWeatherService
    {
        private readonly IWeatherService _weatherService;  //OpenWeatherService
        private readonly ILogger<IWeatherService> _logger;

        public LoggedWeatherService(IWeatherService weatherService, ILogger<IWeatherService> logger)
        {
            _weatherService = weatherService;
            _logger = logger; 
        }

        public async Task<WeatherResponse> GetCurrentWeatherAsync(string city)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                return await _weatherService.GetCurrentWeatherAsync(city);
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation("Weather retrieval for city {0}, {1} ms", city, sw.ElapsedMilliseconds);
            }
        }
    }
    ```
    - Change OpenWeatherService class:
    ```c#
    using System.Diagnostics;
    using System.Net;

    namespace Weather.Api.Weather;

    public class OpenWeatherService : IWeatherService
    {
        private const string OpenWeatherApiKey = "f539ebbe9ad5228403f6c267b7b7743c";
        private readonly IHttpClientFactory _httpClientFactory;

        public OpenWeatherService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<WeatherResponse?> GetCurrentWeatherAsync(string city)
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={OpenWeatherApiKey}&units=metric";
            var _httpClient = _httpClientFactory.CreateClient();
            var weatherResponse = await _httpClient.GetAsync(url);
            if (weatherResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            var weather = await weatherResponse.Content.ReadFromJsonAsync<WeatherResponse>();
            return weather;
        }
    }
    ```
    - Change Program.cs
    ```c#
    builder.Services.AddTransient<OpenWeatherService>();
    builder.Services.AddTransient<IWeatherService>(provider =>
        new LoggedWeatherService(provider.GetRequiredService<OpenWeatherService>(),
            provider.GetRequiredService<ILogger<IWeatherService>>()));
    ```
- The future of dependency injection. 
    - Install Jab nuget package
    - MyServiceProvider.cs
    ```c#
    using System;
    using Jab;

    namespace DependencyInjectionFuture.ConsoleApp
    {
        [ServiceProvider]
        [Transient(typeof(IConsoleWriter), typeof(ConsoleWriter))]
        public partial class MyServiceProvider
        {

        }
    }
    ```
    - Program.cs
    ```c#
    using DependencyInjectionFuture.ConsoleApp;

    Console.WriteLine();

    var serviceProvider = new MyServiceProvider();
    var consoleWriter = serviceProvider.GetService<IConsoleWriter>();

    consoleWriter.WriteLine("Hi From Source generated DI");
    ```
## Extending Dependency Injection with Scrutor

- Scrutor library to extend the built-in DI container with extra functionality. 
- Install Scrutor and change the previous decorator Program.cs configuration:
    ```c#
    builder.Services.AddTransient<IWeatherService, OpenWeatherService>();
    //builder.Services.AddTransient<IWeatherService>(provider =>
    //    new LoggedWeatherService(provider.GetRequiredService<OpenWeatherService>(),
    //        provider.GetRequiredService<ILogger<IWeatherService>>()));
    builder.Services.Decorate<IWeatherService, LoggedWeatherService>();
    ```
- Refactoring lecture (bette way to handle the Stopwatch approach from previous lessons).
- Create TimedLogOperation class:
```c#
using System;
using System.Diagnostics;

namespace Weather.Api.Logging
{
    public class TimedLogOperation<T> : IDisposable
    {
        private readonly ILoggerAdapter<T> _logger;
        private readonly LogLevel _logLevel;
        private readonly string _message;
        private readonly object?[] _args;
        private readonly Stopwatch _stopwatch;

        public TimedLogOperation(ILoggerAdapter<T> logger,
            LogLevel logLevel,
            string message,
            object?[] args)
        {
            _logger = logger;
            _logLevel = logLevel;
            _message = message;
            _args = args;
            _stopwatch = Stopwatch.StartNew();
        }
    
        public void Dispose()
        {
            _stopwatch.Stop();
            _logger.Log(_logLevel, $"{_message} completed in {_stopwatch.ElapsedMilliseconds} ms", _args);
        }
    }
}
```
- Add a new method to ILoggerAdapter and implement it
```c#
namespace Weather.Api.Logging;

public interface ILoggerAdapter<TType>
{
    void Log(LogLevel logLevel, string template, params object[] args);

    void LogInformation(string template, params object[] args);

    IDisposable TimedOperation(string template, params object[] args); 
}
```
```c#
namespace Weather.Api.Logging;

public class LoggerAdapter<TType> : ILoggerAdapter<TType>
{
    private readonly ILogger<LoggerAdapter<TType>> _logger;

    public LoggerAdapter(ILogger<LoggerAdapter<TType>> logger)
    {
        _logger = logger;
    }

    public void Log(LogLevel logLevel, string template, params object[] args)
    {
        _logger.Log(logLevel, template, args);
    }

    public void LogInformation(string template, params object[] args)
    {
        Log(LogLevel.Information, template, args);
    }

    public IDisposable TimedOperation(string template, params object[] args)
    {
        return new TimedLogOperation<TType>(this, LogLevel.Information, template, args);
    }
}
```
- Change LoggedWeatherService:
```c#
using System.Diagnostics;
using Weather.Api.Logging;

namespace Weather.Api.Weather;

public class LoggedWeatherService : IWeatherService
{
    private readonly IWeatherService _weatherService; //<-- OpenWeatherService
    private readonly ILoggerAdapter<IWeatherService> _logger;

    public LoggedWeatherService(IWeatherService weatherService,
        ILoggerAdapter<IWeatherService> logger)
    {
        _weatherService = weatherService;
        _logger = logger;
    }

    public async Task<WeatherResponse?> GetCurrentWeatherAsync(string city)
    {
        using var _ = _logger.TimedOperation("Weather retrieval for city: {0}", city);
        return await _weatherService.GetCurrentWeatherAsync(city);
    }
}
```
- Service registration by scanning. Application on startup will scan the code and automatically register services. 
- Include Microsoft.Extensions.DependencyInjection and Scrutor.
- To scan all the classes of the current project (console.app) project and resolve them with its matching interface (IExampleA -> ExampleA, etc) do the following:
```c#
using Microsoft.Extensions.DependencyInjection;
using ScrutorScanning.ConsoleApp.Services;

var services = new ServiceCollection();

var serviceProvider = services.BuildServiceProvider();

services.Scan(selector =>
{
    selector.FromAssemblyOf<Program>()
        .AddClasses(f => f.InExactNamespaces("ScrutorScanning.ConsoleApp.Services"))
        .AsMatchingInterface();
});

PrintRegisteredService(services);

void PrintRegisteredService(IServiceCollection serviceCollection)
{
    foreach (var service in serviceCollection)
    {
        Console.WriteLine($"{service.ServiceType.Name} -> {service.ImplementationType?.Name} as {service.Lifetime.ToString()}");
    }
}
```
- You can also use as ImplementedInterface() and it will resolve the service with its interface regardless of the service name. It will check only if the service class implements the interface. 
- The default is Transient. You can override that:
```c#
using Microsoft.Extensions.DependencyInjection;
using ScrutorScanning.ConsoleApp.Services;

var services = new ServiceCollection();

var serviceProvider = services.BuildServiceProvider();

services.Scan(selector =>
{
    selector.FromAssemblyOf<Program>()
        .AddClasses(f => f.InExactNamespaces("ScrutorScanning.ConsoleApp.Services"))
        .AsImplementedInterfaces()
        .WithSingletonLifetime();
});

PrintRegisteredService(services);

void PrintRegisteredService(IServiceCollection serviceCollection)
{
    foreach (var service in serviceCollection)
    {
        Console.WriteLine($"{service.ServiceType.Name} -> {service.ImplementationType?.Name} as {service.Lifetime.ToString()}");
    }
}
/*IExampleAService -> ExampleAService as Singleton
IExampleBService -> ExampleBService as Singleton
IExampleCService -> ExampleCService as Singleton */
```
- You can also cange the filter:
```c#
using Microsoft.Extensions.DependencyInjection;
using ScrutorScanning.ConsoleApp.Services;

var services = new ServiceCollection();

var serviceProvider = services.BuildServiceProvider();

services.Scan(selector =>
{
    selector.FromAssemblyOf<Program>()
        .AddClasses(f => f.Where(t => t.Name.EndsWith("Service")))
        .AsImplementedInterfaces()
        .WithSingletonLifetime();
});

PrintRegisteredService(services);

void PrintRegisteredService(IServiceCollection serviceCollection)
{
    foreach (var service in serviceCollection)
    {
        Console.WriteLine($"{service.ServiceType.Name} -> {service.ImplementationType?.Name} as {service.Lifetime.ToString()}");
    }
}
```
- You can chain AddClasses multiple times and also more than one FromAssemblyOf to use multiple assemblies. 
