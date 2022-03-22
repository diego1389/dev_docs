- Authentication: verify you are who you say you are and generate the security context.

- Security context: all your identity info that is relevant to the facility.
    - Contains all the information the user has (user name, addresses, etc).
    - Encapsulated into one single object: claims principal. Object that represents the security context of the user (it is the user).
    - It can have one or many identities.
    - One identity can have many claims. Claims are a key value pair that carry the users information.
     - The security context is under base.User (.net core app razor pages index.cshtml.cs). By default (before creating the Login page) it is anonymous identity. 
        - Type base.User.Identity under the **Watch** window. The IsAuthenticated property should be false.  

- Authorization: Verifying the security context satisfies the access requirements.

- The server verify the user's credentials and returns the identity (security context) to the browser.

- **Add the login page:**
    - Add razor page Login with view model.
    - Login.cshtml
    ```html
    @page
    @model UnderTheHood.Pages.Account.LoginModel
    @{
    }

    <div class="container border" style="padding:20px">
        <form method="post">
            <div class="text-danger" asp-validation-summary="ModelOnly"></div>
            <div class="form-group row">
                <div class="col-2">
                    <label asp-for="Credential.UserName"></label>
                </div>
                <div class="col-5">
                    <input type="text" asp-for="Credential.UserName" class="form-control" />
                    <span class="text-danger" asp-validation-for="Credential.UserName"></span>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-2">
                    <label asp-for="Credential.Password"></label>
                </div>
                <div class="col-5">
                    <input type="password" asp-for="Credential.Password" class="form-control" />
                    <span class="text-danger" asp-validation-for="Credential.Password"></span>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-2">
                    <input type="submit" class="btn btn-primary" value="Login"/>
                </div>
            </div>
        </form>
    </div>
    ```
    - Login.cshtml.cs
    ```c#
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    namespace UnderTheHood.Pages.Account
    {
        public class LoginModel : PageModel
        {
            [BindProperty]
            public Credential Credential { get; set; }
            public void OnGet()
            {
            }

            public void OnPost()
            {';

            }
        }
    }
    ```
    - Credential.cs
    ```c#
    using System.ComponentModel.DataAnnotations;

    namespace UnderTheHood.Pages.Account
    {
        public class Credential
        {
            [Required]
            [Display(Name = "User Name")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
    ```
- Generate cookie with cookie authentication handler:
    - Starup.cs
    ```c#
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
        {
            options.Cookie.Name = "MyCookieAuth";
        });
        services.AddRazorPages();
    }
    ```
    - Login.cshtml.cs
    ```c#
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        //Verify credential
        if(Credential.UserName == "admin" && Credential.Password == "password")
        {
            //Create security context
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Email, "admin@test.com")
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            /*Serialize the claims principle into a stream, then encrypts that stream and save that as a cookie, right into the cookie inside the http context object*/
            await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);
            return RedirectToPage("/Index");
        }

        return Page();
    }
    ```
