## Understanding authentication and authorization in .Net Core

- **Authentication:** you are who you say you are (determining the identity of somebody).

- **Authorization:** claims (name, birthdate, etc). Key to access certain parts of the application. What actions can a user do. 
 
- To add authentication in the ConfigureServices method of the Startup.cs

    ```cs
    public void ConfigureServices(IServiceCollection services)
    {
        /*Other services configurations ommited here*/
        services.AddAuthentication()
                .AddCookie();

    }
    ```
    * AddCookie() to add cookie authentication. This adds an authentication scheme (a way to do authentication) to the configuration. 
    * You have to specify a name for the authentication scheme. If you don't add a name in your cookie authentication scheme by default it adds the name "Cookies".
    * .Net core has to know the default scheme it has to use to make authentication checks, as well as certain actions it has to run against the scheme. 
    ```cs
    public void ConfigureServices(IServiceCollection services)
    {
        /*Other services configurations ommited here*/
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

    }
    ```
- After adding cookie authentication you have to place a piece of middleware that does the actual authentication in the pipeline. 

- Place the middleware between use routing and use endpoints so it will know at which endpoint the request will arrive.

    ```cs
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Conference}/{action=Index}/{id?}");
        });

    }
    ```
- To make the application secure by default, register Authorize as a global filter in ConfigureServices (applied to all controllers).

    ```cs
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews(o => o.Filters.Add(new AuthorizeFilter()));
        /*Other services configurations ommited here*/
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

    }
    ```
- Now every controller in our project needs a logged in user. 
- To allow un-authenticated users to access to controller:

    ```cs
    [AllowAnonymous]
    public class ConferenceController: Controller
    {
        ...
    }
    ```
- By default the cookie scheme redirects to account/login when authentication fails. If you want to change it: 

    ```cs
    public void ConfigureServices(IServiceCollection services)
    {
        /*Other services configurations ommited here*/
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o => o.LoginPath = "/account/signin");
    }
    ```
- You can simply add a Login view in account login and authorize anonymous access to it.

    ```cs
    /*AccountController.cs*/
    [AllowAnonymous]
    public IActionResult Login(string returnUrl = "/")
    {
        return View(new LoginModel { ReturnUrl = returnUrl });
    }
    ```
- Add an action to capture the submitted login form:
    ```cs
     [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = userRepository.GetByUsernameAndPassword(model.Username, model.Password);
            if (user == null)
                return Unauthorized(); //when no user is found

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), //nameidentifier is the id
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("FavoriteColor", user.FavoriteColor) //to add a non-standard claim type
            };
            /*we need to create an identity object with the claims and the authentication scheme*/
            var identity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            /*Then we create a claims principal object with the identity*/
            var principal = new ClaimsPrincipal(identity);

            /*After you finish your claims principal you call HttpContext.SignInAsync*/
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = model.RememberLogin });
            /*Finally we redirect the user to the url requested before the login page was shown*/
            return LocalRedirect(model.ReturnUrl);
        }

        /*The repository method GetByUsernameAndPassword*/

        private List<User> users = new List<User>
        {
            new User { Id = 3522, Name = "roland", Password = "K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols=",
                FavoriteColor = "blue", Role = "Admin", GoogleId = "101517359495305583936" }
        };

        public User GetByUsernameAndPassword(string username, string password)
        {
            var user = users.SingleOrDefault(u => u.Name == username &&
                u.Password == password.Sha256());
            return user;
        }
    ```
    * Claim is a class with a type and a value property. 
    * In asp.net core the object that represents the user is the ClaimsPrincipal. On claimprincipal has to have one identity object per authentication schema (f.e cookie and google auth schemas).
    * All identity objects contain claims.

- To logout from the application:

    ```cs
      public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    ```

- The identity cookie is needed to know which request came from which user.
- Once we sign in the application stores all the user info in the cookie and encrypts it. Cookies are sent to the server and it decrypts it. 
- Encryption works with a framework called Data Protection, which is called behind the scenes (added when you choose cookie authentication).
- When you logout the cookie desapears from the browser.
- To access the user and the claims use User.Identity.
- When the browser does a request and the server wants to authenticate the user, instead of showing an internal login, we redirect to the external identity provider. 
- An external provider needs to know which application is trying to authenticate so we need to send it a client id and a client secret (nothing to do with the user).
- When the user logs in in the external provider, it does an http request, delivering a token to the redirect URI. In the toker are Google user claims that are then use to create .net identity cookie.
- Add Client id and client secret in the secrets management tool. 
- To add a google authentication scheme:
- Challenge scheme, if user tries to access a resource for which authentication is required (for example go to proposals section). 

     ```cs
    public void ConfigureServices(IServiceCollection services)
    {
        /*Other services configurations ommited here*/
        services.AddAuthentication(o=> {
            o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddGoogle(o=>
        {
            o.ClientId = Configuration["Google:ClientId"];
            o.ClientSecret = Configuration["Google:ClientSecret"];
        });
    }
    ```
