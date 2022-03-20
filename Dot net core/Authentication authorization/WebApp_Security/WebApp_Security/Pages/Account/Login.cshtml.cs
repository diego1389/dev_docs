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
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> signInManager;

        [BindProperty]
        public CredentialViewModel Credential { get; set; }

        public LoginModel(SignInManager<User> signInManager)
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