- To actually read the cookie you need UseAuthorization
    - Start.cs
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

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
        });
    }
    ```
- Now the cookie authentication appears under base.User.identities (Watch window) in index.cshtml.cs
- Policy is formed by one or more requirements. 
    - For each requirement it should be an authorization handler.
    - IAuthorizationService.
- To prevent unauthorized access to a page add [Authorize] data attribute in the code behind. It will go to the login page by default.
- To change the default login page (path: /Account/Login)
    ```c#
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
            {
                options.Cookie.Name = "MyCookieAuth";
                options.LoginPath = "/Account2/Login";
            });
            services.AddRazorPages();
        }
    ```
- Configure policy:
    - Add new page (HUman resources)s
    - Configure middleware:
    ```c#
    public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
            {
                options.Cookie.Name = "MyCookieAuth";
                options.LoginPath = "/Account/Login";
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("MustBelongToHR", policy => policy.RequireClaim("Department", "HR"));
            });
            services.AddRazorPages();
        }
    ```
    - Add Authorization data annotation in HR page
    ```c#
    [Authorize(Policy = "MustBelongToHR")]
    public class HumanResourceModel : PageModel
    {
        public void OnGet()
        {
        }
    }
    ```
    - Add the new claim in the login page:
    ```c#
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        //Verify credential
        if(Credential.UserName == "admin" && Credential.Password == "password")
        {
            //Create security context
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Email, "admin@test.com"),
                new Claim("Department", "HR")
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            /*Serialize the claims principle into a stream, then encrypts that stream and save that as a cookie, right into the cookie inside the http context object*/
            await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);
            return RedirectToPage("/Index");
        }

        return Page();
    }
    ```
- Implementing logout page:
    - Add logout page:
    ```c#
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToPage("Index");
        }
    }
    ```
    - Add a login status partial view:
    ```html
    @if (User.Identity.IsAuthenticated)
    {
        <form method="post" class="form-inline" asp-page="/Account/Logout">
            Welcome @User.Identity.Name
            <button type="submit" class="ml-2 btn btn-link" >Logout</button>
        </form>
    }
    else {
        <a class="btn btn-link" asp-page="/Account/Login">Login</a>
    }
    ```
    - Add the partial view in the _Layout
    ```html
     <div class="mr-2">
        <partial name="_LoginStatusPartial_"/>
    </div>
    ```
- Custom policy based authorization
    1. Add two classes to handle the custom authorization logic
    ```c#
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;

    namespace UnderTheHood.Authorization
    {
        public class HRManagerProbationRequirement : IAuthorizationRequirement
        {
            public HRManagerProbationRequirement(int probationMonths)
            {
                ProbationMonths = probationMonths;
            }

            public int ProbationMonths { get; }
        }

        public class HRManagerProbationRequirementHandler : AuthorizationHandler<HRManagerProbationRequirement>
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRManagerProbationRequirement requirement)
            {
                if(!context.User.HasClaim(claim => claim.Type == "EmploymentDate"))
                {
                    return Task.CompletedTask;
                }

                var empDate = DateTime.Parse(context.User.FindFirst(x => x.Type == "EmploymentDate").Value);
                var period = DateTime.Now - empDate;
                if(period.Days > 30 * requirement.ProbationMonths)
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }
        }
    }
    ```
    - Modify the ConfigureServices
    ```c#
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
        {
            options.Cookie.Name = "MyCookieAuth";
            options.LoginPath = "/Account/Login";
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("MustBelongToHR", policy => policy
                .RequireClaim("Department", "HR")
                .Requirements.Add(new HRManagerProbationRequirement(3)));
        });
        services.AddSingleton<IAuthorizationHandler, HRManagerProbationRequirementHandler>();
        services.AddRazorPages();
    }
    ```
    - Modify the login
    ```c#
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        //Verify credential
        if(Credential.UserName == "admin" && Credential.Password == "password")
        {
            //Create security context
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Email, "admin@test.com"),
                new Claim("Department", "HR"),
                new Claim("EmploymentDate", "2021-12-12")
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            /*Serialize the claims principle into a stream, then encrypts that stream and save that as a cookie, right into the cookie inside the http context object*/
            await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);
            return RedirectToPage("/Index");
        }

        return Page();
    }
    ```
- Cookie lifetime
    - Startup.cs
    ```c#
     public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
        {
            options.Cookie.Name = "MyCookieAuth";
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.ExpireTimeSpan = TimeSpan.FromSeconds(30);
        });
    ...
    }
    ```
    - A persistent cookie survives even if the browser closes.
    - Create persistent cookie:
    - Add new property to credential:
    ```c#
    using System.ComponentModel.DataAnnotations;

    namespace UnderTheHood.Pages.Account
    {
        public class Credential
        {
            [Required]
            [Display(Name = "User Name")]
            public string UserName { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me")]
            public bool RememberMe { get; set; }
            }
    }
    ```
    - Modify login.cshtml
    ```html
    @page
    @model UnderTheHood.Pages.Account.LoginModel
    @{
    }

    <div class="container border" style="padding:20px">
        <form method="post">
            <div class="text-danger" asp-validation-summary="ModelOnly"></div>
            <div class="form-group row">
                <div class="col-2">
                    <label asp-for="Credential.UserName"></label>
                </div>
                <div class="col-5">
                    <input type="text" asp-for="Credential.UserName" class="form-control" />
                    <span class="text-danger" asp-validation-for="Credential.UserName"></span>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-2">
                    <label asp-for="Credential.Password"></label>
                </div>
                <div class="col-5">
                    <input type="password" asp-for="Credential.Password" class="form-control" />
                    <span class="text-danger" asp-validation-for="Credential.Password"></span>
                </div>
            </div>
            <div class="row form-check mb-2">
                <div class="col-2">
                    <input type="checkbox" asp-for="Credential.RememberMe" class="form-check-input"/>
                    <label class="form-check-label" asp-for="Credential.RememberMe"></label>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-2">
                    <input type="submit" class="btn btn-primary" value="Login"/>
                </div>
            </div>
        </form>
    </div>
    ```
    - Add AuthenticationProperties (Login.cshtml.cs)
    ```c#
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    namespace UnderTheHood.Pages.Account
    {
        public class LoginModel : PageModel
        {
            [BindProperty]
            public Credential Credential { get; set; }
            public void OnGet()
            {
            }

            public async Task<IActionResult> OnPostAsync()
            {
                if (!ModelState.IsValid) return Page();
                //Verify credential
                if(Credential.UserName == "admin" && Credential.Password == "password")
                {
                    //Create security context
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, "admin"),
                        new Claim(ClaimTypes.Email, "admin@test.com"),
                        new Claim("Department", "HR"),
                        new Claim("EmploymentDate", "2021-12-12")
                    };

                    var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = Credential.RememberMe
                    };

                    /*Serialize the claims principle into a stream, then encrypts that stream and save that as a cookie, right into the cookie inside the http context object*/
                    await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);
                    return RedirectToPage("/Index");
                }

                return Page();
            }
        }
    }
    ```
## Secure Web APIs

- Add a new project ASP.NET core API application (WeatherForecast example).
- Create and consume a Web API. 
- Install Asp.net.core.http.extension in WebApp project.
- Add http client configuration (Startup.cs)
    ```cs
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
        {
            options.Cookie.Name = "MyCookieAuth";
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(2);
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("MustBelongToHR", policy => policy
                .RequireClaim("Department", "HR")
                .Requirements.Add(new HRManagerProbationRequirement(3)));
        });
        services.AddSingleton<IAuthorizationHandler, HRManagerProbationRequirementHandler>();
        services.AddRazorPages();
        services.AddHttpClient("OurWebApi", client =>
        {
            client.BaseAddress = new Uri("https://localhost:42678/"); //New web API url
        });
    }
    ```
    - Retrieve the WeatherForecast items from the WebAPI inside the HR view (WebApp project)
    - HumanResources.cshtml.cs
    ```cs
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using WebProject.DTO;

    namespace UnderTheHood.Pages
    {
        [Authorize(Policy = "MustBelongToHR")]
        public class HumanResourceModel : PageModel
        {
            private readonly IHttpClientFactory httpClientFactory;
            [BindProperty]
            public List<WeatherForecastDTO> WeatherForecastItems { get; set; }
            public HumanResourceModel(IHttpClientFactory httpClientFactory)
            {
                this.httpClientFactory = httpClientFactory;
            }

            public async Task OnGetAsync()
            {
                var httpClient = httpClientFactory.CreateClient("OurWebApi");
                WeatherForecastItems = await httpClient.GetFromJsonAsync<List<WeatherForecastDTO>>("WeatherForecast");
            }
        }
    }
    ```
    - Display the elements in the screen:
    - HumanResources.cshtml
    ```html
    @page
    @model UnderTheHood.Pages.HumanResourceModel
    @{
    }

    <div>
        <h1 class="display-4">Human Resources</h1>
        <table class="table table-bordered table-striped">
            <thead>
            <tr>
                <td>Date</td>
                <td>Temp C</td>
                <td>Temp F</td>
                <td>Summary</td>
            </tr>
            </thead>
            <tbody>
                @foreach (var item in Model.WeatherForecastItems)
                {
                    <tr>
                        <td>@item.Date.ToShortDateString()</td>
                        <td>@item.TemperatureC</td>
                        <td>@item.TemperatureF</td>
                        <td>@item.Summary</td>
                    </tr>
                }
            </tbody>
        </table>
    </div>
    ```
- JWT token (Json Web Token): 
- Same process as the Cookie, the JWT will be carried by the browser in the http request and http response header instead of the cookie. 
- It has three different parts:
    1. Hashing algorithm.
    2. Claims (user info).
    3. Hashed claims. 
- Process: you take the claims, take your chosen hashing algorithm. Apply your secret key the signing process and this generates the hashed claims. It is a one way process. You cannot reverse the process to retrieve the claims. It is a base-64 string. Don't put any sensitive information in the claims. 
- When you want to verify the JWT you repeat the same process. Only the people who have the key can do the process. You do the same process and compare the two hashed claims to see if it is valid. 
- For JWT we don't need the identity. 
- You need to install a new nuget package in order to generate the JWT: System.Identity.Model.Tokens.Jwt and also the Microsoft.AspNetCore.Authentication.JwtBearer. 
- Create a new Auth controller under the API project to generate a JWT token:
- In appsettings.development add a new secret key to read:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "SecretKey" :  "kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk"
}

```
- AuthController.cs
```cs
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace UnderTheHoodApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public AuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody] Credential credential)
        {
            if (credential.UserName == "admin" && credential.Password == "password")
            {
                //Create security context
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@test.com"),
                    new Claim("Department", "HR"),
                    new Claim("EmploymentDate", "2020-12-12")
                };

                var expiresAt = DateTime.UtcNow.AddMinutes(10);

                return Ok(new
                {
                    access_token = CreateToken(claims, expiresAt),
                    expires_at = expiresAt
                });
            }

            ModelState.AddModelError("Unauthorized", "You are not authorized to access the endpoint");
            return Unauthorized(ModelState);
        }

        private string CreateToken(IEnumerable<Claim> claims, DateTime expiresAt) {
            var secretKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretKey"));
            var jwt = new JwtSecurityToken(
                claims : claims,
                notBefore : DateTime.UtcNow,
                expires: expiresAt,
                signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(secretKey),
                        SecurityAlgorithms.HmacSha256Signature
                    )
                );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }

    public class Credential
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
```
- Get the token using Postman (Post tohttps://localhost:42678/auth).
- Body:
```json
{
    "userName" : "admin",
    "password" : "password" 
}
```
- Read JWT token with Authentication and middleware. 
- Add an authorization data annotation in the weatherforecast controller:
```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace UnderTheHoodApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 10).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
```
- Configure the middleware and the handler in order to read the token and validate the request:
- Startup.cs (WebApi)
```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace UnderTheHoodApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetValue<string>("SecretKey"))),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
```
- Consume the endpoint protected by JWT token:
- To get the claims from a jwt token use: https://jwt.io/
- Create a new JwtToken class in the WebApp project:
```c#
using System;
using Newtonsoft.Json;

namespace WebProject.Authorization
{
    public class JwtToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expires_at")]
        public DateTime ExpiresAt { get; set; }
    }
}
```
- Modify HumanResourses.cshtml.cs to retrieve the jwt token from auth endpoint and add it in the headers to be able to consume WeatherForecast endopoint:
```c#
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using UnderTheHood.Pages.Account;
using WebProject.Authorization;
using WebProject.DTO;

namespace UnderTheHood.Pages
{
    [Authorize(Policy = "MustBelongToHR")]
    public class HumanResourceModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        [BindProperty]
        public List<WeatherForecastDTO> WeatherForecastItems { get; set; }
        public HumanResourceModel(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var httpClient = httpClientFactory.CreateClient("OurWebApi");
            var response = await httpClient.PostAsJsonAsync("auth", new Credential {UserName = "admin", Password = "password" });
            response.EnsureSuccessStatusCode();
            string jwtToken = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<JwtToken>(jwtToken);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            WeatherForecastItems = await httpClient.GetFromJsonAsync<List<WeatherForecastDTO>>("WeatherForecast");
        }
    }
}
```
- Store and reuse token in session.
- Refactor HumanResources.cshtml.cs
```c#
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using UnderTheHood.Pages.Account;
using WebProject.Authorization;
using WebProject.DTO;

namespace UnderTheHood.Pages
{
    [Authorize(Policy = "MustBelongToHR")]
    public class HumanResourceModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        [BindProperty]
        public List<WeatherForecastDTO> WeatherForecastItems { get; set; }
        public HumanResourceModel(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            WeatherForecastItems = await InvokeEndpoint<List<WeatherForecastDTO>>("OurWebApi", "WeatherForecast");
        }

        private async Task<T> InvokeEndpoint<T>(string clientName, string url)
        {
            //get token from session
            JwtToken token = null;
            var strToken = HttpContext.Session.GetString("access_token");
            if (string.IsNullOrEmpty(strToken))
                token = await Authenticate();
            else
                token = JsonConvert.DeserializeObject<JwtToken>(strToken);

            if (token == null || string.IsNullOrEmpty(token.AccessToken) || token.ExpiresAt <= DateTime.UtcNow)
                token = await Authenticate();

            var httpClient = httpClientFactory.CreateClient(clientName);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            return await httpClient.GetFromJsonAsync<T>(url);
        }

        private async Task<JwtToken> Authenticate()
        {
            var httpClient = httpClientFactory.CreateClient("OurWebApi");
            var response = await httpClient.PostAsJsonAsync("auth", new Credential { UserName = "admin", Password = "password" });
            response.EnsureSuccessStatusCode();
            string jwtToken = await response.Content.ReadAsStringAsync();
            HttpContext.Session.SetString("access_token", jwtToken);
            return JsonConvert.DeserializeObject<JwtToken>(jwtToken);          
        }
    }
}
```
- Apply Policy to WebApi endpoint:

- Add policy to WeatherForecast controller:
```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace UnderTheHoodApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(policy:"AdminOnly")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 10).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
```
- Add policy in Configure  services add authorization:
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace UnderTheHoodApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetValue<string>("SecretKey"))),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly",
                    policy => policy.RequireClaim("Admin"));
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
```
- Add Admin claim in AuthController:
```cs
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace UnderTheHoodApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public AuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody] Credential credential)
        {
            if (credential.UserName == "admin" && credential.Password == "password")
            {
                //Create security context
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@test.com"),
                    new Claim("Department", "HR"),
                    new Claim("Admin", "true"),
                    new Claim("EmploymentDate", "2020-12-12")
                };

                var expiresAt = DateTime.UtcNow.AddMinutes(10);

                return Ok(new
                {
                    access_token = CreateToken(claims, expiresAt),
                    expires_at = expiresAt
                });
            }

            ModelState.AddModelError("Unauthorized", "You are not authorized to access the endpoint");
            return Unauthorized(ModelState);
        }

        private string CreateToken(IEnumerable<Claim> claims, DateTime expiresAt) {
            var secretKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretKey"));
            var jwt = new JwtSecurityToken(
                claims : claims,
                notBefore : DateTime.UtcNow,
                expires: expiresAt,
                signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(secretKey),
                        SecurityAlgorithms.HmacSha256Signature
                    )
                );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
