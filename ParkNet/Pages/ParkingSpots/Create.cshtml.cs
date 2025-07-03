﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkNet.Data;
using ParkNet.Data.Entities;

namespace ParkNet.Pages.ParkingSpots
{
    public class CreateModel : PageModel
    {
        private readonly ParkNet.Data.ParkNetDbContext _context;

        public CreateModel(ParkNet.Data.ParkNetDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["FloorId"] = new SelectList(_context.Floors, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public ParkingSpot ParkingSpot { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ParkingSpots.Add(ParkingSpot);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
