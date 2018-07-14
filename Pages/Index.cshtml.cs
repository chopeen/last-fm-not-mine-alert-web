﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

            // for debugging only
            Console.WriteLine("Form value:\t" + this.ArtistName);
            Console.WriteLine("Secrets:\t" + this._configuration["NotMyArtistsApiUrl"]);
            
            return RedirectToPage("/Index");
        }
    }
}