```

## ASP.NET Core identity

- Add a new .net core web api project.
- Install Aspnetcore.identy, aspnetcore.identity.entityframeworkcore, entityframeworkcore.design, entityframeworkcore.sqlserver and entityframeworkcore.tools nuget packages.
- Add a new Application identity context class under a new Data folder:
- ApplicationDBContext
```c#
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApp_Security.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options): base(options)
        {

        }
    }
}
```
- Configure Startup.cs (WebApp project)
```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApp_Security.Data;

namespace WebApp_Security
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDBContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });
            services.AddRazorPages();
        }

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
    }
}
```
-appsettings.Development.json
```json
{
  "DetailedErrors": true,
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "ConnectionStrings": {
    "Default": "Server=localhost,1433\\Catalog=AppUsers;Database=AppUsers;User=sa;Password=Strong.Pwd-123;"
  }
}
```
- Execute commands (version for Mac running SQL server in Docker container). Navigate to the web app project and run the following commands:
    1. dotnet restore
    2. dotnet tool install --global dotnet-ef
    3. dotnet-ef
    4. dotnet ef migrations add initial
    5. dotnet ef database update //this creates the new database AppUsers (with the tables already created)
- Configure WebApp to use Identity:
- Startup.cs
```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApp_Security.Data;

namespace WebApp_Security
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDBContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });
            services.AddIdentity<IdentityUser, IdentityRole>(
                    options =>
                    {
                        options.Password.RequiredLength = 8;
                        options.Password.RequireLowercase = true;
                        options.Password.RequireUppercase = true;

                        options.Lockout.MaxFailedAccessAttempts = 5;
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

                        options.User.RequireUniqueEmail = true;
                    }
                )
                .AddEntityFrameworkStores<ApplicationDBContext>();

            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/AccessDenied";
            });
            services.AddRazorPages();
        }

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
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
```
- User registration:
- Add a new razor page:
- Register.cshtml
```html
@page
@model WebApp_Security.Pages.Account.RegisterModel
@{
}
<p>
    <h3>User Registration</h3>
