- Authentication: verify you are who you say you are and generate the security context.

- Security context: all you identity info that is relevant to the facility.
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
            {

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
        services.AddAuthentication().AddCookie("MyCookieAuth", options =>
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
