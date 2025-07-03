using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ParkNet.Data;
using ParkNet.Data.Entities;

namespace ParkNet.Pages.ParkingRates
{
    public class DeleteModel : PageModel
    {
        private readonly ParkNet.Data.ParkNetDbContext _context;

        public DeleteModel(ParkNet.Data.ParkNetDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ParkingRate ParkingRate { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingrate = await _context.ParkingRates.FirstOrDefaultAsync(m => m.Id == id);

            if (parkingrate == null)
            {
                return NotFound();
            }
            else
            {
                ParkingRate = parkingrate;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingrate = await _context.ParkingRates.FindAsync(id);
            if (parkingrate != null)
            {
                ParkingRate = parkingrate;
                _context.ParkingRates.Remove(ParkingRate);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