</p>
<div class="container border" style="padding:20px">
    <form method="post">
        <div class="text-danger" asp-validation-summary="All"></div>
        <div class="form-group row">
            <div class="col-2">
                <label asp-for="RegisterViewModel.Email"></label>
            </div>
            <div class="col-5">
                <input type="text" asp-for="RegisterViewModel.Email" class="form-control" />
                <span class="text-danger" asp-validation-for="RegisterViewModel.Email"></span>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-2">
                <label asp-for="RegisterViewModel.Password"></label>
            </div>
            <div class="col-5">
                <input type="password" asp-for="RegisterViewModel.Password" class="form-control" />
                <span class="text-danger" asp-validation-for="RegisterViewModel.Password"></span>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-2">
                <input type="submit" class="btn btn-primary" value="Login" />
            </div>
        </div>
    </form>
</div>
```
- Register.cshtml.cs
```c#
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_Security.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;

        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; }

        public RegisterModel(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            //Create the user
            var user = new IdentityUser
            {
                Email = RegisterViewModel.Email,
                UserName = RegisterViewModel.Email
            };
            var result = await this.userManager.CreateAsync(user, RegisterViewModel.Password);
            if (result.Succeeded)
            {
                return RedirectToPage("Account/Login");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
                return Page();
            }
        }
    }
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        [DataType(dataType:DataType.Password)]
        public string Password { get; set; }
    }
}
```
- User login
- Add login razor page:
- Login.cshtml:
```html
@page
@model WebApp_Security.Pages.Account.LoginModel
@{
}
<p>
    <h3>User</h3>
