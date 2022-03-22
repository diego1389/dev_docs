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
