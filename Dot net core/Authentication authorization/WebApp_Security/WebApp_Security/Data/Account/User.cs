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
