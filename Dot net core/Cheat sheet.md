# The complete guide to build restful apis with asp.net core.

- Layout to configure the navigation bar, bootstrap, etc. 
- wwwroot folder to store static files (html, cs, javascript).
- appsettings.json to store db connection strings. Anything defined here can be accessed on runtime. 
- Program.cs: entry point of the application. Main method with a build to create a host for the application. It also defines the Startup class (Startup.cs).
- Startup.cs:
    - ConfigureService
        - Called by the runtime. 
        - Use to add services to the container. 
        - Dependency injection pattern. 
    - Configure
- Views and models:
    - List.cshtml.cs
    ```c#
    namespace QuotesApi.Pages.Restaurants
    {
        public class ListModel : PageModel
        {
            public string Message { get; set; }
            public void OnGet()
            {
                Message = "Hello world";
            }
        }
    }
    ```
    - List.cshtml
    ```html
    @page
    @model QuotesApi.Pages.Restaurants.ListModel
    @{
    }

    <h1>Restaurant</h1>

    @Model.Message
    ```
- Get message from appsettings.json:
    ```json
    {
    "Logging": {
        "LogLevel": {
        "Default": "Information",
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
        }
    },
    "AllowedHosts": "*",
    "Message": "Hello from appsettings"
    }
    ```
- List.cshtml.cs
    ```c#
    public class ListModel : PageModel
    {
        private readonly IConfiguration configuration;
        public ListModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string Message { get; set; }
        public void OnGet()
        {
            Message = configuration["Message"];
        }
    }
    ```
- Add interface and in memory data in new data project:
    ```c#
    using System.Collections.Generic;
    using OdeToFood.Core;
    using System.Linq;

    namespace OdeToFood.Data
    {
        public interface IRestaurantData
        {
            IEnumerable<Restaurant> GetAll();
        }

        public class InMemoryRestaurant : IRestaurantData
        {
            List<Restaurant> restaurants;
            public InMemoryRestaurant()
            {
                restaurants = new List<Restaurant>
                {
                    new Restaurant
                    {
                        Id = 1,
                        Name = "Scotts Pizza",
                        Location = "Maryland",
                        Cuisine = CuisineType.Italian
                    },
                    new Restaurant
                    {
                        Id = 2,
                        Name = "Cinnamon Club",
                        Location = "San Francisco",
                        Cuisine = CuisineType.None
                    },
                    new Restaurant
                    {
                        Id = 3,
                        Name = "Joe's Tacos",
                        Location = "New Mexico",
                        Cuisine = CuisineType.Mexican
                    }
                };
            }
            public IEnumerable<Restaurant> GetAll()
            {
                return from restaurant in restaurants
                    orderby restaurant.Name
                    select restaurant;
            }
        }
    }
    ```
- Configure project for dependency injection (use IRestaurantData) in Startup.cs ConfigureServices method:'
    - Only do this in development, list is not thread safe. 
    ```c#
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IRestaurantData, InMemoryRestaurant>();
        services.AddRazorPages();
    }
    ```
- Building the page model:
    -List.cshtml.cs
    ```c#
    public class ListModel : PageModel
    {
        private readonly IConfiguration configuration;
        private readonly IRestaurantData restaurantData;

        public ListModel(IConfiguration configuration,
            IRestaurantData restaurantData)
        {
            this.configuration = configuration;
            this.restaurantData = restaurantData;
        }

        public string Message { get; set; }
        public IEnumerable<Restaurant> Restaurants { get; set; }
        public void OnGet()
        {
            Message = configuration["Message"];
            Restaurants = restaurantData.GetAll();
        }
    }
    ```
- Display the information in the view:
    - List.cshtml
    ```html
    @page
    @model QuotesApi.Pages.Restaurants.ListModel
    @{
    }

    <h1>Restaurants</h1>
    <table class="table">
        @foreach(var restaurant in Model.Restaurants)
        {
        <tr>
            <td>
                @restaurant.Name
            </td>
            <td>
                @restaurant.Location
            </td>
            <td>
                @restaurant.Cuisine
            </td>
        </tr>
        }
    </table>
    ```

## Working with models and model binding

