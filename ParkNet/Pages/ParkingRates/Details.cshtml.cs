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
    public class DetailsModel : PageModel
    {
        private readonly ParkNet.Data.ParkNetDbContext _context;

        public DetailsModel(ParkNet.Data.ParkNetDbContext context)
        {
            _context = context;
        }

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
    }
}