- Create a login button and remove the defaultchallengescheme to go to the previous login section where we are going to add the new button.

- Add a new action LoginWithGoogle which has a redirect url that gets called once google completed the authentication. I can pass the returnUrl through the Items property of that callback action. 
    ```cs
    [AllowAnonymous]
    public IActionResult LoginWithGoogle(string returnUrl = "/")
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Action("GoogleLoginCallback"),
            Items =
                {
                    { "returnUrl", returnUrl }
                }
        };
        return Challenge(props, GoogleDefaults.AuthenticationScheme);
     }
    ```
- When Google authenticates the user we want to set a cookie.
- Create a cookie container for it and then instruct the google authetication scheme to use the new cookie container for its sign in action. 

     ```cs
    public void ConfigureServices(IServiceCollection services)
    {
        /*Other services configurations ommited here*/
        services.AddAuthentication(o=> {
        o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        //o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
            .AddCookie()
            .AddCookie(ExternalAuthenticationDefaults.AuthenticationScheme)//cookie container
            .AddGoogle(o=>
            {
                o.SignInScheme = ExternalAuthenticationDefaults.AuthenticationScheme;
                o.ClientId = Configuration["Google:ClientId"];
                o.ClientSecret = Configuration["Google:ClientSecret"];
                });
    }

    //ExternalAuthenticationDefaults class
    public static class ExternalAuthenticationDefaults
    {
        public const string AuthenticationScheme = "ExternalIdentity";
    }    
    ```
- Add the google callback method and read Goole identity cookie to retrieve its claims.
    * Create a new authentication cookie with our own claims and sign in.
    * Delete the google temporary cookie after usage using sign out. 

    ```cs
    [AllowAnonymous]
    public async Task<IActionResult> GoogleLoginCallback()
    {
    // read google identity from the temporary cookie
    var result = await HttpContext.AuthenticateAsync(
            ExternalAuthenticationDefaults.AuthenticationScheme);

    var externalClaims = result.Principal.Claims.ToList();

    var subjectIdClaim = externalClaims.FirstOrDefault(
                x => x.Type == ClaimTypes.NameIdentifier);
    var subjectValue = subjectIdClaim.Value;

    var user = userRepository.GetByGoogleId(subjectValue);

    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Role, user.Role),
        new Claim("FavoriteColor", user.FavoriteColor)
    };
    
    var identity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
    var principal = new ClaimsPrincipal(identity);

    // delete temporary cookie used during google authentication
    await HttpContext.SignOutAsync(
            ExternalAuthenticationDefaults.AuthenticationScheme);

    await HttpContext.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return LocalRedirect(result.Properties.Items["returnUrl"]);
    }
    ```
## Asp.net core Identity

- Add Asp.net core web application -> MVC -> Individual User Accounts authentication.

- Register and Update-database so it will create a webapplication1 database with AspNetUsers table where it will store the users.

- Identity is a framework that uses Cookie authentication. It contains helper classes and UI.

- In has EF and database out of the box, and it is customizable.

- It is also configurable (change the password policy for example).

- Application context in Data folder instead of deriving from DBContext derives from IdentityDBContext. At the end it derives from Entity DBContext.
    ```cs
    // Summary:
    //     Base class for the Entity Framework database context used for identity.
    public class IdentityDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {}
    ```
    * First generic parameter IdentityUser is the entity used for the user. 
    * The second is the entity used for a role. 
    * The third parameter is the primary key for both.
    * Application derives from IdentityDBContext because it contains all the DbSets related with identity.

- When can customize the database name in the appsettings. 

- You can change the name of the tables modifying the OnModelCreating method. 

- Create a new column for a user:
    * Create new class that inherits from IdentityUser.
    * Identity has a feature with which users can delete their account. When that happens, all data marked with PersonalData attribute will be deleted.  
    ```cs
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public DateTime CareerStatus { get; set; }
    }
    ```
    * Modify ApplicationDBContext
    ```cs
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
    ```
    * Add a migration
    ```command
    add-migration User-CareerStarted
    Update-Database
    ```
    * Now the CareerStatus column is present in the [dbo].[AspNetUsers] table.
 - Change the AddDefaultIdentity in the ConfigureServices method.
    ```cs
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ApplicationUser>(options => {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireNonAlphanumeric = false;      
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

    ```
    * Modify the password policy to not require NonAlphanumerical characters.

