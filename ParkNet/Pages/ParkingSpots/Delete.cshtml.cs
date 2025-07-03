using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ParkNet.Data;
using ParkNet.Data.Entities;

namespace ParkNet.Pages.ParkingSpots
{
    public class DeleteModel : PageModel
    {
        private readonly ParkNet.Data.ParkNetDbContext _context;

        public DeleteModel(ParkNet.Data.ParkNetDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ParkingSpot ParkingSpot { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingspot = await _context.ParkingSpots.FirstOrDefaultAsync(m => m.Id == id);

            if (parkingspot == null)
            {
                return NotFound();
            }
            else
            {
                ParkingSpot = parkingspot;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingspot = await _context.ParkingSpots.FindAsync(id);
            if (parkingspot != null)
            {
                ParkingSpot = parkingspot;
                _context.ParkingSpots.Remove(ParkingSpot);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
