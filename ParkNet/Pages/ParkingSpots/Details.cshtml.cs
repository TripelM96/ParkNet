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
    public class DetailsModel : PageModel
    {
        private readonly ParkNet.Data.ParkNetDbContext _context;

        public DetailsModel(ParkNet.Data.ParkNetDbContext context)
        {
            _context = context;
        }

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
    }
}
