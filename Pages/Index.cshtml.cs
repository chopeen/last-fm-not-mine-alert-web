using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace last_fm_not_mine_alert_web.Pages
{
    public class IndexModel : PageModel
    {
        // injecting the configuration and logger into the model
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public IndexModel(IConfiguration configuration, ILogger logger)
        {
            this._configuration = configuration;
            this._logger = logger;
        }

        [BindProperty, Required, StringLength(50)]
        public string ArtistName { get; set; }

        public void OnGet()
        {
            System.Security.Principal.IIdentity currentIdentity = ClaimsPrincipal.Current.Identity;
            this._logger.LogInformation($"Logged user AuthenticationType: {currentIdentity.AuthenticationType}");
            this._logger.LogInformation($"Logged user IsAuthenticated: {currentIdentity.IsAuthenticated}");
            this._logger.LogInformation($"Logged user Name: {currentIdentity.Name}");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (HttpClient client = new HttpClient())
            {
                string apiUrl = this._configuration["NotMyArtistsApiUrl"];
                string apiKey = this._configuration["NotMyArtistsApiKey"];

                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string encodedArtistName = Uri.EscapeDataString(this.ArtistName);
                HttpResponseMessage response = await client.PostAsJsonAsync($"api/not-my-artists?code={apiKey}&name={encodedArtistName}", "{}");
                response.EnsureSuccessStatusCode();

                // TODO: How to use `ArtistEntity` here without code duplication to deserialize the JSON into an entity automatically?
                object addedArtist = await response.Content.ReadAsAsync<object>();
            }
            
            return RedirectToPage("/Index", "not-yet-implemented");
        }
    }
}
