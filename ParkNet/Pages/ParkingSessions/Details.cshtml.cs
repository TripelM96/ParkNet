using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ParkNet.Data;
using ParkNet.Data.Entities;

namespace ParkNet.Pages.ParkingSessions
{
    public class DetailsModel : PageModel
    {
        private readonly ParkNet.Data.ParkNetDbContext _context;

        public DetailsModel(ParkNet.Data.ParkNetDbContext context)
        {
            _context = context;
        }

        public ParkingSession ParkingSession { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingsession = await _context.ParkingSessions.FirstOrDefaultAsync(m => m.Id == id);
            if (parkingsession == null)
            {
                return NotFound();
            }
            else
            {
                ParkingSession = parkingsession;
            }
            return Page();
        }
    }
}