</p>
<div class="container border" style="padding:20px">
    <form method="post">
        <div class="text-danger" asp-validation-summary="All"></div>
        <div class="form-group row">
            <div class="col-2">
                <label asp-for="Credential.Email"></label>
            </div>
            <div class="col-5">
                <input type="text" asp-for="Credential.Email" class="form-control" />
                <span class="text-danger" asp-validation-for="Credential.Email"></span>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-2">
                <label asp-for="Credential.Password"></label>
            </div>
            <div class="col-5">
                <input type="password" asp-for="Credential.Password" class="form-control" />
                <span class="text-danger" asp-validation-for="Credential.Password"></span>
            </div>
        </div>
        <div class="row form-check mb-2">
            <div class="col-2">
                <input type="checkbox" asp-for="Credential.RememberMe" class="form-check-input" />
                <label class="form-check-label" asp-for="Credential.RememberMe"></label>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-2">
                <input type="submit" class="btn btn-primary" value="Login" />
            </div>
        </div>
    </form>
</div>
```
- Login.cshtml.cs
```c#
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_Security.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> signInManager;

        [BindProperty]
        public CredentialViewModel Credential { get; set; }

        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            var result = await signInManager.PasswordSignInAsync(this.Credential.Email,
                this.Credential.Password,
                this.Credential.RememberMe,
                false);

            if (result.Succeeded)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("Login","User is locked out");
                }
                else
                {
                    ModelState.AddModelError("Login", "Failed to login");
                }
                return Page();
            }
        }
    }
    public class CredentialViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
