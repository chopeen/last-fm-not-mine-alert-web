using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
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
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IConfiguration configuration, ILogger<IndexModel> logger)
        {
            this._configuration = configuration;
            this._logger = logger;
        }

        [BindProperty, Required, StringLength(50)]
        public string ArtistName { get; set; }

        public void OnGet()
        {
            authenticateOrFail();
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
                client.DefaultRequestHeaders.Add("x-functions-key", apiKey);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string encodedArtistName = Uri.EscapeDataString(this.ArtistName);
                HttpResponseMessage response = await client.PostAsJsonAsync($"api/not-my-artists?name={encodedArtistName}", "{}");
                response.EnsureSuccessStatusCode();

                // TODO: How to use `ArtistEntity` here without code duplication to deserialize the JSON into an entity automatically?
                object addedArtist = await response.Content.ReadAsAsync<object>();
            }
            
            return RedirectToPage("/Index");
        }

        // TODO: Can this be implemented with the Authorize attribute or ModelState? Read more about auth and find
        //       a standard way.
        private void authenticateOrFail()
        {
            string authTokenKey = "code";

            string expectedToken = this._configuration["WebsiteAuthToken"];
            string queryToken = this.Request.Query[authTokenKey];
            string cookieToken = this.Request.Cookies[authTokenKey];

            if (expectedToken.IsNotNullAndEquals(queryToken))
            {
                this.Response.Cookies.Append(authTokenKey, expectedToken, new CookieOptions()
                {
                        HttpOnly = true,
                        Expires = DateTime.Now.AddDays(3),
                        SameSite = SameSiteMode.Strict
                });
                
                return;
            }
            else if (expectedToken.IsNotNullAndEquals(cookieToken))
            {
                return;
            }

            // fail unless authentication was successful
            throw new System.Security.Authentication.InvalidCredentialException("Wrong authentication token specified or none at all.");
        }
    }
}