- The UI of Identity is not in the form of MVC but it uses Razor pages. It is located in the packages Microsoft.AspNetCore.Identity.UI.
- You have to modify the Views -> Shared -> LoginPartial.cshtml.

    ```cs
    @using Microsoft.AspNetCore.Identity
    @using WebApplication1.Data 

    @inject SignInManager<ApplicationUser> SignInManager
    @inject UserManager<ApplicationUser> UserManager
    ```
- You can add Identity to an EXISTING PROJECT (ConfWebArch) select Add -> Add scaffolded item -> Identity (select Login and Register). 

- Check if StaticFiles and Authentication is configured in the middleware. Also check for MVC and RazorPages: 

```cs
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Conference}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
```

- Change db connection string name in appsettings.json.
- Run the migration (you need to specify for which db context the migration will be).

```cmd
 add-migration IdentityInitial -Context ConfArchWebContext
 Update-Database -Context ConfArchWebContext
```
- Configuration specific for Identity is in a separate file called IdentityHostingStartup.

- Modify the layout to add the LoginPartial and the scripts section to display the login and the register sections. 

```html
<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>@ViewBag.Title</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css">
    <link href="~/site.css" rel="stylesheet" />
</head>
<body>
<div class="container">
    <div class="row">
        <div class="col-md-2">
            <img src="~/img/ConfArchSmall.png" style="max-width:100%" />
        </div>
        <div class="col-md-6 titlecol">
            <span class="title">@ViewBag.Title</span>           
        </div>
        <div class="col-md-2 titlecol">
            <partial name="_LoginPartial.cshtml"/>
        </div>
    </div>
        <div>
            @RenderBody()
        </div>
    </div>
</body>
@RenderSection("scripts", false)
</html>

```

- A BindProperty is a property that gets filled by the model binder when the form gets posted. 

- We need to make claims transformation to make identity use our custom claims. 

- Create class ApplicationUserClaimsPrincipalFactory. It's function is to add claims to the claims principal. 

```cs
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, IOptions<IdentityOptions> options) : base(userManager, options)
        {

        }

        protected async override Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("CareerStarted", user.CareerStatus.ToShortDateString()));
            identity.AddClaim(new Claim("FullName", user.FullName));

            return identity;
        }
    }
```

- The last step is to register the new class in the dependency injection container.

```cs
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ConfArchWebContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ConfArchWebContextConnection")));

                services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ConfArchWebContext>();

                services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
            });
        }
    }
```
- To customize the LoginPage, modify the LoginPartial view:

```html
@using Microsoft.AspNetCore.Identity
@using ConfArch.Web.Data

@inject SignInManager<ApplicationUser> SignInManager
@inject UserManager<ApplicationUser> UserManager


@{ 
    var fullName = User.Identity.IsAuthenticated ? User.Claims.First(c => c.Type == "FullName").Value : "";
}

<ul class="navbar-nav">
    @if (SignInManager.IsSignedIn(User))
    {
        <li class="nav-item">
            <a id="manage" class="nav-link text-dark" asp-area="Identity" asp-page="/Account/Manage/Index" title="Manage">Hello @fullName!</a>
        </li>
        <li class="nav-item">
            <form id="logoutForm" class="form-inline" asp-area="Identity" asp-page="/Account/Logout" asp-route-returnUrl="@Url.Action("Index", "Home", new { area = "" })">
                <button id="logout" type="submit" class="nav-link btn btn-link text-dark">Logout</button>
            </form>
        </li>
    }
    else
    {
        <li class="nav-item">
            <a class="nav-link text-dark" id="register" asp-area="Identity" asp-page="/Account/Register">Register</a>
        </li>
        <li class="nav-item">
            <a class="nav-link text-dark" id="login" asp-area="Identity" asp-page="/Account/Login">Login</a>
        </li>
    }
</ul>
```
- Role claims can be associated with roles.
- The easiest way to add roles is to not use AddDefaultIdentity but use AddIdentity instead. 

```cs
    services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ConfArchWebContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();
```
- Modify ApplicationUserClaimsPrincipalFactory to support roles: 

```cs
    services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ConfArchWebContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();
```
- Add role in AspNetRoles table.
- Associate rol to user in the AspnetUsersRoles table.
- Add claim permission in the AspnetRoleClaims. 
- To use one identity mechanism for different applications check the OpenIdConnectIdentityProvider.

## Authorization

- Claims can be used to do authorization.
- Check for AddAuthorization in the middleware (Configure Startup method).
- Add a policy in ConfigureServices:

```cs

```