```
- Index.cshtml.cs
```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApp_Security.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
```
- If you only want to allow login to users that have confirmed their email add a new option in Startup.cs:
```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApp_Security.Data;

namespace WebApp_Security
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDBContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });
            services.AddIdentity<IdentityUser, IdentityRole>(
                    options =>
                    {
                        options.Password.RequiredLength = 8;
                        options.Password.RequireLowercase = true;
                        options.Password.RequireUppercase = true;

                        options.Lockout.MaxFailedAccessAttempts = 5;
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

                        options.User.RequireUniqueEmail = true;
                        options.SignIn.RequireConfirmedEmail = true;
                    }
                )
                .AddEntityFrameworkStores<ApplicationDBContext>();

            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/AccessDenied";
            });
            services.AddRazorPages();
        }

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
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
```
- Email confirmation flow
- Startup.cs (add default token provider)
```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApp_Security.Data;

namespace WebApp_Security
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDBContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });
            services.AddIdentity<IdentityUser, IdentityRole>(
                    options =>
                    {
                        options.Password.RequiredLength = 8;
                        options.Password.RequireLowercase = true;
                        options.Password.RequireUppercase = true;

                        options.Lockout.MaxFailedAccessAttempts = 5;
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

                        options.User.RequireUniqueEmail = true;
                        options.SignIn.RequireConfirmedEmail = true;
                    }
                )
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/AccessDenied";
            });
            services.AddRazorPages();
        }

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
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
```
- Modify Register.cshtml.cs
```c#
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_Security.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;

        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; }

        public RegisterModel(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            //Create the user
            var user = new IdentityUser
            {
                Email = RegisterViewModel.Email,
                UserName = RegisterViewModel.Email
            };
            var result = await this.userManager.CreateAsync(user, RegisterViewModel.Password);
            if (result.Succeeded)
            {
                var confirmationToken = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                return Redirect(Url.PageLink(pageName: "/Account/ConfirmEmail",
                    values: new { userId = user.Id, token = confirmationToken}));
                //return RedirectToPage("Account/Login");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
                return Page();
            }
        }
    }
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        [DataType(dataType:DataType.Password)]
        public string Password { get; set; }
    }
}
```
- Confirm email:
- ConfirmEmail.cshtml:
```html
@page
@model WebApp_Security.Pages.Account.ConfirmEmailModel
@{
}
<h3>@Model.Message</h3>
```
- ConfirmEmail.cshtml.cs
```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_Security.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;
        [BindProperty]
        public string Message { get; set; }
        public ConfirmEmailModel(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await this.userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    this.Message = "Email address is successfully confirmed";
                    return Page();
                }

            }
            this.Message = "Failed to validate email";
            return Page();
        }
    }
}
```
- ConfirmEmail.cshtml.cs
```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_Security.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;
        [BindProperty]
        public string Message { get; set; }
        public ConfirmEmailModel(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await this.userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    this.Message = "Email address is successfully confirmed. Proceed to login";
                    return Page();
                }

            }
            this.Message = "Failed to validate email";
            return Page();
        }
    }
}
```
- Sign out.
- Copy the logout partial view:
```html
@if (User.Identity.IsAuthenticated)
{
    <form method="post" class="form-inline" asp-page="/Account/Logout">
        Welcome @User.Identity.Name
        <button type="submit" class="ml-2 btn btn-link">Logout</button>
    </form>
}
else
{
    <a class="btn btn-link" asp-page="/Account/Login">Login</a>
}
```
- Account/Logout.cshtml.cs
```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_Security.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> signInManager;

        public LogoutModel(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await this.signInManager.SignOutAsync();
            return RedirectToPage("/Account/Login");
        }
    }
}
```
- Add the partial view to the layout
- Collecting more user info with IdentityUser schema change:
- Modify Register to accept Department and Position information:
- Register.cshtml
```html
@page
@model WebApp_Security.Pages.Account.RegisterModel
@{
}
<p>
    <h3>User Registration</h3>
