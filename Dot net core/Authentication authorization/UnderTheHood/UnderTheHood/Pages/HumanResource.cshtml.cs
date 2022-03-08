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