- Building a search form:
    ```html
    @page
    @model QuotesApi.Pages.Restaurants.ListModel
    @{
    }

    <h1>Restaurants</h1>
    <form method="get" >
        <div class="form-group">
            <div class="input-group">
                <input type="search"
                        class="form-control"
                        value=""
                        name="searchTerm"/>
                <span class="input-group-btn">
                    <button class="btn btn-default">
                        Search
                    </button>
                </span>
            </div>
        </div>
    </form>
    <table class="table">
        @foreach(var restaurant in Model.Restaurants)
        {
        <tr>
            <td>
                @restaurant.Name
            </td>
            <td>
                @restaurant.Location
            </td>
            <td>
                @restaurant.Cuisine
            </td>
        </tr>
        }
    </table>
    <div>@Model.Message</div>
    ````
    - List.cshtml.cs
    ```c#
    public void OnGet(string searchTerm)
    {
        Message = configuration["Message"];
        Restaurants = restaurantData.GetRestaurantsByName(searchTerm);
    }
    ```
- Use [BindProperty] to use a variable as an input and output variable. By default it works just for Post operations but you can add SupportGet. 
    - List.cshtml
    ```html
    <form method="get" >
        <div class="form-group">
            <div class="input-group">
                <input type="search"
                        class="form-control"
                        asp-for="SearchTerm"/> <!--Without @ (we're already working with instance of model)-->
                <span class="input-group-btn">
                    <button class="btn btn-default">
                        Search
                    </button>
                </span>
            </div>
        </div>
    </form>
    ```
    - List.cshtml.cs
    ```c#
    [BindProperty(SupportsGet =true)]
    public string SearchTerm { get; set; }
    public void OnGet()
    {
        Message = configuration["Message"];
        Restaurants = restaurantData.GetRestaurantsByName(SearchTerm);
    }
    ```
- Add detail view:
    - Detal.cshtml
    ```html
    @page
    @model QuotesApi.Pages.Restaurants.DetailModel
    @{
    }
    <h2>@Model.Restaurant.Name</h2>
    <div>
        Id: @Model.Restaurant.Id
    </div>
    <div>
        Location: @Model.Restaurant.Location
    </div>
    <div>
        Cuisine: @Model.Restaurant.Cuisine
    </div>
    <a asp-page="./List" class="btn bnt-default">All restaurants</a>
    ```
- Modify the list to send the restaurantId:
    -List.cshtml
    ```html
    <table class="table">
    @foreach(var restaurant in Model.Restaurants)
    {
    <tr>
        <td>
            @restaurant.Name
        </td>
        <td>
            @restaurant.Location
        </td>
        <td>
            @restaurant.Cuisine
        </td>
        <td>
            <a class="btn btn-default"
               asp-page="./Detail" asp-route-restaurantId="@restaurant.Id">
                Details
            </a>
        </td>
    </tr>
    }
    </table>
    ```
- If you type https://localhost:5001/Restaurants/Detail/5 you get an error. You need to specify that the route requires a restaurantId parameter:
    - Details.cshtml
    ```html
    @page "{restaurantId:int}"
    @model QuotesApi.Pages.Restaurants.DetailModel
    @{
    }
    <h2>@Model.Restaurant.Name</h2>
    <div>
        Id: @Model.Restaurant.Id
    </div>
    <div>
        Location: @Model.Restaurant.Location
    </div>
    <div>
        Cuisine: @Model.Restaurant.Cuisine
    </div>
    <a asp-page="./List" class="btn bnt-default">All restaurants</a>
    ```
- Show details 
    -Details.cshtml.cs
    ```c#
    public class DetailModel : PageModel
    {
        private readonly IRestaurantData restaurantData;
        public DetailModel(IRestaurantData restaurantData)
        {
            this.restaurantData = restaurantData;
        }
        public Restaurant Restaurant { get; set; }
        public void OnGet(int restaurantId)
        {
            Restaurant = restaurantData.GetById(restaurantId);
        }
    }
    ```
- Handling bad request:
    - Modify the OnGet() method to return an IActionResult.
    - Detail.cshtml.cs
    ```c#
    public IActionResult OnGet(int restaurantId)
    {
        Restaurant = restaurantData.GetById(restaurantId);
        if(Restaurant == null)
        {
            return RedirectToPage("./NotFound");
        }
        return Page();
    }
    ```
## Editing data with Razor pages

- Add a new Edit view:
    - Edit.cshtml
    ```html
    @page "{restaurantId:int}"
    @model QuotesApi.Pages.Restaurants.EditModel
    @{
    }
    <h2>Editing @Model.Restaurant.Name</h2>
    <form method="post">
        <input type="hidden" asp-for="Restaurant.Id" />
        <div class="form-group">
            <label asp-for="Restaurant.Name"></label>
            <input asp-for="Restaurant.Name" class="form-control " />
        </div>
        <div class="form-group">
            <label asp-for="Restaurant.Location"></label>
            <input asp-for="Restaurant.Location" class="form-control " />
        </div>
        <div class="form-group">
            <label asp-for="Restaurant.Cuisine "></label>
            <select class="form-control" asp-for="Restaurant.Cuisine" asp-items="Model.Cuisines"></select>
        </div>
        <button type="submit" class="btn btn-primary">Save</button>
    </form>
    ```
    - Edit.cshtml.cs
    ```cs
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OdeToFood.Core;
    using OdeToFood.Data;

    namespace QuotesApi.Pages.Restaurants
    {
        public class EditModel : PageModel
        {
            private readonly IRestaurantData restaurantData;
            private readonly IHtmlHelper htmlHelper;

            public Restaurant Restaurant  { get; set; }
            public IEnumerable<SelectListItem> Cuisines { get; set; }

            public EditModel(IRestaurantData restaurantData,
                IHtmlHelper htmlHelper)
            {
                this.restaurantData = restaurantData;
                this.htmlHelper = htmlHelper;
            }

            public IActionResult OnGet(int restaurantId)
            {
                Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
                Restaurant = restaurantData.GetById(restaurantId);
                if(Restaurant == null)
                {
                    return RedirectToPage("./NotFound");
                }
                return Page();
            }
        }
    }
    ```
- Post a new resturant.
    - Edit.cshtml.cs
    ```c#
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OdeToFood.Core;
    using OdeToFood.Data;

    namespace QuotesApi.Pages.Restaurants
    {
        public class EditModel : PageModel
        {
            private readonly IRestaurantData restaurantData;
            private readonly IHtmlHelper htmlHelper;

            [BindProperty]
            public Restaurant Restaurant  { get; set; }
            public IEnumerable<SelectListItem> Cuisines { get; set; }

            public EditModel(IRestaurantData restaurantData,
                IHtmlHelper htmlHelper)
            {
                this.restaurantData = restaurantData;
                this.htmlHelper = htmlHelper;
            }

            public IActionResult OnGet(int restaurantId)
            {
                Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
                Restaurant = restaurantData.GetById(restaurantId);
                if(Restaurant == null)
                {
                    return RedirectToPage("./NotFound");
                }
                return Page();
            }

            public IActionResult OnPost()
            {
                Restaurant = restaurantData.Update(Restaurant);
                restaurantData.Commit();
                return Page(); 
            }
        }
    }
- You can add validations as DataAnnotations.
    - You can also implement IValidatableObject for more complex validations
    ```c#
    using System.ComponentModel.DataAnnotations;
    namespace OdeToFood.Core
    {
        public class Restaurant
        {
            public int Id { get; set; }
            [Required, MaxLength(80)]
            public string Name  { get; set; }
            [Required, MaxLength(256)]
            public string Location { get; set; }
            public CuisineType Cuisine { get; set; }
        }
    }
    ```
- Verify validation errors with ModelState:
    ```c#
     public IActionResult OnPost()
    {
        Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
        if (ModelState.IsValid)
        {
            restaurantData.Update(Restaurant);
            restaurantData.Commit();
        }
        return Page(); 
    }
    ```
- Show validation errors to the user:
    - Edit.cshtml
    ```html
    <form method="post">
        <input type="hidden" asp-for="Restaurant.Id" />
        <div class="form-group">
            <label asp-for="Restaurant.Name"></label>
            <input asp-for="Restaurant.Name" class="form-control " />
            <span class="text-danger" asp-validation-for="Restaurant.Name"></span>
        </div>
        <div class="form-group">
            <label asp-for="Restaurant.Location"></label>
            <input asp-for="Restaurant.Location" class="form-control " />
            <span class="text-danger" asp-validation-for="Restaurant.Location"></span>
        </div>
        <div class="form-group">
            <label asp-for="Restaurant.Cuisine "></label>
            <select class="form-control" asp-for="Restaurant.Cuisine" asp-items="Model.Cuisines"></select>
            <span class="text-danger" asp-validation-for="Restaurant.Cuisine"></span>
        </div>
        <button type="submit" class="btn btn-primary">Save</button>
    </form>
    ```
- You don't want to redirect the user to the same page once they finish a post operation because if they refresn the operation will be repetead (duplicate data, double charge in credit card, etc).
    -Edit.cshtml.cs
    ```c#
        public IActionResult OnPost()
    {
        Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
        if (ModelState.IsValid)
        {
            restaurantData.Update(Restaurant);
            restaurantData.Commit();
            return RedirectToPage("./Detail", new { restaurantId = Restaurant.Id});
        }
        return Page(); 
    }
    ```
- Add new restaurant:
    - List.cshtml
    ```html
    <a asp-page=".\Edit" class="btn btn-primary">Add New</a>
    ```
    - Edit.cshtml
    ```html
    @page "{restaurantId:int?}"
    ```
    -Edit.cshtml.cs
    ```c#
     public IActionResult OnGet(int? restaurantId)
    {
        Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
        if (restaurantId.HasValue)
        {
            Restaurant = restaurantData.GetById(restaurantId.Value);
            if (Restaurant == null)
            {
                return RedirectToPage("./NotFound");
            }
        }
        else
        {
            Restaurant = new Restaurant();
        }
        

        return Page();
    }

    public IActionResult OnPost()
    {
        
        if (!ModelState.IsValid)
        {
            Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
            return Page();
        }

        if(Restaurant.Id > 0)
        {
            restaurantData.Update(Restaurant);
        }
        else
        {
            restaurantData.Add(Restaurant);
        }

        restaurantData.Commit();
        return RedirectToPage("./Detail", new { restaurantId = Restaurant.Id });
    }
    ```
- Confirming the last operation:
    - If .netcore (RedirectToPage) doesn't find the value in the route it will place it in the query string.
    - We an also use TempData dictionary.
    - Edit.cshtml.cs
    ```c#
    public IActionResult OnPost()
    {

        if (!ModelState.IsValid)
        {
            Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
            return Page();
        }

        if (Restaurant.Id > 0)
        {
            restaurantData.Update(Restaurant);
        }
        else
        {
            restaurantData.Add(Restaurant);
        }

        restaurantData.Commit();
        TempData["Message"] = "Restaurant saved";
        return RedirectToPage("./Detail", new { restaurantId = Restaurant.Id });

    }
    ```
    - Detail.cshtml.cs
    ```c#
    [TempData]
    public string Message { get; set; }
    ```
    - Detail.cshtml
    ```html
    @if(Model.Message != null)
    {
        <div class="alert alert-info">@Model.Message</div>
    }
    ```
## Working with SQL server and the Entity framework core

- Install dependencies in the data project:
    ```json
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="5.0.11" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="5.0.11" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="5.0.11">
    ```
- Create the DbContext class:
    ```c#
    using Microsoft.EntityFrameworkCore;
    using OdeToFood.Core;

    namespace OdeToFood.Data
    {
        public class OdeToFoodDbContext : DbContext
        {
            public DbSet<Restaurant> Restaurants { get; set; }
        }
    }
    ```
- dotnet ef dbcontext list
- You can create the database using a migration (dotnet ef ... command).
- Configure the DBContext:
    ```c#
    public class OdeToFoodDbContext : DbContext
    {
        public OdeToFoodDbContext(DbContextOptions<OdeToFoodDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OdeToFoodConfiguration());
        }

        public DbSet<Restaurant> Restaurants { get; set; }
    }
    ```
- OdeToFoodConfiguration.cs
    ```c#
    public class OdeToFoodConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public OdeToFoodConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.HasKey(prop => prop.Id);
            builder.Property(prop => prop.Name)
                .HasMaxLength(80)
                .IsRequired();
            builder.Property(prop => prop.Location)
                .HasMaxLength(256)
                .IsRequired();
            builder.Property(prop => prop.Cuisine)
                .IsRequired();
        }
    }
    ```
- Add connection string in appsettings.json:
    ```json
    {
        "ConnectionStrings": {
            "DefaultConnection": "Server=localhost;Port=5433;Database=Food;Username=postgres;Password=******"
        },
        "Logging": {
            "LogLevel": {
            "Default": "Information",
            "Microsoft": "Warning",
            "Microsoft.Hosting.Lifetime": "Information"
            }
        },
        "AllowedHosts": "*",
        "Message": "Hello from appsettings"
    }
    ```

- Add a new class and implement IRestaurantData using entity framework dbcontext:
    - Entity framework attach: to start tracking changes about this object that already exists in the db.
    - SqlRestaurantData.cs
    ```c#
    using System.Collections.Generic;
    using OdeToFood.Core;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    namespace OdeToFood.Data
    {
        public class SqlRestaurantData : IRestaurantData
        {
            private readonly OdeToFoodDbContext db;

            public SqlRestaurantData(OdeToFoodDbContext db)
            {
                this.db = db;
            }

            public Restaurant Add(Restaurant newRestaurant)
            {
                db.Add(newRestaurant);
                return newRestaurant;
            }

            public int Commit()
            {
                return db.SaveChanges(); //returns the number of rows affected
            }

            public Restaurant Delete(int id)
            {
                var restaurant = GetById(id);
                if(restaurant != null)
                {
                    db.Restaurants.Remove(restaurant);
                }
                return restaurant;
            }

            public Restaurant GetById(int restaurantId)
            {
                return db.Restaurants.Find(restaurantId);
            }

            public IEnumerable<Restaurant> GetRestaurantsByName(string name)
            {
                var query = from r in db.Restaurants
                            where r.Name.StartsWith(name) || string.IsNullOrEmpty(name)
                            select r;

                return query;
            }

            public Restaurant Update(Restaurant updatedRestaurant)
            {
                var entity = db.Restaurants.Attach(updatedRestaurant);
                entity.State = EntityState.Modified;
                return updatedRestaurant;
            }
        }
    }
    ```
- Modifying the service register:
    - AddScoped (dont use Singleton for data access). This means to have it scoped to a particular http request (an instance per request).
    - Startup.cs
    ```c#
    public void ConfigureServices(IServiceCollection services)
    {       
        services.AddRazorPages();
        services.AddDbContextPool<OdeToFoodDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))); //UseSqlServer
        services.AddScoped<IRestaurantData, SqlRestaurantData>();
    }
    ```
## Building a UI
- The _ before a .cshtml file indicates this component is part of another view and it is not redirected to it directly.
- @RenderBody in _layout page to render the pages content.
- You can also use @RenderSection.
    -_Layout.cshtml
    ```html
     <div class="container">
        <main role="main" class="pb-3">
            @RenderBody()
        </main>
    </div>

    <footer class="border-top footer text-muted">
       @RenderSection("footer", required: false)
    </footer>
    ```
- Use the footer section inside a view:
    - List.cshtml
    ```html
    <a asp-page=".\Edit" class="btn btn-default">Add New</a>
    @section footer{
        <div>@Model.Message</div>
    }
    ```
- Define a new delete razor page:
    - To explicitly define a layout for a view:
    - Delete.cshtml
    ```html
    @page "{restaurantid}"
    @model QuotesApi.Pages.Restaurants.DeleteModel
    @{
        Layout = "_Layout2";
    }
    <h2>Delete!</h2>
    <div class="alert alert-danger">
        Are you sure you want to delete @Model.Restaurant.Name?
    </div>
    <form method="post">
        <button type="submit" class="btn btn-danger">Yes!</button>
        <a asp-page="List" class="btn btn-default">Cancel</a>
    </form>
    ```
    - Delete.cshtml.cs
    ```c#
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using OdeToFood.Core;
    using OdeToFood.Data;

    namespace QuotesApi.Pages.Restaurants
    {
        public class DeleteModel : PageModel
        {
            private readonly IRestaurantData restaurantData;
            public Restaurant Restaurant { get; set; }

            public DeleteModel(IRestaurantData restaurantData)
            {
                this.restaurantData = restaurantData;
            }

            public IActionResult OnGet(int restaurantId)
            {
                Restaurant = restaurantData.GetById(restaurantId);
                if(Restaurant == null)
                {
                    return RedirectToPage("./NotFound");
                }
                return Page();
            }

            public IActionResult OnPost(int restaurantId)
            {
                var restaurant = restaurantData.Delete(restaurantId);
                restaurantData.Commit();

                if(restaurant == null)
                {
                    return RedirectToPage("./NotFound");
                }

                TempData["Message"] = $"{restaurant.Name} was deleted";
                return RedirectToPage("./List");
            }
        }
    }
    ```
- _ViewStart.cshtml especifies that the views require a layout:
    - _ViewStart.cshtml:
    ```html
    @{
        Layout = "_Layout";
    }
    ```
- Partial views:
    - Add Razor view (it doesn't have a model).
    - _Summary.cshtml
    ```html
    @using OdeToFood.Core 
    @model Restaurant

    <div class="panel panel-default">
        <div class="panel-heading">
            <h3>@Model.Name</h3>
        </div>
        <div class="panel-body">
            <span>Location: @Model.Location</span>
            <span>Cuisine: @Model.Cuisine</span>
        </div>
        <div class="panel-footer">
            <a class="btn btn-default"
            asp-page="./Detail" asp-route-restaurantId="@Model.Id">
                Details
            </a>
            <a class="btn btn-default"
            asp-page="./Edit" asp-route-restaurantId="@Model.Id">
                Edit
            </a>
            <a class="btn btn-default"
            asp-page="./Delete" asp-route-restaurantId="@Model.Id">
                Delete
            </a>
        </div>
    </div>
    ```
- Rendering the partial view:
    - List.cshtml
    ```html
    @foreach(var restaurant in Model.Restaurants)
    {
        <partial name="_Summary" model="restaurant"/>
    }
    ```
- ViewComponents
    - A ViewComponent doesn't respond to an http request (not OnGet or OnPost methods).
    - It is embedded inside another view like th partial views.
    - It is similar to MVC. 
    - RestaurantCountViewComponent.cs
    ```c#
    using Microsoft.AspNetCore.Mvc;
    using OdeToFood.Data;

    namespace QuotesApi.ViewComponents
    {
        public class RestaurantCountViewComponent : ViewComponent 
        {
            private readonly IRestaurantData restaurantData;

            public RestaurantCountViewComponent(IRestaurantData restaurantData)
            {
                this.restaurantData = restaurantData;
            }

            public IViewComponentResult Invoke()
            {
                var count = restaurantData.GetCountOfRestaurants();
                return View(count); 
            }
        }
    }
    ```
    - The view has to be in a folder the matches exactly the ViewComponent name (Shared/Components/RestaurantCount).
    - We create it the view in the share folder because we want to use it in every page in the application.
        - Shared/Components/RestaurantCount/Default.cshtml
        ```html
        @model int
        <div class="well">
            There are @Model restaurants here. <a asp-page="/Restaurants/List">See them all!</a>
        </div>
        ```
    - We need a special taghelper to display the component:
        - ViewImports.cshtml
        ```html
        @using QuotesApi
        @namespace QuotesApi.Pages
        @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
        @addTagHelper *, QuotesApi 
        ```
    - Render the ViewComponent:
    - _Layout.html
    ```html
    <footer class="border-top footer text-muted">
    <vc:restaurant-count></vc:restaurant-count>
    @RenderSection("footer", required: false)
    </footer>
    ```
## Integrating client side

- Properties -> launchSettings.json to control environments (profile -> environment variables). 
- Define environment specif scripts in views:
    - ClientRestaurants.cshtml
    ```html
    <environment include="Development">
        <script src="..."/>
    </environment>
    ```
- The Startup.cs Configure configures the .net core application configuration.
    - Add the middleware pipeline (every http request that comes to the application has to pass through that pipeline).

## Working with the internals of Asp.net core

- launchSettings.json to define how the application behaves.
- To configure dotnet run behavior:
    - launchSettings.json
    ```json
    "QuotesApi": {
        "commandName": "Project",
        "launchBrowser": true,
        "applicationUrl": "https://localhost:5001;http://localhost:5000",
        "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
    }
    ```
- Program.cs CreateWebHostBuilder method to create a web host. Default logging, settings, etc. Also defines the class that will configure the application (Startup class).
- .Net core instanciates that class and call two methods: ConfigureServices and Configure.
- Configure is used for to define the middleware the applicattion will use. 
    - We install middleware by invoking extension methods of an object that implements IApplicationBuilder interface. 
    - Procesing pipeline for http request messges.
    - Logger -> Authorizer -> Router.
    - The middleware pipeline is bidirectional: requests go in and responses flow out.
    - Each piece of middleware should be able to inspect the incoming request and it's also going to know when the output response is occurring.
    - Startup.cs
    -  app.UseDeveloperExceptionPage(); is located at the beggining of the pipeline because it is also the last part that is gonna be called (display the exception page in the http response with a lot of detail so it is not used in prod).
    - app.UseAuthentication() is used later in the pipeline so other pieces of middleware can use the identity of the user. 
    ```c#
       // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
        });
    }
    ```
- A request delegate is a method that takes an http request as a parameter and returns a Task.
- The http request is represent as the HttpContext object. 
    - Startup.cs
    ```c#
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.Use(SayHelloMiddleware);
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
        });
    }

    private RequestDelegate SayHelloMiddleware(RequestDelegate next)
    {
        return async ctx =>
        {
            if (ctx.Request.Path.StartsWithSegments("/hello"))
            {
                await ctx.Response.WriteAsync("Hello, World!");
            }
            else
            {
                await next(ctx); //continue with the pipeline if route is different than /hello
            }
        
        };
    }
    ```
- Loggin application messages:
    - List.cshtml.cs
    ```c#
    using Microsoft.Extensions.Logging;
    using OdeToFood.Core;
    using OdeToFood.Data;

    namespace QuotesApi.Pages.Restaurants
    {
        public class ListModel : PageModel
        {
            private readonly IConfiguration configuration;
            private readonly IRestaurantData restaurantData;
            private readonly ILogger<ListModel> logger;

            public ListModel(IConfiguration configuration,
                IRestaurantData restaurantData,
                ILogger<ListModel> logger)
            {
                this.configuration = configuration;
                this.restaurantData = restaurantData;
                this.logger = logger;
            }

            public string Message { get; set; }
            public IEnumerable<Restaurant> Restaurants { get; set; }
            [BindProperty(SupportsGet =true)]
            public string SearchTerm { get; set; }
            public void OnGet()
            {
                logger.LogError("Executing ListModel");
                Message = configuration["Message"];
                Restaurants = restaurantData.GetRestaurantsByName(SearchTerm);
            }
        }
    }
    ```
- In .net core configuration settings are hierarchical (command line, appsettings.json, appsetings.{env.EnvironmentName}.json, environment variables, etc). 
- To see this hierarchical level add another Message in appsettings.Development.json
    - appsettings.Development.json
    ```json
    {
    "Logging": {
        "LogLevel": {
        "Default": "Information",
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
        }
    },
    "Message": "Hello from Dev appsettings"
    }
    ```
    - Now the UI should display "Hello from Dev appsettings" since we run the application in development mode. It displayed the one from appsettings.json because we didn't have a Message in appsettings.Development.json

## Deploying asp.net core

```bash
dot net publish o c:\temp\odetofood
```

- Using MSBuild to execute npm.
- The .csproject file in .Net is what we called the MSBuild file for a project. MSBuild understands how to process a csproj file and executes the instructions inside to build a project. 
    ```xml
    <Target Name="PostBuil" AfterTargets="ComputeFilesToPublish">
        <Exec Command="npm install"/>
    </Target>
    ```
- By default the node_modules is ignored in the git ignore. To avoid that:
    ```xml
    <ItemGroup>
        <Content Include="node_modules/**" CopyToPublishDirectory="PreserveNewest"/>
    </ItemGroup>
    ```
- We're telling MSBuild that the name of this target is PostBuild and we want to execuite what is inside of the target after it finishes another target (ComputeFilesToPublish).
-Publish self-contained applications (they have everything that they need and don't share dependencies with other applications).
- They are in WIndows RIDs for runtime identifiers
    ```bash
    dotnet publish -o c:\\temp\odetofood --self-contained -r win-x64
    ```
- For IIS you need AspNetCoreModuleV2 under Modules installed.
- If we are going to use a user and password for a connection in prod we dont store that info in the appsettings.Production.json, we do it in an environment variable in the server.



# Building your first ASP.Net Core Application
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
    - Configure method configures the http processing pipeline. Each http request will find the response to that request in this method. 
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