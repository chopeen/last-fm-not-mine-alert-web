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

        [BindProperty]
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string ArtistName { get; set; }

        public void OnGet()
        {
            authenticateOrFail();
        }

        // TODO: Should this API call be placed in a Controller class? Then PostAsJsonAsync<IndexModel> should be possible,
        //       without the anononymous type.
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

                // TODO: Is this safe? Should be user input be sanitized in any way before sending via POST?
                var newArtist = new { name = this.ArtistName };  // anononymous type
                HttpResponseMessage response = await client.PostAsJsonAsync($"api/not-my-artists", newArtist);
                response.EnsureSuccessStatusCode();

                // TODO: The confirmation page could display the response (e.g. new artists's ID).
                //       How to use `ArtistEntity` here, without code duplication to deserialize the JSON into an entity automatically?
                // object addedArtist = await response.Content.ReadAsAsync<object>();
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