</p>
<div class="container border" style="padding:20px">
    <form method="post">
        <div class="text-danger" asp-validation-summary="All"></div>
        <div class="form-group row">
            <div class="col-2">
                <label asp-for="RegisterViewModel.Email"></label>
            </div>
            <div class="col-5">
                <input type="text" asp-for="RegisterViewModel.Email" class="form-control" />
                <span class="text-danger" asp-validation-for="RegisterViewModel.Email"></span>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-2">
                <label asp-for="RegisterViewModel.Password"></label>
            </div>
            <div class="col-5">
                <input type="password" asp-for="RegisterViewModel.Password" class="form-control" />
                <span class="text-danger" asp-validation-for="RegisterViewModel.Password"></span>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-2">
                <label asp-for="RegisterViewModel.Department"></label>
            </div>
            <div class="col-5">
                <input type="text" asp-for="RegisterViewModel.Department" class="form-control" />
                <span class="text-danger" asp-validation-for="RegisterViewModel.Department"></span>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-2">
                <label asp-for="RegisterViewModel.Position"></label>
            </div>
            <div class="col-5">
                <input type="text" asp-for="RegisterViewModel.Position" class="form-control" />
                <span class="text-danger" asp-validation-for="RegisterViewModel.Position"></span>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-2">
                <input type="submit" class="btn btn-primary" value="Login" />
            </div>
        </div>
    </form>
</div>
```
- Register.cshtml.cs
```c#
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_Security.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;

        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; }

        public RegisterModel(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            //Create the user
            var user = new IdentityUser
            {
                Email = RegisterViewModel.Email,
                UserName = RegisterViewModel.Email
            };
            var result = await this.userManager.CreateAsync(user, RegisterViewModel.Password);
            if (result.Succeeded)
            {
                var confirmationToken = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                return Redirect(Url.PageLink(pageName: "/Account/ConfirmEmail",
                    values: new { userId = user.Id, token = confirmationToken}));
                //return RedirectToPage("Account/Login");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
                return Page();
            }
        }
    }
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        [DataType(dataType:DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public string Position { get; set; }
    }
}
```
- Create a new User class that extends IdentityUser
```c#
using System;
using Microsoft.AspNetCore.Identity;

namespace WebApp_Security.Data.Account
{
    public class User : IdentityUser
    {
        public string Department { get; set; }
        public string Position { get; set; }
    }
}
```
-  Replace of the ocurrences of IdentityUser for User (except the one in the User class itself).
- Modify Register.cshtml.cs to map the new properties:
```c#
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp_Security.Data.Account;

namespace WebApp_Security.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> userManager;

        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; }

        public RegisterModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            //Create the user
            var user = new User
            {
                Email = RegisterViewModel.Email,
                UserName = RegisterViewModel.Email,
                Department = RegisterViewModel.Department,
                Position = RegisterViewModel.Position
            };
            var result = await this.userManager.CreateAsync(user, RegisterViewModel.Password);
            if (result.Succeeded)
            {
                var confirmationToken = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                return Redirect(Url.PageLink(pageName: "/Account/ConfirmEmail",
                    values: new { userId = user.Id, token = confirmationToken}));
                //return RedirectToPage("Account/Login");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
                return Page();
            }
        }
    }
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        [DataType(dataType:DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public string Position { get; set; }
    }
}
```
- Modify the DbContext (ApplicationDBContext):
```c#
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp_Security.Data.Account;

