## Building your first ASP.Net Core Application
- Create ASP.Net core application
- Select Asp.net core > 2.0
- Empty
- You can create an  ASP.net OS you can create a project using command line. 
- You can right click and edit .csproj
    1. Has the target framework
    2. Item group: nuget packages. 
- With .Net core the file system determines what is inside of my project. The projects are now based in the file system. 
- **Program.cs**
    - Static void main entry point. That's because the structured as a console mode application (that's why you can run it from the console).
    - It will place the web host behind IIS Express which is going to forward requests into my application.
    - But my application is a separate process that is up and running and has its own web server configured in BuildWebHost (in Microsoft.AspNetCore.Hosting).
        ```cs
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
        ```
  
- **Startup.cs**
    - Configure how the application behaves.
    - Configure method configures the http processuing pipeline. Each http request will find the response to that request in this method. 
    - You can customize the Configure method to use MVC.
    - There is no global.asax nor web.config. 
    - There is no configuration to hold items like database connection string, etc. 
      - A web host builder (in Program.cs) is an object that knows how to set up our web server environment.
        1. By default it will use Kestrel web server.
        2. Sets up IIS integration.
        3. Logging
        4. IConfiguration service made available through:
            1. appsettings.json
            2. User secrets
            3. Environment variables
            4. Command line arguments. 
    - Create appsettings.json into the root file (it will have the database connection string).
    
        ```javascript
        {
            "Greeting" : "Hello!!",
            "ConnectionStrings": {
                "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=_CHANGE_ME;Trusted_Connection=True;MultipleActiveResultSets=true"
            }
        }
        ```
    - To add a custom message add the key in the configuration file and then modify the Configure method:
        ```cs
         // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env,
            IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                var greeting = configuration["Greeting"];
                await context.Response.WriteAsync(greeting);
            });
        }
        ```
    - If there is a an Environment vaiables called Greeting it will override the json file variable.
    - You can implement a custom message using (creating) an IGreeter interface but first you need to inject a service that implements IGreeter. 
        ```cs
        namespace OdeToFood
        {
            public interface IGreeter
            {
                string GetMessageOfTheDay();
            }

            public class Greeter : IGreeter
            {
                public string GetMessageOfTheDay()
                {
                    return "Greetings!";
                }
            }
        }
        ```
    - You have to register the service for custom interfaces in the ConfigureServices methods for itself and also for other areas in the .Net core application. 
    - You can choose AddSingleton to say .Net core to use only one instance of the service for the entire application.
    - Add Transient (create an instance any time someone requires the service).
    - Add Scope, .Net core will create an instance per http request. 
        ```cs
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGreeter, Greeter>(); //TService, implementation
        }
        ```
    - Dependency injection: dependency is passed to me instead of me creating it. 

## Startup and Middleware

- Middleware controls how our application responds to HTTP requests.
- Middleware is a series of controllers that make our application behave a specific way and respond to http requests. 
- Also how we display error information and a key piece of how we authenticate and authorize a user to perform specific actions. 
- For example: Logger -> Authorizer -> Router  
               Logger <- Authorizer <- Router  
- In Startup Configure() method we configure the middleware. 
- An example of middleware:
    ```cs
    app.UseDeveloperExceptionPage();
    ```
- Another one is app.Run();
- app. and get list of extension methods of the IApplicationBuilder where the middleware is.
    ```cs
    public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env,
            IGreeter greeter)
        {
            app.UseWelcomePage(new WelcomePageOptions {
                Path="/wp"
            }); /*Without welcomepageoptions every page goes to the welcome page*/
            ...
        }
    ```
- To customize the middleware section:
    ```cs
    app.Use(next =>
                {
                    //http context object
                    return async context => 
                    {
                        logger.LogInformation("Request incoming");
                        if (context.Request.Path.StartsWithSegments("/mym"))
                        {                        
                            await context.Response.WriteAsync("Hit!");
                            logger.LogInformation("Request handled");
                        }
                        else
                        {
                            await next(context); //forward to next middleware
                            logger.LogInformation("Response incoming");
                        }
                    };
                }
            );
    ```
    - You can check the loggin in the output window (view -> output) and asp.net core server option.
    - The developer exception page allows every request to flow throught it but when another piece of middleware throws and exception it catches and handle it.
    ```cs
     if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }/*Displays a user developer page with details about the exception but only in development mode*/
    ```
- IHostingEnvironment service has information about the application and I can inject it in different parts of my application. For example: Application name, absolute path, if it is development, staging or production, etc.  
- Commonly you set the environment using an environment variable named ASPNETCORE_ENVIRONMENT
- In visual studio there is a launchSettings.json file with the ASPNETCORE_ENVIRONMENT variable. If you change Development to Production it will not display the developerexceptionpage because env.IsDevelopment() returns false. 

    ```javascript
    "profiles": 
    {
        "IIS Express": {
        "commandName": "IISExpress",
        "launchBrowser": true,
        "environmentVariables": {
            "ASPNETCORE_ENVIRONMENT": "Development"
        }
        },
        "OdeToFood": {
        "commandName": "Project",
        "launchBrowser": true,
        "applicationUrl": "https://localhost:5001;http://localhost:5000",
        "environmentVariables": {
            "ASPNETCORE_ENVIRONMENT": "Development"
        }
    }
    ```
