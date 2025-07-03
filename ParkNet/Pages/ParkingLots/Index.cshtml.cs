using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ParkNet.Data;
using ParkNet.Data.Entities;

namespace ParkNet.Pages.ParkingLots
{
    public class IndexModel : PageModel
    {
        private readonly ParkNet.Data.ParkNetDbContext _context;

        public IndexModel(ParkNet.Data.ParkNetDbContext context)
        {
            _context = context;
        }

        public IList<ParkingLot> ParkingLot { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ParkingLot = await _context.ParkingLots.ToListAsync();
        }
    }
}
