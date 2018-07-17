using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace last_fm_not_mine_alert_web.Pages
{
    public class IndexModel : PageModel
    {
        // injecting IConfiguration into the model
        private readonly IConfiguration _configuration;
        public IndexModel(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [BindProperty, Required, StringLength(50)]
        public string ArtistName { get; set; }

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
            
            return RedirectToPage("/Index");
        }
    }
}