- You can create an appSettings.Development.json file to override the default appSettings.json for a particular environment. You can use different db connections strings for different environments or create a different greeting for a development environment. 

## Serving files

- In ASP.net core you cannot serve any file from the file system (js, html, css) without adding a piece of middleware. By default it will only serve files from wwwroot folder.
- Create an html in wwwroot and add this middleware to navigate to static pages:
    ```cs
    app.UseStaticFiles();
    ```
- In order to make an index.html as a default file for incoming requests.
     ```cs
     app.UseDefaultFiles();
    ```

## Setting up ASP.Net MVC 

 - You need to add MVC services and middleware to receive requests. 
 - Middleware takes the request and send it to a C# class (controller).
 - Add the middleware and services:
    ```cs
      public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGreeter, Greeter>(); //TService, implementation
            services.AddMvc();
        }

        /*This method gets called by the runtime. Use this method to configure the HTTP request pipeline.*/
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env,
            IGreeter greeter,
            ILogger<Startup> logger)
        {
            //...
            app.UseMvcWithDefaultRoute();
        }
    ```

## Routing

- Routing: sending an http request to a controller.
- The Asp.net core middleware we installed needs a way to determine if an http request should go to a controller for processing or not, depending on the url and the routing configuration information. 
- We can write the routing configuration inside our Startup.cs or use Attribute routes.  
- If you use UseMVC() not with DefaultRoute you need to pass an IRouteBuilder. 
    ```cs
    app.UseMvc(ConfigureRoutes);
    ```
- Create a method that takes IRouteBuilder:
    ```cs
            private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            //Home/index
            routeBuilder.MapRoute("Default", 
                "{controller}/{action}/{id?}");
        }
    ```
- To set a default action:
    ```cs
        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            //Home/index
            routeBuilder.MapRoute("Default", 
                "{controller=Home}/{action=Index}/{id?}");
        }
    ```
- You can also use route attribute and they can be applied at a controller level or an action level. 
- You can combine attribute routes and route builder templates to enhance functionality.
- If you want your default functionality but in the case of the about controller you want to be Phone the default action you can:
    ```cs
    [Route("about")]
    public class AboutController
    {
        [Route("")]
        public string Phone()
        {
            return "84734166";
        }

        [Route("address")]
        public string Address()
        {
            return "Cartago";
        }
    }
    ```
- A token:
    ```cs
    //it knows it is about in this case
    [Route("[controller]")]
    public class AboutController
    {
    ```
- Attribute routes are useful for behaviours specific to a controller or an action inside that controller.

## Action results

- Normally controllers inherit from Controller so you can have contextual information to use (IActionResult interface).
- With this you can have access to the httpContext (the same you use inside the middleware).
    ```cs
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("Hello from the HomeController");
        }
    }
    ```
- IActionResult has all the result helpers: Content, File, BadRequest, etc. You can return one specific  or IActionResult in case you can return different helpers from the Action.

```cs
public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new Restaurant() { Id = 1, Name = "Scotts Pizza Place"};

            return new ObjectResult(model);
        }
    }
```
- What ObjectResult is doing is content negotiation (it displays a JSON in the browser).

## Rendering views

- The most popular way to deliver html is using Razor view engine. 
- The controller decides to produce a ViewResult 
- A view will be a file in the file system (.cshtml extension).
- The View() method will find a view with the same name as the controller unless you pass a string with the viewname. F.e: View("Home");
- Add Views folder and a folder for each controller where will be located its views. 
- You can pass the model to the view
```cs
        public IActionResult Index()
        {
            var model = new Restaurant() { Id = 1, Name = "Scotts Pizza Place"};

            return View(model);
        }
```
- In the view:
```html
    <h1>@Model.Name</h1>
```
- To get intellise use a model directive (@model) which is information the Razor view engine needs to construct the code behind the scenes.

```html
@model OdeToFood.Models.Restaurant
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
    <h1>@Model.Name</h1>
    <h3>The id value is: @Model.Id</h3>
</body>
</html>
```

- A list is not thread safe. 
- IRestaurantData interface and InMemoryRestaurantData class in the services folder:

```cs
  public interface IRestaurantData
    {
        IEnumerable<Restaurant> GetAll();
    }
```

```cs
public class InMemoryRestaurantsData : IRestaurantData
    {
        public InMemoryRestaurantsData()
        {
            _restaurants = new List<Restaurant>()
            {
                new Restaurant(){Id = 1, Name="Scotts Pizza Place"},
                new Restaurant(){Id = 2, Name="Tersiguels"},
                new Restaurant(){Id = 3, Name="King's Contrivance"}
            };
        }

        public IEnumerable<Restaurant> GetAll()
        {
            return _restaurants.OrderBy(r => r.Id);
        }

        List<Restaurant> _restaurants;
    }
```

