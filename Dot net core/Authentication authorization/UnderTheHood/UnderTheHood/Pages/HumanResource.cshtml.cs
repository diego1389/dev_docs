﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            /*var httpClient = httpClientFactory.CreateClient("OurWebApi");
            WeatherForecastItems = await httpClient.GetFromJsonAsync<List<WeatherForecastDTO>>("WeatherForecast");*/
        }
    }
}