namespace WebApp_Security.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options): base(options)
        {

        }
    }
}
```
- New migration command and update database command. It adds a Department and Position columns in AspNetUsers table.
- Collect more information with claims:
- Modify Register.cshtml.cs
```c#
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp_Security.Data.Account;

namespace WebApp_Security.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> userManager;

        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; }

        public RegisterModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            //Create the user
            var user = new User
            {
                Email = RegisterViewModel.Email,
                UserName = RegisterViewModel.Email
            };

            var claimDepartment = new Claim("Department", RegisterViewModel.Department);
            var claimPosition = new Claim("Position", RegisterViewModel.Position);
            var result = await this.userManager.CreateAsync(user, RegisterViewModel.Password);
            if (result.Succeeded)
            {
                await this.userManager.AddClaimAsync(user, claimDepartment);
                await this.userManager.AddClaimAsync(user, claimPosition);
                var confirmationToken = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                return Redirect(Url.PageLink(pageName: "/Account/ConfirmEmail",
                    values: new { userId = user.Id, token = confirmationToken}));
                //return RedirectToPage("Account/Login");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
                return Page();
            }
        }
    }
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        [DataType(dataType:DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public string Position { get; set; }
    }
}
```
- This will add the department and position information in AspNetUserClaims table.
- Roles. A role is a very simple claim. 
- Create a user profile page:
- User profile.cshtml
```html
@page
@model WebApp_Security.Pages.Account.UserProfileModel
@{
}
<form method="post">
    <div class="text-danger" asp-validation-summary="All"></div>
    <div class="form-group row">
        <div class="col-2">
            <label asp-for="UserProfile.Email"></label>
        </div>
        <div class="col-5">
            <input type="text" asp-for="UserProfile.Email" class="form-control" readonly />
            <span class="text-danger" asp-validation-for="UserProfile.Email"></span>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-2">
            <label asp-for="UserProfile.Department"></label>
        </div>
        <div class="col-5">
            <input type="text" asp-for="UserProfile.Department" class="form-control" />
            <span class="text-danger" asp-validation-for="UserProfile.Department"></span>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-2">
            <label asp-for="UserProfile.Position"></label>
        </div>
        <div class="col-5">
            <input type="text" asp-for="UserProfile.Position" class="form-control" />
            <span class="text-danger" asp-validation-for="UserProfile.Position"></span>
        </div>
    </div>
    <div class="form-group row">
        <div class="col-2">
            <input type="submit" class="btn btn-primary" value="Save" />
        </div>
    </div>
</form>
@if (!string.IsNullOrEmpty(Model.SucessMessage))
{
    <div class="alert alert-success" role="alert">
        @Model.SucessMessage
    </div>
}
```
- UserProfile.cshtml.cs
```c#
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp_Security.Data.Account;


namespace WebApp_Security.Pages.Account
{
    [Authorize]
    public class UserProfileModel : PageModel
    {
        private readonly UserManager<User> userManager;

        [BindProperty]
        public UserProfileViewModel UserProfile { get; set; }
        [BindProperty]
        public string SucessMessage { get; set; }

        public UserProfileModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.UserProfile = new UserProfileViewModel();
            this.SucessMessage = string.Empty;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var (user, departmentClaim, positionClaim) = await GetUserInfoAsync();

            this.UserProfile.Department = departmentClaim?.Value;
            this.UserProfile.Position = positionClaim?.Value;
            this.UserProfile.Email = user.Email;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            try {
                var (user, departmentClaim, positionClaim) = await GetUserInfoAsync();
                await userManager.ReplaceClaimAsync(user, departmentClaim, new Claim(departmentClaim.Type, UserProfile.Department));
                await userManager.ReplaceClaimAsync(user, positionClaim, new Claim(positionClaim.Type, UserProfile.Position));
            }
            catch
            {
                ModelState.AddModelError("UserProfile", "Error occured when saving user profile");
            }

            this.SucessMessage = "The user profile was saved successfully";
            return Page();
        }

        private async Task<(Data.Account.User, Claim, Claim)> GetUserInfoAsync()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var claims = await userManager.GetClaimsAsync(user);

            var departmentClaim = claims.FirstOrDefault(x => x.Type == "Department");
            var position = claims.FirstOrDefault(x => x.Type == "Position");

            return (user, departmentClaim, position);
        }
    }

    public class UserProfileViewModel
    {
        public string Email { get; set; }
        [Required]
        public string Department { get; set; }
        [Required]
        public string Position { get; set; }
    }
}
```