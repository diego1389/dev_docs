using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UnderTheHood.Pages
{
    [Authorize(Policy = "MustBelongToHR")]
    public class HumanResourceModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
