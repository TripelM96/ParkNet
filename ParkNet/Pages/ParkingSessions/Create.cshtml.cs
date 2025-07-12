using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkNet.Data;
using ParkNet.Data.Entities;

namespace ParkNet.Pages.ParkingSessions
{
    public class CreateModel : PageModelBase
    {
        private readonly ParkNet.Data.ParkNetDbContext _context;

        public CreateModel(ParkNet.Data.ParkNetDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["ParkingSpotId"] = new SelectList(_context.ParkingSpots, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");

            AvailableSpots = await _context.ParkingSpots
            .Where(s => !s.Occupy && (!s.Reserved || (s.Reserved && s.ReservedForUserId == this.UserId)))
            .Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = $"{s.Code} ({s.Type})"
            }).ToListAsync();

            return Page();
        }

        [BindProperty]
        public ParkingSession ParkingSession { get; set; }
        public List<SelectListItem> AvailableSpots { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {


            ParkingSession.UserId = this.UserId;
            ParkingSession.Entrada = DateTime.UtcNow;

            // Marcar o lugar como ocupado
            var spot = await _context.ParkingSpots.FindAsync(ParkingSession.ParkingSpotId);
            if (spot == null || spot.Occupy)
            {
                ModelState.AddModelError("", "Lugar inválido ou já ocupado.");
                return await OnGetAsync();
            }

            if (spot.Reserved && spot.ReservedForUserId != this.UserId)
            {
                ModelState.AddModelError("", "Lugar Reservado.");
                return await OnGetAsync();
            }

            var subscription = await _context.Subscriptions
                    .Where(s => s.UserId == this.UserId &&
                           s.Active &&
                           s.IncialDate <= DateTime.UtcNow &&
                           s.EndTime >= DateTime.UtcNow)
                    .FirstOrDefaultAsync();

            if (spot.Reserved && spot.ReservedForUserId == this.UserId)
            {               
                var hasValidSubscription = subscription != null;

                if (!hasValidSubscription)
                {
                    ModelState.AddModelError("", "A sua subscrição expirou. Lugar reservado não disponível.");
                    return await OnGetAsync();
                }


            }
            spot.Occupy = true;

            _context.ParkingSessions.Add(ParkingSession);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
