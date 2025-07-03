using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ParkNet.Data;
using ParkNet.Data.Entities;

namespace ParkNet.Pages.Subscriptions
{
    public class IndexModel : PageModel
    {
        private readonly ParkNetDbContext _context;

        public IndexModel(ParkNetDbContext context)
        {
            _context = context;
        }

        public IList<Subscription> Subscription { get;set; } 

        public async Task OnGetAsync()
        {
            Subscription = await _context.Subscriptions
                .Include(s => s.ParkingLot)
                .Include(s => s.User).ToListAsync();
        }
    }
}