- Subscribe the service in Startup.cs
```cs
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IRestaurantData, InMemoryRestaurantsData>();
            ...
```
- Then change the model in the controller:
```cs
public class HomeController : Controller
    {
        private IRestaurantData _restaurantData;

        public HomeController(IRestaurantData restaurantData)
        {
            _restaurantData = restaurantData;
        }

        public IActionResult Index()
        {
            var model = _restaurantData.GetAll();

            return View(model);
        }
    }
```
- Change the view:

```html
@model IEnumerable<OdeToFood.Models.Restaurant>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
    <table>
        @foreach (var restaurant in Model)
        {
            <tr>
                <td>@restaurant.Id</td>
                <td>@restaurant.Name</td>
            </tr>
        }
    </table>
</body>
</html>
```

## Models and view models

- An entity is an object that I persist in the database.
- An entity model looks like my database schema. 
- A viewmodel is an object that I use to carry information between a view and a controller, it contains anything that a view to render html. Normally the model needs to make multiple database queries and pull back multiple entities and then place all the information in the ViewModel. It is a DTO because it carries information around but it does not persist it in the database. 

- Create a ViewModel for the information needed in the methods of the controller. An object that encapsulates everything that the Index action of the home controller needs. 

- Add ViewModel:
```cs
namespace OdeToFood.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Restaurant> Restaurants { get; set; }
        public string CurrentMessage { get; set; }
    }
}
```
- Change controller:

```cs
  public class HomeController : Controller
    {
        private IGreeter _greeter;
        private IRestaurantData _restaurantData;

        public HomeController(IRestaurantData restaurantData,
            IGreeter greeter)
        {
            _greeter = greeter;
            _restaurantData = restaurantData;
        }

        public IActionResult Index()
        {
            var model = new HomeIndexViewModel();
            model.Restaurants = _restaurantData.GetAll();
            model.CurrentMessage = _greeter.GetMessageOfTheDay();
            return View(model);
        }
    }
```

- Change the view to consume the viewmodel instead of the model:
```html
@model OdeToFood.ViewModels.HomeIndexViewModel
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
    <h1>@Model.CurrentMessage</h1>
    <table>
        @foreach (var restaurant in Model.Restaurants)
        {
            <tr>
                <td>@restaurant.Id</td>
                <td>@restaurant.Name</td>
            </tr>
        }
    </table>
</body>
</html>
```
## Input models - detail a restaurant

- To display details add a new controller  method:

```cs
 public IActionResult Details(int id)
        {
            var model = _restaurantData.Get(id);
            return View(model);
        }
```
- Modify the interface IRestaurantData and its implementation:
```cs
 public interface IRestaurantData
    {
        IEnumerable<Restaurant> GetAll();
        Restaurant Get(int id);
    }
```
```cs
        public Restaurant Get(int id)
        {
            return _restaurants.FirstOrDefault(r => r.Id == id);
        }

```
- Create new view for the details:

```html
@model OdeToFood.Models.Restaurant

<h1>@Model.Name</h1>
<h2>Details...</h2>
```
- If the object is null it is better to handle it in the controller, maybe adding a different view for not found. Do not create this logic inside de view, only presentation logic.

- In api you can return a NotFound() but in web applications you normally want to create html to inform the client that something wrong happened. 
- I can also redirect to another action if model not found:
```cs
 public IActionResult Details(int id)
        {
            var model = _restaurantData.Get(id);
            if(model == null)
            {
                  return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
```
- You can add additional parameters in the redirect to action to call actions in different controllers. 
- Instead of creating an anchor or a helper method with an action link you can use tag helpers.
- First need to import RazorImports as a special razor view that does not render anything. Add a special directive in that new file:
```html
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, AuthoringTagHelpers
```
- Update the view to use the tag helpers:
```html
    <table>
        @foreach (var restaurant in Model.Restaurants)
        {
            <tr>
                <td>@restaurant.Id</td>
                <td>@restaurant.Name</td>
                <td>
                    <a asp-action="Details" asp-route-id="@restaurant.Id">More</a>                 
                </td>
            </tr>
        }
    </table>
```
- To go back to the Index action update the Details view:
```html
 <a asp-action="Index" asp-controller="Home">Go to restaurants menu</a>
```

## Create a restaurant

- To create a restaurant we need an action that returns a view with a form. 
- Add action tag helper in the index view below the table:
```html
<a asp-action="Create">Add restaurant</a>
```
- Create an enum in the Models folder:
```cs
public enum CuisineType
    {
        None,
        Italian,
        French,
        German
    }
```
- Add a property in the Restaurant entity:
```cs
public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CuisineType Cuisine { get; set; }
    }
```