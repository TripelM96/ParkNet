using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ParkNet.Data;
using ParkNet.Data.Entities;

namespace ParkNet.Pages.Floors
{
    public class IndexModel : PageModel
    {
        private readonly ParkNet.Data.ParkNetDbContext _context;

        public IndexModel(ParkNet.Data.ParkNetDbContext context)
        {
            _context = context;
        }

        public IList<Floor> Floor { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Floor = await _context.Floors
                .Include(f => f.ParkingLot).ToListAsync();
        }
    }
}
