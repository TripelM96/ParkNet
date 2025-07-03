using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkNet.Data;
using ParkNet.Data.Entities;

namespace ParkNet.Pages.ParkingLots
{
    public class CreateModel : PageModelBase
    {
        private readonly ParkNet.Data.ParkNetDbContext _context;

        public CreateModel(ParkNet.Data.ParkNetDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public int SelectedParkingLotId { get; set; }

        [BindProperty]
        public int SelectedFloorId { get; set; }

        public List<SelectListItem> ParkingLots { get; set; }
        public List<SelectListItem> Floors { get; set; }

        [BindProperty]
        public ParkingLot ParkingLot { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            ParkingLots = await _context.ParkingLots
           .Select(p => new SelectListItem
           {
               Value = p.Id.ToString(),
               Text = p.Name
           }).ToListAsync();

            Floors = new List<SelectListItem>();
            return Page();
        }

       
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ParkingLots.Add(ParkingLot);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
