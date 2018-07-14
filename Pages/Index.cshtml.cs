using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace last_fm_not_mine_alert_web.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty, Required, StringLength(50)]
        public string ArtistName { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // for debugging only
            Console.WriteLine(this.ArtistName);

            return RedirectToPage("/Index");
        }
    }
}
